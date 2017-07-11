using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using NLog.Extensions.Logging;
using Filenet.Apis.Services;
using Microsoft.Extensions.Configuration;
using Filenet.Apis.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;



namespace Filenet.Apis
{
    public class Startup
    {

        public static IConfigurationRoot Configuration;


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();  
                                             
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)  //for dependecny injection
        {
            services.AddMvc() 
                .AddMvcOptions(o => o.OutputFormatters.Add(

                    new XmlDataContractSerializerOutputFormatter()
                    ))   //allows you to configure supported formatters for input and output

            ;

            ///////Most likely this will not be required ///// Most likely having first letter of propery lowercase is what will be wanted.  Angular likes first letter lowercase
            //.AddJsonOptions(o =>  //this code makes it so that automatic camel case is not sent back to caller - will take property names as they are defined
            //{
            //    if(o.SerializerSettings.ContractResolver != null)
            //    {

            //        var castedResolver = o.SerializerSettings.ContractResolver
            //           as DefaultContractResolver;

            //        castedResolver.NamingStrategy = null;  //will take property names as they are defined
            //    }
            //}

            //);


            //compiler directives
#if DEBUG

            services.AddTransient<IMailService, LocalMailService>();

#else
            services.AddTransient<IMailService, CloudMailService>();
            services.Configure<SMSoptions>(Configuration);
#endif
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString)); //default scope lifetime


            services.AddScoped<ICityInfoRepository, CityInfoRepository>(); //Scoped

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {




            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            loggerFactory.AddNLog();  
            loggerFactory.ConfigureNLog("nlog.config");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //adds exception capability handling to the pipeline.
            }
            else
            {
                app.UseExceptionHandler();
                //app.UseExceptionHandler(
                // options =>
                // {
                //     options.Run(
                //     async context =>
                //     {
                //         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //         context.Response.ContentType = "text/html";
                //         var ex = context.Features.Get<IExceptionHandlerFeature>();
                //         if (ex != null)
                //         {
                //             var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
                //             await context.Response.WriteAsync(err).ConfigureAwait(false);
                //         }
                //     });
                // }
                //);
            }

            cityInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();  //will receive the text based information for status code.



            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
                cfg.CreateMap<Entities.City, Models.CityDto>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
                cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
                cfg.CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
            });



            app.UseMvc(); //add the MVC  handles HTTP Middleware requests now

            //app.Run((context) =>
            //{
            //    throw new Exception("Exception");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}