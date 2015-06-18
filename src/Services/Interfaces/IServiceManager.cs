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
            /// <summary>
            /// Gets the <see cref="ServiceController"/> that is associated with an existing service on the specified computer.
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>The <see cref="ServiceController"/> that is associated with an existing service on the specified computer.</returns>
            ServiceController GetService(string name, string computer = "");

            /// <summary>
            /// Checks if the named service exists
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>If the service exists.</returns>
            bool ServiceExists(string name, string computer = "");



            /// <summary>
            /// Gets the <see cref="ServiceControllerStatus"/> status of a service on the specified computer.
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>The <see cref="ServiceControllerStatus"/> status of a service on the specified computer.</returns>
            ServiceControllerStatus GetStatus(string name, string computer = "");

            /// <summary>
            /// Checks if the named service is running
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>If the service is running.</returns>
            bool IsRunning(string name, string computer = "");

            /// <summary>
            /// Checks if the named service is stopped
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>If the service is stopped.</returns>
            bool IsStopped(string name, string computer = "");



            /// <summary>
            /// Checks if the named service can be paused and continued
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>If the service can be paused and continued.</returns>
            bool CanPauseAndContinue(string name, string computer = "");

            /// <summary>
            /// Checks if the named service can be stopped
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>If the service can be stopped.</returns>
            bool CanStop(string name, string computer = "");

            /// <summary>
            /// Checks if the named service can be shutdown
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>If the service can be shutdown.</returns>
            bool CanShutdown(string name, string computer = "");



            /// <summary>
            /// Starts a named service
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <param name="timeout">The duration to wait for the command to complete.</param>
            /// <param name="args">The arguments to pass to the service when starting</param>
            /// <returns>If the service was started.</returns>
            bool Start(string name, string computer = "", int timeout = 60000, string[] args = null);

            /// <summary>
            /// Stops a named service
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <param name="timeout">The duration to wait for the command to complete.</param>
            /// <returns>If the service was stopped.</returns>
            bool Stop(string name, string computer = "", int timeout = 60000);

            /// <summary>
            /// Restarts a named service
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <param name="timeout">The duration to wait for the command to complete.</param>
            /// <returns>If the service was restarted.</returns>
            bool Restart(string name, string computer = "", int timeout = 60000);



            /// <summary>
            /// Pauses a named service
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <param name="timeout">The duration to wait for the command to complete.</param>
            /// <returns>If the service was paused.</returns>
            bool Pause(string name, string computer = "", int timeout = 60000);

            /// <summary>
            /// Continues a named service
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <param name="timeout">The duration to wait for the command to complete.</param>
            /// <returns>If the service was continued.</returns>
            bool Continue(string name, string computer = "", int timeout = 60000);



            /// <summary>
            /// Executes a command on a named service
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <param name="command">The command to execute.</param>
            /// <returns>If the command was executed.</returns>
            bool ExecuteCommand(string name, string computer = "", int command = 0);



            /// <summary>
            /// Installs a service on a computer
            /// </summary>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <param name="settings">The settings to use when installing the service.</param>
            /// <returns>If the service was installed.</returns>
            void Install(string computer, InstallSettings settings);

            /// <summary>
            /// Uninstalls a service from a computer
            /// </summary>
            /// <param name="name">The name that identifies the service to the system.</param>
            /// <param name="computer">The computer on which the service resides.</param>
            /// <returns>If the service was uninstalled.</returns>
            bool Uninstall(string name, string computer = "");
        #endregion
    }
}
