CREATE DATABASE QA_TMDT
GO
USE QA_TMDT
GO

-- 1. BẢNG TÀI KHOẢN
CREATE TABLE TAI_KHOAN (
    maTaiKhoan VARCHAR(50) PRIMARY KEY, -- GUID
    tenDangNhap VARCHAR(100) NOT NULL UNIQUE,
    matKhau VARCHAR(255) NOT NULL,
    email VARCHAR(100) UNIQUE,
    hoTen NVARCHAR(100), -- Tiếng Việt
    soDienThoai VARCHAR(20),
    vaiTro NVARCHAR(50), 
    diaChi NVARCHAR(MAX), -- Tiếng Việt
    ngayTao DATETIME DEFAULT GETDATE()
);

-- 2. DANH MỤC (Giữ INT để sort cho dễ, ít khi xóa)
CREATE TABLE DANH_MUC (
    maDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    tenDanhMuc NVARCHAR(100) NOT NULL, -- Tiếng Việt
    moTa NVARCHAR(MAX) -- Tiếng Việt
);

-- 3. KÍCH THƯỚC & MÀU SẮC (Giữ INT cho nhẹ bảng biến thể)
CREATE TABLE KICH_THUOC (
    maSize INT IDENTITY(1,1) PRIMARY KEY,
    tenSize NVARCHAR(50) NOT NULL -- S, M, L...
);

CREATE TABLE MAU_SAC (
    maMau INT IDENTITY(1,1) PRIMARY KEY,
    tenMau NVARCHAR(50) NOT NULL, -- Đỏ, Xanh
    maHex VARCHAR(20) -- #FFFFFF
);

-- 4. SẢN PHẨM (Đã đổi PK sang VARCHAR)
CREATE TABLE SAN_PHAM (
    maSP VARCHAR(50) PRIMARY KEY, -- GUID
    tenSP NVARCHAR(200) NOT NULL, -- Tiếng Việt
    tenTimKiem VARCHAR(200), -- Không dấu (để search)
    maDanhMuc INT,
    moTa NVARCHAR(MAX), -- Tiếng Việt HTML
    giaGoc DECIMAL(18, 2) NOT NULL,
    chatLieu NVARCHAR(100), -- Tiếng Việt
    trangThai BIT DEFAULT 1,
    FOREIGN KEY (maDanhMuc) REFERENCES DANH_MUC(maDanhMuc)
);

-- 5. CHI TIẾT SẢN PHẨM (SKU - Đã đổi PK sang VARCHAR)
CREATE TABLE CHI_TIET_SP (
    maChiTietSP VARCHAR(50) PRIMARY KEY, -- GUID
    maSP VARCHAR(50) NOT NULL,
    maKichThuoc INT NOT NULL,
    maMauSac INT NOT NULL,
    soLuongTon INT DEFAULT 0,
    giaBan DECIMAL(18, 2) NULL, 
    anhMinhHoa VARCHAR(255), 
    
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP),
    FOREIGN KEY (maKichThuoc) REFERENCES KICH_THUOC(maSize),
    FOREIGN KEY (maMauSac) REFERENCES MAU_SAC(maMau)
);

-- 6. ẢNH SẢN PHẨM
CREATE TABLE ANH_SP (
    maAnh INT IDENTITY(1,1) PRIMARY KEY, -- Ảnh thì để tự tăng cũng được
    maSP VARCHAR(50) NOT NULL,
    duongDan VARCHAR(255) NOT NULL,
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP)
);

-- 7. VOUCHER (Giảm thẳng tiền)
CREATE TABLE VOUCHER (
    maVoucher VARCHAR(50) PRIMARY KEY, -- Code nhập tay: SALE50
    tenVoucher NVARCHAR(100), -- Tiếng Việt
    giamGia DECIMAL(18, 2) NOT NULL, 
    ngayBatDau DATETIME NOT NULL,
    ngayKetThuc DATETIME NOT NULL,
    dieuKienApDung DECIMAL(18, 0),
    moTa NVARCHAR(MAX) -- Tiếng Việt
);

