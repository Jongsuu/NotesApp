using Npgsql;
using NotesAppAPI.Models;

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

        public async Task<List<dtoNote>> GetNotes()
        {
            List<dtoNote> notes = new List<dtoNote>();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM notes ORDER BY 5 DESC, 4 DESC";

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

            return notes;
        }


        public async Task<bool> AddNote(dtoAddNote newNote)
        {
            bool inserted = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("INSERT INTO notes(description, read, created_at, last_updated) VALUES('{0}', DEFAULT, DEFAULT, DEFAULT)", newNote.description);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                inserted = await cmd.ExecuteNonQueryAsync() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return inserted;
        }

        public async Task<bool> DeleteNote(int id)
        {
            bool deleted = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("DELETE FROM notes WHERE id={0}", id);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                deleted = await cmd.ExecuteNonQueryAsync() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return deleted;
        }

        public async Task<dtoNote?> GetNoteById(int id)
        {
            dtoNote? result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM notes WHERE id=" + id;

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result = new dtoNote
                    {
                        id = (int)reader["id"],
                        description = (string)reader["description"],
                        read = (bool)reader["read"]
                    };
                }

                cmd.Dispose();
                conn.Close();
            }

            return result;
        }

        public async Task<bool> UpdateNote(dtoUpdateNote modifiedNote)
        {
            bool updated = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("UPDATE notes SET read=false, last_updated=DEFAULT, description='{1}' WHERE id={0}", modifiedNote.id, modifiedNote.description);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                updated = await cmd.ExecuteNonQueryAsync() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return updated;
        }

        public async Task<bool> MarkAsRead(int id)
        {
            bool read = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("UPDATE notes SET read=true WHERE id={0}", id);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                read = await cmd.ExecuteNonQueryAsync() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return read;
        }
    }
}
