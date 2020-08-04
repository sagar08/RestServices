using Microsoft.EntityFrameworkCore;

namespace JwtAuthWithRefreshToken.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }
    }
}