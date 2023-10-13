<template>
  <v-table>
    <thead>
      <tr>
        <th>Дата</th>
        <th>Город</th>
        <th>Температура</th>
        <th>Облачность</th>
        <th>Влажность</th>
        <th>Скорость ветра</th>
        <th>Давление</th>
        <th>Описание</th>
      </tr>
    </thead>
    <tbody>
      <weather-item v-for="weather in weathers.edges" :weather="weather.node" :key="weather.node.id"/>
    </tbody>
  </v-table>
</template>

<script>
import WeatherItem from '@/components/WeatherItem.vue';

export default {
  components: {
    'weather-item': WeatherItem,
  },
  props: {
    weathers: {
      type: Array,
    }
  },
}
</script>