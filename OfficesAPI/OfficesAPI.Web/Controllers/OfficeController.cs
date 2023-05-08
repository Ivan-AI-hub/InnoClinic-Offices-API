using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Services;
using OfficesAPI.Services.Models;
using OfficesAPI.Web.Models.ErrorModels;

namespace OfficesAPI.Web.Controllers
{
    [ApiController]
    [Route("offices")]
    public class OfficeController : ControllerBase
    {
        private OfficeService _officeService;
        public OfficeController(OfficeService officeService)
        {
            _officeService = officeService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateOfficeModel model)
        {
            var result = await _officeService.Create(model);
            if (!result.IsComplite)
            {
                return BadRequest(new ErrorDetails(400, result.Errors));
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetOffices(int pageNumber = 1, int pageSize = 10)
        {
            var offices = await _officeService.GetOfficesPageAsync(pageNumber, pageSize);
            return new JsonResult(offices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOffice(Guid id)
        {
            var office = await _officeService.GetOfficeAsync(id);
            return new JsonResult(office);
        }
    }
}