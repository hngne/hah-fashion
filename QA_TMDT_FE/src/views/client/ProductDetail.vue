<template>
  <div class="min-h-screen bg-[#FAFAFA]">
    <!-- Loading -->
    <div v-if="loading" class="flex items-center justify-center py-32">
      <Loader2Icon class="h-8 w-8 animate-spin text-blue-500" />
    </div>

    <!-- Not Found -->
    <div v-else-if="!product" class="text-center py-32">
      <PackageXIcon class="h-16 w-16 mx-auto text-gray-300 mb-4" />
      <h2 class="text-xl font-bold text-gray-600">Sản phẩm không tồn tại</h2>
      <p class="text-gray-400 mt-2">Sản phẩm bạn tìm kiếm không có hoặc đã bị xóa.</p>
      <RouterLink to="/"
        class="inline-flex items-center mt-6 px-6 py-3 bg-blue-600 text-white font-semibold rounded-full hover:bg-blue-700 transition">
        <ArrowLeftIcon class="w-4 h-4 mr-2" /> Quay về trang chủ
      </RouterLink>
    </div>

    <!-- Product Detail -->
    <div v-else>
      <!-- Breadcrumb -->
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <nav class="flex items-center text-sm text-gray-400 space-x-2">
          <RouterLink to="/" class="hover:text-blue-600 transition">Trang chủ</RouterLink>
          <ChevronRightIcon class="w-3.5 h-3.5" />
          <RouterLink v-if="product.tenDanhMuc && categoryId"
            :to="{ name: 'category-products', params: { maDM: categoryId } }"
            class="hover:text-blue-600 transition">{{ product.tenDanhMuc }}</RouterLink>
          <span v-else-if="product.tenDanhMuc" class="text-gray-500">{{ product.tenDanhMuc }}</span>
          <ChevronRightIcon v-if="product.tenDanhMuc" class="w-3.5 h-3.5" />
          <span class="text-gray-700 font-medium truncate max-w-[200px]">{{ product.tenSp }}</span>
        </nav>
      </div>

      <!-- Main Content -->
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 pb-16">
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-8 lg:gap-12">

          <!-- ===== IMAGE GALLERY ===== -->
          <div class="space-y-4">
            <!-- Main Image -->
            <div class="relative overflow-hidden rounded-2xl bg-white aspect-[3/4] shadow-sm">
              <img :src="selectedImage || product.anhDaiDien || placeholderImg" :alt="product.tenSp"
                class="w-full h-full object-cover transition-transform duration-500"
                :class="{ 'scale-150': zoomed }"
                @click="zoomed = !zoomed"
                :style="zoomed ? `transform-origin: ${zoomOrigin}` : ''"
                @mousemove="handleZoomMove" />
              <!-- Sale Badge -->
              <span v-if="product.giaGoc && product.giaGoc > product.giaKm"
                class="absolute top-4 left-4 bg-red-500 text-white text-xs font-bold px-3 py-1 rounded-full shadow-lg">
                -{{ Math.round((1 - product.giaKm / product.giaGoc) * 100) }}%
              </span>
            </div>

            <!-- Thumbnails -->
            <div v-if="allImages.length > 1" class="flex space-x-3 overflow-x-auto pb-2 scrollbar-hide">
              <button v-for="(img, idx) in allImages" :key="idx" @click="selectedImage = img"
                :class="['flex-shrink-0 w-20 h-20 rounded-xl overflow-hidden border-2 transition-all duration-200',
                  selectedImage === img ? 'border-blue-500 shadow-md scale-105' : 'border-transparent hover:border-gray-300']">
                <img :src="img" :alt="`Ảnh ${idx + 1}`" class="w-full h-full object-cover" />
              </button>
            </div>
          </div>

          <!-- ===== PRODUCT INFO ===== -->
          <div class="space-y-6">
            <!-- Category tag -->
            <span v-if="product.tenDanhMuc"
              class="inline-block text-xs font-semibold tracking-wider uppercase text-blue-600 bg-blue-50 px-3 py-1 rounded-full">
              {{ product.tenDanhMuc }}
            </span>

            <!-- Product Name -->
            <h1 class="text-2xl sm:text-3xl font-black text-[#111827] leading-tight">{{ product.tenSp }}</h1>

            <!-- Price -->
            <div class="flex items-baseline space-x-3">
              <span class="text-3xl font-extrabold text-red-600">{{ formatPrice(product.giaKm) }}</span>
              <span v-if="product.giaGoc && product.giaGoc > product.giaKm"
                class="text-lg text-gray-400 line-through">{{ formatPrice(product.giaGoc) }}</span>
            </div>

            <!-- Material -->
            <div v-if="product.chatLieu" class="flex items-center text-sm text-gray-500">
              <RulerIcon class="w-4 h-4 mr-2 text-gray-400" />
              Chất liệu: <span class="font-medium text-gray-700 ml-1">{{ product.chatLieu }}</span>
            </div>

            <hr class="border-gray-100" />

            <!-- Color Selection -->
            <div v-if="availableColors.length">
              <h3 class="text-sm font-bold text-[#111827] mb-3">MÀU SẮC</h3>
              <div class="flex flex-wrap gap-2">
                <button v-for="color in availableColors" :key="color" @click="selectedColor = color"
                  :class="['px-4 py-2 rounded-full text-sm font-semibold border-2 transition-all duration-200',
                    selectedColor === color
                      ? 'border-blue-500 bg-blue-50 text-blue-700 shadow-sm'
                      : 'border-gray-200 text-gray-600 hover:border-gray-400 hover:bg-gray-50']">
                  {{ color }}
                </button>
              </div>
            </div>

            <!-- Size Selection -->
            <div v-if="availableSizes.length">
              <h3 class="text-sm font-bold text-[#111827] mb-3">KÍCH CỠ</h3>
              <div class="flex flex-wrap gap-2">
                <button v-for="size in availableSizes" :key="size.name" @click="selectedSize = size.name"
                  :disabled="!size.inStock"
                  :class="['w-14 h-14 rounded-xl text-sm font-bold border-2 transition-all duration-200',
                    !size.inStock
                      ? 'border-gray-100 text-gray-300 cursor-not-allowed bg-gray-50 line-through'
                      : selectedSize === size.name
                        ? 'border-blue-500 bg-blue-50 text-blue-700 shadow-sm'
                        : 'border-gray-200 text-gray-600 hover:border-gray-400 hover:bg-gray-50']">
                  {{ size.name }}
                </button>
              </div>
            </div>

            <!-- Stock Info -->
            <div v-if="selectedVariant" class="flex items-center text-sm">
              <CheckCircleIcon v-if="selectedVariant.soLuongTon > 0" class="w-4 h-4 text-green-500 mr-1.5" />
              <XCircleIcon v-else class="w-4 h-4 text-red-500 mr-1.5" />
              <span :class="selectedVariant.soLuongTon > 0 ? 'text-green-600' : 'text-red-600'">
                {{ selectedVariant.soLuongTon > 0 ? `Còn ${selectedVariant.soLuongTon} sản phẩm` : 'Hết hàng' }}
              </span>
            </div>

            <!-- Quantity + Add to Cart -->
            <div class="flex items-center space-x-4 pt-2">
              <div class="flex items-center border-2 border-gray-200 rounded-xl overflow-hidden">
                <button @click="quantity > 1 && quantity--"
                  class="w-11 h-11 flex items-center justify-center text-gray-500 hover:bg-gray-100 transition">
                  <MinusIcon class="w-4 h-4" />
                </button>
                <span class="w-12 text-center font-bold text-[#111827]">{{ quantity }}</span>
                <button @click="quantity++"
                  class="w-11 h-11 flex items-center justify-center text-gray-500 hover:bg-gray-100 transition">
                  <PlusIcon class="w-4 h-4" />
                </button>
              </div>
              <button @click="addToCart"
                :disabled="!selectedVariant || selectedVariant.soLuongTon <= 0"
                :class="['flex-1 h-12 rounded-xl font-bold text-sm transition-all duration-300 flex items-center justify-center space-x-2',
                  !selectedVariant || selectedVariant.soLuongTon <= 0
                    ? 'bg-gray-200 text-gray-400 cursor-not-allowed'
                    : 'bg-[#111827] text-white hover:bg-blue-600 hover:shadow-lg active:scale-[0.98]']">
                <ShoppingBagIcon class="w-5 h-5" />
                <span>THÊM VÀO GIỎ</span>
              </button>
            </div>

            <!-- Cart Success Message -->
            <transition name="fade">
              <div v-if="cartMessage"
                :class="['flex items-center space-x-2 px-4 py-3 rounded-xl text-sm font-medium',
                  cartSuccess ? 'bg-green-50 text-green-700' : 'bg-red-50 text-red-700']">
                <CheckCircleIcon v-if="cartSuccess" class="w-5 h-5" />
                <XCircleIcon v-else class="w-5 h-5" />
                <span>{{ cartMessage }}</span>
              </div>
            </transition>

            <hr class="border-gray-100" />

            <!-- Trust Badges -->
            <div class="grid grid-cols-3 gap-3">
              <div class="flex flex-col items-center text-center p-3 bg-white rounded-xl">
                <TruckIcon class="w-5 h-5 text-blue-500 mb-1.5" />
                <span class="text-[11px] font-semibold text-gray-600">Miễn phí giao hàng</span>
              </div>
              <div class="flex flex-col items-center text-center p-3 bg-white rounded-xl">
                <ShieldCheckIcon class="w-5 h-5 text-green-500 mb-1.5" />
                <span class="text-[11px] font-semibold text-gray-600">Hàng chính hãng</span>
              </div>
              <div class="flex flex-col items-center text-center p-3 bg-white rounded-xl">
                <RotateCcwIcon class="w-5 h-5 text-orange-500 mb-1.5" />
                <span class="text-[11px] font-semibold text-gray-600">Đổi trả 30 ngày</span>
              </div>
            </div>
          </div>
        </div>

        <!-- ===== DESCRIPTION TABS ===== -->
        <div class="mt-16">
          <div class="flex border-b border-gray-200">
            <button v-for="tab in tabs" :key="tab" @click="activeTab = tab"
              :class="['px-6 py-3 text-sm font-bold transition border-b-2 -mb-px',
                activeTab === tab
                  ? 'text-[#111827] border-blue-600'
                  : 'text-gray-400 border-transparent hover:text-gray-600']">
              {{ tab }}
            </button>
          </div>
          <div class="bg-white rounded-b-2xl p-6 sm:p-8 shadow-sm">
            <!-- Mô tả -->
            <div v-if="activeTab === 'Mô tả sản phẩm'">
              <div v-if="product.moTa" class="prose prose-sm max-w-none text-gray-600 leading-relaxed whitespace-pre-line">
                {{ product.moTa }}
              </div>
              <p v-else class="text-gray-400 italic">Chưa có mô tả cho sản phẩm này.</p>
            </div>
            <!-- Chính sách -->
            <div v-if="activeTab === 'Chính sách đổi trả'">
              <div class="space-y-4 text-sm text-gray-600 leading-relaxed">
                <div class="flex items-start space-x-3">
                  <CheckCircleIcon class="w-5 h-5 text-green-500 flex-shrink-0 mt-0.5" />
                  <p>Đổi trả miễn phí trong vòng <strong>30 ngày</strong> kể từ ngày mua hàng.</p>
                </div>
                <div class="flex items-start space-x-3">
                  <CheckCircleIcon class="w-5 h-5 text-green-500 flex-shrink-0 mt-0.5" />
                  <p>Sản phẩm còn nguyên tem, nhãn mác và chưa qua sử dụng.</p>
                </div>
                <div class="flex items-start space-x-3">
                  <CheckCircleIcon class="w-5 h-5 text-green-500 flex-shrink-0 mt-0.5" />
                  <p>Hoàn tiền 100% nếu sản phẩm bị lỗi từ nhà sản xuất.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- ===== RELATED PRODUCTS ===== -->
      <div v-if="relatedProducts.length > 0" class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 pb-16">
        <h2 class="text-xl sm:text-2xl font-black text-[#111827] mb-6">Sản phẩm tương tự</h2>
        <div class="flex space-x-4 overflow-x-auto scrollbar-hide scroll-smooth pb-4">
          <RouterLink v-for="sp in relatedProducts" :key="sp.maSp"
            :to="{ name: 'product-detail', params: { maSP: sp.maSp } }"
            class="flex-shrink-0 w-[200px] sm:w-[220px] group cursor-pointer">
            <div class="relative overflow-hidden rounded-xl bg-gray-100 aspect-[3/4]">
              <img :src="sp.anhDaiDien || placeholderImg" :alt="sp.tenSp"
                class="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105" />
              <span v-if="sp.giaGoc && sp.giaGoc > sp.giaKm"
                class="absolute top-2 left-2 bg-red-500 text-white text-[10px] font-bold px-2 py-0.5 rounded">
                -{{ Math.round((1 - sp.giaKm / sp.giaGoc) * 100) }}%
              </span>
            </div>
            <div class="mt-2">
              <h3 class="text-sm font-semibold text-[#111827] line-clamp-2 group-hover:text-blue-600 transition">{{ sp.tenSp }}</h3>
              <div class="flex items-center space-x-2 mt-1">
                <span class="text-sm font-bold text-red-600">{{ formatPrice(sp.giaKm) }}</span>
                <span v-if="sp.giaGoc && sp.giaGoc > sp.giaKm" class="text-xs text-gray-400 line-through">{{ formatPrice(sp.giaGoc) }}</span>
              </div>
            </div>
          </RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import { useRoute, RouterLink } from "vue-router";
