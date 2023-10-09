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
    <tbody ref="tbody">
      <tr class="firstRow" :style="{height: `${firstRowHeight}px`}">
        <td colspan="100%"></td>
      </tr>
      <weather-item v-for="weather in renderWeathers" :weather="weather" :key="weather.id"/>
      <tr class="lastRow" :style="{height: `${lastRowHeight}px`}">
        <td colspan="100%"></td>
      </tr>
    </tbody>
  </v-table>
</template>

<script>
import WeatherItem from '@/components/WeatherItem.vue';

export default {
  data() {
    return {
      startIndex: 0,
      step: 1,
      elementHeight: 38,
      firstRowHeight: 0,
      lastRowHeight: 0,
      lastScrollPosition: window.scrollY,
    };
  },
  components: {
    'weather-item': WeatherItem,
  },
  props: {
    weathers: {
      type: Array,
    }
  },
  computed: {
    renderWeathers() {
      return this.weathers.slice(this.startIndex, this.startIndex + this.step);
    },
  },
  created() {
    
  },
  mounted() {
    this.initTable();
    window.addEventListener("scroll", this.handleScroll);
  },
  unmounted() {
    window.removeEventListener("scroll", this.handleScroll);
  },
  methods: {
    initTable() {
      const tableTop = this.$refs.tbody.getBoundingClientRect().top;
      const viewPortY = document.documentElement.clientHeight;

      if (tableTop > 0) {
        this.step = Math.floor((viewPortY - tableTop) / this.elementHeight);
      } else {
        this.step = Math.floor(viewPortY / this.elementHeight);
        this.startIndex = Math.floor(-tableTop / this.elementHeight);
      }

      this.firstRowHeight = this.startIndex * this.elementHeight;
      this.lastRowHeight =
          this.weathers.length * this.elementHeight - this.step * this.elementHeight;
    },
    handleScroll() {
      const top = this.$refs.tbody.getBoundingClientRect().top;
      const viewportY = document.documentElement.clientHeight;

      console.log(`top: ${top}`);
      if (top < 0) {
        this.step = Math.floor(viewportY / this.elementHeight);
        this.startIndex = Math.floor(-top / this.elementHeight);
      } else {
        this.startIndex = 0;
        this.step = Math.floor((viewportY - top) / this.elementHeight);
      }

      this.$nextTick(() => {
        this.firstRowHeight = this.startIndex * this.elementHeight;
        this.lastRowHeight =
            this.weathers.length * this.elementHeight -
            this.step * this.elementHeight -
            this.firstRowHeight;
      });
    },
  },
};
</script>

<style scoped>

</style>