#region Using Statements
using System;
using System.IO;
using System.Collections.ObjectModel;

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
        [Fact]
        public void Start()
        {
            bool result = CakeHelper.CreateServiceManager().Stop("MpsSvc");

            Assert.True(result, "Check Rights");
        }
    }
}
