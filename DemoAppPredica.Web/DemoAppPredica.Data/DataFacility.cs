using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using DemoAppPredica.Data.Interfaces;
using DemoAppPredica.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DemoAppPredica.Data
{
    public class DataFacility
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IJourneyRepository, JourneyRepository>();
            services.AddScoped<CustomDbContext>();
            return services;
        }
    }
}
