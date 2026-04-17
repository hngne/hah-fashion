<template>
  <div class="min-h-screen bg-[var(--bg-page)]">
    <section class="border-b border-[var(--ui-border)] bg-white">
      <div class="mx-auto max-w-[1400px] px-4 py-6 sm:px-6 lg:px-8">
        <nav class="mb-3 flex items-center gap-2 text-sm text-slate-400">
          <RouterLink to="/" class="transition hover:text-[var(--accent)]">Trang chủ</RouterLink>
          <ChevronRightIcon class="h-3.5 w-3.5" />
          <span class="font-medium text-slate-700">{{ pageTitle }}</span>
        </nav>
        <div class="flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
          <div>
            <h1 class="text-2xl font-black tracking-tight text-slate-950 sm:text-3xl">
              {{ pageTitle }}
            </h1>
            <p class="mt-1 text-sm text-slate-500">
              {{ totalFilteredItems }} sản phẩm phù hợp sau khi áp dụng bộ lọc hiện tại.
            </p>
          </div>

          <div class="flex gap-3">
            <div class="relative min-w-[240px] flex-1">
              <SearchIcon class="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
              <input
                v-model="searchInput"
                type="text"
                placeholder="Tìm theo tên sản phẩm..."
                class="w-full rounded-2xl border border-[var(--ui-border)] bg-white py-3 pl-11 pr-4 text-sm text-slate-900 outline-none transition focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]"
                @keyup.enter="doSearch"
              />
            </div>
            <button
              type="button"
              class="inline-flex h-12 items-center justify-center rounded-2xl bg-slate-950 px-5 text-sm font-semibold text-white transition hover:bg-slate-800"
              @click="doSearch"
            >
              Tìm
            </button>
          </div>
        </div>
      </div>
    </section>

    <div class="mx-auto max-w-[1400px] px-4 py-8 sm:px-6 lg:px-8">
      <div class="flex flex-col gap-8 lg:flex-row">
        <aside class="hidden w-[280px] shrink-0 lg:block">
          <div class="sticky top-24 space-y-4">
            <CatalogFilterPanel
              :categories="rootCategories"
              :colors="colors"
              :sizes="sizes"
              :active-category-id="activeCategoryId"
              :active-price-key="priceKey"
              :active-color-id="activeColorId"
              :active-size-id="activeSizeId"
              :get-children="getChildren"
              @select-category="selectCategory"
              @toggle-price="togglePrice"
              @toggle-color="toggleColor"
              @toggle-size="toggleSize"
            />
          </div>
        </aside>

        <div class="min-w-0 flex-1">
          <div class="mb-4 flex flex-wrap items-center justify-between gap-3 rounded-3xl border border-[var(--ui-border)] bg-white px-4 py-3 shadow-[var(--shadow-soft)] sm:px-5">
            <button
              type="button"
              class="inline-flex items-center gap-2 rounded-full border border-[var(--ui-border)] bg-slate-50 px-4 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-100 lg:hidden"
              @click="filterOpen = true"
            >
              <SlidersHorizontalIcon class="h-4 w-4" />
              Bộ lọc
            </button>

            <div class="flex flex-wrap items-center gap-2 text-sm text-slate-500">
              <span class="font-medium text-slate-700">{{ totalFilteredItems }}</span>
              <span>sản phẩm</span>
              <span v-if="activeCategoryName" class="rounded-full bg-slate-100 px-2.5 py-1 text-xs font-semibold text-slate-600">
                {{ activeCategoryName }}
              </span>
              <span v-if="activeSearchQuery" class="rounded-full bg-slate-100 px-2.5 py-1 text-xs font-semibold text-slate-600">
                "{{ activeSearchQuery }}"
              </span>
              <span v-if="activeColorName" class="rounded-full bg-slate-100 px-2.5 py-1 text-xs font-semibold text-slate-600">
                Mau: {{ activeColorName }}
              </span>
              <span v-if="activeSizeName" class="rounded-full bg-slate-100 px-2.5 py-1 text-xs font-semibold text-slate-600">
                Size: {{ activeSizeName }}
              </span>
            </div>

            <div class="flex items-center gap-2">
              <label for="sort" class="text-sm font-medium text-slate-500">Sắp xếp</label>
              <select
                id="sort"
                v-model="sortValue"
                class="rounded-full border border-[var(--ui-border)] bg-white px-4 py-2 text-sm font-medium text-slate-700 outline-none transition focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]"
                @change="changeSort"
              >
                <option value="default">Mặc định</option>
                <option value="price-asc">Giá tăng dần</option>
                <option value="price-desc">Giá giảm dần</option>
                <option value="name-asc">Tên A-Z</option>
              </select>
            </div>
          </div>

          <div v-if="loading" class="grid grid-cols-2 gap-4 sm:grid-cols-3 xl:grid-cols-4">
            <div
              v-for="index in 8"
              :key="index"
              class="overflow-hidden rounded-3xl border border-[var(--ui-border)] bg-white p-3 shadow-[var(--shadow-soft)]"
            >
              <div class="aspect-[3/4] animate-pulse rounded-2xl bg-slate-100" />
              <div class="mt-3 h-4 animate-pulse rounded bg-slate-100" />
              <div class="mt-2 h-4 w-2/3 animate-pulse rounded bg-slate-100" />
            </div>
          </div>

          <div
            v-else-if="errorMessage"
            class="rounded-3xl border border-rose-200 bg-rose-50 px-4 py-6 text-sm font-medium text-rose-700"
          >
            {{ errorMessage }}
          </div>

          <div
            v-else-if="!paginatedProducts.length"
            class="rounded-3xl border border-dashed border-[var(--ui-border)] bg-white px-4 py-16 text-center"
          >
            <SearchXIcon class="mx-auto h-16 w-16 text-slate-200" />
            <h2 class="mt-4 text-lg font-semibold text-slate-700">Không tìm thấy sản phẩm</h2>
            <p class="mt-2 text-sm text-slate-500">
              Thử đổi từ khóa, danh mục hoặc khoảng giá để có kết quả tốt hơn.
            </p>
          </div>

          <div
            v-else
            class="grid grid-cols-2 gap-4 sm:grid-cols-3 xl:grid-cols-4"
          >
            <RouterLink
              v-for="product in paginatedProducts"
              :key="product.maSp"
              :to="{ name: 'product-detail', params: { maSP: product.maSp } }"
              class="group overflow-hidden rounded-3xl border border-[var(--ui-border)] bg-white p-3 shadow-[var(--shadow-soft)] transition hover:-translate-y-0.5 hover:shadow-[var(--shadow-card)]"
            >
              <div class="relative overflow-hidden rounded-2xl bg-slate-100">
                <img
                  :src="product.anhDaiDien || placeholderImage"
                  :alt="product.tenSp"
                  class="aspect-[3/4] h-full w-full object-cover transition duration-500 group-hover:scale-105"
                />
                <span
                  v-if="product.giaGoc > product.giaKm"
                  class="absolute left-3 top-3 rounded-full bg-rose-500 px-2.5 py-1 text-[11px] font-semibold text-white"
                >
                  -{{ Math.round((1 - product.giaKm / product.giaGoc) * 100) }}%
                </span>
              </div>

              <div class="mt-3">
                <p class="text-sm font-semibold text-slate-900 line-clamp-2 transition group-hover:text-[var(--accent)]">
                  {{ product.tenSp }}
                </p>
                <div class="mt-2 flex flex-wrap items-center gap-2">
                  <span class="text-sm font-bold text-rose-600">{{ formatMoney(product.giaKm) }}</span>
                  <span
                    v-if="product.giaGoc > product.giaKm"
                    class="text-xs text-slate-400 line-through"
                  >
                    {{ formatMoney(product.giaGoc) }}
                  </span>
                </div>
                <span
                  v-if="product.tenDanhMuc"
                  class="mt-2 inline-flex rounded-full bg-slate-100 px-2.5 py-1 text-[11px] font-semibold text-slate-500"
                >
                  {{ product.tenDanhMuc }}
                </span>
              </div>
            </RouterLink>
          </div>

          <div
            v-if="totalPages > 1"
            class="mt-8 flex flex-wrap items-center justify-center gap-2"
          >
            <button
              type="button"
              :disabled="currentPage <= 1"
              class="inline-flex h-11 w-11 items-center justify-center rounded-full border border-[var(--ui-border)] bg-white text-slate-600 transition hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-40"
              @click="goPage(currentPage - 1)"
            >
              <ChevronLeftIcon class="h-4 w-4" />
            </button>
            <button
              v-for="value in visiblePages"
              :key="value"
              type="button"
              :class="pageButtonClass(value)"
              @click="goPage(value)"
            >
              {{ value }}
            </button>
            <button
              type="button"
              :disabled="currentPage >= totalPages"
              class="inline-flex h-11 w-11 items-center justify-center rounded-full border border-[var(--ui-border)] bg-white text-slate-600 transition hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-40"
              @click="goPage(currentPage + 1)"
            >
              <ChevronRightIcon2 class="h-4 w-4" />
            </button>
          </div>
        </div>
      </div>
    </div>

    <teleport to="body">
      <transition name="drawer-fade">
        <div
          v-if="filterOpen"
          class="fixed inset-0 z-50 bg-slate-950/50"
          @click.self="filterOpen = false"
        >
          <div class="ml-auto h-full w-full max-w-sm overflow-y-auto bg-white p-4 shadow-2xl">
            <div class="mb-4 flex items-center justify-between">
              <div>
                <h3 class="text-lg font-semibold text-slate-900">Bộ lọc</h3>
                <p class="text-sm text-slate-500">Danh mục và khoảng giá</p>
              </div>
              <button
                type="button"
                class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] text-slate-500 transition hover:text-slate-900"
                @click="filterOpen = false"
              >
                <XIcon class="h-4 w-4" />
              </button>
            </div>
            <CatalogFilterPanel
              :categories="rootCategories"
              :colors="colors"
              :sizes="sizes"
              :active-category-id="activeCategoryId"
              :active-price-key="priceKey"
              :active-color-id="activeColorId"
              :active-size-id="activeSizeId"
              :get-children="getChildren"
              @select-category="selectCategoryAndClose"
              @toggle-price="togglePriceAndClose"
              @toggle-color="toggleColorAndClose"
              @toggle-size="toggleSizeAndClose"
            />
          </div>
        </div>
      </transition>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import { RouterLink, useRoute, useRouter } from "vue-router";
