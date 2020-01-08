﻿using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentComodityPriceControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public GarmentComodityPriceControllerTests() : base()
        {
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }

        private GarmentComodityPriceController CreateGarmentComodityPriceController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentComodityPriceController controller = (GarmentComodityPriceController)Activator.CreateInstance(typeof(GarmentComodityPriceController), _MockServiceProvider.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        [Fact]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentComodityPriceController();

            _mockComodityPriceRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentComodityPriceReadModel>().AsQueryable());

            _mockComodityPriceRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentComodityPriceReadModel>>()))
                .Returns(new List<GarmentComodityPrice>()
                {
                    new GarmentComodityPrice(Guid.NewGuid(),true ,DateTimeOffset.Now, new UnitDepartmentId(1), null, null,  new GarmentComodityId(1),null, null,10)
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
