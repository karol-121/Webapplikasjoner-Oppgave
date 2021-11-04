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
    public class DepartureControllerTest
    {
        private readonly string _authorizationToken = "authorizationToken";

        private readonly Mock<IAppDataRepository> mockRep = new Mock<IAppDataRepository>();
        private readonly Mock<ILogger<DepartureController>> mockLog = new Mock<ILogger<DepartureController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        //summary: sjekk for hent alle objekter vellykket
        [Fact]
        public async Task GetAllAuthorized()
        {
            //Arrange
            var departureA = new Departure { Id = 1, Cruise = It.IsAny<Cruise>(), Date = It.IsAny<DateTime>() }; //det er ikke sikkert om objekter med it.isany kan sammenliknes
            var departureB = new Departure { Id = 1, Cruise = It.IsAny<Cruise>(), Date = It.IsAny<DateTime>() }; //det er ikke sikkert om objekter med it.isany kan sammenliknes

            var departures = new List<Departure>
            {
                departureA,
                departureB
            };

            mockRep.Setup(r => r.GetDepartures()).ReturnsAsync(departures);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Get() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(departures, (List<Departure>)result.Value);

        }

        //summary: sjekk for hent alle objekter ikke logget inn
        [Fact]
        public async Task GetAllUnauthorized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Get() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for hent et objekt vellykket
        [Fact]
        public async Task GetOneAuthorized()
        {
            //Arrange
            var departure = new Departure { Id = 1, Cruise = It.IsAny<Cruise>(), Date = It.IsAny<DateTime>() };

            mockRep.Setup(r => r.GetDeparture(It.IsAny<int>())).ReturnsAsync(departure);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Get(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(departure, (Departure)result.Value);

        }

        //summary: sjekk for hent et objekt ikke logget inn
        [Fact]
        public async Task GetOneUnauthorized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Get(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for legg inn et objekt vellykket
        [Fact]
        public async Task PostAuthorized()
        {
            //Arrange
            var inncomingDeparture = new DepartureBinding { Id = 1, cruiseId = 1, dateString = "" }; //it.isAny<DepartureBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.AddDeparture(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Post(inncomingDeparture) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully added the new departure", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil ved registrering
        [Fact]
        public async Task PostAuthorizedFail()
        {
            //Arrange
            var inncomingDeparture = new DepartureBinding { Id = 1, cruiseId = 1, dateString = "" }; //it.isAny<DepartureBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.AddDeparture(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Post(inncomingDeparture) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new departure cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt ikke logget inn
        [Fact]
        public async Task PostUnauthorized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Post(It.IsAny<DepartureBinding>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for endre et objekt vellykket
        [Fact]
        public async Task PutAuthorized()
        {
            //Arrange
            var inncomingDeparture = new DepartureBinding { Id = 1, cruiseId = 1, dateString = "" }; //it.isAny<DepartureBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.EditDeparture(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Put(inncomingDeparture) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully changed the departure", result.Value);
        }

        //summary: sjekk for endre et objekt feil ved endring
        [Fact]
        public async Task PutAuthorizedFail()
        {
            //Arrange
            var inncomingDeparture = new DepartureBinding { Id = 1, cruiseId = 1, dateString = "" }; //it.isAny<DepartureBindig> fungerer ikke, det blir null reference exception.

            mockRep.Setup(r => r.EditDeparture(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Put(inncomingDeparture) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The departure could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt ikke logget inn
        [Fact]
        public async Task PutUnauthorized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Put(It.IsAny<DepartureBinding>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for slett et objekt vellykket
        [Fact]
        public async Task DeleteAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteDeparture(It.IsAny<int>())).ReturnsAsync(true);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Delete(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully removed the departure", result.Value);
        }

        //summary: sjekk for slett et objekt feil ved slettning
        [Fact]
        public async Task DeleteAuthorizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteDeparture(It.IsAny<int>())).ReturnsAsync(false);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The departure could not be removed", result.Value);
        }

        //summary: sjekk for slett et objekt ikke logget inn
        [Fact]
        public async Task DeleteUnauthorized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_authorizationToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Delete(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }
    }
}
