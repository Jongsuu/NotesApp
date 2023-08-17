
using NotesAppAPI.Models;

namespace NotesAppAPI.Services
{
    public interface INotesService
    {
        public Task<List<dtoNote>> GetNotes();
        public Task<dtoNote?> GetNoteById(int id);
        public Task<bool> AddNote(dtoAddNote newNote);
        public Task<bool> UpdateNote(dtoUpdateNote modifiedNote);
        public Task<bool> DeleteNote(int id);
        public Task<bool> MarkAsRead(int id);
    }
}
