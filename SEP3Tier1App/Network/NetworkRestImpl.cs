using System;
 using System.Collections.Generic;
 using System.Drawing;
 using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Radzen;
using SEP3Tier1App.Util;
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
            if (info.StatusCode != HttpStatusCode.Created)
            {
                throw new ErrorException(info.StatusCode + "");
            }
                
        }
        
        public async Task CreateProfile(ProfileData profileData)
        {
            profileData.jsonSelf = JsonSerializer.Serialize(profileData.self);
            string message = JsonSerializer.Serialize(profileData);
            Console.WriteLine(message);
            HttpContent content = new StringContent(message, Encoding.UTF8, "application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Profile/CreateProfile", content);
        }

        public async Task CreatePreference(ProfileData profileData)
        {
            profileData.jsonPref = JsonSerializer.Serialize(profileData.preferences);
            string message = JsonSerializer.Serialize(profileData);
            Console.WriteLine(message);
            HttpContent content = new StringContent(message, Encoding.UTF8, "application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Profile/CreatePreference", content);
        }

        public async Task<ProfileData> GetProfile(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Profile?username={username}");
            if(httpResponseMessage.StatusCode != HttpStatusCode.OK)
                throw new ErrorException("Database connection lost");

            string message = await httpResponseMessage.Content.ReadAsStringAsync();
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(message);
            
            return profileData;
        }

        public async Task<string> GetCoverPicture(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Image?username=" + username);
            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                return "";
            }
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                throw new ErrorException(httpResponseMessage.StatusCode + "");
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<IList<string>> GetPictures(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Image/All?username={username}");
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new ErrorException(httpResponseMessage.StatusCode+ "");
            }

            string message = await httpResponseMessage.Content.ReadAsStringAsync();
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
            if (httpResponseMessage.StatusCode != HttpStatusCode.Created)
            {
                throw new ErrorException(httpResponseMessage.StatusCode + "");

            }
        }

        public async Task EditProfile(ProfileData profileData, RequestOperationEnum requestOperationEnum)
        {
            Request request = new Request()
            {
                Username = profileData.username,
                o = JsonSerializer.Serialize(profileData),
                requestOperation = requestOperationEnum
            };
            string message = JsonSerializer.Serialize(request);
            HttpContent content = new StringContent(
                message,
                Encoding.UTF8,
                "application/json");
                
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Profile/All", content);
            if (info.StatusCode != HttpStatusCode.Created)
            {
                throw new ErrorException(info.StatusCode + "");
            }
        }

        public async Task ChangeCoverPicture(string pictureName)
        {
            string message = JsonSerializer.Serialize(pictureName);

            HttpContent content = new StringContent(message,Encoding.UTF8,"application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Image/UpdateCover", content);
            if (info.StatusCode != HttpStatusCode.OK)
            {
                throw new ErrorException(info.StatusCode + "");
            }
        }

        public async Task<string> GetProfilePicture(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Image/ProfilePic?username=" + username);
            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                return "";
            }
            else if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new ErrorException(httpResponseMessage.StatusCode + "");
            }

            return await httpResponseMessage.ReadAsync<string>();
        }

        public async Task ChangeProfilePic(string picturename)
        {
            
                string message = JsonSerializer.Serialize(picturename);

                HttpContent content = new StringContent(message,Encoding.UTF8,"application/json");
                HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Image/UpdateProfilePic", content);
                if (info.StatusCode != HttpStatusCode.OK)
                {
                    throw new ErrorException("Database connection lost");
                }

        }
        
        public async Task<IList<Review>> GetReviews(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Profile/Reviews?username={username}");
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new ErrorException(httpResponseMessage.StatusCode + "");
            }
            string message = await httpResponseMessage.Content.ReadAsStringAsync();
            List<Review> reviews = JsonSerializer.Deserialize<List<Review>>(message);
            return reviews;        
        }

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