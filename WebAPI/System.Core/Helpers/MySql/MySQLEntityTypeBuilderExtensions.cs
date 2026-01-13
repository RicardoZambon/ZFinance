using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.EntityFrameworkCore.Extensions;

namespace Niten.System.Core.Helpers.MySql
{
    internal static class MySQLEntityTypeBuilderExtensions
    {
        public static void HasCharSet(this EntityTypeBuilder builder, string charSet)
        {
            builder.ForMySQLHasCharset(charSet);
        }
    }
}