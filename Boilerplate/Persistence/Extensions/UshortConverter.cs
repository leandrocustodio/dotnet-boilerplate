using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Persistence.Extensions
{
    public static class UshortConverter
    {
        private static readonly ValueConverter<ushort, string> _converter
                  = new(value => value.ToString(), value => ushort.Parse(value));

        public static PropertyBuilder HasUshortConversion(this PropertyBuilder builder)
        {
            builder.HasConversion(_converter);

            return builder;
        }
    }

}
