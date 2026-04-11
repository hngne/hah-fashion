<template>
  <div class="min-h-screen flex flex-col bg-[#FAFAFA]">
    <!-- ===== HEADER ===== -->
    <header class="sticky top-0 z-50 bg-white border-b border-gray-200 shadow-sm">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex items-center justify-between h-16">
          <!-- Logo -->
          <RouterLink to="/" class="flex items-center space-x-2 flex-shrink-0">
            <img v-if="logoUrl" :src="logoUrl" alt="HaHFashion" class="h-12 w-auto max-w-[200px] object-contain" />
            <span v-else class="text-xl font-extrabold tracking-tight text-[#111827]">
              <span class="text-blue-600">HaH</span>Fashion
            </span>
          </RouterLink>

          <!-- Desktop Nav with Mega Menu -->
          <nav class="hidden lg:flex items-center space-x-1">
            <RouterLink to="/" class="px-3 py-2 text-sm font-semibold text-[#111827] hover:text-blue-600 transition rounded-md hover:bg-gray-50">
              Trang chủ
            </RouterLink>

            <RouterLink to="/search" class="px-3 py-2 text-sm font-semibold text-gray-600 hover:text-blue-600 transition rounded-md hover:bg-gray-50">
              Sản phẩm mới
            </RouterLink>

            <!-- Category items with mega dropdown -->
            <div v-for="root in rootCategories" :key="root.maDanhMuc"
              class="relative" @mouseenter="openMega = root.maDanhMuc" @mouseleave="openMega = null">
              <button class="flex items-center px-3 py-2 text-sm font-semibold text-gray-600 hover:text-blue-600 transition rounded-md hover:bg-gray-50">
                {{ root.tenDanhMuc }}
                <ChevronDownIcon v-if="getChildren(root.maDanhMuc).length" class="w-3.5 h-3.5 ml-1 opacity-50" />
              </button>
              <!-- Dropdown -->
              <transition name="megafade">
                <div v-if="openMega === root.maDanhMuc && getChildren(root.maDanhMuc).length"
                  class="absolute left-0 top-full mt-0 bg-white text-[#111827] rounded-lg shadow-2xl border border-gray-100 min-w-[220px] py-2 z-50">
                  <RouterLink v-for="child in getChildren(root.maDanhMuc)" :key="child.maDanhMuc"
                    :to="{ name: 'category-products', params: { maDM: child.maDanhMuc } }"
                    class="block px-4 py-2.5 text-sm font-medium text-gray-700 hover:bg-blue-50 hover:text-blue-600 transition">
                    {{ child.tenDanhMuc }}
                  </RouterLink>
                  <div class="border-t border-gray-100 mt-1 pt-1">
                    <RouterLink :to="{ name: 'category-products', params: { maDM: root.maDanhMuc } }"
                      class="block px-4 py-2.5 text-sm font-semibold text-blue-600 hover:bg-blue-50 transition">
                      Xem tất cả {{ root.tenDanhMuc }} →
                    </RouterLink>
                  </div>
                </div>
              </transition>
            </div>

            <RouterLink to="/sale" class="px-3 py-2 text-sm font-bold text-red-500 hover:text-red-600 transition rounded-md hover:bg-red-50">
              Sale
            </RouterLink>
          </nav>

          <!-- Right Actions -->
          <div class="flex items-center space-x-3">
            <div class="hidden md:flex items-center bg-gray-100 rounded-full px-3 py-1.5">
              <SearchIcon class="h-4 w-4 text-gray-400" stroke-width="2" />
              <input v-model="searchQuery" @keyup.enter="handleSearch" type="text" placeholder="Tìm kiếm sản phẩm..."
                class="bg-transparent border-none text-sm text-[#111827] placeholder-gray-400 ml-2 w-40 focus:outline-none focus:w-56 transition-all" />
            </div>
            <RouterLink to="/cart" class="relative p-2 text-gray-500 hover:text-[#111827] transition">
              <ShoppingBagIcon class="h-5 w-5" stroke-width="2" />
              <span v-if="cartStore.totalItems > 0"
                class="absolute -top-0.5 -right-0.5 flex items-center justify-center bg-red-500 text-white text-[10px] font-bold min-w-[16px] h-4 px-0.5 rounded-full">
                {{ cartStore.totalItems > 99 ? '99+' : cartStore.totalItems }}
              </span>
            </RouterLink>
            <!-- User Menu -->
            <div class="relative" @mouseleave="userMenuOpen = false">
              <button @mouseenter="userMenuOpen = true" @click="userMenuOpen = !userMenuOpen"
                class="flex items-center space-x-1.5 p-2 text-gray-500 hover:text-[#111827] transition">
                <UserIcon class="h-5 w-5" stroke-width="2" />
                <span v-if="authStore.isAuthenticated()" class="hidden sm:inline text-xs font-semibold text-[#111827] max-w-[80px] truncate">
                  {{ authStore.user?.hoTen || authStore.user?.tenDangNhap }}
                </span>
              </button>
              <transition name="megafade">
                <div v-if="userMenuOpen"
                  class="absolute right-0 top-full mt-0 bg-white rounded-lg shadow-2xl border border-gray-100 min-w-[180px] py-2 z-50">
                  <template v-if="authStore.isAuthenticated()">
                    <div class="px-4 py-2 border-b border-gray-100">
                      <p class="text-sm font-semibold text-[#111827]">{{ authStore.user?.hoTen || authStore.user?.tenDangNhap }}</p>
                      <p class="text-xs text-gray-400">{{ authStore.user?.email }}</p>
                    </div>
                    <RouterLink to="/orders" @click="userMenuOpen = false"
                      class="flex items-center w-full px-4 py-2.5 text-sm text-gray-700 hover:bg-blue-50 hover:text-blue-600 transition">
                      <PackageIcon class="w-4 h-4 mr-2" /> Đơn hàng của tôi
                    </RouterLink>
                    <button @click="handleLogout"
                      class="flex items-center w-full px-4 py-2.5 text-sm text-red-500 hover:bg-red-50 transition">
                      <LogOutIcon class="w-4 h-4 mr-2" /> Đăng xuất
                    </button>
                  </template>
                  <template v-else>
                    <RouterLink to="/login" @click="userMenuOpen = false"
                      class="block px-4 py-2.5 text-sm font-medium text-gray-700 hover:bg-blue-50 hover:text-blue-600 transition">
                      Đăng nhập
                    </RouterLink>
                    <RouterLink :to="{ path: '/login', query: { tab: 'register' } }" @click="userMenuOpen = false"
                      class="block px-4 py-2.5 text-sm font-medium text-gray-700 hover:bg-blue-50 hover:text-blue-600 transition">
                      Đăng ký
                    </RouterLink>
                  </template>
                </div>
              </transition>
            </div>
            <button @click="mobileOpen = !mobileOpen" class="lg:hidden p-2 text-gray-500 hover:text-[#111827]">
              <MenuIcon v-if="!mobileOpen" class="h-5 w-5" />
              <XIcon v-else class="h-5 w-5" />
            </button>
          </div>
        </div>
      </div>

      <!-- Mobile Nav -->
      <transition name="slide">
        <div v-if="mobileOpen" class="lg:hidden bg-white border-t border-gray-100 max-h-[70vh] overflow-y-auto">
          <div class="px-4 py-3 space-y-1">
            <RouterLink to="/" @click="mobileOpen = false"
              class="block px-3 py-2 text-sm font-semibold text-[#111827] hover:bg-gray-50 rounded-lg">
              Trang chủ
            </RouterLink>
            <RouterLink to="/search" @click="mobileOpen = false"
              class="block px-3 py-2 text-sm font-semibold text-gray-600 hover:bg-gray-50 rounded-lg">
              Sản phẩm mới
            </RouterLink>
            <!-- Mobile categories with accordion -->
            <div v-for="root in rootCategories" :key="root.maDanhMuc">
              <button @click="mobileExpanded === root.maDanhMuc ? mobileExpanded = null : mobileExpanded = root.maDanhMuc"
                class="flex items-center justify-between w-full px-3 py-2 text-sm font-semibold text-gray-600 hover:bg-gray-50 rounded-lg">
                {{ root.tenDanhMuc }}
                <ChevronDownIcon v-if="getChildren(root.maDanhMuc).length" class="w-4 h-4 transition-transform"
                  :class="mobileExpanded === root.maDanhMuc ? 'rotate-180' : ''" />
              </button>
              <div v-if="mobileExpanded === root.maDanhMuc && getChildren(root.maDanhMuc).length" class="pl-6 space-y-0.5">
                <RouterLink v-for="child in getChildren(root.maDanhMuc)" :key="child.maDanhMuc"
                  :to="{ name: 'category-products', params: { maDM: child.maDanhMuc } }" @click="mobileOpen = false"
                  class="block px-3 py-1.5 text-sm text-gray-500 hover:text-blue-600 hover:bg-gray-50 rounded-lg">
                  {{ child.tenDanhMuc }}
                </RouterLink>
              </div>
            </div>
            <RouterLink to="/sale" @click="mobileOpen = false"
              class="block px-3 py-2 text-sm font-bold text-red-500 hover:bg-red-50 rounded-lg">
              KHUYEN MAI
            </RouterLink>
          </div>
        </div>
      </transition>
    </header>

    <!-- ===== MAIN ===== -->
    <main class="flex-grow">
      <RouterView />
    </main>

    <!-- ===== FOOTER ===== -->
    <footer class="bg-[#111827] text-white">
      <div class="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
          <div>
            <span class="text-lg font-extrabold"><span class="text-blue-400">HaH</span>Fashion</span>
            <p class="text-sm text-white/50 mt-3 leading-relaxed">Thời trang hiện đại cho mọi phong cách. Chất lượng cao, giá hợp lý.</p>
            <div class="flex space-x-3 mt-4">
              <span class="flex h-8 w-8 items-center justify-center rounded-full bg-white/10 text-white/60"><FacebookIcon class="w-4 h-4" /></span>
              <span class="flex h-8 w-8 items-center justify-center rounded-full bg-white/10 text-white/60"><InstagramIcon class="w-4 h-4" /></span>
              <a href="mailto:support@hahfashion.vn" class="flex h-8 w-8 items-center justify-center rounded-full bg-white/10 text-white/60 transition hover:bg-blue-400 hover:text-white"><MailIcon class="w-4 h-4" /></a>
            </div>
          </div>
          <div>
            <h4 class="text-sm font-bold uppercase tracking-wider text-white/80 mb-4">Hỗ trợ</h4>
            <ul class="space-y-2 text-sm text-white/50">
              <li class="transition hover:text-white">Hướng dẫn mua hàng</li>
              <li class="transition hover:text-white">Chính sách đổi trả</li>
              <li class="transition hover:text-white">Chính sách bảo hành</li>
              <li class="transition hover:text-white">Câu hỏi thường gặp</li>
            </ul>
          </div>
          <div>
            <h4 class="text-sm font-bold uppercase tracking-wider text-white/80 mb-4">Chính sách</h4>
            <ul class="space-y-2 text-sm text-white/50">
              <li class="transition hover:text-white">Điều khoản sử dụng</li>
              <li class="transition hover:text-white">Chính sách bảo mật</li>
              <li class="transition hover:text-white">Chính sách vận chuyển</li>
              <li class="transition hover:text-white">Phương thức thanh toán</li>
            </ul>
          </div>
          <div>
            <h4 class="text-sm font-bold uppercase tracking-wider text-white/80 mb-4">Liên hệ</h4>
            <ul class="space-y-2 text-sm text-white/50">
              <li class="flex items-center space-x-2"><MapPinIcon class="w-4 h-4 flex-shrink-0" /><span>123 Đường Thời Trang, TP.HCM</span></li>
              <li class="flex items-center space-x-2"><PhoneIcon class="w-4 h-4 flex-shrink-0" /><span>1900 1234 56</span></li>
              <li class="flex items-center space-x-2"><MailIcon class="w-4 h-4 flex-shrink-0" /><span>support@hahfashion.vn</span></li>
            </ul>
          </div>
        </div>
        <div class="border-t border-white/10 mt-8 pt-6 text-center text-xs text-white/40">© 2026 HaHFashion. All rights reserved.</div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { RouterView, RouterLink, useRouter } from "vue-router";
