using Microsoft.VisualStudio.TestTools.UnitTesting;
using JuntoSeguros.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using JuntoSeguros.Services;
using Microsoft.AspNetCore.Mvc;

namespace JuntoSeguros.Controllers.Tests
{
    [TestClass()]
    public class UsuariosControllerTests
    {
        private readonly IUsuarioService _service;
        UsuariosController _us;
        public UsuariosControllerTests(IUsuarioService service)
        {
            _service = service;
            _us = new UsuariosController(_service);
        }

        [TestMethod()]
        public void FindUserByEmailTest()
        {
            var mockService = new Mock<ILoggingService>();//Mock

            var okResult = _us.FindUserByEmail("");

            Assert.IsNotNull(okResult.Result);
        }
    }
}