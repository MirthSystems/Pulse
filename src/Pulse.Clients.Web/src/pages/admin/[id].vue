<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="4">
        <VenueDetailsCard v-if="venue" :venue="venue" @update="loadVenueDetails" />
      </v-col>
      <v-col cols="12" md="8">
        <VenueSpecialsTable :specials="venue?.specials || []" @create="openCreateSpecialDialog" @delete="openDeleteSpecialDialog" @edit="openEditSpecialDialog" />
      </v-col>
    </v-row>

    <VenueSpecialForm v-model="specialFormVisible" :special="selectedSpecial" :venue-id="venueId" @save="loadVenueDetails" />
    <SpecialConfirmDeleteDialog v-model="deleteSpecialDialogVisible" :special="selectedSpecial" @confirm="deleteSpecial" />
  </v-container>
</template>

<script lang="ts" setup>
  import { onMounted, ref } from 'vue';
  import { useRoute } from 'vue-router';
  import { AdminClient } from '@/api';
  import type { SpecialItem, VenueWithDetails } from '@/models';

  const route = useRoute();
  const venueId = 'id' in route.params ? Number(route.params.id) : 0;

  const venue = ref<VenueWithDetails | null>(null);
  const specialFormVisible = ref(false);
  const deleteSpecialDialogVisible = ref(false);
  const selectedSpecial = ref<SpecialItem | null>(null);

  onMounted(async () => {
    await loadVenueDetails();
  });

  async function loadVenueDetails () {
    try {
      venue.value = await AdminClient.getVenue(venueId);
    } catch (error) {
      console.error('Failed to load venue details:', error);
    }
  }

  function openCreateSpecialDialog () {
    selectedSpecial.value = null;
    specialFormVisible.value = true;
  }

  function openEditSpecialDialog (special: SpecialItem) {
    selectedSpecial.value = special;
    specialFormVisible.value = true;
  }

  function openDeleteSpecialDialog (special: SpecialItem) {
    selectedSpecial.value = special;
    deleteSpecialDialogVisible.value = true;
  }

  async function deleteSpecial () {
    if (selectedSpecial.value) {
      try {
        await AdminClient.deleteSpecial(selectedSpecial.value.id);
        await loadVenueDetails();
      } catch (error) {
        console.error('Failed to delete special:', error);
      }
    }
  }
</script>
