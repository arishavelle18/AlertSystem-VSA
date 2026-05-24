using AlertSystem.API.Common.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AlertSystem.API.Features.AlertItems.UpdateAlert;

public record UpdateAlertRequest(Guid Id, UpdateAlertRequestModel UpdateAlertRequestModel) : IRequest<UpdateAlertResponse>;
public record UpdateAlertRequestModel(string Title, string Description, DateOnly ExpiryDate, bool IsNotified, int NotificationLeadDate);
public record UpdateAlertResponse(Guid Id);

internal sealed class UpdateAlertRequestHandler(AppDbContext appDbContext) : IRequestHandler<UpdateAlertRequest, UpdateAlertResponse>
{
    public async Task<UpdateAlertResponse> Handle(UpdateAlertRequest request, CancellationToken cancellationToken)
    {
        var getAlertItem = await appDbContext.AlertItemView.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (getAlertItem is null)
            throw new Exception("Not found with the given credentials");

        //check if the alert item title is being updated to a title that already exists for another alert item
        var checkIfTitleExists = await appDbContext.AlertItemView.FirstOrDefaultAsync(a => a.Title == request.UpdateAlertRequestModel.Title && a.Id != request.Id, cancellationToken);
        if(checkIfTitleExists is not null)
            throw new Exception($"w{request.UpdateAlertRequestModel.Title} is already existing");

        getAlertItem.Update(request.UpdateAlertRequestModel.Title, request.UpdateAlertRequestModel.Description, request.UpdateAlertRequestModel.ExpiryDate, request.UpdateAlertRequestModel.IsNotified, request.UpdateAlertRequestModel.NotificationLeadDate);
        appDbContext.Update(getAlertItem);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new UpdateAlertResponse(getAlertItem.Id);
    }
}