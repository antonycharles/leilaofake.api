using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LeilaoFake.Me.Api.ValidationAttributeCustom
{
    public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }
    
    protected override ValidationResult IsValid(
    object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult(GetErrorMessage());
            }
        }
        
        return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
        return string.Format("Tipo do arquivo não é permitido. Extensões permitidas: {0}!", string.Join(",",_extensions));
    }
}
}