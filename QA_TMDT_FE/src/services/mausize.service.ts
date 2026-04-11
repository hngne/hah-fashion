import api from "./api";
import type { APIResponse, ColorItem, SizeItem } from "../types";

export const mauSizeService = {
  // ===== MÀU SẮC =====
  getAllMau: async (): Promise<APIResponse<ColorItem[]>> => api.get("/MauSize/mau"),
  createMau: async (data: { tenMau: string; maHex?: string }) =>
    api.post("/MauSize/mau", data),
  updateMau: async (id: number, data: { tenMau: string; maHex?: string }) =>
    api.put(`/MauSize/mau/${id}`, data),
  deleteMau: async (id: number) => api.delete(`/MauSize/mau/${id}`),

  // ===== KÍCH THƯỚC =====
  getAllSize: async (): Promise<APIResponse<SizeItem[]>> =>
    api.get("/MauSize/size"),
  createSize: async (data: { tenSize: string }) => api.post("/MauSize/size", data),
  updateSize: async (id: number, data: { tenSize: string }) =>
    api.put(`/MauSize/size/${id}`, data),
  deleteSize: async (id: number) => api.delete(`/MauSize/size/${id}`),
};
