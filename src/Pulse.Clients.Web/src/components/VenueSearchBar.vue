<template>
  <v-text-field
    density="compact"
    hide-details
    label="Search venues"
    :model-value="modelValue"
    prepend-inner-icon="mdi-magnify"
    @update:model-value="$emit('update:modelValue', $event)"
  />
</template>

<script lang="ts" setup>
  defineProps<{
    modelValue: string;
  }>();

  defineEmits<{
    (e: 'update:modelValue', value: string): void;
  }>();
</script>
