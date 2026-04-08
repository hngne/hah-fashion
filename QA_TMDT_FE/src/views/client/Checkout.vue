<template>
  <div class="min-h-[70vh] bg-[#FAFAFA]">
    <!-- Breadcrumb -->
    <div class="bg-white border-b border-gray-100">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <nav class="flex items-center text-sm text-gray-400 space-x-2">
          <RouterLink to="/" class="hover:text-blue-600 transition">Trang chủ</RouterLink>
          <ChevronRightIcon class="w-3.5 h-3.5" />
          <RouterLink to="/cart" class="hover:text-blue-600 transition">Giỏ hàng</RouterLink>
          <ChevronRightIcon class="w-3.5 h-3.5" />
          <span class="text-gray-700 font-medium">Thanh toán</span>
        </nav>
        <h1 class="text-2xl sm:text-3xl font-black text-[#111827] tracking-tight mt-2">Thanh toán</h1>
      </div>
    </div>

    <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Error -->
      <transition name="fade">
        <div v-if="errorMsg" class="flex items-center space-x-2 px-4 py-3 rounded-xl bg-red-50 text-red-700 text-sm font-medium mb-6">
          <AlertCircleIcon class="w-5 h-5 flex-shrink-0" />
          <span>{{ errorMsg }}</span>
        </div>
      </transition>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Left: Form -->
        <div class="lg:col-span-2 space-y-6">
          <!-- Shipping Info -->
          <div class="bg-white rounded-2xl p-6 shadow-sm">
            <h3 class="text-lg font-black text-[#111827] mb-4 flex items-center">
              <MapPinIcon class="w-5 h-5 mr-2 text-blue-500" /> Thông tin giao hàng
            </h3>
            <div class="space-y-4">
              <div>
                <label class="block text-sm font-semibold text-[#111827] mb-1.5">Tên người nhận <span class="text-red-500">*</span></label>
                <input v-model="form.tenNguoiNhan" type="text" required placeholder="Nguyễn Văn A"
                  class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
              </div>
              <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div>
                  <label class="block text-sm font-semibold text-[#111827] mb-1.5">Số điện thoại <span class="text-red-500">*</span></label>
                  <input v-model="form.soDienThoai" type="text" required placeholder="0901234567"
                    class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
                </div>
                <div>
                  <label class="block text-sm font-semibold text-[#111827] mb-1.5">Email</label>
                  <input :value="authStore.user?.email" disabled type="email"
                    class="w-full px-4 py-2.5 border border-gray-100 rounded-xl text-sm bg-gray-50 text-gray-400" />
                </div>
              </div>
              <div>
                <label class="block text-sm font-semibold text-[#111827] mb-1.5">Địa chỉ giao hàng <span class="text-red-500">*</span></label>
                <textarea v-model="form.diaChiGiaoHang" required rows="2" placeholder="Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành"
                  class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition resize-none"></textarea>
              </div>
            </div>
          </div>

          <!-- Payment Method -->
          <div class="bg-white rounded-2xl p-6 shadow-sm">
            <h3 class="text-lg font-black text-[#111827] mb-4 flex items-center">
              <CreditCardIcon class="w-5 h-5 mr-2 text-green-500" /> Phương thức thanh toán
            </h3>
            <div class="space-y-3">
              <label v-for="pt in paymentMethods" :key="pt.id"
                :class="['flex items-center p-4 border-2 rounded-xl cursor-pointer transition',
                  form.maPTTT === pt.id ? 'border-blue-500 bg-blue-50' : 'border-gray-200 hover:border-gray-300']">
                <input type="radio" :value="pt.id" v-model="form.maPTTT" class="sr-only" />
                <div :class="['w-5 h-5 rounded-full border-2 mr-3 flex items-center justify-center flex-shrink-0',
                  form.maPTTT === pt.id ? 'border-blue-500' : 'border-gray-300']">
                  <div v-if="form.maPTTT === pt.id" class="w-2.5 h-2.5 rounded-full bg-blue-500"></div>
                </div>
                <div>
                  <p class="text-sm font-semibold text-[#111827]">{{ pt.name }}</p>
                  <p class="text-xs text-gray-400">{{ pt.desc }}</p>
                </div>
              </label>
            </div>
          </div>

          <!-- Shipping Method -->
          <div class="bg-white rounded-2xl p-6 shadow-sm">
            <h3 class="text-lg font-black text-[#111827] mb-4 flex items-center">
              <TruckIcon class="w-5 h-5 mr-2 text-orange-500" /> Phương thức vận chuyển
            </h3>
            <div class="space-y-3">
              <label v-for="vc in shippingMethods" :key="vc.id"
                :class="['flex items-center justify-between p-4 border-2 rounded-xl cursor-pointer transition',
                  form.maPTVC === vc.id ? 'border-blue-500 bg-blue-50' : 'border-gray-200 hover:border-gray-300']">
                <div class="flex items-center">
                  <input type="radio" :value="vc.id" v-model="form.maPTVC" class="sr-only" />
                  <div :class="['w-5 h-5 rounded-full border-2 mr-3 flex items-center justify-center flex-shrink-0',
                    form.maPTVC === vc.id ? 'border-blue-500' : 'border-gray-300']">
                    <div v-if="form.maPTVC === vc.id" class="w-2.5 h-2.5 rounded-full bg-blue-500"></div>
                  </div>
                  <div>
                    <p class="text-sm font-semibold text-[#111827]">{{ vc.name }}</p>
                    <p class="text-xs text-gray-400">{{ vc.desc }}</p>
                  </div>
                </div>
                <span class="text-sm font-bold text-[#111827]">{{ vc.fee > 0 ? formatPrice(vc.fee) : 'Miễn phí' }}</span>
              </label>
            </div>
          </div>

          <!-- Voucher -->
          <div class="bg-white rounded-2xl p-6 shadow-sm">
            <h3 class="text-lg font-black text-[#111827] mb-4 flex items-center">
              <TicketIcon class="w-5 h-5 mr-2 text-purple-500" /> Mã giảm giá
            </h3>
            <div class="flex space-x-3">
              <input v-model="voucherCode" type="text" placeholder="Nhập mã voucher..."
                class="flex-1 px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
              <button @click="applyVoucher" :disabled="!voucherCode.trim() || voucherLoading"
                class="px-5 py-2.5 bg-[#111827] text-white text-sm font-bold rounded-xl hover:bg-blue-600 transition disabled:opacity-50">
                {{ voucherLoading ? '...' : 'Áp dụng' }}
              </button>
            </div>
            <div v-if="appliedVoucher" class="mt-3 flex items-center justify-between px-4 py-3 bg-green-50 rounded-xl">
              <div class="flex items-center space-x-2">
                <CheckCircleIcon class="w-4 h-4 text-green-500" />
                <span class="text-sm font-medium text-green-700">{{ appliedVoucher.tenVoucher || appliedVoucher.maVoucher }} — Giảm {{ formatPrice(appliedVoucher.giamGia) }}</span>
              </div>
              <button @click="removeVoucher" class="text-xs text-red-500 font-semibold hover:underline">Bỏ</button>
            </div>
            <p v-if="voucherError" class="mt-2 text-xs text-red-500 font-medium">{{ voucherError }}</p>
          </div>
        </div>

        <!-- Right: Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-2xl p-6 shadow-sm sticky top-24">
            <h3 class="text-lg font-black text-[#111827] mb-4">Đơn hàng của bạn</h3>
            <!-- Items -->
            <div class="space-y-3 max-h-[300px] overflow-y-auto">
              <div v-for="item in cartStore.items" :key="item.maCTSP" class="flex items-center space-x-3">
                <div class="w-14 h-14 rounded-lg overflow-hidden bg-gray-100 flex-shrink-0">
                  <img :src="item.anh || placeholderImg" :alt="item.tenSP" class="w-full h-full object-cover" />
                </div>
                <div class="flex-1 min-w-0">
                  <p class="text-xs font-semibold text-[#111827] line-clamp-1">{{ item.tenSP }}</p>
                  <p class="text-[10px] text-gray-400">{{ item.tenMau }} / {{ item.tenSize }} × {{ item.soLuong }}</p>
                </div>
                <span class="text-xs font-bold text-[#111827] flex-shrink-0">{{ formatPrice(item.thanhTien) }}</span>
              </div>
            </div>
            <hr class="my-4 border-gray-100" />
            <!-- Totals -->
            <div class="space-y-2 text-sm">
              <div class="flex justify-between text-gray-500">
                <span>Tạm tính</span>
                <span class="font-semibold text-gray-700">{{ formatPrice(cartStore.totalPrice) }}</span>
              </div>
              <div class="flex justify-between text-gray-500">
                <span>Phí vận chuyển</span>
                <span class="font-semibold" :class="shippingFee > 0 ? 'text-gray-700' : 'text-green-600'">
                  {{ shippingFee > 0 ? formatPrice(shippingFee) : 'Miễn phí' }}
                </span>
              </div>
              <div v-if="appliedVoucher" class="flex justify-between text-green-600">
                <span>Giảm giá voucher</span>
                <span class="font-semibold">-{{ formatPrice(appliedVoucher.giamGia) }}</span>
              </div>
              <hr class="border-gray-100" />
              <div class="flex justify-between text-lg font-black text-[#111827]">
                <span>Tổng thanh toán</span>
                <span class="text-red-600">{{ formatPrice(finalTotal) }}</span>
              </div>
            </div>
            <button @click="submitOrder" :disabled="submitting"
              class="w-full mt-6 py-3.5 bg-[#111827] text-white font-bold text-sm rounded-xl hover:bg-blue-600 transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center space-x-2 active:scale-[0.98]">
              <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
              <span>{{ submitting ? 'Đang xử lý...' : 'ĐẶT HÀNG' }}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { RouterLink, useRouter } from "vue-router";
