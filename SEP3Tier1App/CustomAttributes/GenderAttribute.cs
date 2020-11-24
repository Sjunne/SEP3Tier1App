using System.ComponentModel.DataAnnotations;

namespace SEP3Tier1App.CustomAttributes
{
    public class GenderAttribute : ValidationAttribute
    {
        public GenderAttribute()
        {
            ErrorMessage = " field Must be a valid gender Either Male or Female, Enter N if Nonbinary";
        }
        
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var str = (string)value;
            if (str.Equals("Male")|| str.Equals("Female") || str.Equals("N"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("The " + validationContext.MemberName + ErrorMessage,new []{validationContext.MemberName});
            
        }
    }
}