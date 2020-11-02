using System.Text.Json.Serialization;

namespace WebApplication.Data
{
    public class ProfileData : IProfile
    {
        public string intro { get; set; }
        public string username { get; set; }

        [JsonIgnore] public bool isEditing { get; set; }
    }
}