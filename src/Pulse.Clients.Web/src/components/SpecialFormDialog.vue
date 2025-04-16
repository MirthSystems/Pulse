<template>
  <v-dialog v-model="dialog" max-width="600px">
    <v-card>
      <v-card-title>
        <span class="text-h5">{{ isEdit ? 'Edit Special' : 'Add Special' }}</span>
      </v-card-title>

      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-text-field
            v-model="special.content"
            label="Content"
            required
            :rules="[rules.required]"
          />
          <v-select
            v-model="special.type"
            :items="specialTypes"
            label="Type"
            required
            :rules="[rules.required]"
          />
          <v-text-field
            v-model="special.startDate"
            label="Start Date"
            required
            :rules="[rules.required]"
            type="date"
          />
          <v-text-field
            v-model="special.startTime"
            label="Start Time"
            required
            :rules="[rules.required]"
            type="time"
          />
          <v-text-field v-model="special.endTime" label="End Time" type="time" />
          <v-text-field v-model="special.expirationDate" label="Expiration Date" type="date" />
          <v-switch v-model="special.isRecurring" label="Is Recurring" />
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
  import { ref, watch } from 'vue';
  import { AdminClient } from '@/api';
  import type { NewSpecialRequest, UpdateSpecialRequest } from '@/models';
  import { SpecialTypes } from '@/enums';

  const dialog = ref(false);
  const valid = ref(false);
  const form = ref();
  const isEdit = ref(false);
  const special = ref<NewSpecialRequest | (UpdateSpecialRequest & { id?: number })>({
    content: '',
    type: SpecialTypes.Food,
    startDate: '',
    startTime: '',
    isRecurring: false,
    venueId: 0,
    id: undefined,
  });
  const specialTypes = [
    { text: 'Food', value: SpecialTypes.Food },
    { text: 'Drink', value: SpecialTypes.Drink },
    { text: 'Entertainment', value: SpecialTypes.Entertainment },
  ];

  const rules = {
    required: (value: string) => !!value || 'Required.',
  };

  watch(dialog, newVal => {
    if (!newVal) {
      form.value?.reset();
      special.value = {
        content: '',
        type: SpecialTypes.Food,
        startDate: '',
        startTime: '',
        isRecurring: false,
        venueId: 0,
      };
      isEdit.value = false;
    }
  });

  function save () {
    if (form.value?.validate()) {
      const action = isEdit.value && 'id' in special.value && special.value.id !== undefined
        ? AdminClient.updateSpecial(special.value.id, special.value as UpdateSpecialRequest)
        : AdminClient.createSpecial(special.value as NewSpecialRequest);

      action
        .then(() => {
          close();
        })
        .catch(err => {
          console.error('Failed to save special:', err);
        });
    }
  }

  function close () {
    dialog.value = false;
  }
</script>
