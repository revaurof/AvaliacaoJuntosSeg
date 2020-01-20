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
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

            _controller = new UsuariosController(_userManager, _signInManager, _configuration);
        }

        [Fact]
        public void UsuariosControllerGet()
        {
            // Act
            var okResult = _controller.Get();
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void UsuariosControllerListar()
        {
            // Act
            var okResult = _controller.GetAllUser();
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void UsuariosControllerBuscarPorId()
        {
            // Act
            var okResult = _controller.GetById("2132131");
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void UsuariosControllerBuscarPorEmail()
        {
            // Act
            var okResult = _controller.FindUser("revaurof@yahoo.com.br");
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void UsuariosControllerExcluir()
        {
            // Act
            var okResult = _controller.DeleteUser("2132131");
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        
    }
}
