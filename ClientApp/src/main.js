import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from '@/store';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';
import { ApolloClient, createHttpLink, InMemoryCache } from '@apollo/client/core';
import { createApolloProvider } from '@vue/apollo-option';

import 'vuetify/styles'
import {createVuetify} from "vuetify"

import {RecycleScroller} from 'vue-virtual-scroller';

const app = createApp(App)
const vuetify = createVuetify()

// HTTP connection to the API
const httpLink = createHttpLink({
    // You should use an absolute URL here
    uri: 'http://localhost:5097/graphql',
    })

// Cache implementation
const cache = new InMemoryCache()

// Create the apollo client
const apolloClient = new ApolloClient({
    cache,
    link: httpLink,
})

const apolloProvider = createApolloProvider({
    defaultClient: apolloClient,
  })

app.use(router)
app.component('recycler-scroller', RecycleScroller)
app.component('date-picker', VueDatePicker)
app.use(vuetify)
app.use(store)
app.use(apolloProvider)
app.mount('#app')
