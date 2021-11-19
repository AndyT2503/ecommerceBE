using Ecommerce.Application.Services.AuthService;
using Ecommerce.Application.Services.MailNotifyService;
using Ecommerce.Application.Services.NotificationService;
using Ecommerce.Application.Services.Socket.Hubs;
using Ecommerce.Application.Services.Socket.SocketService;
using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.User;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.ConfigureCors();
            services.ConfigureMediatR();
            services.ConfigureAuthentication(Configuration);
            services.ConfigureMailService(Configuration);
            services.ConfigureDbContext(Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
            services.ConfigureSocketService();
            services.ConfigureRedis(Configuration);
            services.ConfigureNotificationService();
            services.AddSingleton<ICurrentUser, CurrentUser>();
            services.AddSingleton<AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(
            () => app.ConfigUserDb());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce.API v1"));
            }
            app.UseCors("CorsPolicy");
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notification-hub");
            });

            app.ConfigUserDb();
        }

    }

    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4500")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });
        }

        public static void ConfigureMailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            var mailSettings = configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);
            services.AddTransient<IMailNotifyService, MailNotifyService>();
        }

        public static void ConfigureNotificationService(this IServiceCollection services)
        {
            services.AddScoped<NotificationService>();
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Ecommerce")).EnableSensitiveDataLogging());
        }

        public static void ConfigureMediatR(this IServiceCollection services)
        {
            //Register Assembly Where All Handlers Stored
            var assembly = AppDomain.CurrentDomain.Load("Ecommerce.Application");
            services.AddMediatR(assembly);
        }

        public static void ConfigureSocketService(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddScoped<SocketService>();
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Url"];
                options.InstanceName = configuration["Redis:Prefix"];
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var credential = configuration["AppCredential"];
            var key = Encoding.ASCII.GetBytes(credential);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
                option.Events = new JwtBearerEvents // Config get token from signalR connection request
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/notification-hub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }

    public static class AppExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Code = context.Response.StatusCode,
                            Detail = $"{contextFeature.Error.Message}"
                        });
                    }
                });
            });
        }

        public static void ConfigUserDb(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MainDbContext>();

                var users = context.Users.ToListAsync().Result;
                if (users.Count == 0)
                {
                    var newUser = new User() { FirstName = "admin", LastName = "super", Role = "super_admin", Username = "admin", Password = BCrypt.Net.BCrypt.HashPassword("admin") };
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
                else
                {
                    foreach (User user in users)
                    {
                        try
                        {
                            BCrypt.Net.BCrypt.Verify("admin", user.Password);
                        }
                        catch
                        {
                            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                            context.Update(user);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }

}
