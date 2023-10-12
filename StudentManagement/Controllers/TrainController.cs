using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService trainService;
        private readonly IBookingService bookingService;

        public TrainController(ITrainService trainService, IBookingService bookingService)
        {
            this.trainService = trainService;
            this.bookingService = bookingService;
        }
        // GET: api/<StudentsController>
        [HttpGet]
        public ActionResult<List<Train>> Get()
        {
            return trainService.Get();
        }


        // POST api/<StudentsController>
        [HttpPost]
        public ActionResult<Train> Post([FromBody] Train train)
        {
            trainService.Create(train);

            return CreatedAtAction(nameof(Get), new { id = train.Id }, train);
        }

        // PUT api/train/status
        [HttpPut("status")]
        public ActionResult UpdateStatus([FromQuery] string id, [FromBody] Train t)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t.Status))
            {
                return BadRequest("Train Id and new status must be provided.");
            }

            var train = trainService.GetById(id);

            if (train == null)
            {
                return NotFound("Train not found.");
            }

            // Check if there are bookings for the train
            if (t.Status == "Deactive")
            {
                bool hasBookings = bookingService.HasBookingsForDateOrFuture(id, DateTime.Today);

                if (hasBookings)
                {
                    // There are bookings, so you cannot deactivate the train
                    return BadRequest("Train cannot be deactivated as there are bookings.");
                }
            }

            trainService.UpdateStatus(id, t.Status);

            return NoContent();
        }
    }
}
