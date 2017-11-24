using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Wedding
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("name=DbConnectionString") { }
        public DbSet<Wishes> Wishes { get; set; }
    }
}