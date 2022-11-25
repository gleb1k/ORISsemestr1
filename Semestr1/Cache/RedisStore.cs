using StackExchange.Redis;
namespace Semestr1;

public static class RedisStore
{
    private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new (
        () => ConnectionMultiplexer.Connect(new ConfigurationOptions 
        { 
            EndPoints =
            {
                "localhost:49153"
            } ,
            Password = "redispw"
        }));
    
    public static ConnectionMultiplexer Connection => LazyConnection.Value;

    public static IDatabase RedisCache => Connection.GetDatabase();
}