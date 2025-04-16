<template>
  <v-container>
    <h1 class="text-h4 mb-6">Venue Management</h1>

    <div class="d-flex align-center mb-6">
      <v-text-field
        v-model="search"
        bg-color="transparent"
        class="rounded-lg flex-grow-1 mr-4"
        density="comfortable"
        hide-details
        placeholder="Search venues"
        prepend-inner-icon="mdi-magnify"
        single-line
      />
      <v-btn
        class="mr-2"
        color="primary"
        prepend-icon="mdi-refresh"
        @click="loadVenues"
      >
        REFRESH
      </v-btn>
      <v-btn
        color="primary"
        prepend-icon="mdi-plus"
        @click="openCreateVenueDialog"
      >
        ADD VENUE
      </v-btn>
    </div>

    <v-table
      class="rounded-lg"
    >
      <thead>
        <tr>
          <th>Name</th>
          <th>Address</th>
          <th>City</th>
          <th>State</th>
          <th>Type</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in venues" :key="item.id" class="venue-row">
          <td>{{ item.name }}</td>
          <td>{{ item.addressLine1 }}</td>
          <td>{{ item.locality }}</td>
          <td>{{ item.region }}</td>
          <td>{{ item.venueTypeName }}</td>
          <td>
            <div class="d-flex">
              <v-btn icon variant="text" @click="manageVenue(item.id)">
                <v-icon>mdi-pencil</v-icon>
              </v-btn>
              <v-btn color="error" icon variant="text" @click="openDeleteVenueDialog(item)">
                <v-icon>mdi-delete</v-icon>
              </v-btn>
            </div>
          </td>
        </tr>
      </tbody>
    </v-table>

    <div class="d-flex align-center justify-end mt-4">
      <span class="mr-4">Items per page: </span>
      <v-select
        v-model="itemsPerPage"
        class="items-per-page-select"
        density="compact"
        hide-details
        :items="[5, 10, 15, 20]"
        style="width: 80px"
        variant="outlined"
      />
      <span class="mx-4">1-{{ venues.length }} of {{ venues.length }}</span>
      <v-btn disabled icon variant="text">
        <v-icon>mdi-page-first</v-icon>
      </v-btn>
      <v-btn disabled icon variant="text">
        <v-icon>mdi-chevron-left</v-icon>
      </v-btn>
      <v-btn disabled icon variant="text">
        <v-icon>mdi-chevron-right</v-icon>
      </v-btn>
      <v-btn disabled icon variant="text">
        <v-icon>mdi-page-last</v-icon>
      </v-btn>
    </div>

    <VenueConfirmDeleteDialog
      v-model="deleteDialogVisible"
      :venue="selectedVenue"
      @confirm="deleteVenue"
    />
    <VenueFormDialog
      v-model="createDialogVisible"
      :venue="selectedVenue"
      @save="loadVenues"
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
  const deleteDialogVisible = ref(false);
  const createDialogVisible = ref(false);
  const selectedVenue = ref<VenueItem | null>(null);
  const search = ref('');
  const itemsPerPage = ref(10);

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

  function openCreateVenueDialog () {
    selectedVenue.value = null;
    createDialogVisible.value = true;
  }

  function manageVenue (id: number) {
    router.push(`/admin/${id}`);
  }

  function openDeleteVenueDialog (venue: VenueItem) {
    selectedVenue.value = venue;
    deleteDialogVisible.value = true;
  }

  async function deleteVenue () {
    if (selectedVenue.value) {
      try {
        await AdminClient.deleteVenue(selectedVenue.value.id);
        await loadVenues();
      } catch (error) {
        console.error('Failed to delete venue:', error);
      } finally {
        deleteDialogVisible.value = false;
      }
    }
  }

</script>

<style scoped>
.venue-row:hover {
  background-color: rgba(255, 255, 255, 0.05);
}

.items-per-page-select :deep(.v-field__input) {
  padding-top: 0;
  padding-bottom: 0;
  min-height: 32px;
}
</style>
