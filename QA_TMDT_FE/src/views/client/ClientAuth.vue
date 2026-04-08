<template>
  <div class="min-h-[80vh] flex items-center justify-center py-12 px-4">
    <div class="w-full max-w-md">
      <!-- Tabs -->
      <div class="flex bg-white rounded-2xl shadow-sm overflow-hidden mb-0">
        <button @click="mode = 'login'"
          :class="['flex-1 py-3.5 text-sm font-bold transition',
            mode === 'login' ? 'bg-[#111827] text-white' : 'bg-white text-gray-500 hover:bg-gray-50']">
          Đăng nhập
        </button>
        <button @click="mode = 'register'"
          :class="['flex-1 py-3.5 text-sm font-bold transition',
            mode === 'register' ? 'bg-[#111827] text-white' : 'bg-white text-gray-500 hover:bg-gray-50']">
          Đăng ký
        </button>
      </div>

      <!-- Form Card -->
      <div class="bg-white rounded-2xl shadow-sm p-6 sm:p-8 mt-4">
        <!-- Error / Success -->
        <transition name="fade">
          <div v-if="errorMsg" class="flex items-center space-x-2 px-4 py-3 rounded-xl bg-red-50 text-red-700 text-sm font-medium mb-4">
            <AlertCircleIcon class="w-5 h-5 flex-shrink-0" />
            <span>{{ errorMsg }}</span>
          </div>
        </transition>
        <transition name="fade">
          <div v-if="successMsg" class="flex items-center space-x-2 px-4 py-3 rounded-xl bg-green-50 text-green-700 text-sm font-medium mb-4">
            <CheckCircleIcon class="w-5 h-5 flex-shrink-0" />
            <span>{{ successMsg }}</span>
          </div>
        </transition>

        <!-- LOGIN FORM -->
        <form v-if="mode === 'login'" @submit.prevent="handleLogin" class="space-y-4">
          <div>
            <label class="block text-sm font-semibold text-[#111827] mb-1.5">Tên đăng nhập</label>
            <div class="relative">
              <UserIcon class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
              <input v-model="loginForm.tenDangNhap" type="text" required placeholder="Nhập tên đăng nhập"
                class="w-full pl-10 pr-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
            </div>
          </div>
          <div>
            <label class="block text-sm font-semibold text-[#111827] mb-1.5">Mật khẩu</label>
            <div class="relative">
              <LockIcon class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
              <input v-model="loginForm.matKhau" :type="showPw ? 'text' : 'password'" required placeholder="Nhập mật khẩu"
                class="w-full pl-10 pr-10 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
              <button type="button" @click="showPw = !showPw" class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600">
                <EyeIcon v-if="!showPw" class="w-4 h-4" />
                <EyeOffIcon v-else class="w-4 h-4" />
              </button>
            </div>
          </div>
          <button type="submit" :disabled="submitting"
            class="w-full py-3 bg-[#111827] text-white font-bold text-sm rounded-xl hover:bg-blue-600 transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center space-x-2">
            <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
            <span>{{ submitting ? 'Đang xử lý...' : 'Đăng nhập' }}</span>
          </button>
        </form>

        <!-- REGISTER FORM -->
        <form v-else @submit.prevent="handleRegister" class="space-y-4">
          <div>
            <label class="block text-sm font-semibold text-[#111827] mb-1.5">Tên đăng nhập <span class="text-red-500">*</span></label>
            <div class="relative">
              <UserIcon class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
              <input v-model="registerForm.tenDangNhap" type="text" required placeholder="Nhập tên đăng nhập"
                class="w-full pl-10 pr-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
            </div>
          </div>
          <div>
            <label class="block text-sm font-semibold text-[#111827] mb-1.5">Mật khẩu <span class="text-red-500">*</span></label>
            <div class="relative">
              <LockIcon class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
              <input v-model="registerForm.matKhau" :type="showPw ? 'text' : 'password'" required placeholder="Nhập mật khẩu"
                class="w-full pl-10 pr-10 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
              <button type="button" @click="showPw = !showPw" class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600">
                <EyeIcon v-if="!showPw" class="w-4 h-4" />
                <EyeOffIcon v-else class="w-4 h-4" />
              </button>
            </div>
          </div>
          <div>
            <label class="block text-sm font-semibold text-[#111827] mb-1.5">Họ tên</label>
            <input v-model="registerForm.hoTen" type="text" placeholder="Nguyễn Văn A"
              class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
          </div>
          <div class="grid grid-cols-2 gap-3">
            <div>
              <label class="block text-sm font-semibold text-[#111827] mb-1.5">Email</label>
              <input v-model="registerForm.email" type="email" placeholder="email@example.com"
                class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
            </div>
            <div>
              <label class="block text-sm font-semibold text-[#111827] mb-1.5">Số điện thoại</label>
              <input v-model="registerForm.soDienThoai" type="text" placeholder="0901234567"
                class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
            </div>
          </div>
          <div>
            <label class="block text-sm font-semibold text-[#111827] mb-1.5">Địa chỉ</label>
            <input v-model="registerForm.diaChi" type="text" placeholder="123 Đường ABC, Quận 1, TP.HCM"
              class="w-full px-4 py-2.5 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/30 focus:border-blue-500 transition" />
          </div>
          <button type="submit" :disabled="submitting"
            class="w-full py-3 bg-[#111827] text-white font-bold text-sm rounded-xl hover:bg-blue-600 transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center space-x-2">
            <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
            <span>{{ submitting ? 'Đang xử lý...' : 'Đăng ký' }}</span>
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import {
  User as UserIcon, Lock as LockIcon, Eye as EyeIcon,
  EyeOff as EyeOffIcon, AlertCircle as AlertCircleIcon,
  CheckCircle as CheckCircleIcon, Loader2 as Loader2Icon,
} from "lucide-vue-next";
import { authService } from "../../services/auth.service";
import { useAuthStore } from "../../stores/auth";
import { useCartStore } from "../../stores/cart";

