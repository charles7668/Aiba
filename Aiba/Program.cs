using Aiba.Entities;
using Aiba.Extensions;
using Aiba.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Aiba
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(configure =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                configure.Filters.Add(new AuthorizeFilter(policy));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("allow-develop-frontend", corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowCredentials()
                        .WithOrigins(["http://localhost:5173"])
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
            builder.Services.ConfigureApplicationCookie(options =>
            {
                // handle 401, 403, 200 status code from frontend
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToLogout = context =>
                {
                    context.Response.StatusCode = 200;
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddResponseCaching();

            builder.Services.AddMediaInfoProviders();
            builder.Services.AddDecompressServices();

            builder.Services.AddDbContextFactory<AppDBContext>(option =>
            {
                string? connectionString = builder.Configuration["PGDB:connection-string"];
                option.UseNpgsql(connectionString);
                option.EnableSensitiveDataLogging(false);
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
                app.UseCors("allow-develop-frontend");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCaching();

            app.MapControllers();

            app.Run();
        }
    }
}