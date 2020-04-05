using Microsoft.EntityFrameworkCore;

namespace SampleWebSocket.Context
{
    public class RealTimeContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<Session> Sessions { get; set; }

        public RealTimeContext(DbContextOptions options) : base(options)
        {

        }

        public RealTimeContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
                base.OnConfiguring(optionsBuilder.UseSqlServer(_connectionString));
            else
                base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var session = modelBuilder.Entity<Session>();
            session.ToTable("Session");


            base.OnModelCreating(modelBuilder);
        }
    }
}
