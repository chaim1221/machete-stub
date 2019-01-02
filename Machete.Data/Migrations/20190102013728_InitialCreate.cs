using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Machete.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Workers_ID",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Emails_EmailID",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_EmailID",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "EmailID",
                table: "WorkOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Workers",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Persons",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "JoinWorkOrderEmail",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    datecreated = table.Column<DateTime>(nullable: false),
                    dateupdated = table.Column<DateTime>(nullable: false),
                    Createdby = table.Column<string>(maxLength: 30, nullable: true),
                    Updatedby = table.Column<string>(maxLength: 30, nullable: true),
                    WorkOrderID = table.Column<int>(nullable: false),
                    EmailID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinWorkOrderEmail", x => new { x.EmailID, x.WorkOrderID });
                    table.ForeignKey(
                        name: "FK_JoinWorkOrderEmail_Emails_EmailID",
                        column: x => x.EmailID,
                        principalTable: "Emails",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinWorkOrderEmail_WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoinWorkOrderEmail_WorkOrderID",
                table: "JoinWorkOrderEmail",
                column: "WorkOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Persons_ID",
                table: "Workers",
                column: "ID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Persons_ID",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "JoinWorkOrderEmail");

            migrationBuilder.AddColumn<int>(
                name: "EmailID",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Workers",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Persons",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_EmailID",
                table: "WorkOrders",
                column: "EmailID");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Workers_ID",
                table: "Persons",
                column: "ID",
                principalTable: "Workers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Emails_EmailID",
                table: "WorkOrders",
                column: "EmailID",
                principalTable: "Emails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
