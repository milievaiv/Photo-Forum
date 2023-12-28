using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoForum.Data;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Repositories;
using PhotoForum.Services.Contracts;
using PhotoForum.Services;
using Microsoft.AspNetCore.Identity;
using PhotoForum.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PhotoForum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    //ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]))
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
             
            builder.Services.AddDbContext<PhotoForumContext>(options =>
            {
                string connectionString = @"Data Source=127.0.0.1,1435;Initial Catalog=PhotoForum;User Id=sqlserver;Password=D?3F&>#(}HAmCOi%;";
                //string connectionString = "Server=localhost;Database=Demo;Trusted_Connection=True;";
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(PhotoForum.Data.PhotoForumContext).Assembly.FullName));
                options.EnableSensitiveDataLogging();
            });

            builder.Services.AddIdentity<BaseUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            var app = builder.Build();

            var roleManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = app.Services.GetRequiredService<UserManager<BaseUser>>();

            string roleName = "Admin";
            var roleExists = roleManager.RoleExistsAsync(roleName).Result;

            if (!roleExists)
            {
                roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        //public async Task CreateAdminUser(UserManager<BaseUser> userManager)
        //{
        //    var adminUser = new ApplicationUser
        //    {
        //        UserName = "admin@example.com",
        //        Email = "admin@example.com",
        //        // other properties...
        //    };

        //    var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

        //    if (result.Succeeded)
        //    {
        //        await userManager.AddToRoleAsync(adminUser, "Admin");
        //    }
        //}
    }
}