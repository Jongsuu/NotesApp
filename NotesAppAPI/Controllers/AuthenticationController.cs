using NotesAppAPI.Models.Response;
using NotesAppAPI.Services.Authentication;

namespace NotesAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService authenticationManager;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            System.Console.WriteLine("AuthenticationController constructor");
            this.authenticationManager = authenticationService;
        }

        [HttpPost("~/login/{username}")]
        public async Task<ActionResult<dtoResponse<string>>> Login(string username, string password)
        {
            return Ok(await authenticationManager.Login(username, password));
        }

        [HttpPost("~/register")]
        public async Task<ActionResult<dtoResponse<string>>> Register(string username, string password)
        {
            return Ok(await authenticationManager.Register(username, password));
        }
    }
}
