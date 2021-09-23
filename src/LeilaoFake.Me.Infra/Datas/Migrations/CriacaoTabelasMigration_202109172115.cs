using FluentMigrator;

namespace LeilaoFake.Me.Infra.Datas.Migrations
{
    [Migration(202109172115, "primeira criação das tabelas.")]
    public class CriacaoTabelasMigration_202109172115 : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("FK_LEILOES_USUARIOS");
            Delete.ForeignKey("FK_LANCES_LEILOES");
            Delete.ForeignKey("FK_LANCES_USUARIOS");
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
                .WithColumn("leiloadoporid").AsString(36).NotNullable()
                .WithColumn("lanceminimo").AsDecimal(10,2)
                .WithColumn("datainicio").AsDateTime().NotNullable()
                .WithColumn("datafim").AsDateTime().Nullable()
                .WithColumn("ispublico").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("status").AsInt32().Nullable()
                .WithColumn("lanceganhadorid").AsString(36).Nullable()
                .WithColumn("criadoem").AsDateTime().NotNullable()
                .WithColumn("alteradoem").AsDateTime().Nullable();

            Create.Table("lances")
                .WithColumn("id").AsString(36).NotNullable().PrimaryKey().WithDefaultValue(RawSql.Insert("gen_random_uuid()"))
                .WithColumn("valor").AsDecimal(10,2).NotNullable()
                .WithColumn("interessadoid").AsString(36).NotNullable()
                .WithColumn("leilaoid").AsString(36).NotNullable()
                .WithColumn("criadoem").AsDateTime().NotNullable();


            Create.ForeignKey("FK_LEILOES_USUARIOS")
                .FromTable("leiloes").ForeignColumn("leiloadoporid")
                .ToTable("usuarios").PrimaryColumn("id");

            Create.ForeignKey("FK_LANCES_LEILOES")
                .FromTable("lances").ForeignColumn("leilaoid")
                .ToTable("leiloes").PrimaryColumn("id");

            Create.ForeignKey("FK_LANCES_USUARIOS")
                .FromTable("lances").ForeignColumn("interessadoid")
                .ToTable("usuarios").PrimaryColumn("id");
            
        }
    }
}