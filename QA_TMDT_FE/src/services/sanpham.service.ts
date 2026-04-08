import api from "./api";

export const sanPhamService = {
  getAll: (page = 1, pageSize = 10) =>
    api.get("/SanPham/GetAllSP", { params: { page, pageSize } }),
  getByCategory: (maDM: number, page = 1, pageSize = 10) =>
    api.get(`/SanPham/Get-by-maDM/${maDM}`, { params: { page, pageSize } }),
  searchByName: (tenSP: string, page = 1, pageSize = 10) =>
    api.get(`/SanPham/Get-by-tenSP/${tenSP}`, { params: { page, pageSize } }),
  getFullInfo: (maSP: string) =>
    api.get(`/SanPham/Get-fullinfo-by-maSP/${maSP}`),
  create: (formData: FormData) =>
    api.post("/SanPham/Create-SP", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    }),
  update: (formData: FormData) =>
    api.put("/SanPham/Update-SP", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    }),
  delete: (maSP: string) => api.delete(`/SanPham/Delete-SP/${maSP}`),
};
