<template>
  <q-layout view="hhh lpR fff">
    <q-header class="tw-bg-[#1A202C] tw-border-b tw-border-white tw-text-white">
      <div
        class="
          tw-flex
          tw-justify-between
          tw-max-w-7xl
          tw-mx-auto
          tw-items-center
          tw-h-16
          tw-px-3
          lg:tw-px-0
        "
      >
        <router-link to="/">
          <q-toolbar-title class="tw-text-2xl"
            ><i class="fas fa-film"></i> MovieFlix</q-toolbar-title
          >
        </router-link>

        <div class="tw-hidden md:tw-flex">
          <search></search>
        </div>

        <div class="tw-items-center tw-gap-5 tw-hidden md:tw-flex">
          <router-link v-if="!loggedIn" to="/login"
            ><a class="tw-text-lg">Login</a></router-link
          >
          <router-link v-if="!loggedIn" to="/register"
            ><a class="tw-text-lg">Register</a></router-link
          >
        </div>

        <q-drawer
          v-if="leftDrawerOpen"
          v-model="leftDrawerOpen"
          bordered
          content-class="bg-black"
          :width="240"
        >
          <q-scroll-area class="fit">
            <q-list padding>
              <q-item to="/" exact clickable v-close-popup>
                <q-item-section>
                  <q-item-label class="tw-text-black">Home</q-item-label>
                </q-item-section>
              </q-item>
              <q-item
                to="/profile"
                v-if="loggedIn"
                exact
                clickable
                v-close-popup
              >
                <q-item-section>
                  <q-item-label class="tw-text-black">Profile</q-item-label>
                </q-item-section>
              </q-item>
              <q-item
                clickable
                class="tw-text-black"
                v-close-popup
                to="/dashboard/movies"
                v-if="loggedIn && user != null && roles.includes('Admin')"
              >
                <q-item-section>Dashboard</q-item-section>
              </q-item>
              <q-item
                v-if="!loggedIn"
                to="/login"
                exact
                clickable
                v-close-popup
                class="tw-text-black"
              >
                <q-item-section>
                  <q-item-label>Login</q-item-label>
                </q-item-section>
              </q-item>
              <q-item
                v-if="!loggedIn"
                to="/register"
                exact
                clickable
                v-close-popup
                class="tw-text-black"
              >
                <q-item-section>
                  <q-item-label>Register</q-item-label>
                </q-item-section>
              </q-item>
              <q-item
                v-if="loggedIn"
                clickable
                @click.prevent="logout"
                v-close-popup
                class="tw-text-black"
              >
                <q-item-section>
                  <q-item-label>Logout</q-item-label>
                </q-item-section>
              </q-item>
              <q-separator class="q-my-md" />
            </q-list>
          </q-scroll-area>
        </q-drawer>

        <q-btn
          class="tw-flex md:tw-hidden"
          flat
          dense
          round
          @click="leftDrawerOpen = !leftDrawerOpen"
          aria-label="Menu"
          icon="menu"
          color="white"
        />
        
        <div class="tw-gap-3 tw-items-center tw-hidden md:tw-flex" v-if="user && loggedIn">
          <q-item to="/favorites" exact-active-class="active-class">
            <q-item-section avatar>
              <q-icon color="red" name="fas fa-heart" />
            </q-item-section>
            <q-item-section>My Favorites</q-item-section>
          </q-item>
          <q-btn-dropdown
            class="glossy q-ml-md"
            color="purple"
            :label="user.fullName"
          >
            <div class="q-pa-md">
              <div class="column items-center">
                <q-avatar size="72px">
                  <img
                    :src="config.url + user.image"
                    v-if="user.image"
                    class="q-mx-auto"
                  />
                  <img
                    class="q-mx-auto"
                    src="~assets/defaultavatar.png"
                    v-else
                  />
                </q-avatar>
                <q-list>
                  <q-item
                    to="/dashboard/movies"
                    clickable
                    v-if="loggedIn && user != null && roles.includes('Admin')"
                  >
                    <q-item-section>
                      <q-item-label>Dashboard</q-item-label>
                    </q-item-section>
                  </q-item>
                  <q-item to="/profile" clickable v-close-popup>
                    <q-item-section>
                      <q-item-label>Profile</q-item-label>
                    </q-item-section>
                  </q-item>
                </q-list>
                <q-btn
                  color="primary"
                  label="Logout"
                  push
                  @click.prevent="logout"
                  size="sm"
                  class="q-mt-md"
                  v-close-popup
                />
              </div>
            </div>
          </q-btn-dropdown>
        </div>
      </div>
    </q-header>

    <q-page-container>
      <router-view />
    </q-page-container>

    <q-footer
      class="tw-bg-[#1A202C] tw-border-t tw-border-white tw-text-white tw-mt-5"
    >
      <div
        class="
          tw-max-w-7xl
          tw-mx-auto
          tw-flex
          tw-justify-center
          tw-items-center
          tw-h-16
          tw-px-3
          lg:tw-px-0
        "
      >
        <div class="tw-text-2xl"><i class="fas fa-film"></i> MovieFlix</div>
      </div>
    </q-footer>
  </q-layout>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { useAuthStore } from 'src/store/Auth';
import { useRouter } from 'vue-router';
import config from 'src/config';
import Search from '../components/Search.vue';

const leftDrawerOpen = ref<boolean>(false);
const authStore = useAuthStore();
const router = useRouter();
const loggedIn = computed(() => {
  return authStore.getLoggedIn;
});

const roles = computed(() => {
  return authStore.getRoles;
});

const user = computed(() => {
  return authStore.getUser;
});

async function logout() {
  authStore.logout();
  await router.push('/');
}
</script>

<style scoped>
.active-class {
  color: red;
}
</style>
