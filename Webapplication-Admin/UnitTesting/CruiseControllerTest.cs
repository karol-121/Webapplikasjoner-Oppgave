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
    public class CruiseControllerTest
    {
        private readonly string _autorizaionToken = "autorizaionToken";

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

            mockSession[_autorizaionToken] = "admin";
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
        public async Task GetAllUnautohrized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Get() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for hent et objekt vellykket
        [Fact]
        public async Task GetOneAutohrized()
        {
            //Arrange
            var cruise = new Cruise { Id = 1, CruiseDetails = It.IsAny<CruiseDetails>(), Route = It.IsAny<Route>() }; //det er usikkert om objekter med is.any kan sammenliknes

            mockRep.Setup(r => r.GetCruise(It.IsAny<int>())).ReturnsAsync(cruise);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
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
        public async Task GetOneUnautohrized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Get(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for legg inn et objekt vellykket
        [Fact]
        public async Task PostAutohrized()
        {
            //Arrange
            mockRep.Setup(r => r.AddCruise(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Post(It.IsAny<CruiseBinding>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully added the new cruise", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil ved registrering
        [Fact]
        public async Task PostAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.AddCruise(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Post(It.IsAny<CruiseBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new cruise cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt ikke logget inn
        [Fact]
        public async Task PostUnautohrized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Post(It.IsAny<CruiseBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for endre et objekt vellykket
        [Fact]
        public async Task PutAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.EditCruise(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Put(It.IsAny<CruiseBinding>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully changed the cruise", result.Value);
        }

        //summary: sjekk for endre et objekt feil ved endring
        [Fact]
        public async Task PutAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.EditCruise(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Put(It.IsAny<CruiseBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The cruise could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt ikke logget inn
        [Fact]
        public async Task PutUnautohrized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Put(It.IsAny<CruiseBinding>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for slett et objekt vellykket
        [Fact]
        public async Task DeleteAutohrized()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteCruise(It.IsAny<int>())).ReturnsAsync(true);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
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
        public async Task DeleteAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteCruise(It.IsAny<int>())).ReturnsAsync(false);

            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
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
        public async Task DeleteUnautohrized()
        {
            //Arrange
            var cruiseController = new CruiseController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }
    }
}


