﻿using System;
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
            Console.WriteLine("Test");
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
            //Henter en STREAM fra vores getter (file)
            Stream message = await client.GetStreamAsync($"https://localhost:5003/Image");
            //Laver et stort bye array
            byte[] b = new byte[16*1024];
            //Laver et uden declartion, for at sætte "b" ind i. (Så vi slipper for tomme pladser, hvis billedet er mindre end 16 bits
            byte[] b2;
            
            using (MemoryStream ms = new MemoryStream())
            {
                //holder styr på hvor meget vi har læst
                int read;
                //Begynder at læse vores "STREAM"(message)
                while ((read = message.Read(b, 0, b.Length)) > 0)
                {
                    //gemmer bytes i memory stream, ved at bruge b???
                    ms.Write(b, 0, read);
                }
                //ligger ind i b2 for at få et præcist "størrelse" array
                b2 = ms.ToArray();
            }

            var base64 = Convert.ToBase64String(b2);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
            return imgSrc;
            
            
            //Denne virker V hvis skal gemme på tier 1 for caching.. Skal huske at sige .jpg og wwwroot folder er den eneste man kan gemme billeder i
            //**
            //ByteArrayToFile("wwwroot/test2.jpg", b2);
         
            Console.WriteLine(message);
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