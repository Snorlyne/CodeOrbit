using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Context
{
    public class ApplicationDBContext : DbContext
    {
        public virtual DbSet<Personas> Personas { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Personas>().HasQueryFilter(x => !x.IsDeleted);

        }
    }
}