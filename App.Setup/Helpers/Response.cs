
using IqraBase.Setup.Models;

namespace App.Setup.Helpers
{
    public static class ApiResponse
    {
        public static ResponseJson Error()
        {
            return Factory(true, "Error");
        }

        public static ResponseJson Error(string message)
        {
            return Factory(true, message);
        }

        public static ResponseJson Error(object id, string message = "Error")
        {
            return Factory(true, message);
        }

        public static ResponseJson Success()
        {
            return Factory(false, "Successed");
        }

        public static ResponseJson Success(string message)
        {
            return Factory(false, message);
        }

        public static ResponseJson Success(object id, string message = "Successed")
        {
            return Factory(false, id: id, message: message);
        }

        public static ResponseJson Success(object id, object data, string message = "Successed")
        {
            return Factory(false, id: id, message: message, data: data);
        }

        public static ResponseJson Factory(bool isError = false,
                                                   string message = null,
                                                   object id = null,
                                                   object data = null)
        {
            return new ResponseJson()
            {
                 Id = id,
                 IsError = isError,
                 Msg = message,
                 Data = data
            };
        }
    }
}
