using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

using IdentityServer4.AccessTokenValidation;

using Tattel.WebApi.Middleware;
using Tattel.WebApi.Persistence;
using Tattel.WebApi.Realtime;
using Tattel.WebApi.Interfaces;
using Hangfire;
using System;

namespace Tattel.WebApi
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
            //you will use some way to get your connection string
            var mongoConnection = "mongodb://tattledb:g0JveHuBmR5Wa5BXeovMFkWL8NgKt4wxpdsywPZKzQE5wyaYT316pxAbQMIaXQIzZp1xBrDFJLUMBTezZJZV7Q==@tattledb.mongo.cosmos.azure.com:10255/tattle-db?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@tattledb@";
            // Add Hangfire services. Hangfire.AspNetCore nuget required
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(@"Server=tcp:tattla-sql-server.database.windows.net,1433;Initial Catalog=tattla-sql-server;Persist Security Info=False;User ID=tattle-sql;Password=Hangfire@123")
                //.UseMongoStorage(mongoConnection, new MongoStorageOptions
                //{
                //    MigrationOptions = new MongoMigrationOptions
                //    {
                //        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                //        BackupStrategy = new CollectionMongoBackupStrategy()
                //    },
                //    Prefix = "hangfire.mongo",
                //    CheckConnection = false, 
                //  //  ConnectionCheckTimeout = TimeSpan.FromSeconds(5)
                //})
            );
            // Add the processing server as IHostedService
            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = "Hangfire.Mongo.server";
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                      .WithOrigins("http://localhost:5006")
                      .AllowCredentials();
                });
            });

            services.AddControllers();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetSection("Urls:AuthServer").Value;
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "tattelapi";
                });


            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();

                options.AddPolicy("UserOnly", policy => policy.RequireClaim("id"));
            });


            services.AddMvc(options =>
            {
                options.Filters.Add(new RequestResponseLogHandler());
            })
            .AddNewtonsoftJson()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSingleton<IConversationRepository, ConversationRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IGPSLocationRepository, GPSLocationRepository>();
            services.AddSingleton<IMessageRepository, MessageRepository>();
            services.AddSingleton<IUserLocationRepository, UserLocationRepository>();
            services.AddSingleton<IConfigurationRepository, ConfigurationRepository>();
            services.AddSingleton(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("doc", new OpenApiInfo
                {
                    Title = "Tattel Web Api",
                    Version = "v1",
                    Description = "Tattel Web Service",
                    Contact = new OpenApiContact
                    {
                        Name = "Tattel",
                        Email = "tattelglobal@gmail.com",
                   
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == Environments.Development)
            {
            }
            else
            {
                app.UseHsts();
            }


            //app.UseCustomExceptionHandler();

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/doc/swagger.json", "Tattle API");

                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
 