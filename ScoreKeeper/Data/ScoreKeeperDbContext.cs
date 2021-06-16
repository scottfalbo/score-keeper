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
        public DbSet<Rummy> Rummy { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RummyPlayer>().HasKey(x => new { x.RummyId, x.PlayerId });
            modelBuilder.Entity<PlayerScore>().HasKey(x => new { x.PlayerId, x.ScoreId });
         }
    }

}
