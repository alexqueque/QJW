# QJW

QJW is a C# utility library for .NET Framework projects. It collects common helpers used in classic ASP.NET and desktop/server-side applications, including dynamic JSON objects, cookies, files, networking, strings, sessions, IP handling, and other reusable utilities.

The repository contains the `QJW` class library and a small `QJW.Demo` console project.

## NuGet Status

QJW is published on NuGet:

- Package: [QJW](https://www.nuget.org/packages/QJW)
- Latest version: `1.0.3`
- Total downloads: `2,038`
- Current version downloads: `273`
- Owner: `quejuwen`

Install it with the .NET CLI:

```bash
dotnet add package QJW --version 1.0.3
```

Or with the Visual Studio Package Manager Console:

```powershell
NuGet\Install-Package QJW -Version 1.0.3
```

## Features

- Dynamic JSON/object helper through `ClayObject.Clay`
- ASP.NET-oriented helpers for cookies, sessions, pages, paths, cache, uploads, downloads, and validation
- File, directory, FTP, ZIP, XML, CSV, image, email, and network utilities
- String, date/time, random, RMB amount, encryption, barcode, QR code, and pinyin helpers
- Demo project showing how to create, update, delete, enumerate, serialize, and deserialize `Clay` dynamic objects

> Note: the source tree includes many historical helper files under `QJW/Common` and `QJW/Tools`. The current `QJW/QJW.csproj` compiles a smaller selected subset. If you need another helper in the output DLL, add the corresponding `.cs` file to the project before building.

## Project Structure

```text
QJW/
|-- QJW.sln                 # Visual Studio solution
|-- QJW/                    # .NET Framework class library
|   |-- Clay/               # Dynamic JSON/object implementation
|   |-- Common/             # Utility helper source files
|   |-- Tools/              # Additional helper classes
|   |-- QJW.csproj
|   `-- packages.config
|-- QJW.Demo/               # Console demo project
`-- LICENSE                 # GPL-3.0 license
```

## Requirements

- Visual Studio 2013 or newer, or MSBuild compatible with legacy `.csproj` projects
- .NET Framework 4.6.2 Developer Pack
- NuGet package restore enabled

Main NuGet dependencies include:

- `Newtonsoft.Json`
- `CYQ.Data`
- `System.Text.Json`
- `Microsoft.Bcl.AsyncInterfaces`
- related `System.*` support packages for .NET Framework 4.6.2

## Build

Open `QJW.sln` in Visual Studio and build the solution.

Or build from a Developer Command Prompt:

```bat
nuget restore QJW.sln
msbuild QJW.sln /p:Configuration=Release
```

The compiled library is generated at:

```text
QJW/bin/Release/QJW.dll
```

## Quick Start

Reference `QJW.dll` from your .NET Framework project, then import the namespace for the helper you need.

### Dynamic JSON with Clay

```csharp
using ClayObject;
using System;
using System.Collections.Generic;

dynamic clay = Clay.Parse(@"{""Foo"":""json"", ""Bar"":100, ""Nest"":{""Foobar"":true}}");

Console.WriteLine(clay.Foo);
Console.WriteLine(clay["Nest"]["Foobar"]);

clay.Arr = new[] { "NOR", "XOR" };
clay.Foo = "QJW";
clay.Delete("Bar");

foreach (KeyValuePair<string, dynamic> item in clay)
{
    Console.WriteLine(item.Key + ": " + item.Value);
}

string json = clay.ToString();
```

### Create a Clay Object from C# Data

```csharp
using ClayObject;

var user = Clay.Object(new
{
    Id = 1,
    Name = "Alice",
    Profile = new
    {
        Enabled = true
    }
});

user.Tags = new[] { "admin", "tester" };
var json = user.ToString();
```

### Common Helpers

```csharp
using QJW.Common;

string clientIp = IpHelper.GetUserIp();
string random = RandomHelper.GetRandomString(8);
```

Available helper namespaces include:

- `QJW.Common`
- `QJW.Tools`
- `ClayObject`
- `ClayObject.Extensions`

## Demo

`QJW.Demo/Program.cs` contains a runnable example for `ClayObject.Clay`.

Build and run the demo from Visual Studio, or set `QJW.Demo` as the startup project and press `F5`.

## Notes

- This is a legacy .NET Framework project, not an SDK-style .NET project.
- Several helpers depend on ASP.NET `System.Web`, so they are intended for .NET Framework applications.
- Some helper files may require extra project references or NuGet packages if you include them in `QJW.csproj`.

## License

This project is licensed under the GNU General Public License v3.0. See [LICENSE](LICENSE) for details.
