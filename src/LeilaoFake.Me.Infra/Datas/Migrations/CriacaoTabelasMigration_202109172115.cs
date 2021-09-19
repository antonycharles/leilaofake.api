using FluentMigrator;

namespace LeilaoFake.Me.Infra.Datas.Migrations
{
    [Migration(202109172115, "primeira criação das tabelas.")]
    public class CriacaoTabelasMigration_202109172115 : Migration
    {
        public override void Down()
        {
            Delete.Table("Usuarios");
            Delete.Table("leiloes");
            Delete.Table("lances");
        }

        public override void Up()
        {
            Create.Table("Usuarios")
                .WithColumn("Id").AsString(36).NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
                .WithColumn("Nome").AsAnsiString(50).NotNullable()
                .WithColumn("Email").AsAnsiString(250).NotNullable();
            
            Create.Table("leiloes")
                .WithColumn("Id").AsString(36).NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
                .WithColumn("Titulo").AsString(250).NotNullable()
                .WithColumn("Descricao").AsString().Nullable()
                .WithColumn("LeiloadoPorId").AsString(36).NotNullable()
                .WithColumn("LanceMinimo").AsDecimal(10,2)
                .WithColumn("DataInicio").AsDateTime().NotNullable()
                .WithColumn("DataFim").AsDateTime().Nullable()
                .WithColumn("Status").AsInt32().Nullable()
                .WithColumn("LanceGanhadorId").AsString(36).Nullable();

            Create.ForeignKey("FK_LEILOES_USUARIOS")
                .FromTable("leiloes").ForeignColumn("LeiloadoPorId")
                .ToTable("Usuarios").PrimaryColumn("Id");

            Create.Table("lances")
                .WithColumn("Id").AsString(36).NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
                .WithColumn("Data").AsDateTime().NotNullable()
                .WithColumn("Valor").AsDecimal(10,2).NotNullable()
                .WithColumn("InteressadoId").AsString(36).NotNullable()
                .WithColumn("LeilaoId").AsString(36).NotNullable();

            Create.ForeignKey("FK_LANCES_LEILOES")
                .FromTable("lances").ForeignColumn("LeilaoId")
                .ToTable("leiloes").PrimaryColumn("Id");

            Create.ForeignKey("FK_LANCES_USUARIOS")
                .FromTable("lances").ForeignColumn("InteressadoId")
                .ToTable("Usuarios").PrimaryColumn("Id");
            
        }
    }
}