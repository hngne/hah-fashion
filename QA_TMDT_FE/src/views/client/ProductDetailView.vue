<template>
  <div class="min-h-screen bg-[var(--bg-page)]">
    <div v-if="loading" class="flex items-center justify-center py-28">
      <Loader2Icon class="h-8 w-8 animate-spin text-[var(--accent)]" />
    </div>

    <div v-else-if="errorMessage" class="mx-auto max-w-[1400px] px-4 py-20 sm:px-6 lg:px-8">
      <div class="rounded-3xl border border-rose-200 bg-rose-50 px-4 py-6 text-sm font-medium text-rose-700">
        {{ errorMessage }}
      </div>
    </div>

    <div v-else-if="!product" class="mx-auto max-w-[1400px] px-4 py-24 text-center sm:px-6 lg:px-8">
      <PackageXIcon class="mx-auto h-16 w-16 text-slate-200" />
      <h2 class="mt-4 text-xl font-semibold text-slate-700">Sản phẩm không tồn tại</h2>
      <RouterLink
        to="/search"
        class="mt-6 inline-flex items-center rounded-full bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800"
      >
        Quay lại danh sách
      </RouterLink>
    </div>

    <div v-else>
      <section class="mx-auto max-w-[1400px] px-4 py-5 sm:px-6 lg:px-8">
        <nav class="flex items-center gap-2 text-sm text-slate-400">
          <RouterLink to="/" class="transition hover:text-[var(--accent)]">Trang chủ</RouterLink>
          <ChevronRightIcon class="h-3.5 w-3.5" />
          <RouterLink
            v-if="product.maDanhMuc"
            :to="{ name: 'category-products', params: { maDM: String(product.maDanhMuc) } }"
            class="transition hover:text-[var(--accent)]"
          >
            {{ product.tenDanhMuc }}
          </RouterLink>
          <span v-else class="text-slate-500">{{ product.tenDanhMuc || "Sản phẩm" }}</span>
          <ChevronRightIcon class="h-3.5 w-3.5" />
          <span class="max-w-[220px] truncate font-medium text-slate-700">{{ product.tenSp }}</span>
        </nav>
      </section>

      <section class="mx-auto max-w-[1400px] px-4 pb-16 sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 gap-8 lg:grid-cols-[minmax(0,1.05fr)_minmax(0,0.95fr)]">
          <div class="space-y-4">
            <div class="overflow-hidden rounded-[32px] border border-[var(--ui-border)] bg-white shadow-[var(--shadow-soft)]">
              <img
                :src="selectedImage || product.anhDaiDien || placeholderImage"
                :alt="product.tenSp"
                class="aspect-[4/5] h-full w-full object-cover transition duration-500"
              />
            </div>

            <div v-if="allImages.length > 1" class="flex gap-3 overflow-x-auto pb-1">
              <button
                v-for="image in allImages"
                :key="image"
                type="button"
                class="overflow-hidden rounded-2xl border border-[var(--ui-border)] bg-white"
                @click="selectedImage = image"
              >
                <img :src="image" class="h-20 w-20 object-cover" />
              </button>
            </div>
          </div>

          <div class="space-y-6">
            <div>
              <span
                v-if="product.tenDanhMuc"
                class="inline-flex rounded-full bg-[var(--accent-soft)] px-3 py-1 text-xs font-semibold uppercase tracking-[0.16em] text-[var(--accent)]"
              >
                {{ product.tenDanhMuc }}
              </span>
              <h1 class="mt-3 text-3xl font-black leading-tight tracking-tight text-slate-950">
                {{ product.tenSp }}
              </h1>
              <div class="mt-4 flex flex-wrap items-end gap-3">
                <span class="text-3xl font-black text-rose-600">{{ formatMoney(product.giaKm) }}</span>
                <span
                  v-if="product.giaGoc > product.giaKm"
                  class="text-lg font-medium text-slate-400 line-through"
                >
                  {{ formatMoney(product.giaGoc) }}
                </span>
              </div>
              <p v-if="product.chatLieu" class="mt-3 text-sm text-slate-500">
                Chất liệu: <span class="font-medium text-slate-700">{{ product.chatLieu }}</span>
              </p>
            </div>

            <div class="rounded-3xl border border-[var(--ui-border)] bg-white p-5 shadow-[var(--shadow-soft)]">
              <div v-if="availableColors.length" class="mb-5">
                <h3 class="mb-3 text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">Màu sắc</h3>
                <div class="flex flex-wrap gap-2">
                  <button
                    v-for="color in availableColors"
                    :key="color"
                    type="button"
                    :class="chipClass(selectedColor === color)"
                    @click="selectedColor = color"
                  >
                    {{ color }}
                  </button>
                </div>
              </div>

              <div v-if="availableSizes.length" class="mb-5">
                <h3 class="mb-3 text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">Kích cỡ</h3>
                <div class="flex flex-wrap gap-2">
                  <button
                    v-for="size in availableSizes"
                    :key="size.name"
                    type="button"
                    :disabled="!size.inStock"
                    :class="sizeClass(size.name, size.inStock)"
                    @click="selectedSize = size.name"
                  >
                    {{ size.name }}
                  </button>
                </div>
              </div>

              <div v-if="selectedVariant" class="mb-5 rounded-2xl bg-slate-50 px-4 py-3 text-sm">
                <p :class="(selectedVariant.soLuongTon ?? 0) > 0 ? 'text-emerald-600' : 'text-rose-600'">
                  {{ (selectedVariant.soLuongTon ?? 0) > 0 ? `Còn ${selectedVariant.soLuongTon ?? 0} sản phẩm` : "Tạm hết hàng" }}
                </p>
              </div>

              <div class="flex flex-col gap-4 sm:flex-row">
                <div class="inline-flex items-center rounded-2xl border border-[var(--ui-border)] bg-white">
                  <button
                    type="button"
                    class="inline-flex h-12 w-12 items-center justify-center text-slate-500 transition hover:bg-slate-50"
                    @click="decreaseQuantity"
                  >
                    <MinusIcon class="h-4 w-4" />
                  </button>
                  <span class="inline-flex min-w-[56px] justify-center text-sm font-semibold text-slate-900">
                    {{ quantity }}
                  </span>
                  <button
                    type="button"
                    class="inline-flex h-12 w-12 items-center justify-center text-slate-500 transition hover:bg-slate-50"
                    :disabled="maxQuantityReached"
                    @click="increaseQuantity"
                  >
                    <PlusIcon class="h-4 w-4" />
                  </button>
                </div>

                <button
                  type="button"
                  :disabled="!canAddToCart"
                  class="inline-flex flex-1 items-center justify-center gap-2 rounded-2xl bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:bg-slate-200 disabled:text-slate-400"
                  @click="addToCart"
                >
                  <ShoppingBagIcon class="h-5 w-5" />
                  Thêm vào giỏ
                </button>
              </div>

              <p v-if="feedbackMessage" :class="feedbackClass" class="mt-4 rounded-2xl px-4 py-3 text-sm font-medium">
                {{ feedbackMessage }}
              </p>
            </div>

            <div class="grid grid-cols-1 gap-3 sm:grid-cols-3">
              <div class="rounded-3xl border border-[var(--ui-border)] bg-white p-4 text-center shadow-[var(--shadow-soft)]">
                <TruckIcon class="mx-auto h-5 w-5 text-[var(--accent)]" />
                <p class="mt-2 text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Vận chuyển</p>
                <p class="mt-1 text-sm font-medium text-slate-700">Miễn phí từ 500k</p>
              </div>
              <div class="rounded-3xl border border-[var(--ui-border)] bg-white p-4 text-center shadow-[var(--shadow-soft)]">
                <ShieldCheckIcon class="mx-auto h-5 w-5 text-emerald-500" />
                <p class="mt-2 text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Đảm bảo</p>
                <p class="mt-1 text-sm font-medium text-slate-700">Hàng chính hãng</p>
              </div>
              <div class="rounded-3xl border border-[var(--ui-border)] bg-white p-4 text-center shadow-[var(--shadow-soft)]">
                <RotateCcwIcon class="mx-auto h-5 w-5 text-orange-500" />
                <p class="mt-2 text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">Hỗ trợ</p>
                <p class="mt-1 text-sm font-medium text-slate-700">Đổi trả 30 ngày</p>
              </div>
            </div>
          </div>
        </div>

        <section class="mt-14 rounded-[32px] border border-[var(--ui-border)] bg-white p-6 shadow-[var(--shadow-soft)] sm:p-8">
          <div class="flex flex-wrap items-center gap-2 border-b border-slate-100 pb-3">
            <button
              v-for="tab in tabs"
              :key="tab"
              type="button"
              :class="tabClass(tab)"
              @click="activeTab = tab"
            >
              {{ tab }}
            </button>
          </div>

          <div class="pt-6">
            <div v-if="activeTab === 'Mô tả'" class="whitespace-pre-line text-sm leading-7 text-slate-600">
              {{ product.moTa || "Sản phẩm chưa có mô tả chi tiết." }}
            </div>
            <div v-else class="space-y-3 text-sm leading-7 text-slate-600">
              <p>Hỗ trợ đổi trả trong 30 ngày nếu sản phẩm còn nguyên trạng thái.</p>
              <p>Hoàn tiền 100% nếu lỗi do nhà sản xuất.</p>
              <p>Vui lòng giữ tem, nhãn và hóa đơn để được xử lý nhanh nhất.</p>
            </div>
          </div>
        </section>

        <section v-if="relatedProducts.length" class="mt-14">
          <div class="mb-5 flex items-center justify-between">
            <div>
              <h2 class="text-2xl font-black tracking-tight text-slate-950">Sản phẩm tương tự</h2>
              <p class="mt-1 text-sm text-slate-500">Cùng danh mục và đang được quan tâm.</p>
            </div>
            <RouterLink
              v-if="product.maDanhMuc"
              :to="{ name: 'category-products', params: { maDM: String(product.maDanhMuc) } }"
              class="text-sm font-semibold text-[var(--accent)] transition hover:opacity-80"
            >
              Xem thêm
            </RouterLink>
          </div>

          <div class="grid grid-cols-2 gap-4 sm:grid-cols-3 xl:grid-cols-4">
            <RouterLink
              v-for="related in relatedProducts"
              :key="related.maSp"
              :to="{ name: 'product-detail', params: { maSP: related.maSp } }"
              class="group overflow-hidden rounded-3xl border border-[var(--ui-border)] bg-white p-3 shadow-[var(--shadow-soft)] transition hover:-translate-y-0.5 hover:shadow-[var(--shadow-card)]"
            >
              <div class="overflow-hidden rounded-2xl bg-slate-100">
                <img
                  :src="related.anhDaiDien || placeholderImage"
                  :alt="related.tenSp"
                  class="aspect-[3/4] h-full w-full object-cover transition duration-500 group-hover:scale-105"
                />
              </div>
              <p class="mt-3 text-sm font-semibold text-slate-900 line-clamp-2 transition group-hover:text-[var(--accent)]">
                {{ related.tenSp }}
              </p>
              <div class="mt-2 flex flex-wrap items-center gap-2">
                <span class="text-sm font-bold text-rose-600">{{ formatMoney(related.giaKm) }}</span>
                <span
                  v-if="related.giaGoc > related.giaKm"
                  class="text-xs text-slate-400 line-through"
                >
                  {{ formatMoney(related.giaGoc) }}
                </span>
              </div>
            </RouterLink>
          </div>
        </section>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import { RouterLink, useRoute } from "vue-router";
