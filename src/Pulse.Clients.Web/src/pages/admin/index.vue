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
      <v-row class="mb-4">
        <v-col class="d-flex align-center" cols="12" sm="6">
          <VenueSearchBar v-model="search" />
        </v-col>
        <v-col class="d-flex justify-end" cols="12" sm="6">
          <VenueActionBar @add="addVenue" @refresh="loadVenues" />
        </v-col>
      </v-row>

      <VenuesTable
        :loading="loading"
        :search="search"
        :venues="venues"
        @delete="confirmDelete"
        @edit="editVenue"
        @view="viewVenue"
      />
    </template>

    <VenueConfirmDeleteDialog
      v-model="deleteDialog"
      :venue="venueToDelete"
      @confirm="deleteVenue"
    />
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

  function addVenue () {
    router.push('/venues/create');
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
