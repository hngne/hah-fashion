<template>
  <section class="py-16 px-4 sm:px-6 lg:px-8 max-w-[1400px] mx-auto">
    <h2
      class="text-2xl sm:text-3xl font-black text-[#111827] tracking-tight mb-8"
    >
      Khuyến mãi nổi bật
    </h2>
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4 sm:gap-6">
      <!-- Banner 1: Sale -->
      <RouterLink
        to="/sale"
        class="group relative overflow-hidden rounded-xl h-[280px] sm:h-[350px]"
      >
        <div
          class="absolute inset-0 bg-cover bg-center transition-transform duration-700 group-hover:scale-105"
          style="
            background-image: url(&quot;https://images.unsplash.com/photo-1469334031218-e382a71b716b?w=960&q=80&quot;);
          "
        ></div>
        <div
          class="absolute inset-0 bg-gradient-to-t from-black/60 via-black/20 to-transparent"
        ></div>
        <div class="absolute bottom-0 left-0 p-6 sm:p-8">
          <h3 class="text-2xl sm:text-3xl font-black text-white leading-tight">
            SALE MÙA HÈ<br />GIẢM ĐẾN 50%
          </h3>
          <p class="text-sm text-white/70 mt-2">
            Phong cách rực rỡ cho những ngày hè năng động.
          </p>
          <span
            class="inline-flex items-center mt-4 px-5 py-2.5 bg-white text-[#111827] text-sm font-bold rounded-full hover:bg-gray-100 transition"
          >
            Mua ngay
          </span>
        </div>
      </RouterLink>
      <!-- Banner 2: Collection -->
      <RouterLink
        to="/search"
        class="group relative overflow-hidden rounded-xl h-[280px] sm:h-[350px]"
      >
        <div
          class="absolute inset-0 bg-cover bg-center transition-transform duration-700 group-hover:scale-105"
          style="
            background-image: url(&quot;https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=960&q=80&quot;);
          "
        ></div>
        <div
          class="absolute inset-0 bg-gradient-to-t from-black/60 via-black/20 to-transparent"
        ></div>
        <div class="absolute bottom-0 left-0 p-6 sm:p-8">
          <h3 class="text-2xl sm:text-3xl font-black text-white leading-tight">
            BST THU ĐÔNG<br />2026
          </h3>
          <p class="text-sm text-white/70 mt-2">
            Sự thanh lịch và ấm áp cho mùa mới.
          </p>
          <span
            class="inline-flex items-center mt-4 px-5 py-2.5 bg-blue-600 text-white text-sm font-bold rounded-full hover:bg-blue-700 transition"
          >
            Khám phá
          </span>
        </div>
      </RouterLink>
    </div>
  </section>
</template>

<script setup lang="ts">
import { RouterLink } from "vue-router";
</script>
