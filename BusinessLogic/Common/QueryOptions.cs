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

        public static void CheckRangeDate(QueryOptions options, int maxRangeInDays)
        {
            if (options.Filters == null) return;

            foreach (var (key, _) in options.Filters)
            {
                if (!key.EndsWith("From", StringComparison.OrdinalIgnoreCase)) continue;

                var baseKey = key[..^4]; // Remueve "From"
                var fromKey = key;
                var toKey = baseKey + "To";

                if (!options.Filters.TryGetValue(fromKey, out var fromValue) || string.IsNullOrWhiteSpace(fromValue))
                    continue;

                if (!options.Filters.TryGetValue(toKey, out var toValue) || string.IsNullOrWhiteSpace(toValue))
                    continue;

                if (!DateTime.TryParse(fromValue, out var fromDate))
                    throw new ArgumentException($"El valor '{fromValue}' de '{fromKey}' no es una fecha válida.");

                if (!DateTime.TryParse(toValue, out var toDate))
                    throw new ArgumentException($"El valor '{toValue}' de '{toKey}' no es una fecha válida.");

                if (fromDate > toDate)
                    throw new ArgumentException($"El rango de fechas para '{baseKey}' es inválido: '{fromKey}' > '{toKey}'.");

                if ((toDate - fromDate).TotalDays > maxRangeInDays)
                    throw new ArgumentException($"El rango de fechas para '{baseKey}' no puede exceder los {maxRangeInDays} días.");
            }
        }
    }

}
