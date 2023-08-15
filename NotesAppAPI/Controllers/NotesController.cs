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
    }
}
