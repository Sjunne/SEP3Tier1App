﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication.Data
{
    public class User
    {
        [Required] public string username { get; set; }
        [Required] public string password { get; set; }
        public string pwdagain { get; set; }
        public string email { get; set; }
        public bool above18 { get; set; }

        public override string ToString()
        {
            return username + " " + password;
        }
    }
}