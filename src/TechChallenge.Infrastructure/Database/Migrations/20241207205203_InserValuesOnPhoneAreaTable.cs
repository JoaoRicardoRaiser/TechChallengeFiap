using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechChallenge.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InserValuesOnPhoneAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO ""PhoneArea"" (""Code"", ""Region"") VALUES
                    -- Região Norte
                    ('91', 'Pará - Belém'),
                    ('92', 'Amazonas - Manaus'),
                    ('93', 'Pará - Santarém'),
                    ('94', 'Pará - Marabá'),
                    ('95', 'Roraima - Boa Vista'),
                    ('96', 'Amapá - Macapá'),
                    ('97', 'Amazonas - Interior'),
                    ('98', 'Maranhão - São Luís'),
                    ('99', 'Maranhão - Interior'),

                    -- Região Nordeste
                    ('71', 'Bahia - Salvador'),
                    ('73', 'Bahia - Ilhéus e Itabuna'),
                    ('74', 'Bahia - Juazeiro e Petrolina'),
                    ('75', 'Bahia - Feira de Santana'),
                    ('77', 'Bahia - Vitória da Conquista'),
                    ('81', 'Pernambuco - Recife'),
                    ('82', 'Alagoas - Maceió'),
                    ('83', 'Paraíba - João Pessoa'),
                    ('84', 'Rio Grande do Norte - Natal'),
                    ('85', 'Ceará - Fortaleza'),
                    ('86', 'Piauí - Teresina'),
                    ('87', 'Pernambuco - Interior'),
                    ('88', 'Ceará - Interior'),
                    ('89', 'Piauí - Interior'),

                    -- Região Centro-Oeste
                    ('61', 'Distrito Federal - Brasília'),
                    ('62', 'Goiás - Goiânia'),
                    ('64', 'Goiás - Interior'),
                    ('65', 'Mato Grosso - Cuiabá'),
                    ('66', 'Mato Grosso - Interior'),
                    ('67', 'Mato Grosso do Sul - Campo Grande'),

                    -- Região Sudeste
                    ('11', 'São Paulo - Capital'),
                    ('12', 'São Paulo - Vale do Paraíba'),
                    ('13', 'São Paulo - Santos e Região'),
                    ('14', 'São Paulo - Bauru e Marília'),
                    ('15', 'São Paulo - Sorocaba'),
                    ('16', 'São Paulo - Ribeirão Preto'),
                    ('17', 'São Paulo - São José do Rio Preto'),
                    ('18', 'São Paulo - Presidente Prudente'),
                    ('19', 'São Paulo - Campinas'),
                    ('21', 'Rio de Janeiro - Capital'),
                    ('22', 'Rio de Janeiro - Interior Norte'),
                    ('24', 'Rio de Janeiro - Interior Sul'),
                    ('27', 'Espírito Santo - Vitória'),
                    ('28', 'Espírito Santo - Interior'),
                    ('31', 'Minas Gerais - Belo Horizonte'),
                    ('32', 'Minas Gerais - Juiz de Fora'),
                    ('33', 'Minas Gerais - Governador Valadares'),
                    ('34', 'Minas Gerais - Uberlândia'),
                    ('35', 'Minas Gerais - Pouso Alegre'),
                    ('37', 'Minas Gerais - Divinópolis'),
                    ('38', 'Minas Gerais - Montes Claros'),

                    -- Região Sul
                    ('41', 'Paraná - Curitiba'),
                    ('42', 'Paraná - Ponta Grossa'),
                    ('43', 'Paraná - Londrina'),
                    ('44', 'Paraná - Maringá'),
                    ('45', 'Paraná - Foz do Iguaçu'),
                    ('46', 'Paraná - Francisco Beltrão'),
                    ('47', 'Santa Catarina - Joinville'),
                    ('48', 'Santa Catarina - Florianópolis'),
                    ('49', 'Santa Catarina - Chapecó'),
                    ('51', 'Rio Grande do Sul - Porto Alegre'),
                    ('53', 'Rio Grande do Sul - Pelotas'),
                    ('54', 'Rio Grande do Sul - Caxias do Sul'),
                    ('55', 'Rio Grande do Sul - Santa Maria');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""PhoneArea"";");
        }
    }
}
