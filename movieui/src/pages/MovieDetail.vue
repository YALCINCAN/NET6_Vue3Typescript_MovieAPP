<template>
  <div
    class="
      tw-max-w-7xl tw-mx-auto tw-mt-8 tw-px-3
      lg:tw-px-0
      tw-mb-8 tw-text-white
    "
    v-if="movie"
  >
    <div
      class="
        tw-flex-col tw-flex tw-justify-center tw-items-center tw-gap-10
        md:tw-flex-row md:tw-justify-start md:tw-items-start md:tw-gap-20
      "
    >
      <img
        :src="config.url + movie.mainImage"
        class="tw-w-full sm:tw-w-[370px] md:tw-w-96"
      />
      <div
        class="
          tw-flex tw-flex-col tw-justify-center tw-items-center
          md:tw-justify-start md:tw-items-start
        "
      >
        <h2 class="tw-text-2xl sm:tw-text-4xl tw-font-semibold">
          {{ movie.name }}
        </h2>
        <div
          class="
            tw-gap-2 tw-mt-6 tw-items-center tw-text-xs
            md:tw-text-sm
            tw-flex
          "
        >
          <q-icon size="30px" name="fab fa-imdb"></q-icon>
          <span v-if="movie.imdb">{{ movie.imdb }}</span>
          <span v-else>0</span>
          <div>|</div>
          <div>{{ movie.releaseDate }}</div>
          <div>|</div>
          <div v-if="movie.categories" class="tw-text-tw-leading-relaxed">
            {{
              movie.categories
                .map(function (category) {
                  return category.name;
                })
                .join(', ')
            }}
          </div>
        </div>
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
          <div v-html="movie.summary"></div>
        </div>
      </div>
    </div>

    <div
      class="tw-mt-10 tw-border-t tw-pt-5"
      v-if="movie.images && movie.images.length > 0"
    >
      <div
        class="
          tw-text-3xl tw-text-center
          lg:tw-text-left
          tw-text-white tw-mb-10
        "
      >
        Images
      </div>
      <div
        class="
          tw-grid tw-grid-cols-1
          sm:tw-grid-cols-2
          md:tw-grid-cols-4 md:tw-place-items-start
          tw-place-item-center
          md:tw-gap-4
          tw-gap-3
        "
      >
        <div
          class="tw-flex tw-flex-col tw-items-center tw-gap-5"
          v-for="image in movie.images"
          :key="image.id"
        >
          <div><img :src="config.url + image.image" /></div>
        </div>
      </div>
    </div>

    <div
      class="tw-mt-10 tw-border-t tw-pt-5"
      v-if="movie.actors && movie.actors.length > 0"
    >
      <div
        class="
          tw-text-3xl tw-text-center
          lg:tw-text-left
          tw-text-white tw-mb-10
        "
      >
        Cast
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
        <div
          class="tw-flex tw-flex-col tw-items-center tw-gap-5"
          v-for="actor in movie.actors"
          :key="actor.id"
        >
          <div><img :src="config.url + actor.image" /></div>
          <router-link
            :to="{ name: 'ActorDetail', params: { slug: actor.slug } }"
            class="tw-text-base md:tw-text-lg"
            >{{ actor.fullName }}</router-link
          >
        </div>
      </div>
    </div>

    <div
      class="tw-mt-10 tw-border-t tw-pt-5"
      v-if="movie.comments && movie.comments.length > 0"
    >
      <div
        class="
          tw-text-3xl tw-text-center
          lg:tw-text-left
          tw-text-white tw-mb-10
        "
      >
        Comments
      </div>
      <div
        class="
          tw-flex
          tw-flex-col
          tw-items-start
          tw-py-5
          tw-px-3
          tw-gap-5
          tw-bg-gray-600
          tw-rounded-lg
        "
      >
        <div
          class="
            tw-flex
            tw-items-center
            tw-gap-5
            tw-border
            tw-rounded-md
            tw-w-full
            tw-px-2
            tw-py-3
          "
          v-for="comment in movie.comments"
          :key="comment.id"
        >
          <q-avatar v-if="comment.user.image" size="48px">
            <img :src="config.url + comment.user.image" />
          </q-avatar>
          <q-avatar v-else size="48px">
            <img src="~assets/defaultavatar.png" />
          </q-avatar>
          <div class="tw-flex tw-flex-col tw-gap-2">
            <div>
              {{ comment.user.fullName }} -
              {{ formatDate(comment.commentDate) }}
            </div>
            <div>
              {{ comment.description }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="tw-mt-10 tw-pt-5" v-if="loggedIn">
      <textarea
        class="
          tw-form-control
          tw-block
          tw-w-full
          tw-px-3
          tw-py-1.5
          tw-text-base
          tw-font-normal
          tw-text-black
          tw-bg-white
          tw-bg-clip-padding
          tw-border
          tw-border-solid
          tw-border-black
          tw-rounded
          tw-transition
          tw-ease-in-out
          tw-m-0
          focus:tw-text-gray-700
          focus:tw-bg-white
          focus:tw-border-blue-600
          focus:tw-outline-none
        "
        v-model="comment.description"
        rows="3"
        placeholder="Your comment"
      ></textarea>
      <div class="tw-flex tw-justify-center tw-my-3">
        <q-btn
          class="
            tw-rounded-lg tw-px-4 tw-py-2 tw-bg-gray-500 tw-text-white
            hover:tw-text-gray-500 hover:tw-bg-white
          "
          @click="sendComment()"
          :disable="$v.$invalid"
        >
          Send Comment
        </q-btn>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed, ref } from 'vue';
import { useRoute } from 'vue-router';
import { useMovieStore } from 'src/store/Movie';
import { useAuthStore } from 'src/store/Auth';
import { Comment } from 'src/models/Comment';
import { useCommentStore } from 'src/store/Comment';
import { useQuasar } from 'quasar';
import useVuelidate from '@vuelidate/core';
import { date } from 'quasar';
import { required, helpers } from '@vuelidate/validators';
import config from 'src/config';

const route = useRoute();
const $q = useQuasar();
const movieStore = useMovieStore();
const authStore = useAuthStore();
const commentStore = useCommentStore();
onMounted(async () => {
  await movieStore.getMovieDetail(route.params.slug as string);
  comment.value.movieId = movie.value.id;
});
const movie = computed(() => {
  return movieStore.getMovieDetailFromState;
});
const comment = ref<Comment>({} as Comment);

async function sendComment() {
  const response = await commentStore.addComment(comment.value);
  if (response) {
    $q.notify({
      message: response,
      color: 'positive',
    });
    await movieStore.getMovieDetail(route.params.slug as string);
    comment.value.movieId = movie.value.id;
    comment.value.description = '';
  }
}

const rules = computed(() => {
  return {
    description: {
      required: helpers.withMessage('Description is required', required),
    },
  };
});

function formatDate(commentDate: Date) {
  return date.formatDate(commentDate, 'DD.MM.YYYY HH:mm');
}
const $v = useVuelidate(rules, comment, { $autoDirty: true });
const loggedIn = computed(() => {
  return authStore.getLoggedIn;
});
</script>

<style scoped></style>
