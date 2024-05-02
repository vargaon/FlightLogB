namespace FlightLogNet.Tests
{
    using Microsoft.Extensions.DependencyInjection;

    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            InjectConfiguration.Initialization(services, isInTestEnvironment: true);
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}
