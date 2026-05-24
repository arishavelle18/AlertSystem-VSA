using Carter;
using MediatR;

namespace AlertSystem.API.Features.AlertItems.QueryAlert;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Routes.GetAll, async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new QueryAlertRequest(), cancellationToken);
            return Results.Ok(result);
        });
    }
}
