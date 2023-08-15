using NotesAppAPI.Models;

namespace NotesAppAPI.Services
{
    public class NotesManager : INotesService
    {
        private IConfiguration _configuration;
        private string connectionString;

        public NotesManager(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("NotesAppConnectionString")!;
        }

        public int AddNote(dtoAddNote newNote)
        {
            throw new NotImplementedException();
        }

        public bool DeleteNote(int id)
        {
            throw new NotImplementedException();
        }

        public dtoNote GetNoteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<dtoNote> GetNotes()
        {
            throw new NotImplementedException();
        }

        public int UpdateNote(dtoUpdateNote modifiedNote)
        {
            throw new NotImplementedException();
        }
    }
}
