using RepositoryLayer.ContextDB;
using BusinessLayer.Interface;
using BusinessLayer.Service;
//using RepositoryLayer.Dto;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Entity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ContextDataBase>();
builder.Services.AddTransient<IStudentBL, StudentBL>();
builder.Services.AddTransient<IStudentRL,StudentRL>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
