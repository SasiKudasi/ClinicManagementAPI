using ClinicManagementAPI.Application.Contracts;
using ClinicManagementAPI.Application.Services;
using ClinicManagementAPI.Application.Utilities;
using ClinicManagementAPI.Core.Abstraction;
using ClinicManagementAPI.DataAccess;
using ClinicManagementAPI.DataAccess.Entities;
using ClinicManagementAPI.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

/* ToDo
 * 1. Cделать какую никакую валидацию
 * 2. база данных
 * 3. Тесты :(
 * 4. посмотреть че по рефакторингу
 */
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClinicManagementDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));
builder.Services.AddScoped<IEntityResolver, EntityResolver>();
builder.Services.AddScoped<ICrudService<PatientEntity, PatientDto>, PatientService>();
builder.Services.AddScoped<ICrudService<DoctorEntity, DoctorDto>, DoctorService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

