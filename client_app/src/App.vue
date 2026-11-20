<template>
  <div class="app-wrapper">
    <AppHeader :minimal="isMinimalHeader" />
    <main class="main-content">
      <router-view />
    </main>
    <AppFooter />
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import AppHeader from './components/AppHeader.vue';
import AppFooter from './components/AppFooter.vue';

const route = useRoute();
const isMinimalHeader = computed(() => ['login', 'enroll'].includes(route.name));
</script>
