<template>
  <section
    class="relative w-full overflow-hidden"
    style="height: 85vh; min-height: 500px"
  >
    <!-- Slides -->
    <div
      v-for="(slide, idx) in slides"
      :key="idx"
      class="absolute inset-0 transition-opacity duration-700 ease-in-out"
      :class="idx === current ? 'opacity-100 z-10' : 'opacity-0 z-0'"
    >
      <!-- Background Image -->
      <div
        class="absolute inset-0 bg-cover bg-center"
        :style="{ backgroundImage: `url(${slide.image})` }"
      ></div>
      <!-- Overlay -->
      <div
        class="absolute inset-0 bg-gradient-to-r from-black/60 via-black/30 to-transparent"
      ></div>
      <!-- Content -->
      <div class="relative z-10 h-full flex items-center">
        <div class="max-w-[1400px] mx-auto px-6 lg:px-8 w-full">
          <span
            class="inline-block text-xs font-bold uppercase tracking-widest text-blue-400 mb-3"
            >{{ slide.tag }}</span
          >
          <h1
            class="text-4xl sm:text-5xl lg:text-7xl font-black text-white leading-tight max-w-xl"
          >
            {{ slide.title }}
          </h1>
          <p
            class="text-base sm:text-lg text-white/70 mt-4 max-w-md leading-relaxed"
          >
            {{ slide.subtitle }}
          </p>
          <div class="flex flex-wrap items-center gap-3 mt-8">
            <a
              href="#"
              class="inline-flex items-center px-6 py-3 bg-white text-[#111827] font-bold text-sm rounded-full hover:bg-gray-100 transition shadow-lg"
            >
              Khám phá ngay
              <ArrowRightIcon class="w-4 h-4 ml-2" />
            </a>
            <a
              href="#"
              class="inline-flex items-center px-6 py-3 bg-white/10 backdrop-blur-sm text-white font-semibold text-sm rounded-full border border-white/20 hover:bg-white/20 transition"
            >
              Xem Lookbook
            </a>
          </div>
        </div>
      </div>
    </div>

    <!-- Arrows -->
    <button
      @click="prev"
      class="absolute left-4 top-1/2 -translate-y-1/2 z-20 w-10 h-10 rounded-full bg-white/10 backdrop-blur-sm flex items-center justify-center text-white hover:bg-white/25 transition"
    >
      <ChevronLeftIcon class="w-5 h-5" />
    </button>
    <button
      @click="next"
      class="absolute right-4 top-1/2 -translate-y-1/2 z-20 w-10 h-10 rounded-full bg-white/10 backdrop-blur-sm flex items-center justify-center text-white hover:bg-white/25 transition"
    >
      <ChevronRightIcon class="w-5 h-5" />
    </button>

    <!-- Dots -->
    <div
      class="absolute bottom-6 left-1/2 -translate-x-1/2 z-20 flex space-x-2"
    >
      <button
        v-for="(_, idx) in slides"
        :key="idx"
        @click="current = idx"
        :class="[
          'w-2.5 h-2.5 rounded-full transition-all',
          idx === current ? 'bg-white w-7' : 'bg-white/40 hover:bg-white/60',
        ]"
      ></button>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from "vue";
import {
  ArrowRight as ArrowRightIcon,
  ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon,
} from "lucide-vue-next";

// Placeholder images — replace with real ones from assets/client/
const slides = [
  {
    tag: "New Arrival",
    title: "BỘ SƯU TẬP HÈ 2026",
    subtitle:
      "Phong cách năng động cho mùa hè rực rỡ. Khám phá những thiết kế mới nhất.",
    image:
      "https://images.unsplash.com/photo-1523381210434-271e8be1f52b?w=1920&q=80",
  },
  {
    tag: "Thu Đông",
    title: "BST THU ĐÔNG 2026",
    subtitle:
      "Sự thanh lịch và ấm áp cho mùa mới. Chất liệu cao cấp, form dáng hiện đại.",
    image:
      "https://images.unsplash.com/photo-1490481651871-ab68de25d43d?w=1920&q=80",
  },
  {
    tag: "Streetwear",
    title: "PHONG CÁCH ĐƯỜNG PHỐ",
    subtitle: "Tự do, cá tính, phá cách. Định nghĩa lại thời trang đường phố.",
    image:
      "https://images.unsplash.com/photo-1509631179647-0177331693ae?w=1920&q=80",
  },
];

const current = ref(0);
let timer: ReturnType<typeof setInterval>;

const next = () => {
  current.value = (current.value + 1) % slides.length;
};
const prev = () => {
  current.value = (current.value - 1 + slides.length) % slides.length;
};

onMounted(() => {
  timer = setInterval(next, 5000);
});
onUnmounted(() => {
  clearInterval(timer);
});
</script>
