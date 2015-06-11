#region Using Statements
    using System;
    using System.IO;
    using System.Collections.ObjectModel;
    using System.ServiceProcess;

    using Xunit;

    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using Cake.Core.Diagnostics;
    using Cake.Core.IO;
    using Cake.Powershell;
#endregion



namespace Cake.Services.Tests
{
    public class ServiceTests
    {
        /*
        [Fact]
        public void GetService()
        {
            IServiceManager manager = CakeHelper.CreateServiceManager();

            ServiceController controller = manager.GetService("MpsSvc");

            Assert.True(controller != null, "Check Rights");
        }

        [Fact]
        public void ChangeService()
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

            Assert.True(result, "Check Rights");
        }
        */
    }
}
