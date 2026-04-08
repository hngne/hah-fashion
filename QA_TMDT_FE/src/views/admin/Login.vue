<template>
  <div class="min-h-screen flex items-center justify-center bg-[#F8FAFC]">
    <div
      class="relative w-full max-w-[420px] bg-white rounded-2xl shadow-[0_8px_30px_rgb(0,0,0,0.04)] p-10 border border-gray-100"
    >
      <!-- Logo & Title -->
      <div class="flex flex-col items-center mb-8">
        <div
          class="w-14 h-14 bg-blue-50 rounded-full flex items-center justify-center mb-6"
        >
          <svg
            class="w-7 h-7 text-blue-600"
            viewBox="0 0 24 24"
            fill="currentColor"
          >
            <!-- Diamond shape matching the image roughly, but using an SVG path -->
            <path
              d="M12 2L2 9l10 13L22 9L12 2zM4.3 9L12 3.8L19.7 9L12 19.3L4.3 9z"
            />
            <path d="M12 5l-4 4h8zM8.5 10L12 17l3.5-7z" />
          </svg>
        </div>
        <h2
          class="text-[22px] font-extrabold text-[#111827] mb-2 tracking-tight"
        >
          HaHFashion CMS
        </h2>
        <p class="text-[13px] text-gray-500 font-medium">
          Hệ thống Quản trị Nội bộ Bảo mật
        </p>
      </div>

      <!-- Form -->
      <form @submit.prevent="handleLogin" class="space-y-6">
        <!-- Username -->
        <div class="space-y-1.5">
          <label
            class="block text-xs font-bold text-gray-700 uppercase tracking-wide"
            >Tên đăng nhập</label
          >
          <div class="relative">
            <div
              class="absolute inset-y-0 left-0 pl-3.5 flex items-center pointer-events-none"
            >
              <UserIcon class="h-4 w-4 text-gray-400" stroke-width="2.5" />
            </div>
            <input
              v-model="username"
              type="text"
              required
              class="block w-full pl-10 pr-3 py-2.5 bg-[#F8FAFC] border border-gray-200 rounded-lg text-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-600/20 focus:border-blue-600 transition-colors font-medium text-gray-800"
              placeholder="Nhập tên đăng nhập quản trị"
            />
          </div>
        </div>

        <!-- Password -->
        <div class="space-y-1.5">
          <div class="flex items-center justify-between">
            <label
              class="block text-xs font-bold text-gray-700 uppercase tracking-wide"
              >Mật khẩu</label
            >
            <a
              href="#"
              class="text-xs font-semibold text-blue-600 hover:text-blue-700 transition"
              >Quên mật khẩu?</a
            >
          </div>
          <div class="relative">
            <div
              class="absolute inset-y-0 left-0 pl-3.5 flex items-center pointer-events-none"
            >
              <LockIcon class="h-4 w-4 text-gray-400" stroke-width="2.5" />
            </div>
            <input
              v-model="password"
              :type="showPassword ? 'text' : 'password'"
              required
              class="block w-full pl-10 pr-10 py-2.5 bg-[#F8FAFC] border border-gray-200 rounded-lg text-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-600/20 focus:border-blue-600 transition-colors font-medium text-gray-800"
              placeholder="Nhập mật khẩu bảo mật"
            />
            <div
              class="absolute inset-y-0 right-0 pr-3.5 flex items-center cursor-pointer"
              @click="showPassword = !showPassword"
            >
              <EyeIcon
                v-if="!showPassword"
                class="h-4 w-4 text-gray-400 hover:text-gray-600 transition-colors"
                stroke-width="2.5"
              />
              <EyeOffIcon
                v-else
                class="h-4 w-4 text-gray-400 hover:text-gray-600 transition-colors"
                stroke-width="2.5"
              />
            </div>
          </div>
        </div>

        <!-- Remember Me -->
        <div class="flex items-center pt-1">
          <input
            id="remember-me"
            type="checkbox"
            class="h-4 w-4 bg-[#F8FAFC] border-gray-300 rounded text-blue-600 focus:ring-blue-600 cursor-pointer"
          />
          <label
            for="remember-me"
            class="ml-2.5 block text-[13px] font-medium text-gray-600 cursor-pointer"
          >
            Ghi nhớ thiết bị này trong 30 ngày
          </label>
        </div>

        <!-- Error Alert -->
        <div
          v-if="errorMessage"
          class="bg-red-50 text-red-600 p-3 rounded-lg text-sm flex items-start font-medium border border-red-100"
        >
          <AlertCircleIcon class="h-5 w-5 mr-2 flex-shrink-0" />
          <span>{{ errorMessage }}</span>
        </div>

        <!-- Submit Button -->
        <button
          type="submit"
          :disabled="isLoading"
          class="w-full flex justify-center items-center py-3.5 px-4 rounded-lg shadow-[0_4px_14px_0_rgba(29,78,216,0.39)] text-sm font-bold text-white bg-blue-600 hover:bg-blue-700 hover:shadow-[0_6px_20px_rgba(29,78,216,0.23)] focus:outline-none focus:ring-4 focus:ring-blue-500/30 disabled:opacity-50 transition-all duration-200"
        >
          <LogInIcon
            v-if="!isLoading"
            class="h-4 w-4 mr-2"
            stroke-width="2.5"
          />
          <Loader2Icon
            v-else
            class="h-4 w-4 mr-2 animate-spin"
            stroke-width="2.5"
          />
          <span>{{
            isLoading ? "Đang xác thực..." : "Đăng nhập Hệ thống"
          }}</span>
        </button>
      </form>

      <!-- Footer Indicator -->
      <div
        class="mt-8 pt-6 border-t border-gray-100 flex items-center justify-center space-x-1.5 text-[10px] font-bold text-slate-400 uppercase tracking-widest"
      >
        <ShieldCheckIcon class="h-3 w-3" stroke-width="2.5" />
        <span>Chứng nhận Bảo mật Tiêu chuẩn V2.4</span>
      </div>
    </div>

    <!-- Page Footer -->
    <div class="absolute bottom-8 w-full text-center">
      <p class="text-[10px] text-gray-400 font-bold uppercase tracking-widest">
        © 2026 HaHFashion Administration. Mọi hoạt động đều được giám sát bảo
        mật.
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../../stores/auth";
import { authService } from "../../services/auth.service";
import {
  User as UserIcon,
  Lock as LockIcon,
  Eye as EyeIcon,
  EyeOff as EyeOffIcon,
  LogIn as LogInIcon,
  ShieldCheck as ShieldCheckIcon,
  AlertCircle as AlertCircleIcon,
  Loader2 as Loader2Icon,
} from "lucide-vue-next";

const router = useRouter();
const authStore = useAuthStore();

const username = ref("");
const password = ref("");
const showPassword = ref(false);
const errorMessage = ref("");
const isLoading = ref(false);

const handleLogin = async () => {
  try {
    isLoading.value = true;
    errorMessage.value = "";

    const res = await authService.login({
      tenDangNhap: username.value,
      matKhau: password.value,
    });

    if (res.success && res.data) {
      authStore.setAuth(res.data.token, res.data);
      if (res.data.vaiTro === "admin") {
        router.push("/admin");
      } else {
        errorMessage.value =
          "Chỉ tài khoản Quản trị viên mới được phép truy cập.";
        authStore.logout();
      }
    } else {
      errorMessage.value = res.message || "Đăng nhập thất bại.";
    }
  } catch (error: any) {
    if (error.response?.data?.message) {
      errorMessage.value = error.response.data.message;
    } else {
      errorMessage.value = "Không thể kết nối đến máy chủ bảo mật.";
    }
  } finally {
    isLoading.value = false;
  }
};
</script>
