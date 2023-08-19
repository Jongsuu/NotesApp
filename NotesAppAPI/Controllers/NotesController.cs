using Microsoft.AspNetCore.Authorization;
using NotesAppAPI.Models.Response;
using NotesAppAPI.Models;
using NotesAppAPI.Services;
using NotesAppAPI.Common;

namespace NotesAppAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class NotesController : ControllerBase
    {
        private INotesService notesManager;
        private string connectionString;

        public NotesController(IConfiguration configuration, INotesService notesService)
        {
            connectionString = configuration.GetConnectionString("NotesAppConnectionString")!;
            this.notesManager = notesService;
        }

        [HttpGet("~/notes")]
        public async Task<ActionResult<dtoResponse<List<dtoNote>>>> GetNotes()
        {
            return Ok(await notesManager.GetNotes(Utils.GetUserId(connectionString, HttpContext)));
        }

        [HttpGet("~/notes/{noteId}")]
        public async Task<ActionResult<dtoResponse<dtoNote>>> GetNoteById(int noteId)
        {
            return Ok(await notesManager.GetNoteById(noteId, Utils.GetUserId(connectionString, HttpContext)));
        }

        [HttpPost("~/notes")]
        public async Task<ActionResult<dtoResponse<bool>>> AddNote(string description)
        {
            return Ok(await notesManager.AddNote(new dtoAddNote { description = description }, Utils.GetUserId(connectionString, HttpContext)));
        }

        [HttpPut("~/notes/{noteId}")]
        public async Task<ActionResult<dtoResponse<bool>>> UpdateNote(int noteId, string description)
        {
            return Ok(await notesManager.UpdateNote(new dtoUpdateNote { description = description, id = noteId }, Utils.GetUserId(connectionString, HttpContext)));
        }

        [HttpDelete("~/notes/{noteId}")]
        public async Task<ActionResult<dtoResponse<bool>>> DeleteNote(int noteId)
        {
            return Ok(await notesManager.DeleteNote(noteId, Utils.GetUserId(connectionString, HttpContext)));
        }

        [HttpPut("~/notes/{noteId}/markAsRead")]
        public async Task<ActionResult<dtoResponse<bool>>> MarkNoteAsRead(int noteId)
        {
            return Ok(await notesManager.MarkAsRead(noteId, Utils.GetUserId(connectionString, HttpContext)));
        }
    }
}
