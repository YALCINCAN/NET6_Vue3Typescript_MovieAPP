<template>
  <div class="tw-max-w-7xl tw-mx-auto tw-mt-8 tw-px-3 lg:tw-px-0 tw-mb-8">
    <div
      v-if="movies && movies.length > 0"
      class="
        tw-grid tw-grid-cols-1
        sm:tw-grid-cols-2
        md:tw-grid-cols-3
        lg:tw-grid-cols-5
        tw-gap-5 tw-place-items-center
      "
    >
      <MovieVue
        v-for="movie in movies"
        :key="movie.id"
        :movie="movie"
      ></MovieVue>
    </div>

    <q-banner v-else class="tw-text-white tw-bg-red-500"
      >Your favorites are empty</q-banner
    >
  </div>
</template>

<script setup lang="ts">
import MovieVue from 'src/components/Movie.vue';
import { useMovieStore } from 'src/store/Movie';
import { computed, onMounted } from 'vue';
import { Movie } from 'src/models/Movie';

const movieStore = useMovieStore();
onMounted(async () => {
  await movieStore.getMovies();
});
const movies = computed<Movie[]>(() => {
  return movieStore.getUserFavoriteMovies;
});
</script>

<style scoped></style>
