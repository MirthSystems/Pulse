<template>
  <v-app-bar>
    <v-app-bar-nav-icon v-if="authStore.isAuthenticated" @click="emit('update:drawer', !props.drawer)" />

    <v-app-bar-title>Pulse</v-app-bar-title>

    <v-spacer />

    <template v-if="!authStore.isAuthenticated && !authStore.isLoading">
      <v-btn color="primary" @click="authStore.login">Admin</v-btn>
    </template>

    <template v-else>
      <v-menu offset-y>
        <template #activator="{ props: menuProps }">
          <v-btn v-bind="menuProps" icon>
            <v-avatar size="32">
              <img alt="User's profile picture" :src="authStore.user?.picture">
            </v-avatar>
          </v-btn>
        </template>
        <v-list>
          <v-list-item>
            <v-list-item-title>{{ authStore.user?.name }}</v-list-item-title>
          </v-list-item>
          <v-divider />
          <v-list-item @click="$router.push('/profile')">
            <v-list-item-title>Profile</v-list-item-title>
          </v-list-item>
          <v-list-item @click="authStore.logout">
            <v-list-item-title>Log out</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>
  </v-app-bar>
</template>

<script lang="ts" setup>
  import { useAuthStore } from '@/stores/auth';

  const props = defineProps({
    drawer: Boolean,
  });

  const emit = defineEmits(['update:drawer']);

  const authStore = useAuthStore();
</script>
