using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Core.ActionFilters;
using Core.Configurations;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.MemoryCache;
using Core.CrossCuttingConcerns.Caching.RedisCache;
using Core.DependencyResolvers;
using Core.Utilities.Extensions;
using Core.Utilities.Helpers;
using Core.Utilities.IoC;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers().AddNewtonsoftJson(opts =>
{
    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
})
   .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});
builder.Services.AddDbContext<MovieContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
});
builder.Services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<MovieContext>()
               .AddDefaultTokenProviders();
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWTOptions"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var jwtOptions = builder.Configuration.GetSection("JWTOptions").Get<JWTOptions>();
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience[0],
        IssuerSigningKey = SecurityKeyHelper.GetSymmetricSecurityKey(jwtOptions.SecurityKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var cacheSettings = builder.Configuration.GetSection("CacheSettings").Get<CacheSettings>();
if (cacheSettings.UseDistributedCache)
{
    if (cacheSettings.PreferRedis)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = cacheSettings.RedisURL;
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { cacheSettings.RedisURL }
            };
        });
    }
    else
    {
        builder.Services.AddDistributedMemoryCache();
    }

    builder.Services.AddTransient<ICacheService, RedisCacheService>();
}
else
{
    builder.Services.AddMemoryCache();
    builder.Services.AddTransient<ICacheService, MemoryCacheService>();
}


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 9;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddScoped<NullFilterAttribute>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddDependencyResolvers((IConfiguration)builder.Configuration, new ICoreModule[] {
               new CoreModule()
            });
// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
ValidatorOptions.Global.LanguageManager.Enabled = false;
var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



var path = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(path))
{
    Directory.CreateDirectory(path);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<MovieContext>();
    dataContext.Database.Migrate();
}
app.UseCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors(builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
