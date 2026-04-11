<template>
  <div class="min-h-[70vh] bg-[var(--bg-page)]">
    <section class="border-b border-[var(--ui-border)] bg-white">
      <div class="mx-auto max-w-[1400px] px-4 py-5 sm:px-6 lg:px-8">
        <nav class="flex items-center gap-2 text-sm text-slate-400">
          <RouterLink to="/" class="transition hover:text-[var(--accent)]">Trang chủ</RouterLink>
          <ChevronRightIcon class="h-3.5 w-3.5" />
          <RouterLink to="/cart" class="transition hover:text-[var(--accent)]">Giỏ hàng</RouterLink>
          <ChevronRightIcon class="h-3.5 w-3.5" />
          <span class="font-medium text-slate-700">Thanh toán</span>
        </nav>
        <h1 class="mt-2 text-2xl font-black tracking-tight text-slate-950 sm:text-3xl">
          Thanh toán
        </h1>
      </div>
    </section>

    <div class="mx-auto max-w-[1400px] px-4 py-8 sm:px-6 lg:px-8">
      <div
        v-if="message"
        :class="messageClass"
        class="mb-6 rounded-2xl px-4 py-3 text-sm font-medium"
      >
        {{ message }}
      </div>

      <div class="grid grid-cols-1 gap-8 lg:grid-cols-[minmax(0,1fr)_360px]">
        <section class="space-y-6">
          <article class="rounded-[32px] border border-[var(--ui-border)] bg-white p-6 shadow-[var(--shadow-soft)]">
            <h3 class="text-lg font-semibold text-slate-900">Thông tin giao hàng</h3>
            <div class="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2">
              <div class="sm:col-span-2">
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Tên người nhận</label>
                <input
                  v-model="form.tenNguoiNhan"
                  type="text"
                  placeholder="Nguyễn Văn A"
                  :class="inputClass(fieldErrors.tenNguoiNhan)"
                />
                <p v-if="fieldErrors.tenNguoiNhan" class="mt-1 text-xs font-medium text-rose-600">
                  {{ fieldErrors.tenNguoiNhan }}
                </p>
              </div>

              <div>
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Số điện thoại</label>
                <input
                  v-model="form.soDienThoai"
                  type="text"
                  placeholder="0901234567"
                  :class="inputClass(fieldErrors.soDienThoai)"
                />
                <p v-if="fieldErrors.soDienThoai" class="mt-1 text-xs font-medium text-rose-600">
                  {{ fieldErrors.soDienThoai }}
                </p>
              </div>

              <div>
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Email</label>
                <input
                  :value="authStore.user?.email || ''"
                  disabled
                  type="email"
                  class="w-full rounded-2xl border border-slate-100 bg-slate-50 px-4 py-3 text-sm text-slate-400 outline-none"
                />
              </div>

              <div class="sm:col-span-2">
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Địa chỉ giao hàng</label>
                <textarea
                  v-model="form.diaChiGiaoHang"
                  rows="3"
                  placeholder="Số nhà, đường, phường, quận, tỉnh/thành"
                  :class="textareaClass(fieldErrors.diaChiGiaoHang)"
                />
                <p v-if="fieldErrors.diaChiGiaoHang" class="mt-1 text-xs font-medium text-rose-600">
                  {{ fieldErrors.diaChiGiaoHang }}
                </p>
              </div>
            </div>
          </article>

          <article class="rounded-[32px] border border-[var(--ui-border)] bg-white p-6 shadow-[var(--shadow-soft)]">
            <h3 class="text-lg font-semibold text-slate-900">Phương thức thanh toán</h3>
            <div class="mt-5 space-y-3">
              <label
                v-for="method in paymentMethods"
                :key="method.id"
                class="flex cursor-pointer items-start gap-3 rounded-2xl border border-[var(--ui-border)] bg-slate-50 px-4 py-4 transition hover:border-slate-300"
              >
                <input v-model="form.maPTTT" class="mt-1" type="radio" :value="method.id" />
                <div>
                  <p class="text-sm font-semibold text-slate-900">{{ method.name }}</p>
                  <p class="mt-1 text-sm text-slate-500">{{ method.desc }}</p>
                </div>
              </label>
            </div>
          </article>

          <article class="rounded-[32px] border border-[var(--ui-border)] bg-white p-6 shadow-[var(--shadow-soft)]">
            <h3 class="text-lg font-semibold text-slate-900">Phương thức vận chuyển</h3>
            <div class="mt-5 space-y-3">
              <label
                v-for="method in shippingMethods"
                :key="method.id"
                class="flex cursor-pointer items-start justify-between gap-3 rounded-2xl border border-[var(--ui-border)] bg-slate-50 px-4 py-4 transition hover:border-slate-300"
              >
                <div class="flex gap-3">
                  <input v-model="form.maPTVC" class="mt-1" type="radio" :value="method.id" />
                  <div>
                    <p class="text-sm font-semibold text-slate-900">{{ method.name }}</p>
                    <p class="mt-1 text-sm text-slate-500">{{ method.desc }}</p>
                  </div>
                </div>
                <span class="text-sm font-semibold text-slate-900">
                  {{ method.fee ? formatMoney(method.fee) : "Miễn phí" }}
                </span>
              </label>
            </div>
          </article>

          <article class="rounded-[32px] border border-[var(--ui-border)] bg-white p-6 shadow-[var(--shadow-soft)]">
            <div class="flex flex-col gap-3 sm:flex-row sm:items-end">
              <div class="flex-1">
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Voucher</label>
                <input
                  v-model="voucherCode"
                  type="text"
                  placeholder="Nhập mã voucher"
                  class="w-full rounded-2xl border border-[var(--ui-border)] bg-white px-4 py-3 text-sm text-slate-900 outline-none transition focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]"
                />
              </div>
              <button
                type="button"
                :disabled="voucherLoading || !voucherCode.trim()"
                class="inline-flex items-center justify-center gap-2 rounded-2xl bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60"
                @click="applyVoucher"
              >
                <Loader2Icon v-if="voucherLoading" class="h-4 w-4 animate-spin" />
                Áp dụng
              </button>
            </div>

            <div
              v-if="appliedVoucher"
              class="mt-4 flex flex-col gap-3 rounded-2xl bg-emerald-50 px-4 py-4 text-sm text-emerald-700 sm:flex-row sm:items-center sm:justify-between"
            >
              <div>
                <p class="font-semibold">{{ appliedVoucher.tenVoucher || appliedVoucher.maVoucher }}</p>
                <p class="mt-1">Giảm {{ formatMoney(appliedVoucher.giamGia) }}</p>
              </div>
              <button
                type="button"
                class="text-left font-semibold text-rose-600 transition hover:opacity-80"
                @click="removeVoucher"
              >
                Bỏ voucher
              </button>
            </div>
          </article>
        </section>

        <aside>
          <div class="sticky top-24 rounded-[32px] border border-[var(--ui-border)] bg-white p-6 shadow-[var(--shadow-soft)]">
            <h3 class="text-lg font-semibold text-slate-900">Đơn hàng của bạn</h3>

            <div v-if="cartStore.items.length" class="mt-5 space-y-4">
              <div
                v-for="item in cartStore.items"
                :key="item.maCTSP"
                class="flex gap-3 rounded-2xl bg-slate-50 px-3 py-3"
              >
                <div class="h-16 w-16 overflow-hidden rounded-2xl bg-slate-100">
                  <img :src="item.anh || placeholderImage" class="h-full w-full object-cover" />
                </div>
                <div class="min-w-0 flex-1">
                  <p class="text-sm font-semibold text-slate-900 line-clamp-2">{{ item.tenSP }}</p>
                  <p class="mt-1 text-xs text-slate-500">{{ item.tenMau }} / {{ item.tenSize }} x {{ item.soLuong }}</p>
                </div>
                <span class="text-sm font-semibold text-slate-900">{{ formatMoney(item.thanhTien) }}</span>
              </div>
            </div>

            <div v-else class="mt-5 rounded-2xl border border-dashed border-[var(--ui-border)] px-4 py-10 text-center text-sm text-slate-500">
              Giỏ hàng đang trống.
            </div>

            <div class="mt-6 space-y-3 text-sm">
              <div class="flex items-center justify-between text-slate-500">
                <span>Tạm tính</span>
                <span class="font-semibold text-slate-900">{{ formatMoney(cartStore.totalPrice) }}</span>
              </div>
              <div class="flex items-center justify-between text-slate-500">
                <span>Phí vận chuyển</span>
                <span class="font-semibold text-slate-900">{{ shippingFee ? formatMoney(shippingFee) : "Miễn phí" }}</span>
              </div>
              <div v-if="appliedVoucher" class="flex items-center justify-between text-emerald-600">
                <span>Giảm giá</span>
                <span class="font-semibold">-{{ formatMoney(appliedVoucher.giamGia) }}</span>
              </div>
              <div class="border-t border-slate-100 pt-3">
                <div class="flex items-center justify-between">
                  <span class="text-base font-semibold text-slate-900">Tổng thanh toán</span>
                  <span class="text-xl font-black text-rose-600">{{ formatMoney(finalTotal) }}</span>
                </div>
              </div>
            </div>

            <button
              type="button"
              :disabled="submitting || !cartStore.items.length"
              class="mt-6 inline-flex w-full items-center justify-center gap-2 rounded-2xl bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60"
              @click="submitOrder"
            >
              <Loader2Icon v-if="submitting" class="h-4 w-4 animate-spin" />
              Đặt hàng
            </button>
          </div>
        </aside>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { RouterLink, useRouter } from "vue-router";
