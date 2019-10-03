using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;

namespace OGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://www.c-sharpcorner.com/article/building-api-gateway-using-ocelot-in-asp-net-core-part-two/

            new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                       .AddJsonFile("ocelot.json")
                       .AddEnvironmentVariables();
               })
               .ConfigureServices(s => {
                   s.AddOcelot();

                   var key = "YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv";
                   var iss = "http://localhost:45092/";
                   var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                   var tokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = signingKey,
                       ValidateIssuer = true,
                       ValidIssuer = iss,
                       ValidateAudience = true,
                       ValidAudience = iss,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       RequireExpirationTime = true,
                   };

                   s.AddAuthentication(o =>
                   {
                       //o.DefaultAuthenticateScheme = "TestKey";
                       o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                       o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   })
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = tokenValidationParameters;
                    });
               })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   //add your logging
               })
               .UseIISIntegration()
               .Configure(app =>
               {
                   app.UseAuthentication();
                   app.UseOcelot().Wait();
               })
               .Build()
               .Run();
        }
    }
}
