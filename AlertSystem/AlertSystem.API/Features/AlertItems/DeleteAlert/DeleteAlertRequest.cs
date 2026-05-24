using AlertSystem.API.Common.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AlertSystem.API.Features.AlertItems.DeleteAlert;

public record DeleteAlertRequest(Guid Id) : IRequest<DeleteAlertResponse>;
public record DeleteAlertResponse(Guid Id);

internal sealed class DeleteAlertHandler(AppDbContext appDbContext) : IRequestHandler<DeleteAlertRequest, DeleteAlertResponse>
{
    public async Task<DeleteAlertResponse> Handle(DeleteAlertRequest request, CancellationToken cancellationToken)
    {
        var getData = await appDbContext.AlertItems.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (getData is null)
            throw new Exception("Not found with the given credentials");
        appDbContext.Remove(getData);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new DeleteAlertResponse(request.Id);
    }
}