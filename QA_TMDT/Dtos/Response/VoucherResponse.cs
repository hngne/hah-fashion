namespace QA_TMDT.Dtos.Response
{
    public class VoucherResponse
    {
        public string MaVoucher { get; set; } = null!;

        public string? TenVoucher { get; set; }

        public decimal GiamGia { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public decimal? DieuKienApDung { get; set; }
        public string? MoTa { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}
