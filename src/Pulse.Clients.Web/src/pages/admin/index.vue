<template>
  <v-container>
    <v-row>
      <v-col>
        <h1 class="text-h4 mb-4">Administration</h1>
      </v-col>
    </v-row>

    <div v-if="loading && !venues.length" class="d-flex justify-center align-center" style="height: 400px;">
      <v-progress-circular color="primary" indeterminate />
    </div>

    <v-alert v-else-if="error" class="mb-4" type="error">
      {{ error }}
    </v-alert>

    <template v-else>
      <v-row>
        <v-col cols="12">
          <v-data-table :headers="headers" :items="venues" :loading="loading">
            <template #[`item.actions`]="{ item }">
              <v-btn color="primary" @click="manageVenue(item.id)">Manage</v-btn>
              <v-btn color="secondary" @click="editVenue(item.id)">Edit</v-btn>
              <v-btn color="error" @click="deleteVenue(item)">Delete</v-btn>
              <v-btn color="info" @click="viewSpecials(item.id)">View Specials</v-btn>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12">
          <v-btn color="primary" @click="openCreateVenueDialog">Create New Venue</v-btn>
        </v-col>
      </v-row>
    </template>
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

  const headers = [
    { text: 'Name', value: 'name' },
    { text: 'Address', value: 'addressLine1' },
    { text: 'Actions', value: 'actions', sortable: false },
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

  function manageVenue (id: number) {
    router.push(`/admin/${id}`);
  }

  function openCreateVenueDialog () {
    // Logic to open the create new venue dialog
  }

  function editVenue (id: number) {
    // Logic to open the edit venue dialog
  }

  function deleteVenue (venue: VenueItem) {
    // Logic to open the delete venue dialog
  }

  function viewSpecials (id: number) {
    // Logic to open the view specials dialog
  }
</script>
