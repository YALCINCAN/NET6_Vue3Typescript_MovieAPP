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
          <div class="tw-text-3xl">Profile</div>
          <Alert class="tw-w-full sm:tw-w-2/3"></Alert>
          <q-input
            outlined
            color="primary"
            label-color="white"
            v-model="user.userName"
            class="auth tw-w-full sm:tw-w-2/3"
            label="Username"
            :error="$v.userName.$error"
          >
            <template #error>
              <li v-for="(error, index) in $v.userName.$errors" :key="index">
                <span>{{ error.$message }}</span>
              </li>
            </template>
          </q-input>
          <q-input
            outlined
            color="primary"
            label-color="white"
            v-model="user.fullName"
            class="auth tw-w-full sm:tw-w-2/3"
            label="Full Name"
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
            label-color="white"
            v-model="user.email"
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
            @click="updateUser()"
          >
            Update
          </q-btn>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import ProfileSidebar from 'src/components/ProfileSidebar.vue';
import { computed, reactive } from 'vue';
import useVuelidate from '@vuelidate/core';
import { required, email, helpers } from '@vuelidate/validators';
import { useAuthStore } from 'src/store/Auth';
import { useUserStore } from 'src/store/User';
import { useQuasar } from 'quasar';
import Alert from 'src/components/Alert.vue';
import { useAlertStore } from 'src/store/Alert';
const $q = useQuasar();
const authStore = useAuthStore();
const userStore = useUserStore();
const alertStore = useAlertStore();
const user = computed(() => {
  return reactive({ ...authStore.getUser });
});

async function updateUser() {
  const response = await userStore.updateUser(user.value);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    alertStore.clearAlert();
    $v.value.$reset();
  }
}
const rules = computed(() => {
  return {
    userName: {
      required: helpers.withMessage('Username is required', required),
    },
    fullName: {
      required: helpers.withMessage('Full Name is required', required),
    },
    email: {
      required: helpers.withMessage('Email is required', required),
      email: email,
    },
  };
});

const $v = useVuelidate(rules, user, { $autoDirty: true });
</script>

<style>
.auth .q-field__control::before {
  border-color: white !important;
}

.auth .q-field__native {
  color: white !important;
}
</style>
