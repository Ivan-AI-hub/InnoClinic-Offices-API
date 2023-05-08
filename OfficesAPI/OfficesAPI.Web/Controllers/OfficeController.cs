using Microsoft.AspNetCore.Http.Extensions;
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

        /// <summary>
        /// Creates the office in the system
        /// </summary>
        /// <param name="model">Model for creating office</param>
        [HttpPost]
        [ProducesResponseType(typeof(OfficeDTO),201)]
        [ProducesResponseType(typeof(ErrorDetails),400)]
        public async Task<IActionResult> CreateOffice(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            var result = await _officeService.CreateAsync(model, cancellationToken);
            if (!result.IsComplite)
            {
                return BadRequest(new ErrorDetails(400, result.Errors));
            }
            return Created(Request.GetDisplayUrl() + $"/{result.Value.Id}", result.Value);
        }


        /// <summary>
        /// Updates office with a specific id in database
        /// </summary>
        /// <param name="id">office id</param>
        /// <param name="model">model for updating</param>
        [HttpPut("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> UpdateOffice(Guid id, [FromForm] UpdateOfficeModel model, CancellationToken cancellationToken = default)
        {
            var result = await _officeService.UpdateAsync(id, model, cancellationToken);
            if (!result.IsComplite)
            {
                return BadRequest(new ErrorDetails(400, result.Errors));
            }
            return Accepted();
        }

        /// <summary>
        /// Updates status for office with a specific id
        /// </summary>
        /// <param name="id">Office id</param>
        /// <param name="newStatus">new status</param>
        [HttpPut("{id}/status")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> UpdateOfficeStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default)
        {
            var result = await _officeService.UpdateStatus(id, newStatus, cancellationToken);
            if (!result.IsComplite)
            {
                return BadRequest(new ErrorDetails(400, result.Errors));
            }
            return Accepted();
        }

        /// <param name="pageNumber">number of page</param>
        /// <param name="pageSize">size of page</param>
        /// <returns>Information about offices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OfficeDTO>),200)]
        public async Task<IActionResult> GetOffices(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var offices = await _officeService.GetOfficesPageAsync(pageNumber, pageSize, cancellationToken);
            return new JsonResult(offices);
        }

        /// <param name="id">Office id</param>
        /// <returns>info about an office with a specific id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OfficeDTO), 200)]
        public async Task<IActionResult> GetOffice(Guid id, CancellationToken cancellationToken = default)
        {
            var office = await _officeService.GetOfficeAsync(id, cancellationToken);
            return new JsonResult(office);
        }
    }
}