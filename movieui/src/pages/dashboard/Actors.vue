<template>
  <q-page class="q-pa-sm">
    <q-card>
      <q-table
        title="Actors"
        :rows="actors"
        :hide-header="mode === 'grid'"
        :columns="columns"
        :grid="mode == 'grid'"
        :filter="filter"
        :pagination="pagination"
        separator="cell"
      >
        <template v-slot:top-right="props">
          <q-btn
            @click="showmodal = true"
            outline
            color="primary"
            label="Add New"
            class="q-mr-xs"
          />
          <q-input
            outlined
            dense
            debounce="300"
            v-model="filter"
            placeholder="Search"
          >
            <template v-slot:append>
              <q-icon name="search" />
            </template>
          </q-input>
          <q-btn
            flat
            round
            dense
            :icon="props.inFullscreen ? 'fullscreen_exit' : 'fullscreen'"
            @click="props.toggleFullscreen"
            v-if="mode === 'list'"
          >
            <q-tooltip :disable="$q.platform.is.mobile" v-close-popup>
              {{ props.inFullscreen ? 'Exit Fullscreen' : 'Toggle Fullscreen' }}
            </q-tooltip>
          </q-btn>

          <q-btn
            flat
            round
            dense
            :icon="mode === 'grid' ? 'list' : 'grid_on'"
            @click="mode = mode === 'grid' ? 'list' : 'grid'"
            v-if="!props.inFullscreen"
          >
            <q-tooltip :disable="$q.platform.is.mobile" v-close-popup>{{
              mode === 'grid' ? 'List' : 'Grid'
            }}</q-tooltip>
          </q-btn>

          <q-btn
            color="primary"
            icon-right="archive"
            label="Export to csv"
            no-caps
            @click="exportTable"
          />
        </template>
        <template v-slot:body-cell-action="props">
          <q-td :props="props">
            <div class="q-gutter-sm">
              <q-btn
                dense
                @click="getActor(props.row.id)"
                color="primary"
                icon="edit"
              />
              <q-btn
                dense
                @click="removeActor(props.row.id)"
                color="red"
                icon="delete"
              />
            </div>
          </q-td>
        </template>
        <template v-slot:body-cell-Image="props">
          <q-td :props="props">
            <q-img
              fit="contain"
              height="72px"
              width="72px"
              :src="config.url + props.row.image"
            ></q-img>
          </q-td>
        </template>
      </q-table>
    </q-card>
    <q-dialog v-model="showmodal" persistent>
      <q-card style="width: 600px; max-width: 60vw">
        <q-card-section>
          <Alert></Alert>
          <div class="text-h6">
            <span v-if="actor.id > 0">Update Actor</span>
            <span v-else>Add New Actor</span>
            <q-btn
              round
              flat
              dense
              icon="close"
              class="float-right"
              color="grey-8"
              v-close-popup
              @click="resetActor()"
            ></q-btn>
          </div>
        </q-card-section>
        <q-separator inset></q-separator>
        <q-card-section class="q-pt-none">
          <q-form class="q-gutter-md">
            <q-list>
              <q-item>
                <q-item-section>
                  <q-file
                    clearable
                    v-model="imagefile"
                    accept="image/png, image/jpeg"
                    label="Select Image"
                    :error="imagefile == null && !actor.id"
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
                  <q-input
                    dense
                    type="text"
                    outlined
                    v-model="actor.fullName"
                    label="Full Name"
                    :error="$v.fullName.$error"
                  >
                    <template #error>
                      <li
                        v-for="(error, index) in $v.fullName.$errors"
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
                  <q-editor v-model="actor.about" min-height="5rem" />
                </q-item-section>
              </q-item>

              <div class="row justify-center">
                <div class="col-3" v-if="previewimageurl">
                  <div class="row justify-center">
                    <label class="text-dark q-mb-xs">Selected Image</label>
                  </div>
                  <div class="row justify-center">
                    <q-img
                      fit="contain"
                      style="width: 80px; height: 80px"
                      :src="previewimageurl"
                    />
                  </div>
                </div>
                <div class="col-3" v-if="actor.image">
                  <div class="row justify-center">
                    <label class="text-dark q-mb-xs">Current Image</label>
                  </div>
                  <div class="row justify-center">
                    <q-img
                      fit="contain"
                      style="width: 80px; height: 80px"
                      :src="config.url + actor.image"
                    />
                  </div>
                </div>
              </div>
            </q-list>
          </q-form>
        </q-card-section>
        <q-card-actions align="right" class="text-teal">
          <q-btn
            label="Update"
            @click="updateActor"
            :disable="$v.$invalid"
            color="primary"
            v-if="actor.id > 0"
          />
          <q-btn
            label="Add"
            :disable="imagefile == null || $v.$invalid"
            @click="addActor"
            color="primary"
            v-else
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useImageUpload } from 'src/composables/useImageUpload';
import { useExportCsv } from 'src/composables/useExportCsv';
import Alert from 'components/Alert.vue';
import config from 'src/config';
import useVuelidate from '@vuelidate/core';
import { required, helpers } from '@vuelidate/validators';
import { useActorStore } from 'src/store/Actor';
import { useAlertStore } from 'src/store/Alert';
import { Actor } from 'src/models/Actor';
import { QTableColumn } from 'src/composables/QTableColumn';
const columns: QTableColumn<Actor>[] = [
  {
    name: 'Image',
    align: 'left',
    label: 'Image',
    field: 'image',
    sortable: true,
  },
  {
    name: 'Full Name',
    align: 'left',
    label: 'Full Name',
    field: 'fullName',
    sortable: true,
  },
  {
    name: 'action',
    align: 'left',
    label: 'Actions',
    field: () => undefined,
  },
];
const filter = ref('');
const mode = ref('list');
const pagination = {
  rowsPerPage: 20,
};
const actorStore = useActorStore();
const alertStore = useAlertStore();
onMounted(async () => {
  await actorStore.getActors();
});
const showmodal = ref<boolean>(false);
const actor = ref<Actor>({
  about: '',
} as Actor);
const actors = computed<Actor[]>(() => {
  return actorStore.getActorsFromState;
});
const $q = useQuasar();