import { ChevronRight as ChevronRightIcon, Loader2 as Loader2Icon } from "lucide-vue-next";
import { donHangService } from "../../services/donhang.service";
import { extractErrorMessage, hasFieldErrors } from "../../services/service-helpers";
import { voucherService } from "../../services/voucher.service";
import { useAuthStore } from "../../stores/auth";
import { useCartStore } from "../../stores/cart";
import type { CheckoutFormValues, FieldErrorMap, Voucher } from "../../types";
import { validateCheckoutForm } from "../../utils/validators";

const router = useRouter();
const authStore = useAuthStore();
const cartStore = useCartStore();

const placeholderImage = "https://via.placeholder.com/100x100?text=SP";

const paymentMethods = [
  { id: 1, name: "Thanh toán khi nhận hàng", desc: "Thanh toán bằng tiền mặt khi giao." },
  { id: 2, name: "Chuyển khoản ngân hàng", desc: "Thanh toán trước khi giao hàng." },
];

const shippingMethods = [
  { id: 1, name: "Giao hàng tiêu chuẩn", desc: "Từ 3 đến 5 ngày", fee: 0 },
  { id: 2, name: "Giao hàng nhanh", desc: "Từ 1 đến 2 ngày", fee: 30000 },
];

const form = ref<CheckoutFormValues>({
  tenNguoiNhan: "",
  soDienThoai: "",
  diaChiGiaoHang: "",
  maPTTT: 1,
  maPTVC: 1,
  maVoucher: undefined,
});

