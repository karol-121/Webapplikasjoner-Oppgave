using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
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
        private readonly string _authorizationToken = "authorizationToken";

        private readonly Mock<IAppDataRepository> mockRep = new Mock<IAppDataRepository>();
        private readonly Mock<ILogger<RouteController>> mockLog = new Mock<ILogger<RouteController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        //summary: sjekk for hent alle objekter vellykket
        [Fact]
        public async Task GetAllAuthorized()
        {
            //Arrange
            var routeA = new Route { Id = 1, Origin = "Oslo", Destination = "Bergen", Return_id = 2 };
            var routeB = new Route { Id = 2, Origin = "Bergen", Destination = "Oslo", Return_id = 1 };

            var routes = new List<Route>
            {
                routeA,
                routeB
            };

            mockRep.Setup(r => r.GetRoutes()).ReturnsAsync(routes);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Get() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(routes, (List<Route>)result.Value);

        }

        //summary: sjekk for hent alle objekter ikke logget inn
        [Fact]
        public async Task GetAllUnauthorized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Get() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for hent et objekt vellykket
        [Fact]
        public async Task GetOneAuthorized()
        {
            //Arrange
            var route = new Route { Id = 1, Origin = "Oslo", Destination = "Bergen", Return_id = 2 };

            mockRep.Setup(r => r.GetRoute(It.IsAny<int>())).ReturnsAsync(route);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Get(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(route, (Route)result.Value);

        }

        //summary: sjekk for hent et objekt ikke logget inn
        [Fact]
        public async Task GetOneUnauthorized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Get(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for legg inn et objekt vellykket
        [Fact]
        public async Task PostAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.AddRoute(It.IsAny<Route>())).ReturnsAsync(true);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Post(It.IsAny<Route>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully added the new route", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil inn data
        [Fact]
        public async Task PostAuthorizedInvalidModel()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            routeController.ModelState.AddModelError("Origin", "The new route cound not be added");

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Post(It.IsAny<Route>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new route cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil ved registrering
        [Fact]
        public async Task PostAuthorizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.AddRoute(It.IsAny<Route>())).ReturnsAsync(false);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Post(It.IsAny<Route>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new route cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt ikke logget inn
        [Fact]
        public async Task PostUnauthorized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Post(It.IsAny<Route>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for endre et objekt vellykket
        [Fact]
        public async Task PutAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.EditRoute(It.IsAny<Route>())).ReturnsAsync(true);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Put(It.IsAny<Route>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully changed the route", result.Value);
        }

        //summary: sjekk for endre et objekt feil inn data 
        [Fact]
        public async Task PutAuthorizedInvalidModel()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            routeController.ModelState.AddModelError("Origin", "The route could not be changed");

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Put(It.IsAny<Route>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The route could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt feil ved endring
        [Fact]
        public async Task PutAuthorizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.EditRoute(It.IsAny<Route>())).ReturnsAsync(false);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Put(It.IsAny<Route>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The route could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt ikke logget inn
        [Fact]
        public async Task PutUnauthorized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Put(It.IsAny<Route>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for slett et objekt vellykket
        [Fact]
        public async Task DeleteAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteRoute(It.IsAny<int>())).ReturnsAsync(true);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Delete(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully removed the route", result.Value);
        }

        //summary: sjekk for slett et objekt feil ved slettning
        [Fact]
        public async Task DeleteAuthorizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteRoute(It.IsAny<int>())).ReturnsAsync(false);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The route could not be removed", result.Value);
        }

        //summary: sjekk for slett et objekt ikke logget inn
        [Fact]
        public async Task DeleteUnauthorized()
        {
            //Arrange
            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await routeController.Delete(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }
    }
}
