using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace KiwiLib.Dto
{
    public class Book
    {
        public int Id;
        public string Isbn;
        public string Title;
        public Category Category;
        public Author Author;
        public int YearPublished;

        public Book()
        { 
        }

        public Book(int id, string isbn, string title, Category cat, Author author, int yearPublished)
        {
            Id = id;
            Isbn = isbn;
            Title = title;
            Category = cat;
            Author = author;
            YearPublished = yearPublished;
        }

        public override string ToString() => $"  Id: {Id}, Isbn: {Isbn}, Title: {Title}, Author: {Author.FullName}, Category: {Category.Name}";
    }
}
