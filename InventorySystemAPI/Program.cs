using Application.Extensions;
using Application.Mappers;
using Application.Validators;
using FluentValidation;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace InventorySystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory Management System API", Version = "v1" });
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(SupplierProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(InventoryProfile).Assembly);

            // Application services
            builder.Services.AddApplicationDependencies();

            // Infrastructure registrations
            builder.Services.AddInfrastructureDependencies();

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<SupplierCreateValidator>();

            //services cors
            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
                  {
                      builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                  }));

            var app = builder.Build();

            // Apply EF Core migrations
            if (!app.Environment.IsEnvironment("Testing"))
            {
                DatabaseMigrationHelper.ApplyMigrations<ApiDbContext>(app.Services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("corsapp");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
