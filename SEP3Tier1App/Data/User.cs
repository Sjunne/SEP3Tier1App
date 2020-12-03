﻿using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace WebApplication.Data
{
    public class User
    {
        [Required] public string username { get; set; }
        [Required] public string password { get; set; }
        public string email { get; set; }
        public string newPassword { get; set; }

        public string City { get; set; }
        
        public string Role { get; set; }


        public override string ToString()
        {
            return username + " " + password;
        }
    }
}