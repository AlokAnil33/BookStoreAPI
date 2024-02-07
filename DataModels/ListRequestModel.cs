namespace BookStore.API.DataModels
{
    public class ListRequestModel
    { 
        public PaginationModel ? Pagination {  get; set; }
        public List<FilterModel>? Filters { get; set; }
        public SortingModel? Sorting { get; set; }

    }
}
