using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Service
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherResponse>> GetAllVouchers();
        Task<VoucherResponse?> GetVoucherByCode(string maVoucher);
        Task<(bool Success, string Message, VoucherResponse? response)> CreateVoucher(VoucherRequest request);
        Task<(bool Success, string Message, VoucherResponse? response)> UpdateVoucher(VoucherRequest request);
        Task<(bool Success, string Message)> DeleteVoucher(string maVoucher);
        Task<IEnumerable<VoucherResponse>> GetVouchersForUser(string maTk);
    }
}
