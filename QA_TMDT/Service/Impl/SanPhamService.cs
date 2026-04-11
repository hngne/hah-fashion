using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Helper;
using QA_TMDT.Repository;
using QA_TMDT.Mapper;
using QA_TMDT.Model;

namespace QA_TMDT.Service.Impl
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _repo;

        private readonly CloudinaryService _cloudinary;
        public SanPhamService(ISanPhamRepository repo, CloudinaryService cloudinary)
        {
            _repo = repo;
            _cloudinary = cloudinary;
        }
        public async Task<PageResult<SanPhamResponse>> GetAllSP(int page, int pageSize)
        {
            var (Items, TotalItem) = await _repo.GetAllSP(page, pageSize);

            var product = Items.Select(SanPhamBuilder.ToResponse).ToList();
            return new PageResult<SanPhamResponse>
            {
                item = product,
                totalItem = TotalItem,
                page = page,
                pageSize = pageSize
            };
        }
        public async Task<(bool, string, SanPhamResponse?)> CreateSP(SanPhamRequest request)
        {
            var checkMaSP = await _repo.CheckExistMaSP(request.MaSp);
            if (checkMaSP)
            {
                return (false, "Mã sản phẩm đã tồn tại", null);
            }
            var checkDanhMuc = await _repo.CheckDMExist(request.MaDanhMuc);
            if (!checkDanhMuc)
            {
                return (false, "Mã danh mục không tồn tại", null);
            }
            if(request.GiaGoc < 0)
            {
                return (false, "Giá gốc không được nhỏ hơn 0", null);
            }

            var imgUrl = new List<string>();
            if (request.AnhSps != null && request.AnhSps.Count > 0)
            {
                foreach (var img in request.AnhSps)
                {
                    var url = await _cloudinary.UploadImageAsync(img);
                    if (!string.IsNullOrEmpty(url))
                    {
                        imgUrl.Add(url);
                    }
                }
            }

            var ChiTietSPs = new List<ChiTietSp>();
            if(request.DsBienThe == null || request.DsBienThe.Count == 0)
            {
                return (false, "Phải có ít nhất một biến thể sản phẩm", null);
            }
            foreach(var ctsp in request.DsBienThe)
            {
                if(ctsp.SoLuongTon < 0)
                {
                    return (false, "Số lượng tồn không được nhỏ hơn 0", null);
                }
                if(ctsp.GiaBan < 0)
                {
                    return (false, "Giá bán không được nhỏ hơn 0", null);
                }
                var chitietsp = new ChiTietSp();
                chitietsp.MaChiTietSp = Guid.NewGuid().ToString();
                chitietsp.MaSp = request.MaSp;
                chitietsp.MaKichThuoc = ctsp.MaKichThuoc!.Value;
                chitietsp.MaMauSac = ctsp.MaMauSac!.Value;
                chitietsp.SoLuongTon = ctsp.SoLuongTon!.Value;
                chitietsp.GiaBan = ctsp.GiaBan;
                ChiTietSPs.Add(chitietsp);
            }

            var sp = new SanPham();
            sp.MaSp = request.MaSp;
            sp.TenSp = request.TenSp;
            sp.MoTa = request.MoTa;
            sp.GiaGoc = request.GiaGoc!.Value;
            sp.ChatLieu = request.ChatLieu;
            sp.MaDanhMuc = request.MaDanhMuc;
            sp.TenTimKiem = RemoveVNI.ConvertToUnsign(request.TenSp);
            sp.TrangThai = request.TrangThai ?? true;
            sp.AnhSps = imgUrl.Select(url => new AnhSp { 
                DuongDan = url,
            }).ToList();
            sp.ChiTietSps = ChiTietSPs;

            var success = await _repo.CreateSP(sp);
            if (!success)
            {
                return (false, "Có lỗi xảy ra khi tạo sản phẩm", null);
            }
            var result = await _repo.GetFullInFoByMaSP(sp.MaSp);
            return (true, "Thêm sản phẩm thành công", SanPhamBuilder.ToResponse(result!));
        }
        public async Task<(bool, string, ChiTietSanPhamResponse?)> CreateVariant(AddBienTheRequest request)
        {
            var checkSP = await _repo.CheckExistMaSP(request.MaSP);
            if(!checkSP)
            {
                return (false, "Mã sản phẩm không tồn tại", null);
            }
            if(request.SoLuongTon < 0)
            {
                return (false, "Số lượng tồn không được nhỏ hơn 0", null);
            }
            if(request.GiaBan < 0)
            {
                return (false, "Giá bán không được nhỏ hơn 0", null);
            }

            var newBienThe = new ChiTietSp();
            newBienThe.MaChiTietSp =  Guid.NewGuid().ToString();
            newBienThe.MaSp = request.MaSP;
            newBienThe.MaKichThuoc = request.MaKichThuoc!.Value;
            newBienThe.MaMauSac = request.MaMauSac!.Value;
            newBienThe.SoLuongTon = request.SoLuongTon!.Value;
            newBienThe.GiaBan = request.GiaBan;

            var success = await _repo.CreateVariant(newBienThe);
            if (!success)
            {
                return (false, "Có lỗi xảy ra khi tạo biến thể sản phẩm", null);
            }
            var result = await _repo.GetChiTietSPByMaCTSP(newBienThe.MaChiTietSp);
            return (true, "Thêm biến thể sản phẩm thành công", ChiTietSanPhamBuilder.ToResponse(result!));
        }
        public async Task<SanPhamResponse?> GetFullInFoByMaSP(string masp)
        {
            var checkSP = await _repo.CheckExistMaSP(masp);
            if(!checkSP)
            {
                return null;
            }
            var sp = await _repo.GetFullInFoByMaSP(masp);
            return SanPhamBuilder.ToResponse(sp!);
        }
        public async Task<PageResult<SanPhamResponse>> GetByMaDM(int maDM, int page, int pageSize)
        {
            var (Items, TotalItem) = await _repo.GetByMaDM(maDM, page, pageSize);

            var product = Items.Select(SanPhamBuilder.ToResponse).ToList();
            return new PageResult<SanPhamResponse>
            {
                item = product,
                totalItem = TotalItem,
                page = page,
                pageSize = pageSize
            };
        }
        public async Task<PageResult<SanPhamResponse>> GetByTenSP(string tenSP, int page, int pageSize)
        {
            var (Items, TotalItem) = await _repo.GetByTenSP(tenSP, page, pageSize);
            var product = Items.Select(SanPhamBuilder.ToResponse).ToList();
            return new PageResult<SanPhamResponse>
            {
                item = product,
                totalItem = TotalItem,
                page = page,
                pageSize = pageSize
            };
        }
        public async Task<ChiTietSanPhamResponse?> GetChiTietSPByMaCTSP(string maCTSP)
        {
            var checkCTSP = await _repo.CheckExistCTSP(maCTSP);
            if(!checkCTSP)
            {
                return null;
            }
            var ctsp = await _repo.GetChiTietSPByMaCTSP(maCTSP);
            return ChiTietSanPhamBuilder.ToResponse(ctsp!);
        }
        public async Task<(bool, string, ChiTietSanPhamResponse?)> UpdateStockCTSP(string maCTSP, int soluongthaydoi)
        {
            var success = await _repo.UpdateStockCTSP(maCTSP, soluongthaydoi);
            if (!success)
            {
                return (false, "Cập nhật số lượng tồn thất bại", null);
            }
            var ctsp = await _repo.GetChiTietSPByMaCTSP(maCTSP);
            return (true, "Cập nhật số lượng tồn thành công", ChiTietSanPhamBuilder.ToResponse(ctsp!));
        }
        public async Task<(bool, string, SanPhamResponse?)> UpdateSP(SanPhamRequest request)
        {
            var checkSP = await _repo.CheckExistMaSP(request.MaSp);
            if (!checkSP)
            {
                return (false, "Mã sản phẩm không tồn tại", null);
            }
            var checkDM = await _repo.CheckDMExist(request.MaDanhMuc);
            if (!checkDM)
            {
                return (false, "Mã danh mục không tồn tại", null);
            }
            var sp = await _repo.GetFullInFoByMaSP(request.MaSp);
            sp.TenSp = request.TenSp;
            sp.MoTa = request.MoTa;
            sp.MaDanhMuc = request.MaDanhMuc;
            sp.GiaGoc = request.GiaGoc!.Value;
            sp.ChatLieu = request.ChatLieu;
            sp.TenTimKiem = RemoveVNI.ConvertToUnsign(request.TenSp);

            var success = await _repo.UpdateSP(sp); 
            if (!success)
            {
                return (false, "Cập nhật sản phẩm thất bại", null);
            }
            var result = await _repo.GetFullInFoByMaSP(sp.MaSp);
            return (true, "Cập nhật sản phẩm thành công", SanPhamBuilder.ToResponse(result!));

        }
        public async Task<(bool, string, ChiTietSanPhamResponse?)> UpdateCTSP(UpdateCTSPRequest request)
        {
            var checkCTSP = await _repo.CheckExistCTSP(request.MaChiTietSp);
            if (!checkCTSP)
            {
                return (false, "Mã chi tiết sản phẩm không tồn tại", null);
            }
            if(request.SoLuongTon < 0)
            {
                return (false, "Số lượng tồn không được nhỏ hơn 0", null);
            }
            if(request.GiaBan < 0)
            {
                return (false, "Giá bán không được nhỏ hơn 0", null);
            }
            var ctsp = await _repo.GetChiTietSPByMaCTSP(request.MaChiTietSp);
            ctsp.GiaBan = request.GiaBan;
            ctsp.SoLuongTon = request.SoLuongTon!.Value;
            var success = await _repo.UpdateCTSP(ctsp);
            if (!success)
            {
                return (false, "Cập nhật chi tiết sản phẩm thất bại", null);
            }
            var result = await _repo.GetChiTietSPByMaCTSP(request.MaChiTietSp);
            return (true, "Cập nhật chi tiết sản phẩm thành công", ChiTietSanPhamBuilder.ToResponse(result!));
        }
        public async Task<(bool, string)> DeleteSP(string maSP)
        {
            var checkSP = await _repo.CheckExistMaSP(maSP);
            if (!checkSP)
            {
                return (false, "Mã sản phẩm không tồn tại");
            }
            var success = await _repo.DeleteSP(maSP);
            if (!success)
            {
                return (false, "Có lỗi khi xóa sản phẩm");
            }
            return (true, "Xóa sản phẩm thành công");
        }
    }
}
