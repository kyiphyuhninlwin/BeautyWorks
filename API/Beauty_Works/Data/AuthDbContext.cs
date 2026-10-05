using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var adminRoleID = "1";

        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Id = adminRoleID,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = adminRoleID
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);

        var adminUserID = "6";
        var admin = new IdentityUser()
        {
            Id = adminUserID,
            UserName = "admin",
            Email = "adminbw@gmail.com",
            NormalizedEmail = "ADMINBW@GMAIL.COM",
            NormalizedUserName = "ADMIN",
        };

        admin.PasswordHash = "AQAAAAIAAYagAAAAE...";

        builder.Entity<IdentityUser>().HasData(admin);

        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new()
            {
                UserId = adminUserID,
                RoleId = adminRoleID
            }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}