import {
  ChevronRight as ChevronRightIcon,
  Loader2 as Loader2Icon,
  Minus as MinusIcon,
  PackageX as PackageXIcon,
  Plus as PlusIcon,
  RotateCcw as RotateCcwIcon,
  ShieldCheck as ShieldCheckIcon,
  ShoppingBag as ShoppingBagIcon,
  Truck as TruckIcon,
} from "lucide-vue-next";
import { useCartStore } from "../../stores/cart";
import type { GuestCartItem } from "../../stores/cart";
import { getPageItems } from "../../services/service-helpers";
import { sanPhamService } from "../../services/sanpham.service";
import type { ProductDetail, ProductSummary } from "../../types";

const route = useRoute();
const cartStore = useCartStore();
const props = defineProps<{ maSP?: string }>();

const placeholderImage = "https://via.placeholder.com/600x800?text=No+Image";
const tabs = ["Mô tả", "Chính sách"];

const product = ref<ProductDetail | null>(null);
const relatedProducts = ref<ProductSummary[]>([]);
const loading = ref(true);
const errorMessage = ref("");
const selectedImage = ref("");
const selectedColor = ref("");
const selectedSize = ref("");
const quantity = ref(1);
const activeTab = ref("Mô tả");
const feedbackMessage = ref("");
const feedbackSuccess = ref(true);

