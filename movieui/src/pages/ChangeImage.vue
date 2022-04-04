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
          <q-file
            clearable
            v-model="imagefile"
            accept="image/png, image/jpeg"
            class="auth tw-w-full sm:tw-w-2/3"
            label="Select Image"
            :error="imagefile == null"
          >
            <template #error>
              <span>Please select image</span>
            </template>
            <template v-slot:prepend>
              <q-icon name="cloud_upload" />
            </template>
          </q-file>
          <div class="tw-flex tw-flex-row justify-center tw-gap-4">
            <div v-if="previewimageurl">
              <div class="row justify-center">
                <label class="text-white q-mb-xs">Selected Image</label>
              </div>
              <div class="row justify-center">
                <q-avatar size="72px">
                  <img :src="previewimageurl" />
                </q-avatar>
              </div>
            </div>
            <div v-if="user.image">
              <div class="row justify-center">
                <label class="text-white q-mb-xs">Current Image</label>
              </div>
              <div class="row justify-center">
                <q-avatar size="72px">
                  <img :src="config.url + user.image" />
                </q-avatar>
              </div>
            </div>
            <div class="col-3" v-else>
              <div class="row justify-center">
                <label class="text-white q-mb-xs">Current Image</label>
              </div>
              <div class="row justify-center">
                <q-avatar size="72px">
                  <img src="~assets/defaultavatar.png" />
                </q-avatar>
              </div>
            </div>
          </div>
          <q-btn
            :disable="imagefile == null"
            class="
              tw-rounded-lg tw-px-4 tw-py-2 tw-bg-gray-500 tw-text-white
              hover:tw-text-gray-500 hover:tw-bg-white
            "
            @click="changeImage()"
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
import { useAuthStore } from 'src/store/Auth';
import { useUserStore } from 'src/store/User';
import { useQuasar } from 'quasar';
import { useImageUpload } from 'src/composables/useImageUpload';
import config from 'src/config';

const $q = useQuasar();
const authStore = useAuthStore();
const userStore = useUserStore();
const user = computed(() => {
  return reactive({ ...authStore.getUser });
});

async function changeImage() {
  const formdata = new FormData();
  formdata.append('ImageFile', imagefile.value as unknown as Blob);
  const response = await userStore.changeImage(formdata);
  if (response) {
    $q.notify({
      message: response,
      type: 'positive',
    });
    imagefile.value=null;
  }
}

const { imagefile, previewimageurl } = useImageUpload();
</script>

<style>
.auth .q-field__control::before {
  border-color: white !important;
}

.auth .q-field__native {
  color: white !important;
}
</style>
