<template>
  <v-container>
    <v-row>
      <v-col>
        <h1 class="text-h4 mb-4">Venue Details</h1>
      </v-col>
    </v-row>

    <div v-if="loading" class="d-flex justify-center align-center" style="height: 400px;">
      <v-progress-circular color="primary" indeterminate />
    </div>

    <v-alert v-else-if="error" class="mb-4" type="error">
      {{ error }}
    </v-alert>

    <template v-else-if="venue">
      <v-card class="mb-4">
        <v-card-title>
          <span class="text-h5">Venue Information</span>
          <v-spacer />
          <v-btn color="primary" @click="editVenue">Edit</v-btn>
        </v-card-title>
        <v-card-text>
          <p><strong>Name:</strong> {{ venue.name }}</p>
          <p><strong>Address:</strong> {{ venue.addressLine1 }}, {{ venue.locality }}, {{ venue.region }}, {{ venue.postcode }}, {{ venue.country }}</p>
          <p><strong>Phone:</strong> {{ venue.phoneNumber }}</p>
          <p><strong>Email:</strong> {{ venue.email }}</p>
          <p><strong>Website:</strong> <a :href="venue.website" target="_blank">{{ venue.website }}</a></p>
        </v-card-text>
      </v-card>

      <v-card class="mb-4">
        <v-card-title>
          <span class="text-h5">Operating Schedule</span>
        </v-card-title>
        <v-card-text>
          <v-data-table :headers="scheduleHeaders" :items="venue.businessHours" />
        </v-card-text>
      </v-card>

      <v-card>
        <v-card-title>
          <span class="text-h5">Specials</span>
          <v-spacer />
          <v-btn color="primary" @click="addSpecial">Add Special</v-btn>
        </v-card-title>
        <v-card-text>
          <v-data-table :headers="specialHeaders" :items="venue.specials">
            <template #[`item.actions`]="{ item }">
              <v-btn icon @click="editSpecial(item)">
                <v-icon>mdi-pencil</v-icon>
              </v-btn>
              <v-btn icon @click="deleteSpecial(item)">
                <v-icon>mdi-delete</v-icon>
              </v-btn>
            </template>
          </v-data-table>
        </v-card-text>
      </v-card>
    </template>

    <VenueFormDialog ref="venueFormDialog" />
    <SpecialFormDialog ref="specialFormDialog" />
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
  const loading = ref(true);
  const error = ref<string | null>(null);
  interface SpecialFormDialogInstance {
    dialog: boolean;
    special: SpecialItem;
  }

  interface VenueFormDialogInstance {
    dialog: boolean;
    venue: VenueWithDetails;
  }

  const venueFormDialog = ref<VenueFormDialogInstance | null>(null);
  const specialFormDialog = ref<SpecialFormDialogInstance | null>(null);

  const scheduleHeaders = [
    { text: 'Day', value: 'dayOfWeek' },
    { text: 'Open', value: 'timeOfOpen' },
    { text: 'Close', value: 'timeOfClose' },
    { text: 'Closed', value: 'isClosed' },
  ];

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
    loading.value = true;
    error.value = null;
    try {
      venue.value = await AdminClient.getVenue(venueId);
    } catch (err) {
      error.value = 'Failed to load venue details';
      console.error(err);
    } finally {
      loading.value = false;
    }
  }

  function addSpecial () {
    if (specialFormDialog.value) {
      specialFormDialog.value.dialog = true;
    }
  }

  function editSpecial (special: SpecialItem) {
    if (specialFormDialog.value) {
      specialFormDialog.value.dialog = true;
      specialFormDialog.value.special = special;
    }
  }

  function editVenue () {
    if (venueFormDialog.value && venue.value) {
      venueFormDialog.value.dialog = true;
      venueFormDialog.value.venue = venue.value;
    }
  }

  function deleteSpecial (special: SpecialItem) {
    AdminClient.deleteSpecial(special.id)
      .then(() => {
        venue.value!.specials = venue.value!.specials.filter(s => s.id !== special.id);
      })
      .catch(err => {
        console.error('Failed to delete special:', err);
      });
  }
</script>
