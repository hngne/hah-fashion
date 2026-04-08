<template>
  <div class="min-h-[70vh] bg-[#FAFAFA] flex items-center justify-center py-12 px-4">
    <div class="w-full max-w-lg text-center">
      <!-- Loading -->
      <div v-if="loading" class="py-16">
        <Loader2Icon class="h-8 w-8 animate-spin text-blue-500 mx-auto" />
      </div>

      <template v-else>
        <!-- Success Icon -->
        <div class="w-20 h-20 mx-auto bg-green-100 rounded-full flex items-center justify-center mb-6">
          <CheckCircleIcon class="w-10 h-10 text-green-500" />
        </div>

        <h1 class="text-2xl sm:text-3xl font-black text-[#111827]">Đặt hàng thành công!</h1>
        <p class="text-gray-500 mt-2">Cảm ơn bạn đã mua sắm tại HaHFashion</p>

        <!-- Order Info -->
        <div v-if="order" class="bg-white rounded-2xl p-6 shadow-sm mt-8 text-left">
          <div class="space-y-3 text-sm">
            <div class="flex justify-between">
              <span class="text-gray-400">Mã đơn hàng</span>
              <span class="font-bold text-blue-600">{{ order.maDonHang }}</span>
            </div>
            <div class="flex justify-between">
              <span class="text-gray-400">Ngày đặt</span>
              <span class="font-medium text-[#111827]">{{ formatDate(order.ngayDatHang) }}</span>
            </div>
            <div class="flex justify-between">
              <span class="text-gray-400">Trạng thái</span>
              <span class="font-semibold text-orange-500">{{ order.trangThaiDonHang }}</span>
            </div>
            <div class="flex justify-between">
              <span class="text-gray-400">Địa chỉ giao hàng</span>
              <span class="font-medium text-[#111827] text-right max-w-[60%]">{{ order.diaChiGiaoHang }}</span>
            </div>
            <hr class="border-gray-100" />
            <div class="flex justify-between text-lg font-black">
              <span class="text-[#111827]">Tổng thanh toán</span>
              <span class="text-red-600">{{ formatPrice(order.thanhToan) }}</span>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="flex flex-col sm:flex-row items-center justify-center gap-3 mt-8">
          <RouterLink to="/orders"
            class="px-6 py-3 bg-[#111827] text-white font-bold text-sm rounded-xl hover:bg-blue-600 transition">
            Xem đơn hàng của tôi
          </RouterLink>
          <RouterLink to="/"
            class="px-6 py-3 border-2 border-gray-200 text-gray-600 font-bold text-sm rounded-xl hover:border-[#111827] hover:text-[#111827] transition">
            Tiếp tục mua sắm
          </RouterLink>
        </div>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute, RouterLink } from "vue-router";
import { CheckCircle as CheckCircleIcon, Loader2 as Loader2Icon } from "lucide-vue-next";
import { donHangService } from "../../services/donhang.service";

const route = useRoute();
const props = defineProps<{ maDH?: string }>();

const order = ref<any>(null);
const loading = ref(true);

const formatPrice = (v: number) => v ? v.toLocaleString("vi-VN") + "đ" : "0đ";
const formatDate = (d: string) => {
  if (!d) return "";
  return new Date(d).toLocaleDateString("vi-VN", { day: "2-digit", month: "2-digit", year: "numeric", hour: "2-digit", minute: "2-digit" });
};

onMounted(async () => {
  const maDH = props.maDH || (route.params.maDH as string);
  if (maDH) {
    try {
      const res: any = await donHangService.getById(maDH);
      if (res.success && res.data) {
        order.value = res.data;
      }
    } catch (e) {
      console.warn("Failed to load order", e);
    }
  }
  loading.value = false;
});
</script>
