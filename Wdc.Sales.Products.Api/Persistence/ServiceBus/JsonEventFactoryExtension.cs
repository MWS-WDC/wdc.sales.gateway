using System.Text.Json;
using System.Text.Json.Serialization;
using Wdc.Sales.Products.Api.Events;

namespace Wdc.Sales.Products.Api.Persistence.ServiceBus
{
    public static class JsonEventFactoryExtension
    {
        public static Event ToEvent(this JsonDocument json, string subject, ILogger logger)
        {
            try
            {
                var type = typeof(Event).Assembly.ExportedTypes.SingleOrDefault(x => x.Name == subject);

                return type is null
                    ? throw new TypeLoadException("Event type could not be loaded.")
                    : (Event?)json.Deserialize(type, JsonSerializerOptions)
                    ?? throw new InvalidOperationException("Event deserialization failed.");

            }
            catch (Exception e)
            {
                logger.LogError(e, "failed to deserialize event");

                throw;
            }
        }

        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

}


