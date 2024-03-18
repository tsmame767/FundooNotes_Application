using RepositoryLayer.ContextDB;
using BusinessLayer.Interface;
using BusinessLayer.Service;
//using RepositoryLayer.Dto;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Entity;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTO;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add Authentication with JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7086",
            ValidAudience = "https://localhost:7086",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TusharBhairemanekaSecretKeyHai007"))
        };
    });

builder.Services.AddControllers();
//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));



builder.Services.AddSingleton<ContextDataBase>();
builder.Services.AddScoped<IStudentBL, StudentBL>();
builder.Services.AddScoped<IStudentRL,StudentRL>();
builder.Services.AddScoped<INoteBL, NoteBL>();
builder.Services.AddScoped<INoteRL, NoteRL>();
builder.Services.AddScoped<ICollabBL, CollabBL>();
builder.Services.AddScoped<ICollabRL, CollabRL>();
builder.Services.AddScoped<IEmailServiceBL, EmailServiceBL>();
builder.Services.AddScoped<IEmailServiceRL, EmailServiceRL>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FundooNotes_WebAPI",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.Example: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();


//custom jwt auth middleware
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
