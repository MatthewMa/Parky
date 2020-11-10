using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }
    }
}
