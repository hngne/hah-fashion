<template>
  <div>
    <Toast />

    <div class="mb-6 flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
      <div>
        <h3 class="text-xl font-bold text-slate-900">Quản lý sản phẩm</h3>
        <p class="mt-1 text-sm text-slate-500">
          Tìm theo tên hoặc mã, xem nhanh chi tiết và điều hướng tới form sửa.
        </p>
      </div>

      <router-link
        to="/admin/products/new"
        class="inline-flex items-center justify-center gap-2 rounded-full bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800"
      >
        <PlusIcon class="h-4 w-4" />
        Thêm sản phẩm
      </router-link>
    </div>

    <section class="rounded-3xl border border-[var(--ui-border)] bg-white p-4 shadow-[var(--shadow-soft)]">
      <div class="grid grid-cols-1 gap-3 lg:grid-cols-[auto_minmax(0,1fr)]">
        <div class="inline-flex rounded-full border border-[var(--ui-border)] bg-slate-50 p-1">
          <button
            type="button"
            @click="searchType = 'name'"
            :class="segmentClass(searchType === 'name')"
          >
            Tên sản phẩm
          </button>
          <button
            type="button"
            @click="searchType = 'code'"
            :class="segmentClass(searchType === 'code')"
          >
            Mã sản phẩm
          </button>
        </div>

        <div class="relative">
          <SearchIcon class="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
          <input
            v-model="searchQuery"
            type="text"
            :placeholder="searchType === 'name' ? 'Nhập tên sản phẩm...' : 'Nhập mã sản phẩm...'"
            class="w-full rounded-2xl border border-[var(--ui-border)] bg-white py-3 pl-11 pr-4 text-sm text-slate-900 outline-none transition focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]"
            @input="debouncedSearch"
          />
        </div>
      </div>
    </section>

    <div v-if="loading" class="flex justify-center py-16 text-slate-400">
      <Loader2Icon class="h-8 w-8 animate-spin" />
    </div>

    <div
      v-else-if="errorMessage"
      class="mt-6 rounded-3xl border border-rose-200 bg-rose-50 px-4 py-5 text-sm font-medium text-rose-700"
    >
      {{ errorMessage }}
    </div>

    <div
      v-else-if="!items.length"
      class="mt-6 rounded-3xl border border-dashed border-[var(--ui-border)] bg-white px-4 py-14 text-center text-sm text-slate-500"
    >
      Không tìm thấy sản phẩm phù hợp.
    </div>

    <section
      v-else
      class="mt-6 overflow-hidden rounded-3xl border border-[var(--ui-border)] bg-white shadow-[var(--shadow-soft)]"
    >
      <div class="hidden overflow-x-auto lg:block">
        <table class="min-w-full">
          <thead class="bg-slate-50 text-left text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
            <tr>
              <th class="px-5 py-4">Ảnh</th>
              <th class="px-5 py-4">Sản phẩm</th>
              <th class="px-5 py-4">Danh mục</th>
              <th class="px-5 py-4">Giá gốc</th>
              <th class="px-5 py-4">Biến thể</th>
              <th class="px-5 py-4 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100">
            <tr
              v-for="item in items"
              :key="item.maSp"
              class="transition hover:bg-slate-50/70"
            >
              <td class="px-5 py-4">
                <div class="h-12 w-12 overflow-hidden rounded-2xl bg-slate-100">
                  <img
                    v-if="item.anhDaiDien"
                    :src="item.anhDaiDien"
                    class="h-full w-full object-cover"
                  />
                  <PackageIcon v-else class="m-3 h-6 w-6 text-slate-300" />
                </div>
              </td>
              <td class="px-5 py-4">
                <button
                  type="button"
                  class="text-left"
                  @click="openDetail(item.maSp)"
                >
                  <p class="text-sm font-semibold text-slate-900">{{ item.tenSp }}</p>
                  <p class="mt-1 text-xs font-medium text-slate-400">{{ item.maSp }}</p>
                </button>
              </td>
              <td class="px-5 py-4 text-sm text-slate-600">{{ item.tenDanhMuc || "-" }}</td>
              <td class="px-5 py-4 text-sm font-semibold text-slate-900">{{ formatMoney(item.giaGoc) }}</td>
              <td class="px-5 py-4">
                <span class="rounded-full bg-slate-100 px-2.5 py-1 text-xs font-semibold text-slate-700">
                  {{ variantCount(item) }}
                </span>
              </td>
              <td class="px-5 py-4">
                <div class="flex justify-end gap-2">
                  <button
                    type="button"
                    @click="openDetail(item.maSp)"
                    class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-[var(--accent)] hover:text-[var(--accent)]"
                  >
                    <EyeIcon class="h-4 w-4" />
                  </button>
                  <router-link
                    :to="`/admin/products/${item.maSp}/edit`"
                    class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-[var(--accent)] hover:text-[var(--accent)]"
                  >
                    <PencilIcon class="h-4 w-4" />
                  </router-link>
                  <button
                    type="button"
                    @click="confirmDelete(item)"
                    class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-rose-200 bg-white text-rose-500 transition hover:bg-rose-50"
                  >
                    <Trash2Icon class="h-4 w-4" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="divide-y divide-slate-100 lg:hidden">
        <div
          v-for="item in items"
          :key="item.maSp"
          class="space-y-3 p-4"
        >
          <div class="flex gap-3">
            <div class="h-16 w-16 overflow-hidden rounded-2xl bg-slate-100">
              <img
                v-if="item.anhDaiDien"
                :src="item.anhDaiDien"
                class="h-full w-full object-cover"
              />
              <PackageIcon v-else class="m-4 h-8 w-8 text-slate-300" />
            </div>
            <div class="min-w-0 flex-1">
              <button type="button" class="text-left" @click="openDetail(item.maSp)">
                <p class="text-sm font-semibold text-slate-900 line-clamp-2">{{ item.tenSp }}</p>
                <p class="mt-1 text-xs font-medium text-slate-400">{{ item.maSp }}</p>
              </button>
              <div class="mt-2 flex flex-wrap gap-2 text-xs text-slate-500">
                <span class="rounded-full bg-slate-100 px-2.5 py-1">{{ item.tenDanhMuc || "-" }}</span>
                <span class="rounded-full bg-slate-100 px-2.5 py-1">
                  {{ variantCount(item) }} biến thể
                </span>
              </div>
            </div>
          </div>

          <div class="flex items-center justify-between">
            <span class="text-sm font-semibold text-slate-900">{{ formatMoney(item.giaGoc) }}</span>
            <div class="flex gap-2">
              <button
                type="button"
                @click="openDetail(item.maSp)"
                class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-[var(--accent)] hover:text-[var(--accent)]"
              >
                <EyeIcon class="h-4 w-4" />
              </button>
              <router-link
                :to="`/admin/products/${item.maSp}/edit`"
                class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-[var(--accent)] hover:text-[var(--accent)]"
              >
                <PencilIcon class="h-4 w-4" />
              </router-link>
              <button
                type="button"
                @click="confirmDelete(item)"
                class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-rose-200 bg-white text-rose-500 transition hover:bg-rose-50"
              >
                <Trash2Icon class="h-4 w-4" />
              </button>
            </div>
          </div>
        </div>
      </div>

      <div
        v-if="totalPages > 1"
        class="flex flex-col gap-3 border-t border-slate-100 px-4 py-4 sm:flex-row sm:items-center sm:justify-between"
      >
        <p class="text-sm text-slate-500">Trang {{ page }} / {{ totalPages }} · {{ totalItems }} sản phẩm</p>
        <div class="flex flex-wrap items-center gap-2">
          <button
            type="button"
            :disabled="page <= 1"
            class="rounded-full border border-[var(--ui-border)] px-4 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-40"
            @click="goPage(page - 1)"
          >
            Trước
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
            :disabled="page >= totalPages"
            class="rounded-full border border-[var(--ui-border)] px-4 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-40"
            @click="goPage(page + 1)"
          >
            Sau
          </button>
        </div>
      </div>
    </section>

    <teleport to="body">
      <transition name="modal-fade">
        <div
          v-if="detailOpen"
          class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/55 px-4 py-8"
          @click.self="detailOpen = false"
        >
          <div class="max-h-[90vh] w-full max-w-4xl overflow-hidden rounded-3xl bg-white shadow-2xl">
            <div class="flex items-center justify-between border-b border-slate-100 px-5 py-4">
              <div>
                <h4 class="text-lg font-semibold text-slate-900">Chi tiết sản phẩm</h4>
                <p v-if="detailData" class="mt-1 text-sm text-slate-500">{{ detailData.maSp }}</p>
              </div>
              <button
                type="button"
                class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-slate-300 hover:text-slate-900"
                @click="detailOpen = false"
              >
                <XIcon class="h-4 w-4" />
              </button>
            </div>

            <div v-if="detailLoading" class="flex justify-center py-16 text-slate-400">
              <Loader2Icon class="h-8 w-8 animate-spin" />
            </div>

            <div v-else-if="detailData" class="max-h-[calc(90vh-88px)] overflow-y-auto px-5 py-5">
              <div class="grid grid-cols-1 gap-6 lg:grid-cols-[1.1fr_1fr]">
                <div class="space-y-3">
                  <div class="overflow-hidden rounded-3xl bg-slate-100">
                    <img
                      v-if="detailHeroImage"
                      :src="detailHeroImage"
                      class="aspect-[4/5] h-full w-full object-cover"
                    />
                    <div
                      v-else
                      class="flex aspect-[4/5] items-center justify-center text-sm font-medium text-slate-400"
                    >
                      Chưa có hình ảnh
                    </div>
                  </div>

                  <div
                    v-if="detailData.duongDanAnhSPs?.length"
                    class="flex gap-3 overflow-x-auto pb-1"
                  >
                    <button
                      v-for="image in detailData.duongDanAnhSPs"
                      :key="image"
                      type="button"
                      class="overflow-hidden rounded-2xl border border-[var(--ui-border)]"
                      @click="detailHeroImage = image"
                    >
                      <img :src="image" class="h-20 w-20 object-cover" />
                    </button>
                  </div>
                </div>

                <div>
                  <div class="grid grid-cols-1 gap-3 sm:grid-cols-2">
                    <div class="rounded-2xl border border-[var(--ui-border)] bg-slate-50 p-4">
                      <p class="text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Tên</p>
                      <p class="mt-2 text-sm font-semibold text-slate-900">{{ detailData.tenSp }}</p>
                    </div>
                    <div class="rounded-2xl border border-[var(--ui-border)] bg-slate-50 p-4">
                      <p class="text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Danh mục</p>
                      <p class="mt-2 text-sm font-semibold text-slate-900">{{ detailData.tenDanhMuc || "-" }}</p>
                    </div>
                    <div class="rounded-2xl border border-[var(--ui-border)] bg-slate-50 p-4">
                      <p class="text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Giá gốc</p>
                      <p class="mt-2 text-sm font-semibold text-slate-900">{{ formatMoney(detailData.giaGoc) }}</p>
                    </div>
                    <div class="rounded-2xl border border-[var(--ui-border)] bg-slate-50 p-4">
                      <p class="text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Giá KM</p>
                      <p class="mt-2 text-sm font-semibold text-emerald-600">{{ formatMoney(detailData.giaKm) }}</p>
                    </div>
                  </div>

                  <div class="mt-4 rounded-2xl border border-[var(--ui-border)] bg-slate-50 p-4">
                    <p class="text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Mô tả</p>
                    <p class="mt-2 whitespace-pre-line text-sm leading-6 text-slate-600">
                      {{ detailData.moTa || "Không có mô tả." }}
                    </p>
                  </div>

                  <div class="mt-4 rounded-2xl border border-[var(--ui-border)] bg-slate-50 p-4">
                    <div class="mb-3 flex items-center justify-between">
                      <p class="text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                        Biến thể
                      </p>
                      <span class="rounded-full bg-white px-2.5 py-1 text-xs font-semibold text-slate-600">
                        {{ detailData.dsBienThe.length }}
                      </span>
                    </div>
                    <div class="overflow-x-auto">
                      <table class="min-w-full text-sm">
                        <thead class="text-left text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                          <tr>
                            <th class="pb-2 pr-4">Size</th>
                            <th class="pb-2 pr-4">Màu</th>
                            <th class="pb-2 pr-4">Tồn</th>
                            <th class="pb-2">Giá bán</th>
                          </tr>
                        </thead>
                        <tbody class="divide-y divide-slate-200">
                          <tr v-for="variant in detailData.dsBienThe" :key="variant.maChiTietSP">
                            <td class="py-2 pr-4 font-medium text-slate-900">{{ variant.tenSize }}</td>
                            <td class="py-2 pr-4 text-slate-600">{{ variant.tenMau }}</td>
                            <td class="py-2 pr-4 text-slate-600">{{ variant.soLuongTon ?? 0 }}</td>
                            <td class="py-2 font-semibold text-slate-900">{{ formatMoney(variant.giaBan || 0) }}</td>
                          </tr>
                        </tbody>
                      </table>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </transition>
    </teleport>

    <teleport to="body">
      <transition name="modal-fade">
        <div
          v-if="deleteTarget"
          class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/55 px-4"
          @click.self="deleteTarget = null"
        >
          <div class="w-full max-w-sm rounded-3xl bg-white p-6 shadow-2xl">
            <h4 class="text-lg font-semibold text-slate-900">Xóa sản phẩm</h4>
            <p class="mt-2 text-sm text-slate-500">
              Bạn chắc chắn muốn xóa <strong class="text-slate-900">{{ deleteTarget.tenSp }}</strong>?
            </p>

            <div class="mt-6 flex gap-3">
              <button
                type="button"
                class="inline-flex flex-1 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white px-4 py-3 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
                @click="deleteTarget = null"
              >
                Hủy
              </button>
              <button
                type="button"
                :disabled="deleting"
                class="inline-flex flex-1 items-center justify-center gap-2 rounded-2xl bg-rose-600 px-4 py-3 text-sm font-semibold text-white transition hover:bg-rose-500 disabled:cursor-not-allowed disabled:opacity-60"
                @click="handleDelete"
              >
                <Loader2Icon v-if="deleting" class="h-4 w-4 animate-spin" />
                Xóa
              </button>
            </div>
          </div>
        </div>
      </transition>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import {
  Eye as EyeIcon,
  Loader2 as Loader2Icon,
  Package as PackageIcon,
  Pencil as PencilIcon,
  Plus as PlusIcon,
  Search as SearchIcon,
  Trash2 as Trash2Icon,
  X as XIcon,
} from "lucide-vue-next";
import Toast from "../../components/admin/Toast.vue";
import { useToast } from "../../composables/useToast";
import {
  extractErrorMessage,
  getPageItems,
  getTotalItems,
  getTotalPages,
  isNotFoundError,
} from "../../services/service-helpers";
import { sanPhamService } from "../../services/sanpham.service";
import type { ProductDetail, ProductSummary } from "../../types";

