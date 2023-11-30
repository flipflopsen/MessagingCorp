using System.Reflection;

namespace MessagingCorp.Utils.Converters
{
    public class StaticDaoToBoConverter<TSource, TDestination>
    where TSource : class
    where TDestination : class, new()
    {
        public TDestination Convert(TSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var destination = new TDestination();

            foreach (var sourceProperty in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var destinationProperty = typeof(TDestination).GetProperty(sourceProperty.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }

            foreach (var sourceField in typeof(TSource).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var destinationField = typeof(TDestination).GetField(sourceField.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (destinationField != null)
                {
                    var value = sourceField.GetValue(source);
                    destinationField.SetValue(destination, value);
                }
            }

            return destination;
        }

        public TDestination ConvertWithPrivateProperties(TSource source, bool withReadOnlyProperties)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var destination = new TDestination();

            foreach (var sourceProperty in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var destinationProperty = typeof(TDestination).GetProperty(sourceProperty.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (destinationProperty != null && (withReadOnlyProperties || destinationProperty.CanWrite))
                {
                    var value = sourceProperty.GetValue(source);

                    SetSpecialFields(destination, sourceProperty.Name, value);
                    destinationProperty.SetValue(destination, value);
                }
            }

            foreach (var sourceField in typeof(TSource).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var destinationField = typeof(TDestination).GetField(sourceField.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (destinationField != null)
                {
                    var value = sourceField.GetValue(source);

                    SetSpecialFields(destination, sourceField.Name, value);
                    destinationField.SetValue(destination, value);
                }
            }

            return destination;
        }


        private void SetSpecialFields(TDestination destination, string fieldName, object value)
        {
            // Add logic for special fields here
            if (fieldName == "ActiveLobbyParticipations" || fieldName == "FriendList")
            {
                typeof(TDestination).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(destination, value);
            }
            // Add additional special fields if needed
        }
    }
}
