<template>
  <v-dialog v-model="dialog" max-width="600px">
    <v-card>
      <v-card-title>
        <span class="text-h5">Create New Venue</span>
      </v-card-title>

      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-text-field
            v-model="venue.name"
            label="Name"
            required
            :rules="[rules.required]"
          />
          <v-text-field
            v-model="venue.addressLine1"
            label="Address Line 1"
            required
            :rules="[rules.required]"
          />
          <v-text-field v-model="venue.addressLine2" label="Address Line 2" />
          <v-text-field v-model="venue.locality" label="City" required :rules="[rules.required]" />
          <v-text-field v-model="venue.region" label="State/Region" required :rules="[rules.required]" />
          <v-text-field v-model="venue.postcode" label="Postcode" required :rules="[rules.required]" />
          <v-text-field v-model="venue.country" label="Country" required :rules="[rules.required]" />
          <v-text-field v-model="venue.phoneNumber" label="Phone Number" />
          <v-text-field v-model="venue.email" label="Email" type="email" />
          <v-text-field v-model="venue.website" label="Website" type="url" />
          <v-select
            v-model="venue.venueTypeId"
            item-text="name"
            item-value="id"
            :items="venueTypes"
            label="Venue Type"
            required
            :rules="[rules.required]"
          />
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" :disabled="!valid" @click="save">Save</v-btn>
        <v-btn text @click="close">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
  import { onMounted, ref } from 'vue';
  import { AdminClient } from '@/api';
  import type { VenueRequest, VenueTypeItem } from '@/models';

  const dialog = ref(false);
  const valid = ref(false);
  const form = ref();
  const venue = ref<VenueRequest>({
    name: '',
    addressLine1: '',
    locality: '',
    region: '',
    postcode: '',
    country: '',
    venueTypeId: 0,
  });
  const venueTypes = ref<VenueTypeItem[]>([]);

  const rules = {
    required: (value: string) => !!value || 'Required.',
  };

  onMounted(async () => {
    venueTypes.value = await AdminClient.getVenueTypes();
  });

  function save () {
    if (form.value?.validate()) {
      AdminClient.createVenue(venue.value)
        .then(() => {
          close();
        })
        .catch(err => {
          console.error('Failed to create venue:', err);
        });
    }
  }

  function close () {
    dialog.value = false;
    form.value?.reset();
  }
</script>
