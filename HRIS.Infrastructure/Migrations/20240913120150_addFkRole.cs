using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFkRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSequences_RequiredRole",
                table: "WorkflowSequences",
                column: "RequiredRole",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowSequences_AspNetRoles_RequiredRole",
                table: "WorkflowSequences",
                column: "RequiredRole",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowSequences_AspNetRoles_RequiredRole",
                table: "WorkflowSequences");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowSequences_RequiredRole",
                table: "WorkflowSequences");
        }
    }
}
