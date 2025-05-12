using Microsoft.IdentityModel.Tokens;

namespace WinformDemoApp
{
    public partial class Form1 : Form
    {
        private readonly UpdateManager _updateManager;

        public Form1(UpdateManager updateManager)
        {
            InitializeComponent();
            _updateManager = updateManager;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Only display current version on load
            labelVersionStatus.Text = $"Current version: {_updateManager.CurrentVersion}";
        }

        private async void btnFtpUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                labelVersionStatus.Text = "Checking for updates...";

                await Task.Delay(1000);

                var newVersion = await Task.Run(() => _updateManager.CheckForUpdates(UpdateSource.FTP));

                if (!string.IsNullOrEmpty(newVersion))
                {
                    var result = MessageBox.Show(
                        "A new version is available. Would you like to update now?",
                        "Update Available",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        await Task.Run(() => _updateManager.DownloadAndInstallUpdate(newVersion));
                    }
                    else
                    {
                        labelVersionStatus.Text = $"Current version: {_updateManager.CurrentVersion} (Update available)";
                    }
                }
                else
                {
                    labelVersionStatus.Text = $"Current version: {_updateManager.CurrentVersion} (Up to date)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}");
                labelVersionStatus.Text = $"Current version: {_updateManager.CurrentVersion}";
            }
        }

        private async void buttonDatabaseUpdate_Click(object sender, EventArgs e)
        {
            labelVersionStatus.Text = "Checking for updates...";

            await Task.Delay(1000);

            var newVersion = await Task.Run(() => _updateManager.CheckForUpdates(UpdateSource.Database));

            if (!string.IsNullOrEmpty(newVersion))
            {
                var result = MessageBox.Show(
                    "A new version is available. Would you like to update now?",
                    "Update Available",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    await Task.Run(() => _updateManager.DownloadAndInstallUpdate(newVersion));
                }
                else
                {
                    labelVersionStatus.Text = $"Current version: {_updateManager.CurrentVersion} (Update available)";
                }
            }
            else
            {
                labelVersionStatus.Text = $"Current version: {_updateManager.CurrentVersion} (Up to date)";
            }
        }
    }
}
