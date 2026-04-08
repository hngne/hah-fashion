import api from "./api";

export const gioHangService = {
  getCart: (matk: string) => api.get("/GioHang", { params: { matk } }),

  addToCart: (matk: string, mactsp: string, soluongMua: number) =>
    api.post("/GioHang/Add-to-cart", null, {
      params: { matk, mactsp, soluongMua },
    }),

  decreaseItem: (matk: string, mactsp: string, soluongGiam: number) =>
    api.post("/GioHang/Decrease-cart", null, {
      params: { matk, mactsp, soluongGiam },
    }),

  updateItemQty: (matk: string, mactsp: string, soluongMoi: number) =>
    api.post("/GioHang/Update-value-cart", null, {
      params: { matk, mactsp, soluongMoi },
    }),

  removeItem: (matk: string, mactsp: string) =>
    api.delete("/GioHang/Remove-item-cart", { params: { matk, mactsp } }),

  clearCart: (matk: string) =>
    api.delete("/GioHang/Remove-all-items", { params: { matk } }),
};
