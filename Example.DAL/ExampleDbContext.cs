using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using Example.DAL.Entities;
using Example.DAL.Entities.Abstract;

namespace Example.DAL
{
    public class ExampleDbContext : DbContext
    {
        private static readonly string _connectionStringName = "ExampleContext";

        public ExampleDbContext() : base(_connectionStringName)
        {
        }

        public static ExampleDbContext Create()
        {
            return new ExampleDbContext();
        }

        #region DbSets

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<BinaryData> Blobs { get; set; }

        #endregion

        #region AutoUpdate Created/Modified dates

        public override int SaveChanges()
        {
            setDatesProperies();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            setDatesProperies();
            return base.SaveChangesAsync();
        }

        private void setDatesProperies()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity as IDatesEntity;
                if (entity != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedDate = DateTime.UtcNow;
                        entity.ModifiedDate = DateTime.UtcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.ModifiedDate = DateTime.UtcNow;
                    }
                }
            }
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
