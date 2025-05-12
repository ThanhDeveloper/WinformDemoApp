using FluentFTP;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Diagnostics;
using System.Net;

namespace WinformDemoApp
{
    public class UpdateManager
    {
        private readonly string _ftpBaseUrl;
        public string CurrentVersion { get; }

        public UpdateManager(string currentVersion)
        {
            CurrentVersion = currentVersion;
            _ftpBaseUrl = ConfigurationManager.AppSettings["FtpUrl"] ?? "ftp://localhost";
        }

        public async Task<string?> CheckForUpdates(UpdateSource updateSource)
        {
            try
            {
                var newVersion = string.Empty;
                if (updateSource is UpdateSource.FTP)
                {
                    newVersion = GetVersionFromFtp();
                }
                else
                {
                    newVersion = await GetVersionFromDatabase();
                }


                return (!string.IsNullOrEmpty(newVersion) && CurrentVersion.Trim() == newVersion.Trim()) ? null : newVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}");
                return null;
            }
        }

        private static async Task<string?> GetVersionFromDatabase()
        {
            try
            {
                using var context = new DatabaseContext();
                var appVersion = await context.AppVersions.FirstOrDefaultAsync(default);
                // Ensure we trim any whitespace from database version
                return appVersion?.Version?.Trim() ?? null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not connect to server: {ex.Message}");
            }
            return null;
        }

        private string GetVersionFromFtp()
        {
            try
            {
                // Connect to FTP
                string host = _ftpBaseUrl.Replace("ftp://", "");
                using var client = new FtpClient(host);
                
                // Use anonymous credentials
                client.Credentials = new NetworkCredential("anonymous", "anonymous@example.com");
                client.Config.EncryptionMode = FtpEncryptionMode.None;
                client.Connect();

                // Download version.txt
                string tempFilePath = Path.GetTempFileName();
                var status = client.DownloadFile(tempFilePath, "version.txt");

                if (status == FtpStatus.Success)
                {
                    // Ensure we trim any whitespace or newlines
                    string version = File.ReadAllText(tempFilePath).Trim();
                    File.Delete(tempFilePath);
                    return version;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading version: {ex.Message}");
                return string.Empty;
            }
        }

        public void DownloadAndInstallUpdate(string? newVersion)
        {
            try
            {
                if (string.IsNullOrEmpty(newVersion)) return;
                
                // Connect to FTP
                string host = _ftpBaseUrl.Replace("ftp://", "");
                string updateDir = Path.Combine(Application.StartupPath, "Updates");
                
                // Create update directory if needed
                if (!Directory.Exists(updateDir))
                    Directory.CreateDirectory(updateDir);
                
                // Construct update filename using version
                string updateFileName = $"WinformDemo-{newVersion}.zip";
                string localZipPath = Path.Combine(updateDir, updateFileName);
                
                // Download update zip file
                using (var client = new FtpClient(host))
                {
                    client.Credentials = new NetworkCredential("anonymous", "anonymous@example.com");
                    client.Config.EncryptionMode = FtpEncryptionMode.None;
                    client.Connect();
                    
                    var status = client.DownloadFile(localZipPath, updateFileName);
                    if (status != FtpStatus.Success)
                    {
                        MessageBox.Show("Failed to download update");
                        return;
                    }
                }
                
                // Create batch file for post-exit update
                string batchFilePath = Path.Combine(updateDir, "update.bat");
                using (StreamWriter writer = new(batchFilePath))
                {
                    string exePath = Application.ExecutablePath;
                    
                    writer.WriteLine("@echo off");
                    writer.WriteLine("echo Waiting for application to close...");
                    writer.WriteLine("timeout /t 2 /nobreak > nul");
                    
                    // Extract files
                    writer.WriteLine("echo Extracting update files...");
                    writer.WriteLine($"powershell -command \"Expand-Archive -Path '{localZipPath}' -DestinationPath '{updateDir}\\temp' -Force\"");
                    
                    // Copy update files
                    writer.WriteLine("echo Installing update...");
                    writer.WriteLine($"xcopy \"{updateDir}\\temp\\*\" \"{Application.StartupPath}\" /E /Y /Q");
                    
                    // Save new version
                    writer.WriteLine($"echo {newVersion} > \"{Path.Combine(Application.StartupPath, "version.txt")}\"");
                    
                    // Clean up
                    writer.WriteLine("echo Cleaning up...");
                    writer.WriteLine($"rmdir \"{updateDir}\\temp\" /S /Q");
                    writer.WriteLine($"del \"{localZipPath}\"");
                    
                    // Restart app
                    writer.WriteLine("echo Starting updated application...");
                    writer.WriteLine($"start \"\" \"{exePath}\"");
                    
                    // Self-delete
                    writer.WriteLine("del \"%~f0\"");
                }
                
                // Execute update
                var result = MessageBox.Show(
                    "Update downloaded. The application needs to restart to apply updates. Restart now?", 
                    "Update Ready", 
                    MessageBoxButtons.YesNo);
                
                if (result == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = batchFilePath,
                        UseShellExecute = true,
                        CreateNoWindow = true
                    });
                    
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating: {ex.Message}");
            }
        }
    }
}