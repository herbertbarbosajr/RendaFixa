﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RendaFixa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Indexador = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Taxa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estoque = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            // Inserir dados iniciais na tabela Contas
            migrationBuilder.InsertData(
                table: "Contas",
                columns: new[] { "Id", "Saldo" },
                values: new object[,]
                {
                    { 1, 1000.00m },
                    { 2, 2000.00m }
                });

            // Inserir dados iniciais na tabela Produtos
            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Nome", "Indexador", "PrecoUnitario", "Taxa", "Estoque" },
                values: new object[,]
                {
                    { 1, "Produto 1", "IPCA", 100.00m, 5.00m, 50 },
                    { 2, "Produto 2", "CDI", 200.00m, 6.00m, 30 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contas");

            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
