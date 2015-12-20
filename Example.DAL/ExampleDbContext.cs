using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using Example.DAL.Entities;
using Example.DAL.Entities.Abstract;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Example.DAL
{
    public class ExampleDbContext : IdentityDbContext<ExampleUser>
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

        public DbSet<IdentityUserClaim> Claims { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }
        public DbSet<IdentityUserLogin> Logins { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; } // already contains by default
        //public DbSet<ExampleUser> Users { get; set; } // already contains by default
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

            modelBuilder.Entity<Section>()
                .HasOptional(x => x.Parent)
                .WithMany(x => x.ChildSections)
                .Map(x => x.MapKey("Parent_Id"));

            //modelBuilder.Entity<Topic>()
            //    .HasRequired(t => t.Author)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Message>()
            //    .HasRequired(m => m.Author)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
