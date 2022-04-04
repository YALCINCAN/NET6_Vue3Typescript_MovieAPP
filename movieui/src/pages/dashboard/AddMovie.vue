<template>
  <q-page class="q-pa-sm">
    <div class="row q-col-gutter-md">
      <div class="col-12">
        <q-card>
          <Alert></Alert>
          <q-card-section>
            <div class="text-h6">
              <span>Add Movie</span>
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
                      v-model="addmovie.name"
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
                      :error="imagefile == null"
                    >
                      <template #error>
                        <span>Please select image</span>
                      </template>
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
                      :error="!imagefiles"
                    >
                      <template #error>
                        <span>Please select image</span>
                      </template>
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
                        <q-img :src="previewimageurl" />
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
                        <q-img :src="item"></q-img>
                      </div>
                    </div>
                  </div>
                </div>
                <q-item>
                  <q-item-section>
                    <q-input
                      dense
                      outlined
                      v-model="addmovie.imdb"
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
                        v-model="addmovie.releaseDateString"
                        mask="DD/MM/YYYY"
                      />
                      <span
                        v-if="
                          $v.releaseDateString.$error ||
                          !addmovie.releaseDateString
                        "
                        class="text-red text-center tw-my-2"
                        >Release Date is required</span
                      >
                    </q-item-section>
                  </q-item>
                </div>

                <q-item>
                  <q-item-section>
                    <q-editor v-model="addmovie.summary" min-height="15rem" placeholder="Summary">
                    </q-editor>
                    <span v-if="$v.summary.$error" class="text-red"
                      >Summary is required</span
                    >
                  </q-item-section>
                </q-item>
                <q-item>
                  <q-item-section>
                    <q-select
                      v-model="addmovie.actorIds"
                      :options="actors"
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
                      v-model="addmovie.categoryIds"
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
              label="Add Movie"
              :disable="imagefile == null || $v.$invalid"
              @click="addMovie()"
              color="primary"
            />
          </q-card-actions>
        </q-card>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { AddMovie } from 'src/models/AddMovie';
import { reactive, computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import Alert from 'src/components/Alert.vue';
import { useCategoryStore } from 'src/store/Category';
import { Category } from 'src/models/Category';
import { useActorStore } from 'src/store/Actor';
import { useAlertStore } from 'src/store/Alert';
import { useMovieStore } from 'src/store/Movie';
import useVuelidate from '@vuelidate/core';
import { required, helpers } from '@vuelidate/validators';
import { useQuasar } from 'quasar';
import { useImageUpload } from 'src/composables/useImageUpload';
import { useMultipleImageUpload } from 'src/composables/useMultipleImageUpload';
import { Actor } from 'src/models/Actor';

const addmovie = reactive<AddMovie>({ summary: '' } as AddMovie);
const $q = useQuasar();
const router = useRouter();
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
});

const actors = computed(() => {
  return actorStore.getActorsFromState;
});
const categories = computed(() => {
  return categoryStore.getCategoriesFromState;
});
async function addMovie() {
  const formdata = new FormData();
  formdata.append('Name', addmovie.name);
  formdata.append('Summary', addmovie.summary);
  formdata.append('ReleaseDateString', addmovie.releaseDateString);
  formdata.append('Imdb', addmovie.imdb as unknown as Blob);
  formdata.append('Image', imagefile.value as unknown as Blob);
  for (let index = 0; index < addmovie.actorIds.length; index++) {
    formdata.append('ActorIds', addmovie.actorIds[index] as unknown as Blob);
  }
  for (let index = 0; index < addmovie.categoryIds.length; index++) {
    formdata.append(
      'CategoryIds',
      addmovie.categoryIds[index] as unknown as Blob
    );
  }
  for (let index = 0; index < imagefiles.value.length; index++) {
    formdata.append('Images', imagefiles.value[index] as unknown as Blob);
  }
  const response = await movieStore.addMovie(formdata);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetMovie();
    await router.push({ name: 'DashboardMovies' });
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

const $v = useVuelidate(rules, addmovie, { $autoDirty: true });

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
