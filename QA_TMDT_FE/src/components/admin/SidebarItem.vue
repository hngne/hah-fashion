<template>
  <RouterLink
    :to="to"
    :class="[
      'flex items-center rounded-lg transition-colors duration-150 group',
      collapsed ? 'justify-center p-3' : 'px-3 py-2.5',
      active
        ? 'bg-blue-600/10 text-blue-400'
        : 'text-slate-400 hover:text-white hover:bg-slate-700/50',
    ]"
    :title="collapsed ? label : undefined"
  >
    <component
      :is="icon"
      :class="['flex-shrink-0', collapsed ? 'h-5 w-5' : 'h-[18px] w-[18px]']"
      stroke-width="1.5"
    />
    <transition name="fade">
      <span
        v-if="!collapsed"
        class="ml-3 text-[13px] font-semibold whitespace-nowrap"
        >{{ label }}</span
      >
    </transition>
  </RouterLink>
</template>

<script setup lang="ts">
import { RouterLink } from "vue-router";
import type { Component } from "vue";

defineProps<{
  icon: Component;
  label: string;
  to: string;
  collapsed: boolean;
  active: boolean;
}>();
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
