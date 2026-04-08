<template>
  <div class="min-h-screen bg-[#FAFAFA]">
    <!-- Breadcrumb + Title -->
    <div class="bg-white border-b border-gray-100">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-6">
        <nav class="flex items-center text-sm text-gray-400 space-x-2 mb-3">
          <RouterLink to="/" class="hover:text-blue-600 transition">Trang chủ</RouterLink>
          <ChevronRightIcon class="w-3.5 h-3.5" />
          <span class="text-gray-700 font-medium">{{ pageTitle }}</span>
        </nav>
        <h1 class="text-2xl sm:text-3xl font-black text-[#111827] tracking-tight">{{ pageTitle }}</h1>
        <p v-if="totalItems > 0" class="text-sm text-gray-400 mt-1">
          {{ totalItems }} sản phẩm được tìm thấy
        </p>
      </div>
    </div>

    <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="flex flex-col lg:flex-row gap-8">

        <!-- ===== SIDEBAR FILTERS ===== -->
        <aside class="w-full lg:w-[260px] flex-shrink-0">
          <!-- Search box -->
          <div class="bg-white rounded-2xl p-5 shadow-sm mb-4">
            <h3 class="text-sm font-bold text-[#111827] mb-3 flex items-center">
              <SearchIcon class="w-4 h-4 mr-2 text-gray-400" /> TÌM KIẾM
            </h3>
            <div class="relative">
              <input v-model="searchInput" @keyup.enter="doSearch" type="text" placeholder="Nhập tên sản phẩm..."
                class="w-full px-4 py-2.5 pr-10 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
              <button @click="doSearch"
                class="absolute right-2 top-1/2 -translate-y-1/2 p-1.5 text-gray-400 hover:text-blue-600 transition">
                <SearchIcon class="w-4 h-4" />
              </button>
            </div>
          </div>

          <!-- Categories -->
          <div class="bg-white rounded-2xl p-5 shadow-sm mb-4">
            <h3 class="text-sm font-bold text-[#111827] mb-3 flex items-center">
              <LayoutGridIcon class="w-4 h-4 mr-2 text-gray-400" /> DANH MỤC
            </h3>
            <ul class="space-y-1">
              <li>
                <button @click="selectCategory(null)"
                  :class="['w-full text-left px-3 py-2 rounded-lg text-sm transition',
                    !activeCategoryId ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-gray-600 hover:bg-gray-50']">
                  Tất cả sản phẩm
                </button>
              </li>
              <li v-for="cat in rootCategories" :key="cat.maDanhMuc">
                <button @click="selectCategory(cat.maDanhMuc)"
                  :class="['w-full text-left px-3 py-2 rounded-lg text-sm transition',
                    activeCategoryId === cat.maDanhMuc ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-gray-600 hover:bg-gray-50']">
                  {{ cat.tenDanhMuc }}
                </button>
                <!-- Sub categories -->
                <ul v-if="getChildren(cat.maDanhMuc).length" class="ml-4 mt-0.5 space-y-0.5">
                  <li v-for="child in getChildren(cat.maDanhMuc)" :key="child.maDanhMuc">
                    <button @click="selectCategory(child.maDanhMuc)"
                      :class="['w-full text-left px-3 py-1.5 rounded-lg text-xs transition',
                        activeCategoryId === child.maDanhMuc ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-gray-500 hover:bg-gray-50']">
                      {{ child.tenDanhMuc }}
                    </button>
                  </li>
                </ul>
              </li>
            </ul>
          </div>

          <!-- Price Range -->
          <div class="bg-white rounded-2xl p-5 shadow-sm">
            <h3 class="text-sm font-bold text-[#111827] mb-3 flex items-center">
              <FilterIcon class="w-4 h-4 mr-2 text-gray-400" /> KHOẢNG GIÁ
            </h3>
            <div class="space-y-2">
              <button v-for="range in priceRanges" :key="range.label" @click="togglePriceRange(range)"
                :class="['w-full text-left px-3 py-2 rounded-lg text-sm transition',
                  activePriceRange === range ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-gray-600 hover:bg-gray-50']">
                {{ range.label }}
              </button>
            </div>
          </div>
        </aside>

        <!-- ===== MAIN CONTENT ===== -->
        <div class="flex-1">
          <!-- Sort bar -->
          <div class="flex items-center justify-between mb-6 bg-white rounded-2xl px-5 py-3 shadow-sm">
            <span class="text-sm text-gray-500 hidden sm:block">
              <span class="font-semibold text-gray-700">{{ filteredProducts.length }}</span> sản phẩm
            </span>
            <div class="flex items-center space-x-2">
              <span class="text-sm text-gray-500">Sắp xếp:</span>
              <select v-model="sortBy"
                class="text-sm border border-gray-200 rounded-lg px-3 py-1.5 focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 bg-white appearance-none pr-8 cursor-pointer">
                <option value="default">Mặc định</option>
                <option value="price-asc">Giá tăng dần</option>
                <option value="price-desc">Giá giảm dần</option>
                <option value="name-asc">Tên A → Z</option>
              </select>
            </div>
          </div>

          <!-- Loading -->
          <div v-if="loadingProducts" class="flex justify-center py-20">
            <Loader2Icon class="h-8 w-8 animate-spin text-blue-500" />
          </div>

          <!-- Empty -->
          <div v-else-if="filteredProducts.length === 0" class="text-center py-20">
            <SearchXIcon class="h-16 w-16 mx-auto text-gray-300 mb-4" />
            <h2 class="text-lg font-bold text-gray-500">Không tìm thấy sản phẩm</h2>
            <p class="text-sm text-gray-400 mt-2">Thử thay đổi từ khóa hoặc bộ lọc khác.</p>
          </div>

          <!-- Product Grid -->
          <div v-else class="grid grid-cols-2 md:grid-cols-3 xl:grid-cols-4 gap-4 sm:gap-5">
            <RouterLink v-for="sp in filteredProducts" :key="sp.maSp"
              :to="{ name: 'product-detail', params: { maSP: sp.maSp } }"
              class="group cursor-pointer bg-white rounded-2xl overflow-hidden shadow-sm hover:shadow-md transition-all duration-300">
              <div class="relative overflow-hidden aspect-[3/4]">
                <img :src="sp.anhDaiDien || placeholderImg" :alt="sp.tenSp"
                  class="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105" />
                <span v-if="sp.giaGoc && sp.giaGoc > sp.giaKm"
                  class="absolute top-3 left-3 bg-red-500 text-white text-[10px] font-bold px-2 py-0.5 rounded-full">
                  -{{ Math.round((1 - sp.giaKm / sp.giaGoc) * 100) }}%
                </span>
              </div>
              <div class="p-3 sm:p-4">
                <h3 class="text-sm font-semibold text-[#111827] line-clamp-2 group-hover:text-blue-600 transition min-h-[2.5rem]">
                  {{ sp.tenSp }}
                </h3>
                <div class="flex items-center space-x-2 mt-2">
                  <span class="text-sm font-bold text-red-600">{{ formatPrice(sp.giaKm) }}</span>
                  <span v-if="sp.giaGoc && sp.giaGoc > sp.giaKm"
                    class="text-xs text-gray-400 line-through">{{ formatPrice(sp.giaGoc) }}</span>
                </div>
                <span v-if="sp.tenDanhMuc" class="inline-block mt-2 text-[10px] font-semibold text-gray-400 bg-gray-100 px-2 py-0.5 rounded-full">
                  {{ sp.tenDanhMuc }}
                </span>
              </div>
            </RouterLink>
          </div>

          <!-- Pagination -->
          <div v-if="totalPages > 1" class="flex items-center justify-center space-x-2 mt-10">
            <button @click="goToPage(currentPage - 1)" :disabled="currentPage <= 1"
              class="w-10 h-10 rounded-xl border border-gray-200 flex items-center justify-center text-gray-500 hover:bg-gray-100 disabled:opacity-30 disabled:cursor-not-allowed transition">
              <ChevronLeftIcon class="w-4 h-4" />
            </button>
            <button v-for="p in visiblePages" :key="p" @click="goToPage(p)"
              :class="['w-10 h-10 rounded-xl text-sm font-bold transition',
                currentPage === p
                  ? 'bg-[#111827] text-white shadow'
                  : 'border border-gray-200 text-gray-600 hover:bg-gray-100']">
              {{ p }}
            </button>
            <button @click="goToPage(currentPage + 1)" :disabled="currentPage >= totalPages"
              class="w-10 h-10 rounded-xl border border-gray-200 flex items-center justify-center text-gray-500 hover:bg-gray-100 disabled:opacity-30 disabled:cursor-not-allowed transition">
              <ChevronRightIcon2 class="w-4 h-4" />
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import { useRoute, useRouter, RouterLink } from "vue-router";
import {
  Search as SearchIcon, SearchX as SearchXIcon,
  LayoutGrid as LayoutGridIcon, Filter as FilterIcon,
  ChevronRight as ChevronRightIcon, ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon2,
  Loader2 as Loader2Icon,
} from "lucide-vue-next";
import { sanPhamService } from "../../services/sanpham.service";
import { danhMucService } from "../../services/danhmuc.service";

