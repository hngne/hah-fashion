namespace QA_TMDT.Dtos.Request
{
    public class DanhMucRequest
    {
        public string TenDanhMuc { get; set; } = null!;

        public string? MoTa { get; set; }

        public int? MaDanhMucCha { get; set; }
    }
}
