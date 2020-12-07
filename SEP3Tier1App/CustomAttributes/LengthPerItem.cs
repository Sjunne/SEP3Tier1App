using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SEP3Tier1App.CustomAttributes
{
    public class LengthPerItem : ValidationAttribute
    {
        private int checker = 0;
        public LengthPerItem(int length)
        {
            ErrorMessage = "Must be under " + length + " characters per item (before each comma) in ";
            checker = length;
        }
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var str = value.ToString();
            
            if (checkLength(str, checker))
            {
                return new ValidationResult(ErrorMessage+validationContext.MemberName,new []{validationContext.MemberName});
            }
            return ValidationResult.Success;
        }

        private bool checkLength(string stringToBeChecked ,int length)
        {
            string[] split = stringToBeChecked.Split(",");
            foreach (var item in split)
            {
                if (item.Length > length)
                {
                    return true;
                }
            }

            return false;
        }
        
    }
}