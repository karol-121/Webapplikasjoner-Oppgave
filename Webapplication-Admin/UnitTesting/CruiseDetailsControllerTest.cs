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
    public class CruiseDetailsControllerTest
    {
        private readonly string _autorizaionToken = "autorizaionToken";

        private readonly Mock<IAppDataRepository> mockRep = new Mock<IAppDataRepository>();
        private readonly Mock<ILogger<CruiseDetailsController>> mockLog = new Mock<ILogger<CruiseDetailsController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        //summary: sjekk for hent alle objekter vellykket
        [Fact]
        public async Task GetAllAuthorized()
        {
            //Arrange
            var cruiseDetailsA = new CruiseDetails { Id = 1, Max_Passengers = 100, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 60 };
            var cruiseDetailsB = new CruiseDetails { Id = 2, Max_Passengers = 200, Passeger_Price = 500, Passegner_Underage_Price = 200, Pet_Price = 250, Vehicle_Price = 150 };

            var details = new List<CruiseDetails>
            {
                cruiseDetailsA,
                cruiseDetailsB
            };

            mockRep.Setup(r => r.GetCruisesDetails()).ReturnsAsync(details);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Get() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(details, (List<CruiseDetails>)result.Value);

        }

        //summary: sjekk for hent alle objekter ikke logget inn
        [Fact]
        public async Task GetAllUnautohrized()
        {
            //Arrange
            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Get() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for hent et objekt vellykket
        [Fact]
        public async Task GetOneAutohrized()
        {
            //Arrange
            var details = new CruiseDetails { Id = 1, Max_Passengers = 100, Passeger_Price = 200, Passegner_Underage_Price = 150, Pet_Price = 50, Vehicle_Price = 60 };

            mockRep.Setup(r => r.GetCruiseDetails(It.IsAny<int>())).ReturnsAsync(details);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Get(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(details, (CruiseDetails)result.Value);

        }

        //summary: sjekk for hent et objekt ikke logget inn
        [Fact]
        public async Task GetOneUnautohrized()
        {
            //Arrange
            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Get(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for legg inn et objekt vellykket
        [Fact]
        public async Task PostAutohrized()
        {
            //Arrange
            mockRep.Setup(r => r.AddCruiseDetails(It.IsAny<CruiseDetails>())).ReturnsAsync(true);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Post(It.IsAny<CruiseDetails>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully added the new cruise details object", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil inn data
        [Fact]
        public async Task PostAutohrizedInvalidModel()
        {
            //Arrange
            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            cruiseDetailsController.ModelState.AddModelError("Max_Passengers", "The new cruise details object cound not be added");

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Post(It.IsAny<CruiseDetails>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new cruise details object cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt feil ved registrering
        [Fact]
        public async Task PostAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.AddCruiseDetails(It.IsAny<CruiseDetails>())).ReturnsAsync(false);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Post(It.IsAny<CruiseDetails>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The new cruise details object cound not be added", result.Value);
        }

        //summary: sjekk for legg inn et objekt ikke logget inn
        [Fact]
        public async Task PostUnautohrized()
        {
            //Arrange
            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Post(It.IsAny<CruiseDetails>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for endre et objekt vellykket
        [Fact]
        public async Task PutAuthorized()
        {
            //Arrange
            mockRep.Setup(r => r.EditCruiseDetails(It.IsAny<CruiseDetails>())).ReturnsAsync(true);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Put(It.IsAny<CruiseDetails>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfullyy changed the cruise details object", result.Value);
        }

        //summary: sjekk for endre et objekt feil inn data 
        [Fact]
        public async Task PutAutohrizedInvalidModel()
        {
            //Arrange
            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            cruiseDetailsController.ModelState.AddModelError("Max_Passengers", "The cruise details object could not be changed");

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Put(It.IsAny<CruiseDetails>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The cruise details object could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt feil ved endring
        [Fact]
        public async Task PutAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.EditCruiseDetails(It.IsAny<CruiseDetails>())).ReturnsAsync(false);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Put(It.IsAny<CruiseDetails>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The cruise details object could not be changed", result.Value);
        }

        //summary: sjekk for endre et objekt ikke logget inn
        [Fact]
        public async Task PutUnautohrized()
        {
            //Arrange
            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Put(It.IsAny<CruiseDetails>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }

        //summary: sjekk for slett et objekt vellykket
        [Fact]
        public async Task DeleteAutohrized()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteRoute(It.IsAny<int>())).ReturnsAsync(true);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Delete(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Sucessfully removed the cruise details object", result.Value);
        }

        //summary: sjekk for slett et objekt feil ved slettning
        [Fact]
        public async Task DeleteAutohrizedFail()
        {
            //Arrange
            mockRep.Setup(r => r.DeleteCruiseDetails(It.IsAny<int>())).ReturnsAsync(false);

            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "admin";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("The cruise details object could not be removed", result.Value);
        }

        //summary: sjekk for slett et objekt ikke logget inn
        [Fact]
        public async Task DeleteUnautohrized()
        {
            //Arrange
            var cruiseDetailsController = new CruiseDetailsController(mockRep.Object, mockLog.Object);

            mockSession[_autorizaionToken] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            cruiseDetailsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await cruiseDetailsController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Access denied", result.Value);
        }
    }
}

