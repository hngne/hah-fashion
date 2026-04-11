<template>
  <section>
    <!-- Trust Bar -->
    <div class="bg-[#F5F5F5] py-10">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8">
        <div class="grid grid-cols-2 lg:grid-cols-4 gap-6">
          <div
            v-for="item in trustItems"
            :key="item.title"
            class="flex flex-col items-center text-center"
          >
            <div
              class="w-12 h-12 rounded-full bg-blue-50 flex items-center justify-center mb-3"
            >
              <component
                :is="item.icon"
                class="w-5 h-5 text-blue-600"
                stroke-width="2"
              />
            </div>
            <h4 class="text-sm font-bold text-[#111827]">{{ item.title }}</h4>
            <p class="text-xs text-gray-500 mt-0.5">{{ item.desc }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Newsletter -->
    <div class="bg-[#0F172A] py-14">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8">
        <div
          class="flex flex-col lg:flex-row items-center justify-between gap-8"
        >
          <div class="lg:max-w-md">
            <h3
              class="text-2xl sm:text-3xl font-black text-white leading-tight"
            >
              ĐĂNG KÝ NHẬN ƯU ĐÃI ĐỘC QUYỀN
            </h3>
            <p class="text-sm text-white/50 mt-2">
              Đăng ký để nhận thông tin sớm nhất về hàng mới, khuyến mãi và mã
              giảm giá độc quyền.
            </p>
          </div>
          <div class="w-full lg:w-auto">
            <div class="flex">
              <div class="relative flex-grow lg:w-80">
                <MailIcon
                  class="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-white/40"
                />
                <input
                  type="email"
                  v-model="email"
                  placeholder="Nhập email của bạn..."
                  class="w-full pl-10 pr-4 py-3 bg-white/10 border border-white/10 rounded-l-full text-sm text-white placeholder-white/40 focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500/30"
                />
              </div>
              <button
                @click="subscribe"
                class="px-6 py-3 bg-white text-[#0F172A] font-bold text-sm rounded-r-full hover:bg-gray-100 transition flex-shrink-0"
              >
                Đăng ký
              </button>
            </div>
            <p class="text-[10px] text-white/30 mt-2">
              Bằng cách đăng ký, bạn đồng ý với chính sách bảo mật của chúng
              tôi.
            </p>
            <p
              v-if="notice"
              class="mt-3 rounded-2xl bg-white/10 px-4 py-3 text-xs font-medium text-white/80"
            >
              {{ notice }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, markRaw } from "vue";
import {
  Mail as MailIcon,
  Truck as TruckIcon,
  RefreshCw as ReturnIcon,
  ShieldCheck as ShieldIcon,
  Headphones as SupportIcon,
} from "lucide-vue-next";
import { isValidEmail } from "../../utils/validators";

const email = ref("");
const notice = ref("");

const trustItems = [
  {
    icon: markRaw(TruckIcon),
    title: "Giao hàng miễn phí",
    desc: "Đơn hàng từ 500.000đ",
  },
  {
    icon: markRaw(ReturnIcon),
    title: "Đổi trả 30 ngày",
    desc: "Hoàn tiền bảo đảm",
  },
  {
    icon: markRaw(ShieldIcon),
    title: "Thanh toán an toàn",
    desc: "Bảo mật 100%",
  },
  { icon: markRaw(SupportIcon), title: "Hỗ trợ 24/7", desc: "Đội ngũ tận tâm" },
];

const subscribe = () => {
  if (!email.value.trim()) {
    notice.value = "Vui lòng nhập email để nhận thông báo mới.";
    return;
  }

  if (!isValidEmail(email.value)) {
    notice.value = "Email chưa đúng định dạng.";
    return;
  }

  notice.value = "Đã ghi nhận email của bạn. Chúng tôi sẽ gửi ưu đãi sớm nhất.";
  email.value = "";
};
</script>
