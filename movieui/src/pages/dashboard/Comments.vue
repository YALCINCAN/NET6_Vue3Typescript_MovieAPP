<template>
  <q-page class="q-pa-sm">
    <q-card>
      <q-table
        title="Comments"
        :rows="comments"
        :hide-header="mode === 'grid'"
        :columns="columns"
        :grid="mode == 'grid'"
        :filter="filter"
        :pagination="pagination"
        separator="cell"
      >
        <template v-slot:top-right="props">
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
            <q-tooltip :disable="$q.platform.is.mobile" v-close-popup
              >{{
                props.inFullscreen ? 'Exit Fullscreen' : 'Toggle Fullscreen'
              }}
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
            <q-tooltip :disable="$q.platform.is.mobile" v-close-popup
              >{{ mode === 'grid' ? 'List' : 'Grid' }}
            </q-tooltip>
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
                @click="removeComment(props.row.id)"
                color="red"
                icon="delete"
              />
            </div>
          </q-td>
        </template>
      </q-table>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useCommentStore } from 'src/store/Comment';
import { useExportCsv } from 'src/composables/useExportCsv';
import { QTableColumn } from 'src/composables/QTableColumn';
import { AdminComment } from 'src/models/AdminComment';
import { date } from 'quasar';

const columns: QTableColumn<AdminComment>[] = [
  {
    name: 'description',
    align: 'left',
    label: 'Description',
    field: row=>row.description,
    sortable: true,
  },
  {
    name: 'commentDate',
    align: 'left',
    label: 'CommentDate',
    field: (row) => formatDate(row.commentDate),
    sortable: true,
  },
  {
    name: 'action',
    align: 'left',
    label: 'Actions',
    field: () => undefined,
  },
];

const commentStore = useCommentStore();
onMounted(async () => {
  await commentStore.getComments();
});
const filter = ref<string>('');
const mode = ref<string>('list');
const pagination = {
  rowsPerPage: 20,
};
const $q = useQuasar();

const comments = computed<AdminComment[]>(() => {
  return commentStore.getAdminCommentsFromState;
});

const { exportTable } = useExportCsv(comments, columns, 'comments');
function formatDate(commentDate:Date) {
  return date.formatDate(commentDate, 'DD.MM.YYYY HH:mm');
}
async function removeComment(id: number) {
  const response = await commentStore.removeComment(id);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
  }
}
</script>
<style scoped>
.q-chip__content {
  display: block;
  text-align: center;
}
</style>