-- 8. KHUYẾN MÃI
CREATE TABLE KHUYEN_MAI (
    maKhuyenMai VARCHAR(50) PRIMARY KEY, -- GUID hoặc mã tự nhập
    tenKhuyenMai NVARCHAR(100) NOT NULL, -- Tiếng Việt
    ngayBatDau DATETIME NOT NULL,
    ngayKetThuc DATETIME NOT NULL,
    moTa NVARCHAR(MAX) -- Tiếng Việt
);

CREATE TABLE CHI_TIET_KM (
    maKhuyenMai VARCHAR(50),
    maSP VARCHAR(50), -- Link với SP cha
    phanTramGiam INT NOT NULL,
    PRIMARY KEY (maKhuyenMai, maSP),
    FOREIGN KEY (maKhuyenMai) REFERENCES KHUYEN_MAI(maKhuyenMai),
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP)
);

-- 9. THANH TOÁN & VẬN CHUYỂN
CREATE TABLE PHUONG_THUC_TT (
    maPTTT INT IDENTITY(1,1) PRIMARY KEY,
    tenPTTT NVARCHAR(100) NOT NULL -- Tiếng Việt
);

CREATE TABLE PHUONG_THUC_VC (
    maPTVC INT IDENTITY(1,1) PRIMARY KEY,
    tenPTVC NVARCHAR(100) NOT NULL, -- Tiếng Việt
    phiVanChuyen DECIMAL(18, 2) NOT NULL
);

-- 10. GIỎ HÀNG (Đã đổi PK sang VARCHAR)
CREATE TABLE GIO_HANG (
    maGioHang VARCHAR(50) PRIMARY KEY, -- GUID
    maTaiKhoan VARCHAR(50) UNIQUE,
    FOREIGN KEY (maTaiKhoan) REFERENCES TAI_KHOAN(maTaiKhoan)
);

CREATE TABLE CHI_TIET_GH (
    maGioHang VARCHAR(50),
    maChiTietSP VARCHAR(50), -- Link tới SKU (Biến thể)
    soLuong INT NOT NULL,
    PRIMARY KEY (maGioHang, maChiTietSP),
    FOREIGN KEY (maGioHang) REFERENCES GIO_HANG(maGioHang),
    FOREIGN KEY (maChiTietSP) REFERENCES CHI_TIET_SP(maChiTietSP)
);

-- 11. ĐƠN HÀNG (Đã đổi PK sang VARCHAR)
CREATE TABLE DON_HANG (
    maDonHang VARCHAR(50) PRIMARY KEY, -- GUID
    maTaiKhoan VARCHAR(50) NOT NULL,
    maPTTT INT,
    maPTVC INT,
    maVoucher VARCHAR(50) NULL,
    
    tenNguoiNhan NVARCHAR(100), -- Tiếng Việt
    soDienThoaiNhan VARCHAR(20),
    diaChiGiaoHang NVARCHAR(MAX) NOT NULL, -- Tiếng Việt
    
    tongTienHang DECIMAL(18, 2) NOT NULL,
    phiVanChuyen DECIMAL(18, 2),
    giamGia DECIMAL(18, 2) DEFAULT 0,
    tongThanhToan DECIMAL(18, 2) NOT NULL,
    
    trangThaiDonHang NVARCHAR(50) DEFAULT N'Chờ xác nhận', -- Tiếng Việt
    ngayDatHang DATETIME DEFAULT GETDATE(),
    thanhToanVNPay BIT DEFAULT 0,
    
    FOREIGN KEY (maPTTT) REFERENCES PHUONG_THUC_TT(maPTTT),
    FOREIGN KEY (maVoucher) REFERENCES VOUCHER(maVoucher),
    FOREIGN KEY (maTaiKhoan) REFERENCES TAI_KHOAN(maTaiKhoan),
    FOREIGN KEY (maPTVC) REFERENCES PHUONG_THUC_VC(maPTVC)
);

