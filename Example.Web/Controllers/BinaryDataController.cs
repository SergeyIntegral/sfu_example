using System;
using System.Web.Mvc;
using Example.DAL;
using Example.Services.Context;
using Example.Services.Services;
using Example.Web.Infrastructure.Filters;

namespace Example.Web.Controllers
{
    public class BinaryDataController : Controller
    {
        private readonly IBinaryDataService _binaryDataService;
        private readonly IDbContextProvider _provider;

        public BinaryDataController(IBinaryDataService binaryDataService, IDbContextProvider provider)
        {
            _binaryDataService = binaryDataService;
            _provider = provider;
        }

        [NoCache]
        public FileContentResult GetFile(int? id)
        {
            try
            {
                if (!id.HasValue) return null;

                var file = _binaryDataService.FindById(id.Value);

                return File(file.Data, file.MimeType);
            }
            catch (Exception ex)
            {
                ExampleContext.Log.ErrorFormat("BinaryDataController.GetFile: {0}", ex);
                return null;
            }
        }
    }
}