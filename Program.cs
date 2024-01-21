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
using PhotoForum.Helpers.Contracts;
using PhotoForum.Helpers;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PhotoForum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.

            //builder.Services.AddControllers()
            //    .AddNewtonsoftJson(options =>
            //    {
            //        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    });

            builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

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

            // Http Session
            builder.Services.AddSession(options =>
            {
                // With IdleTimeout you can change the number of seconds after which the session expires.
                // The seconds reset every time you access the session.
                // This only applies to the session, not the cookie.
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IAdminsRepository, AdminsRepository>();

            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IAdminsService, AdminsService>();
            builder.Services.AddScoped<IPostService, PostService>();

            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IVerificationService, VerificationService>();

            builder.Services.AddScoped<IModelMapper, ModelMapper>();

            builder.Services.AddSwaggerGen(c =>
            {
                // Register ConflictingActionsResolver to handle conflicts
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // Your other Swagger configurations...
            });

            builder.Services.AddSignalR();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSession();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCoreDemo API V1");
                options.RoutePrefix = "api/swagger";
            });

            app.UseHttpsRedirection();

            // Enable authentication and authorization.
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseDeveloperExceptionPage();

            app.UseMiddleware<HeaderInspectionMiddleware>();

            app.Run();

        }
    }
}