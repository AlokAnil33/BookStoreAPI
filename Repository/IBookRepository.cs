using BookStore.API.DataModels;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStore.API.Repository
{
    public interface IBookRepository
    {
        //Task<List<BookModel>> GetAllBooksAsync();
        Task<ListResponseModel<BookModel>> GetAllBooksAsync(ListRequestModel requestModel);
        Task<BookModel> GetBookByIdAsync(int id);
        Task<int> AddBookAsync(BookModel bookModel);
        Task UpdateBookAsync(int bookId, BookModel bookModel);
        Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel);
        Task DeleteBookAsync(int bookId);
    }
}
