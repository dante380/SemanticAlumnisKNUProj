using Microsoft.EntityFrameworkCore;
using SemanticKNUProj.Data;
using VDS.RDF.Query.Algebra;
using SemanticKNUProj.Controllers;

namespace SemanticKNUProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDBCont>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            };

            app.UseHttpsRedirection();
            app.UseCors(options =>
            {
                options.WithOrigins("https://localhost:7245").AllowCredentials().AllowAnyMethod().AllowAnyHeader();


                //options.AllowAnyOrigin(); for any IP Address (Use only for test)
                //options.WithMethods(); for custom allows of Get Post Put Delete Methods!!!
                //options.WithOrigin(); for custom IP Address
            });
            app.UseAuthorization();


            app.MapControllers();



            app.Run();
        }
    }
}