const { exportTable } = useExportCsv(actors, columns, 'actors');
const { imagefile, previewimageurl } = useImageUpload();

function getActor(id: number) {
  showmodal.value = true;
  actor.value = { ...actorStore.getActor(id) };
}
async function addActor() {
  const formdata = new FormData();
  formdata.append('ImageFile', imagefile.value as unknown as Blob);
  formdata.append('FullName', actor.value.fullName);
  formdata.append('About', actor.value.about);
  const response = await actorStore.addActor(formdata);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    await actorStore.getActors();
    resetActor();
  }
}

async function updateActor() {
  const formdata = new FormData();
  formdata.append('Id', actor.value.id as unknown as Blob);
  formdata.append('FullName', actor.value.fullName);
  formdata.append('About', actor.value.about);
  formdata.append('ImageFile', imagefile.value as unknown as Blob);
  const response = await actorStore.updateActor(formdata);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetActor();
  }
}

async function removeActor(id: number) {
  const response = await actorStore.removeActor(id);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetActor();
  }
}

const rules = computed(() => {
  return {
    fullName: {
      required: helpers.withMessage('Name is required', required),
    },
    about: {
      required: helpers.withMessage('About is required', required),
    },
  };
});

const $v = useVuelidate(rules, actor, { $autoDirty: true });

function resetActor() {
  showmodal.value = false;
  actor.value = {} as Actor;
  actor.value.about='';
  imagefile.value = null;
  alertStore.clearAlert();
  $v.value.$reset();
}
</script>
<style scoped>
.q-chip__content {
  display: block;
  text-align: center;
}
</style>
