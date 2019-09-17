﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_GarmentSewingIns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSewingIns",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SewingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    LoadingId = table.Column<Guid>(nullable: false),
                    LoadingNo = table.Column<string>(nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    SewingInDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSewingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSewingInItems",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SewingInId = table.Column<Guid>(nullable: false),
                    LoadingItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 100, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Color = table.Column<string>(maxLength: 100, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSewingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSewingInItems_GarmentSewingIns_SewingInId",
                        column: x => x.SewingInId,
                        principalTable: "GarmentSewingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSewingInItems_SewingInId",
                table: "GarmentSewingInItems",
                column: "SewingInId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSewingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSewingIns");
        }
    }
}
