using System;
 using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
 using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.VisualBasic;
using Radzen;
using SEP3Tier1App.Network;
using SEP3Tier1App.Pages;
//using SEP3Tier1App.Network;
using SEP3Tier1App.Util;
using WebApplication.Data;
using Connections = WebApplication.Data.Connections;


namespace WebApplication.Network
{
    public class NetworkImpl : INetworkComp
    {
        private HttpClient client;
        private NetworkStream _networkStream;
        private string Username = "Maria";
        public Delegating _delegating { get; set; }


        public NetworkImpl()
        {
            /*
            _delegating = new Delegating();
            _networkStream = NetworkStream();
            SendUsername(Username);
            Thread thread = new Thread(() => ListenToServer());
            thread.Start();
            */
            client = new HttpClient();
        }

        private void ListenToServer()
        {
            while (true)
            {
                byte[] dataFromClient = new byte[1024];
                _networkStream.Read(dataFromClient, 0, dataFromClient.Length);
                var trimEmptyBytes = TrimEmptyBytes(dataFromClient);
                string s = Encoding.ASCII.GetString(trimEmptyBytes, 0, trimEmptyBytes.Length);
                Request request = JsonSerializer.Deserialize<Request>(s);
                switch (request.requestOperation)
                {
                    case RequestOperationEnum.GETCONNECTIONS:
                    {   
                        _delegating.fromNetwork?.Invoke(request);
                        IList<string> Images = new Collection<string>();
                        int numberOfImages = JsonSerializer.Deserialize<IList<Connections>>(request.o.ToString()).Count;
                        for (int i = 0; i < numberOfImages; i++)
                        {
                            byte[] newArray = new byte[1024 * 1024];
                            _networkStream.Read(newArray, 0, newArray.Length);
                            var trimEmptyBytes2 = TrimEmptyBytes(newArray);
                            string s2 = Encoding.ASCII.GetString(trimEmptyBytes2, 0, trimEmptyBytes2.Length);
                            Images.Add(s2);
                        }
                        _delegating.ImagesFromNetwork?.Invoke(Images);
                        break;
                    }
                }
            }
        }

        private void SendUsername(string username)
        {
            var bytes = Encoding.ASCII.GetBytes(username);
            _networkStream.Write(bytes,0,bytes.Length);
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

        public async Task DeletePhoto(string pictureName)
        {
            HttpResponseMessage info = await client.DeleteAsync($"https://localhost:5003/Image/DeletePhoto/{pictureName}");
            if (info.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(info);

                throw new ErrorException(info.StatusCode + "");
            }
        }

        public async Task<ProfileData> GetPreference(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Profile/Preference?username={username}");
            if(httpResponseMessage.StatusCode != HttpStatusCode.OK)
                throw new ErrorException("Database connection lost");

            string message = await httpResponseMessage.Content.ReadAsStringAsync();
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(message);
            
            return profileData;
        }

        public async Task<Request> ValidateLogin(string argsUsername, string argsPassword)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Login?username={argsUsername}&&password={argsPassword}");
            
            if (httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                string readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
                Console.WriteLine(httpResponseMessage.RequestMessage.ToString());
                throw new ErrorException(httpResponseMessage.StatusCode + " " + readAsStringAsync);
            }
            
            string message = await httpResponseMessage.Content.ReadAsStringAsync();
            
            Request request = JsonSerializer.Deserialize<Request>(message);
            
            return request;
        }

        public async Task<Request> RegisterUser(User user)
        {
            Request request = new Request()
            {
                Username = user.username,
                o = user,
                requestOperation = RequestOperationEnum.REGISTERUSER
            };

            HttpContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");
            
            HttpResponseMessage httpResponseMessage = await client.
                PostAsync("https://localhost:5003/Login", content);
            string readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            Request response = JsonSerializer.Deserialize<Request>(readAsStringAsync);
            if (httpResponseMessage.StatusCode != HttpStatusCode.Created)
            {
                Console.WriteLine(httpResponseMessage);
                throw new ErrorException(httpResponseMessage.StatusCode + response.o.ToString());
            }

            return response;
        }

