# <img src="docs/images/logo.svg" alt="" width="32" /> paralax.Rin — .NET 10 fork of Rin

[![Build-Release](https://github.com/paralaxsd/Rin/workflows/Build-Release/badge.svg)](https://github.com/paralaxsd/Rin/actions?query=workflow%3ABuild-Release)
[![Build-Development](https://github.com/paralaxsd/Rin/workflows/Build-Development/badge.svg)](https://github.com/paralaxsd/Rin/actions?query=workflow%3ABuild-Development)
[![NuGet version](https://badge.fury.io/nu/paralax.Rin.svg)](https://badge.fury.io/nu/paralax.Rin)

> **This is a personal fork of [mayuki/Rin](https://github.com/mayuki/Rin) (v2.6.0), ported to .NET 10.**
>
> I made this because the original project has been inactive since 2021 and I needed it to work with .NET 10.
> **I do not maintain this project** beyond my own needs — use at your own risk.
> That said, feel free to copy, fork, or build on this work. MIT license, go nuts.
>
> Notable changes vs. original:
> - All packages target **net10.0** only
> - NuGet dependencies updated to current versions
> - Fixed `CapturePipeWriter` for .NET 10 (`CanGetUnflushedBytes`, `UnflushedBytes`) — required by `System.Text.Json`'s new PipeWriter serialization path
> - `Rin.Extensions.MagicOnion` is included but **not updated** — the MagicOnion API changes were breaking and I don't use it
> - Published as `paralax.*` packages to avoid collision with the original

---

**R**equest/response **In**spector middleware for ASP.NET Core. like Glimpse.

Rin captures HTTP requests to your ASP.NET Core app and provides a web-based viewer for captured data. Useful for debugging Web applications (API apps, web sites, etc.).

![](docs/images/Demo-01.gif)

# ✅ Features
## Capture requests and responses
- Headers + Body
- Traces (`Microsoft.Extensions.Logging.ILogger`, log4net, ...)
- Unhandled Exceptions
- Entity Framework Core queries (via `paralax.Rin.Extensions.EntityFrameworkCore`)

## Inspect from a browser in realtime

### View events timeline
![](docs/images/Screenshot-02.png)

### Preview request/response body
Rin inspector can display request and response body with a preview (JSON, Image, HTML, JavaScript ...).

![](docs/images/Screenshot-03.png)

### View related trace logs
- Built-in `Microsoft.Extensions.Logging.ILogger` integration
- log4net Appender

### Save and export request/response
- Save request/response body
- Copy request as cURL and C#

### Integrate with ASP.NET Core MVC
- Record timings of view rendering and action execution
- In-View Inspector (like MiniProfiler)

![](docs/images/Screenshot-04.png)

# 📝 Requirements
- .NET 10+
- ASP.NET Core 10+
- Modern browser (Edge, Chrome, Firefox, Safari)
    - WebSocket connectivity

# ⚡ QuickStart

## Install NuGet Package
```
dotnet add package paralax.Rin
dotnet add package paralax.Rin.Mvc  # if using ASP.NET Core MVC
```

## Setup

### Program.cs (minimal hosting)
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddRinLogger();
builder.Services.AddRin();
builder.Services.AddControllersWithViews()
    .AddRinMvcSupport(); // optional, MVC only

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseRin();           // must be at the top of the pipeline
    app.UseRinMvcSupport(); // optional, MVC only
    app.UseDeveloperExceptionPage();
    app.UseRinDiagnosticsHandler(); // must be after UseDeveloperExceptionPage
}
```

### _Layout.cshtml (ASP.NET Core MVC only)
```cshtml
@inject Rin.Mvc.View.RinHelperService RinHelper
...
    <environment include="Development">
        @* Add: Enable In-View Inspector *@
        @RinHelper.RenderInViewInspector()
    </environment>
```

Open `http://[Host:Port]/rin/` to access the Rin Inspector.

# 🔨 Building locally

Frontend assets are not checked in and must be built before `dotnet build`. Run from the repo root:

```powershell
.\build-frontend.ps1
dotnet build
```

The script sets `NODE_OPTIONS=--openssl-legacy-provider` which is required because the frontend uses Webpack 4, incompatible with Node.js 17+ default OpenSSL settings.

# License
[MIT License](LICENSE) — original work by Mayuki Sawatari, .NET 10 port by paralax.
