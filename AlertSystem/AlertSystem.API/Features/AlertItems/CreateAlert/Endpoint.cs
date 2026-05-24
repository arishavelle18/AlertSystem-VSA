using Carter;
using MediatR;

namespace AlertSystem.API.Features.AlertItems.CreateAlert;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(Routes.Create, async (CreateAlertRequest createAlertRequest,ISender sender,CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(createAlertRequest, cancellationToken);
            return Results.Ok(result);
        });
    }
}

