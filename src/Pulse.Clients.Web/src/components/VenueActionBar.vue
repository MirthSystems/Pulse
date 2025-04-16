<template>
  <v-btn class="mr-2" color="secondary" prepend-icon="mdi-refresh" @click="$emit('refresh')">
    Refresh
  </v-btn>
  <v-btn color="primary" prepend-icon="mdi-plus" @click="$emit('add')">
    Add Venue
  </v-btn>
</template>

<script lang="ts" setup>
  defineEmits<{
    (e: 'refresh'): void;
    (e: 'add'): void;
  }>();
</script>
