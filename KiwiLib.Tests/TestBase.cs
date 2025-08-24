using KiwiLib.DAL;
using KiwiLib.Services;

namespace KiwiLib.Tests
{
    [TestClass]
    public class TestBase
    {
        protected BookServices bookServices;
        protected BookRepository bookRepository;

        [TestInitialize]
        public async Task TestInit()
        {
            bookServices = new BookServices();
            bookRepository = new BookRepository();
            await bookServices.GenerateDemoDataAsync();
        }
    }
}
