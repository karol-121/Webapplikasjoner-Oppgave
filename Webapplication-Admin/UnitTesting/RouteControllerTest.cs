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
    public class RouteControllerTest
    {
        private readonly string _autorizaionToken = "autorizaionToken";

        private readonly Mock<IAppDataRepository> mockRep = new Mock<IAppDataRepository>();
        private readonly Mock<ILogger<RouteController>> mockLog = new Mock<ILogger<RouteController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();


        [Fact]
        public void GetAllAuthorized()
        {

        }

        [Fact]
        public async Task GetAllUnautohrized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Get() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        [Fact]
        public void GetOneAutohrized()
        {

        }

        [Fact]
        public async Task GetOneUnautohrized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Get(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        [Fact]
        public void PostAutohrized()
        {

        }

        [Fact]
        public void PostAutohrizedInvalidModel()
        {

        }

        [Fact]
        public void PostAutohrizedFail()
        {

        }

        [Fact]
        public async Task PostUnautohrized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Post(It.IsAny<Route>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        [Fact]
        public void PutAutohrizedInvalidModel()
        {

        }

        [Fact]
        public void PutAutohrizedFail()
        {

        }

        [Fact]
        public async Task PutUnautohrized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Put(It.IsAny<Route>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        [Fact]
        public void DeleteAutohrized()
        {

        }

        [Fact]
        public void DeleteAutohrizedFail()
        {

        }

        [Fact]
        public async Task DeleteUnautohrized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }
    }
}
