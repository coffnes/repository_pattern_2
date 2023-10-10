import axios from 'axios';

export const requestModule= {
  state() {
    return {
      weathers: [],
      selectedSort: '',
      selectedDate: '',
    }
  },
  mutations: {
    setWeathers(state, items) {
      state.weathers = items;
    },
    setSelectedSort(state, selectedSort) {
      state.selectedSort = selectedSort;
    }
  },
  getters: {
    sortedWeather(state) {
      return [...state.weathers].sort((w1, w2) => {
        if (w1[state.selectedSort] < w2[state.selectedSort]) {
          return -1;
        }
        if (w1[state.selectedSort] > w2[state.selectedSort]) {
          return 1;
        }
        return 0;
      });
    },
  },
  actions: {
    async fetchWeathers({state, commit}) {
      try {
        const response = await axios.get('/weatherforecast');
        commit('setWeathers', response.data);
      } catch (e) {
        console.log(e);
      }
    },
    async fetchWeathersByCity({state, commit}, city) {
      try {
        const response = await axios.get(`/weatherforecast/city/${city}`);
        commit('setWeathers', response.data);
      } catch (e) {
        console.log(e);
      }
    },
    async fetchWeathersByDate({state, commit}, {dateFrom, dateTo}) {
      try {
        console.log(dateFrom);
        console.log(dateTo);
        const response = await axios.get(`/weatherforecast/date/${dateFrom}/${dateTo}`);
        commit('setWeathers', response.data);
      } catch (e) {
        console.log(e);
      }
    },
    async fetchOnlyZeroes({state, commit}) {
      try {
        const response = await axios.get('/weatherforecast/zero');
        commit('setWeathers', response.data);
      } catch (e) {
        console.log(e);
      }
    },
  },
  namespaced: true
}