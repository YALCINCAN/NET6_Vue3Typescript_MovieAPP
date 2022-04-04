<template>
  <q-page class="q-pa-sm">
    <q-card>
      <q-table
        title="Users"
        :rows="userswithroles"
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
                color="primary"
                @click="getUser(props.row.user.id)"
                label="Edit"
              />
              <q-btn
                dense
                color="orange"
                @click="changeImage(props.row.user.id,props.row.user.image)"
                label="Change Image"
              />
              <q-btn
                dense
                color="purple"
                @click="getAssignedRolesByUserId(props.row.user.id)"
                label="Assign Role"
              />
              <q-btn
                dense
                color="green"
                @click="getUserForChangePassword(props.row.user.id)"
                label="Change Password"
              />
              <q-btn
                dense
                color="red"
                @click="deleteUser(props.row.user.id)"
                label="Remove"
              />
            </div>
          </q-td>
        </template>
        <template v-slot:body-cell-Roles="props">
          <q-td :props="props">
            <div class="q-gutter-sm">
              <q-badge
                v-for="role in props.row.roles"
                :key="role.id"
                color="teal"
                size="sm"
              >
                {{ role }}
              </q-badge>
            </div>
          </q-td>
        </template>
        <template v-slot:body-cell-Image="props">
          <q-td :props="props">
            <q-avatar size="72px" v-if="props.row.user.image">
              <img :src="config.url + props.row.user.image" />
            </q-avatar>
            <q-avatar size="72px" v-else>
              <img src="~assets/defaultavatar.png" />
            </q-avatar>
          </q-td>
        </template>
      </q-table>
    </q-card>
    <q-dialog v-model="edituser" persistent>
      <q-card style="width: 600px; max-width: 60vw">
        <q-card-section>
          <Alert></Alert>
          <div class="text-h6">
            <span>Update User</span>
            <q-btn
              round
              flat
              dense
              icon="close"
              class="float-right"
              color="grey-8"
              v-close-popup
              @click="resetUser()"
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
                    outlined
                    v-model="user.userName"
                    label="Username"
                    :error="$vuser.userName.$error"
                  >
                    <template #error>
                      <li
                        v-for="(error, index) in $vuser.userName.$errors"
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
                  <q-input
                    dense
                    outlined
                    v-model="user.fullName"
                    :error="$vuser.fullName.$error"
                    label="Full Name"
                  >
                    <template #error>
                      <li
                        v-for="(error, index) in $vuser.fullName.$errors"
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
                  <q-input
                    dense
                    outlined
                    v-model="user.email"
                    label="Email"
                    :error="$vuser.email.$error"
                  >
                    <template #error>
                      <li
                        v-for="(error, index) in $vuser.email.$errors"
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
            :disable="$vuser.$invalid"
            @click="UpdateUserByAdmin"
            color="primary"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="changeimage">
      <q-card style="width: 600px; max-width: 60vw">
        <q-card-section>
          <Alert></Alert>
          <div class="text-h6">
            <span>Change Photo</span>
            <q-btn
              round
              flat
              dense
              icon="close"
              class="float-right"
              color="grey-8"
              v-close-popup
              @click="resetUser()"
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
                    :error="imagefile == null && !changeimageuser.id"
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
                    <label class="text-dark q-mb-xs">Selected Image</label>
                  </div>
                  <div class="row justify-center">
                    <q-avatar size="72px">
                      <img :src="previewimageurl" />
                    </q-avatar>
                  </div>
                </div>
                <div class="col-3" v-if="changeimageuser.image">
                  <div class="row justify-center">
                    <label class="text-dark q-mb-xs">Current Image</label>
                  </div>
                  <div class="row justify-center">
                    <q-avatar size="72px">
                      <img :src="config.url + changeimageuser.image" />
                    </q-avatar>
                  </div>
                </div>
                <div class="col-3" v-else>
                  <div class="row justify-center">
                    <label class="text-dark q-mb-xs">Current Image</label>
                  </div>
                  <div class="row justify-center">
                    <q-avatar size="72px">
                      <img src="~assets/defaultavatar.png" />
                    </q-avatar>
                  </div>
                </div>
              </div>
            </q-list>
          </q-form>
        </q-card-section>
        <q-card-actions align="right" class="text-teal">
          <q-btn
            label="Chage Image"
            :disable="imagefile == null || !changeimageuser.id"
            @click="changeImageByAdmin()"
            color="primary"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="showassignedroles" persistent>
      <q-card style="width: 600px; max-width: 60vw">
        <q-card-section>
          <Alert></Alert>
          <div class="text-h6">
            <span>Assigned Roles</span>
            <q-btn
              round
              flat
              dense
              icon="close"
              class="float-right"
              color="grey-8"
              v-close-popup
              @click="resetUser()"
            ></q-btn>
            <div
              class="q-pa-md"
              v-for="userrole in assignedroles"
              :key="userrole.roleId"
            >
              <q-checkbox
                type="checkbox"
                color="deep-purple-7"
                v-model="userrole.exist"
              />
              {{ userrole.roleName }}
            </div>
          </div>
        </q-card-section>
        <q-card-actions align="right" class="text-teal">
          <q-btn label="Assign Role" @click="assignRole" color="primary" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="changepassword" persistent>
      <q-card style="width: 600px; max-width: 60vw">
        <q-card-section>
          <Alert></Alert>
          <div class="text-h6">
            <span>Change Password</span>
            <q-btn
              round
              flat
              dense
              icon="close"
              class="float-right"
              color="grey-8"
              v-close-popup
              @click="resetUser()"
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
                    outlined
                    type="password"
                    v-model="changepassworduserbyadmin.newpassword"
                    label="New Password"
                    :error="$vchangepassword.newpassword.$error"
                  >
                    <template #error>
                      <li
                        v-for="(error, index) in $vchangepassword.newpassword
                          .$errors"
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
                  <q-input
                    dense
                    outlined
                    type="password"
                    v-model="changepassworduserbyadmin.confirmpassword"
                    label="Confirm Password"
                    :error="$vchangepassword.confirmpassword.$error"
                  >
                    <template #error>
                      <li
                        v-for="(error, index) in $vchangepassword
                          .confirmpassword.$errors"
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
            label="Change Password"
            @click="passwordChangeByAdmin"
            color="primary"
            :disable="$vchangepassword.$invalid"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useUserStore } from 'src/store/User';
