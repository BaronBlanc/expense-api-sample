using ExpenseApi.Commands;
using ExpenseApi.Exceptions;
using ExpenseApi.Model;
using ExpenseApi.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Collections;

namespace ExpenseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        // GET: api/Expenses
        // This endpoint helps you to quickly get the infos of users that have been created
        [HttpGet]
        public IActionResult GetUsers(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(_userRepository.GetAllAsync().Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
