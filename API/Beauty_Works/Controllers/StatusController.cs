using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.Status;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatus(CreateStatusRequestDto request)
        {
            // Map Domain to Dto
            var status = new Status
            {
                Name = request.Name
            };

            await statusRepository.CreateAsync(status);

            var response = new StatusDto
            {
                ID = status.ID,
                Name = status.Name
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStatus()
        {
            var statuses = await statusRepository.GetAllAsync();

            // Map Domain to Dto
            var response = new List<StatusDto>();
            foreach(var status in statuses)
            {
                response.Add(new StatusDto
                {
                    ID = status.ID,
                    Name = status.Name
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{statusID:int}")]
        public async Task<IActionResult> GetStatusByID([FromRoute] int statusID)
        {
            var existingStatus = await statusRepository.GetByID(statusID);

            if(existingStatus == null)
            {
                return NotFound();
            }

            // Map Domain to Dto
            var response = new StatusDto
            {
                ID = existingStatus.ID,
                Name = existingStatus.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{statusID:int}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int statusID, [FromBody] UpdateStatusRequestDto request)
        {
            var status = new Status
            {
                ID = statusID,
                Name = request.Name
            };

            status = await statusRepository.UpdateAsync(status);

            if(status == null)
            {
                return NotFound();
            }

            // Map Domain to dto
            var response = new StatusDto
            {
                ID = statusID,
                Name = status.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{statusID:int}")]
        public async Task<IActionResult> DeleteStatus([FromRoute] int statusID)
        {
            var status = await statusRepository.DeleteAsync(statusID);

            if(status == null)
            {
                return NotFound();
            }

            var response = new StatusDto
            {
                ID = status.ID,
                Name = status.Name
            };

            return Ok(response);
        }
    }
}
