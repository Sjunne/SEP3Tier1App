using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SEP3Tier1App.CustomAttributes
{
    public class NoNumbersAttribute : ValidationAttribute
    {
        public NoNumbersAttribute()
        {
            ErrorMessage = "Numbers not allowed in ";
        }
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var str = (string)value;
            if (str.Any(char.IsDigit))
            {
                return new ValidationResult(ErrorMessage+validationContext.MemberName,new []{validationContext.MemberName});
            }
            return ValidationResult.Success;
        }
        
    }
}