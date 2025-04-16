<template>
  <v-container>
    <v-row>
      <v-col cols="3">
        <v-card>
          <v-card-title>Venue Details</v-card-title>
          <v-card-text>
            <p><strong>Name:</strong> {{ venue?.name }}</p>
            <p><strong>Address:</strong> {{ venue?.addressLine1 }}</p>
            <p><strong>City:</strong> {{ venue?.locality }}</p>
            <p><strong>State:</strong> {{ venue?.region }}</p>
            <p><strong>Postcode:</strong> {{ venue?.postcode }}</p>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="9">
        <v-card>
          <v-card-title>Specials</v-card-title>
          <v-card-text>
            <v-data-table :headers="specialHeaders" inline-edit :items="venue?.specials">
              <template #[`item.actions`]="{ item }">
                <v-btn icon @click="deleteSpecial(item.id)">
                  <v-icon>mdi-delete</v-icon>
                </v-btn>
              </template>
            </v-data-table>
            <v-btn color="primary" @click="addSpecial">Add Special</v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
  import { onMounted, ref } from 'vue';
  import { useRoute } from 'vue-router';
  import { AdminClient } from '@/api';
  import type { VenueWithDetails } from '@/models';

  const route = useRoute();
  const venueId = 'id' in route.params ? Number(route.params.id) : 0;
  const venue = ref<VenueWithDetails | null>(null);
  const specialHeaders = [
    { text: 'Content', value: 'content' },
    { text: 'Type', value: 'type' },
    { text: 'Start Date', value: 'startDate' },
    { text: 'Actions', value: 'actions', sortable: false },
  ];

  onMounted(async () => {
    await loadVenue();
  });

  async function loadVenue () {
    try {
      venue.value = await AdminClient.getVenue(venueId);
    } catch (err) {
      console.error('Failed to load venue:', err);
    }
  }

  function addSpecial () {
    console.log('Add special logic here');
  }

  function deleteSpecial (id: number) {
    console.log('Delete special logic here', id);
  }
</script>
