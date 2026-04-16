import api from "./api";
import type { APIResponse, CheckoutFormValues, Order } from "../types";

export const donHangService = {
  // Admin: get all orders (used by both OrderManager and Dashboard)
  getAll: async (params?: {
    maDonHang?: string;
    trangThai?: string;
  }): Promise<APIResponse<Order[]>> =>
    api.get("/DonHang/Admin-getAll", { params }),
  getAllAdmin: async (params?: {
    maDonHang?: string;
    trangThai?: string;
  }): Promise<APIResponse<Order[]>> =>
    api.get("/DonHang/Admin-getAll", { params }),
  // Get order by ID (detail)
  getById: async (maDH: string): Promise<APIResponse<Order>> =>
    api.get(`/DonHang/get-dh-by/${maDH}`),
  // Update order status (admin)
  updateStatus: async (
    maDH: string,
    trangThaiMoi: string,
  ): Promise<APIResponse<Order>> =>
    api.put("/DonHang/trangthai", null, {
      params: { maDonHang: maDH, trangThaiMoi },
    }),
  // Delete order (admin)
  delete: async (maDH: string): Promise<APIResponse<string>> =>
    api.delete(`/DonHang?maDH=${maDH}`),

  // ===== CLIENT =====
  // Create order from cart
  createOrder: async (
    data: CheckoutFormValues,
  ): Promise<APIResponse<Order>> => api.post("/DonHang", data),
  // Get user's orders
  getMyOrders: async (): Promise<APIResponse<Order[]>> =>
    api.get("/DonHang/get-by/ignored"),
  // Cancel order
  cancelOrder: async (
    maDH: string,
    lyDoHuy: string,
  ): Promise<APIResponse<Order>> => {
    const rawUser = localStorage.getItem("user");
    const user = rawUser ? (JSON.parse(rawUser) as { maTaiKhoan?: string }) : null;

    return api.put(`/DonHang/huydon/${maDH}`, {
      maTaiKhoan: user?.maTaiKhoan || "",
      lyDoHuy,
    });
  },
};
