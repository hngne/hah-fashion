using OfficeOpenXml;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Repository;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace QA_TMDT.Service.Impl
{
    public class ThongKeService : IThongKeService
    {
        private readonly IThongKeRepositoy _repo;

        public ThongKeService(IThongKeRepositoy repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<DoanhThuResponse>> GetDoanhThu(DateTime from, DateTime to, string Type)
        {
            return await _repo.GetDoanhThu(from, to, Type);
        }
        public async Task<IEnumerable<TopSanPhamResponse>> GetTopBanChay(int top)
        {
            return await _repo.GetTopBanChay(top);
        }

        public async Task<IEnumerable<TopSanPhamResponse>> GetTopBanE(int top)
        {
            return await _repo.GetTopKhongBanDuoc(top);
        }
        public async Task<byte[]> ExportExcelDoanhThu(DateTime from, DateTime to, string Type)
        {
            var data = await _repo.GetDoanhThu(from, to, Type);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("DoanhThu");

            // Dynamic Title based on Type
            string titleType = Type == "year" ? "NĂM" : (Type == "month" ? "THÁNG" : "NGÀY");
            string headerThoiGian = Type == "year" ? "Năm" : (Type == "month" ? "Tháng" : "Ngày");

            // Header
            ws.Cells[1, 1].Value = headerThoiGian;
            ws.Cells[1, 2].Value = "Số Đơn";
            ws.Cells[1, 3].Value = "Doanh Thu";
            ws.Cells[1, 1, 1, 3].Style.Font.Bold = true;

            int row = 2;
            foreach (var item in data)
            {
                ws.Cells[row, 1].Value = item.ThoiGian;
                ws.Cells[row, 2].Value = item.SoLuongDon;
                ws.Cells[row, 3].Value = item.DoanhThu;
                // Format tiền tệ
                ws.Cells[row, 3].Style.Numberformat.Format = "#,##0";
                row++;
            }
            ws.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
        // --- EXPORT PDF (QuestPDF) ---
        public async Task<byte[]> ExportPdfDoanhThu(DateTime from, DateTime to, string Type)
        {
            var data = await _repo.GetDoanhThu(from, to, Type);

            QuestPDF.Settings.License = LicenseType.Community;

            string title = Type switch
            {
                "year" => "BÁO CÁO DOANH THU THEO NĂM",
                "month" => "BÁO CÁO DOANH THU THEO THÁNG",
                _ => "BÁO CÁO DOANH THU THEO NGÀY"
            };

            string colHeader = Type == "year" ? "Năm" : (Type == "month" ? "Tháng" : "Ngày");

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                    page.Header().Text(title).SemiBold().FontSize(20).AlignCenter().FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(100);
                            c.ConstantColumn(100);
                            c.RelativeColumn();
                        });

                        table.Header(h =>
                        {
                            h.Cell().Element(CellStyle).Text(colHeader);
                            h.Cell().Element(CellStyle).Text("Số Đơn");
                            h.Cell().Element(CellStyle).Text("Doanh Thu");

                            static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).BorderBottom(1).Padding(5);
                        });

                        foreach (var item in data)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.ThoiGian);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.SoLuongDon.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.DoanhThu:N0} đ");
                        }
                    });

                    // Footer: Thời gian xuất báo cáo
                    page.Footer().AlignCenter().Text($"Xuất ngày: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            }).GeneratePdf();
        }
    }
}
