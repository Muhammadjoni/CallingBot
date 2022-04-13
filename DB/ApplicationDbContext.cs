// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using CallingBotSample.Model;

namespace CallingBotSample.DB
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // public virtual DbSet<User> Users { get; set; }
    public DbSet<Actions> Actions { get; set; }
    // public DbSet<ParticipantDetails> Participants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
      var config = builder.Build();
      var connectionString = config.GetConnectionString("DefaultConnection");

      if (!optionsBuilder.IsConfigured)
      {

        optionsBuilder.UseNpgsql(connectionString);
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

  }
}

// {
//   public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
//   {
//   }

//   public DbSet<User> User { get; set; }

//   protected override void OnModelCreating(ModelBuilder modelBuilder)
//   {
//     base.OnModelCreating(modelBuilder);
//   }
// }