const { error, success } = useToast();

const items = ref<ProductSummary[]>([]);
const loading = ref(true);
const deleting = ref(false);
const errorMessage = ref("");
const searchQuery = ref("");
const searchType = ref<"name" | "code">("name");
const page = ref(1);
const totalItems = ref(0);
const totalPages = ref(1);
const deleteTarget = ref<ProductSummary | null>(null);
const detailOpen = ref(false);
const detailLoading = ref(false);
const detailData = ref<ProductDetail | null>(null);
const detailHeroImage = ref("");

let searchTimer: ReturnType<typeof setTimeout> | null = null;

const visiblePages = computed(() => {
  const pages: number[] = [];
  const start = Math.max(1, page.value - 2);
  const end = Math.min(totalPages.value, start + 4);
  for (let current = start; current <= end; current += 1) {
    pages.push(current);
  }
  return pages;
});

function segmentClass(active: boolean) {
  return [
    "rounded-full px-4 py-2 text-sm font-semibold transition",
    active ? "bg-white text-slate-900 shadow-sm" : "text-slate-500 hover:text-slate-900",
  ].join(" ");
}

function pageButtonClass(value: number) {
  return [
    "inline-flex h-10 w-10 items-center justify-center rounded-full text-sm font-semibold transition",
    value === page.value
      ? "bg-slate-950 text-white"
      : "border border-[var(--ui-border)] text-slate-700 hover:bg-slate-50",
  ].join(" ");
}

