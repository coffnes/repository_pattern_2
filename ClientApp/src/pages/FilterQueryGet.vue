<template>
  <div>
    <h1>Filter data by GET request</h1>
    <div class="request_options">
      <v-select v-if="sortOptions"
                :items="sortOptions"
                :item-title="'name'"
                :item-value="'value'"
                v-model="selectedSort"
                label="Sort options">
      </v-select>
      <h3>Filter by city:</h3>
      <v-select
          label="Choose city"
          :items="cities"
          v-model="selectedCity">
      </v-select>
      <h3>Filter by date</h3>
      <p>Select DateFrom and DateTo</p>
      <date-picker v-model.lazy="selectedDate" range
                   :partial-range="false"
                   :enable-time-picker="false">
      </date-picker>
      <h3>Get only zeroes</h3>
      <v-btn @click="this.weathers=filterOnlyZeroes">Get only zeroes</v-btn>
      <v-btn @click="resetFilters()">Default</v-btn>
      <v-btn @click="fetchWeather()">Test GET request</v-btn>
      <v-divider
          :thickness="3"
          class="border-opacity-50">
      </v-divider>
    </div>
    <add-dialog @create="addWeather"></add-dialog>
    <weather-list :weathers="weathers"/>
  </div>
</template>

<script>
import axios from 'axios';
import WeatherList from '@/components/WeatherList.vue';
import AddDialog from '@/components/UI/AddDialog.vue';

export default {
  components: {
    'weather-list': WeatherList,
    'add-dialog': AddDialog,
  },
  data() {
    return {
      sortOptions: [
        { value: '', name: 'None' },
        { value: 'date', name: 'Date' },
        { value: 'city', name: 'City' },
        { value: 'temperature', name: 'Temperature' },
      ],
      weathers: [],
      cities: [],
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
      await axios({
        method: 'get',
        url: '/weatherforecast/filter_get',
        params: {
          selectedSort: this.selectedSort,
          selectedCity: this.selectedCity,
          selectedDateFrom: this.selectedDate === '' ? '' : (this.selectedDate[0].getTime() / 1000).toString(),
          selectedDateTo: this.selectedDate === '' ? '' : (this.selectedDate[1].getTime() / 1000).toString(),
        },
        headers: {},
      }).then((response) => {
        this.weathers = response.data;
        console.log(typeof response.data);
      }).catch((error) => {
        console.log(error);
      });
    },
    resetFilters() {
      this.selectedSort = 'None';
      this.selectedCity = 'None';
      this.selectedDate = '';
      this.fetchWeather();
    },
  },
  mounted() {
    this.getCities();
    this.fetchWeather();
  },
  watch: {
    selectedSort() {
      this.fetchWeather();
    },
    selectedCity() {
      this.fetchWeather();
    },
    selectedDate() {
      this.fetchWeather();
    },
  },
};
</script>

<style scoped>
.request_options {
  margin-bottom: 20px;
}
</style>