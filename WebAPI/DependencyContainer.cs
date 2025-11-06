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
        services.AddScoped(typeof(IPlanMedicoService), typeof(PlanMedicoService));
        services.AddScoped(typeof(IEspecialidadService), typeof(EspecialidadService));
        services.AddScoped(typeof(IPersonaService), typeof(PersonaService));
        services.AddScoped(typeof(IAfiliadoService), typeof(AfiliadoService));
        services.AddScoped(typeof(IReporteService), typeof(ReporteService));
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Ejemplo:
        //services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        services.AddScoped(typeof(ISituacionTerapeuticaRepository), typeof(SituacionTerapeuticaRepository));
        services.AddScoped(typeof(IPlanMedicoRepository), typeof(PlanMedicoRepository));
        services.AddScoped(typeof(IEspecialidadRepository), typeof(EspecialidadRepository));
        services.AddScoped(typeof(IAfiliadoRepository), typeof(AfiliadoRepository));
        services.AddScoped(typeof(IPersonaRepository), typeof(PersonaRepository));
        return services;
    }

}
