using Carter;
using MediatR;

namespace AlertSystem.API.Features.AlertItems.DeleteAlert;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(Routes.Delete, async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var req = new DeleteAlertRequest(id);
            var res = await sender.Send(req, cancellationToken);
            return Results.Ok(res);
        });
    }
}
