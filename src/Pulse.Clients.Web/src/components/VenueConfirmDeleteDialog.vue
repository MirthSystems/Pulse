<template>
  <v-dialog max-width="500px" :model-value="modelValue" @update:model-value="$emit('update:modelValue', $event)">
    <v-card>
      <v-card-title class="text-h5">Confirm Delete</v-card-title>
      <v-card-text>
        Are you sure you want to delete the venue "{{ venue?.name }}"? This action cannot be undone.
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" variant="text" @click="$emit('update:modelValue', false)">Cancel</v-btn>
        <v-btn color="error" variant="text" @click="$emit('confirm')">Delete</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
  import type { VenueItem } from '@/models';

  defineProps<{
    modelValue: boolean;
    venue: VenueItem | null;
  }>();

  defineEmits<{
    (e: 'update:modelValue', value: boolean): void;
    (e: 'confirm'): void;
  }>();
</script>
