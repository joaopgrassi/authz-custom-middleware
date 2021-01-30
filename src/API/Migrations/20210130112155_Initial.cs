using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("007b1d74-7e69-466a-8681-61121e5422c1"), "Login" },
                    { new Guid("c8b858d6-404d-4127-b376-126adb852b83"), "Create" },
                    { new Guid("7762be20-68cb-44eb-8f70-a58b0b3c91cc"), "Update" },
                    { new Guid("f9e09403-a61e-4c45-8f06-7ca058f3317e"), "Delete" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "ExternalId" },
                values: new object[,]
                {
                    { new Guid("16e41b39-a078-4496-83b9-ef6fa7074e4c"), "bobsmith@email.com", "88421113" },
                    { new Guid("bebc650b-a3e4-4b76-a2f2-019413d0542f"), "alicesmith@email.com", "818727" }
                });
            
            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "Id", "UserId", "PermissionId" },
                values: new object[,]
                {
                    { new Guid("426E8F4D-6E47-4A81-A1D4-E1B5DE0366FD"), new Guid("16e41b39-a078-4496-83b9-ef6fa7074e4c"), new Guid("007b1d74-7e69-466a-8681-61121e5422c1") },
                    { new Guid("4B57B9F8-DF55-44AC-8FBB-E63DCB9474D1"), new Guid("bebc650b-a3e4-4b76-a2f2-019413d0542f"), new Guid("007b1d74-7e69-466a-8681-61121e5422c1") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