import {
  Search as SearchIcon, ShoppingBag as ShoppingBagIcon, User as UserIcon,
  Menu as MenuIcon, X as XIcon, ChevronDown as ChevronDownIcon,
  Facebook as FacebookIcon, Instagram as InstagramIcon,
  Mail as MailIcon, MapPin as MapPinIcon, Phone as PhoneIcon,
  LogOut as LogOutIcon, Package as PackageIcon,
} from "lucide-vue-next";
import { danhMucService } from "../../services/danhmuc.service";
import { useAuthStore } from "../../stores/auth";
import { useCartStore } from "../../stores/cart";
import logoSrc from "../../assets/client/logo.png";
import type { CategoryItem } from "../../types";

const router = useRouter();
const authStore = useAuthStore();
const cartStore = useCartStore();
const userMenuOpen = ref(false);

const searchQuery = ref("");
const handleSearch = () => {
  const q = searchQuery.value.trim();
  if (q) {
    router.push({ name: "product-search", query: { q } });
    searchQuery.value = "";
  }
};

const handleLogout = () => {
  authStore.logout();
  cartStore.clearGuestCart();
  userMenuOpen.value = false;
  router.push("/");
};

const mobileOpen = ref(false);
const mobileExpanded = ref<number | null>(null);
const openMega = ref<number | null>(null);
const allCategories = ref<CategoryItem[]>([]);

// Logo from assets/client/logo.png
const logoUrl = ref(logoSrc);

// Root categories (no parent)
const rootCategories = computed(() =>
  allCategories.value.filter((category) => !category.maDanhMucCha)
);

// Get children of a category
const getChildren = (parentId: number) =>
  allCategories.value.filter((category) => category.maDanhMucCha === parentId);

onMounted(async () => {
  try {
    const response = await danhMucService.getAll();
    if (response.success) {
      allCategories.value = response.data || [];
    }
  } catch {
    allCategories.value = [];
  }
});
</script>

<style scoped>
.slide-enter-active { transition: all 0.3s ease-out; }
.slide-leave-active { transition: all 0.2s ease-in; }
.slide-enter-from { opacity: 0; transform: translateY(-8px); }
.slide-leave-to { opacity: 0; transform: translateY(-8px); }
.megafade-enter-active { transition: all 0.15s ease-out; }
.megafade-leave-active { transition: all 0.1s ease-in; }
.megafade-enter-from { opacity: 0; transform: translateY(-4px); }
.megafade-leave-to { opacity: 0; transform: translateY(-4px); }
</style>
