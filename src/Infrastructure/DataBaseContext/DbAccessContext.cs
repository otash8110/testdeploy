using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Entities;

namespace Infrastructure.DataBaseContext
{
    public class DbAccessContext: DbContext
    {
        private readonly IConfiguration _configuration;
        public DbAccessContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TaskManager"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.CreatorTasks)
                .HasForeignKey(t => t.CreatorId);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Performer)
                .WithMany(u => u.PerformerTasks)
                .HasForeignKey(t => t.PerformerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectTask>()
                .Property(t => t.PerformerId)
                .IsRequired(false);

            modelBuilder.Entity<Role>().HasData(
                new Role()
                {
                    Id = 1,
                    Name = "TeamLead"
                },
                new Role()
                {
                    Id = 2,
                    Name = "Senior"
                },
                new Role()
                {
                    Id = 3,
                    Name = "Middle"
                },
                new Role()
                {
                    Id = 4,
                    Name = "Junior"
                });

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    FullName = "Otabek Bokhodirov",
                    RoleId = 1,
                    Email = "TeamLead@gmail.com",
                    Password = "pass123"
                },
                new User()
                {
                    Id = 2,
                    FullName = "Alexander Belov",
                    RoleId = 2,
                    Email = "Senior@gmail.com",
                    Password = "pass123"
                },
                new User()
                {
                    Id = 3,
                    FullName = "Akmal Ibragimov",
                    RoleId = 3,
                    Email = "Middle@gmail.com",
                    Password = "pass123"
                },
                new User()
                {
                    Id = 4,
                    FullName = "Otabek Yunusov",
                    RoleId = 4,
                    Email = "Junior@mail.ru",
                    Password = "pass123"
                },
                new User()
                {
                    Id = 5,
                    FullName = "Diyor Fayzullaev",
                    RoleId = 4,
                    Email = "Junior2@mail.ru",
                    Password = "pass123"
                });
        }

    }
}
