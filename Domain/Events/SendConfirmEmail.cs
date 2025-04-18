using Domain.Entities.Partners;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Domain.Events;

public class SendConfirmEmail(
    UserManager<AppUser> userManager) : INotificationHandler<AppUserEvent>
{
    public async Task Handle(AppUserEvent notification, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(notification.UserId.ToString());
        if (user is null) return;


    }
}
