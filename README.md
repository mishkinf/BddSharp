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

### Creating Pages 
```csharp
 // Set your host properly (most times this is http://localhost:<portNum>)
 Nav.Port = "44444";
 Nav.Host = "http://localhost";
 // Easily set pages on Nav.Pages 
 Nav.Pages.Home = "/";
 Nav.Pages.About = "/About";
```

### Using a Generic Context

### Creating a Test Context

### Creating Test Fixtures

### Using Test Fixtures

## Examples

### SpecFlow & WaTin
*Integration Testing using Test Fixture Data*

### NUnit
*Unit Testing using Test Fixture Data*

### Jasmine
*JavaScript Testing*

## License

The MIT License Copyright (c) 2011 TrueCar, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
