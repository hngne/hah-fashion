<template>
  <div class="min-h-screen flex bg-[#F3F4F6]">
    <!-- Mobile Overlay -->
    <transition name="overlay">
      <div
        v-if="mobileOpen"
        class="fixed inset-0 z-40 bg-black/40 lg:hidden"
        @click="mobileOpen = false"
      ></div>
    </transition>

    <!-- Sidebar -->
    <aside
      :class="[
        'fixed inset-y-0 left-0 z-50 flex flex-col transition-all duration-300 bg-[#0F172A]',
        // Mobile: ẩn hoàn toàn, hiện khi mobileOpen
        mobileOpen ? 'translate-x-0 w-[260px]' : '-translate-x-full w-[260px]',
        // Desktop: luôn hiện, co/mở theo sidebarCollapsed
        'lg:translate-x-0',
        sidebarCollapsed ? 'lg:w-[72px]' : 'lg:w-[260px]',
      ]"
    >
      <!-- Logo -->
      <div
        class="h-16 flex items-center justify-between px-5 border-b border-slate-700/50 flex-shrink-0"
      >
        <div class="flex items-center">
          <div
            class="w-9 h-9 rounded-lg bg-blue-600 flex items-center justify-center flex-shrink-0"
          >
            <span class="text-white font-extrabold text-sm">HF</span>
          </div>
          <transition name="fade">
            <span
              v-if="!sidebarCollapsed || mobileOpen"
              class="ml-3 text-white font-bold text-lg tracking-tight whitespace-nowrap"
              >HaHFashion</span
            >
          </transition>
        </div>
        <!-- Mobile close button -->
        <button
          @click="mobileOpen = false"
          class="lg:hidden p-1.5 rounded-lg text-slate-400 hover:text-white hover:bg-slate-700/50 transition-colors"
        >
          <XIcon class="h-5 w-5" stroke-width="1.5" />
        </button>
      </div>

      <!-- Navigation -->
      <nav class="flex-1 py-4 px-3 space-y-1 overflow-y-auto">
        <SidebarItem
          v-for="item in menuItems"
          :key="item.name"
          :icon="item.icon"
          :label="item.label"
          :to="item.to"
          :collapsed="sidebarCollapsed && !mobileOpen"
          :active="isActive(item.to)"
          @click="mobileOpen = false"
        />
      </nav>

      <!-- Collapse Toggle (Desktop only) -->
      <div class="hidden lg:block p-3 border-t border-slate-700/50">
        <button
          @click="sidebarCollapsed = !sidebarCollapsed"
          class="w-full flex items-center justify-center p-2 rounded-lg text-slate-400 hover:text-white hover:bg-slate-700/50 transition-colors"
        >
          <PanelLeftCloseIcon
            v-if="!sidebarCollapsed"
            class="h-5 w-5"
            stroke-width="1.5"
          />
          <PanelLeftOpenIcon v-else class="h-5 w-5" stroke-width="1.5" />
        </button>
      </div>
    </aside>

    <!-- Main Content -->
    <main
      :class="[
        'flex-1 flex flex-col transition-all duration-300',
        // Mobile: không margin-left (sidebar là overlay)
        'ml-0',
        // Desktop: margin-left theo sidebar width
        sidebarCollapsed ? 'lg:ml-[72px]' : 'lg:ml-[260px]',
      ]"
    >
      <!-- Topbar -->
      <header
        class="h-14 sm:h-16 bg-white border-b border-gray-200 flex items-center justify-between px-4 sm:px-6 sticky top-0 z-20"
      >
        <div class="flex items-center space-x-3">
          <!-- Hamburger (Mobile only) -->
          <button
            @click="mobileOpen = true"
            class="lg:hidden p-2 -ml-2 rounded-lg text-gray-500 hover:bg-gray-100 hover:text-gray-700 transition-colors"
          >
            <MenuIcon class="h-5 w-5" stroke-width="1.5" />
          </button>
          <h2 class="text-base sm:text-lg font-bold text-[#1F2937] truncate">
            {{ currentPageTitle }}
          </h2>
        </div>
        <div class="flex items-center space-x-2 sm:space-x-4">
          <!-- Bell -->
          <button
            class="relative p-2 rounded-lg text-gray-500 hover:bg-gray-100 hover:text-gray-700 transition-colors"
          >
            <BellIcon class="h-5 w-5" stroke-width="1.5" />
            <span
              class="absolute top-1.5 right-1.5 w-2 h-2 bg-red-500 rounded-full"
            ></span>
          </button>

          <!-- User -->
          <div
            class="flex items-center space-x-2 sm:space-x-3 pl-2 sm:pl-4 border-l border-gray-200"
          >
            <div
              class="w-8 h-8 rounded-full bg-blue-600 flex items-center justify-center flex-shrink-0"
            >
              <span class="text-white text-xs font-bold">{{
                userInitials
              }}</span>
            </div>
            <div class="hidden md:block">
              <p class="text-sm font-semibold text-gray-800 leading-none">
                {{ authStore.user?.hoTen || "Admin" }}
              </p>
              <p class="text-[11px] text-gray-400 mt-0.5">Quản trị viên</p>
            </div>
            <button
              @click="handleLogout"
              class="p-2 rounded-lg text-gray-400 hover:bg-red-50 hover:text-red-500 transition-colors"
              title="Đăng xuất"
            >
              <LogOutIcon class="h-4 w-4" stroke-width="2" />
            </button>
          </div>
        </div>
      </header>

      <!-- Page Content -->
      <div class="p-3 sm:p-4 lg:p-6 flex-1 overflow-auto">
        <RouterView />
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { RouterView, useRoute, useRouter } from "vue-router";
import { useAuthStore } from "../../stores/auth";
import {
  LayoutDashboard,
  Package,
  ShoppingCart,
  Users,
  FolderTree,
  Ticket,
  Settings,
  Bell as BellIcon,
  LogOut as LogOutIcon,
  PanelLeftClose as PanelLeftCloseIcon,
  PanelLeftOpen as PanelLeftOpenIcon,
  Menu as MenuIcon,
  X as XIcon,
  Layers,
  BadgePercent,
} from "lucide-vue-next";
import SidebarItem from "../../components/admin/SidebarItem.vue";

