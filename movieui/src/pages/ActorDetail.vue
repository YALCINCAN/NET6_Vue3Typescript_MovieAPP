<template>
  <div
    class="
      tw-max-w-7xl tw-mx-auto tw-mt-8 tw-px-3
      lg:tw-px-0
      tw-mb-8 tw-text-white
    "
    v-if="actor"
  >
    <div
      class="
        tw-flex-col tw-flex tw-justify-center tw-items-center tw-gap-10
        md:tw-flex-row md:tw-justify-start md:tw-items-start md:tw-gap-20
      "
    >
      <img
        :src="config.url + actor.image"
        class="tw-w-full sm:tw-w-[370px] md:tw-w-96"
      />
      <div
        class="
          tw-flex tw-flex-col tw-justify-center tw-items-center
          md:tw-justify-start md:tw-items-start
        "
      >
        <h2 class="tw-text-2xl sm:tw-text-4xl tw-font-semibold">
          {{ actor.fullName }}
        </h2>
        <div
          class="
            tw-mt-10 tw-text-base
            md:tw-text-lg
            tw-leading-relaxed tw-w-full
            sm:tw-w-[370px]
            md:tw-w-full
            tw-text-center
            md:tw-text-left
          "
        >
          <div v-html="actor.about"></div>
        </div>
      </div>
    </div>
    <div
      class="tw-mt-10 tw-border-t tw-pt-5"
      v-if="actor.movies && actor.movies.length > 0"
    >
      <div
        class="
          tw-text-3xl tw-text-center
          lg:tw-text-left
          tw-text-white tw-mb-10
        "
      >
        Actor Movies
      </div>
      <div
        class="
          tw-grid tw-grid-cols-1
          sm:tw-grid-cols-2
          md:tw-grid-cols-5 md:tw-place-items-start
          tw-place-item-center
          md:tw-gap-4
          tw-gap-3
        "
      >
         <MovieVue  v-for="movie in actor.movies" :key="movie.id" :movie="movie"></MovieVue>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import config from 'src/config';
import { useActorStore } from 'src/store/Actor';
import MovieVue from 'src/components/Movie.vue';

const route = useRoute();

const actorStore = useActorStore();

onMounted(async () => {
  await actorStore.getActorDetail(route.params.slug as string);
});

const actor = computed(() => {
  return actorStore.getActorDetailFromState;
});
</script>

<style scoped></style>
