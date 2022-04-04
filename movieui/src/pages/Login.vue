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
      <h2 class="tw-text-4xl">Login</h2>
      <Alert class="tw-w-full sm:tw-w-2/3"></Alert>
      <q-input
        outlined
        type="text"
        color="primary"
        label-color="white"
        v-model="logincredentials.username"
        class="auth tw-w-full sm:tw-w-2/3"
        label="Username"
        :error="$v.username.$error"
      >
        <template #error>
          <li v-for="(error, index) in $v.username.$errors" :key="index">
            <span>{{ error.$message }}</span>
          </li>
        </template>
      </q-input>
      <q-input
        outlined
        color="primary"
        type="password"
        label-color="white"
        v-model="logincredentials.password"
        class="auth tw-w-full sm:tw-w-2/3"
        label="Password"
        :error="$v.password.$error"
      >
        <template #error>
          <li v-for="(error, index) in $v.password.$errors" :key="index">
            <span>{{ error.$message }}</span>
          </li>
        </template>
      </q-input>
      <div
        class="tw-flex tw-items-center tw-justify-between tw-w-full sm:tw-w-2/3"
      >
        <q-btn
          :disable="$v.$invalid"
          class="
            tw-rounded-lg tw-px-4 tw-py-2 tw-bg-gray-500 tw-text-white
            hover:tw-text-gray-500 hover:tw-bg-white
          "
          @click="login()"
        >
          Login
        </q-btn>
        <a
          class="
            tw-inline-block
            tw-align-baseline
            tw-font-bold
            tw-text-sm
            tw-text-blue-500
            hover:tw-text-white
          "
          href="/forgotpassword"
        >
          Forgot Password?
        </a>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Login } from 'src/models/Login';
import { reactive, computed } from 'vue';
import { useRouter } from 'vue-router';
import useVuelidate from '@vuelidate/core';
import { required, helpers } from '@vuelidate/validators';
import { useAuthStore } from 'src/store/Auth';
import Alert from 'src/components/Alert.vue';

const logincredentials = reactive<Login>({} as Login);
const authStore = useAuthStore();
const router = useRouter();
async function login() {
  var response = await authStore.login(logincredentials);
  if (response) {
    await router.push('/');
  }
}
const rules = computed(() => {
  return {
    username: {
      required: helpers.withMessage('Username is required', required),
    },
    password: {
      required: helpers.withMessage('Password is required', required),
    },
  };
});

const $v = useVuelidate(rules, logincredentials, { $autoDirty: true });
</script>

<style>
.auth .q-field__control::before {
  border-color: white !important;
}

.auth .q-field__native {
  color: white !important;
}
</style>
