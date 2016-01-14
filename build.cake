#addin "Cake.Slack"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var tools = Argument("tools", "./tools");

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var appName = "Cake.Services";





//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////

// Get whether or not this is a local build.
var local = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Get version.
var buildNumber = AppVeyor.Environment.Build.Number;
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));

// Define directories.
var buildDir = "./src/Services/bin/" + configuration;
var buildResultDir = "./build/v" + semVersion;
var testResultsDir = buildResultDir + "/test-results";
var nugetRoot = buildResultDir + "/nuget";
var binDir = buildResultDir + "/bin";

// Get Solutions
var solutions       = GetFiles("./**/*.sln");

// Package
var zipPackage = buildResultDir + "/Cake-Services-v" + semVersion + ".zip";
var nugetPackage = nugetRoot + "/Cake.Services." + version + ".nupkg";



///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
	//Executed BEFORE the first task.
	Information("Building version {0} of {1}.", semVersion, appName);

	NuGetInstall("xunit.runner.console", new NuGetInstallSettings 
	{
		ExcludeVersion  = true,
		OutputDirectory = tools
    });
});

Teardown(() =>
{
	// Executed AFTER the last task.
	Information("Finished building version {0} of {1}.", semVersion, appName);
});





///////////////////////////////////////////////////////////////////////////////
// PREPARE
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
    // Clean solution directories.
	Information("Cleaning old files");
	CleanDirectories(new DirectoryPath[] 
	{
        buildResultDir, binDir, testResultsDir, nugetRoot
	});
});

Task("Restore-Nuget-Packages")
	.IsDependentOn("Clean")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}", solution);
        NuGetRestore(solution);
    }
});





///////////////////////////////////////////////////////////////////////////////
// BUILD
///////////////////////////////////////////////////////////////////////////////

Task("Patch-Assembly-Info")
    .IsDependentOn("Restore-Nuget-Packages")
    .Does(() =>
{
    var file = "./src/SolutionInfo.cs";

    CreateAssemblyInfo(file, new AssemblyInfoSettings 
    {
		Product = appName,
        Version = version,
        FileVersion = version,
        InformationalVersion = semVersion,
        Copyright = "Copyright (c) Phillip Sharpe 2015"
    });
});

Task("Build")
    .IsDependentOn("Patch-Assembly-Info")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
		Information("Building {0}", solution);
		MSBuild(solution, settings => 
			settings.SetPlatformTarget(PlatformTarget.MSIL)
				.WithProperty("TreatWarningsAsErrors","true")
				.WithTarget("Build")
				.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./src/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings 
    {
        OutputDirectory = testResultsDir,
        XmlReportV1 = true
    });
});





///////////////////////////////////////////////////////////////////////////////
// PACKAGE
///////////////////////////////////////////////////////////////////////////////

Task("Copy-Files")
    .IsDependentOn("Build")
    .Does(() =>
{
    CopyFileToDirectory(buildDir + "/Cake.Powershell.dll", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Services.dll", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Services.pdb", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Services.xml", binDir);

    CopyFileToDirectory("./lib/System.ServiceProcess.dll", binDir);
    CopyFileToDirectory("./lib/System.Management.Automation.dll", binDir);

    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);



	CreateDirectory("./test/tools/Addins/Cake.Services/lib/net45/");

	CopyFileToDirectory(buildDir + "/Cake.Services.dll", "./test/tools/Addins/Cake.Services/lib/net45/");
    CopyFileToDirectory(buildDir + "/Cake.Powershell.dll", "./test/tools/Addins/Cake.Services/lib/net45/");
    CopyFileToDirectory("./lib/System.ServiceProcess.dll", "./test/tools/Addins/Cake.Services/lib/net45/");
	CopyFileToDirectory("./lib/System.Management.Automation.dll", "./test/tools/Addins/Cake.Services/lib/net45/");
});

Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{
    Zip(binDir, zipPackage);
});



Task("Create-NuGet-Packages")
    .IsDependentOn("Zip-Files")
    .Does(() =>
{
    NuGetPack("./nuspec/Cake.Services.nuspec", new NuGetPackSettings 
    {
        Version = version,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,        
        Symbols = false,
        NoPackageAnalysis = true
    });
});

Task("Publish-Nuget")
	.IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => !isPullRequest) 
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("NUGET_API_KEY");

    if(string.IsNullOrEmpty(apiKey)) 
	{
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }



    // Push the package.
    NuGetPush(nugetPackage, new NuGetPushSettings 
	{
        ApiKey = apiKey
    }); 
});

Task("Publish-MyGet")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => !isPullRequest) 
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("MYGET_API_KEY");
	
    if(string.IsNullOrEmpty(apiKey)) 
	{
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

	
	
    // Push the package.
    NuGetPush(nugetPackage, new NuGetPushSettings 
	{
        Source = "https://www.myget.org/F/cake/api/v2/package",
        ApiKey = apiKey
    });
});





///////////////////////////////////////////////////////////////////////////////
// APPVEYOR
///////////////////////////////////////////////////////////////////////////////

Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(semVersion);
}); 

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Zip-Files")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UploadArtifact(zipPackage);
}); 





///////////////////////////////////////////////////////////////////////////////
// ALERT
///////////////////////////////////////////////////////////////////////////////

Task("Slack")
    .Does(() =>
{
    // Resolve the API key.
    var token = EnvironmentVariable("SLACK_TOKEN");

    if(string.IsNullOrEmpty(token)) 
	{
        throw new InvalidOperationException("Could not resolve Slack token.");
    }



	// Post Message
	var text = "Published " + appName + " v" + version;

	var result = Slack.Chat.PostMessage(token, "#code", text);

	if (result.Ok)
	{
		//Posted
		Information("Message was succcessfully sent to Slack.");
	}
	else
	{
		//Error
		Error("Failed to send message to Slack: {0}", result.Error);
	}
});





//////////////////////////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////////////////////////

Task("Package")
	.IsDependentOn("Zip-Files")
    .IsDependentOn("Create-NuGet-Packages");

Task("Publish")
    .IsDependentOn("Publish-Nuget")
	.IsDependentOn("Publish-MyGet");

Task("AppVeyor")
	.IsDependentOn("Publish")
    .IsDependentOn("Update-AppVeyor-Build-Number")
    .IsDependentOn("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Slack");
    


Task("Default")
    .IsDependentOn("Package");





///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);