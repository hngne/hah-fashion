import api from "./api";

export const mauSizeService = {
  // ===== MÀU SẮC =====
  getAllMau: () => api.get("/MauSize/mau"),
  createMau: (data: { tenMau: string; maHex?: string }) =>
    api.post("/MauSize/mau", data),
  updateMau: (id: number, data: { tenMau: string; maHex?: string }) =>
    api.put(`/MauSize/mau/${id}`, data),
  deleteMau: (id: number) => api.delete(`/MauSize/mau/${id}`),

  // ===== KÍCH THƯỚC =====
  getAllSize: () => api.get("/MauSize/size"),
  createSize: (data: { tenSize: string }) => api.post("/MauSize/size", data),
  updateSize: (id: number, data: { tenSize: string }) =>
    api.put(`/MauSize/size/${id}`, data),
  deleteSize: (id: number) => api.delete(`/MauSize/size/${id}`),
};
