namespace KiwiLib.Dto
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public Author(int id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public override string ToString() => $"Author: <{FullName}> Id: {Id}>";
    }

}
