using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text.Json.Serialization;

namespace WebApplication.Data
{
    public class ProfileData : IProfile
    {
        public Details self;
        public Details preferences;

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
        public string firstName { get; set; }
        
        [Required]
        [StringLength(20)]
        public string lastName { get; set; }
        
        [Required]
        [StringLength(30)]
        public string city { get; set; }
        
        [Required]
        [StringLength(40)]
        public string education { get; set; }
        
        [StringLength(40)]
        public string hobbies { get; set; }

        [Range(18, 150, ErrorMessage = "Must be at least 18")]
        public int age { get; set; }
        public string instragram { get; set; }
        public string idealdate { get; set; }
        public string interests { get; set; }

        public string spotify { get; set; }

        [JsonIgnore] public bool isEditingAbout { get; set; }
        [JsonIgnore] public bool isEditingIntro { get; set; }

    }
    
}