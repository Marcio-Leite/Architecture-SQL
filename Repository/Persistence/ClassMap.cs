using MongoDB.Bson.Serialization;

namespace MongoDbRepository.Persistence
{
    public class ClassMap
    {
        public static void Configure<T>() where T : class
        {
            BsonClassMap.RegisterClassMap<T>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Description).SetIsRequired(true);
            });
        }
    }
}