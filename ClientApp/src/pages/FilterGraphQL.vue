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
    fetchPageInfo() {
      this.pageInfo.startCursor = this.weathers.pageInfo.startCursor;
      this.pageInfo.endCursor = this.weathers.pageInfo.endCursor;
      this.pageInfo.hasNextPage = this.weathers.pageInfo.hasNextPage;
      this.pageInfo.hasPreviousPage = this.weathers.pageInfo.hasPreviousPage;
    },
    fetchMoreWeathers() {
      this.$apollo.queries.weathers.fetchMore({
        variables: {
          selectedSort: this.selectedSort,
          selectedCity: this.selectedCity,
          selectedDateFrom: (this.selectedDate === '' ? 0 : this.selectedDate[0].getTime() / 1000).toString(),
          selectedDateTo: (this.selectedDate === '' ? 0 : this.selectedDate[1].getTime() / 1000).toString(),
          first: 20,
          cursor: this.weathers.pageInfo.endCursor,
        },
        updateQuery: (previousResult, { fetchMoreResult }) => {
          const newEdges = fetchMoreResult.weathers.edges;
          const pageInfo = fetchMoreResult.weathers.pageInfo;

          return newEdges.length ? {
            ...previousResult,
            weathers: {
              ...previousResult.weathers,
              // Concat edges
              edges: [
                ...previousResult.weathers.edges,
                ...newEdges,
              ],
              // Override with new pageInfo
              pageInfo,
            }
          } : previousResult
        },
      })
    },
    showMore() {
      window.onscroll = () => {
        let bottomOfWindow = document.documentElement.scrollTop + window.innerHeight === document.documentElement.offsetHeight;
        if(bottomOfWindow) {
          this.fetchMoreWeathers();
        }
      }
    },
  },
  mounted() {
    this.getCities();
    this.showMore();
  },
  apollo: {
    weathers: {
      query: gql`query fetchWeathers($selectedSort: String!, $selectedCity: String!, $selectedDateFrom: String!, $selectedDateTo: String!, $first: Int, $cursor: String){
        weathers(selectedSort: $selectedSort, selectedCity: $selectedCity, selectedDateFrom: $selectedDateFrom, selectedDateTo: $selectedDateTo, first: $first, after: $cursor) {
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
          }
          pageInfo {
            hasNextPage
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
          first: 20,
          after: this.cursor,
        }
      }
    }
  },
}
</script>

<style scoped>

</style>