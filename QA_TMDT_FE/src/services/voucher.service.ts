import api from "./api";

export const voucherService = {
  getAll: () => api.get("/Voucher"),
  getByCode: (maVoucher: string) =>
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
  getAvailableForUser: (maTK: string) =>
    api.get(`/Voucher/avaiable-user-canuse/${maTK}`),
};
