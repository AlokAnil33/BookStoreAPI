using static BookStore.API.Common.Enumerations;

namespace BookStore.API.DataModels
{
    public class FilterModel
    {
        public required string Column { get; set; }
        public Operators Operator { get; set; }
        public required string[] Values { get; set; }

    }
}
