import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from '@/store';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';

import 'vuetify/styles'
import {createVuetify} from "vuetify"

import {RecycleScroller} from 'vue-virtual-scroller';

const app = createApp(App)
const vuetify = createVuetify()

app.use(router)
app.component('recycler-scroller', RecycleScroller)
app.use(vuetify)
app.use(store)
app.component('date-picker', VueDatePicker)
app.mount('#app')
