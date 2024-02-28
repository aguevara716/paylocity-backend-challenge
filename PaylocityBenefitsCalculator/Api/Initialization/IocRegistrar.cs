using Api.Mappers;
using Api.Repositories;
using Api.Services;
using Api.Validators;

namespace Api.Initialization;

public static class IocRegistrar
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        RegisterDataAccessLayer(services);
        RegisterMappers(services);
        RegisterValidators(services);
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
            // Dependent Mappers
            .AddTransient<IGetDependentDtoMapper, GetDependentDtoMapper>()
            .AddTransient<ICreateDependentDtoMapper, CreateDependentDtoMapper>()

            // Employee Mappers
            .AddTransient<IGetEmployeeDtoMapper, GetEmployeeDtoMapper>()
            .AddTransient<ICreateEmployeeDtoMapper, CreateEmployeeDtoMapper>()
            ;
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services
            .AddTransient<IEmployeeValidator, EmployeeValidator>()
            ;
    }
}
