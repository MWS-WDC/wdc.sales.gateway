using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Wdc.Sales.Orders.Api.Models;
using Wdc.Sales.Orders.Api.Persistence;

namespace Wdc.Sales.Orders.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add JWT auth
            builder.Services.AddAuthentication("JwtBearer")
                .AddJwtBearer("JwtBearer", options =>
                {
                    options.Authority = "http://localhost:5000"; // ����� ��� Gateway
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });


            builder.Services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHostedService<DatabaseMigrationHostedService>();

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
