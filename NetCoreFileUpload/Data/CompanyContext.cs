using Microsoft.EntityFrameworkCore;
using NetCoreFileUpload.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreFileUpload.Data
{
    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> options)
            :base (options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppFile> AppFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppFile>(entity =>
            {
                entity.HasOne(e => e.Employee)
                    .WithMany(f => f.AppFiles)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
