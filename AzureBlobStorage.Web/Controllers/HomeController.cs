using System.Diagnostics;
using AzureBlobStorage.Web.Models;
using AzureBlobStorage.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContainerService _containerService;
        private readonly IBlobService _blobService;

        public HomeController(IContainerService containerService, IBlobService blobService)
        {
            _containerService = containerService;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _containerService.GetAllContainerAndBlobs());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Images()
        {
            return View(await _blobService.GetAllBlobsWithUri("privatecontainer"));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
