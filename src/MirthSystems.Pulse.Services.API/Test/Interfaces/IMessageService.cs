namespace MirthSystems.Pulse.Services.API.Test.Interfaces
{
    using MirthSystems.Pulse.Services.API.Test.Models;

    public interface IMessageService
    {
        Message GetPublicMessage();
        Message GetProtectedMessage();
        Message GetAdminMessage();
    }
}
