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
using StackExchange.Redis;
using Confluent.Kafka;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);


    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


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

    // Configure Redis Cache
    var redisConnectionString = builder.Configuration.GetValue<string>("RedisCacheSettings:ConnectionString");
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));


    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<IUserRL, UserRL>();
    builder.Services.AddScoped<INoteBL, NoteBL>();
    builder.Services.AddScoped<INoteRL, NoteRL>();
    builder.Services.AddScoped<ICollabBL, CollabBL>();
    builder.Services.AddScoped<ICollabRL, CollabRL>();
    builder.Services.AddScoped<IEmailServiceBL, EmailServiceBL>();
    builder.Services.AddScoped<IEmailServiceRL, EmailServiceRL>();
    builder.Services.AddScoped<ICacheService, CacheService>();
    builder.Services.AddScoped<ICacheServiceRL, CacheServiceRL>();
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


    // Register Kafka producer config
    builder.Services.AddSingleton<ProducerConfig>(sp =>
    {
        // Configure Kafka producer properties
        return new ProducerConfig
        {
            BootstrapServers = "localhost:9092", // Kafka broker address
            ClientId = "my-producer" // Client ID for the producer
        };
    });

    // Register Kafka consumer config
    builder.Services.AddSingleton<ConsumerConfig>(sp =>
    {
        // Configure Kafka consumer properties
        return new ConsumerConfig
        {
            BootstrapServers = "localhost:9092", // Kafka broker address
            GroupId = "my-consumer-group", // Consumer group ID
            AutoOffsetReset = AutoOffsetReset.Earliest // Reset offset to beginning
        };
    });

    // Register Kafka producer
    builder.Services.AddSingleton(sp =>
    {
        // Retrieve the registered ProducerConfig service
        var producerConfig = sp.GetRequiredService<ProducerConfig>();

        // Build the producer using the retrieved config
        return new ProducerBuilder<string, string>(producerConfig).Build();
    });

    // Register Kafka consumer
    builder.Services.AddSingleton(sp =>
    {
        // Retrieve the registered ConsumerConfig service
        var consumerConfig = sp.GetRequiredService<ConsumerConfig>();

        // Build the consumer using the retrieved config
        return new ConsumerBuilder<string, string>(consumerConfig).Build();
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:4200")
                              .AllowAnyMethod()
                              .AllowAnyHeader());
    });


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
    app.UseCors("AllowSpecificOrigin");

    //custom jwt auth middleware
    app.UseAuthentication();
    app.UseAuthorization();



    app.MapControllers();
    
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}