const route = useRoute();
const router = useRouter();
const props = defineProps<{ maDM?: string }>();

const PAGE_SIZE = 12;
const placeholderImg = "https://via.placeholder.com/400x533?text=No+Image";

// State
const allProducts = ref<any[]>([]);
const allCategories = ref<any[]>([]);
const loadingProducts = ref(true);
const currentPage = ref(1);
const totalItems = ref(0);
const totalPages = ref(1);
const searchInput = ref("");
const activeSearchQuery = ref("");
const activeCategoryId = ref<number | null>(null);
const sortBy = ref("default");
const activePriceRange = ref<any>(null);

const priceRanges = [
  { label: "Dưới 200.000đ", min: 0, max: 200000 },
  { label: "200.000đ - 500.000đ", min: 200000, max: 500000 },
  { label: "500.000đ - 1.000.000đ", min: 500000, max: 1000000 },
  { label: "Trên 1.000.000đ", min: 1000000, max: Infinity },
];

const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";

// Dynamic page title
const pageTitle = computed(() => {
  if (activeSearchQuery.value) return `Kết quả tìm kiếm: "${activeSearchQuery.value}"`;
  if (activeCategoryId.value) {
    const cat = allCategories.value.find(c => c.maDanhMuc === activeCategoryId.value);
    return cat ? cat.tenDanhMuc : "Danh mục";
  }
  return "Tất cả sản phẩm";
});

