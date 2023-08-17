using NotesAppAPI.Models;
using NotesAppAPI.Services;

namespace NotesAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private INotesService notesManager;

        public NotesController(INotesService notesService)
        {
            System.Console.WriteLine("NotesController constructor");
            this.notesManager = notesService;
        }

        [HttpGet("~/notes")]
        public ActionResult<List<dtoNote>> GetNotes()
        {
            return Ok(notesManager.GetNotes());
        }

        [HttpGet("~/notes/{noteId}")]
        public ActionResult<dtoNote> GetNoteById(int noteId)
        {
            return Ok(notesManager.GetNoteById(noteId));
        }

        [HttpPost("~/notes")]
        public ActionResult<bool> AddNote(string description)
        {
            return Ok(notesManager.AddNote(new dtoAddNote { description = description }));
        }

        [HttpPut("~/notes/{noteId}")]
        public ActionResult<bool> UpdateNote(int noteId, string description)
        {
            return Ok(notesManager.UpdateNote(new dtoUpdateNote { description = description, id = noteId }));
        }

        [HttpDelete("~/notes/{noteId}")]
        public ActionResult<bool> DeleteNote(int noteId)
        {
            return Ok(notesManager.DeleteNote(noteId));
        }

        [HttpPut("~/notes/{noteId}/markAsRead")]
        public ActionResult<bool> MarkNoteAsRead(int noteId)
        {
            return Ok(notesManager.MarkAsRead(noteId));
        }
    }
}