const fieldErrors = ref<FieldErrorMap>({});
const message = ref("");
const messageIsSuccess = ref(false);
const voucherCode = ref("");
const voucherLoading = ref(false);
const submitting = ref(false);
const appliedVoucher = ref<Voucher | null>(null);

const shippingFee = computed(() => {
  return shippingMethods.find((method) => method.id === form.value.maPTVC)?.fee || 0;
});

const finalTotal = computed(() => {
  const discount = appliedVoucher.value?.giamGia || 0;
  return Math.max(cartStore.totalPrice + shippingFee.value - discount, 0);
});

const messageClass = computed(() =>
  messageIsSuccess.value
    ? "bg-emerald-50 text-emerald-700"
    : "bg-rose-50 text-rose-700",
);

function formatMoney(value: number) {
  return `${(value || 0).toLocaleString("vi-VN")} đ`;
}

function inputClass(errorText?: string) {
  return [
    "w-full rounded-2xl border bg-white px-4 py-3 text-sm text-slate-900 outline-none transition",
    errorText
      ? "border-rose-300 focus:border-rose-400 focus:ring-4 focus:ring-rose-100"
      : "border-[var(--ui-border)] focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]",
  ].join(" ");
}

function textareaClass(errorText?: string) {
  return [
    "w-full rounded-2xl border bg-white px-4 py-3 text-sm text-slate-900 outline-none transition",
    errorText
      ? "border-rose-300 focus:border-rose-400 focus:ring-4 focus:ring-rose-100"
      : "border-[var(--ui-border)] focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]",
  ].join(" ");
}

function setMessage(nextMessage: string, success: boolean) {
  message.value = nextMessage;
  messageIsSuccess.value = success;
}

async function applyVoucher() {
  voucherLoading.value = true;
  setMessage("", false);
  try {
    const response = await voucherService.getByCode(voucherCode.value.trim());
    appliedVoucher.value = response.data;
    form.value.maVoucher = response.data.maVoucher;
    setMessage("Đã áp dụng voucher.", true);
  } catch (caughtError) {
    appliedVoucher.value = null;
    form.value.maVoucher = undefined;
    setMessage(
      extractErrorMessage(caughtError, "Voucher không hợp lệ hoặc đã hết hạn."),
      false,
    );
  } finally {
    voucherLoading.value = false;
  }
}

function removeVoucher() {
  appliedVoucher.value = null;
  voucherCode.value = "";
  form.value.maVoucher = undefined;
}

async function submitOrder() {
  if (!authStore.isAuthenticated()) {
    router.push({ name: "client-login", query: { redirect: "/checkout" } });
    return;
  }

  if (!cartStore.items.length) {
    setMessage("Giỏ hàng đang trống, không thể đặt hàng.", false);
    return;
  }

  fieldErrors.value = validateCheckoutForm(form.value);
  if (hasFieldErrors(fieldErrors.value)) {
    setMessage("Vui lòng kiểm tra lại thông tin giao hàng.", false);
    return;
  }

  submitting.value = true;
  setMessage("", false);
  try {
    const response = await donHangService.createOrder(form.value);
    await cartStore.loadCart();
    router.push({ name: "order-success", params: { maDH: response.data.maDonHang } });
  } catch (caughtError) {
    setMessage(
      extractErrorMessage(caughtError, "Đặt hàng thất bại. Vui lòng thử lại."),
      false,
    );
  } finally {
    submitting.value = false;
  }
}

onMounted(async () => {
  await cartStore.loadCart();

  if (authStore.user) {
    form.value.tenNguoiNhan = authStore.user.hoTen || "";
    form.value.soDienThoai = authStore.user.soDienThoai || "";
    form.value.diaChiGiaoHang = authStore.user.diaChi || "";
  }

  if (!cartStore.items.length) {
    router.replace("/cart");
  }
});
</script>
