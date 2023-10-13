<template>
  <div>
    <h1>Фильтрация через GraphQL</h1>
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
      <p>Select DateFrom and DateTo</p>
      <date-picker v-model.lazy="selectedDate" range
                   :partial-range="false"
                   :enable-time-picker="false">
      </date-picker>
      <h3>Get only zeroes</h3>
      <v-btn @click="this.weathers=filterOnlyZeroes">Get only zeroes</v-btn>
      <v-btn @click="resetFilters()">Default</v-btn>
      <v-btn @click="fetchWeather()">Test post request</v-btn>
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
        { value: 'date', name: 'Дата' },
        { value: 'city', name: 'Город' },
        { value: 'temperature', name: 'Температура' },
        { value: 'cloudiness', name: 'Облачность' },
        { value: 'wetness', name: 'Влажность' },
        { value: 'windSpeed', name: 'Скорость ветра' },
        { value: 'pressure', name: 'Давление' },
      ],
      weathers: [],
      cities: [],
      selectedSort: '',
      selectedCity: '',
      selectedDate: '',
    };
  },
}
</script>

<style scoped>

</style>