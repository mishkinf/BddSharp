# BddSharp
 *A Simple Behavior Driven Development Setup for C#*

### Credits
Mishkin Faustini, Author

### Description
BddSharp is intended to make setting up a behavior driven test development environment for ASP.Net Entity Framework based web applications easy.

*What is BDD?* It is an acronym for Behavior Driven Development. It is a process of developing code based on the desired behavior of your application.
For instance if I was building a website for finding restaurants and I wanted to behavior drive the development, I would begin by writing my tests as the desired behavior I wanted. For example:

#### Example Desired Behavior 
"When I load the restaurants app home page, I want to see a list of all the restaurants that have a 5-star rating"

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

## Setup / Requirements

 1. Install the following NuGet Packages 
   * [BddSharp](https://nuget.org/packages/BddSharp)
   * [SpecFlow](https://nuget.org/packages/SpecFlow/)
   * [SpecFlow.NUnit](https://nuget.org/packages/SpecFlow.NUnit/)
   * [WaTin](https://nuget.org/packages/watin/)
   * [NUnit](https://nuget.org/packages/nunit/)
 2. You must have IIS Installed and Configured for your needs
 3. General Folder Structure Recommended for Test Project
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
  public MyProjectTestServer(string port, string physicalPathCompiledApp) : base(port, physicalPathCompiledApp)
  {
    // CTOR
  }
}

// Spawn your test server before running tests

MyProjectTestServer server = new MyProjectTestServer("33333", @"c:/somePathToCompiledApp");
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
    private static MyProjectDataContext MyProjectContext;
    private static string serverPath = "<path to deployed app>";
    public static MyProjectTestServer testServer = new MyProjectTestServer("44444", serverPath);

    [SetUp]
    public static void BeforeAllTests()
    {
      // Runs before any of the tests are run
      Database.SetInitializer(new MyProjectSeedInitializer(context => dataFixtures.Load(context, Assembly.GetExecutingAssembly())));;
      MyProjectContext.Database.Initialize(true);
      
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
 public class MyProjectSeedInitializer : IDatabaseInitializer<MyProjectDataContext>
 {
     private Action<MyProjectDataContext> FixtureSeeder; 

     public MyProjectSeedInitializer()
     {
         
     }

     public MyProjectSeedInitializer(Action<MyProjectDataContext> fixtureSeeder)
     {
         FixtureSeeder = fixtureSeeder;
     }

     protected void Seed(MyProjectDataContext context)
     {
         // Populate DB with Fixture data, if passed in
         if (FixtureSeeder != null)
         {
             // Seeds the fixtures
             FixtureSeeder(context);
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
class RestaurantsFixtures
{
	[FixtureLoader]
	public static void Restaurants(dynamic fixtures)
	{
		fixtures.mishkinsRestaurant = new Restaurant()
		{
			Name = "Mishkin's Amazing Italian Food",
			OpeningTime = DateTime.Now().AddHours(-2),
			ClosingTime = DateTime.Now().AddHours(2)
		};
		
		// Add to our test context
		SpecHelper.AddFixtureToContext(fixtures.mishkinsRestaurant);
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
