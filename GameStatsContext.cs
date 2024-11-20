using FpsMatchmaker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FpsMatchmaker
{


    public class GameStatsContext : DbContext
    {
        public GameStatsContext(DbContextOptions<GameStatsContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Nickname)
                .IsUnique();

            modelBuilder.Entity<Team>()
                .HasIndex(t => t.TeamName)
                .IsUnique();
        }
    }

}
