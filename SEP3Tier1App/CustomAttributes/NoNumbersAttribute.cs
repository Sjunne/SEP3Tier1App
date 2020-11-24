using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

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
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var str = value.ToString();
            
            if (str.Any(char.IsDigit))
            {
                
                return new ValidationResult(ErrorMessage+validationContext.MemberName,new []{validationContext.MemberName});
            }
            return ValidationResult.Success;
        }
        
    }
}