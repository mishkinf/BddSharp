BddSharp - A Simple Behavior Driven Development Setup for C#

Instructions: 

 1) Incorporate 'BddSharp' NuGet package in your project by using the NuGet package manager or manually compiling the DLL and including the reference in your project

Create A Test Server

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


License

The MIT License Copyright (c) 2011 TrueCar, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