function formatMoney(value: number) {
  return `${(value || 0).toLocaleString("vi-VN")} đ`;
}

function variantCount(item: ProductSummary) {
  return item.soBienThe ?? item.dsBienThe?.length ?? 0;
}

async function loadProducts() {
  loading.value = true;
  errorMessage.value = "";

  try {
    const keyword = searchQuery.value.trim();
    if (keyword && searchType.value === "code") {
      const response = await sanPhamService.searchByCode(keyword);
      items.value = response.data ? [response.data] : [];
      totalItems.value = items.value.length;
      totalPages.value = 1;
      return;
    }

    const response = keyword
      ? await sanPhamService.searchByName(keyword, page.value, 10)
      : await sanPhamService.getAll(page.value, 10);
    const pageData = response.data;

    items.value = getPageItems(pageData);
    totalItems.value = getTotalItems(pageData);
    totalPages.value = getTotalPages(pageData);
  } catch (caughtError) {
    if (searchType.value === "code" && isNotFoundError(caughtError)) {
      items.value = [];
      totalItems.value = 0;
      totalPages.value = 1;
      return;
    }

    errorMessage.value = extractErrorMessage(
      caughtError,
      "Không thể tải danh sách sản phẩm.",
    );
  } finally {
    loading.value = false;
  }
}

