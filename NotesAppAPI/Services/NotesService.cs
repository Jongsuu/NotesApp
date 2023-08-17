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

        public bool AddNote(dtoAddNote newNote)
        {
            bool inserted = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("INSERT INTO notes(description, read) VALUES('{0}', false)", newNote.description);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                inserted = cmd.ExecuteNonQuery() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return inserted;
        }

        public bool DeleteNote(int id)
        {
            bool deleted = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("DELETE FROM notes WHERE id={0}", id);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                deleted = cmd.ExecuteNonQuery() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return deleted;
        }

        public dtoNote? GetNoteById(int id)
        {
            dtoNote? result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM notes WHERE id=" + id;

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
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

        public List<dtoNote> GetNotes()
        {
            List<dtoNote> notes = new List<dtoNote>();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM notes";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    notes.Add(new dtoNote
                    {
                        id = (int)reader["id"],
                        description = (string)reader["description"],
                        read = (bool)reader["read"]
                    });
                }

                cmd.Dispose();
                conn.Close();
            }

            return notes;
        }

        public bool UpdateNote(dtoUpdateNote modifiedNote)
        {
            bool updated = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("UPDATE notes SET read=false, description='{1}' WHERE id={0}", modifiedNote.id, modifiedNote.description);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                updated = cmd.ExecuteNonQuery() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return updated;
        }

        public bool MarkAsRead(int id)
        {
            bool read = false;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Format("UPDATE notes SET read=true WHERE id={0}", id);

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                read = cmd.ExecuteNonQuery() == 1;

                cmd.Dispose();
                conn.Close();
            }

            return read;
        }
    }
}
