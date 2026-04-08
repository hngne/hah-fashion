using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Service
{
    public interface IAnhService
    {
        Task<IEnumerable<AnhResponse>> GetAllAnh();
        Task<IEnumerable<AnhResponse>?> GetAnhByMaSp(string maSp);
        Task<(bool success, string message, IEnumerable<AnhResponse>? anhResponses)> AddAnh(AnhRequest request);
        Task<(bool success, string message)> DeleteAnhByMaAnh(int maAnh);
    }
}
