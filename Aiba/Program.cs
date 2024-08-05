using Aiba.Entities;
using Aiba.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Aiba
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("allow-all", corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
                {
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.SignIn.RequireConfirmedAccount = false;
                    option.SignIn.RequireConfirmedEmail = false;
                    option.SignIn.RequireConfirmedPhoneNumber = false;
                    option.User.RequireUniqueEmail = true;
                    option.Tokens.AuthenticatorTokenProvider = null!;
                }).AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddMediaInfoProviders();
            builder.Services.AddDecompressServices();

            builder.Services.AddDbContextFactory<AppDBContext>(option =>
            {
                string? connectionString = builder.Configuration["PGDB:connection-string"];
                option.UseNpgsql(connectionString);
            });

            WebApplication app = builder.Build();

            app.InitDatabase();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment())
            {
                app.UseCors("allow-all");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}