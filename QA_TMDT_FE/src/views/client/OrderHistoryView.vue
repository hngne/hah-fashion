<template>
  <div class="min-h-[70vh] bg-[#fafafa]">
    <div class="border-b border-gray-100 bg-white">
      <div class="mx-auto max-w-[1400px] px-4 py-4 sm:px-6 lg:px-8">
        <nav class="flex items-center space-x-2 text-sm text-gray-400">
          <RouterLink to="/" class="transition hover:text-blue-600">Trang chủ</RouterLink>
          <ChevronRightIcon class="h-3.5 w-3.5" />
          <span class="font-medium text-gray-700">Đơn hàng của tôi</span>
        </nav>
        <h1 class="mt-2 text-2xl font-black tracking-tight text-[#111827] sm:text-3xl">
          Đơn hàng của tôi
        </h1>
      </div>
    </div>

    <div class="mx-auto max-w-[1400px] px-4 py-8 sm:px-6 lg:px-8">
      <div
        v-if="message"
        :class="messageClass"
        class="mb-6 rounded-2xl px-4 py-3 text-sm font-medium"
      >
        {{ message }}
      </div>

      <div v-if="loading" class="flex items-center justify-center py-20">
        <Loader2Icon class="h-8 w-8 animate-spin text-blue-500" />
      </div>

      <div v-else-if="orders.length === 0" class="py-20 text-center">
        <PackageIcon class="mx-auto mb-4 h-16 w-16 text-gray-200" />
        <h2 class="text-xl font-bold text-gray-500">Chưa có đơn hàng nào</h2>
        <RouterLink
          to="/search"
          class="mt-6 inline-flex items-center rounded-full bg-[#111827] px-6 py-3 text-sm font-bold text-white transition hover:bg-blue-600"
        >
          Mua sắm ngay
        </RouterLink>
      </div>

      <div v-else class="space-y-4">
        <article
          v-for="order in orders"
          :key="order.maDonHang"
          class="overflow-hidden rounded-2xl bg-white shadow-sm"
        >
          <header class="flex flex-wrap items-center justify-between gap-3 border-b border-gray-100 bg-gray-50 px-5 py-4">
            <div class="flex items-center gap-4">
              <span class="text-sm font-bold text-[#111827]">{{ order.maDonHang }}</span>
              <span class="text-xs text-gray-400">{{ formatDate(order.ngayDatHang) }}</span>
            </div>
            <span :class="statusClass(order.trangThaiDonHang)" class="rounded-full px-3 py-1 text-xs font-bold">
              {{ statusLabel(order.trangThaiDonHang) }}
            </span>
          </header>

          <div class="space-y-3 px-5 py-4">
            <div
              v-for="item in order.chiTietDonHang"
              :key="`${order.maDonHang}-${item.maCTSP}`"
              class="flex items-center gap-3"
            >
              <div class="h-14 w-14 flex-shrink-0 overflow-hidden rounded-xl bg-gray-100">
                <img
                  :src="item.hinhAnhUrl || placeholderImage"
                  :alt="item.tenSP"
                  class="h-full w-full object-cover"
                />
              </div>
              <div class="min-w-0 flex-1">
                <p class="line-clamp-1 text-sm font-semibold text-[#111827]">{{ item.tenSP }}</p>
                <p class="mt-1 text-xs text-gray-400">
                  {{ item.tenMau }} / {{ item.tenSize }} × {{ item.soLuong }}
                </p>
              </div>
              <span class="flex-shrink-0 text-sm font-bold text-[#111827]">
                {{ formatPrice(item.donGia * item.soLuong) }}
              </span>
            </div>
          </div>

          <footer class="flex flex-wrap items-center justify-between gap-3 border-t border-gray-100 px-5 py-4">
            <button
              v-if="canCancel(order.trangThaiDonHang)"
              type="button"
              class="rounded-lg border border-red-200 px-3 py-1.5 text-xs font-semibold text-red-500 transition hover:bg-red-50 hover:text-red-600"
              @click="openCancelModal(order)"
            >
              Hủy đơn
            </button>
            <span v-else class="text-xs text-gray-400">Đơn hàng đang được xử lý theo trạng thái hiện tại.</span>

            <div class="text-right">
              <span class="text-xs text-gray-400">Tổng thanh toán:</span>
              <span class="ml-2 text-lg font-black text-red-600">{{ formatPrice(order.thanhToan) }}</span>
            </div>
          </footer>
        </article>
      </div>
    </div>

    <transition name="fade">
      <div
        v-if="cancelModal"
        class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 px-4"
        @click.self="cancelModal = null"
      >
        <div class="w-full max-w-sm rounded-2xl bg-white p-6 shadow-2xl">
          <h3 class="mb-3 text-lg font-black text-[#111827]">Hủy đơn hàng</h3>
          <p class="mb-4 text-sm text-gray-500">
            Bạn có chắc muốn hủy đơn <b>{{ cancelModal.maDonHang }}</b>?
          </p>
          <textarea
            v-model="cancelReason"
            rows="3"
            placeholder="Lý do hủy đơn..."
            class="mb-4 w-full resize-none rounded-xl border border-gray-200 px-4 py-2.5 text-sm transition focus:border-red-500 focus:outline-none focus:ring-2 focus:ring-red-500/30"
          />
          <div class="flex space-x-3">
            <button
              type="button"
              class="flex-1 rounded-xl border border-gray-200 py-2.5 text-sm font-semibold text-gray-600 transition hover:bg-gray-50"
              @click="cancelModal = null"
            >
              Không
            </button>
            <button
              type="button"
              :disabled="cancelling"
              class="flex flex-1 items-center justify-center rounded-xl bg-red-500 py-2.5 text-sm font-semibold text-white transition hover:bg-red-600 disabled:opacity-50"
              @click="confirmCancel"
            >
              <Loader2Icon v-if="cancelling" class="mr-1 h-4 w-4 animate-spin" />
              {{ cancelling ? "Đang xử lý..." : "Xác nhận hủy" }}
            </button>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { RouterLink } from "vue-router";
