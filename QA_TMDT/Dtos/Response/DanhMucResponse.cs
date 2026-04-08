namespace QA_TMDT.Dtos.Response
{
    public class DanhMucResponse
    {
        public int MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; } = null!;

        public string? MoTa { get; set; }

        public int? MaDanhMucCha { get; set; }
    }
}
