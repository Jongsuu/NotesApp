
using NotesAppAPI.Models;
using NotesAppAPI.Models.Response;

namespace NotesAppAPI.Services
{
    public interface INotesService
    {
        public Task<dtoResponse<List<dtoNote>>> GetNotes(int userId);
        public Task<dtoResponse<dtoNote>> GetNoteById(int id, int userId);
        public Task<dtoResponse<bool>> AddNote(dtoAddNote newNote, int userId);
        public Task<dtoResponse<bool>> UpdateNote(dtoUpdateNote modifiedNote, int userId);
        public Task<dtoResponse<bool>> DeleteNote(int id, int userId);
        public Task<dtoResponse<bool>> MarkAsRead(int id, int userId);
    }
}
