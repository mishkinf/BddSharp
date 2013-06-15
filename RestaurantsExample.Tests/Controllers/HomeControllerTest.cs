using System.Web.Mvc;
using NUnit.Framework;
using RestaurantsExample.Controllers;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace RestaurantsExample.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Restaurants Example Home Page", result.ViewBag.Message);
        }
    }
}
