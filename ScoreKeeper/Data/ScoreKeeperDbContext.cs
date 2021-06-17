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
        public DbSet<RummyPlayer> RummyPlayers { get; set; }
        public DbSet<PlayerScore> PlayerScores { get; set; }

        public ScoreKeeperDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RummyPlayer>().HasKey(x => new { x.RummyId, x.PlayerId });
            modelBuilder.Entity<PlayerScore>().HasKey(x => new { x.PlayerId, x.ScoreId });

            modelBuilder.Entity<Rummy>().HasData(
                new Rummy
                {
                    Id = 1,
                    SaveAs = "testGame",
                    Limit = 1000
                });

            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    Id = 1,
                    Name = "Spaceghost",
                    Wins = 0
                },
                new Player
                {
                    Id = 2,
                    Name = "Harry Winston",
                    Wins = 0
                }
                );

            modelBuilder.Entity<Score>().HasData(
                new Score
                { Id = 1, Points = 100 },
                new Score
                { Id = 2, Points = 150 },
                new Score
                { Id = 3, Points = 80 },
                new Score
                { Id = 4, Points = 130 }
                );

            modelBuilder.Entity<RummyPlayer>().HasData(
                new RummyPlayer
                { RummyId = 1, PlayerId = 1 },
                new RummyPlayer
                { RummyId = 1, PlayerId = 2 }
                );

            modelBuilder.Entity<PlayerScore>().HasData(
                new PlayerScore
                { PlayerId = 1, ScoreId =1 },
                new PlayerScore
                { PlayerId = 1, ScoreId = 2 },
                new PlayerScore
                { PlayerId = 2, ScoreId = 3 },
                new PlayerScore
                { PlayerId = 2, ScoreId = 4 }
                );

        }
    }

}
