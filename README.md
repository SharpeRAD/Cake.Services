# Cake.Services
Cake Addon that extends Cake with windows service commands

[![Build status](https://ci.appveyor.com/api/projects/status/bg004fntkfkjji83?svg=true)](https://ci.appveyor.com/project/PhillipSharpe/cake-services)

[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net/)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)



## Implemented functionality

This is a list of some the currently implemented functionality:

* Start
* Stop
* Restart
* Pause
* Continue
* Execute Command
* Get Service
* Get Status
* Install
* Uninstall
* Manage remote services



## Usage

```csharp
#addin "Cake.Services"


Task("Start-Service")
    .Description("Start a stopped windows service")
    .Does(() =>
{
    StartService("MpsSvc");
});

Task("Stop-Service")
    .Description("Stop a running windows service")
    .Does(() =>
{
    StopService("MpsSvc", "remote-location");
});

Task("Restart-Service")
    .Description("Restart a running windows service")
    .Does(() =>
{
    RestartService("MpsSvc");
});


Task("Pause-Service")
    .Description("Pause a running windows service")
    .Does(() =>
{
    PauseService("MpsSvc");
});

Task("Continue-Service")
    .Description("Continue a paused windows service")
    .Does(() =>
{
    ContinueService("MpsSvc", "remote-location");
});

Task("Execute-Command")
    .Description("Execute a command on a running service")
    .Does(() =>
{
    ExecuteServiceCommand("MyService", 4);
});


Task("Get-Service")
    .Description("Get a windows service object installed on a machine")
    .Does(() =>
{
    ServiceController controller = GetService("MpsSvc", "remote-location");

	if (controller != null)
	{
		controller.Stop();
	}
});

Task("Is-Service-Running")
    .Description("Check if a winodws service is running")
    .Does(() =>
{
    bool status = IsServiceRunning("MpsSvc");

	if (status)
	{
		Debug("YAY!");
	}
});

Task("Is-Service-Stopped")
    .Description("Check if a winodws service is stopped")
    .Does(() =>
{
    bool status = IsServiceStopped("MpsSvc");

	if (status)
	{
		Debug("YAY!");
	}
});


Task("Install-Service")
    .Description("Install a windows service")
    .Does(() =>
{
    InstallService("remote-location", new InstallSettings()
	{
		ServiceName = "Popup",
		DisplayName = "Annoying popup",
		Description = "Displays adds everytime you move the mouse",

		ExecutablePath = "C:/LOL/Popup.exe",
		StartMode = "auto",

		Username = "Admin",
		Password = "pass1"
	});
});

Task("Uninstall-Service")
    .Description("Uninstall a windows service")
    .Does(() =>
{
    UninstallService("Popup", "remote-location");
});


RunTarget("Start-Service");
```



## Example

A complete cake build example can be found [here](https://github.com/SharpeRAD/Cake.Services/blob/master/test/build.cake)
