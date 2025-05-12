namespace MirthSystems.Pulse.Services.API.Test.Services
{
    using MirthSystems.Pulse.Services.API.Test.Interfaces;
    using MirthSystems.Pulse.Services.API.Test.Models;

    public class MessageService : IMessageService
    {
        public Message GetAdminMessage()
        {
            return new Message { Text = "This is an admin message." };
        }

        public Message GetProtectedMessage()
        {
            return new Message { Text = "This is a protected message." };
        }

        public Message GetPublicMessage()
        {
            return new Message { Text = "This is a public message." };
        }
    }
}
