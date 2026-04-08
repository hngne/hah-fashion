using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;
using System;

namespace QA_TMDT.Repository.Impl
{
    public class DanhMucRepository : IDanhMucRepository
    {
        private readonly QaTmdtContext _context;
        public DanhMucRepository(QaTmdtContext context)
        {
            _context = context;
        }
        public async Task<List<DanhMuc>> GetAllDM()
        {
            return await _context.DanhMucs.AsNoTracking().ToListAsync();
        }
        public async Task<DanhMuc?> GetDMByMaDM(int madm)
        {
            return await _context.DanhMucs.FirstOrDefaultAsync(dm => dm.MaDanhMuc == madm);
        }
        public async Task<List<DanhMuc>> GetDMByTenDM(string tendm)
        {
            var result = _context.DanhMucs.Where(dm => dm.TenDanhMuc.ToLower().Contains(tendm));
            if (!await result.AnyAsync())
            {
                return null;
            }
            var data = await result.ToListAsync();
            return data;
        }
        public async Task<bool> PostDM(DanhMuc dm)
        {
            try
            {
                _context.DanhMucs.Add(dm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateDM(DanhMuc dm)
        {
            try
            {
                _context.DanhMucs.Update(dm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteDM(int madm)
        {
            try
            {
                var exist = await _context.DanhMucs.FirstOrDefaultAsync(dm => dm.MaDanhMuc == madm);
                if(exist == null)
                {
                    return false;
                }
                _context.DanhMucs.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> checkExistDM(int madm)
        {
            var result = await _context.DanhMucs.AnyAsync(dm => dm.MaDanhMuc == madm);
            return result;
        }
        public async Task<bool> CheckHasChild(int madm)
        {
            // Tìm xem có thằng nào nhận madm này làm Cha không
            return await _context.DanhMucs.AnyAsync(dm => dm.MaDanhMucCha == madm);
        }

        public async Task<bool> CheckHasProduct(int madm)
        {
            // Tìm xem có sản phẩm nào thuộc danh mục này không
            return await _context.SanPhams.AnyAsync(sp => sp.MaDanhMuc == madm);
        }
    }
}