const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();
const cartStore = useCartStore();

const mode = ref<"login" | "register">((route.query.tab as string) === "register" ? "register" : "login");
const showPw = ref(false);
const submitting = ref(false);
const errorMsg = ref("");
const successMsg = ref("");

const loginForm = ref({ tenDangNhap: "", matKhau: "" });
const registerForm = ref({
  tenDangNhap: "",
  matKhau: "",
  hoTen: "",
  email: "",
  soDienThoai: "",
  diaChi: "",
});

const handleLogin = async () => {
  errorMsg.value = "";
  successMsg.value = "";
  submitting.value = true;
  try {
    const res: any = await authService.login(loginForm.value);
    if (res.success && res.data) {
      authStore.setAuth(res.data.token, res.data);
      // Sync guest cart to server
      await cartStore.syncGuestCartToServer();
      await cartStore.loadCart();
      // Redirect
      const redirect = (route.query.redirect as string) || "/";
      router.push(redirect);
    } else {
      errorMsg.value = res.message || "Đăng nhập thất bại";
    }
  } catch (e: any) {
    errorMsg.value = e.response?.data?.message || "Tên đăng nhập hoặc mật khẩu không đúng";
  } finally {
    submitting.value = false;
  }
};

const handleRegister = async () => {
  errorMsg.value = "";
  successMsg.value = "";
  submitting.value = true;
  try {
    const res: any = await authService.register(registerForm.value);
    if (res.success) {
      // If BE returns token directly, use it
      if (res.data?.token) {
        authStore.setAuth(res.data.token, res.data);
        await cartStore.syncGuestCartToServer();
        await cartStore.loadCart();
        const redirect = (route.query.redirect as string) || "/";
        router.push(redirect);
      } else {
        // Fallback: auto-login with the same credentials
        try {
          const loginRes: any = await authService.login({
            tenDangNhap: registerForm.value.tenDangNhap,
            matKhau: registerForm.value.matKhau,
          });
          if (loginRes.success && loginRes.data) {
            authStore.setAuth(loginRes.data.token, loginRes.data);
            await cartStore.syncGuestCartToServer();
            await cartStore.loadCart();
            const redirect = (route.query.redirect as string) || "/";
            router.push(redirect);
          }
        } catch {
          // Auto-login failed, show success and let user login manually
          successMsg.value = "Đăng ký thành công! Vui lòng đăng nhập.";
          mode.value = "login";
          loginForm.value.tenDangNhap = registerForm.value.tenDangNhap;
        }
      }
    } else {
      errorMsg.value = res.message || "Đăng ký thất bại";
    }
  } catch (e: any) {
    errorMsg.value = e.response?.data?.message || "Đăng ký thất bại. Tên đăng nhập có thể đã tồn tại.";
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
