﻿using System;
 using System.Collections.Generic;
 using System.Drawing;
 using System.IO;
 using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Mvc;
 using WebApplication.Data;

namespace WebApplication.Network
{
    public class NetworkRestImpl : INetworkComp
    {
        private HttpClient client;

        public NetworkRestImpl()
        {
            client = new HttpClient();
        }

        public async Task EditIntroduction(ProfileData profileData)
        {
            string message = JsonSerializer.Serialize(profileData);
            HttpContent content = new StringContent(message,Encoding.UTF8,"application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Profile", content);
            Console.WriteLine(info);
        }

        public async Task<ProfileData> GetProfile(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Profile?username={username}");
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(message);
            return profileData;
        }

        public async Task<string> GetFilePath(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Image");
            //string image = JsonSerializer.Deserialize<string>(message);

            return message;


        }

        public async Task<IList<string>> GetPictures(string username)
        {
            string message = await client.GetStringAsync("https://localhost:5003/Image/All");
            IList<string> images = JsonSerializer.Deserialize<List<string>>(message);
            return images;
        }

        /*
        public async Task<string> GetFilePath(string username)
        {
            Stream message = await client.GetStreamAsync($"https://localhost:5003/Image");
            byte[] b = new byte[16*1024];
            byte[] b2;
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = message.Read(b, 0, b.Length)) > 0)
                {
                    ms.Write(b, 0, read);
                }

                b2 = ms.ToArray();
            }

            var base64 = Convert.ToBase64String(b2);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
            return imgSrc;
            //ByteArrayToFile("wwwroot/test2.jpg", b2);
         
            Console.WriteLine(message);
        }
        */
        
        
        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
    }

    
    
}