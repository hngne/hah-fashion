<template>
  <div class="min-h-[70vh] bg-[#FAFAFA]">
    <!-- Header -->
    <div class="bg-white border-b border-gray-100">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <nav class="flex items-center text-sm text-gray-400 space-x-2">
          <RouterLink to="/" class="hover:text-blue-600 transition">Trang chủ</RouterLink>
          <ChevronRightIcon class="w-3.5 h-3.5" />
          <span class="text-gray-700 font-medium">Đơn hàng của tôi</span>
        </nav>
        <h1 class="text-2xl sm:text-3xl font-black text-[#111827] tracking-tight mt-2">Đơn hàng của tôi</h1>
      </div>
    </div>

    <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Loading -->
      <div v-if="loading" class="flex items-center justify-center py-20">
        <Loader2Icon class="h-8 w-8 animate-spin text-blue-500" />
      </div>

      <!-- Empty -->
      <div v-else-if="orders.length === 0" class="text-center py-20">
        <PackageIcon class="h-16 w-16 mx-auto text-gray-200 mb-4" />
        <h2 class="text-xl font-bold text-gray-500">Chưa có đơn hàng nào</h2>
        <RouterLink to="/search"
          class="inline-flex items-center mt-6 px-6 py-3 bg-[#111827] text-white font-bold text-sm rounded-full hover:bg-blue-600 transition">
          Mua sắm ngay
        </RouterLink>
      </div>

      <!-- Order List -->
      <div v-else class="space-y-4">
        <div v-for="order in orders" :key="order.maDonHang"
          class="bg-white rounded-2xl shadow-sm overflow-hidden">
          <!-- Order Header -->
          <div class="flex flex-wrap items-center justify-between px-5 py-4 bg-gray-50 border-b border-gray-100">
            <div class="flex items-center space-x-4">
              <span class="text-sm font-bold text-[#111827]">{{ order.maDonHang }}</span>
              <span class="text-xs text-gray-400">{{ formatDate(order.ngayDatHang) }}</span>
            </div>
            <span :class="['text-xs font-bold px-3 py-1 rounded-full', statusClass(order.trangThaiDonHang)]">
              {{ order.trangThaiDonHang }}
            </span>
          </div>
          <!-- Items -->
          <div class="px-5 py-4 space-y-3">
            <div v-for="item in order.chiTietDonHang" :key="item.maCTSP" class="flex items-center space-x-3">
              <div class="w-12 h-12 rounded-lg overflow-hidden bg-gray-100 flex-shrink-0">
                <img :src="item.hinhAnhUrl || placeholderImg" :alt="item.tenSP" class="w-full h-full object-cover" />
              </div>
              <div class="flex-1 min-w-0">
                <p class="text-sm font-semibold text-[#111827] line-clamp-1">{{ item.tenSP }}</p>
                <p class="text-[10px] text-gray-400">{{ item.tenMau }} / {{ item.tenSize }} × {{ item.soLuong }}</p>
              </div>
              <span class="text-sm font-bold text-[#111827] flex-shrink-0">{{ formatPrice(item.donGia * item.soLuong) }}</span>
            </div>
          </div>
          <!-- Order Footer -->
          <div class="flex flex-wrap items-center justify-between px-5 py-4 border-t border-gray-100">
            <div class="flex items-center space-x-3">
              <button v-if="canCancel(order.trangThaiDonHang)" @click="openCancelModal(order)"
                class="text-xs font-semibold text-red-500 hover:text-red-600 border border-red-200 px-3 py-1.5 rounded-lg hover:bg-red-50 transition">
                Hủy đơn
              </button>
            </div>
            <div class="text-right">
              <span class="text-xs text-gray-400">Tổng thanh toán:</span>
              <span class="text-lg font-black text-red-600 ml-2">{{ formatPrice(order.thanhToan) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Cancel Modal -->
    <transition name="fade">
      <div v-if="cancelModal" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 px-4" @click.self="cancelModal = null">
        <div class="bg-white rounded-2xl p-6 w-full max-w-sm shadow-2xl">
          <h3 class="text-lg font-black text-[#111827] mb-3">Hủy đơn hàng</h3>
          <p class="text-sm text-gray-500 mb-4">Bạn có chắc muốn hủy đơn <b>{{ cancelModal.maDonHang }}</b>?</p>
          <textarea v-model="cancelReason" rows="3" placeholder="Lý do hủy đơn..."
            class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-red-500/30 focus:border-red-500 transition resize-none mb-4"></textarea>
          <div class="flex space-x-3">
            <button @click="cancelModal = null"
              class="flex-1 py-2.5 border border-gray-200 text-gray-600 font-semibold text-sm rounded-xl hover:bg-gray-50 transition">
              Không
            </button>
            <button @click="confirmCancel" :disabled="cancelling"
              class="flex-1 py-2.5 bg-red-500 text-white font-semibold text-sm rounded-xl hover:bg-red-600 transition disabled:opacity-50 flex items-center justify-center">
              <Loader2Icon v-if="cancelling" class="w-4 h-4 animate-spin mr-1" />
              {{ cancelling ? '...' : 'Xác nhận hủy' }}
            </button>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { RouterLink } from "vue-router";
import {
  ChevronRight as ChevronRightIcon, Loader2 as Loader2Icon,
  Package as PackageIcon,
} from "lucide-vue-next";
import { useAuthStore } from "../../stores/auth";
import { donHangService } from "../../services/donhang.service";

const authStore = useAuthStore();
const orders = ref<any[]>([]);
const loading = ref(true);

const cancelModal = ref<any>(null);
const cancelReason = ref("");
const cancelling = ref(false);

const placeholderImg = "https://via.placeholder.com/100x100?text=SP";
const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";
const formatDate = (d: string) => {
  if (!d) return "";
  return new Date(d).toLocaleDateString("vi-VN", { day: "2-digit", month: "2-digit", year: "numeric" });
};

const statusClass = (status: string) => {
  const s = status?.toLowerCase() || "";
  if (s.includes("hủy")) return "bg-red-100 text-red-600";
  if (s.includes("giao") || s.includes("hoàn")) return "bg-green-100 text-green-600";
  if (s.includes("xử lý") || s.includes("chờ")) return "bg-orange-100 text-orange-600";
  if (s.includes("xác nhận")) return "bg-blue-100 text-blue-600";
  return "bg-gray-100 text-gray-600";
};

const canCancel = (status: string) => {
  const s = status?.toLowerCase() || "";
  return s.includes("chờ") || s.includes("xử lý") || s.includes("xác nhận");
};

const openCancelModal = (order: any) => {
  cancelModal.value = order;
  cancelReason.value = "";
};

const confirmCancel = async () => {
  if (!cancelModal.value) return;
  cancelling.value = true;
  try {
    await donHangService.cancelOrder(cancelModal.value.maDonHang, cancelReason.value || "Khách hàng tự hủy");
    cancelModal.value = null;
    // Reload
    await loadOrders();
  } catch (e) {
    console.warn("Cancel failed", e);
  } finally {
    cancelling.value = false;
  }
};

const loadOrders = async () => {
  if (!authStore.user?.maTaiKhoan) return;
  try {
    const res: any = await donHangService.getMyOrders(authStore.user.maTaiKhoan);
    if (res.success && res.data) {
      orders.value = res.data;
    }
  } catch (e) {
    console.warn("Failed to load orders", e);
  }
};

onMounted(async () => {
  await loadOrders();
  loading.value = false;
});
</script>

<style scoped>
.fade-enter-active { transition: all 0.2s ease-out; }
.fade-leave-active { transition: all 0.15s ease-in; }
.fade-enter-from, .fade-leave-to { opacity: 0; }
</style>
