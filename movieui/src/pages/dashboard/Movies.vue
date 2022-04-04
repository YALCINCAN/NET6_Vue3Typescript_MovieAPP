<template>
  <q-page class="q-pa-sm">
    <q-card>
      <q-table
        title="Movies"
        :rows="movies"
        :hide-header="mode === 'grid'"
        :columns="columns"
        :grid="mode == 'grid'"
        :filter="filter"
        :pagination="pagination"
        separator="cell"
      >
        <template v-slot:top-right="props">
          <q-btn
            outline
            color="primary"
            label="Add New"
            to="/dashboard/movies/add"
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
                :to="{
                  name: 'DashboardMovieUpdate',
                  params: { id: props.row.id },
                }"
                color="primary"
                icon="edit"
              />
              <q-btn
                dense
                @click="removeMovie(props.row.id)"
                color="red"
                icon="delete"
              />
            </div>
          </q-td>
        </template>
        <template v-slot:body-cell-Image="props">
          <q-td :props="props">
            <q-img  class="tw-object-contain" :src="config.url + props.row.mainImage"></q-img>
          </q-td>
        </template>
      </q-table>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useExportCsv } from 'src/composables/useExportCsv';
import { useMovieStore } from 'src/store/Movie';
import config from 'src/config';
import { Movie } from 'src/models/Movie';
import { QTableColumn } from 'src/composables/QTableColumn';

const columns: QTableColumn<Movie>[] = [
  {
    name: 'Image',
    align: 'left',
    label: 'Main Image',
    field: 'mainImage',
    sortable: true,
  },
  {
    name: 'Name',
    align: 'left',
    label: 'Name',
    field: 'name',
    sortable: true,
  },
  {
    name: 'Release Date',
    align: 'left',
    label: 'Release Date',
    field: 'releaseDate',
    sortable: true,
  },
  {
    name: 'Categories',
    align: 'left',
    label: 'Categories',
    field: (row) =>
      row.categories
        .map(function (category) {
          return category.name;
        })
        .join(', '),
    sortable: true,
  },
  {
    name: 'action',
    align: 'left',
    label: 'Actions',
    field: () => undefined,
  },
];

const filter = ref<string>('');
const mode = ref<string>('list');
const pagination = {
  rowsPerPage: 20,
};

onMounted(async () => {
  await movieStore.getMovies();
});
const movieStore = useMovieStore();
const movies = computed<Movie[]>(() => {
  return movieStore.getMoviesFromState;
});
const $q = useQuasar();

const { exportTable } = useExportCsv<Movie>(movies, columns, 'movies');


async function removeMovie(id: number) {
  const response = await movieStore.removeMovie(id);
  if(response){
    $q.notify({
      message:response,
      color:'positive'
    })
  }
}
</script>
<style scoped>
.q-chip__content {
  display: block;
  text-align: center;
}
</style>
