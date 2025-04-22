using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TestsDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ThemeCount> ThemeCounts { get; set; }
        public TestsDbContext(DbContextOptions<TestsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}