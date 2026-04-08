<template>
  <section class="py-16 bg-white">
    <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8">
      <!-- Title -->
      <div class="text-center mb-10">
        <span class="text-xs font-bold uppercase tracking-widest text-blue-600"
          >MÙA HÈ 2026</span
        >
        <h2 class="text-3xl sm:text-4xl font-black text-[#111827] mt-2">
          BỘ SƯU TẬP
        </h2>
        <p class="text-sm text-gray-500 mt-3 max-w-lg mx-auto">
          Khám phá những phong cách mới nhất được tuyển chọn kỹ lưỡng bởi các
          biên tập viên thời trang hàng đầu.
        </p>
      </div>

      <!-- Masonry Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-12">
        <!-- Large left -->
        <a
          href="#"
          class="group relative overflow-hidden rounded-xl row-span-2 min-h-[400px] md:min-h-[500px]"
        >
          <div
            class="absolute inset-0 bg-cover bg-center transition-transform duration-700 group-hover:scale-105"
            style="
              background-image: url(&quot;https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=800&q=80&quot;);
            "
          ></div>
          <div
            class="absolute inset-0 bg-gradient-to-t from-black/60 via-transparent to-transparent"
          ></div>
          <div class="absolute bottom-0 left-0 p-6">
            <span
              class="text-xs font-bold text-white/60 uppercase tracking-wide"
              >Nổi bật</span
            >
            <h3 class="text-xl sm:text-2xl font-black text-white mt-1">
              Streetwear Essentials
            </h3>
            <p class="text-sm text-white/60 mt-1">
              Phong cách đường phố hiện đại kết hợp sự thoải mái và cá tính
              riêng biệt.
            </p>
          </div>
        </a>
        <!-- Top right -->
        <a
          href="#"
          class="group relative overflow-hidden rounded-xl min-h-[240px]"
        >
          <div
            class="absolute inset-0 bg-cover bg-center transition-transform duration-700 group-hover:scale-105"
            style="
              background-image: url(&quot;https://images.unsplash.com/photo-1483985988355-763728e1935b?w=800&q=80&quot;);
            "
          ></div>
          <div
            class="absolute inset-0 bg-gradient-to-t from-black/50 to-transparent"
          ></div>
          <div class="absolute bottom-0 left-0 p-5">
            <h3 class="text-lg font-bold text-white">Color Block Trends</h3>
            <p class="text-xs text-white/60 mt-0.5">Sắc màu rực rỡ</p>
          </div>
        </a>
        <!-- Bottom right -->
        <a
          href="#"
          class="group relative overflow-hidden rounded-xl min-h-[240px]"
        >
          <div
            class="absolute inset-0 bg-cover bg-center transition-transform duration-700 group-hover:scale-105"
            style="
              background-image: url(&quot;https://images.unsplash.com/photo-1558618666-fcd25c85f82e?w=800&q=80&quot;);
            "
          ></div>
          <div
            class="absolute inset-0 bg-gradient-to-t from-black/50 to-transparent"
          ></div>
          <div class="absolute bottom-0 left-0 p-5">
            <h3 class="text-lg font-bold text-white">Evening Elegance</h3>
            <p class="text-xs text-white/60 mt-0.5">Dạ hội sang trọng</p>
          </div>
        </a>
      </div>

      <!-- Trending This Week -->
      <div class="flex items-center justify-between mb-6">
        <h3 class="text-xl font-bold text-[#111827]">Xu hướng tuần này</h3>
        <a
          href="#"
          class="text-sm font-semibold text-blue-600 hover:text-blue-700 flex items-center"
        >
          Xem tất cả <ArrowRightIcon class="w-4 h-4 ml-1" />
        </a>
      </div>
      <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
        <div
          v-for="(trend, idx) in trends"
          :key="idx"
          class="group cursor-pointer"
        >
          <div class="relative overflow-hidden rounded-xl aspect-[3/4]">
            <div
              class="absolute inset-0 bg-cover bg-center transition-transform duration-500 group-hover:scale-105"
              :style="{ backgroundImage: `url(${trend.image})` }"
            ></div>
            <!-- Heart -->
            <button
              class="absolute bottom-3 right-3 w-8 h-8 rounded-full bg-white/90 flex items-center justify-center text-gray-600 hover:text-red-500 transition shadow"
            >
              <HeartIcon class="w-4 h-4" />
            </button>
          </div>
          <div class="mt-2.5">
            <h4 class="text-sm font-bold text-[#111827]">{{ trend.name }}</h4>
            <p class="text-xs text-gray-400">{{ trend.desc }}</p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import {
  ArrowRight as ArrowRightIcon,
  Heart as HeartIcon,
} from "lucide-vue-next";

const trends = [
  {
    name: "Minimalist Beige",
    desc: "Phong cách tối giản",
    image:
      "https://images.unsplash.com/photo-1544441893-675973e31985?w=500&q=80",
  },
  {
    name: "Vintage Denim",
    desc: "Cổ điển thập niên 90",
    image:
      "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=500&q=80",
  },
  {
    name: "Modern Tailoring",
    desc: "Âu phục hiện đại",
    image:
      "https://images.unsplash.com/photo-1507679799987-c73b7f05d498?w=500&q=80",
  },
  {
    name: "Winter Layers",
    desc: "Ấm áp & Sành điệu",
    image:
      "https://images.unsplash.com/photo-1539109136881-3be0616acf4b?w=500&q=80",
  },
];
</script>
