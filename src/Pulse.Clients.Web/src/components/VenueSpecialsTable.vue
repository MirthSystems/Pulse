<template>
  <v-card>
    <v-card-title>
      <span class="text-h5">Specials</span>
      <v-spacer />
      <v-btn color="primary" @click="$emit('create')">Create Special</v-btn>
    </v-card-title>
    <v-data-table
      class="elevation-1"
      :headers="headers"
      :items="specials"
    >
      <template #[`item.actions`]="{ item }">
        <v-btn icon size="small" variant="text" @click="$emit('edit', item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
        <v-btn icon size="small" variant="text" @click="$emit('delete', item)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script lang="ts" setup>
  import type { SpecialItem } from '@/models';

  defineProps<{
    specials: SpecialItem[];
  }>();

  defineEmits<{
    (e: 'create'): void;
    (e: 'edit', special: SpecialItem): void;
    (e: 'delete', special: SpecialItem): void;
  }>();

  const headers = [
    { text: 'Content', value: 'content' },
    { text: 'Type', value: 'type' },
    { text: 'Start Date', value: 'startDate' },
    { text: 'Start Time', value: 'startTime' },
    { text: 'End Time', value: 'endTime' },
    { text: 'Actions', value: 'actions', sortable: false },
  ];
</script>
