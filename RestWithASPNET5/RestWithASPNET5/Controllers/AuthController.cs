using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET5.Business;
using RestWithASPNET5.Data.VO;

namespace RestWithASPNET5.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:ApiVersion}")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginBussines _loginBussines;

        public AuthController(ILoginBussines loginBussines)
        {
            _loginBussines = loginBussines;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult Signin([FromBody] UserVO user)
        {
            if (user == null) return BadRequest("Invalid client request");
            var token = _loginBussines.ValidateCredentials(user);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVO)
        {
            if (tokenVO == null) return BadRequest("Invalid client request");
            var token = _loginBussines.ValidateCredentials(tokenVO);
            if (token == null) return BadRequest("Invalid client request");
            return Ok(token);
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            var userName = User.Identity.Name;
            var result = _loginBussines.RevokeToken(userName);
            if (!result) return BadRequest("Invalid client request");
            return NoContent();
        }
    }
}
