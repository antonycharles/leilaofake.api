using FluentMigrator;

namespace LeilaoFake.Me.Infra.Datas.Migrations
{
    [Migration(202109261804)]
    public class CriacaoTabelasMigration_202109261804 : Migration
    {
        public override void Down()
        {
            Delete.Column("role").FromTable("usuarios");
            Delete.ForeignKey("FK_LEILOES_IMAGENS");
            Delete.Table("leilaoimagens");

        }

        public override void Up()
        {
            Alter.Table("usuarios")
                .AddColumn("role")
                .AsAnsiString(80)
                .Nullable()
                .WithDefaultValue("default");

            Create.Table("leilaoimagens")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("leilaoid").AsString(36).NotNullable()
                .WithColumn("url").AsAnsiString(150).NotNullable();

            
            Create.ForeignKey("FK_LEILOES_IMAGENS")
                .FromTable("leilaoimagens").ForeignColumn("leilaoid")
                .ToTable("leiloes").PrimaryColumn("id");
        }
    }
}