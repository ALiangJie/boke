﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolg.Data;
using Bolg.Data.FileManager;
using Bolg.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bolg
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_config["DefaultConnection"]));

            services.AddIdentity<IdentityUser, IdentityRole>(options => 
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
                //.AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/login";
            });

            services.AddTransient<IFileManager, FileManager>();

            services.AddTransient<IRepository, Repository>();

            services.AddMvc(options => 
            {
                options.CacheProfiles.Add("Monthly", new CacheProfile { Duration = 60 * 60 * 24 * 7 * 4 });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
            }

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute(); 

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