        public async Task<Request> ChangePassword(User user)
        {
            Request request = new Request()
            {
                o = user,
                Username = user.username,
                requestOperation = RequestOperationEnum.CHANGEPASSWORD
            };
            string serialize = JsonSerializer.Serialize(request);
            
            HttpContent content = new StringContent(serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage info = await client.PatchAsync("https://localhost:5003/Login", content);
            string readAsStringAsync = await info.Content.ReadAsStringAsync();
            if (info.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ErrorException(info.StatusCode + " " + readAsStringAsync);
            }

            Request response = JsonSerializer.Deserialize<Request>(readAsStringAsync);
            return response;
        }

        public async Task EditPreference(ProfileData profileData)
        {
            profileData.jsonPref = JsonSerializer.Serialize(profileData.preferences);
            string message = JsonSerializer.Serialize(profileData);
            
            HttpContent content = new StringContent(message, Encoding.UTF8, "application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Profile/EditPreference", content);
        }

        public async void bigEditProfile(ProfileData profileData)
        {
            profileData.jsonSelf = JsonSerializer.Serialize(profileData.self);
            string message = JsonSerializer.Serialize(profileData);
            Console.WriteLine(message);
            HttpContent content = new StringContent(message, Encoding.UTF8, "application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Profile/bigEditProfile", content);
        }

        public async void deleteProfile(string username)
        {
            client.DeleteAsync("https://localhost:5003/Profile/delete?username="+ username);
        }

        public async Task<ProfileData> GetProfile(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Profile?username={username}");
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(httpResponseMessage);
                throw new ErrorException("Database connection lost");
            }

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
            {
                Console.WriteLine(httpResponseMessage);
                throw new ErrorException(httpResponseMessage.StatusCode + "");
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<IList<string>> GetPictures(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Image/All?username={username}");
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(httpResponseMessage);
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
                Console.WriteLine(httpResponseMessage);
                throw new ErrorException(httpResponseMessage.StatusCode + "");
            }
        }

        public async Task EditProfile(ProfileData profileData, RequestOperationEnum requestOperationEnum)
        {
            profileData.jsonPref = null;
            profileData.jsonSelf = null;
            
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
                Console.WriteLine(info);
                throw new ErrorException(info.StatusCode + "");
            }
        }
        
        public async Task<IList<String>> GetMatches(int userId)
        {
            string profile = await client.GetStringAsync($"https://localhost:5003/Match?user1={userId}");
            Console.WriteLine(profile.ToString());
            IList<string> profiles = JsonSerializer.Deserialize<IList<string>>(profile);
            return profiles;
        }

        public async Task getConnections(string username)
        {
            string request = JsonSerializer.Serialize(new Request()
            {
                Username = username,
                requestOperation = RequestOperationEnum.GETCONNECTIONS
            });

            byte[] bytes = Encoding.ASCII.GetBytes(request);
            _networkStream.Write(bytes, 0, bytes.Length);
        }

        public Delegating getDelegating()
        {
            return _delegating;
        }


        public async Task<ProfileData> GetProfile(int userId)
        {
            string message = await client.GetStringAsync($"https://localhost:5003/Profile?userid={userId}");
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(message);
            return profileData;
        }

        
        public async Task ChangeCoverPicture(string pictureName)
        {
            string message = JsonSerializer.Serialize(pictureName);

            HttpContent content = new StringContent(message,Encoding.UTF8,"application/json");
            HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Image/UpdateCover", content);
            if (info.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(info);
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
                Console.WriteLine(httpResponseMessage);
                throw new ErrorException(httpResponseMessage.StatusCode + "");
            }
            string readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            return readAsStringAsync;
        }

        public async Task ChangeProfilePic(string picturename)
        {
            
                string message = JsonSerializer.Serialize(picturename);

                HttpContent content = new StringContent(message,Encoding.UTF8,"application/json");
                HttpResponseMessage info = await client.PostAsync("https://localhost:5003/Image/UpdateProfilePic", content);
                if (info.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(info);
                    throw new ErrorException("Database connection lost");
                }

        }
        
        public async Task<IList<Review>> GetReviews(string username)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"https://localhost:5003/Profile/Reviews?username={username}");
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(httpResponseMessage);
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
        
        
        
        
        
        
    
        private static NetworkStream NetworkStream()
        {
            NetworkStream stream = null;

            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 4500);
                stream = tcpClient.GetStream();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return stream;
        }
        
        
        private byte[] TrimEmptyBytes(byte[] array)
        {
            int i = array.Length - 1;
            while (array[i] == 0)
            {
                --i;
            }

            byte[] bar = new byte[i + 1];
            Array.Copy(array, bar, i+1);
            return bar;
        }
        
    }

}