CREATE TABLE CHI_TIET_DH (
    maDonHang VARCHAR(50),
    maChiTietSP VARCHAR(50), -- Link tới SKU
    soLuong INT NOT NULL,
    donGiaLucDat DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (maDonHang, maChiTietSP),
    FOREIGN KEY (maDonHang) REFERENCES DON_HANG(maDonHang),
    FOREIGN KEY (maChiTietSP) REFERENCES CHI_TIET_SP(maChiTietSP)
);

-- 12. ĐÁNH GIÁ (Đã đổi PK sang VARCHAR)
CREATE TABLE DANH_GIA (
    maDanhGia VARCHAR(50) PRIMARY KEY, -- GUID
    maSP VARCHAR(50),
    maTaiKhoan VARCHAR(50),
    soSao INT CHECK (soSao BETWEEN 1 AND 5),
    binhLuan NVARCHAR(MAX), -- Tiếng Việt
    ngayDanhGia DATETIME DEFAULT GETDATE(),
    trangThaiDG NVARCHAR(50),
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP),
    FOREIGN KEY (maTaiKhoan) REFERENCES TAI_KHOAN(maTaiKhoan)
);
-- 13. CỬA HÀNG
CREATE TABLE CUA_HANG (
    maCuaHang VARCHAR(50) PRIMARY KEY, -- GUID
    tenCuaHang NVARCHAR(100) NOT NULL,
    diaChi NVARCHAR(MAX) NOT NULL,
    soDienThoai VARCHAR(20),
    email VARCHAR(100),
    moTa NVARCHAR(MAX)
);
GO
ALTER TABLE DANH_MUC
ADD maDanhMucCha INT NULL;
GO

-- 2. Thêm khóa ngoại trỏ về chính bảng DANH_MUC
ALTER TABLE DANH_MUC
ADD CONSTRAINT FK_DanhMuc_Cha
FOREIGN KEY (maDanhMucCha) REFERENCES DANH_MUC(maDanhMuc);
GO
INSERT INTO TAI_KHOAN (maTaiKhoan, tenDangNhap, matKhau, email, hoTen, soDienThoai, vaiTro, diaChi) VALUES ('0f4f9a9f-2a0c-4a2b-9a6a-3b7c8e1a1111', 'admin1', 'AQAAAAIAAYagAAAAEM0qJZEyAd0FPve/1SEN52HonqeFFKkA+DVoFfb1xT/KECS8dl/aq94NeX45dCEnHw==', 'admin1@unishop.vn', N'Quản trị viên 1', '0901000101', N'admin', N'Số 1, Quận Ba Đình, Hà Nội'), ('8a2b5c7d-9e01-4f23-b456-7890abcde222', 'admin2', 'AQAAAAIAAYagAAAAEM0qJZEyAd0FPve/1SEN52HonqeFFKkA+DVoFfb1xT/KECS8dl/aq94NeX45dCEnHw==', 'admin2@unishop.vn', N'Quản trị viên 2', '0902000202', N'admin', N'Số 2, Quận 1, TP. Hồ Chí Minh'); GO
INSERT INTO PHUONG_THUC_VC (tenPTVC, phiVanChuyen) VALUES (N'Nhận tại cửa hàng', 0), (N'Giao tại nhà', 30000);
GO
-- Thanh toán: COD
INSERT INTO PHUONG_THUC_TT (tenPTTT) VALUES
(N'Thanh toán khi nhận hàng (COD)');

-- Danh mục gốc
DECLARE @NamId INT, @NuId INT, @TreEmId INT;

INSERT INTO DANH_MUC (tenDanhMuc, moTa) VALUES (N'Nam', N'Thời trang nam');
SET @NamId = SCOPE_IDENTITY();

