﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using srrf.Models;
using System.Text;

namespace srrf.Data
{
    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<HardwareRequest> HardwareRequests { get; set; }
        public DbSet<Hardware> Hardware { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet <AuditLog> AuditLogs { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified
                || e.State == EntityState.Added
                || e.State == EntityState.Deleted)
                .ToList();

            foreach (var entity in modifiedEntities)
            {
                var auditlog = new AuditLog
                {
                    EntityName = entity.Entity.GetType().Name,
                    Action = entity.State.ToString(),
                    TimeStamp = DateTime.UtcNow,
                    Changes = GetChanges(entity)
                };

                AuditLogs.Add(auditlog);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private string GetChanges(EntityEntry entity)
        {
            var changes = new StringBuilder();

            foreach (var property in entity.OriginalValues.Properties) 
            {
                var originalValue = entity.OriginalValues[property];
                var currentValue = entity.CurrentValues[property];

                if (!Equals(originalValue, currentValue))
                {
                    changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
                }
            }
            return changes.ToString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }
}
