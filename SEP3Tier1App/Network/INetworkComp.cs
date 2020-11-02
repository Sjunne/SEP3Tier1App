﻿using System.IO;
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
    }
}