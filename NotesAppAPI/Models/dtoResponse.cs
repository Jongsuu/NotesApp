using System.Text.Json.Serialization;

namespace NotesAppAPI.Models.Response
{
    public class dtoResponse<T>
    {
        public dtoResponseMessageCodes? message { get; set; }
        public bool success { get; set; } = true;
        public T? data { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum dtoResponseMessageCodes
    {
        DATABASE_OPERATION,
        USER_EXISTS,
        WRONG_PASSWORD,
        NOT_EXISTS,
        OPERATION_NOT_PERFORMED
    }
}
