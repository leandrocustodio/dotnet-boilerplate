using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Persistence.Extensions
{
    public static class UintConverter
    {
        private static readonly ValueConverter<uint, string> _converter
                  = new(value => value.ToString(), value => uint.Parse(value));

        public static PropertyBuilder HasUintConversion(this PropertyBuilder builder)
        {
            builder.HasConversion(_converter);

            return builder;
        }
    }

}
