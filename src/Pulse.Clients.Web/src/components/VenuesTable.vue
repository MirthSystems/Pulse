<template>
  <v-card>
    <v-data-table
      class="elevation-1"
      :headers="headers"
      :items="venues"
      :loading="loading"
      :search="search"
    >
      <template #[`item.address`]="{ item }">
        {{ formatAddress(item) }}
      </template>

      <template #[`item.actions`]="{ item }">
        <v-btn icon size="small" variant="text" @click="$emit('view', item.id)">
          <v-icon>mdi-eye</v-icon>
        </v-btn>
        <v-btn icon size="small" variant="text" @click="$emit('edit', item.id)">
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
  import type { VenueItem } from '@/models';

  defineProps<{
    venues: VenueItem[];
    loading: boolean;
    search: string;
  }>();

  defineEmits<{
    (e: 'view', id: number): void;
    (e: 'edit', id: number): void;
    (e: 'delete', venue: VenueItem): void;
  }>();

  const headers = [
    { title: 'Venue Name', key: 'name' },
    { title: 'Address', key: 'address' },
    { title: 'City', key: 'locality' },
    { title: 'State', key: 'region' },
    { title: 'Type', key: 'venueTypeName' },
    { title: 'Actions', key: 'actions', sortable: false },
  ];

  function formatAddress (venue: VenueItem): string {
    return venue.addressLine1 + (venue.addressLine2 ? `, ${venue.addressLine2}` : '');
  }
</script>
