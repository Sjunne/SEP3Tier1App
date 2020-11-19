﻿﻿using System.Collections.Generic;
 using System.IO;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Mvc;
 using WebApplication.Data;

namespace WebApplication.Network
{
    public interface INetworkComp
    {
        public Task EditIntroduction(ProfileData profileData);
        Task<ProfileData> GetProfile(string username);
        Task<string> GetFilePath(string username);
        Task<IList<string>> GetPictures(string username);
        Task UploadPicture(string username, string dataUri);
        Task EditProfile(ProfileData profileData, RequestOperationEnum requestOperationEnum);
        Task ChangeCoverPicture(string pictureName);
        Task<string> GetProfilePicture(string username);
        Task ChangeProfilePic(string picturename);
        Task<IList<string>> GetMatches(int userId);
        Task<ProfileData> GetProfile(int userId);
    }
}