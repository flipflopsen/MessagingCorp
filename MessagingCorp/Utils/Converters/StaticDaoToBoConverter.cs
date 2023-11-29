using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Utils.Converters
{
    public class StaticDaoToBoConverter<TSource, TDestination>
    where TSource : class
    where TDestination : class, new()
    {
        public TDestination Convert(TSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var destination = new TDestination();

            foreach (var sourceProperty in typeof(TSource).GetProperties())
            {
                var destinationProperty = typeof(TDestination).GetProperty(sourceProperty.Name);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }

            return destination;
        }
    }
}
