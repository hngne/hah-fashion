import api from "./api";
import type {
  APIResponse,
  DangNhapRequest,
  DangNhapResponse,
  RegisterFormValues,
} from "../types";

export const authService = {
  login: async (
    request: DangNhapRequest,
  ): Promise<APIResponse<DangNhapResponse>> => {
    return await api.post("/Authentication/Login", request);
  },

  register: async (
    data: RegisterFormValues,
  ): Promise<APIResponse<DangNhapResponse>> => {
    return await api.post("/Authentication/Register", data);
  },
};
