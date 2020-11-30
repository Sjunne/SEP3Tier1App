using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using WebApplication.Data;

namespace SEP3Tier1App.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;

        private User cachedUser;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (cachedUser == null)
            {
                string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    cachedUser = JsonSerializer.Deserialize<User>(userAsJson);

                    identity = SetupClaimsForUser(cachedUser);
                }
            }
            else
            {
                identity = SetupClaimsForUser(cachedUser);
            }

            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public void SetClaimsIdentity(string username, string password)
        {
            Console.WriteLine("Validating log in");
            //if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            //if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");

            ClaimsIdentity identity = new ClaimsIdentity();
            try
            {
                User user = new User()
                {
                    username =  username,
                    password =  password
                };
                identity = SetupClaimsForUser(user);
                string serialisedData = JsonSerializer.Serialize(user);
                jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
                cachedUser = user;
                Console.WriteLine("You can been logged in!" + user.ToString());
            }
            catch (Exception e)
            {
                //throw own exception
                Console.WriteLine(e.Message);
            }

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void Logout()
        {
            cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            Console.WriteLine(cachedUser);
        }

        private ClaimsIdentity SetupClaimsForUser(User user)
        {
            List<Claim> claims = new List<Claim>();
            /*claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("Role", user.Role));
            claims.Add(new Claim("City", user.City));
            claims.Add(new Claim("Domain", user.Domain));
            claims.Add(new Claim("BirthYear", user.BirthYear.ToString()));
            claims.Add(new Claim("Level", user.SecurityLevel.ToString()));
            */

            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }
    }
}