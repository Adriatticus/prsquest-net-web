using Microsoft.EntityFrameworkCore;
using prsquest_api_controllers.Models;

namespace prsquest_api_controllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opt => {
                opt.JsonSerializerOptions.ReferenceHandler =
                  System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddDbContext<prsquestContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("PrsquestConnectionString"))
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
