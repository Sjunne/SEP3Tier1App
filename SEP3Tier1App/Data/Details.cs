﻿using System.ComponentModel.DataAnnotations;
using SEP3Tier1App.CustomAttributes;

namespace WebApplication.Data
{
    public class Details : IDetails
    {
        [StringLength(20)]
        [NoNumbers]
        [Required]
        [NoSpecialCharacters]
        public string hairColor { get; set; }
        
        [StringLength(20)]
        [NoNumbers]
        [Required]
        [NoSpecialCharacters]
        public string skinColor { get; set; }
        
        [StringLength(20)]
        [NoNumbers]
        [Required]
        [NoSpecialCharacters]
        public string nationality { get; set; }
        
        [Range(18, 120, ErrorMessage = "must be at least 18 and not over 150")]
        public int minimumAge { get; set; }
        
        [Range(18, 120, ErrorMessage = "must be at least 18 and not over 150")]
        public int maximumAge { get; set; }
        
        [StringLength(20)]
        [NoNumbers]
        [Required]
        [NoSpecialCharacters]
        public string bodyShape { get; set; }
        
        [StringLength(300)]
        [NoNumbers]
        [NoSpecialCharacters]
        [LengthPerItem(20)]
        public string hobbies { get; set; }
        
        [StringLength(50)]
        [NoNumbers]
        [NoSpecialCharacters]
        public string education { get; set; }
        
        [StringLength(50)]
        [NoNumbers]
        [Required]
        [NoSpecialCharacters]
        public string job { get; set; }
        
        public bool kids { get; set; }
        
        public bool lookingFor { get; set; }
        
        [StringLength(20)]
        [NoNumbers]
        [Required]
        [NoSpecialCharacters]
        public string eyeColor { get; set; }
        
        [Gender]
        public string gender { get; set; }
        
        [StringLength(20)]
        [NoNumbers]
        [NoSpecialCharacters]
        public string city { get; set; }
        
    }
}