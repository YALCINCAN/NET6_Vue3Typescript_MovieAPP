<template>
  <div
    class="
      tw-max-w-7xl
      tw-mx-auto
      tw-flex
      tw-justify-center
      tw-items-center
      tw-min-h-screen
      tw-px-3
      tw-my-5
      sm:tw-my-0
    "
  >
    <div
      class="
        tw-text-white
        tw-w-full
        tw-max-w-lg
        tw-border
        tw-border-white
        tw-p-3
        tw-flex
        tw-flex-col
        tw-justify-center
        tw-items-center
        tw-gap-4
      "
    >
      <h2 class="tw-text-4xl">Reset Password</h2>
      <Alert class="tw-w-full sm:tw-w-2/3"></Alert>
      <q-input
        outlined
        color="primary"
        type="password"
        label-color="white"
        v-model="resetpassworddata.newpassword"
        class="auth tw-w-full sm:tw-w-2/3"
        label="Password"
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
        v-model="resetpassworddata.confirmpassword"
        class="auth tw-w-full sm:tw-w-2/3"
        label="Confirm Password"
        :error="$v.confirmpassword.$error"
      >
        <template #error>
          <li v-for="(error, index) in $v.confirmpassword.$errors" :key="index">
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
        @click="resetPassword()"
      >
        Reset Password
      </q-btn>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, computed } from 'vue';
import useVuelidate from '@vuelidate/core';
import {
  required,
  helpers,
  minLength,
  sameAs,
} from '@vuelidate/validators';
import { oneUpperCase } from 'src/utilities/validators';
import { useUserStore } from 'src/store/User';
import {useRoute} from 'vue-router'
import Alert from 'src/components/Alert.vue';
import { ResetPassword } from 'src/models/ResetPassword';
const route = useRoute();
const resetpassworddata = reactive<ResetPassword>({
token:route.params.token as string,
email:route.params.email as string
} as ResetPassword);
const userStore = useUserStore();
async function resetPassword(){
 await userStore.resetPassword(resetpassworddata);
}

const rules = computed(() => {
  return {
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
        sameAs(resetpassworddata.newpassword)
      ),
    },
  };
});

const $v = useVuelidate(rules, resetpassworddata, { $autoDirty: true });
</script>

<style>
.auth .q-field__control::before {
  border-color: white !important;
}

.auth .q-field__native {
  color: white !important;
}
</style>
