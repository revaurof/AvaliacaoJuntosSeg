using Xunit;
using JuntoSeguros.Controllers;
using Microsoft.AspNetCore.Identity;
using JuntoSeguros.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;

namespace XUnitTestJuntosSeg
{

    public class UnitTestJuntosSeg
    {
        
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IConfiguration _configuration;
        //UsuariosController _controller;


        //public UnitTestJuntosSeg(UserManager<ApplicationUser> userManager,
        //    SignInManager<ApplicationUser> signInManager,
        //    IConfiguration configuration)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _configuration = configuration;

        //    _controller = new UsuariosController(_userManager, _signInManager, _configuration);
        //}

        //[Fact]
        //public void Get_index()
        //{
        //    // Act
        //    var okResult = _controller.Get();
        //    // Assert
        //    Assert.IsType<OkObjectResult>(okResult.Result);
        //}

        //[Fact]
        //public void Get_ListarTodos()
        //{
        //    // Act
        //    var okResult = _controller.GetById("2132131");
        //    // Assert
        //    Assert.IsType<OkObjectResult>(okResult.Result);
        //}
        [Fact]
        public async Task GetPublicHealthEndpoint()
        {
            var apiClient = new HttpClient();

            var apiResponse = await apiClient.GetAsync("http://localhost:5001/api/Usuarios/");

            Assert.True(apiResponse.IsSuccessStatusCode);

            var stringResponse = await apiResponse.Content.ReadAsStringAsync();

            Assert.Equal("Healthy", stringResponse);
        }

    }
}
