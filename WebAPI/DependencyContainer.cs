namespace WebAPI;

public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Ejemplo:
        //services.AddScoped(typeof(IUserService), typeof(UserService));
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Ejemplo:
        //services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        return services;
    }

}
