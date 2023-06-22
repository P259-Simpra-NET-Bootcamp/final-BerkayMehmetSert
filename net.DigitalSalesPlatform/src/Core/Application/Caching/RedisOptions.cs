namespace Core.Application.Caching;

public class RedisOptions
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string DefaultDatabase { get; set; }
    public string InstanceName { get; set; }
}