using System;
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
                
        }

        public async Task<ProfileData> GetProfile(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Profile?username={username}");
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(message);
            return profileData;
        }

        public async Task<string> GetFilePath(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Image?username=" + username);
            //string image = JsonSerializer.Deserialize<string>(message);

            return message;


        }

        public async Task<IList<string>> GetPictures(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Image/All?username={username}");
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
            
            HttpResponseMessage httpResponseMessage = await client.PostAsync("https://localhost:5003/Image", content);
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
        }

        public async Task ChangeCoverPicture(string pictureName)
        {
            string message = JsonSerializer.Serialize(pictureName);

            HttpContent content = new StringContent(message,Encoding.UTF8,"application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Image/UpdateCover", content);
        }

        public async Task<string> GetProfilePicture(string username)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Image/ProfilePic?username=" + username);
            //string image = JsonSerializer.Deserialize<string>(message);

            return message;
        }

        public async Task ChangeProfilePic(string picturename)
        {
            
                string message = JsonSerializer.Serialize(picturename);

                HttpContent content = new StringContent(message,Encoding.UTF8,"application/json");
                HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Image/UpdateProfilePic", content);
            
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