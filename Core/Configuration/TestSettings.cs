using Microsoft.Extensions.Configuration;

namespace SeleniumFrameworkProject.Core.Configuration
{
    /// <summary>
    /// Provides centralized access to application and test configuration settings.
    /// Implements the singleton pattern to ensure a single, consistent instance across the application.
    /// Retrieves values from appsettings.json, environment variables, and provides default fallbacks
    /// for properties such as URLs, browser configuration, timeouts, and test credentials.
    /// </summary>
    public sealed class TestSettings
    {
        #region Fields and Constants

        /// <summary>
        /// Lazily initialized singleton instance of the <see cref="TestSettings"/> class.
        /// </summary>
        private static readonly Lazy<TestSettings> _instance =
            new(() => new TestSettings());

        /// <summary>
        /// An <see cref="IConfiguration"/> object holding the application configuration.
        /// Initialized in the class constructor.
        /// </summary>
        private readonly IConfiguration _configuration;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the single instance of the <see cref="TestSettings"/> class (singleton).
        /// </summary>
        public static TestSettings Instance => _instance.Value;

        /// <summary>
        /// The base URL of the application under test. 
        /// Defaults to "https://automationexercise.com" if not specified in configuration.
        /// </summary>
        public string BaseUrl => _configuration["TestSettings:BaseUrl"] ?? "https://automationexercise.com";

        /// <summary>
        /// The browser to use for tests, determined first from the BROWSER environment variable,
        /// then from configuration, and defaults to "Chrome" if neither is set.
        /// </summary>
        public string Browser =>
            Environment.GetEnvironmentVariable("BROWSER")
            ?? _configuration["TestSettings:Browser"]
            ?? "Chrome";

        /// <summary>
        /// Indicates whether tests should run in headless mode. 
        /// Determined from the HEADLESS environment variable, configuration, or defaults to false.
        /// </summary>
        public bool Headless =>
            GetBool(
                envKey: "HEADLESS",
                configKey: "TestSettings:Headless",
                defaultValue: false);

        /// <summary>
        /// Implicit wait timeout in seconds for WebDriver operations.
        /// Retrieved from configuration or defaults to 10 seconds.
        /// </summary>
        public int ImplicitWait =>
            GetInt(
                configKey: "TestSettings:ImplicitWait",
                defaultValue: 10);

        /// <summary>
        /// Page load timeout in seconds for WebDriver operations.
        /// Retrieved from configuration or defaults to 30 seconds.
        /// </summary>
        public int PageLoadTimeout =>
            GetInt(
                configKey: "TestSettings:PageLoadTimeout",
                defaultValue: 30);

        /// <summary>
        /// Determines whether screenshots should be taken on test failure.
        /// Checked first from SCREENSHOT_ON_FAILURE environment variable, then configuration, defaults to true.
        /// </summary>
        public bool ScreenshotOnFailure =>
            GetBool(
                envKey: "SCREENSHOT_ON_FAILURE",
                configKey: "TestSettings:ScreenshotOnFailure",
                defaultValue: true);

        /// <summary>
        /// Returns the valid username for tests from configuration.
        /// Defaults to "standard_user" if not specified.
        /// </summary>
        public string ValidUsername => _configuration["TestData:ValidUsername"] ?? "admin";

        /// <summary>
        /// Returns the valid password for tests from configuration.
        /// Defaults to "secret_sauce" if not specified.
        /// </summary>
        public string ValidPassword => _configuration["TestData:ValidPassword"] ?? "password";

        #endregion

        #region Constructors and destructors

        /// <summary>
        /// Private constructor for the <see cref="TestSettings"/> singleton.
        /// Initializes the <see cref="_configuration"/> field by building the application configuration
        /// from appsettings.json and environment variables.
        /// </summary>
        private TestSettings()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables() //For CI/CD
                .Build();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the URL for a specified environment from the configuration.
        /// If the environment-specific URL is not found, returns the default <see cref="BaseUrl"/>.
        /// </summary>
        /// <param name="environment">The name of the environment.</param>
        /// <returns>
        /// The URL associated with the specified environment, or the default base URL if not configured.
        /// </returns>
        public string GetEnvironmentUrl(string environment)
        {
            return _configuration[$"TestSettings:Environments:{environment}"]
                ?? BaseUrl;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves the value associated with the specified configuration key.
        /// Returns null if the key does not exist.
        /// </summary>
        /// <param name="key">The configuration key to look up.</param>
        /// <returns>
        /// The value from configuration, or null if not found.
        /// </returns>
        private string? GetValue(string key) => _configuration[key];

        /// <summary>
        /// Retrieves a boolean value from the environment variable or configuration.
        /// If neither is set or parsing fails, returns the specified default value.
        /// </summary>
        /// <param name="envKey">The name of the environment variable to check first.</param>
        /// <param name="configKey">The configuration key to use if the environment variable is not set.</param>
        /// <param name="defaultValue">The default value to return if neither source provides a valid boolean.</param>
        /// <returns>
        /// The boolean value from environment, configuration, or the default.
        /// </returns>
        private bool GetBool(string envKey, string configKey, bool defaultValue)
        {
            var value =
                Environment.GetEnvironmentVariable(envKey) ??
                _configuration[configKey];

            return bool.TryParse(value, out var result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Retrieves an integer value from the configuration.
        /// If parsing fails or the key does not exist, returns the specified default value.
        /// </summary>
        /// <param name="configKey">The configuration key to look up.</param>
        /// <param name="defaultValue">The default value to return if the configuration key is missing or invalid.</param>
        /// <returns>
        /// The integer value from configuration, or the default value.
        /// </returns>
        private int GetInt(string configKey, int defaultValue)
        {
            var value = _configuration[configKey];

            return int.TryParse(value, out var result)
                ? result
                : defaultValue;
        }

        #endregion
    }
}
