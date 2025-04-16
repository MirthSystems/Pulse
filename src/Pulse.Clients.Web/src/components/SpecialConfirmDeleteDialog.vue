<template>
  <v-dialog max-width="500px" :model-value="modelValue" @update:model-value="$emit('update:model-value', $event)">
    <v-card>
      <v-card-title class="text-h5">Confirm Delete</v-card-title>
      <v-card-text>
        Are you sure you want to delete the special "{{ special?.content }}"? This action cannot be undone.
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" variant="text" @click="$emit('update:model-value', false)">Cancel</v-btn>
        <v-btn color="error" variant="text" @click="handleDelete">Delete</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
  import type { SpecialItem } from '@/models';
  import { AdminClient } from '@/api';

  defineProps<{
    modelValue: boolean;
    special: SpecialItem | null;
  }>();

  defineEmits<{
    (e: 'update:model-value', value: boolean): void;
    (e: 'confirm'): void;
  }>();

  async function handleDelete() {
    if (special) {
      try {
        await AdminClient.deleteSpecial(special.id);
        $emit('confirm');
      } catch (error) {
        console.error('Failed to delete special:', error);
      }
    }
  }
</script>
