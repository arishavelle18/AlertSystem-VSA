using Carter;
using MediatR;

namespace AlertSystem.API.Features.AlertItems.UpdateAlert;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(Routes.Update, async (Guid id, UpdateAlertRequestModel updateAlertRequestModel, ISender sender, CancellationToken cancellationToken) =>
        {
            var req = new UpdateAlertRequest(id, updateAlertRequestModel);
            var res = await sender.Send(req, cancellationToken);
            return Results.Ok(res);
        });
    }
}
