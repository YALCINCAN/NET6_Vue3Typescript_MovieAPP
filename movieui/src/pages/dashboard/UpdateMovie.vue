<template>
  <q-page class="q-pa-sm">
    <div class="row q-col-gutter-md">
      <div class="col-12">
        <q-card>
          <Alert></Alert>
          <q-card-section>
            <div class="text-h6">
              <span>Update Movie</span>
            </div>
          </q-card-section>
          <q-separator inset></q-separator>
          <q-card-section class="q-pt-none">
            <q-form class="q-gutter-md">
              <q-list>
                <q-item>
                  <q-item-section>
                    <q-input
                      dense
                      outlined
                      v-model="movie.name"
                      label="Name"
                      :error="$v.name.$error"
                    >
                      <template #error>
                        <li
                          v-for="(error, index) in $v.name.$errors"
                          :key="index"
                        >
                          <span>{{ error.$message }}</span>
                        </li>
                      </template>
                    </q-input>
                  </q-item-section>
                </q-item>
                <q-item>
                  <q-item-section>
                    <q-file
                      clearable
                      v-model="imagefile"
                      accept="image/png, image/jpeg"
                      label="Select Main Image"
                    >
                      <template v-slot:prepend>
                        <q-icon name="cloud_upload" />
                      </template>
                    </q-file>
                  </q-item-section>
                </q-item>
                <q-item>
                  <q-item-section>
                    <q-file
                      clearable
                      v-model="imagefiles"
                      append
                      multiple
                      use-chips
                      accept="image/png, image/jpeg"
                      label="Select Image"
                    >
                      <template v-slot:prepend>
                        <q-icon name="cloud_upload" />
                      </template>
                    </q-file>
                  </q-item-section>
                </q-item>
                <div class="row justify-center">
                  <div class="col-3" v-if="previewimageurl">
                    <div class="row justify-center">
                      <label class="text-dark q-mb-xs"
                        >Selected Main Image</label
                      >
                    </div>
                    <div class="row justify-center">
                      <div class="col-3">
                        <q-img fit="contain" :src="previewimageurl" />
                      </div>
                    </div>
                  </div>
                  <div class="col-3" v-if="previewimageurls.length > 0">
                    <div class="row justify-center">
                      <label class="text-dark q-mb-xs">Selected Images</label>
                    </div>
                    <div class="row justify-center q-col-gutter-sm">
                      <div
                        class="col-3"
                        v-for="(item, index) in previewimageurls"
                        :key="index"
                      >
                        <q-img fit="contain" :src="item"></q-img>
                      </div>
                    </div>
                  </div>
                  <div class="col-3" v-if="movie.mainImage">
                    <div class="row justify-center">
                      <label class="text-dark q-mb-xs"
                        >Current Main Image</label
                      >
                    </div>
                    <div class="row justify-center">
                      <div class="col-3">
                        <q-img
                          fit="contain"
                          :src="config.url + movie.mainImage"
                        />
                      </div>
                    </div>
                  </div>
                  <div class="col-3">
                    <div class="row justify-center">
                      <label class="text-dark q-mb-xs">Current Images</label>
                    </div>
                    <div class="row justify-center q-col-gutter-sm">
                      <div
                        class="col-3"
                        v-for="movieimage in movie.movieImages"
                        :key="movieimage.id"
                      >
                        <q-img :src="config.url + movieimage.image" />
                        <div class="text-center">
                          <i
                            @click="confirm(movieimage.id)"
                            class="fas fa-trash cursor-pointer"
                          >
                            <q-tooltip>Remove Image</q-tooltip>
                          </i>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <q-item>
                  <q-item-section>
                    <q-input
                      dense
                      outlined
                      v-model="movie.imdb"
                      label="Imdb"
                      mask="#,#"
                      hint="#,#"
                      :error="$v.imdb.$error"
                    >
                      <template #error>
                        <li
                          v-for="(error, index) in $v.imdb.$errors"
                          :key="index"
                        >
                          <span>{{ error.$message }}</span>
                        </li>
                      </template>
                    </q-input>
                  </q-item-section>
                </q-item>
                <div class="tw-flex tw-items-center tw-justify-center">
                  <q-item>
                    <q-item-section>
                      <q-date
                        v-model="movie.releaseDateString"
                        mask="DD/MM/YYYY"
                      />
                      <span
                        v-if="
                          $v.releaseDateString.$error ||
                          !movie.releaseDateString
                        "
                        class="text-red text-center tw-my-2"
                        >Release Date is required</span
                      >
                    </q-item-section>
                  </q-item>
                </div>

                <q-item>
                  <q-item-section>
                    <q-editor
                      v-model="movie.summary"
                      min-height="15rem"
                      placeholder="Summary"
                    >
                    </q-editor>
                    <span v-if="$v.summary.$error" class="text-red"
                      >Summary is required</span
                    >
                  </q-item-section>
                </q-item>
                <q-item>
                  <q-item-section>
                    <q-select
                      v-model="movie.actorIds"
                      :options="filteredactors"
                      map-options
                      @filter="filterFnActor"
                      dense
                      outlined
                      use-input
                      use-chips
                      multiple
                      emit-value
                      option-value="id"
                      option-label="fullName"
                      label="Actor Name"
                      :error="$v.actorIds.$error"
                    >
                      <template #error>
                        <li
                          v-for="(error, index) in $v.actorIds.$errors"
                          :key="index"
                        >
                          <span>{{ error.$message }}</span>
                        </li>
                      </template>
                    </q-select>
                  </q-item-section>
                </q-item>
                <q-item>
                  <q-item-section>
                    <q-select
                      v-model="movie.categoryIds"
                      :options="filteredcategories"
                      map-options
                      @filter="filterFnCategory"
                      dense
                      outlined
                      use-input
                      use-chips
                      multiple
                      emit-value
                      option-value="id"
                      option-label="name"
                      label="Category Name"
                      :error="$v.categoryIds.$error"
                    >
                      <template #error>
                        <li
                          v-for="(error, index) in $v.categoryIds.$errors"
                          :key="index"
                        >
                          <span>{{ error.$message }}</span>
                        </li>
                      </template>
                    </q-select>
                  </q-item-section>
                </q-item>
              </q-list>
            </q-form>
          </q-card-section>
          <q-card-actions align="center" class="text-teal">
            <q-btn
              label="Update Movie"
              :disable="$v.$invalid"
              @click="updateMovie()"
              color="primary"
            />
          </q-card-actions>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import Alert from 'src/components/Alert.vue';