// Root categories
const rootCategories = computed(() =>
  allCategories.value.filter((c: any) => !c.maDanhMucCha)
);
const getChildren = (parentId: number) =>
  allCategories.value.filter((c: any) => c.maDanhMucCha === parentId);

// Client-side filter + sort
const filteredProducts = computed(() => {
  let items = [...allProducts.value];
  // Price range filter
  if (activePriceRange.value) {
    const { min, max } = activePriceRange.value;
    items = items.filter(sp => sp.giaKm >= min && sp.giaKm < max);
  }
  // Sort
  if (sortBy.value === "price-asc") items.sort((a, b) => a.giaKm - b.giaKm);
  else if (sortBy.value === "price-desc") items.sort((a, b) => b.giaKm - a.giaKm);
  else if (sortBy.value === "name-asc") items.sort((a, b) => (a.tenSp || "").localeCompare(b.tenSp || ""));
  return items;
});

// Visible pagination pages
const visiblePages = computed(() => {
  const pages: number[] = [];
  const start = Math.max(1, currentPage.value - 2);
  const end = Math.min(totalPages.value, currentPage.value + 2);
  for (let i = start; i <= end; i++) pages.push(i);
  return pages;
});

// Load products from API
const loadProducts = async () => {
  loadingProducts.value = true;
  try {
    let res: any;
    if (activeSearchQuery.value) {
      res = await sanPhamService.searchByName(activeSearchQuery.value, currentPage.value, PAGE_SIZE);
    } else if (activeCategoryId.value) {
      res = await sanPhamService.getByCategory(activeCategoryId.value, currentPage.value, PAGE_SIZE);
    } else {
      res = await sanPhamService.getAll(currentPage.value, PAGE_SIZE);
    }

    if (res.success && res.data) {
      allProducts.value = res.data.item || res.data.items || [];
      totalItems.value = res.data.totalItem || res.data.totalItems || allProducts.value.length;
      totalPages.value = res.data.totalPage || res.data.totalPages || Math.ceil(totalItems.value / PAGE_SIZE);
    } else {
      allProducts.value = [];
      totalItems.value = 0;
      totalPages.value = 1;
    }
  } catch (e) {
    console.warn("Failed to load products", e);
    allProducts.value = [];
  } finally {
    loadingProducts.value = false;
  }
};

