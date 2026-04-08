<template>
  <section class="py-16 bg-[#F9FAFB]">
    <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8">
      <h2 class="text-2xl sm:text-3xl font-black text-[#111827] text-center tracking-tight">SẢN PHẨM BÁN CHẠY</h2>
      <!-- Filter Tabs -->
      <div class="flex justify-center mt-5 mb-8 space-x-6">
        <button v-for="tab in tabs" :key="tab" @click="activeTab = tab"
          :class="['text-sm font-semibold pb-1.5 transition border-b-2',
            activeTab === tab ? 'text-[#111827] border-blue-600' : 'text-gray-400 border-transparent hover:text-gray-600']">
          {{ tab }}
        </button>
      </div>
      <div v-if="loading" class="flex justify-center py-10"><Loader2Icon class="h-6 w-6 animate-spin text-gray-400" /></div>
      <div v-else-if="filteredProducts.length === 0" class="text-center py-10 text-gray-400 text-sm">Chưa có sản phẩm nào.</div>
      <!-- Product Grid -->
      <div v-else class="grid grid-cols-2 lg:grid-cols-4 gap-4 sm:gap-6">
        <RouterLink v-for="(sp, idx) in filteredProducts" :key="sp.maSp"
          :to="{ name: 'product-detail', params: { maSP: sp.maSp } }"
          class="group cursor-pointer">
          <div class="relative overflow-hidden rounded-lg bg-white aspect-[3/4]">
            <img :src="sp.anhDaiDien || 'https://via.placeholder.com/400x533'" :alt="sp.tenSp"
              class="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105" />
            <span v-if="badges[idx % badges.length]" :class="['absolute top-3 left-3 text-[10px] font-bold px-2 py-0.5 rounded',
              badges[idx % badges.length] === 'HOT' ? 'bg-red-500 text-white' : 'bg-green-500 text-white']">
              {{ badges[idx % badges.length] }}
            </span>
          </div>
          <div class="mt-3">
            <h3 class="text-sm font-semibold text-[#111827] line-clamp-2 group-hover:text-blue-600 transition">{{ sp.tenSp }}</h3>
            <div class="flex items-center space-x-2 mt-1">
              <span class="text-sm font-bold text-red-600">{{ formatPrice(sp.giaKm) }}</span>
              <span v-if="sp.giaGoc && sp.giaGoc > sp.giaKm" class="text-xs text-gray-400 line-through">{{ formatPrice(sp.giaGoc) }}</span>
            </div>
            <div class="flex items-center mt-1.5 space-x-0.5">
              <StarIcon v-for="s in 5" :key="s" class="w-3 h-3" :class="s <= 4 ? 'text-yellow-400 fill-yellow-400' : 'text-gray-200'" />
              <span class="text-[10px] text-gray-400 ml-1">(4.0)</span>
            </div>
          </div>
        </RouterLink>
      </div>
      <div class="text-center mt-10">
        <a href="#" class="inline-flex items-center px-6 py-3 border-2 border-[#111827] text-[#111827] font-bold text-sm rounded-full hover:bg-[#111827] hover:text-white transition">
          Xem tất cả sản phẩm <ArrowRightIcon class="w-4 h-4 ml-2" />
        </a>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { RouterLink } from "vue-router";
import { ArrowRight as ArrowRightIcon, Star as StarIcon, Loader2 as Loader2Icon } from "lucide-vue-next";
import { sanPhamService } from "../../services/sanpham.service";

const tabs = ["Tất cả", "Áo", "Quần", "Phụ kiện"];
const activeTab = ref("Tất cả");
const badges = ["HOT", "SALE", null, "HOT", null, "SALE", null, "HOT"];
const products = ref<any[]>([]);
const loading = ref(true);

const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";

const filteredProducts = computed(() => {
  if (activeTab.value === "Tất cả") return products.value.slice(0, 8);
  // Simple keyword filter on tenDanhMuc
  const keyword = activeTab.value.toLowerCase();
  return products.value.filter((sp: any) =>
    sp.tenDanhMuc?.toLowerCase().includes(keyword)
  ).slice(0, 8);
});

onMounted(async () => {
  try {
    const res: any = await sanPhamService.getAll(1, 20);
    if (res.success && res.data) {
      products.value = res.data.item || res.data.items || [];
    }
  } catch (e) {
    console.warn("Failed to load best sellers", e);
  } finally {
    loading.value = false;
  }
});
</script>
