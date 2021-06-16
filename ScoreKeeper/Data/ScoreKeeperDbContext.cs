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

            modelBuilder.Entity<Rummy>().HasData(
                new Rummy
                {
                    Id = 1,
                    SaveAs = "testGame"
                });

            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    Id = 1,
                    Name = "Spaceghost",
                    Wins = 0
                });

            modelBuilder.Entity<Score>().HasData(
                new Score
                { Id = 1, Points = 100 },
                new Score
                { Id = 2, Points = 150 }
                );

            modelBuilder.Entity<RummyPlayer>().HasData(
                new RummyPlayer
                { RummyId = 1, PlayerId = 1 },
                new RummyPlayer
                { RummyId = 1, PlayerId = 2}
                );
         }
    }

}
