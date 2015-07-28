#region Using Statements
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceProcess;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Annotations;
    using Cake.Powershell;
#endregion



namespace Cake.Services
{
    [CakeAliasCategory("Services")]
    public static class CakeAliases
    {
        private static ServiceManager CreateManager(this ICakeContext context)
        {
            return new ServiceManager(context.Environment, context.Log, new PowershellRunner(context.Environment, context.Log));
        }



        /// <summary>
        /// Gets the <see cref="ServiceController"/> that is associated with an existing service on the specified computer.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>The <see cref="ServiceController"/> that is associated with an existing service on the specified computer.</returns>
        [CakeMethodAlias]
        public static ServiceController GetService(this ICakeContext context, string name)
        {
            return context.CreateManager().GetService(name, "");
        }

        /// <summary>
        /// Gets the <see cref="ServiceController"/> that is associated with an existing service on the specified computer.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>The <see cref="ServiceController"/> that is associated with an existing service on the specified computer.</returns>
        [CakeMethodAlias]
        public static ServiceController GetService(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().GetService(name, computer);
        }

        /// <summary>
        /// Gets the <see cref="ServiceControllerStatus"/> status of a service on the specified computer.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>The <see cref="ServiceControllerStatus"/> status of a service on the specified computer.</returns>
        [CakeMethodAlias]
        public static ServiceControllerStatus GetServiceStatus(this ICakeContext context, string name)
        {
            return context.CreateManager().GetStatus(name, "");
        }
        
        /// <summary>
        /// Gets the <see cref="ServiceControllerStatus"/> status of a service on the specified computer.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>The <see cref="ServiceControllerStatus"/> status of a service on the specified computer.</returns>
        [CakeMethodAlias]
        public static ServiceControllerStatus GetServiceStatus(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().GetStatus(name, computer);
        }



        /// <summary>
        /// Checks if the named service is installed
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service is installed.</returns>
        [CakeMethodAlias]
        public static bool IsServiceInstalled(this ICakeContext context, string name)
        {
            return context.CreateManager().IsRunning(name, "");
        }

        /// <summary>
        /// Checks if the named service is installed
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is installed.</returns>
        [CakeMethodAlias]
        public static bool IsServiceInstalled(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().IsRunning(name, computer);
        }

        /// <summary>
        /// Checks if the named service is running
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service is running.</returns>
        [CakeMethodAlias]
        public static bool IsServiceRunning(this ICakeContext context, string name)
        {
            return context.CreateManager().IsRunning(name, "");
        }

        /// <summary>
        /// Checks if the named service is running
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is running.</returns>
        [CakeMethodAlias]
        public static bool IsServiceRunning(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().IsRunning(name, computer);
        }

        /// <summary>
        /// Checks if the named service is stopped
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service is stopped.</returns>
        [CakeMethodAlias]
        public static bool IsServiceStopped(this ICakeContext context, string name)
        {
            return context.CreateManager().IsStopped(name, "");
        }

        /// <summary>
        /// Checks if the named service is stopped
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is stopped.</returns>
        [CakeMethodAlias]
        public static bool IsServiceStopped(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().IsStopped(name, computer);
        }



        /// <summary>
        /// Checks if the named service can be paused and continued
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service can be paused and continued.</returns>
        [CakeMethodAlias]
        public static bool CanServicePauseAndContinue(this ICakeContext context, string name)
        {
            return context.CreateManager().CanPauseAndContinue(name);
        }

        /// <summary>
        /// Checks if the named service can be paused and continued
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service can be paused and continued.</returns>
        [CakeMethodAlias]
        public static bool CanServicePauseAndContinue(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().CanPauseAndContinue(name, computer);
        }

        /// <summary>
        /// Checks if the named service can be stopped
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service can be stopped.</returns>
        [CakeMethodAlias]
        public static bool CanServiceStop(this ICakeContext context, string name)
        {
            return context.CreateManager().CanStop(name);
        }

        /// <summary>
        /// Checks if the named service can be stopped
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service can be stopped.</returns>
        [CakeMethodAlias]
        public static bool CanServiceStop(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().CanStop(name, computer);
        }

        /// <summary>
        /// Checks if the named service can be shutdown
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service can be shutdown.</returns>
        [CakeMethodAlias]
        public static bool CanServiceShutdown(this ICakeContext context, string name)
        {
            return context.CreateManager().CanShutdown(name);
        }

        /// <summary>
        /// Checks if the named service can be shutdown
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service can be shutdown.</returns>
        [CakeMethodAlias]
        public static bool CanServiceShutdown(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().CanShutdown(name, computer);
        }



        /// <summary>
        /// Starts a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service was started.</returns>
        [CakeMethodAlias]
        public static bool StartService(this ICakeContext context, string name)
        {
            return context.CreateManager().Start(name, "", 60000, null);
        }

        /// <summary>
        /// Starts a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service was started.</returns>
        [CakeMethodAlias]
        public static bool StartService(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().Start(name, computer, 60000, null);
        }

