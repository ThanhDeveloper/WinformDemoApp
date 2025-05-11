using System.IO;

namespace WinformDemoApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Read version from file if exists
            string versionFilePath = Path.Combine(Application.StartupPath, "version.txt");
            string currentVersion = "1.0.0.0"; 
            
            if (File.Exists(versionFilePath))
            {
                currentVersion = File.ReadAllText(versionFilePath).Trim();
            }
            
            var updateManager = new UpdateManager(currentVersion);
            Application.Run(new Form1(updateManager));
        }
    }
}