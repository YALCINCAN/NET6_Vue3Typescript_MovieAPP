<template>
  <div class="tw-relative">
    <input
      type="text"
      class="
        tw-bg-gray-800
        tw-text-sm
        tw-rounded-full
        tw-w-64
        tw-px-4
        tw-pl-8
        tw-py-1
        focus:tw-outline-none focus:tw-shadow-outline
      "
      placeholder="Search"
      v-model="text"
    />
    <div class="tw-absolute tw-top-0">
      <svg
        class="tw-fill-current tw-w-4 tw-text-gray-500 tw-mt-2 tw-ml-2"
        viewBox="0 0 24 24"
      >
        <path
          class="heroicon-ui"
          d="M16.32 14.9l5.39 5.4a1 1 0 01-1.42 1.4l-5.38-5.38a8 8 0 111.41-1.41zM10 16a6 6 0 100-12 6 6 0 000 12z"
        />
      </svg>
    </div>

    <div
      class="
        tw-z-50 tw-absolute tw-bg-gray-800 tw-text-sm tw-rounded tw-w-64 tw-mt-4
      "
    >
      <ul v-if="movies && movies.length>0 && text !=''">
        <li
          class="tw-border-b tw-border-gray-700"
          v-for="movie in movies"
          :key="movie.id"
        >
          <router-link 
              :to="{ name: 'MovieDetail', params: { slug: movie.slug } }"
              class="
              hover:tw-bg-gray-700
              tw-px-3
              tw-py-3
              tw-flex
              tw-items-center
              tw-transition
              tw-ease-in-out
              tw-duration-150
            "
          >
            <img
              :src="config.url + movie.mainImage"
              style="tw-object-contain"
              class="tw-w-16"
            />
            <span class="tw-ml-4">{{ movie.name }}</span>
          </router-link>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useMovieStore } from 'src/store/Movie';
import { ref, computed } from 'vue';
import config from 'src/config';
const text = ref<string>('');
const movieStore = useMovieStore();
const movies = computed(() => {
  return movieStore.getMovieBySearch(text.value);
});
</script>

<style scoped></style>
