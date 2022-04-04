<template>
  <q-page class="q-pa-sm">
    <q-card>
      <q-table
        title="Roles"
        :rows="roles"
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
                @click="getRole(props.row.id)"
                color="primary"
                icon="edit"
              />
              <q-btn
                dense
                @click="removeRole(props.row.id)"
                color="red"
                icon="delete"
              />
            </div>
          </q-td>
        </template>
      </q-table>
    </q-card>
    <q-dialog v-model="showmodal" persistent>
      <q-card style="width: 600px; max-width: 60vw">
        <q-card-section>
          <Alert></Alert>
          <div class="text-h6">
            <span v-if="role.id">Update Role</span>
            <span v-else>Add New Role</span>
            <q-btn
              round
              flat
              dense
              icon="close"
              class="float-right"
              color="grey-8"
              v-close-popup
              @click="resetRole()"
            ></q-btn>
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
                    type="text"
                    outlined
                    v-model="role.name"
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
            </q-list>
          </q-form>
        </q-card-section>
        <q-card-actions align="right" class="text-teal">
          <q-btn
            label="Update"
            @click="updateRole"
            color="primary"
            v-if="role.id"
            :disable="$v.$invalid"
          />
          <q-btn
            label="Add"
            @click="addRole"
            :disable="$v.$invalid"
            color="primary"
            v-else
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useRoleStore } from 'src/store/Role';
import { useExportCsv } from 'src/composables/useExportCsv';
import useVuelidate from '@vuelidate/core';
import { required, helpers } from '@vuelidate/validators';
import { Role } from 'src/models/Role';
import { QTableColumn } from 'src/composables/QTableColumn';
import { useAlertStore } from 'src/store/Alert';
import Alert from 'src/components/Alert.vue';
const columns: QTableColumn<Role>[] = [
  {
    name: 'name',
    align: 'left',
    label: 'Role Name',
    field: 'name',
    sortable: true,
  },
  {
    name: 'action',
    align: 'left',
    label: 'Actions',
    field: () => undefined,
  },
];

const roleStore = useRoleStore();
const alertStore = useAlertStore();
onMounted(async () => {
  await roleStore.getRoles();
});
const filter = ref<string>('');
const mode = ref<string>('list');
const pagination = {
  rowsPerPage: 20,
};
const showmodal = ref<boolean>(false);
const $q = useQuasar();
const role = ref<Role>({} as Role);
const roles = computed<Role[]>(() => {
  return roleStore.getRolesFromState;
});

const { exportTable } = useExportCsv(roles, columns, 'roles');

async function addRole() {
  const response = await roleStore.addRole(role.value);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetRole();
  }
}

async function updateRole() {
  const response = await roleStore.updateRole(role.value);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetRole();
  }
}
async function removeRole(id: string) {
  const response = await roleStore.removeRole(id);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetRole();
  }
}

function getRole(id: string) {
  showmodal.value = true;
  role.value = { ...roleStore.getRole(id) };
}

const rules = computed(() => {
  return {
    name: {
      required: helpers.withMessage('Name is required', required),
    },
  };
});

const $v = useVuelidate(rules, role, { $autoDirty: true });

function resetRole() {
  showmodal.value = false;
  role.value = {} as Role;
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
