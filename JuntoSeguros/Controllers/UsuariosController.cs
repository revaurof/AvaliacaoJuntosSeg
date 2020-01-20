using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JuntoSeguros.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JuntoSeguros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UsuariosController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }      

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Controlador UsuariosController :: WebApiUsuarios");
        }

        [HttpPost("Criar")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Nome = model.Nome, Cidade = model.Cidade };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return BuildToken(model);
            }
            else
            {                
                return BadRequest(result.Errors.ToList());                
            }            
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return BuildToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("ListarTodos")]
        public ActionResult<List<ApplicationUser>> GetAllUser()
        {
            var result = _userManager.Users;

            result.ToList().ForEach(x => { x.PasswordHash = null; x.EmailConfirmed = false; x.SecurityStamp = null; x.ConcurrencyStamp = null; });

            return Ok(result.ToList());

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Buscar")]
        public ActionResult<List<ApplicationUser>> FindUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = _userManager.Users.Where(x => x.Email == user.Email);

            result.ToList().ForEach(x => { x.PasswordHash = null; x.EmailConfirmed = false; x.SecurityStamp = null; x.ConcurrencyStamp = null; });

            return result.ToList();

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetById")]
        public ActionResult<ApplicationUser> GetById(string id)
        {
            var result = _userManager.Users.Where(x => x.Id == id);

            if (result != null)
            {
                result.ToList().ForEach(x => { x.PasswordHash = null; x.EmailConfirmed = false; x.SecurityStamp = null; x.ConcurrencyStamp = null; });

                return result.FirstOrDefault();
            }
            else
            {
                return BadRequest("Verifique o id!");
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Atualizar")]
        public async Task<ActionResult<string>> UpdateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { Id = model.id, Nome = model.Nome, Cidade = model.Cidade };

            var result = _userManager.Users.FirstOrDefault(x => x.Id == model.id);

            if (result != null)
            {
                result.Cidade = model.Cidade;
                result.Nome = model.Nome;
                await _userManager.UpdateAsync(result);
                return Ok("Atualizado com sucesso!");
            }
            else
            {
                return BadRequest("Verifique o id!");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("AtualizarSenha")]
        public async Task<ActionResult<string>> UpdatePasswordUser(string id, string senhaAntiga, string novaSenha)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                //result.PasswordHash = user.PasswordHash;
                //await _userManager.UpdateAsync(result);

                var result = await _userManager.ChangePasswordAsync(user, senhaAntiga, novaSenha);
                if (result.Succeeded)
                {
                    return Ok("Atualizado com sucesso!");
                }
                else
                {
                    return BadRequest(result.Errors.ToList());
                }
            }
            else
            {
                return BadRequest("Verifique o id!");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Excluir")]
        public async Task<ActionResult<string>> DeleteUser(string id)
        {
            var result = _userManager.Users.FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                await _userManager.DeleteAsync(result);

                return "Excluido com sucesso!";
            }

            return BadRequest("Verifique o id!");

        }

        private UserToken BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("Nome", "Vaurof"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}