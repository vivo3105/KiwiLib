// See https://aka.ms/new-console-template for more information
using KiwiLib.Dto;
using KiwiLib.Services;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, Wellcome to KiwiLib");

        BookServices bookService = new BookServices();
        await bookService.GenerateDemoDataAsync();

        while (true)
        {
            Console.WriteLine("");
            Console.WriteLine("===========================");
            Console.WriteLine("Press 0 to Exit");
            Console.WriteLine("Press 1 to Add a new book");
            Console.WriteLine("Press 2 to Update an existing book");
            Console.WriteLine("Press 3 to Delete a book");
            Console.WriteLine("Press 4 to List All books");
            Console.WriteLine("Press 5 to View detail of a book");
            Console.WriteLine("Press 6 to Search books");
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    Console.WriteLine("Exiting KiwiLib. Goodbye!");
                    return;
                case "1":
                    await AddNewBook(bookService);
                    break;
                case "2":
                    await UpdateBook(bookService);
                    break;
                case "3":
                    await DeleteBook(bookService);
                    break;
                case "4":
                    await ListAllBooks(bookService);
                    break;
                case "5":
                    await ViewBookDetail(bookService);
                    break;
                case "6":
                    await SearchBooks(bookService);
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    static async Task ListAllBooks(BookServices bookService)
    {
        try
        {
            List<Book> allbooks = await bookService.GetAllBooksAsync();
            if (allbooks.Any())
            {
                Console.WriteLine($"There are {allbooks.Count()} books in the library, as below:");
                foreach (var book in allbooks)
                    Console.WriteLine(book);
            }
            else
            {
                Console.WriteLine("  There is no book in the library");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error listing books: " + ex.Message);
        }
    }

    static async Task ViewBookDetail(BookServices bookService)
    {
        try
        {
            Console.Write("Enter Book Id or ISBN: ");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("No input provided. Please try again");
                return;
            }

            List<Book> allbooks = await bookService.SearchBookAsync(input, true);
            if (allbooks.Any())
            {
                Console.WriteLine($"Book detail as below:");
                Console.WriteLine($"   Book Id\t\t: {allbooks.First().Id}");
                Console.WriteLine($"   ISBN\t\t\t: {allbooks.First().Isbn}");
                Console.WriteLine($"   Title\t\t: {allbooks.First().Title}");
                Console.WriteLine($"   Author\t\t: {allbooks.First().Author.FullName}");
                Console.WriteLine($"   Category\t\t: {allbooks.First().Category.Name}");
                Console.WriteLine($"   Year published\t: {allbooks.First().YearPublished}");
            }
            else
            {
                Console.WriteLine($" No book found with ID or ISBN: {input}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding new books: " + ex.Message);
        }
    }

    static async Task SearchBooks(BookServices bookService)
    {
        try
        {
            Console.WriteLine("You can search by: ID | ISBN | Title | Author | Category");
            Console.Write("Enter search input: ");

            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("No input provided. Please try again");
                return;
            }

            List<Book> books = await bookService.SearchBookAsync(input);
            if (books.Any())
            {
                Console.WriteLine($"There are {books.Count()} books in the library, as below:");
                foreach (var book in books)
                    Console.WriteLine(book);
            }
            else
            {
                Console.WriteLine($" No book found with ID or ISBN: {input}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error listing books: " + ex.Message);
        }
    }

    static async Task DeleteBook(BookServices bookService)
    {
        try
        {
            Console.Write("Enter Book Id to delete: ");

            string input = Console.ReadLine();
            int bookId = 0;
            if (!int.TryParse(input, out bookId))
            {
                Console.WriteLine("No input provided or not a valid Book Id. Please try again");
                return;
            }

            var book = await bookService.GetBooksAsync(bookId);
            if (book != null)
            {
                var result = await bookService.DeleteBookAsync(bookId);
                Console.WriteLine($"  Book deleted {(result.Success ? "successfully" : "failed")}. Message: {result.Message}");
            }
            else
            {
                Console.WriteLine($" No book found with ID: {input}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error deleting books: " + ex.Message);
        }
    }

    static async Task AddNewBook(BookServices bookService)
    {
        try
        {
            Book book = new Book();
            string input;

            Console.Write(" Enter book title: ");
            book.Title = Console.ReadLine();
            Console.Write(" Enter book Isbn: ");
            book.Isbn = Console.ReadLine();
            Console.Write(" Enter book author: ");
            var author = Console.ReadLine();
            book.Author = await bookService.GetOrAddNewAuthorAsync(author);
            Console.Write(" Enter book category: ");
            var cagetory = Console.ReadLine();
            book.Category = await bookService.GetOrAddNewCategoryAsync(cagetory);
            Console.Write(" Enter year published: ");
            var yearInput = Console.ReadLine();
            int.TryParse(yearInput, out var yearInt);
            book.YearPublished = yearInt;

            var result = await bookService.AddBookAsync(book);

            if (result.Success)
            {
                Console.WriteLine($" ->New book is added successfully, Book ID is: {result.BookId}");
            }
            else
            {
                Console.WriteLine($" -> Error while adding new book. {result.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding new books: " + ex.Message);
        }
    }

    static async Task UpdateBook(BookServices bookService)
    {
        try
        {
            string input;
            int bookId;
            Console.Write(" Enter book ID to update: ");
            input = Console.ReadLine();

            if (!int.TryParse(input, out bookId))
            {
                Console.WriteLine($"  Invalid input. Book ID must be a number. Update cancelled.");
                return;
            }

            var updatedBook = await bookService.GetBooksAsync(bookId);
            if (updatedBook == null)
            {
                Console.WriteLine($"  Not found book Id {bookId}. Update cancelled.");
                return;
            }

            Console.WriteLine($" This is current book detail:");
            Console.WriteLine($"   Book Id\t\t: {updatedBook.Id}");
            Console.WriteLine($"   ISBN\t\t\t: {updatedBook.Isbn}");
            Console.WriteLine($"   Title\t\t: {updatedBook.Title}");
            Console.WriteLine($"   Author\t\t: {updatedBook.Author.FullName}");
            Console.WriteLine($"   Category\t\t: {updatedBook.Category.Name}");
            Console.WriteLine($"   Year published\t: {updatedBook.YearPublished}");

            Console.Write(" Enter new book title (leave blank to keep current):");
            input = Console.ReadLine();
            updatedBook.Title = string.IsNullOrEmpty(input) ? updatedBook.Title : input;

            Console.Write(" Enter new book ISBN (leave blank to keep current):");
            input = Console.ReadLine();
            updatedBook.Isbn = string.IsNullOrEmpty(input) ? updatedBook.Isbn : input;

            Console.Write(" Enter new book author (leave blank to keep current):");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && !input.Equals(updatedBook.Author.FullName))
            {
                updatedBook.Author = await bookService.GetOrAddNewAuthorAsync(input);
            }

            Console.Write(" Enter new book cagetory (leave blank to keep current):");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && !input.Equals(updatedBook.Category.Name))
            {
                updatedBook.Category = await bookService.GetOrAddNewCategoryAsync(input);
            }

            Console.Write(" Enter year published (leave blank to keep current):");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input, out var yearInt))
                {
                    updatedBook.YearPublished = yearInt;
                }
                else
                {
                    Console.WriteLine("  Invalid input. Published year must be a number. Update cancelled.");
                    return;
                }
            }
                
            var result = await bookService.UpdateBookAsync(updatedBook);

            if (result.Success)
            {
                Console.WriteLine($" ->Book is updated successfully");
            }
            else
            {
                Console.WriteLine($" ->Error while updating book. {result.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding new books: " + ex.Message);
        }
    }


}