<template>
  <v-dialog max-width="600px" :model-value="modelValue" @update:model-value="emit('update:model-value', $event)">
    <v-card>
      <v-card-title>
        <span class="text-h5">{{ special ? 'Edit Special' : 'Create Special' }}</span>
      </v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-text-field
            v-model="form.content"
            label="Content"
            required
            :rules="[rules.required]"
          />
          <v-select
            v-model="form.type"
            :items="specialTypes"
            label="Type"
            required
            :rules="[rules.required]"
          />
          <v-text-field
            v-model="form.startDate"
            label="Start Date"
            required
            :rules="[rules.required]"
          />
          <v-text-field
            v-model="form.startTime"
            label="Start Time"
            required
            :rules="[rules.required]"
          />
          <v-text-field
            v-model="form.endTime"
            label="End Time"
          />
          <v-text-field
            v-model="form.expirationDate"
            label="Expiration Date"
          />
          <v-checkbox
            v-model="form.isRecurring"
            label="Is Recurring"
          />
          <v-text-field
            v-if="form.isRecurring"
            v-model="form.recurringPeriod"
            label="Recurring Period"
          />
          <v-text-field
            v-model="form.activeDaysOfWeek"
            label="Active Days of Week"
          />
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" @click="saveSpecial">Save</v-btn>
        <v-btn color="secondary" @click="closeDialog">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
  import { defineEmits, defineProps, ref, watch } from 'vue';
  import { AdminClient } from '@/api';
  import type { NewSpecialRequest, SpecialItem, UpdateSpecialRequest } from '@/models';
  import { SpecialTypes } from '@/enums';

  const props = defineProps<{
    modelValue: boolean;
    special: SpecialItem | null;
    venueId: number;
  }>();

  const emit = defineEmits<{
    (e: 'update:model-value', value: boolean): void;
    (e: 'save'): void;
  }>();

  const form = ref<NewSpecialRequest | UpdateSpecialRequest>({
    content: '',
    type: SpecialTypes.Food,
    startDate: '',
    startTime: '',
    endTime: '',
    expirationDate: '',
    isRecurring: false,
    recurringPeriod: undefined,
    activeDaysOfWeek: undefined,
    venueId: props.venueId,
    tagIds: [],
  });

  const valid = ref(false);
  const rules = {
    required: (value: string) => !!value || 'Required.',
  };

  const specialTypes = Object.values(SpecialTypes);

  watch(() => props.special, newSpecial => {
    if (newSpecial) {
      form.value = { ...newSpecial, tagIds: (newSpecial as { tagIds?: number[] }).tagIds || [] };
    } else {
      form.value = {
        content: '',
        type: SpecialTypes.Food,
        startDate: '',
        startTime: '',
        endTime: '',
        expirationDate: '',
        isRecurring: false,
        recurringPeriod: undefined,
        activeDaysOfWeek: undefined,
        venueId: props.venueId,
        tagIds: [],
      };
    }
  });

  function closeDialog () {
    emit('update:model-value', false);
  }

  async function saveSpecial () {
    if (valid.value) {
      try {
        if (props.special) {
          await AdminClient.updateSpecial(props.special.id, form.value as UpdateSpecialRequest);
        } else {
          await AdminClient.createSpecial(form.value as NewSpecialRequest);
        }
        emit('save');
        closeDialog();
      } catch (error) {
        console.error('Failed to save special:', error);
      }
    }
  }
</script>
