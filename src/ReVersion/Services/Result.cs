using System.Collections.Generic;
using System.Linq;

namespace ReVersion.Services
{
    public class Result
    {
        public bool Status { get; set; }

        public List<string> Messages { get; set; } = new List<string>();


        public static Result Success() => new Result { Status = true };
        public static Result Success(string message) => new Result { Status = true, Messages = { message }};
        public static Result Success(List<string> messages) => new Result { Status = true, Messages = messages };
        public static Result Success(params string[] messages) => new Result { Status = true, Messages = messages.ToList() };


        public static Result Error() => new Result { Status = false };
        public static Result Error(string message) => new Result { Status = false, Messages = { message } };
        public static Result Error(List<string> messages) => new Result { Status = false, Messages = messages };
        public static Result Error(params string[] messages) => new Result { Status = false, Messages = messages.ToList() };

    }
}
