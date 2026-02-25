# SeleniumFrameworkProject

A test automation framework built in C# (.NET 8) for testing the [Automation Exercise](https://automationexercise.com) web application. The project covers UI, API, and integration tests.

## Tech Stack

| Technology | Purpose |
|---|---|
| C# / .NET 8 | Core language and runtime |
| Selenium WebDriver 4 | UI test automation |
| NUnit 4 | Test runner and assertions |
| RestSharp | API test client |
| Microsoft.Extensions.Configuration | Config management via `appsettings.json` |
| Newtonsoft.Json | JSON serialization/deserialization |
| Coverlet | Code coverage collection |

## Project Structure

```
SeleniumFrameworkProject/
├── Core/                   # Base classes, driver setup, configuration
├── PageObjects/            # Page Object Model classes
├── Models/                 # Data models / DTOs
├── ApiClients/             # RestSharp API client wrappers
├── Utils/                  # Helper classes and utilities
├── Tests/
│   ├── UI/                 # Selenium UI tests
│   ├── API/                # REST API tests
│   └── IntegrationTests/   # Integration tests (UI + API combined)
└── appsettings.json        # Configuration (base URL, browser, credentials)
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Chrome / Firefox browser
- IDE: Visual Studio 2022 or JetBrains Rider

### Installation

```bash
git clone https://github.com/przemekpsuja/SeleniumFrameworkProject.git
cd SeleniumFrameworkProject
dotnet restore
```

### Configuration

Edit `appsettings.json` to set base URL and other options:

```json
{
  "BaseUrl": "https://automationexercise.com",
  "Browser": "Chrome"
}
```

### Running Tests

Run all tests:
```bash
dotnet test
```

Run only UI tests:
```bash
dotnet test --filter TestCategory=UI
```

Run only API tests:
```bash
dotnet test --filter TestCategory=API
```

## Test Coverage

- **UI Tests** – end-to-end scenarios using Page Object Model (Selenium WebDriver)
- **API Tests** – REST API validation using RestSharp
- **Integration Tests** – combined UI and API flow scenarios

## Author

Przemysław Psuja – [github.com/przemekpsuja](https://github.com/przemekpsuja)