import {
  ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon,
  ChevronRight as ChevronRightIcon2,
  Search as SearchIcon,
  SearchX as SearchXIcon,
  SlidersHorizontal as SlidersHorizontalIcon,
  X as XIcon,
} from "lucide-vue-next";
import CatalogFilterPanel from "../../components/client/CatalogFilterPanel.vue";
import { danhMucService } from "../../services/danhmuc.service";
import { mauSizeService } from "../../services/mausize.service";
import {
  extractErrorMessage,
  getPageItems,
  getTotalItems,
  getTotalPages,
} from "../../services/service-helpers";
import { sanPhamService } from "../../services/sanpham.service";
import type { CategoryItem, ColorItem, ProductSummary, SizeItem } from "../../types";

const route = useRoute();
const router = useRouter();
const props = defineProps<{ maDM?: string }>();

const PAGE_SIZE = 12;
const FETCH_BATCH_SIZE = 20;
const placeholderImage = "https://via.placeholder.com/400x533?text=No+Image";

const categories = ref<CategoryItem[]>([]);
const colors = ref<ColorItem[]>([]);
const products = ref<ProductSummary[]>([]);
const sizes = ref<SizeItem[]>([]);
const totalItems = ref(0);
const serverTotalPages = ref(1);
const loading = ref(true);
const errorMessage = ref("");
const searchInput = ref("");
const filterOpen = ref(false);

