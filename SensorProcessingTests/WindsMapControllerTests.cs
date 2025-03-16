using SensorProcessingDemo.Controllers;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace SensorProcessingTest
{
    public class Tests
    {
        private WindsMapController _controller;
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            
            _configurationMock
                .Setup(config => config["WindyAPI:Key"])
                .Returns("dummy-api-key");

            _controller = new WindsMapController(_configurationMock.Object);
        }

        [Test]
        public void Index_ValidModel_ShouldSetViewData()
        {
            // Arrange
            var model = new Location
            {
                Latitude = 50.4501,
                Longitude = 30.5234
            };

            // Act
            var result = _controller.Index(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(50.4501, result.ViewData["Latitude"]);
            Assert.AreEqual(30.5234, result.ViewData["Longitude"]);
            Assert.AreEqual("dummy-api-key", result.ViewData["WindyAPIKey"]);
        }

        [Test]
        public void Index_NullLocation_ShouldSetDefaultCoordinates()
        {
            // Arrange
            var model = new Location();

            // Act
            var result = _controller.Index(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SensorProcessingDemo.Common.Constants.LAT_KYIV, result.ViewData["Latitude"]);
            Assert.AreEqual(SensorProcessingDemo.Common.Constants.LONG_KYIV, result.ViewData["Longitude"]);
            Assert.AreEqual("dummy-api-key", result.ViewData["WindyAPIKey"]);
        }

        [Test]
        public void Index_InvalidModel_ShouldNotUpdateViewData()
        {
            // Arrange
            var model = new Location
            {
                Latitude = 1000,
                Longitude = 2000
            };

            _controller.ModelState.AddModelError("Latitude", "Invalid latitude");
            _controller.ModelState.AddModelError("Longitude", "Invalid longitude");

            // Act
            var result = _controller.Index(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewData["Latitude"]);
            Assert.IsNull(result.ViewData["Longitude"]);
        }
    }
}