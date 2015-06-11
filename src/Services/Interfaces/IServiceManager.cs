#region Using Statements
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.ServiceProcess;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Diagnostics;
    using Cake.Core.Annotations;
    using Cake.Powershell;
#endregion



namespace Cake.Services
{
    public interface IServiceManager
    {
        #region Functions (16)
            ServiceController GetService(string name, string computer = "");

            bool ServiceExists(string name, string computer = "");



            ServiceControllerStatus GetStatus(string name, string computer = "");

            bool IsRunning(string name, string computer = "");

            bool IsStopped(string name, string computer = "");

            bool CanPauseAndContinue(string name, string computer = "");

            bool CanStop(string name, string computer = "");

            bool CanShutdown(string name, string computer = "");



            bool Start(string name, string computer = "", int timeout = 60000, string[] args = null);

            bool Stop(string name, string computer = "", int timeout = 60000);

            bool Restart(string name, string computer = "", int timeout = 60000);



            bool Pause(string name, string computer = "", int timeout = 60000);

            bool Continue(string name, string computer = "", int timeout = 60000);



            bool ExecuteCommand(string name, string computer = "", int command = 0);



            void Install(string computer, InstallSettings settings);

            bool Uninstall(string name, string computer = "");
        #endregion
    }
}
