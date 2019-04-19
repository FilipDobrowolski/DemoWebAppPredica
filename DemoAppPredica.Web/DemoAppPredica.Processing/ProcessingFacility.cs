using System;
using System.Collections.Generic;
using System.Text;
using DemoAppPredica.Processing.Interfaces;
using DemoAppPredica.Processing.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DemoAppPredica.Processing
{
    public class ProcessingFacility
    {

        public static IServiceCollection Register(IServiceCollection services)
        {
            services.AddScoped<IJourneyService, JourneyService>();

            return services;
        }
    }
}
