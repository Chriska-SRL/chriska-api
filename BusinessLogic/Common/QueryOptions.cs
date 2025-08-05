namespace BusinessLogic.Common
{
    public record QueryOptions
    {
        private int _page = 1;
        private int _pageSize = 20;

        public int Page
        {
            get => _page <= 0 ? 1 : _page;
            set => _page = value;
        }

        public int PageSize
        {
            get => _pageSize <= 0 ? 20 : _pageSize > 100 ? 100 : _pageSize;
            set => _pageSize = value;
        }

        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "ASC"; //DESC
        public Dictionary<string, string>? Filters { get; set; }
    }

}
