import api from "./api";
import type { APIResponse, CategoryFormValues, CategoryItem } from "../types";

export const danhMucService = {
  getAll: async (): Promise<APIResponse<CategoryItem[]>> => api.get("/DanhMuc"),
  getById: async (madm: number): Promise<APIResponse<CategoryItem>> =>
    api.get(`/DanhMuc/by-madm/${madm}`),
  searchByName: async (
    tendm: string,
  ): Promise<APIResponse<CategoryItem[]>> => api.get(`/DanhMuc/by-tendm/${tendm}`),
  create: async (data: CategoryFormValues): Promise<APIResponse<CategoryItem>> =>
    api.post("/DanhMuc", data),
  update: (
    madm: number,
    data: CategoryFormValues,
  ): Promise<APIResponse<CategoryItem>> => api.put(`/DanhMuc?madm=${madm}`, data),
  delete: async (madm: number): Promise<APIResponse<string>> =>
    api.delete(`/DanhMuc?madm=${madm}`),
};
