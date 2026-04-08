<template>
  <teleport to="body">
    <div class="fixed top-4 right-4 z-[100] space-y-2 w-[340px] max-w-[90vw]">
      <transition-group name="toast">
        <div
          v-for="t in toasts"
          :key="t.id"
          :class="[
            'flex items-start p-3.5 rounded-xl shadow-lg border text-sm font-medium backdrop-blur-sm',
            classes[t.type],
          ]"
        >
          <CheckCircleIcon
            v-if="t.type === 'success'"
            class="h-5 w-5 mr-2.5 flex-shrink-0 mt-0.5"
            stroke-width="2"
          />
          <AlertCircleIcon
            v-else-if="t.type === 'error'"
            class="h-5 w-5 mr-2.5 flex-shrink-0 mt-0.5"
            stroke-width="2"
          />
          <InfoIcon
            v-else-if="t.type === 'info'"
            class="h-5 w-5 mr-2.5 flex-shrink-0 mt-0.5"
            stroke-width="2"
          />
          <AlertTriangleIcon
            v-else
            class="h-5 w-5 mr-2.5 flex-shrink-0 mt-0.5"
            stroke-width="2"
          />
          <span class="flex-1 leading-snug">{{ t.message }}</span>
        </div>
      </transition-group>
    </div>
  </teleport>
</template>

<script setup lang="ts">
import { useToast } from "../../composables/useToast";
import {
  CheckCircle2 as CheckCircleIcon,
  AlertCircle as AlertCircleIcon,
  Info as InfoIcon,
  AlertTriangle as AlertTriangleIcon,
} from "lucide-vue-next";

const { toasts } = useToast();

const classes: Record<string, string> = {
  success: "bg-emerald-50/95 text-emerald-800 border-emerald-200",
  error: "bg-red-50/95 text-red-800 border-red-200",
  info: "bg-blue-50/95 text-blue-800 border-blue-200",
  warning: "bg-amber-50/95 text-amber-800 border-amber-200",
};
</script>

<style scoped>
.toast-enter-active {
  transition: all 0.3s ease-out;
}
.toast-leave-active {
  transition: all 0.25s ease-in;
}
.toast-enter-from {
  opacity: 0;
  transform: translateX(60px);
}
.toast-leave-to {
  opacity: 0;
  transform: translateX(60px);
}
</style>
