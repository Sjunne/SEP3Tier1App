using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SEP3Tier1App.Authentication;
//using SEP3Tier1App.Network;
using WebApplication.Data;
using WebApplication.Network;

namespace SEP3Tier1App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            // Nuget.org components
            services.AddServerSideBlazor();
            services.AddBlazoredModal();
            

            // Scoped
            services.AddScoped<ProfileData>();
            services.AddScoped<Details>();
            services.AddScoped<CustomAuthenticationStateProvider>();
            
            //Network
            //Singleton
            services.AddSingleton<INetworkComp, NetworkImpl>();
            
            services.AddAuthorization(options => {
                options.AddPolicy("Vissing",  a => 
                    a.RequireAuthenticatedUser().RequireClaim("City", "Vissing"));
        
                options.AddPolicy("SecurityLevel4",  a => 
                    a.RequireAuthenticatedUser().RequireClaim("Level", "4","5"));
        
                options.AddPolicy("MustBeTeacher",  a => 
                    a.RequireAuthenticatedUser().RequireClaim("Role", "Teacher"));
                
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}