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
      <h2 class="tw-text-4xl">Forgot Password</h2>
      <Alert class="tw-w-full sm:tw-w-2/3"></Alert>
      <q-input
        outlined
        type="text"
        color="primary"
        label-color="white"
        v-model="forgotpassworddata.email"
        class="auth tw-w-full sm:tw-w-2/3"
        label="Email"
        :error="$v.email.$error"
      >
        <template #error>
          <li v-for="(error, index) in $v.email.$errors" :key="index">
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
        @click="forgotPassword()"
      >
        Send Mail
      </q-btn>
    </div>
  </div>
</template>

<script setup lang="ts">

import { ref, computed } from 'vue';
import useVuelidate from '@vuelidate/core';
import { required, helpers ,email} from '@vuelidate/validators';
import { useUserStore } from 'src/store/User';
import Alert from 'src/components/Alert.vue';
import { ForgotPassword } from 'src/models/ForgotPassword';

const forgotpassworddata =ref<ForgotPassword>({} as ForgotPassword);
const userStore = useUserStore();
async function forgotPassword() {
 await userStore.forgotPassword(forgotpassworddata.value)

}
const rules = computed(() => {
  return {
    email: {
      required: helpers.withMessage('Email is required', required),
      email:email
    },
  };
});

const $v = useVuelidate(rules, forgotpassworddata, { $autoDirty: true });
</script>

<style>
.auth .q-field__control::before {
  border-color: white !important;
}

.auth .q-field__native {
  color: white !important;
}
</style>