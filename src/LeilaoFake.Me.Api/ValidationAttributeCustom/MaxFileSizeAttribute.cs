using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LeilaoFake.Me.Api.ValidationAttributeCustom
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize || file.Length <= 0)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"O tamanho máximo de arquivo permitido é {_maxFileSize} bytes.";
        }
    }
}