INSERT INTO DANH_MUC (tenDanhMuc, moTa) VALUES (N'Nữ', N'Thời trang nữ');
SET @NuId = SCOPE_IDENTITY();

INSERT INTO DANH_MUC (tenDanhMuc, moTa) VALUES (N'Trẻ em', N'Thời trang trẻ em');
SET @TreEmId = SCOPE_IDENTITY();

-- Danh mục con cho Nam
INSERT INTO DANH_MUC (tenDanhMuc, moTa, maDanhMucCha) VALUES
(N'Áo thun nam', N'Áo thun cotton, basic', @NamId),
(N'Áo sơ mi nam', N'Sơ mi công sở, casual', @NamId),
(N'Áo polo nam', N'Áo polo ngắn tay, dài tay', @NamId),
(N'Áo khoác nam', N'Áo gió, áo phao, hoodie', @NamId),
(N'Quần jean nam', N'Jean slim, straight', @NamId),
(N'Quần short nam', N'Short kaki, thể thao', @NamId);

-- Danh mục con cho Nữ
INSERT INTO DANH_MUC (tenDanhMuc, moTa, maDanhMucCha) VALUES
(N'Áo thun nữ', N'Áo thun cơ bản, oversize', @NuId),
(N'Áo sơ mi nữ', N'Sơ mi công sở, thời trang', @NuId),
(N'Áo khoác nữ', N'Áo gió, trench coat, hoodie', @NuId),
(N'Váy liền', N'Váy công sở, dạo phố', @NuId),
(N'Chân váy', N'Chân váy xòe, bút chì', @NuId),
(N'Quần jean nữ', N'Jean skinny, mom', @NuId);

-- Danh mục con cho Trẻ em
INSERT INTO DANH_MUC (tenDanhMuc, moTa, maDanhMucCha) VALUES
(N'Áo thun trẻ em', N'Áo thun bé trai, bé gái', @TreEmId),
(N'Áo khoác trẻ em', N'Áo gió, hoodie trẻ em', @TreEmId),
(N'Quần trẻ em', N'Quần jean, quần short trẻ em', @TreEmId),
(N'Váy trẻ em', N'Váy bé gái', @TreEmId);
GO
-- Khuyến mãi cuối năm và đầu năm mới
INSERT INTO KHUYEN_MAI (maKhuyenMai, tenKhuyenMai, ngayBatDau, ngayKetThuc, moTa) VALUES
('KM_NOEL_2025', N'Giáng Sinh Sale', '2025-12-19', '2025-12-31', N'Giảm giá mùa Giáng Sinh'),
('KM_TET_2026', N'Tết Sale 2026', '2025-12-20', '2026-02-15', N'Khuyến mãi lớn dịp Tết'),
('KM_NEWYEAR_2026', N'New Year Sale', '2026-01-01', '2026-01-31', N'Ưu đãi đầu năm mới');
INSERT INTO VOUCHER (maVoucher, tenVoucher, giamGia, ngayBatDau, ngayKetThuc, dieuKienApDung, moTa) VALUES ('SALE50', N'Giảm 50K', 50000, '2025-01-01', '2025-12-31', 300000, N'Giảm 50.000đ cho đơn từ 300.000đ'), ('NEWMEM10', N'Giảm 10%', 0.10, '2025-01-01', '2025-12-31', 400000, N'Giảm 10% cho đơn từ 400.000đ'), ('FREESHIP', N'Miễn phí ship', 30000, '2025-01-01', '2025-12-31', 250000, N'Giảm phí vận chuyển 30.000đ'), ('XMAS100', N'Giáng Sinh 100K', 100000, '2025-12-01', '2025-12-31', 500000, N'Giảm 100.000đ mùa lễ'), ('TET200', N'Tết 200K', 200000, '2025-12-20', '2026-02-15', 800000, N'Giảm 200.000đ dịp Tết'); GO