using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashRegisterDetails");

            migrationBuilder.DropTable(
                name: "CashRegisters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashRegisters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyType = table.Column<int>(type: "int", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegisters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashRegisterDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashRegisterDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashRegisterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegisterDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashRegisterDetails_CashRegisters_CashRegisterId",
                        column: x => x.CashRegisterId,
                        principalTable: "CashRegisters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisterDetails_CashRegisterId",
                table: "CashRegisterDetails",
                column: "CashRegisterId");
        }
    }
}