import {
  Loader2 as Loader2Icon, PackageX as PackageXIcon,
  ArrowLeft as ArrowLeftIcon, ChevronRight as ChevronRightIcon,
  Ruler as RulerIcon, Minus as MinusIcon, Plus as PlusIcon,
  ShoppingBag as ShoppingBagIcon, CheckCircle as CheckCircleIcon,
  XCircle as XCircleIcon, Truck as TruckIcon,
  ShieldCheck as ShieldCheckIcon, RotateCcw as RotateCcwIcon,
} from "lucide-vue-next";
import { sanPhamService } from "../../services/sanpham.service";
import { danhMucService } from "../../services/danhmuc.service";
import { useCartStore } from "../../stores/cart";
import type { GuestCartItem } from "../../stores/cart";

const route = useRoute();
const cartStore = useCartStore();
const props = defineProps<{ maSP?: string }>();

const product = ref<any>(null);
const loading = ref(true);
const selectedImage = ref("");
const selectedColor = ref("");
const selectedSize = ref("");
const quantity = ref(1);
const activeTab = ref("Mô tả sản phẩm");
const zoomed = ref(false);
const zoomOrigin = ref("center center");
const cartMessage = ref("");
const cartSuccess = ref(false);
const allCategories = ref<any[]>([]);
const relatedProducts = ref<any[]>([]);