import { useExportCsv } from 'src/composables/useExportCsv';
import Alert from 'components/Alert.vue';
import useVuelidate from '@vuelidate/core';
import {
  required,
  helpers,
  sameAs,
  minLength,
  email,
} from '@vuelidate/validators';
import { oneUpperCase } from 'src/utilities/validators';
import { UserWithRoles } from 'src/models/UserWithRoles';
import { QTableColumn } from 'src/composables/QTableColumn';
import { User } from 'src/models/User';
import { ChangePasswordUserByAdmin } from 'src/models/ChangePasswordUserByAdmin';
import { useAlertStore } from 'src/store/Alert';
import { useRoleStore } from 'src/store/Role';
import { useImageUpload } from 'src/composables/useImageUpload';
import config from 'src/config';
import { ChangeImageUser } from 'src/models/ChangeImageUser';

const columns: QTableColumn<UserWithRoles>[] = [
  {
    name: 'Image',
    align: 'left',
    label: 'Image',
    field: (row) => row.user.image,
    sortable: true,
  },
  {
    name: 'userName',
    align: 'left',
    label: 'Username',
    field: (row) => row.user.userName,
    sortable: true,
  },
  {
    name: 'email',
    align: 'left',
    label: 'Email',
    field: (row) => row.user.email,
    sortable: true,
  },
  {
    name: 'fullName',
    label: 'Full Name',
    align: 'left',
    field: (row) => row.user.fullName,
    sortable: true,
  },
  {
    name: 'Roles',
    align: 'left',
    label: 'Roles',
    field: (row) => row.roles,
    sortable: true,
  },
  {
    name: 'action',
    align: 'left',
    label: 'Actions',
    field: () => undefined,
  },
];

