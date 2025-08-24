using KiwiLib.DAL;
using KiwiLib.Dto;
using KiwiLib.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KiwiLib.Tests
{
    [TestClass]
    public sealed class BookRepositoryTests : TestBase
    {
        [TestMethod]
        [Timeout(10000)]
        public async Task AddAsync_ShouldSucceed()
        {
            // Prepare
            var newBook = new Book(0, "9781234567897", "New Book for Unitest", new Category(10, "Programming"), new Author(10, "Vi Vo"), 2003);

            // Act
            var bookId = await bookRepository.AddAsync(newBook);

            //Assert
            Assert.IsTrue(bookId > 0);

            // Act
            var book = await bookRepository.GetAsync(bookId);

            // Assert
            Assert.IsNotNull(book);
            Assert.AreEqual(book.Title, "New Book for Unitest");
            Assert.AreEqual(book.Isbn, "9781234567897");
            Assert.AreEqual(book.Category.Name, "Programming");
            Assert.AreEqual(book.Author.FullName, "Vi Vo");
            Assert.AreEqual(book.YearPublished, 2003);
        }

        [TestMethod]
        public async Task AddAsync_DuplicatedBook_ShouldFailed()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldSuccee()
        {
            // To do: due to limitation of time, this test method is not implemented
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidDettail_ShouldFailed()
        {
            // To do: due to limitation of time, this test method is not implemented
        }
    }
}
