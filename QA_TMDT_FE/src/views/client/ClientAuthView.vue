<template>
  <div class="min-h-[80vh] bg-[var(--bg-page)] px-4 py-12">
    <div class="mx-auto max-w-md">
      <div class="grid grid-cols-2 overflow-hidden rounded-3xl border border-[var(--ui-border)] bg-white shadow-[var(--shadow-soft)]">
        <button
          type="button"
          :class="tabClass(mode === 'login')"
          @click="mode = 'login'"
        >
          Đăng nhập
        </button>
        <button
          type="button"
          :class="tabClass(mode === 'register')"
          @click="mode = 'register'"
        >
          Đăng ký
        </button>
      </div>

      <div class="mt-4 rounded-[32px] border border-[var(--ui-border)] bg-white p-6 shadow-[var(--shadow-soft)] sm:p-8">
        <div v-if="message" :class="messageClass" class="mb-5 rounded-2xl px-4 py-3 text-sm font-medium">
          {{ message }}
        </div>

        <form v-if="mode === 'login'" class="space-y-4" @submit.prevent="handleLogin">
          <div>
            <label class="mb-1.5 block text-sm font-semibold text-slate-700">Tên đăng nhập</label>
            <div class="relative">
              <UserIcon class="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
              <input
                v-model="loginForm.tenDangNhap"
                type="text"
                placeholder="Nhập tên đăng nhập"
                :class="inputClass(loginErrors.tenDangNhap)"
              />
            </div>
            <p v-if="loginErrors.tenDangNhap" class="mt-1 text-xs font-medium text-rose-600">
              {{ loginErrors.tenDangNhap }}
            </p>
          </div>

          <div>
            <label class="mb-1.5 block text-sm font-semibold text-slate-700">Mật khẩu</label>
            <div class="relative">
              <LockIcon class="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
              <input
                v-model="loginForm.matKhau"
                :type="showPassword ? 'text' : 'password'"
                placeholder="Nhập mật khẩu"
                :class="inputClass(loginErrors.matKhau)"
              />
              <button
                type="button"
                class="absolute right-4 top-1/2 -translate-y-1/2 text-slate-400 transition hover:text-slate-700"
                @click="showPassword = !showPassword"
              >
                <EyeOffIcon v-if="showPassword" class="h-4 w-4" />
                <EyeIcon v-else class="h-4 w-4" />
              </button>
            </div>
            <p v-if="loginErrors.matKhau" class="mt-1 text-xs font-medium text-rose-600">
              {{ loginErrors.matKhau }}
            </p>
          </div>

          <button
            type="submit"
            :disabled="submitting"
            class="inline-flex w-full items-center justify-center gap-2 rounded-2xl bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60"
          >
            <Loader2Icon v-if="submitting" class="h-4 w-4 animate-spin" />
            Đăng nhập
          </button>
        </form>

        <form v-else class="space-y-4" @submit.prevent="handleRegister">
          <div>
            <label class="mb-1.5 block text-sm font-semibold text-slate-700">Tên đăng nhập</label>
            <div class="relative">
              <UserIcon class="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
              <input
                v-model="registerForm.tenDangNhap"
                type="text"
                placeholder="Nhập tên đăng nhập"
                :class="inputClass(registerErrors.tenDangNhap)"
              />
            </div>
            <p v-if="registerErrors.tenDangNhap" class="mt-1 text-xs font-medium text-rose-600">
              {{ registerErrors.tenDangNhap }}
            </p>
          </div>

          <div>
            <label class="mb-1.5 block text-sm font-semibold text-slate-700">Mật khẩu</label>
            <div class="relative">
              <LockIcon class="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
              <input
                v-model="registerForm.matKhau"
                :type="showPassword ? 'text' : 'password'"
                placeholder="Tối thiểu 6 ký tự"
                :class="inputClass(registerErrors.matKhau)"
              />
            </div>
            <p v-if="registerErrors.matKhau" class="mt-1 text-xs font-medium text-rose-600">
              {{ registerErrors.matKhau }}
            </p>
          </div>

          <div>
            <label class="mb-1.5 block text-sm font-semibold text-slate-700">Họ tên</label>
            <input
              v-model="registerForm.hoTen"
              type="text"
              placeholder="Nguyễn Văn A"
              :class="inputClass()"
            />
          </div>

          <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
            <div>
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Email</label>
              <input
                v-model="registerForm.email"
                type="email"
                placeholder="email@example.com"
                :class="inputClass(registerErrors.email)"
              />
              <p v-if="registerErrors.email" class="mt-1 text-xs font-medium text-rose-600">
                {{ registerErrors.email }}
              </p>
            </div>
            <div>
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Số điện thoại</label>
              <input
                v-model="registerForm.soDienThoai"
                type="text"
                placeholder="0901234567"
                :class="inputClass(registerErrors.soDienThoai)"
              />
              <p
                v-if="registerErrors.soDienThoai"
                class="mt-1 text-xs font-medium text-rose-600"
              >
                {{ registerErrors.soDienThoai }}
              </p>
            </div>
          </div>

          <div>
            <label class="mb-1.5 block text-sm font-semibold text-slate-700">Địa chỉ</label>
            <input
              v-model="registerForm.diaChi"
              type="text"
              placeholder="Địa chỉ liên hệ"
              :class="inputClass()"
            />
          </div>

          <button
            type="submit"
            :disabled="submitting"
            class="inline-flex w-full items-center justify-center gap-2 rounded-2xl bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60"
          >
            <Loader2Icon v-if="submitting" class="h-4 w-4 animate-spin" />
            Đăng ký
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import {
  Eye as EyeIcon,
  EyeOff as EyeOffIcon,
  Loader2 as Loader2Icon,
  Lock as LockIcon,
  User as UserIcon,
} from "lucide-vue-next";
import { authService } from "../../services/auth.service";
import { extractErrorMessage, hasFieldErrors } from "../../services/service-helpers";
import { useAuthStore } from "../../stores/auth";
import { useCartStore } from "../../stores/cart";
import type { FieldErrorMap, LoginFormValues, RegisterFormValues } from "../../types";
import { validateLoginForm, validateRegisterForm } from "../../utils/validators";

