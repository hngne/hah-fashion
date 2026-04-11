<template>
  <section class="py-16 px-4 sm:px-6 lg:px-8 max-w-[1400px] mx-auto">
    <div class="flex items-center justify-between mb-8">
      <h2 class="text-2xl sm:text-3xl font-black text-[#111827] tracking-tight">
        Danh mục nổi bật
      </h2>
      <RouterLink
        to="/search"
        class="text-sm font-semibold text-blue-600 hover:text-blue-700 flex items-center transition"
      >
        Xem tất cả <ArrowRightIcon class="w-4 h-4 ml-1" />
      </RouterLink>
    </div>
    <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
      <RouterLink
        v-for="cat in displayCategories"
        :key="cat.maDanhMuc"
        :to="{ name: 'category-products', params: { maDM: cat.maDanhMuc } }"
        class="group relative overflow-hidden rounded-xl aspect-[3/4] cursor-pointer"
      >
        <!-- Background -->
        <div
          class="absolute inset-0 bg-cover bg-center transition-transform duration-500 group-hover:scale-105"
          :style="{ backgroundImage: `url(${cat.coverImage})` }"
        ></div>
        <!-- Overlay -->
        <div
          class="absolute inset-0 bg-gradient-to-t from-black/70 via-black/20 to-transparent"
        ></div>
        <!-- Label -->
        <div class="absolute bottom-0 left-0 right-0 p-4 sm:p-5">
          <h3 class="text-lg sm:text-xl font-bold text-white">
            {{ cat.tenDanhMuc }}
          </h3>
        </div>
      </RouterLink>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { RouterLink } from "vue-router";
import type { CategoryItem } from "../../types";
import { ArrowRight as ArrowRightIcon } from "lucide-vue-next";
import { danhMucService } from "../../services/danhmuc.service";

const categories = ref<CategoryItem[]>([]);

// Fallback cover images for categories
const defaultCovers = [
  "https://images.unsplash.com/photo-1487222477894-8943e31ef7b2?w=600&q=80",
  "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=600&q=80",
  "https://images.unsplash.com/photo-1471286174890-9c112ffca5b4?w=600&q=80",
  "https://images.unsplash.com/photo-1441984904996-e0b6ba687e04?w=600&q=80",
];

const displayCategories = computed(() =>
  categories.value.map((cat, i) => ({
    ...cat,
    coverImage: cat.anhDaiDien || defaultCovers[i % defaultCovers.length],
  })),
);

onMounted(async () => {
  try {
    const response = await danhMucService.getAll();
    if (response.success) {
      categories.value = (response.data || [])
        .filter((category: CategoryItem) => !category.maDanhMucCha)
        .slice(0, 4);
    }
  } catch {
    categories.value = [];
  }
});
</script>
