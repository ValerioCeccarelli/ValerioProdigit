
namespace ValerioProdigit.Api.Endpoints;

public static class EndpointsExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var type = typeof(Program);
        var endpointsControllers = type.Assembly.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IEndpointsMapper)) && x.IsClass && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointsMapper>();
        foreach (var controller in endpointsControllers)
        {
            controller.MapEndpoints(app);
        }
    }
}