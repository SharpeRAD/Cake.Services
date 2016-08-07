#region Using Statements
using System;
using System.Linq;
using System.ServiceProcess;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Diagnostics;
using Cake.Powershell;
#endregion



namespace Cake.Services
{
    /// <summary>
    /// Responsible for controlling windows services
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        #region Fields (3)
        private readonly ICakeEnvironment _Environment;
        private readonly ICakeLog _Log;

        private readonly IPowershellRunner _PowershellRunner;
        #endregion





        #region Constructor (1)
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceManager" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="powershellRunner">The powershell runner.</param>
        public ServiceManager(ICakeEnvironment environment, ICakeLog log, IPowershellRunner powershellRunner)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (powershellRunner == null)
            {
                throw new ArgumentNullException("powershellRunner");
            }

            _Environment = environment;
            _Log = log;
            _PowershellRunner = powershellRunner;
        }
        #endregion





        #region Functions (10)
        /// <summary>
        /// Gets the <see cref="ServiceController"/> that is associated with an existing service on the specified computer.
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>The <see cref="ServiceController"/> that is associated with an existing service on the specified computer.</returns>
        public ServiceController GetService(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            if (String.IsNullOrEmpty(computer))
            {
                return new ServiceController(name);
            }
            else
            {
                return new ServiceController(name, computer);
            }
        }

        /// <summary>
        /// Checks if the named service exists
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service exists.</returns>
        public bool ServiceExists(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController[] services = null;

            if (String.IsNullOrEmpty(computer))
            {
                services = ServiceController.GetServices();
            }
            else
            {
                services = ServiceController.GetServices(computer);
            }

            if (services.FirstOrDefault(s => s.ServiceName == name) != null)
            {
                _Log.Information(string.Format("The service {0} exists.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} does not exist.", name));
                return false;
            }
        }



        /// <summary>
        /// Gets the <see cref="ServiceControllerStatus"/> status of a service on the specified computer.
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>The <see cref="ServiceControllerStatus"/> status of a service on the specified computer.</returns>
        public ServiceControllerStatus GetStatus(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            _Log.Information(string.Format("Getting the status of the service {0}.", name));

            return service.Status;
        }

        /// <summary>
        /// Checks if the named service is continuing
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is continuing.</returns>
        public bool IsContinuing(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceControllerStatus status = this.GetStatus(name, computer);

            if (status == ServiceControllerStatus.ContinuePending)
            {
                _Log.Information(string.Format("The service {0} is continuing.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} is not continuing!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service is installed
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is installed.</returns>
        public bool IsInstalled(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController controller = this.GetService(name, computer);

            try
            {
                ServiceControllerStatus status = controller.Status;

                _Log.Information(string.Format("The service {0} is installed.", name));
                return true;
            }
            catch
            {
                _Log.Information(string.Format("The service {0} is not installed!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service is paused
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is paused.</returns>
        public bool IsPaused(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceControllerStatus status = this.GetStatus(name, computer);

            if (status == ServiceControllerStatus.Paused)
            {
                _Log.Information(string.Format("The service {0} is paused.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} is not paused!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service is pausing
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is pausing.</returns>
        public bool IsPausing(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceControllerStatus status = this.GetStatus(name, computer);

            if (status == ServiceControllerStatus.PausePending)
            {
                _Log.Information(string.Format("The service {0} is pausing.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} is not pausing!", name));
                return false;
            }
        }


        /// <summary>
        /// Checks if the named service is running
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is running.</returns>
        public bool IsRunning(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceControllerStatus status = this.GetStatus(name, computer);

            if (status == ServiceControllerStatus.Running)
            {
                _Log.Information(string.Format("The service {0} is running.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} is not running!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service is starting
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is starting.</returns>
        public bool IsStarting(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceControllerStatus status = this.GetStatus(name, computer);

            if (status == ServiceControllerStatus.StartPending)
            {
                _Log.Information(string.Format("The service {0} is starting.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} is not starting!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service is stopped
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is stopped.</returns>
        public bool IsStopped(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceControllerStatus status = this.GetStatus(name, computer);

            if (status == ServiceControllerStatus.Stopped)
            {
                _Log.Information(string.Format("The service {0} is stopped.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} is not stopped!", name));
                return false;
            }
        }

        ///<summary>
        /// Checks if the named service is stopping
        /// </summary>
        /// /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service is stopping.</returns>
        public bool IsStopping(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceControllerStatus status = this.GetStatus(name, computer);

            if (status == ServiceControllerStatus.StopPending)
            {
                _Log.Information(string.Format("The service {0} is stopping.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} is not stopping!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service can be paused and continued
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service can be paused and continued.</returns>
        public bool CanPauseAndContinue(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (service.CanPauseAndContinue)
            {
                _Log.Information(string.Format("The service {0} can pause and continue.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} can not pause and continue!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service can be stopped
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service can be stopped.</returns>
        public bool CanStop(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (service.CanStop)
            {
                _Log.Information(string.Format("The service {0} can be stopped.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} can not be stopped!", name));
                return false;
            }
        }

        /// <summary>
        /// Checks if the named service can be shutdown
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service can be shutdown.</returns>
        public bool CanShutdown(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (service.CanShutdown)
            {
                _Log.Information(string.Format("The service {0} can be shutdown.", name));
                return true;
            }
            else
            {
                _Log.Information(string.Format("The service {0} can not be shutdown!", name));
                return false;
            }
        }



        /// <summary>
        /// Starts a named service
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <param name="args">The arguments to pass to the service when starting</param>
        /// <returns>If the service was started.</returns>
        public bool Start(string name, string computer = "", int timeout = 60000, string[] args = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (service.Status == ServiceControllerStatus.Running)
            {
                //Already Running
                _Log.Information(string.Format("The service {0} is already running.", name));
                return true;
            }
            else
            {
                //Start Service
                if (service.Status != ServiceControllerStatus.StartPending)
                {
                    _Log.Information(string.Format("Attempting to start the service {0}.", name));

                    if (args != null)
                    {
                        service.Start(args);
                    }
                    else
                    {
                        service.Start();
                    }
                }



                //Wait for Completion
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeout));

                if (service.Status == ServiceControllerStatus.Running)
                {
                    _Log.Information(string.Format("The service {0} has been started.", name));
                    return true;
                }
                else
                {
                    _Log.Information(string.Format("The service {0} could NOT be started.", name));
                    return false;
                }
            }
        }

        /// <summary>
        /// Stops a named service
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was stopped.</returns>
        public bool Stop(string name, string computer = "", int timeout = 60000)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (!service.CanStop)
            {
                //Can't Stop
                _Log.Information(string.Format("The service {0} can't be stopped.", name));
                return false;
            }
            else
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    //Already Stopped
                    _Log.Information(string.Format("The service {0} is already stopped.", name));
                    return true;
                }
                else
                {
                    //Stop Service
                    if (service.Status != ServiceControllerStatus.StopPending)
                    {
                        _Log.Information(string.Format("Attempting to stop the service {0}.", name));
                        service.Stop();
                    }



                    //Wait for Completion
                    service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeout));

                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        _Log.Information(string.Format("The service {0} has been stopped.", name));
                        return true;
                    }
                    else
                    {
                        _Log.Information(string.Format("The service {0} could not be stopped.", name));
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Restarts a named service
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was restarted.</returns>
        public bool Restart(string name, string computer = "", int timeout = 60000)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            if (this.Stop(name, computer, timeout))
            {
                return this.Start(name, computer, timeout);
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Pauses a named service
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was paused.</returns>
        public bool Pause(string name, string computer = "", int timeout = 60000)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (!service.CanPauseAndContinue)
            {
                //Can't Pause
                _Log.Information(string.Format("The service {0} can't be paused.", name));
                return false;
            }
            else
            {
                if (service.Status == ServiceControllerStatus.Paused)
                {
                    //Already Paused
                    _Log.Information(string.Format("The service {0} is already paused.", name));
                    return true;
                }
                else
                {
                    //Pause Service
                    if (service.Status != ServiceControllerStatus.PausePending)
                    {
                        _Log.Information(string.Format("Attempting to pause the service {0}.", name));
                        service.Pause();
                    }



                    //Wait for Completion
                    service.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromMilliseconds(timeout));

                    if (service.Status == ServiceControllerStatus.Paused)
                    {
                        _Log.Information(string.Format("The service {0} has been paused.", name));
                        return true;
                    }
                    else
                    {
                        _Log.Information(string.Format("The service {0} could not be paused.", name));
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Continues a named service
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="timeout">The duration to wait for the command to complete.</param>
        /// <returns>If the service was continued.</returns>
        public bool Continue(string name, string computer = "", int timeout = 60000)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (!service.CanPauseAndContinue)
            {
                //Can't Continue
                _Log.Information(string.Format("The service {0} can't be continued.", name));
                return false;
            }
            else
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    //Already Running
                    _Log.Information(string.Format("The service {0} is already running.", name));
                    return true;
                }
                else
                {
                    //Pause Service
                    if (service.Status != ServiceControllerStatus.ContinuePending)
                    {
                        _Log.Information(string.Format("Attempting to continue the service {0}.", name));
                        service.Continue();
                    }



                    //Wait for Completion
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeout));

                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        _Log.Information(string.Format("The service {0} has been continued.", name));
                        return true;
                    }
                    else
                    {
                        _Log.Information(string.Format("The service {0} could not be continued.", name));
                        return false;
                    }
                }
            }
        }



        /// <summary>
        /// Executes a command on a named service
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>If the command was executed.</returns>
        public bool ExecuteCommand(string name, string computer = "", int command = 0)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            ServiceController service = this.GetService(name, computer);

            if (service.Status != ServiceControllerStatus.Running)
            {
                //Not Running
                _Log.Information(string.Format("The service {0} is not running.", name));
                return false;
            }
            else
            {
                //Execute Command
                _Log.Information(string.Format("Sending the command to the service {0}.", name));
                service.ExecuteCommand(command);
                return true;
            }
        }



        /// <summary>
        /// Installs a service on a computer
        /// </summary>
        /// <param name="settings">The settings to use when installing the service.</param>
        /// <returns>If the service was installed.</returns>
        public void Install(InstallSettings settings)
        {
            this.Install("", settings);
        }

        /// <summary>
        /// Installs a service on a computer
        /// </summary>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <param name="settings">The settings to use when installing the service.</param>
        /// <returns>If the service was installed.</returns>
        /// <inheritdoc />
        public void Install(string computer, InstallSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (String.IsNullOrEmpty(settings.ServiceName))
            {
                throw new ArgumentNullException("settings.ServiceName");
            }
            if (settings.ExecutablePath == null)
            {
                throw new ArgumentNullException("settings.ExecutablePath");
            }



            this.PowershellCreateCommand(computer, settings);

            if (!String.IsNullOrEmpty(settings.Description))
            {
                this.PowershellDescriptionCommand(computer, settings);
            }
        }

        /// <summary>
        /// Uninstalls a service from a computer
        /// </summary>
        /// <param name="name">The name that identifies the service to the system.</param>
        /// <param name="computer">The computer on which the service resides.</param>
        /// <returns>If the service was uninstalled.</returns>
        public bool Uninstall(string name, string computer = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }



            if (this.ServiceExists(name, computer))
            {
                PowershellSettings powerSettings = new PowershellSettings()
                {
                    FormatOutput = true,
                    LogOutput = true
                }.WithArguments(args =>
                {
                    args.AppendQuoted(name);
                });

                if (!String.IsNullOrEmpty(computer))
                {
                    powerSettings.ComputerName = computer;
                }

                _PowershellRunner.Start("& \"sc.exe\" delete", powerSettings);

                return true;
            }
            else
            {
                return false;
            }
        }



        //Helpers
        private void PowershellCreateCommand(string computer, InstallSettings settings)
        {
            //Get Arguments
            ProcessArgumentBuilder args = new ProcessArgumentBuilder();

            string pathArgs = string.Empty;
            if (settings.Arguments != null)
                pathArgs = settings.Arguments.Render();
            this.SetFilePath(computer, settings);



            if (!String.IsNullOrEmpty(settings.ServiceName))
            {
                args.AppendQuoted(settings.ServiceName);
            }

            if (string.IsNullOrEmpty(pathArgs))
            {
                args.AppendQuoted("binPath", settings.ExecutablePath.FullPath);
            }
            else
            {
                args.AppendQuoted("binPath", "\\\"" + settings.ExecutablePath.FullPath + "\\\" " + pathArgs.Replace("\"", "\\\""));
            }



            if (!String.IsNullOrEmpty(settings.DisplayName))
            {
                args.AppendQuoted("DisplayName", settings.DisplayName);
            }

            if (!String.IsNullOrEmpty(settings.Dependencies))
            {
                args.AppendQuoted("depend", settings.Dependencies);
            }

            if (!String.IsNullOrEmpty(settings.StartMode))
            {
                args.AppendQuoted("start", settings.StartMode);
            }

            if (!String.IsNullOrEmpty(settings.Username))
            {
                args.AppendQuoted("obj", settings.Username);
            }

            if (!String.IsNullOrEmpty(settings.Password))
            {
                args.AppendQuotedSecret("password", settings.Password);
            }



            //Build Script
            string script = "";

            if (!this.ServiceExists(settings.ServiceName, computer))
            {
                //Create
                script = "& \"sc.exe\" create";
            }
            else
            {
                //Config
                script = "& \"sc.exe\" config";
            }



            //Create Settings
            PowershellSettings powerSettings = new PowershellSettings()
            {
                FormatOutput = true,
                LogOutput = true,

                Arguments = args
            };

            //Remote Connection
            if (!String.IsNullOrEmpty(computer))
            {
                powerSettings.ComputerName = computer;
            }

            this.SetWorkingDirectory(powerSettings);



            //Run Powershell
            _PowershellRunner.Start(script, powerSettings);
        }

        private void PowershellDescriptionCommand(string computer, InstallSettings settings)
        {
            //Create Settings
            PowershellSettings powerSettings = new PowershellSettings()
            {
                FormatOutput = true,
                LogOutput = true
            }.WithArguments(args =>
            {
                args.AppendQuoted(settings.Description);
            });

            //Remote Connection
            if (!String.IsNullOrEmpty(computer))
            {
                powerSettings.ComputerName = computer;
            }

            this.SetWorkingDirectory(powerSettings);



            //Run Command
            _PowershellRunner.Start("& \"sc.exe\" description", powerSettings);
        }



        private void SetWorkingDirectory(PowershellSettings settings)
        {
            if (String.IsNullOrEmpty(settings.ComputerName))
            {
                DirectoryPath workingDirectory = _Environment.WorkingDirectory;

                settings.WorkingDirectory = workingDirectory.MakeAbsolute(_Environment);
            }
            else if (settings.WorkingDirectory == null)
            {
                settings.WorkingDirectory = new DirectoryPath("C:/");
            }
        }

        private void SetFilePath(string computer, InstallSettings install)
        {
            if (String.IsNullOrEmpty(computer))
            {
                install.ExecutablePath = install.ExecutablePath.MakeAbsolute(_Environment);
            }
            else
            {
                install.ExecutablePath = install.ExecutablePath.MakeAbsolute(new DirectoryPath("C:/"));
            }
        }
        #endregion
    }
}
