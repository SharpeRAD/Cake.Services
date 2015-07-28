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
            /// <inheritdoc />
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

            /// <inheritdoc />
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
                    _Log.Debug(string.Format("The service {0} exists.", name));
                    return true;
                }
                else
                {
                    _Log.Debug(string.Format("The service {0} does not exist.", name));
                    return false;
                }
            }



            /// <inheritdoc />
            public ServiceControllerStatus GetStatus(string name, string computer = "")
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }



                ServiceController service = this.GetService(name, computer);

                _Log.Debug(string.Format("Getting the status of the service {0}.", name));

                return service.Status;
            }

            /// <inheritdoc />
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
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <inheritdoc />
            public bool IsRunning(string name, string computer = "")
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }



                ServiceControllerStatus status = this.GetStatus(name, computer);

                return (status == ServiceControllerStatus.Running);
            }

            /// <inheritdoc />
            public bool IsStopped(string name, string computer = "")
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }



                ServiceControllerStatus status = this.GetStatus(name, computer);

                return (status == ServiceControllerStatus.Stopped);
            }

            /// <inheritdoc />
            public bool CanPauseAndContinue(string name, string computer = "")
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }



                ServiceController service = this.GetService(name, computer);

                return service.CanPauseAndContinue;
            }

            /// <inheritdoc />
            public bool CanStop(string name, string computer = "")
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }



                ServiceController service = this.GetService(name, computer);

                return service.CanStop;
            }

            /// <inheritdoc />
            public bool CanShutdown(string name, string computer = "")
            {
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }



                ServiceController service = this.GetService(name, computer);

                return service.CanShutdown;
            }



            /// <inheritdoc />
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
                    _Log.Debug(string.Format("The service {0} is already running.", name));
                    return true;
                }
                else
                {
                    //Start Service
                    if (service.Status != ServiceControllerStatus.StartPending)
                    {
                        _Log.Debug(string.Format("Attempting to start the service {0}.", name));

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
                        _Log.Debug(string.Format("The service {0} has been started.", name));
                        return true;
                    }
                    else
                    {
                        _Log.Debug(string.Format("The service {0} could not be started.", name));
                        return false;
                    }
                }
            }
            
            /// <inheritdoc />
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
                    _Log.Debug(string.Format("The service {0} can't be stopped.", name));
                    return false;
                }
                else
                {
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        //Already Stopped
                        _Log.Debug(string.Format("The service {0} is already stopped.", name));
                        return true;
                    }
                    else
                    {
                        //Stop Service
                        if (service.Status != ServiceControllerStatus.StopPending)
                        {
                            _Log.Debug(string.Format("Attempting to stop the service {0}.", name));
                            service.Stop();
                        }



                        //Wait for Completion
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeout));

                        if (service.Status == ServiceControllerStatus.Stopped)
                        {
                            _Log.Debug(string.Format("The service {0} has been stopped.", name));
                            return true;
                        }
                        else
                        {
                            _Log.Debug(string.Format("The service {0} could not be stopped.", name));
                            return false;
                        }
                    }
                }
            }

            /// <inheritdoc />
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



            /// <inheritdoc />
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
                    _Log.Debug(string.Format("The service {0} can't be paused.", name));
                    return false;
                }
                else
                {
                    if (service.Status == ServiceControllerStatus.Paused)
                    {
                        //Already Paused
                        _Log.Debug(string.Format("The service {0} is already paused.", name));
                        return true;
                    }
                    else
                    {
                        //Pause Service
                        if (service.Status != ServiceControllerStatus.PausePending)
                        {
                            _Log.Debug(string.Format("Attempting to pause the service {0}.", name));
                            service.Pause();
                        }



                        //Wait for Completion
                        service.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromMilliseconds(timeout));

                        if (service.Status == ServiceControllerStatus.Paused)
                        {
                            _Log.Debug(string.Format("The service {0} has been paused.", name));
                            return true;
                        }
                        else
                        {
                            _Log.Debug(string.Format("The service {0} could not be paused.", name));
                            return false;
                        }
                    }
                }
            }

            /// <inheritdoc />
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
                    _Log.Debug(string.Format("The service {0} can't be continued.", name));
                    return false;
                }
                else
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        //Already Running
                        _Log.Debug(string.Format("The service {0} is already running.", name));
                        return true;
                    }
                    else
                    {
                        //Pause Service
                        if (service.Status != ServiceControllerStatus.ContinuePending)
                        {
                            _Log.Debug(string.Format("Attempting to continue the service {0}.", name));
                            service.Continue();
                        }



                        //Wait for Completion
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeout));

                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            _Log.Debug(string.Format("The service {0} has been continued.", name));
                            return true;
                        }
                        else
                        {
                            _Log.Debug(string.Format("The service {0} could not be continued.", name));
                            return false;
                        }
                    }
                }
            }


            /// <inheritdoc />
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
                    _Log.Debug(string.Format("The service {0} is not running.", name));
                    return false;
                }
                else
                {
                    //Execute Command
                    _Log.Debug(string.Format("Sending the command to the service {0}.", name));
                    service.ExecuteCommand(command);
                    return true;
                }
            }


        
            /// <inheritdoc />
            public void Install(InstallSettings settings)
            {
                this.Install("", settings);
            }

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

            /// <inheritdoc />
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

                string pathArgs = settings.Arguments.Render();
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
                    args.AppendQuoted("start", settings.Dependencies);
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

                if (this.ServiceExists(settings.ServiceName, computer))
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