const tabs = ["Mô tả sản phẩm", "Chính sách đổi trả"];
const placeholderImg = "https://via.placeholder.com/600x800?text=No+Image";

const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";

// Size ordering
const SIZE_ORDER: Record<string, number> = {
  'xxs': 0, 'xs': 1, 's': 2, 'm': 3, 'l': 4, 'xl': 5,
  'xxl': 6, '2xl': 7, '3xl': 8, '4xl': 9, '5xl': 10,
};
const getSizeOrder = (name: string) => SIZE_ORDER[name.toLowerCase()] ?? 99;

// Look up category ID by name
const categoryId = computed(() => {
  if (!product.value?.tenDanhMuc || !allCategories.value.length) return null;
  const cat = allCategories.value.find((c: any) => c.tenDanhMuc === product.value.tenDanhMuc);
  return cat ? cat.maDanhMuc : null;
});

// All product images
const allImages = computed(() => {
  if (!product.value) return [];
  const imgs: string[] = [];
  if (product.value.anhDaiDien) imgs.push(product.value.anhDaiDien);
  if (product.value.duongDanAnhSPs) {
    product.value.duongDanAnhSPs.forEach((url: string) => {
      if (url && !imgs.includes(url)) imgs.push(url);
    });
  }
  return imgs;
});