function debouncedSearch() {
  page.value = 1;
  if (searchTimer) clearTimeout(searchTimer);
  searchTimer = setTimeout(() => {
    void loadProducts();
  }, 350);
}

function goPage(nextPage: number) {
  if (nextPage < 1 || nextPage > totalPages.value) return;
  page.value = nextPage;
  void loadProducts();
}

function confirmDelete(product: ProductSummary) {
  deleteTarget.value = product;
}

async function handleDelete() {
  if (!deleteTarget.value) return;

  deleting.value = true;
  try {
    await sanPhamService.delete(deleteTarget.value.maSp);
    success("Đã xóa sản phẩm.");
    deleteTarget.value = null;
    await loadProducts();
  } catch (caughtError) {
    error(extractErrorMessage(caughtError, "Không thể xóa sản phẩm."));
  } finally {
    deleting.value = false;
  }
}

async function openDetail(maSp: string) {
  detailOpen.value = true;
  detailLoading.value = true;
  detailData.value = null;

  try {
    const response = await sanPhamService.getFullInfo(maSp);
    detailData.value = response.data;
    detailHeroImage.value =
      response.data.duongDanAnhSPs?.[0] || response.data.anhDaiDien || "";
  } catch (caughtError) {
    error(extractErrorMessage(caughtError, "Không thể tải chi tiết sản phẩm."));
    detailOpen.value = false;
  } finally {
    detailLoading.value = false;
  }
}

onMounted(() => {
  void loadProducts();
});
</script>

<style scoped>
.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.2s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}
</style>
