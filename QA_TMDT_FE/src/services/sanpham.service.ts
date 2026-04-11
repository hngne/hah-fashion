import api from "./api";
import type {
  APIResponse,
  CreateVariantPayload,
  PageResult,
  ProductDetail,
  ProductSummary,
  UpdateVariantPayload,
} from "../types";

export const sanPhamService = {
  getAll: async (
    page = 1,
    pageSize = 10,
  ): Promise<APIResponse<PageResult<ProductSummary>>> =>
    api.get("/SanPham/GetAllSP", { params: { page, pageSize } }),
  getByCategory: async (
    maDM: number,
    page = 1,
    pageSize = 10,
  ): Promise<APIResponse<PageResult<ProductSummary>>> =>
    api.get(`/SanPham/Get-by-maDM/${maDM}`, { params: { page, pageSize } }),
  searchByName: async (
    tenSP: string,
    page = 1,
    pageSize = 10,
  ): Promise<APIResponse<PageResult<ProductSummary>>> =>
    api.get(`/SanPham/Get-by-tenSP/${tenSP}`, { params: { page, pageSize } }),
  getFullInfo: async (maSP: string): Promise<APIResponse<ProductDetail>> =>
    api.get(`/SanPham/Get-fullinfo-by-maSP/${maSP}`),
  create: async (formData: FormData): Promise<APIResponse<ProductDetail>> =>
    api.post("/SanPham/Create-SP", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    }),
  update: async (formData: FormData): Promise<APIResponse<ProductDetail>> =>
    api.put("/SanPham/Update-SP", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    }),
  createVariant: async (
    data: CreateVariantPayload,
  ): Promise<APIResponse<ProductDetail["dsBienThe"][number]>> =>
    api.post("/SanPham/Create-Variant", data),
  updateVariant: async (
    data: UpdateVariantPayload,
  ): Promise<APIResponse<ProductDetail["dsBienThe"][number]>> =>
    api.put("/SanPham/Update-CTSP", data),
  delete: async (maSP: string): Promise<APIResponse<string>> =>
    api.delete(`/SanPham/Delete-SP/${maSP}`),
};
