namespace BusinessLogic.Común
{
    public record QueryOptions
    {
        private int _page = 1;
        private int _pageSize = 20;

        public int Page
        {
            get => _page <= 0 ? 1 : _page;
            init => _page = value;
        }

        public int PageSize
        {
            get => _pageSize <= 0 ? 20 : _pageSize > 100 ? 100 : _pageSize;
            init => _pageSize = value;
        }

        public string SortBy { get; init; } = "Id";
        public string SortDirection { get; init; } = "ASC";
        public Dictionary<string, string>? Filters { get; init; }
    }

}
