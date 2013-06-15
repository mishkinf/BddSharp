# BddSharp
 *A Simple Behavior Driven Development Setup for C#*

### Credits
Mishkin Faustini, Author

### Description
BddSharp is intended to make setting up a behavior-driven test development (BDD) and test-driven development (TDD) environment for ASP.Net Entity Framework based web applications easy.

<b>*What is BDD?*</b> It is an acronym for Behavior-Driven Development. It is a process of developing code based on the desired behavior of your application.
For instance if I was building a website for finding restaurants and I wanted to behavior drive the development, I would begin by writing my tests as the desired behavior I wanted. For example:

<b>*What is TDD?*</b> It is an acroynum of Test-Driven Development which is a specialized version of BDD. TDD is the process of writing a set of tests before writing the code that attempts to make the tests pass. In the context of BDD, test-driven development is the process of writing tests that describe the behavior of your project before writing the logic (code) to fulfill the desired behavior. Please refer to the example below.

<b>*Why BDD/TDD?*</b> By beginning with writing the desired behavior of a product we often have a strong focus on building only the vital parts of our application in a manner that is very *agile* to future changes. In addition, since we are testing everything we are coding, we know why and what a future change to the behavior of our product causes and we can easily dig in and fix it. It provides a higher degree of assurance that our product is fully functioning.  

#### Example Desired Behavior 
> "When I load the restaurants app home page, I want to see a list of all the restaurants that have a 5-star rating"

#### Then I write the test
```csharp
// RestaurantManagerSpec.cs
// Assuming I have created several test data fixtures but only 1 with a 5 star rating
RestaurantManager restaurantManager = new RestaurantManager();

Restaurants[] restaurants = restaurantManager.getRestaurantsWithRating(5);
Assert.Equals(restaurants.Length, 1);
Assert.Equals(restuarants[0].Name, "A 5-Star Restaurant");
```
#### Finally, I fulfill this all by writing the code
```csharp
// RestaurantManager.cs

public class RestaurantManager
{
	// ...some code before...
	public Restaurant[] getRestaurantWithRating(int rating)
	{
		return RestaurantRepository.Entities.Where(x => x.Rating == 5).ToArray();
	}
	// ...some code after...
}
```

### Important Aspects of a Good BDD/TDD setup
 * Has quick and easy way to build fake data to test against
 * Always provides a perfect and untouched representation of the data on each test run
 * Simple to write both unit and integration tests
 * Runs easily on a continuous integration server
 * Allows tests to be written in english in terms of *behaviors*

### BddSharp Features
 * Supports test data fixture building
 	- Works with a test DB and an in-memory version of your Entity Framework context to support both very fast unit tests and fully functional integration testing
 * Provides a way to easily spawn a test server automatically on a specified host/port
 * Supports running tests on a CI environment like TeamCity/Jenkins

### Upcoming BddSharp Features 
 * Automatic Jasmine HTML fixture generation before running tests
 * Improved test data fixture generation
 * Only spawn test server when running integration test that requires it

