using Npgsql;
using NotesAppAPI.Models;
using NotesAppAPI.Models.Response;
using NotesAppAPI.Common;

namespace NotesAppAPI.Services
{
    public class NotesManager : INotesService
    {
        private IConfiguration _configuration;
        private string connectionString;

        public NotesManager(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("NotesAppConnectionString")!;
        }

        public async Task<dtoResponse<List<dtoNote>>> GetNotes(int userId)
        {
            System.Console.WriteLine(userId);
            dtoResponse<List<dtoNote>> response = new dtoResponse<List<dtoNote>>();
            List<dtoNote> notes = new List<dtoNote>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format("SELECT id, description, read, created_at, last_updated FROM notes WHERE user_id = {0} ORDER BY 3 DESC, 5 DESC, 4 DESC", userId);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        notes.Add(new dtoNote
                        {
                            id = (int)reader["id"],
                            description = (string)reader["description"],
                            read = (bool)reader["read"],
                            createdAt = (DateTime)reader["created_at"],
                            lastUpdated = (DateTime)reader["last_updated"]
                        });
                    }

                    cmd.Dispose();
                    conn.Close();
                }

                response.data = notes;
            }
            catch (Exception)
            {
                Utils.SetResponseDBError<List<dtoNote>>(response);
            }

            return response;
        }

        public async Task<dtoResponse<bool>> AddNote(dtoAddNote newNote, int userId)
        {
            dtoResponse<bool> response = new dtoResponse<bool>();
            bool inserted = false;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format(
                            @"INSERT INTO notes(description, read, created_at, last_updated, user_id)
                            VALUES('{0}', DEFAULT, DEFAULT, DEFAULT, {1})", newNote.description, userId);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    inserted = await cmd.ExecuteNonQueryAsync() == 1;

                    cmd.Dispose();
                    conn.Close();
                }

                response.data = inserted;

                if (!inserted)
                    Utils.SetResponseOperationNotPerformedError<bool>(response);
            }
            catch (Exception)
            {
                Utils.SetResponseDBError<bool>(response);
            }

            return response;
        }

        public async Task<dtoResponse<bool>> DeleteNote(int id, int userId)
        {
            dtoResponse<bool> response = new dtoResponse<bool>();
            bool deleted = false;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format("DELETE FROM notes WHERE id = {0} AND user_id = {1}", id, userId);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    deleted = await cmd.ExecuteNonQueryAsync() == 1;

                    cmd.Dispose();
                    conn.Close();
                }

                response.data = deleted;

                if (!deleted)
                    Utils.SetResponseOperationNotPerformedError<bool>(response);
            }
            catch (Exception)
            {
                Utils.SetResponseDBError<bool>(response);
            }

            return response;
        }

        public async Task<dtoResponse<dtoNote>> GetNoteById(int id, int userId)
        {
            dtoResponse<dtoNote> response = new dtoResponse<dtoNote>();
            dtoNote? result = null;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format("SELECT id, description, read, created_at, last_updated FROM notes WHERE id = {0} AND user_id = {1}", id, userId);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        result = new dtoNote
                        {
                            id = (int)reader["id"],
                            description = (string)reader["description"],
                            read = (bool)reader["read"],
                            createdAt = (DateTime)reader["created_at"],
                            lastUpdated = (DateTime)reader["last_updated"]
                        };
                    }

                    cmd.Dispose();
                    conn.Close();
                }
                response.data = result;

                if (result == null)
                    Utils.SetResponseError<dtoNote>(response, dtoResponseMessageCodes.NOT_EXISTS);
            }
            catch (Exception)
            {
                Utils.SetResponseDBError<dtoNote>(response);
            }

            return response;
        }

        public async Task<dtoResponse<bool>> UpdateNote(dtoUpdateNote modifiedNote, int userId)
        {
            dtoResponse<bool> response = new dtoResponse<bool>();
            bool updated = false;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format(
                            @"UPDATE notes SET read=false, last_updated=DEFAULT, description='{0}'
                            WHERE id = {1} AND user_id = {2}", modifiedNote.description, modifiedNote.id, userId);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    updated = await cmd.ExecuteNonQueryAsync() == 1;

                    cmd.Dispose();
                    conn.Close();
                }

                response.data = updated;

                if (!updated)
                    Utils.SetResponseOperationNotPerformedError<bool>(response);
            }
            catch (Exception)
            {
                Utils.SetResponseDBError<bool>(response);
            }

            return response;
        }

        public async Task<dtoResponse<bool>> MarkAsRead(int id, int userId)
        {
            dtoResponse<bool> response = new dtoResponse<bool>();
            bool read = false;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = string.Format("UPDATE notes SET read = true WHERE id = {0} AND user_id = {1}", id, userId);

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    read = await cmd.ExecuteNonQueryAsync() == 1;

                    cmd.Dispose();
                    conn.Close();
                }

                response.data = read;

                if (!read)
                    Utils.SetResponseError<bool>(response, dtoResponseMessageCodes.OPERATION_NOT_PERFORMED);
            }
            catch (Exception)
            {
                Utils.SetResponseDBError<bool>(response);
            }

            return response;
        }
    }
}
