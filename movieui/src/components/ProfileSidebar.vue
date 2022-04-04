<template>
  <q-list bordered seperator class="tw-text-white tw-border-white q-pa-sm">
    <q-item clickable v-ripple to="/profile" exact-active-class="active-class">
      <q-item-section avatar>
        <q-icon name="fas fa-user" />
      </q-item-section>
      <q-item-section>Profile</q-item-section>
    </q-item>
    <q-separator color="white" spaced />
    <q-item
      clickable
      v-ripple
      to="/changepassword"
      exact-active-class="active-class"
    >
      <q-item-section avatar>
        <q-icon name="fas fa-key" />
      </q-item-section>
      <q-item-section>Change Password</q-item-section>
    </q-item>
    <q-separator color="white" spaced />
    <q-item
      clickable
      v-ripple
      to="/changeimage"
      exact-active-class="active-class"
    >
      <q-item-section avatar>
        <q-icon name="far fa-image" />
      </q-item-section>
      <q-item-section>Change Image</q-item-section>
    </q-item>
  </q-list>
</template>

<script setup lang="ts"></script>

<style scoped>
.active-class {
  color: yellow;
}
</style>
