using Api.Mappers;
using Api.Repositories;
using Api.Services;

namespace Api.Initialization;

public static class IocRegistrar
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        RegisterDataAccessLayer(services);
        RegisterMappers(services);
    }

    private static void RegisterDataAccessLayer(IServiceCollection services)
    {
        services
            .AddSingleton<BenefitsDbContext>(sp =>
            {
                var context = new BenefitsDbContext();
                var seeder = new DataSeeder(context);
                seeder.SeedDatabase();
                return context;
            })
            .AddSingleton<IDependentDataService, DependentDataService>()
            .AddSingleton<IEmployeeDataService, EmployeeDataService>()
            .AddSingleton<IDependentRepository, DependentRepository>()
            .AddSingleton<IEmployeeRepository, EmployeeRepository>()
            ;
    }

    private static void RegisterMappers(IServiceCollection services)
    {
        services
            .AddTransient<IGetDependentDtoMapper, GetDependentDtoMapper>()
            .AddTransient<IGetEmployeeDtoMapper, GetEmployeeDtoMapper>()
            ;
    }
}
