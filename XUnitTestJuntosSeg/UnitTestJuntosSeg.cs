using Xunit;
using JuntoSeguros.Controllers;
using Microsoft.AspNetCore.Identity;
using JuntoSeguros.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestJuntosSeg
{

    public class UnitTestJuntosSeg
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        UsuariosController _controller;


        public UnitTestJuntosSeg(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            this._userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

            _controller = new UsuariosController(_userManager, _signInManager, _configuration);
        }

        [Fact]
        public void Get_index()
        {
            // Act
            var okResult = _controller.Get();
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_ListarTodos()
        {
            // Act
            var okResult = _controller.GetById("2132131");
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

    }
}