// Available colors from variants
const availableColors = computed(() => {
  if (!product.value?.dsBienThe) return [];
  const colors = [...new Set<string>(product.value.dsBienThe.map((v: any) => v.tenMau))];
  return colors.filter(Boolean);
});

// Available sizes based on selected color
const availableSizes = computed(() => {
  if (!product.value?.dsBienThe) return [];
  let variants = product.value.dsBienThe;
  if (selectedColor.value) {
    variants = variants.filter((v: any) => v.tenMau === selectedColor.value);
  }
  const sizeMap = new Map<string, boolean>();
  variants.forEach((v: any) => {
    if (v.tenSize) {
      const existing = sizeMap.get(v.tenSize);
      if (!existing) sizeMap.set(v.tenSize, (v.soLuongTon ?? 0) > 0);
      else if ((v.soLuongTon ?? 0) > 0) sizeMap.set(v.tenSize, true);
    }
  });
  return Array.from(sizeMap)
    .map(([name, inStock]) => ({ name, inStock }))
    .sort((a, b) => getSizeOrder(a.name) - getSizeOrder(b.name));
});

// Selected variant
const selectedVariant = computed(() => {
  if (!product.value?.dsBienThe || !selectedColor.value || !selectedSize.value) return null;
  return product.value.dsBienThe.find(
    (v: any) => v.tenMau === selectedColor.value && v.tenSize === selectedSize.value
  ) || null;
});

