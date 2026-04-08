using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QA_TMDT.Model;

public partial class QaTmdtContext : DbContext
{
    public QaTmdtContext()
    {
    }

    public QaTmdtContext(DbContextOptions<QaTmdtContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnhSp> AnhSps { get; set; }

    public virtual DbSet<ChiTietDh> ChiTietDhs { get; set; }

    public virtual DbSet<ChiTietGh> ChiTietGhs { get; set; }

    public virtual DbSet<ChiTietKm> ChiTietKms { get; set; }

    public virtual DbSet<ChiTietSp> ChiTietSps { get; set; }

    public virtual DbSet<CuaHang> CuaHangs { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<KichThuoc> KichThuocs { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<PhuongThucTt> PhuongThucTts { get; set; }

    public virtual DbSet<PhuongThucVc> PhuongThucVcs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=REDAMANCY;Initial Catalog=QA_TMDT;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnhSp>(entity =>
        {
            entity.HasKey(e => e.MaAnh).HasName("PK__ANH_SP__184D773693B13261");

            entity.ToTable("ANH_SP");

            entity.Property(e => e.MaAnh).HasColumnName("maAnh");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("duongDan");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.AnhSps)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ANH_SP__maSP__4BAC3F29");
        });

        modelBuilder.Entity<ChiTietDh>(entity =>
        {
            entity.HasKey(e => new { e.MaDonHang, e.MaChiTietSp }).HasName("PK__CHI_TIET__345514E1C7E41ED2");

            entity.ToTable("CHI_TIET_DH");

            entity.Property(e => e.MaDonHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDonHang");
            entity.Property(e => e.MaChiTietSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maChiTietSP");
            entity.Property(e => e.DonGiaLucDat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGiaLucDat");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.MaChiTietSpNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaChiTietSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maChi__6C190EBB");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maDon__6B24EA82");
        });

        modelBuilder.Entity<ChiTietGh>(entity =>
        {
            entity.HasKey(e => new { e.MaGioHang, e.MaChiTietSp }).HasName("PK__CHI_TIET__9F3EFEFBD8776D8F");

            entity.ToTable("CHI_TIET_GH");

            entity.Property(e => e.MaGioHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maGioHang");
            entity.Property(e => e.MaChiTietSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maChiTietSP");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.MaChiTietSpNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaChiTietSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maChi__5EBF139D");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaGioHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maGio__5DCAEF64");
        });

        modelBuilder.Entity<ChiTietKm>(entity =>
        {
            entity.HasKey(e => new { e.MaKhuyenMai, e.MaSp }).HasName("PK__CHI_TIET__301CFA4E4FE2DF60");

            entity.ToTable("CHI_TIET_KM");

            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maKhuyenMai");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.PhanTramGiam).HasColumnName("phanTramGiam");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.ChiTietKms)
                .HasForeignKey(d => d.MaKhuyenMai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maKhu__52593CB8");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietKms)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET_K__maSP__534D60F1");
        });

        modelBuilder.Entity<ChiTietSp>(entity =>
        {
            entity.HasKey(e => e.MaChiTietSp).HasName("PK__CHI_TIET__3482CF8ACB4A2CE1");

            entity.ToTable("CHI_TIET_SP");

            entity.Property(e => e.MaChiTietSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maChiTietSP");
            entity.Property(e => e.AnhMinhHoa)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("anhMinhHoa");
            entity.Property(e => e.GiaBan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaBan");
            entity.Property(e => e.MaKichThuoc).HasColumnName("maKichThuoc");
            entity.Property(e => e.MaMauSac).HasColumnName("maMauSac");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.SoLuongTon)
                .HasDefaultValue(0)
                .HasColumnName("soLuongTon");

            entity.HasOne(d => d.MaKichThuocNavigation).WithMany(p => p.ChiTietSps)
                .HasForeignKey(d => d.MaKichThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maKic__47DBAE45");

            entity.HasOne(d => d.MaMauSacNavigation).WithMany(p => p.ChiTietSps)
                .HasForeignKey(d => d.MaMauSac)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maMau__48CFD27E");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietSps)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET_S__maSP__46E78A0C");
        });

        modelBuilder.Entity<CuaHang>(entity =>
        {
            entity.HasKey(e => e.MaCuaHang).HasName("PK__CUA_HANG__325705DC7C532EE2");

            entity.ToTable("CUA_HANG");

            entity.Property(e => e.MaCuaHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maCuaHang");
            entity.Property(e => e.DiaChi).HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TenCuaHang)
                .HasMaxLength(100)
                .HasColumnName("tenCuaHang");
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DANH_GIA__6B15DD9A42906C96");

            entity.ToTable("DANH_GIA");

            entity.Property(e => e.MaDanhGia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDanhGia");
            entity.Property(e => e.BinhLuan).HasColumnName("binhLuan");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");
            entity.Property(e => e.NgayDanhGia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayDanhGia");
            entity.Property(e => e.SoSao).HasColumnName("soSao");
            entity.Property(e => e.TrangThaiDg)
                .HasMaxLength(50)
                .HasColumnName("trangThaiDG");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("FK__DANH_GIA__maSP__70DDC3D8");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK__DANH_GIA__maTaiK__71D1E811");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DANH_MUC__6B0F914C8D204A5C");

            entity.ToTable("DANH_MUC");

            entity.Property(e => e.MaDanhMuc).HasColumnName("maDanhMuc");
            entity.Property(e => e.MaDanhMucCha).HasColumnName("maDanhMucCha");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.TenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("tenDanhMuc");

            entity.HasOne(d => d.MaDanhMucChaNavigation).WithMany(p => p.InverseMaDanhMucChaNavigation)
                .HasForeignKey(d => d.MaDanhMucCha)
                .HasConstraintName("FK_DanhMuc_Cha");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DON_HANG__871D3819D0965DA6");

            entity.ToTable("DON_HANG");

            entity.Property(e => e.MaDonHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDonHang");
            entity.Property(e => e.DiaChiGiaoHang).HasColumnName("diaChiGiaoHang");
            entity.Property(e => e.GiamGia)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giamGia");
            entity.Property(e => e.MaPttt).HasColumnName("maPTTT");
            entity.Property(e => e.MaPtvc).HasColumnName("maPTVC");
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");
            entity.Property(e => e.MaVoucher)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maVoucher");
            entity.Property(e => e.NgayDatHang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayDatHang");
            entity.Property(e => e.PhiVanChuyen)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("phiVanChuyen");
            entity.Property(e => e.SoDienThoaiNhan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("soDienThoaiNhan");
            entity.Property(e => e.TenNguoiNhan)
                .HasMaxLength(100)
                .HasColumnName("tenNguoiNhan");
            entity.Property(e => e.ThanhToanVnpay)
                .HasDefaultValue(false)
                .HasColumnName("thanhToanVNPay");
            entity.Property(e => e.TongThanhToan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongThanhToan");
            entity.Property(e => e.TongTienHang)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTienHang");
            entity.Property(e => e.TrangThaiDonHang)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ xác nhận")
                .HasColumnName("trangThaiDonHang");

            entity.HasOne(d => d.MaPtttNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPttt)
                .HasConstraintName("FK__DON_HANG__maPTTT__656C112C");

            entity.HasOne(d => d.MaPtvcNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPtvc)
                .HasConstraintName("FK__DON_HANG__maPTVC__68487DD7");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DON_HANG__maTaiK__6754599E");

            entity.HasOne(d => d.MaVoucherNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaVoucher)
                .HasConstraintName("FK__DON_HANG__maVouc__66603565");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GIO_HANG__2C76D203B27123B0");

            entity.ToTable("GIO_HANG");

            entity.HasIndex(e => e.MaTaiKhoan, "UQ__GIO_HANG__8FFF6A9C997202A7").IsUnique();

            entity.Property(e => e.MaGioHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maGioHang");
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithOne(p => p.GioHang)
                .HasForeignKey<GioHang>(d => d.MaTaiKhoan)
                .HasConstraintName("FK__GIO_HANG__maTaiK__5AEE82B9");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KHUYEN_M__87BEDDE98B78D4BE");

            entity.ToTable("KHUYEN_MAI");

            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maKhuyenMai");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.NgayBatDau)
                .HasColumnType("datetime")
                .HasColumnName("ngayBatDau");
            entity.Property(e => e.NgayKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("ngayKetThuc");
            entity.Property(e => e.TenKhuyenMai)
                .HasMaxLength(100)
                .HasColumnName("tenKhuyenMai");
        });

        modelBuilder.Entity<KichThuoc>(entity =>
        {
            entity.HasKey(e => e.MaSize).HasName("PK__KICH_THU__D83773A3F2B5265B");

            entity.ToTable("KICH_THUOC");

            entity.Property(e => e.MaSize).HasColumnName("maSize");
            entity.Property(e => e.TenSize)
                .HasMaxLength(50)
                .HasColumnName("tenSize");
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.MaMau).HasName("PK__MAU_SAC__27572EAE52DE34E2");

            entity.ToTable("MAU_SAC");

            entity.Property(e => e.MaMau).HasColumnName("maMau");
            entity.Property(e => e.MaHex)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maHex");
            entity.Property(e => e.TenMau)
                .HasMaxLength(50)
                .HasColumnName("tenMau");
        });

        modelBuilder.Entity<PhuongThucTt>(entity =>
        {
            entity.HasKey(e => e.MaPttt).HasName("PK__PHUONG_T__90380CE27EE6B81F");

            entity.ToTable("PHUONG_THUC_TT");

            entity.Property(e => e.MaPttt).HasColumnName("maPTTT");
            entity.Property(e => e.TenPttt)
                .HasMaxLength(100)
                .HasColumnName("tenPTTT");
        });

        modelBuilder.Entity<PhuongThucVc>(entity =>
        {
            entity.HasKey(e => e.MaPtvc).HasName("PK__PHUONG_T__90385F92E3A7DB66");

            entity.ToTable("PHUONG_THUC_VC");

            entity.Property(e => e.MaPtvc).HasColumnName("maPTVC");
            entity.Property(e => e.PhiVanChuyen)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("phiVanChuyen");
            entity.Property(e => e.TenPtvc)
                .HasMaxLength(100)
                .HasColumnName("tenPTVC");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SAN_PHAM__7A227A7AC0AA77D3");

            entity.ToTable("SAN_PHAM");

            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.ChatLieu)
                .HasMaxLength(100)
                .HasColumnName("chatLieu");
            entity.Property(e => e.GiaGoc)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaGoc");
            entity.Property(e => e.MaDanhMuc).HasColumnName("maDanhMuc");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.TenSp)
                .HasMaxLength(200)
                .HasColumnName("tenSP");
            entity.Property(e => e.TenTimKiem)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("tenTimKiem");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("FK__SAN_PHAM__maDanh__4316F928");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TAI_KHOA__8FFF6A9D7E61AADF");

            entity.ToTable("TAI_KHOAN");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TAI_KHOA__59267D4A6539968A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__TAI_KHOA__AB6E6164B47F26DC").IsUnique();

            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");
            entity.Property(e => e.DiaChi).HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("hoTen");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("matKhau");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tenDangNhap");
            entity.Property(e => e.VaiTro)
                .HasMaxLength(50)
                .HasColumnName("vaiTro");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.MaVoucher).HasName("PK__VOUCHER__E335C40091A1527D");

            entity.ToTable("VOUCHER");

            entity.Property(e => e.MaVoucher)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maVoucher");
            entity.Property(e => e.DieuKienApDung)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("dieuKienApDung");
            entity.Property(e => e.GiamGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giamGia");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.NgayBatDau)
                .HasColumnType("datetime")
                .HasColumnName("ngayBatDau");
            entity.Property(e => e.NgayKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("ngayKetThuc");
            entity.Property(e => e.TenVoucher)
                .HasMaxLength(100)
                .HasColumnName("tenVoucher");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
