<template>
  <div
    class="
      tw-max-w-7xl tw-mx-auto tw-mt-8 tw-px-3
      lg:tw-px-0
      tw-mb-8 tw-text-white
    "
  >
    <div class="tw-grid tw-grid-cols-12 tw-gap-2 tw-content-center">
      <div class="tw-col-span-12 sm:tw-col-span-4 md:tw-col-span-3">
        <ProfileSidebar></ProfileSidebar>
      </div>
      <div
        class="
          tw-col-span-12
          sm:tw-col-span-8
          md:tw-col-span-9
          tw-mt-2
          sm:tw-mt-0
        "
      >
        <div
          class="tw-flex tw-flex-col tw-items-center tw-justify-center tw-gap-3"
        >
          <div class="tw-text-3xl">Change Password</div>
          <Alert class="tw-w-full sm:tw-w-2/3"></Alert>
          <q-input
            outlined
            color="primary"
            type="password"
            label-color="white"
            v-model="changepassworddata.currentpassword"
            class="auth tw-w-full sm:tw-w-2/3"
            label="Current Password"
            :error="$v.currentpassword.$error"
          >
            <template #error>
              <li v-for="(error, index) in $v.currentpassword.$errors" :key="index">
                <span>{{ error.$message }}</span>
              </li>
            </template>
          </q-input>
          <q-input
            outlined
            color="primary"
            type="password"
            label-color="white"
            v-model="changepassworddata.newpassword"
            class="auth tw-w-full sm:tw-w-2/3"
            label="New Password"
            :error="$v.newpassword.$error"
          >
            <template #error>
              <li v-for="(error, index) in $v.newpassword.$errors" :key="index">
                <span>{{ error.$message }}</span>
              </li>
            </template>
          </q-input>
          <q-input
            outlined
            color="primary"
            type="password"
            label-color="white"
            v-model="changepassworddata.confirmpassword"
            class="auth tw-w-full sm:tw-w-2/3"
            label="Confirm Password"
            :error="$v.confirmpassword.$error"
          >
            <template #error>
              <li
                v-for="(error, index) in $v.confirmpassword.$errors"
                :key="index"
              >
                <span>{{ error.$message }}</span>
              </li>
            </template>
          </q-input>
          <q-btn
            :disable="$v.$invalid"
            class="
              tw-rounded-lg tw-px-4 tw-py-2 tw-bg-gray-500 tw-text-white
              hover:tw-text-gray-500 hover:tw-bg-white
            "
            @click="changePassword"
          >
            Change Password
          </q-btn>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import ProfileSidebar from 'src/components/ProfileSidebar.vue';
import { computed, ref } from 'vue';
import useVuelidate from '@vuelidate/core';
import { required, helpers, minLength, sameAs } from '@vuelidate/validators';
import { oneUpperCase } from 'src/utilities/validators';
import { ChangePassword } from 'src/models/ChangePassword';
import { useUserStore } from 'src/store/User';
import { useQuasar } from 'quasar';
import Alert from 'src/components/Alert.vue';
import { useAlertStore } from 'src/store/Alert';

const changepassworddata = ref<ChangePassword>({} as ChangePassword);
const userStore = useUserStore();
const alertStore = useAlertStore();
const $q= useQuasar();

async function changePassword(){
  const response = await userStore.changePassword(changepassworddata.value);
  if(response){
     $q.notify({
      message: response,
      type: 'positive',
    });
    changepassworddata.value = {} as ChangePassword;
    alertStore.clearAlert();
    $v.value.$reset();
  }
}
const rules = computed(() => {
  return {
    currentpassword: {
      required: helpers.withMessage('Current Password is required', required),
    },
    newpassword: {
      required: helpers.withMessage('Password is required', required),
      minLength: minLength(9),
      oneUpperCase: helpers.withMessage(
        'Password need one must upper case',
        oneUpperCase
      ),
    },
    confirmpassword: {
      required: helpers.withMessage('Confirm password is required', required),
      sameAs: helpers.withMessage(
        'Confirm password dont match with password',
        sameAs(changepassworddata.value.newpassword)
      ),
    },
  };
});

const $v = useVuelidate(rules, changepassworddata, { $autoDirty: true });
</script>

<style>
.auth .q-field__control::before {
  border-color: white !important;
}

.auth .q-field__native {
  color: white !important;
}
</style>
