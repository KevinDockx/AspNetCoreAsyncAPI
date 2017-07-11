using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Filenet.Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //basically this is just a console application using .NET libraries
            var host = new WebHostBuilder() //necessary for hosting the application.  create instance of it. 
                .UseKestrel() //cross platform web server
                .UseContentRoot(Directory.GetCurrentDirectory())  //base root for content used by applicaiton //not the same as the webroot.
                .UseIISIntegration() //uses as reverse proxy for kestrel  //if on linux use apache as reverse proxy for Kestrel
                .UseStartup<Startup>() //tells it to use the startup class as the begin point of applicaiton
                //.UseApplicationInsights()
                .Build();//builds iweb host instance

            host.Run();
        }
    }
}
