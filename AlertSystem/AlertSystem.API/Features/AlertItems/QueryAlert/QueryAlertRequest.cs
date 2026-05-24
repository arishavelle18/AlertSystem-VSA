using AlertSystem.API.Common.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AlertSystem.API.Features.AlertItems.QueryAlert;

public record QueryAlertRequest() : IRequest<IList<QueryAlertResponseModel>>;
public record QueryAlertResponseModel(Guid Id, string Title, string Description, DateOnly ExpiryDate, bool IsNotified, int NotificationLeadDate, DateTimeOffset CreatedAt, string CreatedBy, DateTimeOffset? UpdatedAt, string? UpdatedBy);

internal sealed class QueryAlertRequestHandler(AppDbContext appDbContext) : IRequestHandler<QueryAlertRequest, IList<QueryAlertResponseModel>>
{
    public async Task<IList<QueryAlertResponseModel>> Handle(QueryAlertRequest request, CancellationToken cancellationToken)
    {
        var alertItems = appDbContext.AlertItemView;
        return  await alertItems.Select(item => new QueryAlertResponseModel(
            item.Id,
            item.Title,
            item.Description,
            item.ExpiryDate,
            item.IsNotified,
            item.NotificationLeadDate,
            item.CreatedAt,
            item.CreatedBy,
            item.UpdatedAt,
            item.UpdatedBy)).ToListAsync(cancellationToken);
    }
}
