import api from "./api";

export const danhMucService = {
  getAll: () => api.get("/DanhMuc"),
  getById: (madm: number) => api.get(`/DanhMuc/by-madm/${madm}`),
  searchByName: (tendm: string) => api.get(`/DanhMuc/by-tendm/${tendm}`),
  create: (data: {
    tenDanhMuc: string;
    moTa?: string;
    maDanhMucCha?: number | null;
  }) => api.post("/DanhMuc", data),
  update: (
    madm: number,
    data: { tenDanhMuc: string; moTa?: string; maDanhMucCha?: number | null },
  ) => api.put(`/DanhMuc?madm=${madm}`, data),
  delete: (madm: number) => api.delete(`/DanhMuc?madm=${madm}`),
};