// Load categories
const loadCategories = async () => {
  try {
    const res: any = await danhMucService.getAll();
    if (res.success) allCategories.value = res.data || [];
  } catch (e) {
    console.warn("Failed to load categories", e);
  }
};

const doSearch = () => {
  const q = searchInput.value.trim();
  if (q) {
    activeCategoryId.value = null;
    activeSearchQuery.value = q;
    currentPage.value = 1;
    router.push({ name: "product-search", query: { q } });
    loadProducts();
  }
};

const selectCategory = (maDM: number | null) => {
  activeCategoryId.value = maDM;
  activeSearchQuery.value = "";
  searchInput.value = "";
  currentPage.value = 1;
  if (maDM) {
    router.push({ name: "category-products", params: { maDM: String(maDM) } });
  } else {
    router.push({ name: "product-search" });
  }
  loadProducts();
};

const togglePriceRange = (range: any) => {
  activePriceRange.value = activePriceRange.value === range ? null : range;
};

const goToPage = (page: number) => {
  if (page < 1 || page > totalPages.value) return;
  currentPage.value = page;
  loadProducts();
  window.scrollTo({ top: 0, behavior: "smooth" });
};

onMounted(async () => {
  await loadCategories();

  // Check route params / query
  if (props.maDM || route.params.maDM) {
    activeCategoryId.value = Number(props.maDM || route.params.maDM);
  }
  if (route.query.q) {
    searchInput.value = route.query.q as string;
    activeSearchQuery.value = route.query.q as string;
  }

  await loadProducts();
});

// Watch for route changes (in case user navigates while on search page)
watch(() => route.fullPath, async () => {
  if (route.name === "category-products" && route.params.maDM) {
    activeCategoryId.value = Number(route.params.maDM);
    activeSearchQuery.value = "";
    searchInput.value = "";
    currentPage.value = 1;
    await loadProducts();
  } else if (route.name === "product-search") {
    if (route.query.q) {
      searchInput.value = route.query.q as string;
      activeSearchQuery.value = route.query.q as string;
    }
    activeCategoryId.value = null;
    currentPage.value = 1;
    await loadProducts();
  }
});
</script>

<style scoped>
select {
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' stroke='%236B7280' stroke-width='2'%3E%3Cpath stroke-linecap='round' stroke-linejoin='round' d='M19 9l-7 7-7-7'%3E%3C/path%3E%3C/svg%3E");
  background-position: right 0.5rem center;
  background-repeat: no-repeat;
  background-size: 1rem;
}
</style>
