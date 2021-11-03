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
        private readonly string _authorizationToken = "authorizationToken";

        private readonly Mock<IAuthRepository> mockRep = new Mock<IAuthRepository>();
        private readonly Mock<ILogger<AuthController>> mockLog = new Mock<ILogger<AuthController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        //summary: sjekk for logg inn administrator vellykket
        [Fact]
        public async Task Authorize()
        {
            //Arrange
            var inncomingUser = new UserInfo { Username = "", Password = "" }; //it.isAny<UserInfo> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.AuthenticateAdministrator(It.IsAny<UserInfo>())).ReturnsAsync(true);

            var authController = new AuthController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            authController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await authController.EstabilishAdministratorToken(inncomingUser) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("sucessfull logg inn", result.Value);
        }

        //summary: sjekk for logg inn administrator feil inn data
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

        //summary: sjekk for logg inn administrator feil ved logg inn
        [Fact]
        public async Task AuthorizeFail()
        {
            //Arrange
            var inncomingUser = new UserInfo { Username = "", Password = "" }; //it.isAny<UserInfo> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.AuthenticateAdministrator(It.IsAny<UserInfo>())).ReturnsAsync(false);
            
            var authController = new AuthController(mockRep.Object, mockLog.Object);

            //Act
            var result = await authController.EstabilishAdministratorToken(inncomingUser) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("wrong username or password", result.Value);

        }

        //summary: sjekk for logg ut administrator vellykket
        [Fact]
        public void Deauthorize()
        {
            //Arrange
            var authController = new AuthController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            authController.ControllerContext.HttpContext = mockHttpContext.Object;


            //Act
            var result = authController.DemolishAdministratorToken() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

        }
    }
}
