using System.ComponentModel.DataAnnotations;
using SEP3Tier1App.CustomAttributes;

namespace WebApplication.Data
{
    public class Details : IDetails
    {
        [StringLength(40)]
        [NoNumbers]
        public string hairColor { get; set; }
        
        [StringLength(40)]
        [NoNumbers]
        public string skinColor { get; set; }
        
        [StringLength(40)]
        [NoNumbers]
        public string nationality { get; set; }
        
        [Range(18, 150, ErrorMessage = "must be at least 18.")]
        public int minimumAge { get; set; }
        
        [Range(18, 150, ErrorMessage = "must be at least 18.")]
        public int maximumAge { get; set; }
        
        [StringLength(40)]
        [NoNumbers]
        public string bodyShape { get; set; }
        
        [StringLength(40)]
        [NoNumbers]
        public string hobbies { get; set; }
        
        [StringLength(40)]
        [NoNumbers]
        public string education { get; set; }
        
        [StringLength(40)]
        [NoNumbers]
        public string job { get; set; }
        
        public bool kids { get; set; }
        
        public bool lookingFor { get; set; }
        
        [StringLength(40)]
        public string eyeColor { get; set; }
        
        [StringLength(40)]
        public string gender { get; set; }
        
        [StringLength(40)]
        public string city { get; set; }
        
    }
}