        /// <summary>
        /// Starts a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was started.</returns>
        [CakeMethodAlias]
        public static bool StartService(this ICakeContext context, string name, string computer, int timeout)
        {
            return context.CreateManager().Start(name, computer, timeout, null);
        }

        /// <summary>
        /// Starts a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <param name="args">The arguments to pass to the service when starting</param>
        /// <returns>If the service was started.</returns>
        [CakeMethodAlias]
        public static bool StartService(this ICakeContext context, string name, string computer, int timeout, string[] args)
        {
            return context.CreateManager().Start(name, computer, timeout, args);
        }

        /// <summary>
        /// Stops a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service was stopped.</returns>
        [CakeMethodAlias]
        public static bool StopService(this ICakeContext context, string name)
        {
            return context.CreateManager().Stop(name, "", 60000);
        }

        /// <summary>
        /// Stops a named service
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service was stopped.</returns>
        [CakeMethodAlias]
        public static bool StopService(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().Stop(name, computer, 60000);
        }

        /// <summary>
        /// Stops a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was stopped.</returns>
        [CakeMethodAlias]
        public static bool StopService(this ICakeContext context, string name, string computer, int timeout)
        {
            return context.CreateManager().Stop(name, computer, timeout);
        }

        /// <summary>
        /// Restarts a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service was restarted.</returns>
        [CakeMethodAlias]
        public static bool RestartService(this ICakeContext context, string name)
        {
            return context.CreateManager().Restart(name, "", 60000);
        }

        /// <summary>
        /// Restarts a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service was restarted.</returns>
        [CakeMethodAlias]
        public static bool RestartService(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().Restart(name, computer, 60000);
        }

        /// <summary>
        /// Restarts a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was restarted.</returns>
        [CakeMethodAlias]
        public static bool RestartService(this ICakeContext context, string name, string computer, int timeout)
        {
            return context.CreateManager().Restart(name, computer, timeout);
        }



        /// <summary>
        /// Pauses a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service was paused.</returns>
        [CakeMethodAlias]
        public static bool PauseService(this ICakeContext context, string name)
        {
            return context.CreateManager().Pause(name, "", 60000);
        }
        
        /// <summary>
        /// Pauses a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service was paused.</returns>
        [CakeMethodAlias]
        public static bool PauseService(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().Pause(name, computer, 60000);
        }
        
        /// <summary>
        /// Pauses a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was paused.</returns>
        [CakeMethodAlias]
        public static bool PauseService(this ICakeContext context, string name, string computer, int timeout)
        {
            return context.CreateManager().Pause(name, computer, timeout);
        }
        
        /// <summary>
        /// Continues a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service was continued.</returns>
        [CakeMethodAlias]
        public static bool ContinueService(this ICakeContext context, string name)
        {
            return context.CreateManager().Continue(name, "", 60000);
        }
        
        /// <summary>
        /// Continues a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service was continued.</returns>
        [CakeMethodAlias]
        public static bool ContinueService(this ICakeContext context, string name, string computer)
        {
            return context.CreateManager().Continue(name, computer, 60000);
        }
        
        /// <summary>
        /// Continues a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was continued.</returns>
        [CakeMethodAlias]
        public static bool ContinueService(this ICakeContext context, string name, string computer, int timeout)
        {
            return context.CreateManager().Continue(name, computer, timeout);
        }



        /// <summary>
        /// Executes a command on a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>If the command was executed.</returns>
        [CakeMethodAlias]
        public static bool ExecuteServiceCommand(this ICakeContext context, string name, int command)
        {
            return context.CreateManager().ExecuteCommand(name, "", command);
        }
        
        /// <summary>
        /// Executes a command on a named service
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>If the command was executed.</returns>
        [CakeMethodAlias]
        public static bool ExecuteServiceCommand(this ICakeContext context, string name, string computer, int command)
        {
            return context.CreateManager().ExecuteCommand(name, computer, command);
        }



        /// <summary>
        /// Installs a service on a computer
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="settings">The settings to use when installing the service.</param>
        /// <returns>If the service was installed.</returns>
        [CakeMethodAlias]
        public static void InstallService(this ICakeContext context, InstallSettings settings)
        {
            context.CreateManager().Install(settings);
        }

        /// <summary>
        /// Installs a service on a computer
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="settings">The settings to use when installing the service.</param>
        /// <returns>If the service was installed.</returns>
        [CakeMethodAlias]
        public static void InstallService(this ICakeContext context, string computer, InstallSettings settings)
        {
            context.CreateManager().Install(computer, settings);
        }
        
        /// <summary>
        /// Uninstalls a service from a computer
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <returns>If the service was uninstalled.</returns>
        [CakeMethodAlias]
        public static void UninstallService(this ICakeContext context, string name)
        {
            context.CreateManager().Uninstall(name, "");
        }
        
        /// <summary>
        /// Uninstalls a service from a computer
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service was uninstalled.</returns>
        [CakeMethodAlias]
        public static void UninstallService(this ICakeContext context, string name, string computer)
        {
            context.CreateManager().Uninstall(name, computer);
        }
    }
}