const currentCategoryParam = computed(() => {
  const rawValue = props.maDM || (route.params.maDM as string | undefined);
  if (!rawValue) return null;
  const parsed = Number(rawValue);
  return Number.isNaN(parsed) ? null : parsed;
});

const activeSearchQuery = computed(() =>
  typeof route.query.q === "string" ? route.query.q.trim() : "",
);

const sortValue = ref("default");

const priceKey = computed(() =>
  typeof route.query.price === "string" ? route.query.price : "",
);

const useClientAggregation = computed(() => false);

const currentPage = computed(() => {
  const rawValue = Number(route.query.page || 1);
  if (!Number.isFinite(rawValue) || rawValue < 1) return 1;
  return Math.floor(rawValue);
});

const rootCategories = computed(() =>
  categories.value.filter((category) => !category.maDanhMucCha),
);

const activeCategoryId = computed(() => currentCategoryParam.value);
const activeColorId = computed(() => {
  const rawValue = Number(route.query.mau);
  if (!Number.isFinite(rawValue) || rawValue < 1) return null;
  return Math.floor(rawValue);
});
const activeSizeId = computed(() => {
  const rawValue = Number(route.query.size);
  if (!Number.isFinite(rawValue) || rawValue < 1) return null;
  return Math.floor(rawValue);
});

