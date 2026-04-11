import api from "./api";
import type { APIResponse, Voucher } from "../types";

export const voucherService = {
  getAll: async (): Promise<APIResponse<Voucher[]>> => api.get("/Voucher"),
  getByCode: async (maVoucher: string): Promise<APIResponse<Voucher>> =>
    api.get(`/Voucher/by-mavoucher/${maVoucher}`),
  create: (data: {
    maVoucher: string;
    tenVoucher?: string;
    giamGia: number;
    ngayBatDau: string;
    ngayKetThuc: string;
    dieuKienApDung?: number;
    moTa?: string;
  }) => api.post("/Voucher", data),
  update: (data: {
    maVoucher: string;
    tenVoucher?: string;
    giamGia: number;
    ngayBatDau: string;
    ngayKetThuc: string;
    dieuKienApDung?: number;
    moTa?: string;
  }) => api.put("/Voucher", data),
  delete: (maVoucher: string) => api.delete(`/Voucher?maVoucher=${maVoucher}`),

  // Client: get vouchers available for user
  getAvailableForUser: async (
    maTK: string,
  ): Promise<APIResponse<Voucher[]>> =>
    api.get(`/Voucher/avaiable-user-canuse/${maTK}`),
};
