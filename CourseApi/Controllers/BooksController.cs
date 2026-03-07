using CourseApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //使用靜態list作為資料來源
        private static List<Book> _books = new List<Book>()
       {
           new  Book {Id = 1, Title = "C#", Author ="A",Price = 500},
           new Book {Id = 2, Title = "Java", Author ="B",Price = 500},
           new Book {Id = 3, Title = "Python", Author ="C",Price = 500},
       };

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Book>))]

        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            await Task.Delay(1);
            return Ok(_books);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Book>))]

        public async Task<ActionResult<Book>> GetBooks(int id)
        {
            await Task.Delay(1);
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = $"找不到ID為{id}的書籍" });
            }
            return Ok(book);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest(new { message = $"書籍資料不為空" });
            }
            await Task.Delay(1);

            book.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;

            _books.Add(book);

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest(new { message = "URL中的id與書籍資料中的id不符合" });
            }
            await Task.Delay(1);

            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
            {
                return NotFound(new { message = $"找不到ID為{id}的書籍" });
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Price = book.Price;
            return Ok(new { message = "書籍更新成功" });
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteBook(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "ID必需大於0" });
            }
            await Task.Delay(1);
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = $"找不到ID{id}的書籍" });
            }
            _books.Remove(book);

            return Ok(new { message = "書籍資料刪除成功" });
        }
        
   [HttpGet("search")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Book>))]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "查詢關鍵字不可為空" });
            }

            await Task.Delay(1);

            var result = _books.Where(b =>
                b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            return Ok(result);
        }


    }
}


