namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Services.API.Controllers.Base;
    using MirthSystems.Pulse.Services.API.Test.Interfaces;

    using MirthSystems.Pulse.Services.API.Test.Models;

    [Route("api/messages")]
    public class MessagesController : ApiController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("public")]
        public ActionResult<Message> GetPublicMessage()
        {
            return _messageService.GetPublicMessage();
        }

        [HttpGet("protected")]
        [Authorize]
        public ActionResult<Message> GetProtectedMessage()
        {
            return _messageService.GetProtectedMessage();
        }
    }
}
