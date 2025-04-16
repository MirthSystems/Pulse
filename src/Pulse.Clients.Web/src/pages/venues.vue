<template>
  <v-container>
    <v-row>
      <v-col>
        <h1 class="text-h4 mb-4">Venue Management</h1>
      </v-col>
    </v-row>

    <div v-if="loading && !venues.length" class="d-flex justify-center align-center" style="height: 400px;">
      <v-progress-circular color="primary" indeterminate />
    </div>

    <v-alert v-else-if="error" class="mb-4" type="error">
      {{ error }}
    </v-alert>

    <template v-else>
      <v-row class="mb-4">
        <v-col class="d-flex align-center" cols="12" sm="6">
          <v-text-field
            v-model="search"
            density="compact"
            hide-details
            label="Search venues"
            prepend-inner-icon="mdi-magnify"
          />
        </v-col>
        <v-col class="d-flex justify-end" cols="12" sm="6">
          <v-btn class="mr-2" color="secondary" prepend-icon="mdi-refresh" @click="loadVenues">
            Refresh
          </v-btn>
          <v-btn color="primary" prepend-icon="mdi-plus">
            Add Venue
          </v-btn>
        </v-col>
      </v-row>

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
            <v-btn icon size="small" variant="text" @click="viewVenue(item.id)">
              <v-icon>mdi-eye</v-icon>
            </v-btn>
            <v-btn icon size="small" variant="text" @click="editVenue(item.id)">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon size="small" variant="text" @click="confirmDelete(item)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>
      </v-card>
    </template>

    <v-dialog v-model="deleteDialog" max-width="500px">
      <v-card>
        <v-card-title class="text-h5">Confirm Delete</v-card-title>
        <v-card-text>
          Are you sure you want to delete the venue "{{ venueToDelete?.name }}"? This action cannot be undone.
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="primary" variant="text" @click="deleteDialog = false">Cancel</v-btn>
          <v-btn color="error" variant="text" @click="deleteVenue">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script lang="ts" setup>
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import { AdminClient } from '@/api';
  import type { VenueItem } from '@/models';
  import { useAuthStore } from '@/stores/auth';

  const router = useRouter();
  const authStore = useAuthStore();

  const venues = ref<VenueItem[]>([]);
  const loading = ref(true);
  const error = ref<string | null>(null);
  const search = ref('');
  const deleteDialog = ref(false);
  const venueToDelete = ref<VenueItem | null>(null);

  const headers = [
    { title: 'Name', key: 'name' },
    { title: 'Address', key: 'address' },
    { title: 'City', key: 'locality' },
    { title: 'State', key: 'region' },
    { title: 'Type', key: 'venueTypeName' },
    { title: 'Actions', key: 'actions', sortable: false },
  ];

  onMounted(async () => {
    if (!authStore.isAuthenticated) {
      authStore.login();
      return;
    }

    await loadVenues();
  });

  async function loadVenues () {
    loading.value = true;
    error.value = null;

    try {
      venues.value = await AdminClient.getVenues();
    } catch (err) {
      console.error('Failed to load venues:', err);
      error.value = 'Failed to load venues. Please try again.';
    } finally {
      loading.value = false;
    }
  }

  function formatAddress (venue: VenueItem): string {
    return venue.addressLine1 + (venue.addressLine2 ? `, ${venue.addressLine2}` : '');
  }

  function viewVenue (id: number) {
    router.push(`/venues/${id}`);
  }

  function editVenue (id: number) {
    router.push(`/venues/${id}/edit`);
  }

  function confirmDelete (venue: VenueItem) {
    venueToDelete.value = venue;
    deleteDialog.value = true;
  }

  async function deleteVenue () {
    if (!venueToDelete.value) return;

    try {
      await AdminClient.deleteVenue(venueToDelete.value.id);
      venues.value = venues.value.filter(v => v.id !== venueToDelete.value?.id);
      deleteDialog.value = false;
      venueToDelete.value = null;
    } catch (err) {
      console.error('Failed to delete venue:', err);
      error.value = 'Failed to delete venue. Please try again.';
    }
  }
</script>
