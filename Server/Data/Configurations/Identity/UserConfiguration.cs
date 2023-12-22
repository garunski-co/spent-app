using Spent.Server.Models.Identity;

namespace Spent.Server.Data.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        const string userName = "garun@garunski.com";

        builder.HasData([
            new()
            {
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                LockoutEnabled = true,
                Gender = Gender.Other,
                BirthDate = new DateTimeOffset(new(2023, 1, 1)),
                FullName = "Spent test account",
                UserName = userName,
                Email = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                NormalizedEmail = userName.ToUpperInvariant(),
                SecurityStamp = "OMV4VEUKRXPGCP6HAPM5ZO36Y2VYCJRO",
                ConcurrencyStamp = "d3e24da3-6afb-4a21-ad55-7edaa33dae36",
                PasswordHash =
                    "AQAAAAIAAYagAAAAEKa6kiu3Rw46KDmV0at9YifdHr2OdulDNuXrDjf2I8UOS62VqgjkBl0Ke/ruTHgA2w==" // 123456
            }
        ]);
    }
}
