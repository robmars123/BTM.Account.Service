using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTM.Account.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimsForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "UserClaimID", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "Role", "Admin", new Guid("14802186-7d6a-41e1-a209-f61fba883837") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "UserClaimID",
                keyValue: 1);
        }
    }
}
