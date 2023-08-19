using NotesAppAPI.Models.Response;
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
        public async Task<ActionResult<dtoResponse<List<dtoNote>>>> GetNotes()
        {
            return Ok(await notesManager.GetNotes());
        }

        [HttpGet("~/notes/{noteId}")]
        public async Task<ActionResult<dtoResponse<dtoNote>>> GetNoteById(int noteId)
        {
            return Ok(await notesManager.GetNoteById(noteId));
        }

        [HttpPost("~/notes")]
        public async Task<ActionResult<dtoResponse<bool>>> AddNote(string description)
        {
            return Ok(await notesManager.AddNote(new dtoAddNote { description = description }));
        }

        [HttpPut("~/notes/{noteId}")]
        public async Task<ActionResult<dtoResponse<bool>>> UpdateNote(int noteId, string description)
        {
            return Ok(await notesManager.UpdateNote(new dtoUpdateNote { description = description, id = noteId }));
        }

        [HttpDelete("~/notes/{noteId}")]
        public async Task<ActionResult<dtoResponse<bool>>> DeleteNote(int noteId)
        {
            return Ok(await notesManager.DeleteNote(noteId));
        }

        [HttpPut("~/notes/{noteId}/markAsRead")]
        public async Task<ActionResult<dtoResponse<bool>>> MarkNoteAsRead(int noteId)
        {
            return Ok(await notesManager.MarkAsRead(noteId));
        }
    }
}
