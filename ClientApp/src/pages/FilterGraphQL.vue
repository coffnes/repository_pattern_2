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
      <v-divider
          :thickness="3"
          class="border-opacity-50">
      </v-divider>
    </div>
    <add-dialog @create="addWeather"></add-dialog>
    <graph-weather-list :weathers="weathers"/>
    <v-btn @click="prevPage">Prev</v-btn>
    <v-btn @click="nextPage">Next</v-btn>
  </div>
</template>

<script>
import GraphWeatherList from '@/components/GraphWeatherList.vue';
import AddDialog from '@/components/UI/AddDialog.vue';
import {gql} from 'graphql-tag';
import axios from 'axios';

export default {
  components: {
    'graph-weather-list': GraphWeatherList,
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
      cursor: 'MA==',
      selectedSort: '',
      selectedCity: '',
      selectedDate: '',
      pageInfo: {
        startCursor: '',
        endCursor: '',
        hasNextPage: false,
        hasPreviousPage: false,
      },
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
    nextPage() {
      this.fetchPageInfo();
      if(this.pageInfo.hasNextPage) {
        this.cursor = this.pageInfo.endCursor;
      }
    },
    prevPage() {
      this.fetchPageInfo();
      if(this.pageInfo.hasPreviousPage) {
        this.cursor = this.pageInfo.startCursor;
      }
    },
    fetchPageInfo() {
      this.pageInfo.startCursor = this.weathers.pageInfo.startCursor;
      this.pageInfo.endCursor = this.weathers.pageInfo.endCursor;
      this.pageInfo.hasNextPage = this.weathers.pageInfo.hasNextPage;
      this.pageInfo.hasPreviousPage = this.weathers.pageInfo.hasPreviousPage;
    },
  },
  mounted() {
    this.getCities();
  },
  apollo: {
    weathers: {
      query: gql`query FetchWeathers($selectedSort: String!, $selectedCity: String!, $selectedDateFrom: String!, $selectedDateTo: String!, $first: Int, $after: String){
        weathers(selectedSort: $selectedSort, selectedCity: $selectedCity, selectedDateFrom: $selectedDateFrom, selectedDateTo: $selectedDateTo, first: $first, after: $after) {
          edges {
            node {
              city
              date
              temperature
              cloudiness
              wetness
              windSpeed
              pressure
              summary
            }
            cursor
          }
          pageInfo {
            hasNextPage
            hasPreviousPage
            startCursor
            endCursor
          }
        }
      }`,
      update: data => data.weathers,
      variables() {
        return {
          selectedSort: this.selectedSort,
          selectedCity: this.selectedCity,
          selectedDateFrom: (this.selectedDate === '' ? 0 : this.selectedDate[0].getTime() / 1000).toString(),
          selectedDateTo: (this.selectedDate === '' ? 0 : this.selectedDate[1].getTime() / 1000).toString(),
          first: 10,
          after: this.cursor,
          
        }
      }
    }
  },
}
</script>

<style scoped>

</style>