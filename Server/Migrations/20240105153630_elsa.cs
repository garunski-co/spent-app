using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spent.Server.Migrations
{
    /// <inheritdoc />
    public partial class elsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e852009e-27d3-431b-bcc9-ce53eb0b4f4d"));

            migrationBuilder.AlterColumn<string>(
                name: "PlaidAccessToken_Value",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "ConfirmationEmailRequestedOn", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageName", "ResetPasswordEmailRequestedOn", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("f516eaa3-6c76-40c9-b47a-9cf684e902bc"), 0, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)), "d3e24da3-6afb-4a21-ad55-7edaa33dae36", null, "garun@garunski.com", true, "Spent test account", 2, true, null, "GARUN@GARUNSKI.COM", "GARUN@GARUNSKI.COM", "AQAAAAIAAYagAAAAEKa6kiu3Rw46KDmV0at9YifdHr2OdulDNuXrDjf2I8UOS62VqgjkBl0Ke/ruTHgA2w==", null, false, null, null, "OMV4VEUKRXPGCP6HAPM5ZO36Y2VYCJRO", false, "garun@garunski.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f516eaa3-6c76-40c9-b47a-9cf684e902bc"));

            migrationBuilder.AlterColumn<string>(
                name: "PlaidAccessToken_Value",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "ConfirmationEmailRequestedOn", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageName", "ResetPasswordEmailRequestedOn", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("e852009e-27d3-431b-bcc9-ce53eb0b4f4d"), 0, new DateTimeOffset(new DateTime(1949, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)), "d3e24da3-6afb-4a21-ad55-7edaa33dae36", null, "garun@garunski.com", true, "Garun Vagidov", 0, true, null, "GARUN@GARUNSKI.COM", "GARUN@GARUNSKI.COM", "AQAAAAIAAYagAAAAEKa6kiu3Rw46KDmV0at9YifdHr2OdulDNuXrDjf2I8UOS62VqgjkBl0Ke/ruTHgA2w==", null, false, null, null, "OMV4VEUKRXPGCP6HAPM5ZO36Y2VYCJRO", false, "garun@garunski.com" });
        }
    }
}
