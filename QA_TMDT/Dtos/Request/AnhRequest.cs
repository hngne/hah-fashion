namespace QA_TMDT.Dtos.Request
{
    public class AnhRequest
    {
        public string maSp { get; set; } = string.Empty;
        public List<IFormFile>? AnhSPs { get; set; }
}
}
