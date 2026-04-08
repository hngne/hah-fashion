import api from "./api";

export const donHangService = {
  // Admin: get all orders (used by both OrderManager and Dashboard)
  getAll: () => api.get("/DonHang/Admin-getAll"),
  getAllAdmin: () => api.get("/DonHang/Admin-getAll"),
  // Get order by ID (detail)
  getById: (maDH: string) => api.get(`/DonHang/get-dh-by/${maDH}`),
  // Update order status (admin)
  updateStatus: (maDH: string, trangThaiMoi: string) =>
    api.put(`/DonHang/trangthai/${maDH}`, { trangThaiMoi }),
  // Delete order (admin)
  delete: (maDH: string) => api.delete(`/DonHang?maDH=${maDH}`),

  // ===== CLIENT =====
  // Create order from cart
  createOrder: (data: {
    maPTTT: number;
    maPTVC: number;
    maVoucher?: string;
    tenNguoiNhan?: string;
    soDienThoai: string;
    diaChiGiaoHang: string;
  }) => api.post("/DonHang", data),
  // Get user's orders
  getMyOrders: (matk: string) => api.get(`/DonHang/get-by/${matk}`),
  // Cancel order
  cancelOrder: (maDH: string, lyDoHuy: string) =>
    api.put(`/DonHang/huydon/${maDH}`, { lyDoHuy }),
};
