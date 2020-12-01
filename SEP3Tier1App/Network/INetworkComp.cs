 using System.Collections.Generic;
 using System.IO;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Mvc;
 using SEP3Tier1App.Network;
 //using SEP3Tier1App.Network;
 using WebApplication.Data;

namespace WebApplication.Network
{
    public interface INetworkComp
    {
        public Task EditIntroduction(ProfileData profileData);
        Task<ProfileData> GetProfile(string username);
        Task<string> GetCoverPicture(string username);
        Task<IList<string>> GetPictures(string username);
        Task UploadPicture(string username, string dataUri);
        Task EditProfile(ProfileData profileData, RequestOperationEnum requestOperationEnum);
        Task<IList<Review>> GetReviews(string username);
        Task ChangeCoverPicture(string pictureName);
        Task<string> GetProfilePicture(string username);
        Task ChangeProfilePic(string picturename);

        Task CreateProfile(ProfileData profileData);

        Task CreatePreference(ProfileData profileData);
        Task DeletePhoto(string holder);
        Task<ProfileData> GetPreference(string username);
        Task EditPreference(ProfileData profileData);
        void bigEditProfile(ProfileData profileData);
        void deleteProfile(string username);
        Task<IList<string>> GetMatches(int userId);

        Task getConnections(string username);

        Delegating getDelegating();

        Task<Request> ValidateLogin(string argsUsername, string argsPassword); 
        Task<Request> RegisterUser(User user);
        Task<Request> ChangePassword(User user);
        Task<Request> ChangeUsername(User user, string profileDataUsername);
    }
}