<template>
  <div>
    <h1>Фильтрация данных только на клиенте</h1>
    <div class="request_options">
      <v-select v-if="sortOptions"
                :items="sortOptions"
                :item-title="'name'"
                :item-value="'value'"
                v-model="selectedSort"
                label="Sort options">
      </v-select>
      <h3>Фильтрация по городу</h3>
      <v-select
          label="Choose city"
          :items="cities"
          v-model="selectedCity">
      </v-select>
      <h3>Фильтрация по дате</h3>
      <p>Выбирете DateFrom и DateTo</p>
      <date-picker v-model.lazy="selectedDate" range
                   :partial-range="false"
                   :enable-time-picker="false">
      </date-picker>
      <h3>Только 0 температура</h3>
      <v-btn @click="this.weathers=filterOnlyZeroes">Только 0</v-btn>
      <v-btn @click="resetFilters()">Default</v-btn>
      <v-divider
          :thickness="3"
          class="border-opacity-50">
      </v-divider>
    </div>
    <add-dialog @create="addWeather"></add-dialog>
    <weather-list :weathers="filterByDate"/>
  </div>
</template>

<script>
import axios from 'axios';
import WeatherList from '@/components/WeatherList.vue';
import AddDialog from '@/components/UI/AddDialog.vue';
import { store } from '@/store/store.js';

export default {
  components: {
    'weather-list': WeatherList,
    'add-dialog': AddDialog,
  },
  data() {
    return {
      store,
      weathers: [],
      cities: [],
      sortOptions: [
        { value: '', name: 'None' },
        { value: 'date', name: 'Дата' },
        { value: 'city', name: 'Город' },
        { value: 'temperature', name: 'Температура' },
        { value: 'cloudiness', name: 'Облачность' },
        { value: 'wetness', name: 'Влажность' },
        { value: 'windSpeed', name: 'Скорость ветра' },
        { value: 'pressure', name: 'Давление' },
      ],
      selectedSort: '',
      selectedCity: '',
      selectedDate: '',
    };
  },
  methods: {
    async getCities() {
      this.cities.push('None');
      await axios.get('/weatherforecast/get_cities')
          .then((response) => {
            response.data.forEach((city) => {
              this.cities.push(city.name)
            })
          })
          .catch((error) => {
            console.log(error)
          })
    },
    async fetchWeather() {
      try {
        const response = await axios.get('/weatherforecast');
        this.store.weathers = response.data;
        this.weathers = response.data;
      } catch (e) {
        console.log(e);
      }
    },
    async addWeather(weather) {
      await axios.post('/weatherforecast', { ...weather })
          .then((response) => {
            console.log(response);
          })
          .catch((error) => {
            console.log(error);
          });
    },
    resetFilters() {
      this.selectedSort = 'None';
      this.selectedCity = 'None';
      this.selectedDate = '';
      this.weathers = this.store.weathers;
    },
  },
  mounted() {
    this.getCities();
    this.fetchWeather();
  },
  computed: {
    sortedWeather() {
      return [...this.weathers].sort((w1, w2) => {
        if (w1[this.selectedSort] < w2[this.selectedSort]) {
          return -1;
        }
        if (w1[this.selectedSort] > w2[this.selectedSort]) {
          return 1;
        }
        return 0;
      });
    },
    filterByCity() {
      return this.sortedWeather.filter((value) => value.city === this.selectedCity || this.selectedCity === '' || this.selectedCity === 'None');
    },
    filterByDate() {
      if (this.selectedDate === '') {
        return this.filterByCity;
      }
      return this.filterByCity.filter((value) =>
          value.date >= (this.selectedDate[0].getTime() / 1000)
          && value.date <= (this.selectedDate[1].getTime() / 1000));
    },
    filterOnlyZeroes() {
      return this.filterByDate.filter((value) => value.temperatureC === 0);
    },
  },
};
</script>

<style scoped>
.request_options {
  margin-bottom: 20px;
}
</style>