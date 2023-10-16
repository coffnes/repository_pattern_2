<template>
  <tr>
    <td>{{ convertDate(weather.date) }}</td>
    <td>{{ weather.city }}</td>
    <td>{{ weather.temperature }}</td>
    <td>{{ convertCloudiness(weather.cloudiness) }}%</td>
    <td>{{ convertWetness(weather.wetness) }}%</td>
    <td>{{ convertWindSpeed(weather.windSpeed) }}</td>
    <td>{{ weather.pressure }}</td>
    <td>{{ weather.summary }}</td>
  </tr>
</template>

<script>
export default {
  props: {
    weather: {
      type: Object,
      required: true,
    },
  },
  methods: {
    convertDate(timestamp) {
      const date = new Date(timestamp * 1000);
      const day = (`0${date.getDate()}`).slice(-2);
      const month = (`0${date.getMonth()}`).slice(-2);
      const year = date.getFullYear();
      const strDate = `${day}-${month}-${year}`;
      return strDate;
    },
    convertCloudiness(cloudiness) {
      return Math.round(cloudiness * 10000) / 100
    },
    convertWetness(wetness) {
      return Math.round(wetness * 10000) / 100
    },
    convertWindSpeed(windSpeed) {
      return Math.round(windSpeed * 10000) / 10000
    },
  },
};
</script>

<style scoped>

</style>