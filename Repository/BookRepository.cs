using BookStore.API.Data;
using BookStore.API.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper.QueryableExtensions;
using BookStore.API.Common;
using BookStore.API.Helpers;
using Microsoft.AspNetCore.Http.Extensions;

namespace BookStore.API.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BookRepository(BookStoreContext _context, IMapper _mapper, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            mapper = _mapper;
            httpContextAccessor = _httpContextAccessor;
        }
        //public async Task<List<BookModel>> GetAllBooksAsync()
        //{
        //    //var records = await context.Books.Select(x => new BookModel()
        //    //{
        //    //    Id = x.Id,
        //    //    Title = x.Title,
        //    //    Description = x.Description
        //    //}).ToListAsync();
        //    //return records;

        //    var records = await context.Books.ToListAsync();
        //    return mapper.Map<List<BookModel>>(records);
        //}


        public async Task<ListResponseModel<BookModel>> GetAllBooksAsync(ListRequestModel requestModel)
        {
            var books = context.Books.ToList();

            if (requestModel.Filters != null && requestModel.Filters.Any())
            {
                books = ListExtensions.DynamicColumnDynamicOperatorWithMultipleCondition(books, requestModel.Filters);
            }
            if (!string.IsNullOrEmpty(requestModel.Sorting?.SortBy))
            {
                books = books.ApplySorting(requestModel);
            }

            var result = mapper.Map<List<BookModel>>(books);
            var filteredResult = result.ApplyPagination<BookModel>(requestModel);

            //var currentPage = requestModel.Pagination?.PageNo ?? 1;
            //var LastPage = (books.Count % requestModel.Pagination.PageSize != 0) ? (books.Count / requestModel.Pagination.PageSize)+1 : (books.Count / requestModel.Pagination.PageSize);
            //var urlDetails = ListExtensions.GetUrl(httpContextAccessor, currentPage, LastPage);

            var totalPages = (int)Math.Ceiling((double)books.Count / requestModel.Pagination.PageSize);
            string? previousPageUrl = null;
            string? nextPageUrl = null;
            if (requestModel.Pagination.PageNo != 1)
                previousPageUrl = ListExtensions.GenerateUrl(httpContextAccessor, requestModel.Pagination.PageNo - 1);
            if (requestModel.Pagination.PageNo != totalPages)
                nextPageUrl = ListExtensions.GenerateUrl(httpContextAccessor, requestModel.Pagination.PageNo + 1);

            return new ListResponseModel<BookModel>()
            {
                //PageNumber = requestModel.Pagination.PageNo == null ? 1 : requestModel.Pagination.PageNo,
                //PageNumber = currentPage,
                //PageSize = requestModel.Pagination.PageSize,
                //TotalPages = LastPage,
                //TotalItems = books.Count,
                Pagination = new PaginationResponseModel()
                {
                    CurrentPage = requestModel.Pagination.PageNo,
                    PageSize = requestModel.Pagination.PageSize,
                    TotalItems = books.Count,
                    TotalPages = totalPages
                },
                Result = filteredResult,
                Filters = requestModel.Filters?.Count == 0 ? null : (from x in requestModel.Filters
                                                                     select new FilterResponseModel
                                                                     {
                                                                         Column = x.Column,
                                                                         Operator = x.Operator.ToString(),
                                                                         Values = x.Values
                                                                     }).ToList(),
                Sort = requestModel.Sorting is null ? null : new SortResponseModel()
                {
                    SortBy = requestModel.Sorting.SortBy,
                    SortOrder = (requestModel.Sorting.SortOrder) ? "Ascending" : "Descending"
                },
                Url = new UrlModel()
                {
                    CurrentUrl = httpContextAccessor.HttpContext.Request.GetDisplayUrl(),
                    FirstPageUrl = ListExtensions.GenerateUrl(httpContextAccessor, 1),
                    LastPageUrl = ListExtensions.GenerateUrl(httpContextAccessor, totalPages),
                    NextUrl = nextPageUrl,
                    PreviousUrl = previousPageUrl
                }
            };
        }

        public async Task<BookModel> GetBookByIdAsync(int bookId)
        {
            //var book = await context.Books.Where(x => x.Id == bookId).Select(x => new BookModel()
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description
            //}).FirstOrDefaultAsync();
            //return book;

            var book = await context.Books.FindAsync(bookId);
            return mapper.Map<BookModel>(book);
        }

        public async Task<int> AddBookAsync(BookModel bookModel)
        {
            var book = new Books
            {
                Title = bookModel.Title,
                Description = bookModel.Description
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return book.Id;
        }

        //public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        //{
        //    var book = await context.Books.FindAsync(bookId);
        //    if (book != null)
        //    {
        //        book.Title = bookModel.Title;
        //        book.Description = bookModel.Description;

        //        await context.SaveChangesAsync();
        //    }
        //}

        public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        {
            var book = new Books
            {
                Id = bookId,
                Title = bookModel.Title,
                Description = bookModel.Description
            };
            context.Books.Update(book);
            await context.SaveChangesAsync();
        }

        public async Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book != null)
            {
                bookModel.ApplyTo(book);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteBookAsync(int bookId)
        {
            var book = new Books()
            {
                Id = bookId
            };
            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }





    }
}
