using NotesAppAPI.Models;
using NotesAppAPI.Services;

namespace NotesAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NotesController : ControllerBase
    {
        private INotesService notesManager;

        public NotesController(INotesService notesService)
        {
            System.Console.WriteLine("NotesController constructor");
            this.notesManager = notesService;
        }

        [HttpGet("~/notes")]
        public async Task<ActionResult<List<dtoNote>>> GetNotes()
        {
            return Ok(await notesManager.GetNotes());
        }

        [HttpGet("~/notes/{noteId}")]
        public async Task<ActionResult<dtoNote>> GetNoteById(int noteId)
        {
            return Ok(await notesManager.GetNoteById(noteId));
        }

        [HttpPost("~/notes")]
        public async Task<ActionResult<bool>> AddNote(string description)
        {
            return Ok(await notesManager.AddNote(new dtoAddNote { description = description }));
        }

        [HttpPut("~/notes/{noteId}")]
        public async Task<ActionResult<bool>> UpdateNote(int noteId, string description)
        {
            return Ok(await notesManager.UpdateNote(new dtoUpdateNote { description = description, id = noteId }));
        }

        [HttpDelete("~/notes/{noteId}")]
        public async Task<ActionResult<bool>> DeleteNote(int noteId)
        {
            return Ok(await notesManager.DeleteNote(noteId));
        }

        [HttpPut("~/notes/{noteId}/markAsRead")]
        public async Task<ActionResult<bool>> MarkNoteAsRead(int noteId)
        {
            return Ok(await notesManager.MarkAsRead(noteId));
        }
    }
}
