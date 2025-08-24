namespace KiwiLib.Dto
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Category(int id, string name, string? description = null)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => $"Category:<{Name}> Id: <{Id}>";
    }
}
