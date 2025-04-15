<template>
  <div class="alert alert-danger alert-dismissible" v-if="msg">
    {{ msg }}
    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
      <span aria-hidden="true">&times;</span>
    </button>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useAuth0 } from "@auth0/auth0-vue";

const auth0 = useAuth0();
const msg = computed(() => auth0.error.value);
</script>