// Auto-select first color
watch(availableColors, (cols) => {
  if (cols.length && !selectedColor.value) {
    selectedColor.value = cols[0] as string;
  }
});

// Reset size when color changes
watch(selectedColor, () => {
  selectedSize.value = "";
});

// Auto-select first available size
watch(availableSizes, (sizes) => {
  if (sizes.length && !selectedSize.value) {
    const inStock = sizes.find(s => s.inStock);
    if (inStock) selectedSize.value = inStock.name;
  }
});

const handleZoomMove = (e: MouseEvent) => {
  const rect = (e.target as HTMLElement).getBoundingClientRect();
  const x = ((e.clientX - rect.left) / rect.width) * 100;
  const y = ((e.clientY - rect.top) / rect.height) * 100;
  zoomOrigin.value = `${x}% ${y}%`;
};

const addToCart = async () => {
  if (!selectedVariant.value) return;
  try {
    const item: GuestCartItem = {
      maCTSP: selectedVariant.value.maChiTietSP,
      maSP: product.value.maSp,
      tenSP: product.value.tenSp,
      anh: product.value.anhDaiDien || "",
      tenMau: selectedColor.value,
      tenSize: selectedSize.value,
      gia: product.value.giaKm || product.value.giaGoc,
      soLuong: quantity.value,
    };
    await cartStore.addToCart(item);
    cartSuccess.value = true;
    cartMessage.value = `Đã thêm "${product.value.tenSp}" (${selectedColor.value} / ${selectedSize.value}) x${quantity.value} vào giỏ hàng!`;
    setTimeout(() => { cartMessage.value = ""; }, 3000);
  } catch (e: any) {
    cartSuccess.value = false;
    cartMessage.value = "Không thể thêm vào giỏ hàng. Vui lòng thử lại.";
    setTimeout(() => { cartMessage.value = ""; }, 3000);
  }
};

onMounted(async () => {
  const maSP = props.maSP || (route.params.maSP as string);
  if (!maSP) {
    loading.value = false;
    return;
  }
  try {
    // Load categories and product in parallel
    const [catRes, spRes]: any[] = await Promise.all([
      danhMucService.getAll(),
      sanPhamService.getFullInfo(maSP),
    ]);
    if (catRes.success) {
      allCategories.value = catRes.data || [];
    }
    if (spRes.success && spRes.data) {
      product.value = spRes.data;
      if (product.value.anhDaiDien) {
        selectedImage.value = product.value.anhDaiDien;
      }
      // Load related products by category
      if (product.value.tenDanhMuc && allCategories.value.length) {
        const cat = allCategories.value.find((c: any) => c.tenDanhMuc === product.value.tenDanhMuc);
        if (cat) {
          try {
            const relRes: any = await sanPhamService.getByCategory(cat.maDanhMuc);
            if (relRes.success && relRes.data) {
              relatedProducts.value = relRes.data
                .filter((p: any) => p.maSp !== maSP)
                .slice(0, 8);
            }
          } catch { /* ignore */ }
        }
      }
    }
  } catch (e) {
    console.warn("Failed to load product detail", e);
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.scrollbar-hide::-webkit-scrollbar { display: none; }
.scrollbar-hide { -ms-overflow-style: none; scrollbar-width: none; }
.fade-enter-active { transition: all 0.3s ease-out; }
.fade-leave-active { transition: all 0.2s ease-in; }
.fade-enter-from, .fade-leave-to { opacity: 0; transform: translateY(-4px); }
</style>