const activeCategoryName = computed(() => {
  if (!activeCategoryId.value) return "";
  return categories.value.find((category) => category.maDanhMuc === activeCategoryId.value)?.tenDanhMuc || "";
});
const activeColorName = computed(() => {
  if (!activeColorId.value) return "";
  return colors.value.find((color) => color.maMau === activeColorId.value)?.tenMau || "";
});
const activeSizeName = computed(() => {
  if (!activeSizeId.value) return "";
  return sizes.value.find((size) => size.maSize === activeSizeId.value)?.tenSize || "";
});

const currentPriceParams = computed(() => {
  if (priceKey.value === "under-200") {
    return { maxPrice: 200000 };
  }

  if (priceKey.value === "200-500") {
    return { minPrice: 200000, maxPrice: 500000 };
  }

  if (priceKey.value === "500-1000") {
    return { minPrice: 500000, maxPrice: 1000000 };
  }

  if (priceKey.value === "over-1000") {
    return { minPrice: 1000000 };
  }

  return {};
});

const currentVariantParams = computed(() => {
  const filters: { maKichThuoc?: number; maMauSac?: number } = {};

  if (activeSizeId.value) {
    filters.maKichThuoc = activeSizeId.value;
  }

  if (activeColorId.value) {
    filters.maMauSac = activeColorId.value;
  }

  return filters;
});

const currentSortParams = computed(() =>
  sortValue.value !== "default" ? { sort: sortValue.value } : {},
);

const pageTitle = computed(() => {
  if (activeSearchQuery.value) {
  return `Kết quả tìm kiếm: "${activeSearchQuery.value}"`;
  }
  if (activeCategoryName.value) {
    return activeCategoryName.value;
  }
  return "Tất cả sản phẩm";
});

const processedProducts = computed(() => {
  let items = [...products.value];

  if (sortValue.value === "price-asc") {
    items.sort((a, b) => (a.giaKm || a.giaGoc) - (b.giaKm || b.giaGoc));
  } else if (sortValue.value === "price-desc") {
    items.sort((a, b) => (b.giaKm || b.giaGoc) - (a.giaKm || a.giaGoc));
  } else if (sortValue.value === "name-asc") {
    items.sort((a, b) => a.tenSp.localeCompare(b.tenSp));
  }
  return items;
});

const totalFilteredItems = computed(() =>
  useClientAggregation.value ? processedProducts.value.length : totalItems.value,
);

