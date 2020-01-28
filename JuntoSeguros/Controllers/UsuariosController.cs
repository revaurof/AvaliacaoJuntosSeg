using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JuntoSeguros.Models;
using JuntoSeguros.Services;
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
        private readonly IUsuarioService _service;         
        public UsuariosController(IUsuarioService service)
        {
            _service = service;
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

            var result = await _service.CreateAsync(user, model.Password);
            if (result != null && result.Succeeded)
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
            var result = await _service.PasswordSignInAsync(userInfo);
            if (result != null && result.Succeeded)
            {
                return BuildToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário não encontrado!");
                return BadRequest(ModelState);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Listar")]
        public ActionResult<List<ApplicationUser>> GetAllUser()
        {
            var result = _service.GetAllUser();

            result.ToList().ForEach(x => { x.PasswordHash = null; x.EmailConfirmed = false; x.SecurityStamp = null; x.ConcurrencyStamp = null; });

            return Ok(result.ToList());

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("BuscarPorEmail")]
        public ActionResult<ApplicationUser> FindUserByEmail(string email)
        {
            var result = _service.FindUserByEmail(email).Result;

            if (result != null)
            {
                result.PasswordHash = null;
                result.EmailConfirmed = false;
                result.SecurityStamp = null; 
                result.ConcurrencyStamp = null; 
                return Ok(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "E-mail não encontrado!");
                return BadRequest(ModelState);
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("BuscarPorId")]
        public ActionResult<ApplicationUser> GetById(string id)
        {
            var result = _service.GetById(id).Result;

            if (result != null)
            {                
                result.PasswordHash = null;
                result.EmailConfirmed = false;
                result.SecurityStamp = null;
                result.ConcurrencyStamp = null;
                return Ok(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Id não encontrado!");
                return BadRequest(ModelState);
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Atualizar")]
        public async Task<ActionResult<string>> UpdateUser([FromBody] UserInfo model)
        {
            var result = _service.GetById(model.id).Result;

            if (result != null)
            {
                result.Cidade = model.Cidade;
                result.Nome = model.Nome;
                await _service.UpdateAsync(result);
                return Ok("Atualizado com sucesso!");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Id não encontrado!");
                return BadRequest(ModelState);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("AtualizarSenha")]
        public async Task<ActionResult<string>> UpdatePasswordUser([Required]string id, string senhaAntiga, string novaSenha)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var result = await _service.UpdatePasswordUser(id, senhaAntiga, novaSenha);
                if (result != null && result.Succeeded)
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
                ModelState.AddModelError(string.Empty, "Id não encontrado!");
                return BadRequest(ModelState);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Excluir")]
        public async Task<ActionResult<string>> DeleteUser(string id)
        {
            var result = await _service.DeleteUser(id);

            if (result != null && result.Succeeded)
            {
                return Ok("Excluido com sucesso!");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Id não encontrado!");
                return BadRequest(ModelState);
            }

        }

        private UserToken BuildToken(UserInfo userInfo)
        {
            return _service.BuildToken(userInfo);
        }
    }
}