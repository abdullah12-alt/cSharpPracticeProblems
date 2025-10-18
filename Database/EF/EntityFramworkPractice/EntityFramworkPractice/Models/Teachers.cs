namespace EntityFramworkPractice.Models
{
    public class Teachers
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Student> Students { get; set; } = new();

    }
}
