using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class MessageSendAndDelivered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredMessage_Recruiters_RecruiterId",
                table: "DeliveredMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredMessage_Students_StudentId",
                table: "DeliveredMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_SendMessage_Recruiters_RecruiterId",
                table: "SendMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_SendMessage_Students_StudentId",
                table: "SendMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SendMessage",
                table: "SendMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveredMessage",
                table: "DeliveredMessage");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SendMessage");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "DeliveredMessage");

            migrationBuilder.RenameTable(
                name: "SendMessage",
                newName: "SendMessages");

            migrationBuilder.RenameTable(
                name: "DeliveredMessage",
                newName: "DeliveredMessages");

            migrationBuilder.RenameIndex(
                name: "IX_SendMessage_StudentId",
                table: "SendMessages",
                newName: "IX_SendMessages_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_SendMessage_RecruiterId",
                table: "SendMessages",
                newName: "IX_SendMessages_RecruiterId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveredMessage_StudentId",
                table: "DeliveredMessages",
                newName: "IX_DeliveredMessages_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveredMessage_RecruiterId",
                table: "DeliveredMessages",
                newName: "IX_DeliveredMessages_RecruiterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SendMessages",
                table: "SendMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveredMessages",
                table: "DeliveredMessages",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_SendMessages",
                table: "SendMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveredMessages",
                table: "DeliveredMessages");

            migrationBuilder.RenameTable(
                name: "SendMessages",
                newName: "SendMessage");

            migrationBuilder.RenameTable(
                name: "DeliveredMessages",
                newName: "DeliveredMessage");

            migrationBuilder.RenameIndex(
                name: "IX_SendMessages_StudentId",
                table: "SendMessage",
                newName: "IX_SendMessage_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_SendMessages_RecruiterId",
                table: "SendMessage",
                newName: "IX_SendMessage_RecruiterId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveredMessages_StudentId",
                table: "DeliveredMessage",
                newName: "IX_DeliveredMessage_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveredMessages_RecruiterId",
                table: "DeliveredMessage",
                newName: "IX_DeliveredMessage_RecruiterId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SendMessage",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DeliveredMessage",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SendMessage",
                table: "SendMessage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveredMessage",
                table: "DeliveredMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredMessage_Recruiters_RecruiterId",
                table: "DeliveredMessage",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredMessage_Students_StudentId",
                table: "DeliveredMessage",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendMessage_Recruiters_RecruiterId",
                table: "SendMessage",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendMessage_Students_StudentId",
                table: "SendMessage",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
