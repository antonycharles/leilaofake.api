using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.Responses
{
    public class ErrorResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public string[] details { get; set; }
        public ErrorResponse innerError { get; set; }

        public static ErrorResponse From(Exception e)
        {
            if(e == null) { return null; }

            return new ErrorResponse
            {
                code = e.HResult,
                message = e.Message,
                innerError = ErrorResponse.From(e.InnerException)
            };
        }

        public static ErrorResponse FromModelState(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(x => x.Errors);
            return new ErrorResponse
            {
                code = 100,
                message = "Requisição inválida!",
                details = errors.Select(s => s.ErrorMessage).ToArray()
            };
        }

        public static ErrorResponse FromMessage(string message)
        {
            return new ErrorResponse{
                code = 100,
                message = message
            };
        }
    }
}
