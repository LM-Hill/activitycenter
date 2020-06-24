using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ActivityCenter.Models
{
    public class StrongNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reg = new Regex(@"^[a-zA-z]*$");
            if(!reg.IsMatch((string)value))
                return new ValidationResult("Name must consist of non-numeric characters.");
            return ValidationResult.Success;
        }
    }
}