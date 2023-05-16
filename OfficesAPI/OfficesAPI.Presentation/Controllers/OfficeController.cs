using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Presentation.Models.ErrorModels;
using OfficesAPI.Services.Abstraction;
using OfficesAPI.Services.Abstraction.Models;

namespace OfficesAPI.Presentation.Controllers
{
    [ApiController]
    [Route("offices")]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        public OfficeController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        /// <summary>
        /// Creates the office in the system
        /// </summary>
        /// <param name="model">Model for creating office</param>
        [HttpPost]
        [ProducesResponseType(typeof(OfficeDTO), 201)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> CreateOffice([FromForm] CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            var office = await _officeService.CreateAsync(model, cancellationToken);
            return Created(Request.GetDisplayUrl() + $"/{office.Id}", office);
        }


        /// <summary>
        /// Updates office with a specific id in database
        /// </summary>
        /// <param name="id">office id</param>
        /// <param name="model">model for updating</param>
        [HttpPut("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> UpdateOffice(Guid id, [FromForm] UpdateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await _officeService.UpdateAsync(id, model, cancellationToken);
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
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> UpdateOfficeStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default)
        {
            await _officeService.UpdateStatus(id, newStatus, cancellationToken);
            return Accepted();
        }

        /// <param name="pageNumber">number of page</param>
        /// <param name="pageSize">size of page</param>
        /// <returns>Information about offices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OfficeDTO>), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public IActionResult GetOffices(int pageNumber = 1, int pageSize = 10)
        {
            var offices = _officeService.GetOffices(pageNumber, pageSize);
            return Ok(offices);
        }

        /// <param name="id">Office id</param>
        /// <returns>info about an office with a specific id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OfficeDTO), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetOffice(Guid id, CancellationToken cancellationToken = default)
        {
            var office = await _officeService.GetOfficeAsync(id, cancellationToken);
            return Ok(office);
        }
    }
}