import {
  ChevronRight as ChevronRightIcon,
  Loader2 as Loader2Icon,
  Package as PackageIcon,
} from "lucide-vue-next";
import { donHangService } from "../../services/donhang.service";
import { extractErrorMessage } from "../../services/service-helpers";
import { useAuthStore } from "../../stores/auth";
import type { Order } from "../../types";

const authStore = useAuthStore();

const orders = ref<Order[]>([]);
const loading = ref(true);
const cancelling = ref(false);
const cancelModal = ref<Order | null>(null);
const cancelReason = ref("");
const message = ref("");
const messageIsSuccess = ref(false);

const placeholderImage = "https://via.placeholder.com/100x100?text=SP";

const messageClass = computed(() =>
  messageIsSuccess.value
    ? "bg-emerald-50 text-emerald-700"
    : "bg-rose-50 text-rose-700",
);

const statusLabels: Record<string, string> = {
  ChoXacNhan: "Chờ xác nhận",
  DangVanChuyen: "Đang vận chuyển",
  GiaoHangThanhCong: "Giao hàng thành công",
  DaHuy: "Đã hủy",
};

function setMessage(nextMessage: string, success: boolean) {
  message.value = nextMessage;
  messageIsSuccess.value = success;
}

function statusLabel(status: string) {
  return statusLabels[status] || status;
}

function statusClass(status: string) {
  if (status === "DaHuy") return "bg-red-100 text-red-600";
  if (status === "GiaoHangThanhCong") return "bg-green-100 text-green-600";
  if (status === "DangVanChuyen") return "bg-blue-100 text-blue-600";
  return "bg-orange-100 text-orange-600";
}

function canCancel(status: string) {
  return status === "ChoXacNhan";
}

function formatPrice(value: number) {
  return `${(value || 0).toLocaleString("vi-VN")} đ`;
}

function formatDate(value: string) {
  if (!value) return "";
  return new Date(value).toLocaleDateString("vi-VN", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  });
}

function openCancelModal(order: Order) {
  cancelModal.value = order;
  cancelReason.value = "";
}

async function loadOrders() {
  if (!authStore.user?.maTaiKhoan) return;

  try {
    const response = await donHangService.getMyOrders();
    orders.value = response.data || [];
  } catch (caughtError) {
    setMessage(
      extractErrorMessage(caughtError, "Không thể tải danh sách đơn hàng."),
      false,
    );
  }
}

async function confirmCancel() {
  if (!cancelModal.value) return;

  cancelling.value = true;
  setMessage("", false);
  try {
    await donHangService.cancelOrder(
      cancelModal.value.maDonHang,
      cancelReason.value || "Khách hàng tự hủy",
    );
    cancelModal.value = null;
    setMessage("Đã hủy đơn hàng thành công.", true);
    await loadOrders();
  } catch (caughtError) {
    setMessage(
      extractErrorMessage(caughtError, "Không thể hủy đơn hàng."),
      false,
    );
  } finally {
    cancelling.value = false;
  }
}

onMounted(async () => {
  await loadOrders();
  loading.value = false;
});
</script>

<style scoped>
.fade-enter-active {
  transition: all 0.2s ease-out;
}

.fade-leave-active {
  transition: all 0.15s ease-in;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
