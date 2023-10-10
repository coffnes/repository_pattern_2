<template>
  <v-form v-model="valid">
    <v-container>
      <v-row>
        <v-col
            cols="12"
            md="4"
        >
          <v-text-field
              v-model="temperature"
              :rules="temperatureRules"
              :counter="10"
              label="Temperature"
          ></v-text-field>
        </v-col>

        <v-col
            cols="12"
            md="4"
        >
          <v-select
              label="Choose city"
              :items="['Moscow',
              'Saint-Petesburg',
              'Novosibirsk',
              'Yekaterinburg',
              'Kazan',
              'Nizhny Novgorod']"
              v-model="city"
              :rules="cityRules">
          </v-select>
        </v-col>

        <v-col
            cols="12"
            md="4"
        >
          <date-picker :enable-time-picker="false" v-model="date" :rules="dateRules">
          </date-picker>
        </v-col>
      </v-row>
      <div class="text-center">
        <v-btn @click="createMeasurement()">Add</v-btn>
      </div>
    </v-container>
  </v-form>
</template>

<script>

export default {
  data: () => ({
    valid: false,
    temperature: '',
    city: '',
    date: '',
    weather: {
      date: '',
      city: '',
      temperatureC: 0,
    },
    temperatureRules: [
      (value) => {
        if (value) return true;
        return 'Temperature is required.';
      },
      (value) => {
        if (Number(value)) return true;
        return 'Temperature must be a number.';
      },
    ],
    cityRules: [
      (value) => {
        if (value) return true;
        return 'City is required.';
      },
    ],
    dateRules: [
      (value) => {
        console.log(value);
        if (value) return true;
        return 'Date is requred.';
      },
    ],
  }),
  methods: {
    createMeasurement() {
      if (this.valid) {
        this.weather.city = this.city;
        const day = (`0${this.date.getDate()}`).slice(-2);
        const month = (`0${this.date.getMonth() + 1}`).slice(-2);
        const year = this.date.getFullYear();
        const date = `${year}-${month}-${day}`;
        console.log(date);
        this.weather.date = date;
        this.weather.temperatureC = Number(this.temperature);
        this.$emit('create', this.weather);
      }
    },
  },
};
</script>

<style scoped>

</style>