namespace API.Utils
{
    public static class ErrorUtils
    {
        public static object FormatError(ArgumentException ex)
        {
            var message = ex.Message.Contains(" (Parameter")
                ? ex.Message.Split(" (Parameter")[0]
                : ex.Message;

            return new { field = ex.ParamName, error = message };
        }
    }
}
