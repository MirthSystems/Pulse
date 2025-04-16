<template>
  <v-container>
    <v-row align="center" class="profile-header">
      <v-col class="mb-3" cols="12" md="2">
        <v-avatar size="150">
          <img alt="User's profile picture" :src="user?.picture">
        </v-avatar>
      </v-col>
      <v-col class="text-center text-md-left" cols="12" md="10">
        <h2>{{ user?.name }}</h2>
        <p class="text-muted">{{ user?.email }}</p>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <v-card>
          <v-card-text>
            <pre>{{ JSON.stringify(user, null, 2) }}</pre>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
  import { useAuth0 } from '@auth0/auth0-vue';

  const { user } = useAuth0();
</script>

<style scoped>
.profile-header {
  margin-bottom: 20px;
}
</style>
