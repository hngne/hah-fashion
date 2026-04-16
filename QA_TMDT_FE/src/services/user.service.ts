import api from "./api";

export const userService = {
  getAll: (params?: {
    keyword?: string;
    vaiTro?: string;
  }) => api.get("/TaiKhoan/admin-getall-acc", { params }),
  getById: (maTk: string) => api.get(`/TaiKhoan/getInfo-acc/${maTk}`),
};
