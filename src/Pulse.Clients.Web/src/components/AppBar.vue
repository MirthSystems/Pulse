<template>
  <v-app-bar>
    <v-app-bar-nav-icon v-if="isAuthenticated" @click="emit('update:drawer', !props.drawer)" />

    <v-app-bar-title>Pulse</v-app-bar-title>

    <v-spacer />

    <template v-if="!isAuthenticated && !isLoading">
      <v-btn color="primary" @click="login">Admin</v-btn>
    </template>

    <template v-else>
      <v-menu offset-y>
        <template #activator="{ props: menuProps }">
          <v-btn v-bind="menuProps" icon>
            <v-avatar size="32">
              <img alt="User's profile picture" :src="user?.picture">
            </v-avatar>
          </v-btn>
        </template>
        <v-list>
          <v-list-item>
            <v-list-item-title>{{ user?.name }}</v-list-item-title>
          </v-list-item>
          <v-divider />
          <v-list-item @click="$router.push('/profile')">
            <v-list-item-title>Profile</v-list-item-title>
          </v-list-item>
          <v-list-item @click="logout">
            <v-list-item-title>Log out</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>
  </v-app-bar>
</template>

<script lang="ts" setup>
  import { useAuth0 } from '@auth0/auth0-vue';

  const props = defineProps({
    drawer: Boolean,
  });

  const emit = defineEmits(['update:drawer']);

  const auth0 = useAuth0();

  const isAuthenticated = auth0.isAuthenticated;
  const isLoading = auth0.isLoading;
  const user = auth0.user;

  function login () {
    auth0.loginWithRedirect();
  }

  function logout () {
    auth0.logout({
      logoutParams: {
        returnTo: window.location.origin,
      },
    });
  }
</script>
