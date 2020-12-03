using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text.Json.Serialization;
using SEP3Tier1App.CustomAttributes;

namespace WebApplication.Data
{
    public class ProfileData : IProfile
    {
        [ValidateComplexType]
        public Details self { get; set; }
        public Details preferences { get; set; }

        public ProfileData()
        {
            
            self = new Details();
            preferences = new Details();
            
        }
        public string jsonPref { get; set; }
        public string jsonSelf { get; set; }
        public string intro { get; set; }
        public string username { get; set; }
        
        [Required]
        [StringLength(20)]
        [NoNumbers]
        public string firstName { get; set; }
        
        [Required]
        [StringLength(20)]
        [NoNumbers]
        public string lastName { get; set; }
        
        [Required]
        [StringLength(20)]
        [NoNumbers]
        public string city { get; set; }
        
        [Required]
        [StringLength(50)]
        [NoNumbers]
        public string education { get; set; }
        
        [StringLength(300)]
        public string hobbies { get; set; }

        public string instagram { get; set; }
        [Range(18, 150, ErrorMessage = "Must be at least 18 and not above 150")]
        public int age { get; set; }
        public string instragram { get; set; }
        public string idealdate { get; set; }
        public string interests { get; set; }

        public string spotify { get; set; }

        [JsonIgnore] public bool isEditingAbout { get; set; }
        [JsonIgnore] public bool isEditingIntro { get; set; }

    }
    
}