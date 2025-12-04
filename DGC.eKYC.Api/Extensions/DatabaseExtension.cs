namespace DGC.eKYC.Api.Extensions;

public static class DatabaseExtension
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<MobileTopUpMiniAppContext>(options =>
        //{
        //    options.UseSqlServer(
        //        configuration.GetConnectionString("DefaultDbConnection"),
        //        builder =>
        //        {
        //            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        //            builder.CommandTimeout(30);
        //            builder.MaxBatchSize(100); // Optimize batch operations
        //        });
        //});

        //services.AddSingleton<IConnectionMultiplexer>(
        //    ConnectionMultiplexer.Connect(configuration.GetConnectionString("DefaultRedisConnection") 
        //                                  ?? throw new InvalidOperationException("DefaultRedisConnection is empty")));
    }
}
