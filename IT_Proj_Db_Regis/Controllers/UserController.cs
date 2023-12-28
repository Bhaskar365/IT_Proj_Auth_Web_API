using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using IT_Proj_Db_Regis.DbContextClass;
using Microsoft.AspNetCore.Authorization;
using IT_Proj_Db_Regis.Models;
using Microsoft.EntityFrameworkCore;
using IT_Proj_Db_Regis.JwtService;

namespace IT_Proj_Db_Regis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly appDbContext _context;

        public UserController(IConfiguration configuration, appDbContext _context) 
        {
            this.configuration = configuration;
            this._context = _context;
        }
        //test comment
        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult Create(User user) 
        {
            if(_context.User.Where(u => u.Email == user.Email).FirstOrDefaultAsync() != null) 
            {
                return Ok("Already Exists");
            }

            user.MemberSince = DateTime.Now;
            _context.User.Add(user);
            _context.SaveChangesAsync();
            return Ok("Success");
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login(Login login) 
        {
            var userAvailable = await _context.User.FirstOrDefaultAsync(u => u.Email == login.Email && u.Pwd == login.Pwd);

            if (userAvailable != null)
            {
                return Ok(new JwtClass(configuration).GenerateToken(
                    userAvailable.UserID.ToString(),
                    userAvailable.FirstName,
                    userAvailable.LastName,
                    userAvailable.Email,
                    userAvailable.Mobile,
                    userAvailable.Gender
                ));
            }
            else
            {
                return Ok("Failure");
                //return BadRequest("Failure");
            }
        }
    }
}

