using Npgsql;
using NotesAppAPI.Models.Response;
using System.Security.Cryptography;
using NotesAppAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using NotesAppAPI.Common;

namespace NotesAppAPI.Services.Authentication
{
    public class AuthenticationManager : IAuthenticationService
    {
        private IConfiguration _configuration;
        private string connectionString;

        public AuthenticationManager(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("NotesAppConnectionString")!;
        }

        public async Task<dtoResponse<string>> Login(string username, string password)
        {
            dtoResponse<string> response = new dtoResponse<string>();
            dtoUser? user = await GetUser(username);

            if (user is null)
                Utils.SetResponseError<string>(response, dtoResponseMessageCodes.NOT_EXISTS);
            else if (!VerifyPasswordHash(password, user.passwordHash, user.passwordSalt))
                Utils.SetResponseError<string>(response, dtoResponseMessageCodes.WRONG_PASSWORD);
            else
                response.data = CreateToken(user);

            return response;
        }

        public async Task<dtoResponse<string>> Register(string username, string password)
        {
            dtoResponse<string> response = new dtoResponse<string>();

            if (await UserExists(username))
            {
                Utils.SetResponseError<string>(response, dtoResponseMessageCodes.USER_EXISTS);
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            dtoUser user = new dtoUser
            {
                username = username,
                passwordHash = passwordHash,
                passwordSalt = passwordSalt
            };
            dtoResponse<int?> addUserResponse = await AddUser(user);

            if (!addUserResponse.success)
            {
                Utils.SetResponseError<string>(response, (dtoResponseMessageCodes)addUserResponse.message!);
                return response;
            }

            user.id = (int)addUserResponse.data!;
            response.data = CreateToken(user);

            return response;
        }

        private async Task<bool> UserExists(string username)
        {
            bool exists = true;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format("SELECT true FROM users WHERE LOWER(username) = LOWER('{0}');", username);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                    object? result = await cmd.ExecuteScalarAsync();
                    exists = result != null;

                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception)
            {
                exists = true;
            }

            return exists;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            bool correct;

            using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                correct = computedHash.SequenceEqual(passwordHash);
            }

            return correct;
        }

        private string CreateToken(dtoUser user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.username)
            };

            string? appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;

            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null!");

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<dtoUser?> GetUser(string username)
        {
            dtoUser? user = null;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format("SELECT * FROM users WHERE LOWER(username) = LOWER('{0}')", username);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        user = new dtoUser
                        {
                            id = (int)reader["id"],
                            username = (string)reader["username"],
                            passwordHash = (byte[])reader["password_hash"],
                            passwordSalt = (byte[])reader["password_salt"]
                        };
                    }

                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception)
            {
                user = null;
            }

            return user;
        }

        private async Task<dtoResponse<int?>> AddUser(dtoUser user)
        {
            dtoResponse<int?> response = new dtoResponse<int?>();
            int? userId = null;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT public.insertuser(@user_name, @passwordHash, @passwordSalt)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Varchar, 50).Value = user.username;
                    cmd.Parameters.Add("@passwordHash", NpgsqlTypes.NpgsqlDbType.Bytea).Value = user.passwordHash;
                    cmd.Parameters.Add("@passwordSalt", NpgsqlTypes.NpgsqlDbType.Bytea).Value = user.passwordSalt;

                    userId = (int?)await cmd.ExecuteScalarAsync();

                    cmd.Dispose();
                    conn.Close();
                }

                if (userId == null)
                    Utils.SetResponseOperationNotPerformedError<int?>(response);
                else
                    response.data = userId;
            }
            catch (Exception ex)
            {
                Utils.SetResponseDBError<int?>(response);
                System.Console.WriteLine(ex.Message);
            }

            return response;
        }
    }
}
