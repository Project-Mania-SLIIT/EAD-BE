using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }
        // GET: api/<StudentsController>
        [HttpGet]
        public ActionResult<List<Booking>> Get(string userId)
        {
            return bookingService.Get(userId);
        }


        // POST api/<StudentsController>
        [HttpPost]
        public ActionResult<Booking> Post([FromBody] Booking booking)
        {
            try
            {
                var createdBooking = bookingService.Create(booking);
                return CreatedAtAction(nameof(Get), new { id = createdBooking.Id }, createdBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return a 500 status code with the error message
            }
        }

        // POST api/<StudentsController>
        [HttpPut]
        public ActionResult<Booking> Update([FromBody] Booking booking)
        {
            try
            {
                var updatedBooking = bookingService.Update(booking);
                return CreatedAtAction(nameof(Get), new { id = updatedBooking.Id }, updatedBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return a 500 status code with the error message
            }
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {

            bookingService.Delete(id);

            return Ok($"Booking with Id = {id} deleted");
        }
    }
}
