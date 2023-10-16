import { createRouter, createWebHistory } from 'vue-router'
import MainPage from "@/pages/MainPage.vue";
import WeatherListPage from "@/pages/WeatherListPage.vue";
import WeatherListClient from "@/pages/WeatherListClient.vue";
import FilterQueryGet from "@/pages/FilterQueryGet.vue";
import FilterQueryPost from "@/pages/FilterQueryPost.vue";
import FilterGraphQL from "@/pages/FilterGraphQL.vue";
import PaginationMongo from "@/pages/PaginationMongo.vue";
import PaginationGraph from "@/pages/PaginationGraph.vue";
import SearchClient from "@/pages/SearchClient.vue";
import SearchServer from "@/pages/SearchServer.vue";
import Elasticsearch from "@/pages/Elasticsearch.vue";
import PathNotFound from "@/pages/PathNotFound.vue";
import BigQueryPage from '@/pages/BigQueryPage.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: MainPage
    },
    {
      path: '/list',
      name: 'list',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      //component: () => import('../views/AboutView.vue')
      component: WeatherListPage
    },
    {
      path: '/filter_client',
      name: 'filter_client',
      component: WeatherListClient
    },
    {
      path: '/filter_get',
      name: 'filter_get',
      component: FilterQueryGet
    },
    {
      path: '/filter_post',
      name: 'filter_post',
      component: FilterQueryPost
    },
    {
      path: '/filter_graph',
      name: 'filter_graph',
      component: FilterGraphQL
    },
    {
      path: '/pag_mongo',
      name: 'pag_mongo',
      component: PaginationMongo
    }
    ,{
      path: '/pag_graph',
      name: 'pag_graph',
      component: PaginationGraph
    },
    {
      path: '/search_client',
      name: 'search_client',
      component: SearchClient
    },
    {
      path: '/search_server',
      name: 'search_server',
      component: SearchServer
    },
    {
      path: '/elasticsearch',
      name: 'elasticsearch',
      component: Elasticsearch
    },
    {
      path: '/big_query',
      name: 'big_query',
      component: BigQueryPage
    },
    {
      path: '/:pathMatch(.*)*',
      component: PathNotFound
    },
  ]
})

export default router
