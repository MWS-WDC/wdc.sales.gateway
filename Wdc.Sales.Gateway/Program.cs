using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

namespace Wdc.Sales.Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);


            builder.Services.AddAuthentication("JwtBearer")
                .AddJwtBearer("JwtBearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddOcelot();

            var app = builder.Build();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            await app.UseOcelot();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.Run();
        }
    }
}
