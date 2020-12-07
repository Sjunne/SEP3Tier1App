using System;
using System.Collections.Generic;

namespace WebApplication.Data
{
    public class Match
    {
        public IList<String> usernames { get; set; }


        public Match()
        {
            usernames = new List<string>();
        }

        public void MakeMatch(string username, string username2)
        {
            usernames.Add(username);
            usernames.Add(username2);
        }
        
        
        
    }
}