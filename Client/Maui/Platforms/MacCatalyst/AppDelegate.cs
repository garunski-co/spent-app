﻿using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Spent.Client.Maui.Platforms.MacCatalyst;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
