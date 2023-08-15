namespace NotesAppAPI.Models
{
    public class dtoNote
    {
        public int id { get; set; }
        public string description { get; set; } = string.Empty;
        public bool read { get; set; }
    }

    public class dtoAddNote
    {
        public string description { get; set; } = string.Empty;
    }

    public class dtoUpdateNote
    {
        public int id { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
