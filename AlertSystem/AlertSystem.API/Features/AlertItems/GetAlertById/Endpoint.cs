using Carter;
using MediatR;

namespace AlertSystem.API.Features.AlertItems.GetAlertById;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Routes.GetById, async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetAlertByIdRequest(id), cancellationToken);
            return Results.Ok(result);
        });
    }
}
