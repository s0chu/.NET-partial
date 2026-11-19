using System.Reflection;

namespace RoomHub.Api.Common.Cqrs;

public static class HandlerRegistrationExtensions
{
    // Registers every ICommandHandler<,> and IQueryHandler<,> in the assembly, without MediatR.
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlers = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                             i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
                .Select(i => new { Contract = i, Implementation = t }));

        foreach (var handler in handlers)
        {
            services.AddScoped(handler.Contract, handler.Implementation);
        }

        return services;
    }
}
