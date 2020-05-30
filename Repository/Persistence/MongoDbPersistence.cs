using Infra.Map;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Repository.Persistence
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            ProductMap.Configure();
            // Set Guid to CSharp style (with dash -)
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;
            
            // Conventions
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true)
            };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }
    }
}