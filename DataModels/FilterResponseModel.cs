using static BookStore.API.Common.Enumerations;

namespace BookStore.API.DataModels
{
    public class FilterResponseModel
    {
        public string? Column { get; set; }
        public string? Operator { get; set; }
        public string[]? Values { get; set; }
    }
}
