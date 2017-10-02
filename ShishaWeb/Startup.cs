using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using ShishaWeb.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using ShishaWeb.Auth;
using ShishaWeb.Repositories;
using ShishaWeb.Repositories.Interfaces;
using ShishaWeb.Services.Interfaces;
using ShishaWeb.Services;

namespace ShishaWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable the use of an [Authorize("Bearer")] attribute on methods and classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });            

            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });
            
            this.RegisterRepositories(services);

            this.RegisterServices(services);

            services.AddMvc();
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IArticlesRepository, ArticlesRepository>();
            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IShishasRepository, ShishasRepository>();
            services.AddTransient<ITabaccoRepository, TabaccoRepository>();
            services.AddTransient<ITabaccoRelationRepository, TabaccoRelationRepository>();
            services.AddTransient<IQrCodeRepository, QrCodeRepository>();
            services.AddTransient<IShishaSmokersRepository, ShishaSmokersRepository>();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IShishasService, ShishasService>();
            services.AddTransient<ITabaccoService, TabaccoService>();
            services.AddTransient<IQrCodeService, QrCodeService>();

            services.AddScoped<IAuditProvider, AuditProvider>();
            services.AddSingleton<MongoContext>(_ => new MongoContext(
                Configuration.GetSection("MongoConnection:ConnectionString").Value.ToString(),
                Configuration.GetSection("MongoConnection:Database").Value.ToString()
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            #region Handle Exception
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    //when authorization has failed, should retrun a json message to client
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new RequestResult
                        {
                            State = RequestState.NotAuth,
                            Message = "token expired"
                        }));
                    }
                    //when orther error, retrun a error message json to client
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new RequestResult
                        {
                            State = RequestState.Failed,
                            Message = error.Error.Message
                        }));
                    }
                    //when no error, do next.
                    else await next();
                });
            });
            #endregion

            #region UseJwtBearerAuthentication
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    // When receiving a token, check that we've signed it.
                    ValidateIssuerSigningKey = true,
                    // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,
                    // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                    // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                    // machines which should have synchronised time, this can be set to zero. Where external tokens are
                    // used, some leeway here could be useful.
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });
      #endregion

            app.Use(async (context, next) => {
              await next();
              if (context.Response.StatusCode == 404 &&
                 !Path.HasExtension(context.Request.Path.Value) &&
                 !context.Request.Path.Value.StartsWith("/api/"))
              {
                //TODO set this to not found page
                //TODO add another case for /ShishAdmin
                context.Request.Path = "/index.html";
                await next();
              }
            });
            //app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:8100/").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseMyAuditAuthorize();

            app.UseMvc();
        }
    }
}
