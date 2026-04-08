import api from "./api";

export const taiKhoanService = {
  // Admin: Lấy tất cả tài khoản
  getAllAdmin() {
    return api.get("/TaiKhoan/admin-getall-acc");
  },
};
