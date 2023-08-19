using NotesAppAPI.Models.Response;

namespace NotesAppAPI.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task<dtoResponse<string>> Register(string username, string password);
        public Task<dtoResponse<string>> Login(string username, string password);
        public Task<bool> UserExists(string username);
    }
}
