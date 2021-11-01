using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Webapplication.Controllers;
using Webapplication.DAL;
using Webapplication.Models;
using Xunit;

namespace UnitTesting
{
    public class AuthControllerTest
    {
        private readonly Mock<IAuthRepository> mockRep = new Mock<IAuthRepository>();
        private readonly Mock<ILogger<AuthController>> mockLog = new Mock<ILogger<AuthController>>();

        [Fact]
        public async Task Authorize()
        {
            //Arrange
            mockRep.Setup(r => r.AuthenticateAdministrator(It.IsAny<UserInfo>())).ReturnsAsync(true);

            var authController = new AuthController(mockRep.Object, mockLog.Object);

            //Act
            var result = await authController.EstabilishAdministratorToken(It.IsAny<UserInfo>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("sucessfull logg inn", result.Value);
        }

        [Fact]
        public async Task AuthorizeInvalidModel()
        {
            //Arrange
            mockRep.Setup(r => r.AuthenticateAdministrator(It.IsAny<UserInfo>())).ReturnsAsync(false);

            var authController = new AuthController(mockRep.Object, mockLog.Object);
            
            authController.ModelState.AddModelError("Username", "invalid user information");

            //Act
            var result = await authController.EstabilishAdministratorToken(It.IsAny<UserInfo>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("invalid user information", result.Value);
        }

        [Fact]
        public async Task AuthorizeFail()
        {
            //Arrange
            mockRep.Setup(r => r.AuthenticateAdministrator(It.IsAny<UserInfo>())).ReturnsAsync(false);

            var authController = new AuthController(mockRep.Object, mockLog.Object);

            //Act
            var result = await authController.EstabilishAdministratorToken(It.IsAny<UserInfo>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("wrong username or password", result.Value);

        }

        [Fact]
        public void Deauthorize()
        {
            //Arrange
            var authController = new AuthController(mockRep.Object, mockLog.Object);

            //Act
            var result = authController.DemolishAdministratorToken() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

        }
    }
}
