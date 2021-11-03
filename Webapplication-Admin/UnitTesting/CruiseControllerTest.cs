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
    public class CruiseControllerTest
    {
        private readonly string _authorizationToken = "authorizationToken";

        private readonly Mock<IAppDataRepository> mockRep = new Mock<IAppDataRepository>();
        private readonly Mock<ILogger<CruiseController>> mockLog = new Mock<ILogger<CruiseController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        //summary: sjekk for hent alle objekter vellykket
        [Fact]
        public async Task GetAllAuthorized()
        {
            //Arrange
            var cruiseA = new Cruise { Id = 1, CruiseDetails = It.IsAny<CruiseDetails>(), Route = It.IsAny<Route>() }; //det er usikkert om objekter med is.any kan sammenliknes
            var cruiseB = new Cruise { Id = 2, CruiseDetails = It.IsAny<CruiseDetails>(), Route = It.IsAny<Route>() }; //det er usikkert om objekter med is.any kan sammenliknes

            var cruises = new List<Cruise>
            {
                cruiseA,
                cruiseB
            };

            mockRep.Setup(r => r.GetCruises()).ReturnsAsync(cruises);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Get() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(cruises, (List<Cruise>)result.Value);

        }

        //summary: sjekk for hent alle objekter ikke logget inn
        [Fact]
        public async Task GetAllUnauthorized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Get() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for hent et objekt vellykket
        [Fact]
        public async Task GetOneAuthorized()
        {
            //Arrange
            var cruise = new Cruise { Id = 1, CruiseDetails = It.IsAny<CruiseDetails>(), Route = It.IsAny<Route>() }; //det er usikkert om objekter med is.any kan sammenliknes

            mockRep.Setup(r => r.GetCruise(It.IsAny<int>())).ReturnsAsync(cruise);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Get(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(cruise, (Cruise)result.Value);

        }

        //summary: sjekk for hent et objekt ikke logget inn
        [Fact]
        public async Task GetOneUnauthorized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Get(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for legg inn et objekt vellykket
        [Fact]
        public async Task PostAuthorized()
        {
            //Arrange
            var inncomingCruise = new CruiseBinding { Id = 1, detailsId = 1, routeId = 1 }; //it.isAny<CruiseBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.AddCruise(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;


            //Act
            var result = await cruiseController.Post(inncomingCruise) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully added the new cruise", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil ved registrering
        [Fact]
        public async Task PostAuthorizedFail()
        {
            //Arrange
            var inncomingCruise = new CruiseBinding { Id = 1, detailsId = 1, routeId = 1 }; //it.isAny<CruiseBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.AddCruise(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Post(inncomingCruise) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new cruise cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt ikke logget inn
        [Fact]
        public async Task PostUnauthorized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Post(It.IsAny<CruiseBinding>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for endre et objekt vellykket
        [Fact]
        public async Task PutAuthorized()
        {
            //Arrange
            var inncomingCruise = new CruiseBinding { Id = 1, detailsId = 1, routeId = 1 }; //it.isAny<CruiseBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.EditCruise(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Put(inncomingCruise) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully changed the cruise", result.Value);
        }

        //summary: sjekk for endre et objekt feil ved endring
        [Fact]
        public async Task PutAuthorizedFail()
        {
            //Arrange
            var inncomingCruise = new CruiseBinding { Id = 1, detailsId = 1, routeId = 1 }; //it.isAny<CruiseBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.EditCruise(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Put(inncomingCruise) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The cruise could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt ikke logget inn
        [Fact]
        public async Task PutUnauthorized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Put(It.IsAny<CruiseBinding>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for slett et objekt vellykket
        [Fact]
        public async Task DeleteAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteCruise(It.IsAny<int>())).ReturnsAsync(true);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Delete(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully removed the cruise", result.Value);
        }

        //summary: sjekk for slett et objekt feil ved slettning
        [Fact]
        public async Task DeleteAuthorizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteCruise(It.IsAny<int>())).ReturnsAsync(false);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The cruise could not be removed", result.Value);
        }

        //summary: sjekk for slett et objekt ikke logget inn
        [Fact]
        public async Task DeleteUnauthorized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Delete(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }
    }
}


