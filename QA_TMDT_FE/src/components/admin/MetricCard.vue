<template>
  <div class="bg-white rounded-xl border border-gray-200 p-5">
    <div class="flex items-center space-x-4">
      <div
        :class="[
          'w-11 h-11 rounded-xl flex items-center justify-center flex-shrink-0',
          iconBg,
        ]"
      >
        <component
          :is="icon"
          :class="['h-5 w-5', iconColor]"
          stroke-width="1.5"
        />
      </div>
      <div class="flex-1 min-w-0">
        <p
          class="text-[11px] font-bold text-gray-400 uppercase tracking-wider truncate"
        >
          {{ label }}
        </p>
        <p class="text-xl font-extrabold text-[#1F2937] mt-0.5">{{ value }}</p>
      </div>
    </div>
    <div class="mt-3 flex items-center">
      <span
        :class="[
          'text-[11px] font-bold px-1.5 py-0.5 rounded',
          changeType === 'up'
            ? 'bg-emerald-50 text-emerald-600'
            : 'bg-red-50 text-red-500',
        ]"
      >
        {{ change }}
      </span>
      <span class="text-[11px] text-gray-400 ml-1.5">so với tháng trước</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Component } from "vue";

defineProps<{
  icon: Component;
  label: string;
  value: string;
  change: string;
  changeType: "up" | "down";
  iconBg: string;
  iconColor: string;
}>();
</script>
