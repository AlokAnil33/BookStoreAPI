namespace BookStore.API.DataModels
{
    public class ListResponseModel<T>
    {
        //public int PageNumber { get; set; }
        //public int PageSize {  get; set; }
        //public int TotalItems{ get; set; }
        //public int TotalPages { get; set; }
        public PaginationResponseModel? Pagination { get; set; }
        public List<T>? Result { get; set;}
        public List<FilterResponseModel>? Filters { get; set;}
        public SortResponseModel? Sort { get; set; }
        public UrlModel? Url { get; set; }
    }
}
