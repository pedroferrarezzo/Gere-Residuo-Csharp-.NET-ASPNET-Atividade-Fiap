using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Br.Com.Fiap.Gere.Residuo.Migrations
{
    /// <inheritdoc />
    public partial class FiapGereResiduoSchemaV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_GR_BAIRRO",
                columns: table => new
                {
                    bairro_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    bairro_nome = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    quantidade_lixeiras = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    peso_medio_lixeiras_kg = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    percentual_coleta_lixo_bairro = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    bairro_esta_disponivel = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GR_BAIRRO", x => x.bairro_id);
                });

            migrationBuilder.CreateTable(
                name: "T_GR_CAMINHAO",
                columns: table => new
                {
                    caminhao_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    caminhao_placa = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    data_fabricacao = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    caminhao_marca = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    caminhao_modelo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    caminhao_esta_disponivel = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GR_CAMINHAO", x => x.caminhao_id);
                });

            migrationBuilder.CreateTable(
                name: "T_GR_MOTORISTA",
                columns: table => new
                {
                    motorista_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    motorista_nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    motorista_cpf = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    motorista_nr_celular = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    motorista_nr_celular_ddd = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    motorista_nr_celular_ddi = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    motorista_esta_disponivel = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GR_MOTORISTA", x => x.motorista_id);
                });

            migrationBuilder.CreateTable(
                name: "T_GR_USUARIO",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    usuario_nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    usuario_email = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    usuario_senha = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    usuario_role = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GR_USUARIO", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "T_GR_MORADOR",
                columns: table => new
                {
                    morador_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    bairro_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    morador_nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    morador_email = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GR_MORADOR", x => x.morador_id);
                    table.ForeignKey(
                        name: "FK_T_GR_MORADOR_BAIRRO",
                        column: x => x.bairro_id,
                        principalTable: "T_GR_BAIRRO",
                        principalColumn: "bairro_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_GR_AGENDA",
                columns: table => new
                {
                    agenda_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    caminhao_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    motorista_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    bairro_id = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    dia_coleta_de_lixo = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    tipo_residuo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    status_coleta_de_lixo_agendada = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    peso_coletado_de_lixo_kg = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GR_AGENDA", x => x.agenda_id);
                    table.ForeignKey(
                        name: "FK_T_GR_AGENDA_BAIRRO",
                        column: x => x.bairro_id,
                        principalTable: "T_GR_BAIRRO",
                        principalColumn: "bairro_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_GR_AGENDA_CAMINHAO",
                        column: x => x.caminhao_id,
                        principalTable: "T_GR_CAMINHAO",
                        principalColumn: "caminhao_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_GR_AGENDA_MOTORISTA",
                        column: x => x.motorista_id,
                        principalTable: "T_GR_MOTORISTA",
                        principalColumn: "motorista_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_GR_NOTIFICACAO",
                columns: table => new
                {
                    notificacao_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    agenda_id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GR_NOTIFICACAO", x => x.notificacao_id);
                    table.ForeignKey(
                        name: "FK_T_GR_NOTIFICACAO_AGENDA",
                        column: x => x.agenda_id,
                        principalTable: "T_GR_AGENDA",
                        principalColumn: "agenda_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_AGENDA_bairro_id",
                table: "T_GR_AGENDA",
                column: "bairro_id");

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_AGENDA_caminhao_id",
                table: "T_GR_AGENDA",
                column: "caminhao_id");

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_AGENDA_motorista_id",
                table: "T_GR_AGENDA",
                column: "motorista_id");

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_BAIRRO_bairro_nome",
                table: "T_GR_BAIRRO",
                column: "bairro_nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_CAMINHAO_caminhao_placa",
                table: "T_GR_CAMINHAO",
                column: "caminhao_placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_MORADOR_bairro_id",
                table: "T_GR_MORADOR",
                column: "bairro_id");

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_MORADOR_morador_email",
                table: "T_GR_MORADOR",
                column: "morador_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_MOTORISTA_motorista_cpf",
                table: "T_GR_MOTORISTA",
                column: "motorista_cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_MOTORISTA_motorista_nr_celular",
                table: "T_GR_MOTORISTA",
                column: "motorista_nr_celular",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_NOTIFICACAO_agenda_id",
                table: "T_GR_NOTIFICACAO",
                column: "agenda_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_GR_USUARIO_usuario_email",
                table: "T_GR_USUARIO",
                column: "usuario_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_GR_MORADOR");

            migrationBuilder.DropTable(
                name: "T_GR_NOTIFICACAO");

            migrationBuilder.DropTable(
                name: "T_GR_USUARIO");

            migrationBuilder.DropTable(
                name: "T_GR_AGENDA");

            migrationBuilder.DropTable(
                name: "T_GR_BAIRRO");

            migrationBuilder.DropTable(
                name: "T_GR_CAMINHAO");

            migrationBuilder.DropTable(
                name: "T_GR_MOTORISTA");
        }
    }
}
