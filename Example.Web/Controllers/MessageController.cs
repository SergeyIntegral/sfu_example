using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Example.DAL;
using Example.Services.Services;

namespace Example.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IDbContextProvider _provider;

        public MessageController(IMessageService messageService, IDbContextProvider provider)
        {
            _messageService = messageService;
            _provider = provider;
        }
    }
}