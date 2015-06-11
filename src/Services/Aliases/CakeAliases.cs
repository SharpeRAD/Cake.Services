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



        [CakeMethodAlias]
        public static ServiceController GetService(this ICakeContext context, string name, string computer = "")
        {
            return context.CreateManager().GetService(name, computer);
        }



        [CakeMethodAlias]
        public static ServiceControllerStatus GetServiceStatus(this ICakeContext context, string name, string computer = "")
        {
            return context.CreateManager().GetStatus(name, computer);
        }

        [CakeMethodAlias]
        public static bool IsServiceRunning(this ICakeContext context, string name, string computer = "")
        {
            return context.CreateManager().IsRunning(name, computer);
        }

        [CakeMethodAlias]
        public static bool IsServiceStopped(this ICakeContext context, string name, string computer = "")
        {
            return context.CreateManager().IsStopped(name, computer);
        }



        [CakeMethodAlias]
        public static bool StartService(this ICakeContext context, string name, string computer = "", int timeout = 60000, string[] args = null)
        {
            return context.CreateManager().Start(name, computer, timeout, args);
        }

        [CakeMethodAlias]
        public static bool StopService(this ICakeContext context, string name, string computer = "", int timeout = 60000)
        {
            return context.CreateManager().Stop(name, computer, timeout);
        }

        [CakeMethodAlias]
        public static bool RestartService(this ICakeContext context, string name, string computer = "", int timeout = 60000)
        {
            return context.CreateManager().Restart(name, computer, timeout);
        }



        [CakeMethodAlias]
        public static bool PauseService(this ICakeContext context, string name, string computer = "", int timeout = 60000)
        {
            return context.CreateManager().Pause(name, computer, timeout);
        }

        [CakeMethodAlias]
        public static bool ContinueService(this ICakeContext context, string name, string computer = "", int timeout = 60000)
        {
            return context.CreateManager().Continue(name, computer, timeout);
        }



        [CakeMethodAlias]
        public static bool ExecuteServiceCommand(this ICakeContext context, string name, int command)
        {
            return context.CreateManager().ExecuteCommand(name, "", command);
        }

        [CakeMethodAlias]
        public static bool ExecuteServiceCommand(this ICakeContext context, string name, string computer, int command)
        {
            return context.CreateManager().ExecuteCommand(name, computer, command);
        }



        [CakeMethodAlias]
        public static void InstallService(this ICakeContext context, string computer, InstallSettings settings)
        {
            context.CreateManager().Install(computer, settings);
        }

        [CakeMethodAlias]
        public static void UninstallService(this ICakeContext context, string name, string computer = "")
        {
            context.CreateManager().Uninstall(name, computer);
        }
    }
}