const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();
const sidebarCollapsed = ref(false);
const mobileOpen = ref(false);

const menuItems = [
  {
    name: "dashboard",
    label: "Bảng điều khiển",
    icon: LayoutDashboard,
    to: "/admin",
  },
  { name: "products", label: "Sản phẩm", icon: Package, to: "/admin/products" },
  {
    name: "categories",
    label: "Danh mục",
    icon: FolderTree,
    to: "/admin/categories",
  },
  {
    name: "variants",
    label: "Biến thể",
    icon: Layers,
    to: "/admin/variant-attributes",
  },
  {
    name: "orders",
    label: "Đơn hàng",
    icon: ShoppingCart,
    to: "/admin/orders",
  },
  { name: "users", label: "Người dùng", icon: Users, to: "/admin/users" },
  {
    name: "vouchers",
    label: "Voucher",
    icon: BadgePercent,
    to: "/admin/vouchers",
  },
  {
    name: "promotions",
    label: "Khuyến mãi",
    icon: Ticket,
    to: "/admin/promotions",
  },
  { name: "settings", label: "Cài đặt", icon: Settings, to: "/admin/settings" },
];

const currentPageTitle = computed(() => {
  const matched = menuItems.find((item) => item.to === route.path);
  return matched?.label || "Bảng điều khiển";
});

const userInitials = computed(() => {
  const name = authStore.user?.hoTen || "A";
  return name
    .split(" ")
    .map((w: string) => w[0])
    .join("")
    .toUpperCase()
    .slice(0, 2);
});

const isActive = (path: string) => {
  if (path === "/admin") return route.path === "/admin";
  return route.path.startsWith(path);
};

const handleLogout = () => {
  authStore.logout();
  router.push("/admin/login");
};
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
.overlay-enter-active,
.overlay-leave-active {
  transition: opacity 0.25s ease;
}
.overlay-enter-from,
.overlay-leave-to {
  opacity: 0;
}
</style>
