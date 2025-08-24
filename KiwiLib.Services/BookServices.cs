using KiwiLib.DAL;
using KiwiLib.Dto;
using KiwiLib.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiLib.Services
{
    public class BookServices
    {
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly CategoryRepository _categoryRepository;

        public BookServices()
        {
            _bookRepository = new BookRepository();
            _authorRepository = new AuthorRepository();
            _categoryRepository = new CategoryRepository();
        }

        /// <summary>
        /// Generate test data for demo purpose
        /// </summary>
        public async Task GenerateDemoDataAsync()
        {
            var author1 = new Author(1, "J.K. Rowling");
            var author2 = new Author(2, "Jules Verne");
            var author3 = new Author(3, "J.R.R. Tolkien");
            var author4 = new Author(4, "Fujiko F. Fujio");

            var categoryFantasy = new Category(1, "Fantasy");
            var categorySciFi = new Category(2, "Science Fiction");
            var categoryManga = new Category(3, "Manga");

            await _authorRepository.AddAsync(author1);
            await _authorRepository.AddAsync(author2);
            await _authorRepository.AddAsync(author3);
            await _authorRepository.AddAsync(author4);

            await _categoryRepository.AddAsync(categoryFantasy);
            await _categoryRepository.AddAsync(categorySciFi);
            await _categoryRepository.AddAsync(categoryManga);

            await _bookRepository.AddAsync(new Book(0, Validation.GenerateRandomIsbn(), "Harry Potter and the Philosopher's Stone", categoryFantasy, author1, 1997));
            await _bookRepository.AddAsync(new Book(0, Validation.GenerateRandomIsbn(), "20,000 Leagues Under the Sea", categorySciFi, author2, 1870));
            await _bookRepository.AddAsync(new Book(0, Validation.GenerateRandomIsbn(), "The Lord of the Rings", categoryFantasy, author3, 1954));
            await _bookRepository.AddAsync(new Book(0, Validation.GenerateRandomIsbn(), "Doraemon, Vol. 1", categoryManga, author4, 1970));
        }

        /// <summary>
        /// Adds a new book
        /// </summary>
        public async Task<(bool Success, string Message, int BookId)> AddBookAsync(Book book)
        {
            if (book == null)
                return (false, "Book cannot be null.", 0);

            if (!Validation.IsValidISBN(book.Isbn, out var isbnError))
                return (false, $"Invalid ISBN: {isbnError}", 0);

            if (string.IsNullOrEmpty(book.Title))
                return (false, $"Missing book title", 0);

            if (book.Id > 0)
            {
                var existing = await _bookRepository.GetAsync(book.Id);
                if (existing != null)
                    return (false, $"A book with ID {book.Id} already exists.", 0);
            }

            var bookId = await _bookRepository.AddAsync(book);
            return bookId > 0
                ? (true, "Book added successfully.", bookId)
                : (false, "Failed to add book.", 0);
        }

        /// <summary>
        /// Updates a book.
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateBookAsync(Book book)
        {
            if (book == null)
                return (false, "Book cannot be null.");

            if (!Validation.IsValidISBN(book.Isbn, out var isbnError))
                return (false, $"Invalid ISBN: {isbnError}");

            if (string.IsNullOrEmpty(book.Title))
                return (false, $"Missing book title");

            var existing = await _bookRepository.GetAsync(book.Id);
            if (existing == null)
                return (false, "Book not found.");

            bool updated = await _bookRepository.UpdateAsync(book);
            return updated
                ? (true, "Book updated.")
                : (false, "Failed to update book.");
        }

        /// <summary>
        /// Deletes a book by ID.
        /// </summary>
        public async Task<(bool Success, string Message)> DeleteBookAsync(int id)
        {
            var existing = await _bookRepository.GetAsync(id);
            if (existing == null)
                return (false, "Book not found.");

            bool deleted = await _bookRepository.DeleteAsync(existing);
            return deleted
                ? (true, "Book deleted.")
                : (false, "Failed to delete book.");
        }

        /// <summary>
        /// Get book.
        /// </summary>
        public async Task<Book> GetBooksAsync(int bookId)
        {
            if (bookId <= 0)
                return null;
            var book = await _bookRepository.GetAsync(bookId);
            return book;
        }

        /// <summary>
        /// Get all books.
        /// </summary>
        public async Task<List<Book>> GetAllBooksAsync()
        {
            List<Book> result = new List<Book>();

            var books = await _bookRepository.GetAllAsync();
            foreach (var book in books)
                result.Add(book);

            return result;
        }

        /// <summary>
        /// Search books.
        /// </summary>
        public async Task<List<Book>> SearchBookAsync(string searchString, bool searchIsbnOrIdOnly = false)
        {
            List<Book> result = new List<Book>();

            // Normalize search string
            searchString = searchString.Trim().ToLower();

            // [Vi] This is not the best approach for searching as it fetchs all books to service layer to do filtering. Need to move this to DAL layer for better performance
            // However, I think this is good enough for demo purpose
            var allBooks = await _bookRepository.GetAllAsync();

            result = allBooks
            .Where(b =>
                (b.Title.ToLower().Contains(searchString) && !searchIsbnOrIdOnly) ||
                b.Isbn.ToLower().Equals(searchString.ToLower().Replace("-", ""), StringComparison.OrdinalIgnoreCase) ||
                (b.Author.FullName.ToLower().Contains(searchString) && !searchIsbnOrIdOnly) ||
                (b.Category.Name.ToLower().Contains(searchString)) && !searchIsbnOrIdOnly)
            .ToList();

            //Extra search by book id
            if (int.TryParse(searchString, out var bookId))
            {
                var book = await _bookRepository.GetAsync(bookId);
                if (book != null)
                    result.Add(book);
            }

            return result;
        }

        /// <summary>
        /// Get or Add New author
        /// </summary>
        public async Task<Author> GetOrAddNewAuthorAsync(string name)
        {
            Author author = await _authorRepository.GetAsync(name);
                        
            if (author == null)
            {
                // New author, need to add to author repos
                var newAuthor = new Author(0, name);
                await _authorRepository.AddAsync(newAuthor);
                author = await _authorRepository.GetAsync(name);
            }
            return author;
        }

        /// <summary>
        /// Get or Add New Category
        /// </summary>
        public async Task<Category> GetOrAddNewCategoryAsync(string name)
        {
            Category cat = await _categoryRepository.GetAsync(name);

            if (cat == null)
            {
                // New Category, need to add to repos
                var newAuthor = new Category(0, name);
                await _categoryRepository.AddAsync(newAuthor);
                cat = await _categoryRepository.GetAsync(name);
            }
            return cat;
        }
    }
}
