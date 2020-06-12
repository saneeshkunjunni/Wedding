using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Wedding
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("name=DbConnectionString")
        {
            Database.SetInitializer<EntityContext>(new CreateDatabaseIfNotExists<EntityContext>());
        }
        public DbSet<Wishes> Wishes { get; set; }
    }
}