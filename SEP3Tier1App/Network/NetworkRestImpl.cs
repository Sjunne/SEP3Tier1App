﻿﻿using System;
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
            //if(info.IsSuccessStatusCode)
        }

        public async Task<ProfileData> GetProfile(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Profile?username={username}");
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(message);
            return profileData;
        }

        public async Task<string> GetCoverPicture(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Image");
            return message;
        }

        public async Task<IList<string>> GetPictures(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Image/ALl?username={username}");
            string[] images = message.Split("å");
            return images;
        }

        public async Task UploadPicture(string username, string dataUri)
        {
            Request request = new Request()
            {
                Username = username,
                o = dataUri,
                requestOperation = RequestOperationEnum.UPLOADPICTURE
            };

            HttpContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");
            
            HttpResponseMessage httpResponseMessage = await client.
                PostAsync("https://localhost:5003/Image", content);
            Console.WriteLine(httpResponseMessage);
        }

        public async Task EditProfile(ProfileData profileData, RequestOperationEnum requestOperationEnum)
        {
            Request request = new Request()
            {
                Username = profileData.username,
                o = profileData,
                requestOperation = requestOperationEnum
            };
            string message = JsonSerializer.Serialize(request);
            HttpContent content = new StringContent(
                message,
                Encoding.UTF8,
                "application/json");
                
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Profile/All", content);
            Console.WriteLine(info + " here");
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