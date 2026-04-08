export interface APIResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface DangNhapRequest {
  tenDangNhap: string;
  matKhau: string;
}

export interface DangNhapResponse {
  maTaiKhoan: string;
  tenDangNhap: string;
  email: string;
  hoTen: string;
  soDienThoai: string;
  vaiTro: string;
  diaChi: string;
  token: string;
}