const user = ref<User>({} as User);
const changepassworduserbyadmin = ref<ChangePasswordUserByAdmin>({} as ChangePasswordUserByAdmin);
const edituser = ref<boolean>(false);
const changeimage = ref<boolean>(false);
const changeimageuser=ref<ChangeImageUser>({} as ChangeImageUser)
const showassignedroles = ref<boolean>(false);
const changepassword = ref<boolean>(false);
onMounted(async () => {
  await getUsersWithRoles();
});
const userswithroles = computed<UserWithRoles[]>(() => {
  return userStore.getUserswithRolesFromState;
});

const assignedroles = computed(() => {
  return roleStore.getAssignedRolesFromState;
});

const { exportTable } = useExportCsv(userswithroles, columns, 'users');
const { imagefile, previewimageurl } = useImageUpload();
const $q = useQuasar();
const userStore = useUserStore();
const alertStore = useAlertStore();
const roleStore = useRoleStore();
const filter = ref<string>('');
const mode = ref<string>('list');
const pagination = {
  rowsPerPage: 7,
};

async function getUsersWithRoles() {
  await userStore.getUsersWithRoles();
}

async function UpdateUserByAdmin() {
  const response = await userStore.updateUserByAdmin(user.value);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetUser();
  }
}
async function deleteUser(userid: string) {
  const response = await userStore.removeUser(userid);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
  }
}

function getUser(id: string) {
  edituser.value = true;
  user.value = { ...userStore.getUser(id) } as User;
}

function changeImage(id: string,image:string) {
  changeimage.value = true;
  changeimageuser.value.id=id;
  changeimageuser.value.image=image
}

async function changeImageByAdmin() {
  const formdata = new FormData();
  formdata.append('Id', changeimageuser.value.id);
  formdata.append('ImageFile', imagefile.value as unknown as Blob);
  const response = await userStore.changeImageByAdmin(formdata);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetUser();
  }
}

async function getAssignedRolesByUserId(userid: string) {
  if (userid != '') {
    showassignedroles.value = true;
    await roleStore.getAssignedRolesByUserId(userid);
  }
}

function getUserForChangePassword(userid: string) {
  if (userid != '') {
    changepassword.value = true;
    changepassworduserbyadmin.value.id = userid;
  }
}

async function passwordChangeByAdmin() {
  const response = await userStore.passwordChangeByAdmin(
    changepassworduserbyadmin.value
  );
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetUser();
  }
}

async function assignRole() {
  const response = await roleStore.assignRole();
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    resetUser();
  }
}

const userrules = computed(() => {
  return {
    userName: {
      required: helpers.withMessage('Username is required', required),
    },
    fullName: {
      required: helpers.withMessage('Full Name is required', required),
    },
    email: {
      required: helpers.withMessage('Email  is required', required),
      email: helpers.withMessage('Email is not valid email address', email),
    },
  };
});

const changepasswordrules = computed(() => {
  return {
    newpassword: {
      required: helpers.withMessage('New Password is required', required),
      minLength: minLength(9),
      oneUpperCase: helpers.withMessage(
        'Password need one must upper case',
        oneUpperCase
      ),
    },
    confirmpassword: {
      required: helpers.withMessage('Confirm password is required', required),
      sameAs: helpers.withMessage(
        'Confirm password dont match with new password',
        sameAs(changepassworduserbyadmin.value.newpassword)
      ),
    },
  };
});

const $vchangepassword = useVuelidate(changepasswordrules, changepassworduserbyadmin, {
  $autoDirty: true,
});

const $vuser = useVuelidate(userrules, user, { $autoDirty: true });

function resetUser() {
  user.value = {} as User;
  changepassworduserbyadmin.value = {} as ChangePasswordUserByAdmin;
  edituser.value = false;
  showassignedroles.value = false;
  changepassword.value = false;
  changeimage.value = false;
  imagefile.value=null;
  changeimageuser.value={} as ChangeImageUser;
  alertStore.clearAlert();
  $vuser.value.$reset();
  $vchangepassword.value.$reset();
}
</script>

<style lang="scss" scoped></style>
