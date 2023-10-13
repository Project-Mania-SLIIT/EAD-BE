using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserssController : ControllerBase
    {
        private readonly IUserService userService;

        public UserssController(IUserService userService)
        {
            this.userService = userService;
        }
        // GET: api/<StudentsController>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            //return all users
            return userService.Get();
        }


        // PUT api/<UserssController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] User u)
        {
            //check user availability
            var user = userService.GetById(id);

            if (user == null)
            {
                return NotFound("User not found.");
            } 
            //if user available update user
            userService.Update(id, u);

            return NoContent();



        }


        // POST api/<UserssController>/login
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] UserLoginRequest user)
        {
            // Authenticate the user based on the NIC and password
            // You can implement your authentication logic here
            // If authentication is successful, return a token or success message
            // If authentication fails, return an error message
            // Example:
            if (userService.AuthenticateUser(user.NIC, user.Password, user.Role))
            {
                // Get the user details, such as userId and role, from your user service
                var userDetails = userService.GetUserDetails(user.NIC);

                if (userDetails != null)
                {
                    // Authentication successful, return user details along with a success message
                    var response = new
                    {
                        message = "Authentication successful",
                        userId = userDetails.Id,
                        role = userDetails.Role,
                        name = userDetails.Name
                    };
                    return Ok(response);
                }
                else
                {
                    // Handle the case where user details cannot be retrieved (optional)
                    return StatusCode(500, "Unable to retrieve user details");
                }
            }
            else
            {
                // Authentication failed, return an error message
                var errorResponse = new { error = new { message = "failed" } };
                return BadRequest(errorResponse);
            }
        }

        // POST api/<UserssController>/register
        [HttpPost("register")]
        public ActionResult<User> Register([FromBody]  UserRegistrationRequest user)
        {
          
                // Validate registration data (e.g., check for required fields)
                if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.NIC) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Mobile) || string.IsNullOrEmpty(user.Role))
                {
                    return BadRequest("Invalid registration data.");
                }
            //check user availability
             User u = userService.GetUserDetails(user.NIC.ToString());

            if(u != null)
            {
                return BadRequest("Nic already Registered");
            }


                // Create the user and store the hashed password
                User newUser = userService.RegisterUser(user.Name , user.NIC, user.Password, user.Mobile, user.Email, user.Role);

                // Return a success response
                return Ok("Registration successful...");
     
        }

        // PUT api/train/status
        [HttpPut("status")]
        public ActionResult UpdateStatus([FromQuery] string id, [FromBody] Train t)
        {
            //add validation
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t.Status))
            {
                return BadRequest("User Id and new status must be provided.");
            }
            //check user availability
            var user = userService.GetById(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            //if user available update status
            userService.UpdateStatus(id, t.Status);

            return NoContent();
        }

    }
}
