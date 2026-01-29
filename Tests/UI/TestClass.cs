using SeleniumFrameworkProject.Core;
using SeleniumFrameworkProject.Core.Configuration;

namespace SeleniumFrameworkProject.Tests.UI
{
    [TestFixture]
    public class TestClass
    {
        public TestSettings settings = TestSettings.Instance;

        [Test]
        public void Test()
        {
            var driver = DriverFactory.GetDriver();
            driver.Navigate().GoToUrl(settings.BaseUrl);

        }
    }
}
