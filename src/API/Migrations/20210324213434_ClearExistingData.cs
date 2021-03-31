using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ClearExistingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // deleted previous inserted data; New data is inserted via DbMigratorHostedService.cs
            migrationBuilder.Sql("DELETE FROM UserPermissions");
            migrationBuilder.Sql("DELETE FROM Users");
            migrationBuilder.Sql("DELETE FROM Permissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
