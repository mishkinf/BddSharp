# BddSharp
 *A Simple Behavior Driven Development Setup for C#*

BddSharp is intended to make setting up a behavior driven test development environment for ASP.net applications much simpler (particularly those using Entity Framework). 

### Credits
Mishkin Faustini, Author

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
 Nav.Pages.About = "/About";
```

### Using a Generic Repository
For more information about the Repository pattern please read [Implementing the Repository and Unit of Work Patterns in an ASP.NET MVC Application](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application).
Your web application must be using a generic repository in order to gain the BDD benefits that this framework provides. 

### Creating a Test Repository
A test context is substituted for unit testing with data fixtures. This gives us tests that run in memory and thus very speedily since they are not polling/resetting the database.

### Creating Test Data Fixtures
Idealy your tests should be driven against test data. Test data fixtures provide the ability to simply generate data which is used for in memory unit testing and for seeding the test database that runs the SpecFlow/WatiN integration tests.

### Using Test Data Fixtures
Test data can be easily referenced in any of your tests


## Examples

### SpecFlow & WatiN
*Integration Testing using Test Fixture Data*

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