import {
  ChevronRight as ChevronRightIcon, MapPin as MapPinIcon,
  CreditCard as CreditCardIcon, Truck as TruckIcon,
  Ticket as TicketIcon, CheckCircle as CheckCircleIcon,
  AlertCircle as AlertCircleIcon, Loader2 as Loader2Icon,
} from "lucide-vue-next";
import { useAuthStore } from "../../stores/auth";
import { useCartStore } from "../../stores/cart";
import { donHangService } from "../../services/donhang.service";
import { voucherService } from "../../services/voucher.service";

const router = useRouter();
const authStore = useAuthStore();
const cartStore = useCartStore();

const placeholderImg = "https://via.placeholder.com/100x100?text=SP";
const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";

const paymentMethods = [
  { id: 1, name: "Thanh toán khi nhận hàng (COD)", desc: "Thanh toán bằng tiền mặt khi nhận hàng" },
  { id: 2, name: "Chuyển khoản ngân hàng", desc: "Chuyển khoản trước khi giao hàng" },
];

const shippingMethods = [
  { id: 1, name: "Giao hàng tiêu chuẩn", desc: "Giao trong 3-5 ngày", fee: 0 },
  { id: 2, name: "Giao hàng nhanh", desc: "Giao trong 1-2 ngày", fee: 30000 },
];