const totalPages = computed(() =>
  useClientAggregation.value
    ? Math.max(1, Math.ceil(totalFilteredItems.value / PAGE_SIZE))
    : Math.max(1, serverTotalPages.value),
);

const safePage = computed(() => Math.min(currentPage.value, totalPages.value));

const paginatedProducts = computed(() => {
  if (!useClientAggregation.value) {
    return products.value;
  }

  const start = (safePage.value - 1) * PAGE_SIZE;
  return processedProducts.value.slice(start, start + PAGE_SIZE);
});

const visiblePages = computed(() => {
  const pages: number[] = [];
  const start = Math.max(1, safePage.value - 2);
  const end = Math.min(totalPages.value, safePage.value + 2);
  for (let value = start; value <= end; value += 1) {
    pages.push(value);
  }
  return pages;
});

function pageButtonClass(value: number) {
  return [
    "inline-flex h-11 w-11 items-center justify-center rounded-full text-sm font-semibold transition",
    value === safePage.value
      ? "bg-slate-950 text-white"
      : "border border-[var(--ui-border)] bg-white text-slate-700 hover:bg-slate-50",
  ].join(" ");
}

function formatMoney(value: number) {
  return `${(value || 0).toLocaleString("vi-VN")} đ`;
}

function getChildren(parentId: number) {
  return categories.value.filter((category) => category.maDanhMucCha === parentId);
}

async function loadCategories() {
  const response = await danhMucService.getAll();
  categories.value = response.data ?? [];
}

async function loadVariantFilters() {
  try {
    const [sizeResponse, colorResponse] = await Promise.all([
      mauSizeService.getAllSize(),
      mauSizeService.getAllMau(),
    ]);

    sizes.value = sizeResponse.data ?? [];
    colors.value = colorResponse.data ?? [];
  } catch {
    sizes.value = [];
    colors.value = [];
  }
}

async function fetchProductPage(page: number, pageSize: number) {
  const filters = {
    ...currentPriceParams.value,
    ...currentVariantParams.value,
    ...currentSortParams.value,
  };

  if (activeSearchQuery.value) {
    return sanPhamService.searchByName(activeSearchQuery.value, page, pageSize, filters);
  }

  if (activeCategoryId.value) {
    return sanPhamService.getByCategory(activeCategoryId.value, page, pageSize, filters);
  }

  return sanPhamService.getAll(page, pageSize, filters);
}

async function loadAllMatchingProducts() {
  const firstResponse = await fetchProductPage(1, FETCH_BATCH_SIZE);
  const firstPage = firstResponse.data;
  const mergedItems = [...getPageItems(firstPage)];
  const totalPageCount = getTotalPages(firstPage);

  for (let page = 2; page <= totalPageCount; page += 1) {
    const response = await fetchProductPage(page, FETCH_BATCH_SIZE);
    mergedItems.push(...getPageItems(response.data));
  }

  return {
    items: mergedItems,
    totalItemCount: getTotalItems(firstPage),
  };
}

async function loadProducts() {
  loading.value = true;
  errorMessage.value = "";
  try {
    if (useClientAggregation.value) {
      const { items, totalItemCount } = await loadAllMatchingProducts();
      products.value = items;
      totalItems.value = totalItemCount;
      serverTotalPages.value = Math.max(1, Math.ceil(totalItemCount / PAGE_SIZE));
      return;
    }

    const response = await fetchProductPage(currentPage.value, PAGE_SIZE);
    const page = response.data;

    products.value = getPageItems(page);
    totalItems.value = getTotalItems(page);
    serverTotalPages.value = getTotalPages(page);
  } catch (caughtError) {
    errorMessage.value = extractErrorMessage(
      caughtError,
      "Không thể tải dữ liệu sản phẩm.",
    );
    products.value = [];
    totalItems.value = 0;
    serverTotalPages.value = 1;
  } finally {
    loading.value = false;
  }
}

