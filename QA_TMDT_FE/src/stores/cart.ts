import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { useAuthStore } from "./auth";
import { gioHangService } from "../services/giohang.service";

// Guest cart item stored in localStorage
export interface GuestCartItem {
  maCTSP: string;
  maSP: string;
  tenSP: string;
  anh: string;
  tenMau: string;
  tenSize: string;
  gia: number;
  soLuong: number;
}

// Server cart item from API
export interface ServerCartItem {
  maGioHang: string;
  maSP: string;
  tenSP: string;
  anh: string;
  tenSize: string;
  tenMau: string;
  soLuongTon: number;
  maChiTietSp?: string;
  maChiTietSP?: string;
  soLuong: number;
  giaGoc: number;
  giaDatHang: number;
  thanhTien: number;
}

const GUEST_CART_KEY = "guest_cart";

export const useCartStore = defineStore("cart", () => {
  const authStore = useAuthStore();

  // State
  const serverItems = ref<ServerCartItem[]>([]);
  const guestItems = ref<GuestCartItem[]>([]);
  const loading = ref(false);

  // Init guest cart from localStorage
  const initGuestCart = () => {
    try {
      const saved = localStorage.getItem(GUEST_CART_KEY);
      if (saved) guestItems.value = JSON.parse(saved);
    } catch {
      guestItems.value = [];
    }
  };

  const saveGuestCart = () => {
    localStorage.setItem(GUEST_CART_KEY, JSON.stringify(guestItems.value));
  };

  const clearGuestCart = () => {
    guestItems.value = [];
    localStorage.removeItem(GUEST_CART_KEY);
  };

  // Computed
  const isLoggedIn = computed(() => authStore.isAuthenticated());
  const maTK = computed(() => authStore.user?.maTaiKhoan || "");

  const items = computed(() => {
    if (isLoggedIn.value) {
      return serverItems.value.map((item) => ({
        maCTSP: item.maChiTietSp || item.maChiTietSP || "",
        maSP: item.maSP,
        tenSP: item.tenSP,
        anh: item.anh,
        tenMau: item.tenMau,
        tenSize: item.tenSize,
        gia: item.giaDatHang,
        giaGoc: item.giaGoc,
        soLuong: item.soLuong,
        soLuongTon: item.soLuongTon,
        thanhTien: item.thanhTien,
      }));
    }
    return guestItems.value.map((item) => ({
      ...item,
      giaGoc: item.gia,
      soLuongTon: 999,
      thanhTien: item.gia * item.soLuong,
    }));
  });

  const totalItems = computed(() =>
    items.value.reduce((sum, item) => sum + item.soLuong, 0),
  );

  const totalPrice = computed(() =>
    items.value.reduce((sum, item) => sum + item.thanhTien, 0),
  );

  // ========== ACTIONS ==========

  // Load cart (from server if logged in, from localStorage if guest)
  const loadCart = async () => {
    if (isLoggedIn.value && maTK.value) {
      loading.value = true;
      try {
        const res: any = await gioHangService.getCart(maTK.value);
        if (res.success && res.data) {
          serverItems.value = res.data.sanPhams || [];
        }
      } catch (e) {
        console.warn("Failed to load cart", e);
      } finally {
        loading.value = false;
      }
    } else {
      initGuestCart();
    }
  };

  // Add to cart
  const addToCart = async (item: GuestCartItem) => {
    if (isLoggedIn.value && maTK.value) {
      // Server mode
      loading.value = true;
      try {
        await gioHangService.addToCart(maTK.value, item.maCTSP, item.soLuong);
        await loadCart();
      } catch (e) {
        console.warn("Failed to add to cart", e);
        throw e;
      } finally {
        loading.value = false;
      }
    } else {
      // Guest mode
      const existing = guestItems.value.find((i) => i.maCTSP === item.maCTSP);
      if (existing) {
        existing.soLuong += item.soLuong;
      } else {
        guestItems.value.push({ ...item });
      }
      saveGuestCart();
    }
  };

  // Update quantity
  const updateQuantity = async (maCTSP: string, qty: number) => {
    if (qty < 1) return;
    if (isLoggedIn.value && maTK.value) {
      loading.value = true;
      try {
        await gioHangService.updateItemQty(maTK.value, maCTSP, qty);
        await loadCart();
      } catch (e) {
        console.warn("Failed to update quantity", e);
      } finally {
        loading.value = false;
      }
    } else {
      const item = guestItems.value.find((i) => i.maCTSP === maCTSP);
      if (item) {
        item.soLuong = qty;
        saveGuestCart();
      }
    }
  };

  // Remove item
  const removeItem = async (maCTSP: string) => {
    if (isLoggedIn.value && maTK.value) {
      loading.value = true;
      try {
        await gioHangService.removeItem(maTK.value, maCTSP);
        await loadCart();
      } catch (e) {
        console.warn("Failed to remove item", e);
      } finally {
        loading.value = false;
      }
    } else {
      guestItems.value = guestItems.value.filter((i) => i.maCTSP !== maCTSP);
      saveGuestCart();
    }
  };

  // Clear cart
  const clearAll = async () => {
    if (isLoggedIn.value && maTK.value) {
      loading.value = true;
      try {
        await gioHangService.clearCart(maTK.value);
        serverItems.value = [];
      } catch (e) {
        console.warn("Failed to clear cart", e);
      } finally {
        loading.value = false;
      }
    } else {
      clearGuestCart();
    }
  };

  // Sync guest cart to server after login
  const syncGuestCartToServer = async () => {
    if (!isLoggedIn.value || !maTK.value) return;
    const pending = [...guestItems.value];
    if (pending.length === 0) return;

    loading.value = true;
    try {
      // Add each guest item to server cart
      for (const item of pending) {
        try {
          await gioHangService.addToCart(maTK.value, item.maCTSP, item.soLuong);
        } catch (e) {
          console.warn(`Failed to sync item ${item.tenSP}`, e);
        }
      }
      // Clear guest cart
      clearGuestCart();
      // Reload server cart
      await loadCart();
    } finally {
      loading.value = false;
    }
  };

  // Initialize
  initGuestCart();

  return {
    items,
    totalItems,
    totalPrice,
    loading,
    loadCart,
    addToCart,
    updateQuantity,
    removeItem,
    clearAll,
    syncGuestCartToServer,
    clearGuestCart,
  };
});
