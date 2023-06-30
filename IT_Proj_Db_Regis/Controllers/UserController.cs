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
        public IActionResult Login(Login login) 
        {
            var userAvailable = _context.User.Where(u => u.Email == login.Email && u.Pwd == login.Pwd).FirstOrDefaultAsync();
            if (userAvailable != null) 
            {
                return Ok(new JwtClass(configuration).GenerateToken(
                    userAvailable.Result.UserID.ToString(),
                    userAvailable.Result.FirstName,
                    userAvailable.Result.LastName,
                    userAvailable.Result.Email,
                    userAvailable.Result.Mobile,
                    userAvailable.Result.Gender  
                    ));
            }
            return Ok("Failure");
        }
    }
}
