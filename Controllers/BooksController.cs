//using Azure;
using BookStore.API.Data;
using BookStore.API.DataModels;
using BookStore.API.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;
        private readonly ILogger<BooksController> logger;

        public BooksController(IBookRepository bookRepository, ILogger<BooksController> logger)
        {
            this.bookRepository = bookRepository;
            this.logger = logger;
        }

        //[HttpGet("")]
        //public async Task<IActionResult> GetAllBooks()
        //{
        //    logger.LogInformation("GetAllBooks Called");
        //    var books  = await bookRepository.GetAllBooksAsync();
        //    logger.LogInformation($"{books.Count} books");
        //    return Ok(books);
        //}

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks([FromQuery] ListRequestModel requestModel)
        {
            logger.LogInformation("GetAllBooks Called");
            var response = await bookRepository.GetAllBooksAsync(requestModel);
            logger.LogInformation($"{response.Pagination.TotalItems} books");
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBookById(int Id)
        {
            var book = await bookRepository.GetBookByIdAsync(Id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBook([FromBody] BookModel bookModel)
        {
            var id = await bookRepository.AddBookAsync(bookModel);
            return CreatedAtAction("GetBookById", new { id = id, Controller = "Books" }, id);
            //return CreatedAtAction(nameof(GetBookById), new { id = id }, null);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int Id, [FromBody] BookModel bookModel)
        {
            await bookRepository.UpdateBookAsync(Id, bookModel);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBookPatch([FromBody] JsonPatchDocument bookModel, [FromRoute] int id)
        {
            await bookRepository.UpdateBookPatchAsync(id, bookModel);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            await bookRepository.DeleteBookAsync(id);
            return Ok();
        }
    }
}
