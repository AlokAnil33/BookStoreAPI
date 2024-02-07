namespace BookStore.API.DataModels
{
    public class BookResponseModel
    {
        public List<BookModel> Books { get; set; } = new List<BookModel>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
