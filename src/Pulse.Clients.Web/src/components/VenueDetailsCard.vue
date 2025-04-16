<template>
  <v-card>
    <v-card-title>
      <span class="text-h5">Venue Details</span>
      <v-spacer />
      <v-btn icon @click="editMode = !editMode">
        <v-icon>{{ editMode ? 'mdi-check' : 'mdi-wrench' }}</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text>
      <v-form ref="form" v-model="valid">
        <v-text-field
          v-model="venueData.name"
          label="Name"
          :readonly="!editMode"
          :rules="[rules.required]"
        />
        <v-text-field
          v-model="venueData.description"
          label="Description"
          :readonly="!editMode"
        />
        <v-text-field
          v-model="venueData.addressLine1"
          label="Address Line 1"
          :readonly="!editMode"
          :rules="[rules.required]"
        />
        <v-text-field
          v-model="venueData.addressLine2"
          label="Address Line 2"
          :readonly="!editMode"
        />
        <v-text-field
          v-model="venueData.locality"
          label="City"
          :readonly="!editMode"
          :rules="[rules.required]"
        />
        <v-text-field
          v-model="venueData.region"
          label="State"
          :readonly="!editMode"
          :rules="[rules.required]"
        />
        <v-text-field
          v-model="venueData.postcode"
          label="Postcode"
          :readonly="!editMode"
          :rules="[rules.required]"
        />
        <v-text-field
          v-model="venueData.country"
          label="Country"
          :readonly="!editMode"
          :rules="[rules.required]"
        />
        <v-text-field
          v-model="venueData.phoneNumber"
          label="Phone Number"
          :readonly="!editMode"
        />
        <v-text-field
          v-model="venueData.email"
          label="Email"
          :readonly="!editMode"
        />
        <v-text-field
          v-model="venueData.website"
          label="Website"
          :readonly="!editMode"
        />
        <v-text-field
          v-model="venueData.imageLink"
          label="Image Link"
          :readonly="!editMode"
        />
        <v-select
          v-model="venueData.venueTypeId"
          :items="venueTypes"
          label="Venue Type"
          :readonly="!editMode"
          :rules="[rules.required]"
        />
        <v-divider />
        <v-card-subtitle>Operating Schedule</v-card-subtitle>
        <v-row v-for="(schedule, index) in venueData.businessHours" :key="index">
          <v-col cols="4">
            <v-select
              v-model="schedule.dayOfWeek"
              item-text="text"
              item-value="value"
              :items="daysOfWeek"
              label="Day of Week"
              :readonly="!editMode"
              :rules="[rules.required]"
            />
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="schedule.timeOfOpen"
              label="Open Time"
              :readonly="!editMode"
              :rules="[rules.required]"
            />
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="schedule.timeOfClose"
              label="Close Time"
              :readonly="!editMode"
              :rules="[rules.required]"
            />
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn v-if="editMode" color="primary" @click="saveChanges">Save</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts" setup>
  import { ref, watch } from 'vue';
  import { AdminClient } from '@/api';
  import type { VenueTypeItem, VenueWithDetails } from '@/models';
  import { DayOfWeek } from '@/enums';

  const props = defineProps<{
    venue: VenueWithDetails;
  }>();

  const emit = defineEmits(['update']);

  const editMode = ref(false);
  const valid = ref(false);

  interface FormInstance {
    validate: () => boolean;
  }

  const form = ref<FormInstance | null>(null);
  const daysOfWeek = Object.keys(DayOfWeek)
    .filter(key => isNaN(Number(key)))
    .map(key => ({
      text: key,
      value: DayOfWeek[key as keyof typeof DayOfWeek],
    }));
  const venueTypes = ref<VenueTypeItem[]>([]);

  const rules = {
    required: (value: string) => !!value || 'Required.',
  };

  const venueData = ref({ ...props.venue });

  watch(editMode, async newValue => {
    if (newValue) {
      try {
        venueTypes.value = await AdminClient.getVenueTypes();
      } catch (error) {
        console.error('Failed to load venue types:', error);
      }
    }
  });

  async function saveChanges () {
    if (form.value && form.value.validate()) {
      try {
        await AdminClient.updateVenue(props.venue.id, venueData.value);
        await AdminClient.updateVenueSchedules(props.venue.id, venueData.value.businessHours);
        emit('update');
        editMode.value = false;
      } catch (error) {
        console.error('Failed to save changes:', error);
      }
    }
  }
</script>
