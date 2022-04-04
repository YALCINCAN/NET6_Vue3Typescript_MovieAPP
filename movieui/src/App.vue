<template>
  <router-view />
</template>
<script setup lang="ts">
import { onMounted } from 'vue';
import { useAuthStore } from 'src/store/Auth';
import { useFavoriteStore } from './store/Favorite';
const authStore = useAuthStore();
const favoriteStore = useFavoriteStore();
onMounted(async () => {
  let accesstoken = localStorage.getItem('accessToken');
  if (accesstoken) {
    await authStore.getAuthenticatedUser();
    await favoriteStore.getFavorites();
  }
});
</script>
