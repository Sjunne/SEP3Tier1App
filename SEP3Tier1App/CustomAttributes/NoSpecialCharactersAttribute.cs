using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SEP3Tier1App.CustomAttributes
{
    public class NoSpecialCharactersAttribute : ValidationAttribute
    {
        public NoSpecialCharactersAttribute()
        {
            ErrorMessage = "Special characters not allowed in ";
        }
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var str = value.ToString();
            
            if (hasSpecialChar(str))
            {
                return new ValidationResult(ErrorMessage+validationContext.MemberName,new []{validationContext.MemberName});
            }
            return ValidationResult.Success;
        }
        private bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_æøåÆØÅäöÄÖôÔâÂëËêÊéÉèÈ";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        
        
        
    }
    }
