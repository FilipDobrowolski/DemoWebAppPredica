using System;
using System.Collections.Generic;
using System.Text;
using DemoAppPredica.Models.Models.Journeys;
using Microsoft.EntityFrameworkCore;

namespace DemoAppPredica.Data
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options)
            : base(options)
        { }

        public DbSet<Journey> Journeys { get; set; }


    }
}
