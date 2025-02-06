using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using srrf.Models;
using System.Security.Claims;
using System.Text;

namespace srrf.Data
{
    public class Context : IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IServiceProvider _serviceProvider;
        public Context(DbContextOptions<Context> options, 
            IHttpContextAccessor contextAccessor,
            IServiceProvider serviceProvider) : base(options)
        {
            _contextAccessor = contextAccessor;
            _serviceProvider = serviceProvider;
        }
        public DbSet<HardwareRequest> HardwareRequests { get; set; }
        public DbSet<Hardware> Hardware { get; set; }
        public DbSet <AuditLog> AuditLogs { get; set; }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified
                || e.State == EntityState.Added
                || e.State == EntityState.Deleted)
                .ToList();


            var email = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
                 ?? _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            
            var user = await userManager.FindByEmailAsync(email);

            var roles = await userManager.GetRolesAsync(user);

            foreach (var entity in modifiedEntities)
            {
                var auditlog = new AuditLog
                {
                    Email = email,
                    Role = string.Join(",", roles),
                    EntityName = entity.Entity.GetType().Name,
                    Action = entity.State.ToString(),
                    TimeStamp = DateTime.UtcNow,
                    Changes = GetChanges(entity)
                };

                AuditLogs.Add(auditlog);
            }

            return await base.SaveChangesAsync(cancellationToken);
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }

    }
}
