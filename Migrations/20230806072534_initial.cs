using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demoAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DSType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TripDetailType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripDetailType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DSAccount",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSAccount", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DSAccount_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DSItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DSItem_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DSTransaction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DSTypeID = table.Column<int>(type: "int", nullable: false),
                    DSAccountID = table.Column<int>(type: "int", nullable: false),
                    DSTransferOutID = table.Column<int>(type: "int", nullable: false),
                    DSItemID = table.Column<int>(type: "int", nullable: false),
                    DSItemSubID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSTransaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DSTransaction_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Todolist",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todolist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Todolist_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripDetail",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripID = table.Column<int>(type: "int", nullable: false),
                    TripDetailTypeID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TripDetail_TripDetailType_TripDetailTypeID",
                        column: x => x.TripDetailTypeID,
                        principalTable: "TripDetailType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripDetail_Trip_TripID",
                        column: x => x.TripID,
                        principalTable: "Trip",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DSItemSub",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DSItemID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSItemSub", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DSItemSub_DSItem_DSItemID",
                        column: x => x.DSItemID,
                        principalTable: "DSItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodolistDone",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TodolistID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodolistDone", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TodolistDone_Todolist_TodolistID",
                        column: x => x.TodolistID,
                        principalTable: "Todolist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DSAccount_MemberID",
                table: "DSAccount",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_DSItem_MemberID_Name",
                table: "DSItem",
                columns: new[] { "MemberID", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DSItemSub_DSItemID_Name",
                table: "DSItemSub",
                columns: new[] { "DSItemID", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DSTransaction_MemberID",
                table: "DSTransaction",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Todolist_MemberID",
                table: "Todolist",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_TodolistDone_TodolistID",
                table: "TodolistDone",
                column: "TodolistID");

            migrationBuilder.CreateIndex(
                name: "IX_TripDetail_TripDetailTypeID",
                table: "TripDetail",
                column: "TripDetailTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TripDetail_TripID",
                table: "TripDetail",
                column: "TripID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DSAccount");

            migrationBuilder.DropTable(
                name: "DSItemSub");

            migrationBuilder.DropTable(
                name: "DSTransaction");

            migrationBuilder.DropTable(
                name: "DSType");

            migrationBuilder.DropTable(
                name: "TodolistDone");

            migrationBuilder.DropTable(
                name: "TripDetail");

            migrationBuilder.DropTable(
                name: "DSItem");

            migrationBuilder.DropTable(
                name: "Todolist");

            migrationBuilder.DropTable(
                name: "TripDetailType");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "Member");
        }
    }
}
