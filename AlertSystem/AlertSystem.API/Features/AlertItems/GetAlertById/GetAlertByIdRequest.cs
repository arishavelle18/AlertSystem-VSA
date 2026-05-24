using AlertSystem.API.Common.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AlertSystem.API.Features.AlertItems.GetAlertById;

public record GetAlertByIdRequest(Guid Id) : IRequest<GetAlertByIdResponse>;
public record GetAlertByIdResponse(Guid Id, string Title, string Description, DateOnly ExpiryDate, bool IsNotified, int NotificationLeadDate, DateTimeOffset CreatedAt, string CreatedBy, DateTimeOffset? UpdatedAt, string? UpdatedBy);

internal sealed class GetAlertByIdRequestHandler(AppDbContext appDbContext) : IRequestHandler<GetAlertByIdRequest, GetAlertByIdResponse>
{
    public async Task<GetAlertByIdResponse> Handle(GetAlertByIdRequest request, CancellationToken cancellationToken)
    {
        var alertItem = await appDbContext.AlertItemView.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (alertItem is null)
            throw new Exception($"Alert with id {request.Id} is not found");
        return new GetAlertByIdResponse(
            alertItem.Id,
            alertItem.Title,
            alertItem.Description,
            alertItem.ExpiryDate,
            alertItem.IsNotified,
            alertItem.NotificationLeadDate,
            alertItem.CreatedAt,
            alertItem.CreatedBy,
            alertItem.UpdatedAt,
            alertItem.UpdatedBy);
    }
}