const form = ref({
  tenNguoiNhan: "",
  soDienThoai: "",
  diaChiGiaoHang: "",
  maPTTT: 1,
  maPTVC: 1,
  maVoucher: undefined as string | undefined,
});

const submitting = ref(false);
const errorMsg = ref("");
const voucherCode = ref("");
const voucherLoading = ref(false);
const voucherError = ref("");
const appliedVoucher = ref<any>(null);

const shippingFee = computed(() => {
  const method = shippingMethods.find(m => m.id === form.value.maPTVC);
  return method ? method.fee : 0;
});

const finalTotal = computed(() => {
  let total = cartStore.totalPrice + shippingFee.value;
  if (appliedVoucher.value) {
    total -= appliedVoucher.value.giamGia;
  }
  return Math.max(total, 0);
});

// Prefill user info
onMounted(() => {
  if (authStore.user) {
    form.value.tenNguoiNhan = authStore.user.hoTen || "";
    form.value.soDienThoai = authStore.user.soDienThoai || "";
    form.value.diaChiGiaoHang = authStore.user.diaChi || "";
  }
  // Redirect if cart empty
  if (cartStore.items.length === 0) {
    router.replace("/cart");
  }
});

const applyVoucher = async () => {
  voucherError.value = "";
  voucherLoading.value = true;
  try {
    const res: any = await voucherService.getByCode(voucherCode.value.trim());
    if (res.success && res.data) {
      appliedVoucher.value = res.data;
      form.value.maVoucher = res.data.maVoucher;
    } else {
      voucherError.value = res.message || "Mã voucher không hợp lệ";
    }
  } catch (e: any) {
    voucherError.value = e.response?.data?.message || "Mã voucher không tồn tại hoặc đã hết hạn";
  } finally {
    voucherLoading.value = false;
  }
};

const removeVoucher = () => {
  appliedVoucher.value = null;
  form.value.maVoucher = undefined;
  voucherCode.value = "";
  voucherError.value = "";
};

const submitOrder = async () => {
  errorMsg.value = "";
  if (!form.value.tenNguoiNhan.trim()) { errorMsg.value = "Vui lòng nhập tên người nhận"; return; }
  if (!form.value.soDienThoai.trim()) { errorMsg.value = "Vui lòng nhập số điện thoại"; return; }
  if (!form.value.diaChiGiaoHang.trim()) { errorMsg.value = "Vui lòng nhập địa chỉ giao hàng"; return; }

  // Must be logged in to order
  if (!authStore.isAuthenticated()) {
    router.push({ name: "client-login", query: { redirect: "/checkout" } });
    return;
  }

  // Sync guest cart to server first
  await cartStore.syncGuestCartToServer();
  await cartStore.loadCart();

  submitting.value = true;
  try {
    const res: any = await donHangService.createOrder({
      maPTTT: form.value.maPTTT,
      maPTVC: form.value.maPTVC,
      maVoucher: form.value.maVoucher,
      tenNguoiNhan: form.value.tenNguoiNhan,
      soDienThoai: form.value.soDienThoai,
      diaChiGiaoHang: form.value.diaChiGiaoHang,
    });
    if (res.success && res.data) {
      // Clear cart and redirect to success
      await cartStore.loadCart();
      router.push({ name: "order-success", params: { maDH: res.data.maDonHang } });
    } else {
      errorMsg.value = res.message || "Đặt hàng thất bại";
    }
  } catch (e: any) {
    errorMsg.value = e.response?.data?.message || "Đặt hàng thất bại. Vui lòng thử lại.";
  } finally {
    submitting.value = false;
  }
};
</script>

<style scoped>
.fade-enter-active { transition: all 0.3s ease-out; }
.fade-leave-active { transition: all 0.2s ease-in; }
.fade-enter-from, .fade-leave-to { opacity: 0; transform: translateY(-4px); }
</style>
