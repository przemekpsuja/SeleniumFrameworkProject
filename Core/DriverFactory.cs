using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SeleniumFrameworkProject.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkProject.Core
{
    /// <summary>
    /// Factory class for creating and managing WebDriver instances.
    /// Supports Chrome, Firefox, and Edge browsers.
    /// Ensures thread-safe usage with <see cref="ThreadLocal{T}"/>.
    /// </summary>
    public class DriverFactory
    {
        #region Fields and Constants

        /// <summary>
        /// Thread-local storage for the WebDriver instance.
        /// </summary>
        private static readonly ThreadLocal<IWebDriver> _driver = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the thread-safe WebDriver instance.
        /// Creates a new instance if none exists.
        /// </summary>
        /// <returns>
        /// The <see cref="IWebDriver"/> instance for the current thread.
        /// </returns>
        public static IWebDriver GetDriver()
        {
            if (_driver.Value == null)
                _driver.Value = CreateDriver();

            return _driver.Value;
        }

        /// <summary>
        /// Quits the WebDriver instance for the current thread and releases resources.
        /// </summary>
        public static void QuitDriver()
        {
            if (_driver.Value != null)
            {
                _driver.Value.Quit();
                _driver.Value = null;
            }
        }

        #endregion

        #region Private and Protected Methods

        /// <summary>
        /// Creates a new WebDriver instance based on browser settings.
        /// Configures timeouts and maximizes the window.
        /// </summary>
        /// <returns>
        /// A new <see cref="IWebDriver"/> instance.
        /// </returns>
        private static IWebDriver CreateDriver()
        {
            var settings = TestSettings.Instance;
            IWebDriver driver = settings.Browser.ToLower() switch
            {
                "chrome" => CreateChromeDriver(settings.Headless),
                "firefox" => CreateFirefoxDriver(settings.Headless),
                "edge" => CreateEdgeDriver(settings.Headless),
                _ => throw new ArgumentException($"Unsupported browser {settings.Browser}.")
            };
            
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.ImplicitWait);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(settings.PageLoadTimeout);
            driver.Manage().Window.Maximize();

            return driver;
        }


        /// <summary>
        /// Creates and configures a ChromeDriver instance.
        /// </summary>
        /// <param name="headless">Whether to run Chrome in headless mode.</param>
        /// <returns>
        /// A configured <see cref="ChromeDriver"/> instance.
        /// </returns>
        private static IWebDriver CreateChromeDriver(bool headless)
        {
            var options = new ChromeOptions();

            if (headless)
                options.AddArgument("--headless=new");

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--log-level=3");
            options.AddExcludedArgument("--log-level=3");

            return new ChromeDriver(options);
        }

        /// <summary>
        /// Creates and configures a EdgeDriver instance.
        /// </summary>
        /// <param name="headless">Whether to run Chrome in headless mode.</param>
        /// <returns>
        /// A configured <see cref="EdgeDriver"/> instance.
        /// </returns>
        private static IWebDriver CreateEdgeDriver(bool headless)
        {
            var options = new EdgeOptions();

            if (headless)
                options.AddArgument("--headless=new");

            options.AddArgument("--window-size=1920,1080");

            return new EdgeDriver(options);
        }

        /// <summary>
        /// Creates and configures a FirefoxDriver instance.
        /// </summary>
        /// <param name="headless">Whether to run Firefox in headless mode.</param>
        /// <returns>
        /// A configured <see cref="FirefoxDriver"/> instance
        /// </returns>
        private static IWebDriver CreateFirefoxDriver(bool headless)
        {
            var options = new FirefoxOptions();

            if (headless)
                options.AddArgument("--headless=new");

            options.AddArgument("--width=1920");
            options.AddArgument("--height=1080");

            return new FirefoxDriver(options);
        }

        #endregion
    }
}
