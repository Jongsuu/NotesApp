namespace NotesAppAPI.Models
{
    public class dtoUser
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; } = new byte[0];
        public byte[] passwordSalt { get; set; } = new byte[0];
    }
}
