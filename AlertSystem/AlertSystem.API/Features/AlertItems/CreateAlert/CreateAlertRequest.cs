using AlertSystem.API.Common.Database;
using AlertSystem.API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AlertSystem.API.Features.AlertItems.CreateAlert;

public record CreateAlertRequest(string Title, string Description, DateOnly ExpiryDate, int NotificationLeadDate) : IRequest<CreateAlertResponse>;

public record CreateAlertResponse(Guid Id);

internal sealed class CreateAlertRequestHandler(AppDbContext appDbContext) : IRequestHandler<CreateAlertRequest, CreateAlertResponse>
{
    public async Task<CreateAlertResponse> Handle(CreateAlertRequest request, CancellationToken cancellationToken)
    {
        var isNotUnique = await appDbContext.AlertItemView.AnyAsync(a => a.Title.ToLower() == request.Title.ToLower(), cancellationToken);

        if (isNotUnique)
            throw new Exception($"{request.Title} is already existing");

       var createAlert = AlertItem.Create(request.Title, request.Description, request.ExpiryDate, request.NotificationLeadDate);

        await appDbContext.AddAsync(createAlert, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new CreateAlertResponse(createAlert.Id);
    }
}