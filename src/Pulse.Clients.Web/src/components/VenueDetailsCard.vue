<template>
  <v-card>
    <v-card-title>
      <span class="text-h5">Venue Details</span>
      <v-spacer></v-spacer>
      <v-btn icon @click="editMode = !editMode">
        <v-icon>{{ editMode ? 'mdi-check' : 'mdi-wrench' }}</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text>
      <v-form v-model="valid" ref="form">
        <v-text-field
          v-model="venue.name"
          :readonly="!editMode"
          label="Name"
          :rules="[rules.required]"
        ></v-text-field>
        <v-text-field
          v-model="venue.description"
          :readonly="!editMode"
          label="Description"
        ></v-text-field>
        <v-text-field
          v-model="venue.addressLine1"
          :readonly="!editMode"
          label="Address Line 1"
          :rules="[rules.required]"
        ></v-text-field>
        <v-text-field
          v-model="venue.addressLine2"
          :readonly="!editMode"
          label="Address Line 2"
        ></v-text-field>
        <v-text-field
          v-model="venue.locality"
          :readonly="!editMode"
          label="City"
          :rules="[rules.required]"
        ></v-text-field>
        <v-text-field
          v-model="venue.region"
          :readonly="!editMode"
          label="State"
          :rules="[rules.required]"
        ></v-text-field>
        <v-text-field
          v-model="venue.postcode"
          :readonly="!editMode"
          label="Postcode"
          :rules="[rules.required]"
        ></v-text-field>
        <v-text-field
          v-model="venue.country"
          :readonly="!editMode"
          label="Country"
          :rules="[rules.required]"
        ></v-text-field>
        <v-text-field
          v-model="venue.phoneNumber"
          :readonly="!editMode"
          label="Phone Number"
        ></v-text-field>
        <v-text-field
          v-model="venue.email"
          :readonly="!editMode"
          label="Email"
        ></v-text-field>
        <v-text-field
          v-model="venue.website"
          :readonly="!editMode"
          label="Website"
        ></v-text-field>
        <v-text-field
          v-model="venue.imageLink"
          :readonly="!editMode"
          label="Image Link"
        ></v-text-field>
        <v-select
          v-model="venue.venueTypeId"
          :items="venueTypes"
          :readonly="!editMode"
          label="Venue Type"
          :rules="[rules.required]"
        ></v-select>
        <v-divider></v-divider>
        <v-card-subtitle>Operating Schedule</v-card-subtitle>
        <v-row v-for="(schedule, index) in venue.businessHours" :key="index">
          <v-col cols="4">
            <v-select
              v-model="schedule.dayOfWeek"
              :items="daysOfWeek"
              :readonly="!editMode"
              label="Day of Week"
              :rules="[rules.required]"
            ></v-select>
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="schedule.timeOfOpen"
              :readonly="!editMode"
              label="Open Time"
              :rules="[rules.required]"
            ></v-text-field>
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="schedule.timeOfClose"
              :readonly="!editMode"
              label="Close Time"
              :rules="[rules.required]"
            ></v-text-field>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="primary" @click="saveChanges" v-if="editMode">Save</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts" setup>
  import { ref, watch } from 'vue';
  import { AdminClient } from '@/api';
  import type { VenueWithDetails, VenueTypeItem } from '@/models';
  import { DayOfWeek } from '@/enums';

  const props = defineProps<{
    venue: VenueWithDetails;
  }>();

  const emit = defineEmits(['update']);

  const editMode = ref(false);
  const valid = ref(false);
  const form = ref(null);

  const venueTypes = ref<VenueTypeItem[]>([]);
  const daysOfWeek = Object.values(DayOfWeek).filter(value => typeof value === 'string');

  const rules = {
    required: (value: string) => !!value || 'Required.',
  };

  watch(editMode, async (newValue) => {
    if (newValue) {
      try {
        venueTypes.value = await AdminClient.getVenueTypes();
      } catch (error) {
        console.error('Failed to load venue types:', error);
      }
    }
  });

  async function saveChanges() {
    if (form.value && (form.value as any).validate()) {
      try {
        await AdminClient.updateVenue(props.venue.id, props.venue);
        await AdminClient.updateVenueSchedules(props.venue.id, props.venue.businessHours);
        emit('update');
        editMode.value = false;
      } catch (error) {
        console.error('Failed to save changes:', error);
      }
    }
  }
</script>
