<template>
  <div class="min-h-[70vh] bg-[#FAFAFA]">
    <!-- Breadcrumb -->
    <div class="bg-white border-b border-gray-100">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <nav class="flex items-center text-sm text-gray-400 space-x-2">
          <RouterLink to="/" class="hover:text-blue-600 transition">Trang chủ</RouterLink>
          <ChevronRightIcon class="w-3.5 h-3.5" />
          <span class="text-gray-700 font-medium">Giỏ hàng</span>
        </nav>
        <h1 class="text-2xl sm:text-3xl font-black text-[#111827] tracking-tight mt-2">
          Giỏ hàng
          <span v-if="cartStore.totalItems > 0" class="text-lg font-semibold text-gray-400 ml-2">({{ cartStore.totalItems }} sản phẩm)</span>
        </h1>
      </div>
    </div>

    <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Empty Cart -->
      <div v-if="cartStore.items.length === 0" class="text-center py-20">
        <ShoppingCartIcon class="h-20 w-20 mx-auto text-gray-200 mb-4" />
        <h2 class="text-xl font-bold text-gray-500">Giỏ hàng trống</h2>
        <p class="text-sm text-gray-400 mt-2">Hãy thêm sản phẩm yêu thích vào giỏ hàng nhé!</p>
        <RouterLink to="/search"
          class="inline-flex items-center mt-6 px-6 py-3 bg-[#111827] text-white font-bold text-sm rounded-full hover:bg-blue-600 transition">
          <ShoppingBagIcon class="w-4 h-4 mr-2" /> Mua sắm ngay
        </RouterLink>
      </div>

      <!-- Cart Content -->
      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Cart Items -->
        <div class="lg:col-span-2 space-y-4">
          <div v-for="item in cartStore.items" :key="item.maCTSP"
            class="bg-white rounded-2xl p-4 sm:p-5 shadow-sm flex gap-4">
            <!-- Image -->
            <RouterLink :to="{ name: 'product-detail', params: { maSP: item.maSP } }"
              class="flex-shrink-0 w-24 h-24 sm:w-28 sm:h-28 rounded-xl overflow-hidden bg-gray-100">
              <img :src="item.anh || placeholderImg" :alt="item.tenSP"
                class="w-full h-full object-cover hover:scale-105 transition-transform duration-300" />
            </RouterLink>

            <!-- Info -->
            <div class="flex-1 min-w-0">
              <RouterLink :to="{ name: 'product-detail', params: { maSP: item.maSP } }"
                class="text-sm font-semibold text-[#111827] hover:text-blue-600 transition line-clamp-2">
                {{ item.tenSP }}
              </RouterLink>
              <div class="flex items-center space-x-3 mt-1.5 text-xs text-gray-400">
                <span v-if="item.tenMau">Màu: <b class="text-gray-600">{{ item.tenMau }}</b></span>
                <span v-if="item.tenSize">Size: <b class="text-gray-600">{{ item.tenSize }}</b></span>
              </div>
              <div class="flex items-center space-x-2 mt-2">
                <span class="text-sm font-bold text-red-600">{{ formatPrice(item.gia) }}</span>
                <span v-if="item.giaGoc && item.giaGoc > item.gia" class="text-xs text-gray-400 line-through">{{ formatPrice(item.giaGoc) }}</span>
              </div>

              <!-- Quantity + Delete -->
              <div class="flex items-center justify-between mt-3">
                <div class="flex items-center border border-gray-200 rounded-lg overflow-hidden">
                  <button @click="updateQty(item.maCTSP, item.soLuong - 1)"
                    :disabled="item.soLuong <= 1"
                    class="w-8 h-8 flex items-center justify-center text-gray-500 hover:bg-gray-100 transition disabled:opacity-30">
                    <MinusIcon class="w-3.5 h-3.5" />
                  </button>
                  <span class="w-10 text-center text-sm font-bold text-[#111827]">{{ item.soLuong }}</span>
                  <button @click="updateQty(item.maCTSP, item.soLuong + 1)"
                    :disabled="item.soLuong >= item.soLuongTon"
                    class="w-8 h-8 flex items-center justify-center text-gray-500 hover:bg-gray-100 transition disabled:opacity-30">
                    <PlusIcon class="w-3.5 h-3.5" />
                  </button>
                </div>
                <div class="flex items-center space-x-3">
                  <span class="text-sm font-bold text-[#111827]">{{ formatPrice(item.thanhTien) }}</span>
                  <button @click="cartStore.removeItem(item.maCTSP)"
                    class="p-1.5 text-gray-400 hover:text-red-500 transition">
                    <Trash2Icon class="w-4 h-4" />
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-2xl p-6 shadow-sm sticky top-24">
            <h3 class="text-lg font-black text-[#111827] mb-4">Tóm tắt đơn hàng</h3>
            <div class="space-y-3 text-sm">
              <div class="flex justify-between text-gray-500">
                <span>Tạm tính ({{ cartStore.totalItems }} SP)</span>
                <span class="font-semibold text-gray-700">{{ formatPrice(cartStore.totalPrice) }}</span>
              </div>
              <div class="flex justify-between text-gray-500">
                <span>Phí vận chuyển</span>
                <span class="font-semibold text-green-600">Miễn phí</span>
              </div>
              <hr class="border-gray-100" />
              <div class="flex justify-between text-lg font-black text-[#111827]">
                <span>Tổng cộng</span>
                <span class="text-red-600">{{ formatPrice(cartStore.totalPrice) }}</span>
              </div>
            </div>
            <button @click="handleCheckout"
              class="w-full mt-6 py-3.5 bg-[#111827] text-white font-bold text-sm rounded-xl hover:bg-blue-600 transition flex items-center justify-center space-x-2 active:scale-[0.98]">
              <CreditCardIcon class="w-5 h-5" />
              <span>TIẾN HÀNH THANH TOÁN</span>
            </button>
            <button @click="cartStore.clearAll"
              class="w-full mt-2 py-2.5 text-gray-400 font-semibold text-xs hover:text-red-500 transition">
              Xóa toàn bộ giỏ hàng
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink, useRouter } from "vue-router";
import {
  ChevronRight as ChevronRightIcon,
  ShoppingCart as ShoppingCartIcon, ShoppingBag as ShoppingBagIcon,
  Minus as MinusIcon, Plus as PlusIcon, Trash2 as Trash2Icon,
  CreditCard as CreditCardIcon,
} from "lucide-vue-next";
import { useCartStore } from "../../stores/cart";
import { useAuthStore } from "../../stores/auth";

const router = useRouter();
const cartStore = useCartStore();
const authStore = useAuthStore();

const placeholderImg = "https://via.placeholder.com/200x200?text=No+Image";
const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";

const updateQty = (maCTSP: string, qty: number) => {
  if (qty < 1) return;
  const currentItem = cartStore.items.find((item) => item.maCTSP === maCTSP);
  if (!currentItem) return;

  const nextQty = Math.min(qty, currentItem.soLuongTon || qty);
  cartStore.updateQuantity(maCTSP, nextQty);
};

const handleCheckout = () => {
  if (!authStore.isAuthenticated()) {
    // Redirect to login, then go to checkout
    router.push({ name: "client-login", query: { redirect: "/checkout" } });
  } else {
    router.push({ name: "checkout" });
  }
};
</script>
