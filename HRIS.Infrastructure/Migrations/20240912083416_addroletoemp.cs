using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addroletoemp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tambahkan kolom sementara dengan tipe integer
            migrationBuilder.AddColumn<int>(
                name: "ActorId_temp",
                table: "WorkflowActions",
                type: "integer",
                nullable: true);

            // Isi kolom sementara dengan data yang dikonversi dari ActorId asli
            migrationBuilder.Sql("UPDATE \"WorkflowActions\" SET \"ActorId_temp\" = CAST(\"ActorId\" AS integer);");

            // Hapus kolom asli ActorId
            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "WorkflowActions");

            // Ubah nama kolom sementara menjadi ActorId
            migrationBuilder.RenameColumn(
                name: "ActorId_temp",
                table: "WorkflowActions",
                newName: "ActorId");

            // Menambahkan kolom id_role ke tabel employees
            migrationBuilder.AddColumn<string>(
                name: "id_role",
                table: "employees",
                type: "text",
                nullable: true);

            // Membuat index untuk id_role
            migrationBuilder.CreateIndex(
                name: "IX_employees_id_role",
                table: "employees",
                column: "id_role");

            // Menambahkan foreign key untuk id_role
            migrationBuilder.AddForeignKey(
                name: "FK_employees_AspNetRoles_id_role",
                table: "employees",
                column: "id_role",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Hapus foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_employees_AspNetRoles_id_role",
                table: "employees");

            // Hapus index
            migrationBuilder.DropIndex(
                name: "IX_employees_id_role",
                table: "employees");

            // Hapus kolom id_role
            migrationBuilder.DropColumn(
                name: "id_role",
                table: "employees");

            // Tambahkan kolom ActorId sebagai string
            migrationBuilder.AddColumn<string>(
                name: "ActorId",
                table: "WorkflowActions",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Hapus kolom ActorId integer
            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "WorkflowActions");

            // Ubah kolom ActorId dari integer menjadi string
            migrationBuilder.AlterColumn<string>(
                name: "ActorId",
                table: "WorkflowActions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
