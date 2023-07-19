using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoWebAPI.Entities
{
    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property("Id").ValueGeneratedOnAdd();

            builder.Property("Label").IsRequired();
            builder.Property("Description").HasColumnType("VARCHAR(255)");
            builder.ToTable("Article").HasCheckConstraint("CK_Price", "Price BETWEEN 0 AND 1000");

        }
    }
}
