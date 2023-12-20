using Microsoft.EntityFrameworkCore;
using PhotoForum.Data;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Repositories;
using PhotoForum.Services.Contracts;
using PhotoForum.Services;

namespace PhotoForum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PhotoForumContext>(options =>
            {
                string connectionString = @"Data Source=127.0.0.1;Initial Catalog=PhotoForum;User Id=sqlserver;Password=D?3F&>#(}HAmCOi%;";

                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(PhotoForum.Data.PhotoForumContext).Assembly.FullName));
                options.EnableSensitiveDataLogging();
            });

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            //builder.Services.AddScoped<IUsersService, UsersService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}