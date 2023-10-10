<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="main">
    <v-app>
      <v-app-bar scroll-behavior="elevate" clipped-left>
        <RouterLink to="/"><v-btn stacked variant="plain">Главная</v-btn></RouterLink>
        <RouterLink to="/list"><v-btn stacked variant="plain">Погода</v-btn></RouterLink>
        <RouterLink to="/filter_client"><v-btn stacked variant="plain">Фильтрация на клиенте</v-btn></RouterLink>
        <RouterLink to="/filter_post"><v-btn stacked variant="plain">Фильтрация POST запросом</v-btn></RouterLink>
        <RouterLink to="/filter_get"><v-btn stacked variant="plain">Фильтрация GET запросом</v-btn></RouterLink>
        <RouterLink to="/filter_graph"><v-btn stacked variant="plain">Фильтрация GrpahQL</v-btn></RouterLink>
        <RouterLink to="/pag_mongo"><v-btn stacked variant="plain">Пагинация MongoDriver</v-btn></RouterLink>
        <RouterLink to="/pag_graph"><v-btn stacked variant="plain">Пагинация GrpahQL</v-btn></RouterLink>
        <RouterLink to="/search_client"><v-btn stacked variant="plain">Поиск на клиенте</v-btn></RouterLink>
        <RouterLink to="/search_server"><v-btn stacked variant="plain">Поиск на сервере</v-btn></RouterLink>
        <RouterLink to="/elasticsearch"><v-btn stacked variant="plain">Поиск Elasticsearch</v-btn></RouterLink>
      </v-app-bar>
      <v-main>
        <RouterView />
      </v-main>
    </v-app>
  </div>
</template>

<style scoped>

</style>
