using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeilaoFake.Me.Api.Responses
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string[] Details { get; set; }
        public ErrorResponse InnerError { get; set; }

        public static ErrorResponse From(Exception e)
        {
            if(e == null) { return null; }

            return new ErrorResponse
            {
                Code = e.HResult,
                Message = e.Message,
                InnerError = ErrorResponse.From(e.InnerException)
            };
        }

        public static ErrorResponse FromModelState(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(x => x.Errors);
            return new ErrorResponse
            {
                Code = 100,
                Message = "Requisição inválida!",
                Details = errors.Select(s => s.ErrorMessage).ToArray()
            };
        }
    }
}
