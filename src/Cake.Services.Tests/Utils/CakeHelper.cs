#region Using Statements
using System.IO;

using Cake.Core;
using Cake.Testing;
using Cake.Powershell;
#endregion



namespace Cake.Services.Tests
{
    internal static class CakeHelper
    {
        #region Methods
        public static ICakeEnvironment CreateEnvironment()
        {
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            environment.WorkingDirectory = Directory.GetCurrentDirectory();
            environment.WorkingDirectory = environment.WorkingDirectory.Combine("../../../");

            return environment;
        }



        public static IPowershellRunner CreatePowershellRunner()
        {
            return new PowershellRunner(CakeHelper.CreateEnvironment(), new DebugLog());
        }

        public static IServiceManager CreateServiceManager()
        {
            return new ServiceManager(CakeHelper.CreateEnvironment(), new DebugLog(), CakeHelper.CreatePowershellRunner());
        }
        #endregion
    }
}
