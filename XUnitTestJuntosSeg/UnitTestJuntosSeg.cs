using Xunit;
using JuntoSeguros.Controllers;
using Microsoft.AspNetCore.Identity;
using JuntoSeguros.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using JuntoSeguros.Services;

namespace XUnitTestJuntosSeg
{

    public class UnitTestJuntosSeg
    {

        private readonly IUsuarioService _service;
        UsuariosController _controller;


        public UnitTestJuntosSeg(IUsuarioService service)
        {
            _service = service;
            _controller = new UsuariosController(_service);
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
            var okResult = _controller.FindUserByEmail("revaurof@yahoo.com.br");
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
