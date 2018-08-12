#region Using Statements
using System.ServiceProcess;

using Xunit;
using Shouldly;

using Cake.Core;
using Cake.Powershell;
#endregion



namespace Cake.Services.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void Should_Service_IsInstalled()
        {
            IServiceManager manager = CakeHelper.CreateServiceManager();

            bool result1 = manager.IsInstalled("MpsSvc");
            result1.ShouldBeTrue();

            bool result2 = manager.IsInstalled("TestSer");
            result2.ShouldBeFalse();
        }

        [Fact]
        public void Should_Get_Service()
        {
            IServiceManager manager = CakeHelper.CreateServiceManager();

            ServiceController controller = manager.GetService("MpsSvc");
            controller.ShouldNotBeNull("Check Rights");
        }

        [Fact]
        public void Should_Change_Service_State()
        {
            IServiceManager manager = CakeHelper.CreateServiceManager();
            bool result = false;

            if (manager.IsRunning("MpsSvc"))
            {
                result = manager.Stop("MpsSvc");
            }
            else
            {
                result = manager.Start("MpsSvc");
            }

            result.ShouldBeTrue("Check Rights");
        }

        [Fact]
        public void Should_Construct_Install_String()
        {
            ServiceManager manager = (ServiceManager) CakeHelper.CreateServiceManager();

            var argumentBuilder = manager.CreateInstallArguments("", new InstallSettings()
            {
                ServiceName = "TestService",
                ExecutablePath = @"C:\my\path\to\bin.exe",
                DisplayName = "Test Service Display Name",
                Dependencies = "TestDependencies",
                Username = "TestUsername",
                Password = "TestPasswordPassword",
                StartMode = "TestStartMode"
            });

            var actual = argumentBuilder.Render();
            System.Diagnostics.Debug.WriteLine(actual);

            var expected = @"""TestService"" binPath= ""C:/my/path/to/bin.exe"" DisplayName= ""Test Service Display Name"" depend= ""TestDependencies"" start= ""TestStartMode"" obj= ""TestUsername"" password= ""TestPasswordPassword""";
            expected.ShouldBe(actual);
        }

        [Fact]
        public void Should_Construct_Intsall_String_WithArgs()
        {
            ServiceManager manager = (ServiceManager) CakeHelper.CreateServiceManager();

            var argumentBuilder = manager.CreateInstallArguments("", new InstallSettings()
            {
                ServiceName = "TestService",
                ExecutablePath = @"C:\my\path\to\bin.exe",
                DisplayName = "Test Service Display Name",
                Dependencies = "TestDependencies",
                Username = "TestUsername",
                Password = "TestPasswordPassword",
                StartMode = "TestStartMode"
            }
            .WithArguments( args => 
            {
                args.AppendQuoted("CustomName", "Bob");
            }));

            var actual = argumentBuilder.Render();
            System.Diagnostics.Debug.WriteLine(actual);

            var expected = @"""TestService"" binPath= '\""C:/my/path/to/bin.exe\"" -CustomName \""Bob\""' DisplayName= ""Test Service Display Name"" depend= ""TestDependencies"" start= ""TestStartMode"" obj= ""TestUsername"" password= ""TestPasswordPassword""";
            expected.ShouldBe(actual);
        }
    }
}
