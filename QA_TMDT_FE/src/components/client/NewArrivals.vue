<template>
  <section class="py-16 bg-white">
    <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between mb-8">
        <h2 class="text-2xl sm:text-3xl font-black text-[#111827] tracking-tight">HÀNG MỚI VỀ</h2>
        <RouterLink to="/search" class="text-sm font-semibold text-blue-600 hover:text-blue-700 flex items-center transition">
          Xem tất cả <ArrowRightIcon class="w-4 h-4 ml-1" />
        </RouterLink>
      </div>
      <div v-if="loading" class="flex justify-center py-10"><Loader2Icon class="h-6 w-6 animate-spin text-gray-400" /></div>
      <div v-else-if="products.length === 0" class="text-center py-10 text-gray-400 text-sm">Chưa có sản phẩm nào.</div>
      <div v-else class="relative">
        <div ref="scrollRef" class="flex space-x-4 overflow-x-auto scrollbar-hide scroll-smooth pb-4">
          <RouterLink v-for="(sp, idx) in products" :key="sp.maSp"
            :to="{ name: 'product-detail', params: { maSP: sp.maSp } }"
            class="flex-shrink-0 w-[260px] group cursor-pointer">
            <div class="relative overflow-hidden rounded-lg bg-gray-100 aspect-[3/4]">
              <img :src="sp.anhDaiDien || 'https://via.placeholder.com/400x533'" :alt="sp.tenSp"
                class="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105" />
              <span v-if="idx < 3" class="absolute top-3 left-3 bg-blue-600 text-white text-[10px] font-bold px-2 py-0.5 rounded">NEW</span>
            </div>
            <div class="mt-3">
              <h3 class="text-sm font-semibold text-[#111827] line-clamp-2 group-hover:text-blue-600 transition">{{ sp.tenSp }}</h3>
              <div class="flex items-center space-x-2 mt-1">
                <span class="text-sm font-bold text-[#111827]">{{ formatPrice(sp.giaKm) }}</span>
                <span v-if="sp.giaGoc && sp.giaGoc > sp.giaKm" class="text-xs text-gray-400 line-through">{{ formatPrice(sp.giaGoc) }}</span>
              </div>
            </div>
          </RouterLink>
        </div>
        <button @click="scrollLeft" class="hidden md:flex absolute -left-4 top-1/3 w-9 h-9 rounded-full bg-white shadow-lg items-center justify-center text-gray-600 hover:text-black transition z-10">
          <ChevronLeftIcon class="w-5 h-5" />
        </button>
        <button @click="scrollRight" class="hidden md:flex absolute -right-4 top-1/3 w-9 h-9 rounded-full bg-white shadow-lg items-center justify-center text-gray-600 hover:text-black transition z-10">
          <ChevronRightIcon class="w-5 h-5" />
        </button>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { RouterLink } from "vue-router";
import type { ProductSummary } from "../../types";
import { ArrowRight as ArrowRightIcon, ChevronLeft as ChevronLeftIcon, ChevronRight as ChevronRightIcon, Loader2 as Loader2Icon } from "lucide-vue-next";
import { sanPhamService } from "../../services/sanpham.service";
import { getPageItems } from "../../services/service-helpers";

const products = ref<ProductSummary[]>([]);
const loading = ref(true);
const scrollRef = ref<HTMLElement>();

const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";
const scrollLeft = () => { scrollRef.value?.scrollBy({ left: -280, behavior: "smooth" }); };
const scrollRight = () => { scrollRef.value?.scrollBy({ left: 280, behavior: "smooth" }); };

onMounted(async () => {
  try {
    const response = await sanPhamService.getAll(1, 12);
    if (response.success && response.data) {
      products.value = getPageItems(response.data);
    }
  } catch {
    products.value = [];
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.scrollbar-hide::-webkit-scrollbar { display: none; }
.scrollbar-hide { -ms-overflow-style: none; scrollbar-width: none; }
</style>
