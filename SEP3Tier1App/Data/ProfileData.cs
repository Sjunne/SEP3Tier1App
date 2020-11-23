using System.Dynamic;
using System.Text.Json.Serialization;

namespace WebApplication.Data
{
    public class ProfileData : IProfile
    {
        public string intro { get; set; }
        public string username { get; set; }

        public int age { get; set; }
        public string instagram { get; set; }
        public string idealdate { get; set; }
        public string interests { get; set; }

        public string spotify { get; set; }

        [JsonIgnore] public bool isEditingAbout { get; set; }
        [JsonIgnore] public bool isEditingIntro { get; set; }

    }
}