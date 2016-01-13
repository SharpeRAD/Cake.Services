#region Using Statements
    using Cake.Core.IO;
#endregion



namespace Cake.Services
{
    /// <summary>
    /// Windows service installation settings.
    /// </summary>
    public class InstallSettings
    {
        #region Properties (10)
            /// <summary>
            /// Specifies the service name returned by the getkeyname operation.
            /// </summary>
            public string ServiceName { get; set; }

            /// <summary>
            /// Specifies a friendly name that can be used by user interface programs to identify the service.
            /// </summary>
            public string DisplayName { get; set; }



            /// <summary>
            /// Specifies a path to the service binary file.
            /// </summary>
            public FilePath ExecutablePath { get; set; }

            /// <summary>
            /// Specifies the start type for the service. The default setting is start= demand.
            /// </summary>
            public string StartMode { get; set; }


            /// <summary>
            /// Specifies the names of services or groups that must start before this service starts. The names are separated by forward slashes (/).
            /// </summary>
            public string Dependencies { get; set; }

            /// <summary>
            /// Specifies a description for the specified service. If no string is specified, the description of the service is not modified. There is no limit to the number of characters in the service description.
            /// </summary>
            public string Description { get; set; }


            /// <summary>
            /// Gets or sets the credentials to use when connecting
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// Gets or sets the credentials to use when connecting
            /// </summary>
            public string Password { get; set; }



            /// <summary>
            /// Sets the arguments to use during installation
            /// </summary>
            public ProcessArgumentBuilder Arguments { get; set; }
        #endregion
    }
}
