<template>
  <q-card
    v-if="movie"
    class="tw-bg-[#1A202C] tw-text-white tw-h-full tw-w-full tw-relative"
  >
    <div class="tw-flex tw-flex-col tw-text-white">
      <img
        class="tw-opacity-90 tw-rounded-lg"
        :src="config.url + movie.mainImage"
      />
      <div class="tw-flex tw-flex-col tw-text-center tw-items-center">
        <router-link
          :to="{ name: 'MovieDetail', params: { slug: movie.slug } }"
          class="tw-text-base tw-mt-5 tw-leading-relaxed"
          >{{ movie.name }}</router-link
        >
        <div class="tw-flex tw-flex-row tw-gap-4 tw-mt-5 tw-items-center">
          <div class="tw-flex tw-gap-2 tw-items-center">
            <q-icon size="30px" name="fab fa-imdb"></q-icon>
            <span v-if="movie.imdb">{{ movie.imdb }}</span>
            <span v-else>0</span>
          </div>
          <div>|</div>
          <div>{{ movie.releaseDate }}</div>
        </div>
        <div class="tw-leading-relaxed tw-py-2">
          {{
            movie.categories
              .map(function (category) {
                return category.name;
              })
              .join(', ')
          }}
        </div>
      </div>
    </div>
    <div v-if="loggedIn">
      <i
        v-if="controlFavorite(movie.id)"
        @click="updateFavorite(movie.id)"
        class="
          tw-absolute tw-top-5 tw-right-5
          fas
          fa-heart
          tw-text-5xl
          sm:tw-text-3xl
          md:tw-text-2xl
          tw-text-[#F44336]
        "
      ></i>
      <i
        v-else
        @click="updateFavorite(movie.id)"
        class="
          tw-absolute tw-top-5 tw-right-5
          far
          fa-heart
          tw-text-5xl
          sm:tw-text-3xl
          md:tw-text-2xl
          hover:tw-text-[#F44336]
        "
      ></i>
    </div>
  </q-card>
</template>

<script setup lang="ts">
import { defineProps, PropType, ref, computed } from 'vue';
import { Movie } from 'src/models/Movie';
import { useFavoriteStore } from 'src/store/Favorite';
import { useAuthStore } from 'src/store/Auth';
import config from 'src/config';

const props = defineProps({
  movie: Object as PropType<Movie>,
});

const movie = ref<Movie>(props.movie as Movie);
const favoriteStore = useFavoriteStore();
const authStore = useAuthStore();

const userfavorites = computed(() => {
  return favoriteStore.getFavoritesFromState;
});

const loggedIn = computed(() => {
  return authStore.loggedIn;
});

function controlFavorite(movieid: number) {
  return userfavorites.value.some((x) => x.movieId == movieid);
}

async function updateFavorite(movieid:number){
  if(controlFavorite(movieid)){
    await removeFromFavorite(movieid);
  }
  else{
    await addToFavorite(movieid);
  }
}

async function addToFavorite(movieid: number) {
  await favoriteStore.addToFavorite(movieid);
}
async function removeFromFavorite(movieid: number) {
  await favoriteStore.removeFromFavorite(movieid);
}
</script>

<style scoped></style>
