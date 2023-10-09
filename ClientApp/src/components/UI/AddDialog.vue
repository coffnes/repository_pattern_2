<template>
  <v-dialog v-model="dialog">
    <template v-slot:activator="{ props }">
      <v-btn
          color="primary"
          v-bind="props"
      >
        Add Measurement
      </v-btn>
    </template>
    <v-card text="Input temperature, city and date of measurement:">
      <add-measurement @create="createMeasurement"></add-measurement>
      <v-card-actions>
        <v-btn color="error" block @click="dialog = false">Close Dialog</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import AddMeasurementForm from '@/components/AddMeasurementForm.vue';

export default {
  components: {
    'add-measurement': AddMeasurementForm,
  },
  data() {
    return {
      dialog: false,
    };
  },
  methods: {
    createMeasurement(weather) {
      this.$emit('create', weather);
    },
  },
};
</script>

<style scoped>

</style>