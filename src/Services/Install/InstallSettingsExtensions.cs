#region Using Statements
    using System;
    using System.Collections.Generic;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.IO.Arguments;
#endregion



namespace Cake.Services
{
    /// <summary>
    /// Contains extension methods for <see cref="InstallSettings" />.
    /// </summary>
    public static class InstallSettingsExtensions
    {
        /// <summary>
        /// Sets the arguments to use during installation
        /// </summary>
        /// <param name="settings">The installation settings.</param>
        /// <param name="arguments">The arguments to append.</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings WithArguments(this InstallSettings settings, Action<ProcessArgumentBuilder> arguments)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (settings.Arguments == null)
            {
                settings.Arguments = new ProcessArgumentBuilder();
            }

            arguments(settings.Arguments);
            return settings;
        }



        /// <summary>
        /// Specifies the service name returned by the getkeyname operation.
        /// </summary>
        /// <param name="settings">The installation settings.</param>
        /// <param name="name">The name of the service</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings SetServiceName(this InstallSettings settings, string name)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.ServiceName = name;
            return settings;
        }

        /// <summary>
        /// Specifies a friendly name that can be used by user interface programs to identify the service.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="name">The friendly name of the service</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings SetDisplayname(this InstallSettings settings, string name)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.ServiceName = name;
            return settings;
        }



        /// <summary>
        /// Specifies a path to the service binary file.
        /// </summary>
        /// <param name="settings">The installation settings.</param>
        /// <param name="path">The path to the service binary file.</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings SetExecutablePath(this InstallSettings settings, FilePath path)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.ExecutablePath = path;
            return settings;
        }

        /// <summary>
        /// Specifies the start type for the service. The default setting is start= demand.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="mode">The start type for the service</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings SetStartMode(this InstallSettings settings, string mode)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.StartMode = mode;
            return settings;
        }



        /// <summary>
        /// Specifies the names of services or groups that must start before this service starts. The names are separated by forward slashes (/).
        /// </summary>
        /// <param name="settings">The installation settings.</param>
        /// <param name="dependencies">The names of services or groups that must start before this service starts</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings SetDependencies(this InstallSettings settings, string dependencies)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Dependencies = dependencies;
            return settings;
        }

        /// <summary>
        /// Specifies a description for the specified service. If no string is specified, the description of the service is not modified. There is no limit to the number of characters in the service description.
        /// </summary>
        /// <param name="settings">The installation settings.</param>
        /// <param name="description">The description of the service</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings SetDescription(this InstallSettings settings, string description)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Description = description;
            return settings;
        }



        /// <summary>
        /// Gets or sets the credentials to use when connecting
        /// </summary>
        /// <param name="settings">The installation settings.</param>
        /// <param name="username">The username to connect with.</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings UseUsername(this InstallSettings settings, string username)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Username = username;
            return settings;
        }

        /// <summary>
        /// Gets or sets the credentials to use when connecting
        /// </summary>
        /// <param name="settings">The installation settings.</param>
        /// <param name="password">The password to connect with.</param>
        /// <returns>The same <see cref="InstallSettings"/> instance so that multiple calls can be chained.</returns>
        public static InstallSettings UsePassword(this InstallSettings settings, string password)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Password = password;
            return settings;
        }
    }
}
