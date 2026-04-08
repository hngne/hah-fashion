import api from "./api";

export const userService = {
  getAll: () => api.get("/TaiKhoan/admin-getall-acc"),
  getById: (maTk: string) => api.get(`/TaiKhoan/getInfo-acc/${maTk}`),
  updateRole: (maTk: string, newRole: string) =>
    api.put(`/TaiKhoan/admin/role/${maTk}?newRole=${newRole}`),
  deleteOrLock: (maTk: string) => api.delete(`/TaiKhoan/admin/${maTk}`),
};
