using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HRIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCurrentStepIdFromWorkflowSequences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
             name: "CurrentStepId",
             table: "WorkflowSequences");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
           name: "CurrentStepId",
           table: "WorkflowSequences",
           type: "integer",
           nullable: false,
           defaultValue: 0);
        }
    }
}
