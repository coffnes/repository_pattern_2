<template>
    <div>
      <h1>Фильтрация</h1>
      <v-container>
        <v-row align="center">
          <v-col>
            <strong>Фильтрация по дате</strong>
            <v-checkbox label="выходные"></v-checkbox>
            <v-checkbox label="будни"></v-checkbox>
          </v-col>
          <v-col>
            <strong>Фильтрация по городам</strong>
            <v-checkbox label="крупные города"></v-checkbox>
            <v-checkbox label="Московская область"></v-checkbox>
            <v-checkbox label="Северо-Западный"></v-checkbox>
          </v-col>
          <v-col>
            <strong>Фильтрация по значениям</strong>
            <v-checkbox label="нормальное давление"></v-checkbox>
            <v-checkbox label="без ветра"></v-checkbox>
          </v-col>
          <v-col>
            <strong>Фильтрация по температуре</strong>
            <v-checkbox label="температура 0"></v-checkbox>
            <v-checkbox label="температура больше 0"></v-checkbox>
            <v-checkbox label="температура меньше 0"></v-checkbox>
          </v-col>
        </v-row>
        <v-row no-gutters>
          <v-col cols="2">
            <v-sheet class="pa-4 mb-6"><p>Температура(C):</p></v-sheet>
            <v-sheet class="pa-4 mb-6"><p>Влажность(%):</p></v-sheet>
            <v-sheet class="pa-4 mb-6"><p>Облачность(%):</p></v-sheet>
          </v-col>
          <v-col>
            <v-sheet class="pa-2 mb-2"><v-range-slider :max="50" :min="-50" :step="1" v-model="temperatureRange" thumb-label="always"/></v-sheet>
            <v-sheet class="pa-2 mb-2"><v-range-slider :max="100" :min="1" :step="1" v-model="wetnessRange" thumb-label="always"/></v-sheet>
            <v-sheet class="pa-2 mb-2"><v-range-slider :max="100" :min="1" :step="1" v-model="cloudnessRange" thumb-label="always"/></v-sheet>
          </v-col>
        </v-row>
      </v-container>
      <v-btn @click="test">Test button</v-btn>
      <weather-list :weathers="weathers"/>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  import WeatherList from '@/components/WeatherList.vue';
  
  export default {
    components: {
      'weather-list': WeatherList,
    },
    data() {
      return {
        weathers: [],
        temperatureRange: [],
        wetnessRange: [],
        cloudnessRange: []
      };
    },
    methods: {
      async test() {
        const filter = {
          "param1": "test",
          "param2": "test",
          "param3": "test",
        };
        const url = new URL(`http://localhost:5097/weatherforecast/`);
        url.search = new URLSearchParams(filter);
        console.log(url);
        await axios({
          method: 'get',
          url: url,
        }).then((response) => {
          console.log(response);
        }).catch((error) => {
          console.log(error);
        });
      },
    },
    mounted() {
    },
    watch: {
    },
  };
  </script>
  
  <style scoped>
  .request_options {
    margin-bottom: 20px;
  }
  </style>