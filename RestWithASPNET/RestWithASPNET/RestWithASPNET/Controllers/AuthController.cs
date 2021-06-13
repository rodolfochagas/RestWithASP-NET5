using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]/v{version:apiVersion}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult Signin([FromBody] UserVO userVO)
        {
            if (userVO == null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(userVO);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVO)
        {
            if (tokenVO == null) return BadRequest("Invalid client request");
            var newTokenVO = _loginBusiness.ValidateCredentials(tokenVO);
            if (newTokenVO == null) return BadRequest("Invalid client request");
            return Ok(newTokenVO);
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            //Não preciso de parâmetro porque estou autenticado (authorize bearer) e posso pegar o userName, por exemplo, do jwt
            var userName = User.Identity.Name;
            var isRevoked = _loginBusiness.RevokeToken(userName);

            if (!isRevoked) return BadRequest("Invalid client request");
            return NoContent();
        }


    }
}
