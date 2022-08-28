using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aspnet_core_dotnet_core.Model;
using aspnet_core_dotnet_core.Helper;
using Microsoft.AspNetCore.Authorization;

namespace aspnet_core_dotnet_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BooksController : Controller
    {
        private readonly DataContext _context;
        private JsonResponser responser = new JsonResponser();

        public BooksController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet, Authorize]
        // GET: Books
        public async Task<IActionResult> Index()
        {
            return Ok(responser.response("Books successfully retreived.", await _context.Books.ToListAsync()));
        }

        [HttpGet("{id}"), Authorize]
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound(responser.response("Book not found."));
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.id == id);
            if (book == null)
            {
                return NotFound(responser.response("Book not found."));
            }

            return Ok(responser.response("Book successfully retreived.", book));
        }

        // POST: Books
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(BookDTO request)
        {
            Book book = new Book();
            book.Name = request.Name;
            book.Author = request.Author;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Ok(responser.response("Book successfully created.", book));
        }

        // PUT: Books/Edit/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Edit(int id, BookDTO request)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound(responser.response("Book not found."));

            book.Name = request.Name;
            book.Author = request.Author;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return Ok(responser.response("Book successfully updated.", book));
        }

        [HttpDelete("{id}"), Authorize]
        // POST: Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound(responser.response("Book not found."));
            

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok(responser.response("Book successfully deleted.", book));
        }
    }
}