const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();
const cartStore = useCartStore();

const mode = ref<"login" | "register">(
  (route.query.tab as string) === "register" ? "register" : "login",
);
const showPassword = ref(false);
const submitting = ref(false);
const message = ref("");
const messageIsSuccess = ref(false);

const loginForm = ref<LoginFormValues>({
  tenDangNhap: "",
  matKhau: "",
});

const registerForm = ref<RegisterFormValues>({
  tenDangNhap: "",
  matKhau: "",
  hoTen: "",
  email: "",
  soDienThoai: "",
  diaChi: "",
});

const loginErrors = ref<FieldErrorMap>({});
const registerErrors = ref<FieldErrorMap>({});

const messageClass = computed(() =>
  messageIsSuccess.value
    ? "bg-emerald-50 text-emerald-700"
    : "bg-rose-50 text-rose-700",
);

function inputClass(errorText?: string) {
  return [
    "w-full rounded-2xl border bg-white py-3 pl-11 pr-4 text-sm text-slate-900 outline-none transition",
    errorText
      ? "border-rose-300 focus:border-rose-400 focus:ring-4 focus:ring-rose-100"
      : "border-[var(--ui-border)] focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]",
  ].join(" ");
}

function tabClass(active: boolean) {
  return [
    "px-4 py-3 text-sm font-semibold transition",
    active
      ? "bg-slate-950 text-white"
      : "bg-white text-slate-500 hover:bg-slate-50 hover:text-slate-900",
  ].join(" ");
}

function setMessage(nextMessage: string, success: boolean) {
  message.value = nextMessage;
  messageIsSuccess.value = success;
}

function redirectAfterAuth() {
  const redirect = (route.query.redirect as string) || "/";
  router.push(redirect);
}

async function handleLogin() {
  loginErrors.value = validateLoginForm(loginForm.value);
  if (hasFieldErrors(loginErrors.value)) return;

  submitting.value = true;
  setMessage("", false);
  try {
    const response = await authService.login(loginForm.value);
    authStore.setAuth(response.data.token, response.data);
    await cartStore.syncGuestCartToServer();
    await cartStore.loadCart();
    redirectAfterAuth();
  } catch (caughtError) {
    setMessage(
      extractErrorMessage(caughtError, "Đăng nhập thất bại. Vui lòng thử lại."),
      false,
    );
  } finally {
    submitting.value = false;
  }
}

async function handleRegister() {
  registerErrors.value = validateRegisterForm(registerForm.value);
  if (hasFieldErrors(registerErrors.value)) return;

  submitting.value = true;
  setMessage("", false);
  try {
    const response = await authService.register(registerForm.value);
    if (response.data?.token) {
      authStore.setAuth(response.data.token, response.data);
      await cartStore.syncGuestCartToServer();
      await cartStore.loadCart();
      redirectAfterAuth();
      return;
    }

    setMessage("Đăng ký thành công. Vui lòng đăng nhập để tiếp tục.", true);
    mode.value = "login";
    loginForm.value.tenDangNhap = registerForm.value.tenDangNhap;
    loginForm.value.matKhau = "";
  } catch (caughtError) {
    setMessage(
      extractErrorMessage(caughtError, "Đăng ký thất bại. Vui lòng thử lại."),
      false,
    );
  } finally {
    submitting.value = false;
  }
}

watch(mode, () => {
  message.value = "";
  loginErrors.value = {};
  registerErrors.value = {};
});
</script>
