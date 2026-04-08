namespace QA_TMDT.Helper
{
    public class PageResult<T>
    {
        public List<T> item { get; set; } = new List<T>();
        public int totalItem { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalPage => (int)Math.Ceiling((double)totalItem / pageSize);
    }
}
