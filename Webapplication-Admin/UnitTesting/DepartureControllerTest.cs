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
using Webapplication_Admin.Models;
using Xunit;

namespace UnitTesting
{
    public class DepartureControllerTest
    {
        private readonly string _autorizaionToken = "autorizaionToken";

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

            mockSession[_autorizaionToken] = "admin";
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
        public async Task GetAllUnautohrized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Get() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for hent et objekt vellykket
        [Fact]
        public async Task GetOneAutohrized()
        {
            //Arrange
            var departure = new Departure { Id = 1, Cruise = It.IsAny<Cruise>(), Date = It.IsAny<DateTime>() }; //det er ikke sikkert om objekter med it.isany kan sammenliknes

            mockRep.Setup(r => r.GetDeparture(It.IsAny<int>())).ReturnsAsync(departure);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
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
        public async Task GetOneUnautohrized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Get(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for legg inn et objekt vellykket
        [Fact]
        public async Task PostAutohrized()
        {
            //Arrange
            mockRep.Setup(r => r.AddDeparture(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Post(It.IsAny<DepartureBinding>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully added the new departure", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil ved registrering
        [Fact]
        public async Task PostAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.AddDeparture(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Post(It.IsAny<DepartureBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new departure cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt ikke logget inn
        [Fact]
        public async Task PostUnautohrized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Post(It.IsAny<DepartureBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for endre et objekt vellykket
        [Fact]
        public async Task PutAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.EditDeparture(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Put(It.IsAny<DepartureBinding>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully changed the departure", result.Value);
        }

        //summary: sjekk for endre et objekt feil ved endring
        [Fact]
        public async Task PutAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.EditDeparture(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Put(It.IsAny<DepartureBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The departure could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt ikke logget inn
        [Fact]
        public async Task PutUnautohrized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Put(It.IsAny<DepartureBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for slett et objekt vellykket
        [Fact]
        public async Task DeleteAutohrized()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteDeparture(It.IsAny<int>())).ReturnsAsync(true);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
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
        public async Task DeleteAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteDeparture(It.IsAny<int>())).ReturnsAsync(false);

            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
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
        public async Task DeleteUnautohrized()
        {
            //Arrange
            var departureController = new DepartureController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            departureController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await departureController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }
    }
}
