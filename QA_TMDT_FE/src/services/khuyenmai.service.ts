import api from "./api";

export const khuyenMaiService = {
  getAll: () => api.get("/KhuyenMai"),
  getByCode: (makm: string) => api.get(`/KhuyenMai/by-makm/${makm}`),
  create: (data: {
    maKhuyenMai: string;
    tenKhuyenMai: string;
    ngayBatDau: string;
    ngayKetThuc: string;
    moTa?: string;
  }) => api.post("/KhuyenMai", data),
  update: (data: {
    maKhuyenMai: string;
    tenKhuyenMai: string;
    ngayBatDau: string;
    ngayKetThuc: string;
    moTa?: string;
  }) => api.put("/KhuyenMai", data),
  delete: (makm: string) => api.delete(`/KhuyenMai/${makm}`),

  // Chi tiết khuyến mãi (áp dụng SP)
  getChiTietByMaKM: (makm: string) =>
    api.get(`/KhuyenMai/chitiet/by-makm/${makm}`),
  addChiTiet: (data: {
    maKhuyenMai: string;
    maSp: string;
    phanTramGiam: number;
  }) => api.post("/KhuyenMai/chitiet", data),
  updateChiTiet: (data: {
    maKhuyenMai: string;
    maSp: string;
    phanTramGiam: number;
  }) => api.put("/KhuyenMai/chitiet", data),
  deleteChiTiet: (makm: string, masp: string) =>
    api.delete(`/KhuyenMai/chitiet/${makm}/${masp}`),
};
