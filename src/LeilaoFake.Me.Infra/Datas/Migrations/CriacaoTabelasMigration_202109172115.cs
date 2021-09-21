using FluentMigrator;

namespace LeilaoFake.Me.Infra.Datas.Migrations
{
    [Migration(202109172115, "primeira criação das tabelas.")]
    public class CriacaoTabelasMigration_202109172115 : Migration
    {
        public override void Down()
        {
            Delete.Table("usuarios");
            Delete.Table("leiloes");
            Delete.Table("lances");
        }

        public override void Up()
        {
            Create.Table("usuarios")
                .WithColumn("id").AsString(36).NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
                .WithColumn("nome").AsAnsiString(80).NotNullable()
                .WithColumn("email").AsAnsiString(250).Unique().NotNullable()
                .WithColumn("criadoem").AsDateTime().NotNullable()
                .WithColumn("alteradoem").AsDateTime().Nullable();
            
            Create.Table("leiloes")
                .WithColumn("id").AsString(36).NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
                .WithColumn("titulo").AsString(250).NotNullable()
                .WithColumn("descricao").AsString().Nullable()
                .WithColumn("leiloadoPorId").AsString(36).NotNullable()
                .WithColumn("lanceMinimo").AsDecimal(10,2)
                .WithColumn("dataInicio").AsDateTime().NotNullable()
                .WithColumn("dataFim").AsDateTime().Nullable()
                .WithColumn("status").AsInt32().Nullable()
                .WithColumn("lanceGanhadorId").AsString(36).Nullable();

            Create.ForeignKey("FK_LEILOES_USUARIOS")
                .FromTable("leiloes").ForeignColumn("leiloadoPorId")
                .ToTable("usuarios").PrimaryColumn("id");

            Create.Table("lances")
                .WithColumn("id").AsString(36).NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
                .WithColumn("data").AsDateTime().NotNullable()
                .WithColumn("valor").AsDecimal(10,2).NotNullable()
                .WithColumn("interessadoId").AsString(36).NotNullable()
                .WithColumn("leilaoId").AsString(36).NotNullable();

            Create.ForeignKey("FK_LANCES_LEILOES")
                .FromTable("lances").ForeignColumn("leilaoId")
                .ToTable("leiloes").PrimaryColumn("id");

            Create.ForeignKey("FK_LANCES_USUARIOS")
                .FromTable("lances").ForeignColumn("interessadoId")
                .ToTable("usuarios").PrimaryColumn("id");
            
        }
    }
}