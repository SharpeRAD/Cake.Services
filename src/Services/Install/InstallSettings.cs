#region Using Statements
    using System;

    using Cake.Core.IO;
#endregion



namespace Cake.Services
{
    public class InstallSettings
    {
        #region Properties (10)
            public string ServiceName { get; set; }

            public string DisplayName { get; set; }



            public FilePath ExecutablePath { get; set; }

            public string StartMode { get; set; }



            public string Dependencies { get; set; }

            public string Description { get; set; }



            public string Username { get; set; }

            public string Password { get; set; }



            public ProcessArgumentBuilder Arguments { get; set; }
        #endregion
    }
}
