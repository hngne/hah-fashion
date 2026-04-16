export interface APIResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface PageResult<T> {
  item: T[];
  totalItem: number;
  page: number;
  pageSize: number;
  totalPage?: number;
}

export interface FieldErrorMap {
  [key: string]: string;
}

export interface DangNhapRequest {
  tenDangNhap: string;
  matKhau: string;
}

export interface DangNhapResponse {
  maTaiKhoan: string;
  tenDangNhap: string;
  email?: string | null;
  hoTen?: string | null;
  soDienThoai?: string | null;
  vaiTro?: string | null;
  diaChi?: string | null;
  trangThai?: boolean;
  token: string;
}

export interface UserAccount {
  maTaiKhoan: string;
  tenDangNhap: string;
  email?: string | null;
  hoTen?: string | null;
  soDienThoai?: string | null;
  vaiTro?: string | null;
  diaChi?: string | null;
  trangThai: boolean;
}

export interface LoginFormValues extends DangNhapRequest {}

export interface RegisterFormValues {
  tenDangNhap: string;
  matKhau: string;
  email?: string;
  hoTen?: string;
  soDienThoai?: string;
  diaChi?: string;
}

export interface CategoryItem {
  maDanhMuc: number;
  tenDanhMuc: string;
  moTa?: string | null;
  maDanhMucCha?: number | null;
  anhDaiDien?: string | null;
}

export interface CategoryFormValues {
  tenDanhMuc: string;
  moTa?: string;
  maDanhMucCha: number | null;
}

export interface SizeItem {
  maSize: number;
  tenSize: string;
}

export interface ColorItem {
  maMau: number;
  tenMau: string;
  maHex?: string | null;
}

export interface VariantItem {
  maChiTietSP: string;
  maKichThuoc: number;
  maMauSac: number;
  tenSize: string;
  tenMau: string;
  soLuongTon: number | null;
  giaBan: number | null;
}

export interface ImageItem {
  maAnh: number;
  maSP: string;
  duongDanAnh: string;
}

export interface ProductSummary {
  maSp: string;
  tenSp: string;
  maDanhMuc?: number | null;
  tenDanhMuc: string;
  tenTimKiem?: string | null;
  giaGoc: number;
  giaKm: number;
  moTa?: string | null;
  chatLieu?: string | null;
  anhDaiDien?: string | null;
  soBienThe?: number;
  duongDanAnhSPs?: string[];
  dsBienThe?: VariantItem[];
}

export interface ProductDetail extends ProductSummary {
  duongDanAnhSPs: string[];
  dsBienThe: VariantItem[];
}

export interface ProductVariantDraft {
  maKichThuoc: number;
  maMauSac: number;
  soLuongTon: number;
  giaBan: number;
}

export interface ExistingProductVariantDraft {
  maChiTietSP: string;
  maKichThuoc: number;
  maMauSac: number;
  tenSize: string;
  tenMau: string;
  soLuongTon: number;
  giaBan: number;
}

export interface ProductFormValues {
  maSp: string;
  tenSp: string;
  maDanhMuc: number | null;
  giaGoc: number;
  chatLieu: string;
  moTa: string;
  newVariants: ProductVariantDraft[];
  existingVariants: ExistingProductVariantDraft[];
}

export interface ProductCreatePayload {
  maSp: string;
  tenSp: string;
  maDanhMuc: number;
  giaGoc: number;
  chatLieu?: string;
  moTa?: string;
  dsBienThe: ProductVariantDraft[];
}

export interface ProductUpdatePayload {
  maSp: string;
  tenSp: string;
  maDanhMuc: number;
  giaGoc: number;
  chatLieu?: string;
  moTa?: string;
}

export interface CreateVariantPayload extends ProductVariantDraft {
  maSP: string;
}

export interface UpdateVariantPayload {
  maChiTietSp: string;
  soLuongTon: number;
  giaBan?: number | null;
}

export interface CartItemView {
  maCTSP: string;
  maSP: string;
  tenSP: string;
  anh: string;
  tenMau: string;
  tenSize: string;
  gia: number;
  giaGoc: number;
  soLuongTon: number;
  soLuong: number;
  thanhTien: number;
}

export interface ServerCartItem {
  maGioHang: string;
  maSP: string;
  tenSP: string;
  anh: string;
  tenSize: string;
  tenMau: string;
  soLuongTon: number;
  maChiTietSp?: string;
  maChiTietSP?: string;
  soLuong: number;
  giaGoc: number;
  giaDatHang: number;
  thanhTien: number;
}

export interface CartResponse {
  maGioHang: string;
  maTaiKhoan?: string | null;
  sanPhams: ServerCartItem[];
  tongSoLuongItem: number;
  tongTien: number;
}

export interface OrderItem {
  maCTSP: string;
  tenSP: string;
  tenMau: string;
  tenSize: string;
  soLuong: number;
  donGia: number;
  hinhAnhUrl?: string | null;
}

export interface Order {
  maDonHang: string;
  ngayDatHang: string;
  trangThaiDonHang: string;
  tongTienHang: number;
  diaChiGiaoHang: string;
  phuongThucThanhToan: string;
  phuongThucVanChuyen: string;
  phiShip?: number | null;
  giamGiaVoucher?: number | null;
  ngayGiaoHang: string;
  thanhToan: number;
  chiTietDonHang: OrderItem[];
}

export interface CheckoutFormValues {
  tenNguoiNhan: string;
  soDienThoai: string;
  diaChiGiaoHang: string;
  maPTTT: number;
  maPTVC: number;
  maVoucher?: string;
}

export interface Voucher {
  maVoucher: string;
  tenVoucher?: string | null;
  giamGia: number;
  ngayBatDau: string;
  ngayKetThuc: string;
  dieuKienApDung?: number | null;
  moTa?: string | null;
  trangThai: string;
}
