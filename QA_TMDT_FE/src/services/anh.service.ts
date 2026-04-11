import api from "./api";
import type { APIResponse, ImageItem } from "../types";

export const anhService = {
  getByProduct: async (maSp: string): Promise<APIResponse<ImageItem[]>> =>
    api.get(`/Anh/by-masp/${maSp}`),

  addImages: async (
    maSp: string,
    files: File[],
  ): Promise<APIResponse<ImageItem[]>> => {
    const formData = new FormData();
    formData.append("maSp", maSp);
    for (const file of files) {
      formData.append("AnhSPs", file);
    }

    return api.post("/Anh", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  },

  deleteImage: async (maAnh: number): Promise<APIResponse<string>> =>
    api.delete("/Anh", { params: { maAnh } }),
};