const allImages = computed(() => {
  if (!product.value) return [];
  const images = [
    product.value.anhDaiDien,
    ...(product.value.duongDanAnhSPs || []),
  ].filter(Boolean) as string[];
  return Array.from(new Set(images));
});

const availableColors = computed(() => {
  if (!product.value?.dsBienThe?.length) return [];
  return Array.from(new Set(product.value.dsBienThe.map((variant) => variant.tenMau))).filter(Boolean);
});

const availableSizes = computed(() => {
  if (!product.value?.dsBienThe?.length) return [];
  const variants = selectedColor.value
    ? product.value.dsBienThe.filter((variant) => variant.tenMau === selectedColor.value)
    : product.value.dsBienThe;

  const sizeMap = new Map<string, boolean>();
  for (const variant of variants) {
    const current = sizeMap.get(variant.tenSize);
    const inStock = (variant.soLuongTon ?? 0) > 0;
    if (current == null) {
      sizeMap.set(variant.tenSize, inStock);
    } else if (inStock) {
      sizeMap.set(variant.tenSize, true);
    }
  }

  return Array.from(sizeMap.entries()).map(([name, inStock]) => ({
    name,
    inStock,
  }));
});

const selectedVariant = computed(() => {
  if (!product.value?.dsBienThe?.length || !selectedColor.value || !selectedSize.value) {
    return null;
  }

  return (
    product.value.dsBienThe.find(
      (variant) =>
        variant.tenMau === selectedColor.value && variant.tenSize === selectedSize.value,
    ) || null
  );
});

