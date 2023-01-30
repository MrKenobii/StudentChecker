using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class MessageSendAndDelivered2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredMessages_Recruiters_RecruiterId",
                table: "DeliveredMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredMessages_Students_StudentId",
                table: "DeliveredMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_SendMessages_Recruiters_RecruiterId",
                table: "SendMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_SendMessages_Students_StudentId",
                table: "SendMessages");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "SendMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RecruiterId",
                table: "SendMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "DeliveredMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RecruiterId",
                table: "DeliveredMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredMessages_Recruiters_RecruiterId",
                table: "DeliveredMessages",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredMessages_Students_StudentId",
                table: "DeliveredMessages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SendMessages_Recruiters_RecruiterId",
                table: "SendMessages",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SendMessages_Students_StudentId",
                table: "SendMessages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredMessages_Recruiters_RecruiterId",
                table: "DeliveredMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredMessages_Students_StudentId",
                table: "DeliveredMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_SendMessages_Recruiters_RecruiterId",
                table: "SendMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_SendMessages_Students_StudentId",
                table: "SendMessages");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "SendMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecruiterId",
                table: "SendMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "DeliveredMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecruiterId",
                table: "DeliveredMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredMessages_Recruiters_RecruiterId",
                table: "DeliveredMessages",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredMessages_Students_StudentId",
                table: "DeliveredMessages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendMessages_Recruiters_RecruiterId",
                table: "SendMessages",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendMessages_Students_StudentId",
                table: "SendMessages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
