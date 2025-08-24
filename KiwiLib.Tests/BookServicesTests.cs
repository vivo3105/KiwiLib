using KiwiLib.DAL;
using KiwiLib.Dto;

namespace KiwiLib.Tests
{
    [TestClass]
    public sealed class BookServicesTests : TestBase
    {
        [TestMethod]
        [Timeout(10000)]
        public async Task AddNewBook_HappyPath_ShouldSucceed()
        {
            // Arrange
            var newBook = new Book(0,"9781234567897", "New Book for Unitest", new Category(10, "Programming"), new Author(10, "Vi Vo"), 2003);
            var  addResult = await bookServices.AddBookAsync(newBook);

            // Assert
            Assert.IsTrue(addResult.Success);
            Assert.AreEqual(addResult.Message, "Book added successfully.");
            Assert.IsTrue(addResult.BookId > 0); // new Id generated
            
            // Act
            var book = await bookServices.GetBooksAsync(addResult.BookId);

            // Assert
            Assert.IsNotNull(book);
            Assert.AreEqual(book.Isbn, "9781234567897");
            Assert.AreEqual(book.Title, "New Book for Unitest");
            Assert.AreEqual(book.Category.Name, "Programming");
            Assert.AreEqual(book.Author.FullName, "Vi Vo");
            Assert.AreEqual(book.YearPublished, 2003);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task AddNewBook_InvalidIsbn_ShouldFailed()
        {
            // Arrange
            var newBook = new Book(0, "123456789", "New Book for Unitest", new Category(10, "Programming"), new Author(10, "Vi Vo"), 2003);
            var addResult = await bookServices.AddBookAsync(newBook);

            // Assert
            Assert.IsFalse(addResult.Success);
            Assert.AreEqual(addResult.Message, "Invalid ISBN: ISBN must be exactly 13 numeric digits.");
            Assert.IsTrue(addResult.BookId == 0);

        }

        [TestMethod]
        [Timeout(10000)]
        public void AddNewBook_DuplicateId_ShouldFailed()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

        [TestMethod]
        [Timeout(10000)]
        public void AddNewBook_DuplicatedBookId_ShouldFailed()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

        [TestMethod]
        [Timeout(10000)]
        public void UpdateBooks_HappyPath_ShouldSucceed()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

        [TestMethod]
        [Timeout(10000)]
        public void UpdateBooks_NoChange_ShouldNotUpdated()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

        [TestMethod]
        [Timeout(10000)]
        public void UpdateBooks_InvalidISBN_ShouldNotUpdated()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

        [TestMethod]
        [Timeout(10000)]
        public void UpdateBooks_SpecialCharactersInTitle_ShouldNotUpdated()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

    }
}
