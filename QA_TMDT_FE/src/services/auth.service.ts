import api from "./api";
import type { DangNhapRequest, DangNhapResponse, APIResponse } from "../types";

export const authService = {
  login: async (
    request: DangNhapRequest,
  ): Promise<APIResponse<DangNhapResponse>> => {
    return await api.post("/Authentication/Login", request);
  },

  register: async (data: {
    tenDangNhap: string;
    matKhau: string;
    email?: string;
    hoTen?: string;
    soDienThoai?: string;
    diaChi?: string;
  }): Promise<APIResponse<DangNhapResponse>> => {
    return await api.post("/Authentication/Register", data);
  },
};