import { useCategoryStore } from 'src/store/Category';
import { Category } from 'src/models/Category';
import { useActorStore } from 'src/store/Actor';
import { useAlertStore } from 'src/store/Alert';
import { useMovieStore } from 'src/store/Movie';
import useVuelidate from '@vuelidate/core';
import { required, helpers } from '@vuelidate/validators';
import config from 'src/config';
import { useQuasar } from 'quasar';
import { useImageUpload } from 'src/composables/useImageUpload';
import { useMultipleImageUpload } from 'src/composables/useMultipleImageUpload';
import { Actor } from 'src/models/Actor';
import { useRoute } from 'vue-router';
const $q = useQuasar();
const router = useRouter();
const route = useRoute();
const movieStore = useMovieStore();
const categoryStore = useCategoryStore();
const actorStore = useActorStore();
const alertStore = useAlertStore();
const { imagefiles, previewimageurls } = useMultipleImageUpload();
const { imagefile, previewimageurl } = useImageUpload();
const filteredcategories = ref<Category[]>([]);
const filteredactors = ref<Actor[]>([]);

onMounted(async () => {
  await categoryStore.getCategories();
  await actorStore.getActors();
  await movieStore.getAdminMovieById(parseInt(route.params.id as string));
  filteredactors.value = actors.value;
  filteredcategories.value = categories.value;
  movie.value.releaseDateString = movie.value.releaseDateString
    .split('.')
    .join('/');
});

const movie = computed(() => {
  return movieStore.getAdminMovie;
});

const actors = computed(() => {
  return actorStore.getActorsFromState;
});
const categories = computed(() => {
  return categoryStore.getCategoriesFromState;
});

async function updateMovie() {
  const formdata = new FormData();
  formdata.append('Id', movie.value.id as unknown as Blob);
  formdata.append('Name', movie.value.name);
  formdata.append('Summary', movie.value.summary);
  formdata.append('ReleaseDateString', movie.value.releaseDateString);
  formdata.append('Imdb', movie.value.imdb as unknown as Blob);
  formdata.append('Image', imagefile.value as unknown as Blob);
  for (let index = 0; index < movie.value.actorIds.length; index++) {
    formdata.append('ActorIds', movie.value.actorIds[index] as unknown as Blob);
  }
  for (let index = 0; index < movie.value.categoryIds.length; index++) {
    formdata.append(
      'CategoryIds',
      movie.value.categoryIds[index] as unknown as Blob
    );
  }
  for (let index = 0; index < imagefiles.value.length; index++) {
    formdata.append('Images', imagefiles.value[index] as unknown as Blob);
  }
  const response = await movieStore.updateMovie(formdata);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetMovie();
    await router.push({ name: 'DashboardMovies' });
  }
}
function confirm(id: number) {
  $q.dialog({
    title: 'Remove Image',
    message: 'Would you like to remove image',
    cancel: true,
    persistent: true,
  }).onOk(() => {
     void removeImage(id);
  });
}

async function removeImage(id: number) {
  const response = await movieStore.removeImageFromMovie(id);
  if (response) {
    let index = movie.value.movieImages.findIndex((c) => c.id == id);
    if (index > -1) {
      movie.value.movieImages.splice(index, 1);
    }
    $q.notify({
      message: response,
      type: 'positive',
    });
  }
}
const rules = computed(() => {
  return {
    name: {
      required: helpers.withMessage('Name is required', required),
    },
    summary: {
      required: helpers.withMessage('Summary is required', required),
    },
    imdb: {
      required: helpers.withMessage('Imdb is required', required),
    },
    releaseDateString: {
      required: helpers.withMessage('Release Date is required', required),
    },
    actorIds: {
      required: helpers.withMessage('Actor is required', required),
    },
    categoryIds: {
      required: helpers.withMessage('Category is required', required),
    },
  };
});

const $v = useVuelidate(rules, movie, { $autoDirty: true });

function filterFnCategory(val: string, update: (callback: () => void) => void) {
  update(() => {
    if (val === '') {
      update(() => {
        filteredcategories.value = categories.value;
      });
      return;
    }
    update(() => {
      const needle = val.toLowerCase();
      filteredcategories.value = categories.value.filter(
        (v) => v.name.toLowerCase().indexOf(needle) > -1
      );
    });
  });
}

function filterFnActor(val: string, update: (callback: () => void) => void) {
  update(() => {
    if (val === '') {
      update(() => {
        filteredactors.value = actors.value;
      });
      return;
    }
    update(() => {
      const needle = val.toLowerCase();
      filteredactors.value = actors.value.filter(
        (v) => v.fullName.toLowerCase().indexOf(needle) > -1
      );
    });
  });
}

function resetMovie() {
  $v.value.$reset();
  alertStore.clearAlert();
}
</script>
