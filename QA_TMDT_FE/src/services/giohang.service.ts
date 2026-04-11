import api from "./api";
import type { APIResponse, CartResponse } from "../types";

export const gioHangService = {
  getCart: async (matk: string): Promise<APIResponse<CartResponse>> =>
    api.get("/GioHang", { params: { matk } }),

  addToCart: async (
    matk: string,
    mactsp: string,
    soluongMua: number,
  ): Promise<APIResponse<string>> =>
    api.post("/GioHang/Add-to-cart", null, {
      params: { matk, mactsp, soluongMua },
    }),

  decreaseItem: async (
    matk: string,
    mactsp: string,
    soluongGiam: number,
  ): Promise<APIResponse<string>> =>
    api.post("/GioHang/Decrease-cart", null, {
      params: { matk, mactsp, soluongGiam },
    }),

  updateItemQty: async (
    matk: string,
    mactsp: string,
    soluongMoi: number,
  ): Promise<APIResponse<string>> =>
    api.post("/GioHang/Update-value-cart", null, {
      params: { matk, mactsp, soluongMoi },
    }),

  removeItem: async (
    matk: string,
    mactsp: string,
  ): Promise<APIResponse<string>> =>
    api.delete("/GioHang/Remove-item-cart", { params: { matk, mactsp } }),

  clearCart: async (matk: string): Promise<APIResponse<string>> =>
    api.delete("/GioHang/Remove-all-items", { params: { matk } }),
};
