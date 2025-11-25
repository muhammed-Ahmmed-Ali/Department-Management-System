using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using session5demo.dl.Models.AuthModel;
using session5demo.dl.Models.AuthModel;
using session5demo.dl.Models.DepartmentModels;
using session5demo.dl.Models.EmployeeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace session5demo.dl.Contexts
{
    public class demoContexsts:IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public demoContexsts(DbContextOptions<demoContexsts> options) : base(options)
        {
        }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(demoContexsts).Assembly);
        }
        public DbSet<Department> departments { get; set; }
        public DbSet<Employee>employees { get; set; }
    }
}
