
using Core.Configurations;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.RedisCache;
using Core.CrossCuttingConcerns.Logging.SeriLog.Loggers;
using Core.Utilities;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddSingleton<Stopwatch>();
            services.AddTransient<IEmailService, EmailManager>();
        }
    }
}
