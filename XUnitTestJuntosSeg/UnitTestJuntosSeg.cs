using Xunit;
using JuntoSeguros.Controllers;
using Microsoft.AspNetCore.Identity;
using JuntoSeguros.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using JuntoSeguros.Services;
using Moq;

namespace XUnitTestJuntosSeg
{

    public class UnitTestJuntosSeg
    {
        UsuariosController _controller;
        public UnitTestJuntosSeg()
        {
            var mockService = new Mock<IUsuarioService>();//Mock
            _controller = new UsuariosController(mockService.Object);
        }

        [Fact]
        public void Get()
        {
            var okResult = _controller.Get();
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        //[Fact]
        //public void CreateUser()
        //{
        //    UserInfo model = new UserInfo
        //    {
        //        Cidade = "Itararé",
        //        Email = "revaurof@yahoo.com.br",
        //        Nome = "Renato",
        //        Password = "Re5322007!!@@"
        //    };
        //    var okResult = _controller.CreateUser(model);
        //    Assert.IsType<OkObjectResult>(okResult.Result);
        //}        

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
            var okResult = _controller.GetById("778df06e-142a-48ff-889c-102eb78c1ffb");
            // Assert
            Assert.IsNotType<BadRequestResult>(okResult.Result);
        }

        [Fact]
        public void UsuariosControllerBuscarPorEmail()
        {
            // Act
            var okResult = _controller.FindUserByEmail("renato@ansata.com.br");
            // Assert
            Assert.IsNotType<BadRequestResult>(okResult.Result);
        }

        [Fact]
        public void UsuariosControllerExcluir()
        {
            // Act
            var okResult = _controller.DeleteUser("2132131");
            // Assert
            Assert.IsNotType<BadRequestResult>(okResult.Result);
        }

    }
}
