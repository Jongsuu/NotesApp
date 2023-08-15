
using NotesAppAPI.Models;

namespace NotesAppAPI.Services
{
    public interface INotesService
    {
        public List<dtoNote> GetNotes();
        public dtoNote GetNoteById(int id);
        public int AddNote(dtoAddNote newNote);
        public int UpdateNote(dtoUpdateNote modifiedNote);
        public bool DeleteNote(int id);
    }
}
