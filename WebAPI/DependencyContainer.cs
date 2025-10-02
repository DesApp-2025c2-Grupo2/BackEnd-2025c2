using Application.Contracts.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Persistence.Repositorios;

namespace WebAPI;

public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Ejemplo:
        //services.AddScoped(typeof(IUserService), typeof(UserService));
        services.AddScoped(typeof(ISituacionTerapeuticaService), typeof(SituacionTerapeuticaService));
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Ejemplo:
        //services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        services.AddScoped(typeof(ISituacionTerapeuticaRepository), typeof(SituacionTerapeuticaRepository));
        return services;
    }

}