const canAddToCart = computed(
  () => !!selectedVariant.value && (selectedVariant.value.soLuongTon ?? 0) > 0,
);

const maxQuantityReached = computed(
  () => quantity.value >= (selectedVariant.value?.soLuongTon ?? 0),
);

const feedbackClass = computed(() =>
  feedbackSuccess.value
    ? "bg-emerald-50 text-emerald-700"
    : "bg-rose-50 text-rose-700",
);

function formatMoney(value: number) {
  return `${(value || 0).toLocaleString("vi-VN")} đ`;
}

function chipClass(active: boolean) {
  return [
    "rounded-full px-4 py-2 text-sm font-semibold transition",
    active
      ? "bg-slate-950 text-white"
      : "border border-[var(--ui-border)] bg-white text-slate-700 hover:bg-slate-50",
  ].join(" ");
}

function sizeClass(sizeName: string, inStock: boolean) {
  return [
    "inline-flex h-12 min-w-[52px] items-center justify-center rounded-2xl border px-4 text-sm font-semibold transition",
    !inStock
      ? "cursor-not-allowed border-slate-200 bg-slate-50 text-slate-300"
      : selectedSize.value === sizeName
        ? "border-slate-950 bg-slate-950 text-white"
        : "border-[var(--ui-border)] bg-white text-slate-700 hover:bg-slate-50",
  ].join(" ");
}

