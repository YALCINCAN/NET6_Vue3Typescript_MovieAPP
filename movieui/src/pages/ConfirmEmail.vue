<template>
  <div class="tw-max-w-7xl tw-mx-auto tw-px-3 tw-py-3">
    <Alert></Alert>
  </div>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';
import { reactive, onMounted } from 'vue';
import { ConfirmEmail } from 'src/models/ConfirmEmail';
import { useAuthStore } from 'src/store/Auth';
import Alert from 'src/components/Alert.vue';

const route = useRoute();
const authStore = useAuthStore();
const confirmemaildata = reactive<ConfirmEmail>({
  userId: route.params.userId as string,
  token: route.params.token as string,
});
async function confirmEmail() {
  await authStore.confirmEmail(confirmemaildata);
}
onMounted(async () => {
  await confirmEmail();
});
</script>

<style scoped></style>
