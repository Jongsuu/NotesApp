
using NotesAppAPI.Models;

namespace NotesAppAPI.Services
{
    public interface INotesService
    {
        public List<dtoNote> GetNotes();
        public dtoNote? GetNoteById(int id);
        public bool AddNote(dtoAddNote newNote);
        public bool UpdateNote(dtoUpdateNote modifiedNote);
        public bool DeleteNote(int id);
        public bool MarkAsRead(int id);
    }
}