function tabClass(tab: string) {
  return [
    "rounded-full px-4 py-2 text-sm font-semibold transition",
    activeTab.value === tab
      ? "bg-slate-950 text-white"
      : "text-slate-500 hover:bg-slate-100 hover:text-slate-900",
  ].join(" ");
}

function showFeedback(message: string, isSuccess: boolean) {
  feedbackMessage.value = message;
  feedbackSuccess.value = isSuccess;
  window.setTimeout(() => {
    feedbackMessage.value = "";
  }, 3000);
}

function decreaseQuantity() {
  quantity.value = Math.max(1, quantity.value - 1);
}

function increaseQuantity() {
  const stock = selectedVariant.value?.soLuongTon ?? 0;
  quantity.value = Math.min(stock, quantity.value + 1);
}

async function loadProduct() {
  loading.value = true;
  errorMessage.value = "";

  try {
    const maSP = props.maSP || (route.params.maSP as string);
    if (!maSP) {
      product.value = null;
      return;
    }

    const response = await sanPhamService.getFullInfo(maSP);
    product.value = response.data;
    selectedImage.value = response.data.anhDaiDien || response.data.duongDanAnhSPs?.[0] || "";

    if (response.data.maDanhMuc) {
      const relatedResponse = await sanPhamService.getByCategory(response.data.maDanhMuc, 1, 20);
      relatedProducts.value = getPageItems(relatedResponse.data)
        .filter((item) => item.maSp !== maSP)
        .slice(0, 8);
    } else {
      relatedProducts.value = [];
    }
  } catch (caughtError) {
    errorMessage.value = "Không thể tải chi tiết sản phẩm.";
    product.value = null;
  } finally {
    loading.value = false;
  }
}

async function addToCart() {
  if (!product.value || !selectedVariant.value) {
    showFeedback("Vui lòng chọn đầy đủ màu sắc và kích cỡ.", false);
    return;
  }

  const stock = selectedVariant.value.soLuongTon ?? 0;
  if (stock <= 0) {
    showFeedback("Biến thể này đang hết hàng.", false);
    return;
  }

  quantity.value = Math.min(quantity.value, stock);

  const payload: GuestCartItem = {
    maCTSP: selectedVariant.value.maChiTietSP,
    maSP: product.value.maSp,
    tenSP: product.value.tenSp,
    anh: product.value.anhDaiDien || "",
    tenMau: selectedColor.value,
    tenSize: selectedSize.value,
    gia: product.value.giaKm || product.value.giaGoc,
    soLuong: quantity.value,
  };

  try {
    await cartStore.addToCart(payload);
    showFeedback("Đã thêm sản phẩm vào giỏ hàng.", true);
  } catch {
    showFeedback("Không thể thêm vào giỏ hàng. Vui lòng thử lại.", false);
  }
}

watch(
  availableColors,
  (colors) => {
    if (colors.length && !colors.includes(selectedColor.value)) {
      selectedColor.value = colors[0];
    }
  },
  { immediate: true },
);

watch(selectedColor, () => {
  selectedSize.value = "";
});

watch(
  availableSizes,
  (sizes) => {
    const firstInStock = sizes.find((size) => size.inStock);
    if (!selectedSize.value || !sizes.some((size) => size.name === selectedSize.value)) {
      selectedSize.value = firstInStock?.name || sizes[0]?.name || "";
    }
  },
  { immediate: true },
);

watch(
  selectedVariant,
  (variant) => {
    const stock = variant?.soLuongTon ?? 1;
    quantity.value = Math.min(Math.max(quantity.value, 1), Math.max(stock, 1));
  },
  { immediate: true },
);

watch(
  () => route.fullPath,
  () => {
    quantity.value = 1;
    activeTab.value = "Mô tả";
    void loadProduct();
  },
);

onMounted(() => {
  void loadProduct();
});
</script>
