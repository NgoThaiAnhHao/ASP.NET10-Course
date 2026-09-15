using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDatafordifficultiesandregions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulty",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("37142dad-1f06-478b-88bb-2d2a23bd7d21"), "Easy" },
                    { new Guid("416fb7e7-74ce-42cd-8187-bfe4b72cc266"), "Medium" },
                    { new Guid("a10f2874-f754-48fc-8782-fc1b4d88a85e"), "Hard" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("552126bf-e417-4a49-90f3-798ddaa3d411"), "WGN", "Wellington", "https://tse3.mm.bing.net/th/id/OIP.3lxQX-ol-rT4Sy0cJCl2MAHaEK?r=0&rs=1&pid=ImgDetMain&o=7&rm=3" },
                    { new Guid("5e7fbf0a-caec-4854-a5ce-e0cb5f99cbca"), "BOP", "Bay Of Plenty", null },
                    { new Guid("6e0e67e0-eddc-499f-8c74-e6bc4fde87c5"), "NSN", "Nelson", null },
                    { new Guid("b1e47b7a-b9b7-4269-a5f3-3dae2dc68f5c"), "STL", "SounthLand", null },
                    { new Guid("bda60134-8884-4c35-a919-9bce0470deb9"), "AKL", "Auckland", "https://tse2.mm.bing.net/th/id/OIP.PAlYd-juBzm50ed8uPN6JAHaE4?r=0&rs=1&pid=ImgDetMain&o=7&rm=3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulty",
                keyColumn: "Id",
                keyValue: new Guid("37142dad-1f06-478b-88bb-2d2a23bd7d21"));

            migrationBuilder.DeleteData(
                table: "Difficulty",
                keyColumn: "Id",
                keyValue: new Guid("416fb7e7-74ce-42cd-8187-bfe4b72cc266"));

            migrationBuilder.DeleteData(
                table: "Difficulty",
                keyColumn: "Id",
                keyValue: new Guid("a10f2874-f754-48fc-8782-fc1b4d88a85e"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("552126bf-e417-4a49-90f3-798ddaa3d411"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("5e7fbf0a-caec-4854-a5ce-e0cb5f99cbca"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("6e0e67e0-eddc-499f-8c74-e6bc4fde87c5"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("b1e47b7a-b9b7-4269-a5f3-3dae2dc68f5c"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("bda60134-8884-4c35-a919-9bce0470deb9"));
        }
    }
}
