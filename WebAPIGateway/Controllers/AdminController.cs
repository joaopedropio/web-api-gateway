using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPIGateway.Domain;
using WebAPIGateway.JSON;

namespace WebAPIGateway
{
    [Produces("application/json")]
    [Route("/admin/{*serviceName}")]
    [ApiController]
    public class AdminController: Controller
    {
        IServiceActions actions;

        public AdminController(IServiceRepository serviceRepo)
        {
            this.actions = new ServiceActions(serviceRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmin(string serviceName)
        {
            var json = await actions.GetService(serviceName);
            return JsonResultHelper.Parse(json);
        }

        [HttpPost]
        public async Task<IActionResult> PostAdmin()
        {
            var service = Service.ParseService(Request.Body);
            var json = await actions.PostService(service);
            return JsonResultHelper.Parse(json);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdmin(string serviceName)
        {
            var json = await actions.DeleteService(serviceName);
            return JsonResultHelper.Parse(json);
        }
    }
}