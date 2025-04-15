<template>
  <div>
    <hero />
    <hr />
    <home-content />
  </div>
</template>

<script setup lang="ts">
import Hero from "../components/Hero.vue";
import HomeContent from "../components/HomeContent.vue";
</script>

<style lang="css" scoped>
.next-steps .fa-link {
  margin-right: 5px;
}
</style>
