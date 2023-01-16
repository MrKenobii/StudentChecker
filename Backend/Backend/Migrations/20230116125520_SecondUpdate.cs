using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class SecondUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecruiterCompanies_RecruiterCompanies_RecruiterCompanyRecrui~",
                table: "RecruiterCompanies");

            migrationBuilder.DropIndex(
                name: "IX_RecruiterCompanies_RecruiterCompanyRecruiterId_RecruiterComp~",
                table: "RecruiterCompanies");

            migrationBuilder.DropColumn(
                name: "RecruiterCompanyCompanyId",
                table: "RecruiterCompanies");

            migrationBuilder.DropColumn(
                name: "RecruiterCompanyRecruiterId",
                table: "RecruiterCompanies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecruiterCompanyCompanyId",
                table: "RecruiterCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecruiterCompanyRecruiterId",
                table: "RecruiterCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecruiterCompanies_RecruiterCompanyRecruiterId_RecruiterComp~",
                table: "RecruiterCompanies",
                columns: new[] { "RecruiterCompanyRecruiterId", "RecruiterCompanyCompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecruiterCompanies_RecruiterCompanies_RecruiterCompanyRecrui~",
                table: "RecruiterCompanies",
                columns: new[] { "RecruiterCompanyRecruiterId", "RecruiterCompanyCompanyId" },
                principalTable: "RecruiterCompanies",
                principalColumns: new[] { "RecruiterId", "CompanyId" });
        }
    }
}
