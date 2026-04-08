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
SELECT * FROM DANH_MUC;
go
select * from TAI_KHOAN;
go
select * from SAN_PHAM
Join CHI_TIET_SP
On SAN_PHAM.maSP = CHI_TIET_SP.maSP;
go
select * from MAU_SAC;
go
select * from KICH_THUOC;
go
INSERT INTO KICH_THUOC (tenSize) VALUES
(N'XS'),
(N'S'),
(N'M'),
(N'L'),
(N'XL'),
(N'XXL'),
(N'3XL'),
(N'Free Size');
go
INSERT INTO MAU_SAC (tenMau, maHex) VALUES
(N'Đỏ', '#FF0000'),
(N'Xanh dương', '#0000FF'),
(N'Xanh lá', '#00FF00'),
(N'Vàng', '#FFFF00'),
(N'Đen', '#000000'),
(N'Trắng', '#FFFFFF'),
(N'Hồng', '#FFC0CB'),
(N'Nâu', '#8B4513'),
(N'Xám', '#808080'),
(N'Tím', '#800080');
go
select * from PHUONG_THUC_TT;
select * from PHUONG_THUC_VC;
go
select * from DON_HANG
inner join CHI_TIET_DH 
on DON_HANG.maDonHang = CHI_TIET_DH.maDonHang
go
select * from VOUCHER;