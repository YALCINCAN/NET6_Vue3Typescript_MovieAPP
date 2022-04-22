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
      <h2 class="tw-text-4xl">Register</h2>
      <Alert class="tw-w-full sm:tw-w-2/3"></Alert>
      <q-input
        outlined
        color="primary"
        label-color="white"
        v-model="registercredentials.username"
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
        type="text"
        label-color="white"
        v-model="registercredentials.fullName"
        class="auth tw-w-full sm:tw-w-2/3"
        label="FullName"
        :error="$v.fullName.$error"
      >
        <template #error>
          <li v-for="(error, index) in $v.fullName.$errors" :key="index">
            <span>{{ error.$message }}</span>
          </li>
        </template>
      </q-input>
      <q-input
        outlined
        color="primary"
        type="password"
        label-color="white"
        v-model="registercredentials.password"
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
      <q-input
        outlined
        color="primary"
        type="password"
        label-color="white"
        v-model="registercredentials.confirmpassword"
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
      
      <q-input
        outlined
        color="primary"
        type="email"
        label-color="white"
        v-model="registercredentials.email"
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
        @click="register()"
      >
        Register
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
  email,
} from '@vuelidate/validators';
import { oneUpperCase } from 'src/utilities/validators';
import { useAuthStore } from 'src/store/Auth';
import { Register } from 'src/models/Register';
import Alert from 'src/components/Alert.vue';
const registercredentials  = reactive<Register>({} as Register);
const authStore = useAuthStore();
async function register(){
 await authStore.register(registercredentials);
}

const rules = computed(() => {
  return {
    username: {
      required: helpers.withMessage('Username is required', required),
    },
    fullName: {
      required: helpers.withMessage('Full Name is required', required),
    },
    password: {
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
        sameAs(registercredentials.password)
      ),
    },
    email: {
      required: helpers.withMessage('Email is required', required),
      email: email,
    },
  };
});

const $v = useVuelidate(rules, registercredentials, { $autoDirty: true });
</script>

<style>
.auth .q-field__control::before {
  border-color: white !important;
}

.auth .q-field__native {
  color: white !important;
}
</style>
