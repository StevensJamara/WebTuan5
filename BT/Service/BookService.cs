using BT.Models;
using System.Collections.Concurrent;

namespace BT.Service
{
    public class BookService : IBookService
    {
        private readonly ConcurrentBag<BookModel> _books = new ConcurrentBag<BookModel>();
        private int _nextId = 1;

        public async Task<IEnumerable<BookModel>> GetAllBooksAsync()
        {
            return await Task.FromResult(_books);
        }

        public async Task<BookModel> GetBookByIdAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            return await Task.FromResult(book);
        }

        public async Task AddBookAsync(BookModel book)
        {
            if (_books.Any(b => b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A book with the same title already exists.");
            }

            book.Id = _nextId++;
            _books.Add(book);
            await Task.CompletedTask;
        }

        public async Task UpdateBookAsync(BookModel book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.PublishedDate = book.PublishedDate;
            await Task.CompletedTask;
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            _books.TryTake(out book);
            await Task.CompletedTask;
        }

        public void ClearBooks()
        {
            while (_books.TryTake(out _)) { }
            _nextId = 1;
        }
    }
}
