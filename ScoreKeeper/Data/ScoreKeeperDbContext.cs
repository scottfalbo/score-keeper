using Microsoft.EntityFrameworkCore;
using ScoreKeeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreKeeper.Data
{
    public class ScoreKeeperDbContext : DbContext
    {
        DbSet<Rummy> Rummy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rummy>().HasData(

            );
        }
    }

}
