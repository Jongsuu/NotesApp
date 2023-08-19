
using NotesAppAPI.Models;
using NotesAppAPI.Models.Response;

namespace NotesAppAPI.Services
{
    public interface INotesService
    {
        public Task<dtoResponse<List<dtoNote>>> GetNotes();
        public Task<dtoResponse<dtoNote>> GetNoteById(int id);
        public Task<dtoResponse<bool>> AddNote(dtoAddNote newNote);
        public Task<dtoResponse<bool>> UpdateNote(dtoUpdateNote modifiedNote);
        public Task<dtoResponse<bool>> DeleteNote(int id);
        public Task<dtoResponse<bool>> MarkAsRead(int id);
    }
}
