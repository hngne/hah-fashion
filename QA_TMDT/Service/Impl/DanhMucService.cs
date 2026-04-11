using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;
using QA_TMDT.Repository;
using QA_TMDT.Mapper;
using QA_TMDT.Dtos.Request;

namespace QA_TMDT.Service.Impl
{
    public class DanhMucService : IDanhMucService
    {
        private readonly IDanhMucRepository _repo;

        public DanhMucService(IDanhMucRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<DanhMucResponse>> GetAllDM()
        {
            var result = await _repo.GetAllDM();
            return result.Select(DanhMucBuilder.ToResponse).ToList();
        }
        public async Task<DanhMucResponse?> GetDMByMaDM(int madm)
        {
            var result = await _repo.GetDMByMaDM(madm);
            if (result == null)
            {
                return null;
            }
            return DanhMucBuilder.ToResponse(result);
        }
        public async Task<IEnumerable<DanhMucResponse>> GetDMByTenDM(string tendm)
        {
             var result = await _repo.GetDMByTenDM(tendm);
            if(result == null)
            {
                return null;
            }
            return result.Select(DanhMucBuilder.ToResponse);
        }
        public async Task<(bool, string, DanhMucResponse?)> PostDM(DanhMucRequest request)
        {
            request.TenDanhMuc = request.TenDanhMuc.Trim();
            request.MoTa = string.IsNullOrWhiteSpace(request.MoTa) ? null : request.MoTa.Trim();

            if (string.IsNullOrWhiteSpace(request.TenDanhMuc))
            {
                return (false, "Tên danh mục không được để trống", null);
            }

            if (request.MaDanhMucCha.HasValue)
            {
                var checkCha = await _repo.checkExistDM(request.MaDanhMucCha.Value);
                if (!checkCha)
                {
                    return (false, "Danh mục cha không tồn tại", null);
                }
            }
            
            var newDM = new DanhMuc();
            newDM.TenDanhMuc = request.TenDanhMuc;
            newDM.MoTa = request.MoTa;
            newDM.MaDanhMucCha = request.MaDanhMucCha;
            var success = await _repo.PostDM(newDM);
            if (!success)
            {
                return (false, "Có lỗi khi thêm", null);
            }
            return (true, "Thêm thành công danh mục", DanhMucBuilder.ToResponse(newDM));
        }
        public async Task<(bool, string, DanhMucResponse?)> UpdateDM(int madm, DanhMucRequest request)
        {
            var exist = await _repo.GetDMByMaDM(madm);
            if (exist == null)
            {
                return (false, "Danh mục không tồn tại", null);
            }

            request.TenDanhMuc = request.TenDanhMuc.Trim();
            request.MoTa = string.IsNullOrWhiteSpace(request.MoTa) ? null : request.MoTa.Trim();

            if (string.IsNullOrWhiteSpace(request.TenDanhMuc))
            {
                return (false, "Tên danh mục không được để trống", null);
            }

            if (request.MaDanhMucCha.HasValue)
            {
                // Rule: Không được chọn cha là chính mình (để tránh vòng lặp vô tận)
                if (request.MaDanhMucCha.Value == madm)
                {
                    return (false, "Không thể chọn danh mục cha là chính danh mục hiện tại", null);
                }

                // Check cha có tồn tại không
                var checkCha = await _repo.checkExistDM(request.MaDanhMucCha.Value);
                if (!checkCha)
                {
                    return (false, "Danh mục cha không tồn tại", null);
                }

                var currentParentId = request.MaDanhMucCha;
                while (currentParentId.HasValue)
                {
                    if (currentParentId.Value == madm)
                    {
                        return (false, "Không thể chọn danh mục con làm danh mục cha", null);
                    }

                    var parent = await _repo.GetDMByMaDM(currentParentId.Value);
                    currentParentId = parent?.MaDanhMucCha;
                }
            }

            exist.TenDanhMuc = request.TenDanhMuc;
            exist.MoTa = request.MoTa;
            exist.MaDanhMucCha = request.MaDanhMucCha;

            var success = await _repo.UpdateDM(exist);

            if (!success)
            {
                return (false, "Có lỗi khi sửa", null);
            }
            return (true, "Sửa thành công", DanhMucBuilder.ToResponse(exist));
        }
        public async Task<(bool, string)> DeleteDM(int madm)
        {
            var exist = await _repo.checkExistDM(madm);
            if (!exist)
            {
                return (false, "Danh mục không tồn tại");
            }

            var hasChild = await _repo.CheckHasChild(madm);
            if (hasChild)
            {
                return (false, "Không thể xóa: Danh mục này đang chứa các danh mục con. Hãy xóa danh mục con trước.");
            }

            // 3. Check xem có chứa sản phẩm không
            // (VD: Xóa 'Áo sơ mi' trong khi vẫn còn 100 cái áo thuộc loại này -> Chặn lại)
            var hasProduct = await _repo.CheckHasProduct(madm);
            if (hasProduct)
            {
                return (false, "Không thể xóa: Danh mục này đang chứa sản phẩm. Hãy xóa hoặc chuyển sản phẩm sang danh mục khác trước.");
            }

            var success = await _repo.DeleteDM(madm);
            if (!success)
            {
                return (false, "có lỗi khi xóa");
            }
            return (true, "Xóa thành công");
        }
    }
}
