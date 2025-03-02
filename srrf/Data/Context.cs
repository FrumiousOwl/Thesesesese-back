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
        public DbSet<AnomalyLog> AnomalyLogs { get; set; }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified
                            || e.State == EntityState.Added
                            || e.State == EntityState.Deleted)
                .ToList();


            var relevantEntities = modifiedEntities.Where(e =>
                e.Entity.GetType().Name == "User" ||
                e.Entity.GetType().Name == "Hardware" ||
                e.Entity.GetType().Name == "HardwareRequest" ||
                e.Entity.GetType().Name.StartsWith("IdentityUserRole`1")).ToList();

            if (!relevantEntities.Any())
            {
                return await base.SaveChangesAsync(cancellationToken); 
            }

            //check if user logged in
            var userPrincipal = _contextAccessor.HttpContext?.User;
            string email = null;
            string roles = null;

            if (userPrincipal?.Identity?.IsAuthenticated == true)
            {

                email = userPrincipal.FindFirst(ClaimTypes.Email)?.Value
                        ?? userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(email))
                {

                    var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();


                    var user = await userManager.FindByEmailAsync(email);

                    if (user != null)
                    {

                        var userRoles = await userManager.GetRolesAsync(user);


                        if (userRoles == null || !userRoles.Any())
                        {
                            await userManager.AddToRoleAsync(user, "User");
                            userRoles = new List<string> { "User" }; 
                        }


                        roles = string.Join(",", userRoles);
                    }
                }
            }


            if (string.IsNullOrEmpty(email))
            {
                email = "NewUserIgnore9182@gmail.com";
                roles = "User"; 
            }


            foreach (var entity in modifiedEntities)
            {
                var auditlog = new AuditLog
                {
                    Email = email,
                    Role = roles, 
                    EntityName = entity.Entity.GetType().Name,
                    Action = entity.State.ToString(),
                    TimeStamp = DateTime.Now,
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
