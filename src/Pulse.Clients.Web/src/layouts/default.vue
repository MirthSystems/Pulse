<template>
  <v-app id="inspire">
    <AppBar v-model:drawer="drawer" :routes="routes" />

    <v-navigation-drawer v-if="isAuthenticated" v-model="drawer" temporary>
      <v-list>
        <v-list-item v-for="route in routes" :key="route.title" link :to="route.to">
          <v-list-item-title>{{ route.title }}</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <router-view />
    </v-main>

    <AppFooter />
  </v-app>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { useAuth0 } from '@auth0/auth0-vue';
  import AppBar from '@/components/AppBar.vue';
  import AppFooter from '@/components/AppFooter.vue';

  const drawer = ref(false);
  const auth0 = useAuth0();

  const isAuthenticated = auth0.isAuthenticated;

  const routes = [
    {
      title: 'Home',
      to: '/',
    },
    {
      title: 'Administration',
      to: '/management',
    },
    {
      title: 'Settings',
      to: '/settings',
    },
  ];
</script>