## Setup / Requirements

 1. Install the following NuGet Packages on your Web Project
   * [BddSharp.Web](https://nuget.org/packages/BddSharp)
 2. Setup a Install the following NuGet Packages on your Test Project
   * [BddSharp](https://nuget.org/packages/BddSharp)
 3. Install [ReSharper](http://www.jetbrains.com/resharper/) for Jasmine testing ease
 4. You must have IIS Installed and Configured for your needs
 5. Configure App.config settings inside of your test project

```xml
  <appSettings>
    <add key="WebDevPath" value="C:\Program Files (x86)\Common Files\Microsoft Shared\DevServer\11.0\WebDev.WebServer40.exe" />
    <add key="AppRoot" value="<physical path to root of asp.net application>" />
    <add key="PortNumber" value="44444" />
    <add key="VirtualPath" value="" />
  </appSettings>
```
 4. General Folder Structure Recommended for Test Project
  * TestProject
     - Feature/
     - Fixtures/
     - JavaScript/
     - Spec/
     - SpecHelper.cs

### Creating A Test Server

```csharp
// Create a class that derives from TestServer
public class MyProjectTestServer : TestServer
{
  public MyProjectTestServer() : base()
  {
    // CTOR
  }
}

// Spawn your test server before running tests

MyProjectTestServer server = new MyProjectTestServer();
server.Spawn();

// Run all SpecFlow/Integration tests

server.Kill(); 
```
 
### Understanding SpecHelper.cs
SpecHelper.cs is the file where you should create a test server, do any global level test setup and cleanup, and seed your test database properly. 

```csharp
[SetUpFixture]
public class SpecHelper
{
	public static dynamic dataFixtures;
	public static RestaurantsTestServer testServer = new RestaurantsTestServer();
	
	[SetUp]
	public static void BeforeAllTests()
	{
	    Nav.Host = "http://localhost:44444";
	
	    RestaurantsContext restaurantsContext = RestaurantsContext.Instance;
	    // Runs before any of the tests are run
	    dataFixtures = new BddSharp.Fixtures();
	
	    if (restaurantsContext.Database.Connection.State == ConnectionState.Open)
	        restaurantsContext.Database.Connection.Close();
	
	    Database.SetInitializer(new RestaurantsSeedInitializer(context => dataFixtures.Load(restaurantsContext, Assembly.GetExecutingAssembly()))); ;
	    restaurantsContext.Database.Initialize(true);
	    restaurantsContext.Database.Connection.Open();
	
	    testServer.Spawn();
	}
	
	[TearDown]
	public static void AfterAllTests()
	{
	    testServer.Kill();
	}
}
```

### Creating a Seed Initializer 
An example of a generic database initializer that seeds the fixture test data 
```csharp
using System;
using System.Data.Entity;

namespace RestaurantsExample.EntityFramework
{
    public class RestaurantsSeedInitializer : IDatabaseInitializer<RestaurantsContext>
    {
        private Action<RestaurantsContext> FixtureSeeder;

        public RestaurantsSeedInitializer()
        {

        }

        public RestaurantsSeedInitializer(Action<RestaurantsContext> fixtureSeeder)
        {
            FixtureSeeder = fixtureSeeder;
        }

        protected void Seed(RestaurantsContext context)
        {
            // Populate DB with Fixture data, if passed in
            if (FixtureSeeder != null)
            {
                FixtureSeeder(context);
            }
        }

        public void InitializeDatabase(RestaurantsContext context)
        {
            Seed(context);
        }
    }
}
```

### Creating Pages 
```csharp
 // Set your host properly (most times this is http://localhost:<portNum>)
 Nav.Port = "44444";
 Nav.Host = "http://localhost";
 // Easily set pages on Nav.Pages 
 Nav.Pages.Home = "/";
 Nav.Pages.Restaurants = "/Restaurants";
```

### Using a Generic Repository
For more information about the Repository pattern please read [Implementing the Repository and Unit of Work Patterns in an ASP.NET MVC Application](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application).
Your web application must be using a generic repository in order to gain the BDD benefits that this framework provides. 


### Creating a Test Repository
A test context is substituted for unit testing with data fixtures. This gives us tests that run in memory and thus very speedily since they are not polling/resetting the database.

### Creating Test Data Fixtures
Idealy your tests should be driven against test data. Test data fixtures provide the ability to simply generate data which is used for in memory unit testing and for seeding the test database that runs the SpecFlow/WatiN integration tests.

TestDataFixtures should be placed in the Fixtures/ folder of your test project. 

A static function annotated with [FixtureLoader] can be used to create fixtures.

```csharp
// Fixtures/RestaurantsFixtures.cs
using BddSharp;
using RestaurantsExample.Models;
using System;
using System.Data.Entity;

namespace RestaurantsExample.Tests.Fixtures
{
    class RestaurantsFixtures
    {
        [FixtureLoader]
        public static void Restaurants(dynamic fixtures, DbContext context)
        {
            fixtures.mishkinsRestaurant = new Restaurant()
            {
                Id = 1,
                Name = "Mishkin's Amazing Italian Food",
                OpeningTime = DateTime.Now.AddHours(-2),
                ClosingTime = DateTime.Now.AddHours(2)
            };

            context.Set<Restaurant>().Add(fixtures.mishkinsRestaurant);
            context.SaveChanges();

            // Add to our test context
            TestRepositories.AddFixtureToContext(fixtures.mishkinsRestaurant);
        }
    }
}
```

### Using Test Data Fixtures
Test data can be easily referenced in any of your tests


## Examples

### SpecFlow & WatiN
*Integration Testing using Test Fixture Data*

```csharp
// RestaurantFixtures.cs
// Insert fixture here
```

```gherkin
# Restaurants.feature
Feature: Restaurants

Scenario: Get Open Restaurants 
 Given I have loaded test data fixtures
	When I visit the restaurants page
	And I click on #open_restaurants
	Then I should see "mishkinsRestaurant" restaurant listed
```

```csharp
// CustomWebSteps.cs
[Binding]
public class CustomWebSteps
{
    [Given]
    public void Given_I_have_loaded_fixtures()
    {
        SpecHelper.LoadTestDataFixtures();
        SpecHelper.EnsureTestServerIsSetup();
    }

    [When(@"I visit the restaurants page")]
    public void WhenIVisitTheRestaurantsPage()
    {
        WebBrowser.Current.GoTo(Nav.Pages.Restaurants);
        WebBrowser.Current.WaitForComplete();            
    }

    [Then("I should see \"(.*)\" restaurant listed)]
    public void Then_I_should_see_restaurant_in_results(string fixtureName)
    {
        Restaurant fixture = SpecHelper.dataFixtures[fixtureName];
        Assert.True(WebBrowser.Current.Div("restaurants").Text.Contains(fixture.RestaurantName));
    }
}
```

### NUnit
*Unit Testing using Test Fixture Data*
```csharp
[TestFixture]
public class RestaurantManagerSpec
{
    RestaurantManager restaurantManager  = new RestaurantManager();

    [SetUp]
    public void SetupRestaurantManagerSpec()
    {
        TestContext<Restaurant> restaurantRepository = SpecHelper.GetContext<Restaurant>();
        restaurantManager.Repository = restaurantRepository;
    }

    [Test, Description("When given a time, should return restaurants that are open")]
    public void TestOpenRestaurants()
    {
        var restaurants = restaurantManager.GetOpenRestaurants(DateTime.Now());
        Assert.IsTrue(restaurants.Count == 1);
    }
}
```

### Jasmine
*JavaScript Testing*
[Testing with Jasmine](http://pivotal.github.io/jasmine/) has some examples of how to write Jasmine tests.

## License

The MIT License Copyright (c) 2013 TrueCar, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