function pushRoute(next: {
  maDM?: number | null;
  q?: string;
  page?: number;
  sort?: string;
  price?: string;
  mau?: number | null;
  size?: number | null;
}) {
  const maDM = Object.prototype.hasOwnProperty.call(next, "maDM")
    ? next.maDM ?? null
    : currentCategoryParam.value;
  const q = next.q ?? activeSearchQuery.value;
  const page = next.page ?? 1;
  const sort = next.sort ?? sortValue.value;
  const price = next.price ?? priceKey.value;
  const mau = Object.prototype.hasOwnProperty.call(next, "mau")
    ? next.mau ?? null
    : activeColorId.value;
  const size = Object.prototype.hasOwnProperty.call(next, "size")
    ? next.size ?? null
    : activeSizeId.value;

  const query: Record<string, string> = {};
  if (q) query.q = q;
  if (page > 1) query.page = String(page);
  if (sort && sort !== "default") query.sort = sort;
  if (price) query.price = price;
  if (mau) query.mau = String(mau);
  if (size) query.size = String(size);

  if (maDM) {
    void router.push({
      name: "category-products",
      params: { maDM: String(maDM) },
      query,
    });
  } else {
    void router.push({
      name: "product-search",
      query,
    });
  }
}

function doSearch() {
  const rawValue = searchInput.value;
  const q = searchInput.value.trim();
  if (!q && rawValue.length > 0) {
    errorMessage.value = "Vui lòng nhập từ khóa tìm kiếm hợp lệ, không chỉ nhập khoảng trắng.";
    return;
  }

  errorMessage.value = "";
  pushRoute({
    maDM: null,
    q,
    page: 1,
  });
}

function selectCategory(maDM: number | null) {
  pushRoute({
    maDM,
    q: "",
    page: 1,
  });
}

function selectCategoryAndClose(maDM: number | null) {
  filterOpen.value = false;
  selectCategory(maDM);
}

function togglePrice(nextPriceKey: string) {
  pushRoute({
    page: 1,
    price: nextPriceKey,
  });
}

function togglePriceAndClose(nextPriceKey: string) {
  filterOpen.value = false;
  togglePrice(nextPriceKey);
}

function toggleColor(nextColorId: number | null) {
  pushRoute({
    page: 1,
    mau: nextColorId,
  });
}

function toggleColorAndClose(nextColorId: number | null) {
  filterOpen.value = false;
  toggleColor(nextColorId);
}

function toggleSize(nextSizeId: number | null) {
  pushRoute({
    page: 1,
    size: nextSizeId,
  });
}

function toggleSizeAndClose(nextSizeId: number | null) {
  filterOpen.value = false;
  toggleSize(nextSizeId);
}

function changeSort() {
  pushRoute({
    page: 1,
    sort: sortValue.value,
  });
}

function goPage(value: number) {
  if (value < 1 || value > totalPages.value) return;
  pushRoute({ page: value });
  window.scrollTo({ top: 0, behavior: "smooth" });
}

watch(
  () => route.fullPath,
  async () => {
    searchInput.value = activeSearchQuery.value;
    const routeSort =
      typeof route.query.sort === "string" ? route.query.sort : "default";
    sortValue.value = ["default", "price-asc", "price-desc", "name-asc"].includes(routeSort)
      ? routeSort
      : "default";

    await loadProducts();
  },
);

watch(
  safePage,
  (value) => {
    if (value !== currentPage.value) {
      pushRoute({ page: value });
    }
  },
);

onMounted(async () => {
  searchInput.value = activeSearchQuery.value;
  const routeSort =
    typeof route.query.sort === "string" ? route.query.sort : "default";
  sortValue.value = ["default", "price-asc", "price-desc", "name-asc"].includes(routeSort)
    ? routeSort
    : "default";

  await Promise.all([loadCategories(), loadVariantFilters(), loadProducts()]);
});
</script>

<style scoped>
.drawer-fade-enter-active,
.drawer-fade-leave-active {
  transition: opacity 0.2s ease;
}

.drawer-fade-enter-from,
.drawer-fade-leave-to {
  opacity: 0;
}
</style>
