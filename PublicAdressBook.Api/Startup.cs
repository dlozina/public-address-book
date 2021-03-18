using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PublicAddressBook.Api.Hubs;
using PublicAddressBook.DataAccess.Context;
using PublicAddressBook.Service.ApplicationService;
using PublicAddressBook.Service.ApplicationService.Interface;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PublicAdressBook.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllers();
            services.AddDbContext<PublicAddressBookContext>(options =>
                     options.UseNpgsql(Configuration.GetConnectionString("PublicAddressBookConnection")));

            services.AddScoped<IContacts, Contacts>();

            // POC Settings
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("*"));
                //options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
                //{
                //    builder.WithOrigins("http://localhost:44350");
                //});
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PublicAdressBook.Api", Version = "v1" });
            });

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Log config file loaded");

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicAdressBook.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // POC Settings
            app.UseCors("Open");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LiveUpdatesHub>("/liveupdateshub");
                endpoints.MapControllers();
            });
        }
    }
}