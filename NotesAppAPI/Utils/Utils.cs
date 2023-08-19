using NotesAppAPI.Models.Response;

namespace NotesAppAPI.Common
{
    public static class Utils
    {
        public static void SetResponseError<T>(dtoResponse<T> response, dtoResponseMessageCodes messageCode)
        {
            response.success = false;
            response.message = messageCode;
        }

        public static void SetResponseDBError<T>(dtoResponse<T> response)
        {
            SetResponseError<T>(response, dtoResponseMessageCodes.DATABASE_OPERATION);
        }

        public static void SetResponseOperationNotPerformedError<T>(dtoResponse<T> response)
        {
            SetResponseError<T>(response, dtoResponseMessageCodes.OPERATION_NOT_PERFORMED);
        }
    }
}
