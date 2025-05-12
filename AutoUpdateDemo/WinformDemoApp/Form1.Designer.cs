namespace WinformDemoApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelVersionStatus = new Label();
            btnFtpUpdate = new Button();
            helpProvider1 = new HelpProvider();
            buttonDatabaseUpdate = new Button();
            SuspendLayout();
            // 
            // labelVersionStatus
            // 
            labelVersionStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelVersionStatus.AutoSize = true;
            labelVersionStatus.Location = new Point(12, 48);
            labelVersionStatus.Margin = new Padding(2, 0, 2, 0);
            labelVersionStatus.Name = "labelVersionStatus";
            labelVersionStatus.Size = new Size(102, 15);
            labelVersionStatus.TabIndex = 0;
            labelVersionStatus.Text = "labelVersionStatus";
            // 
            // btnFtpUpdate
            // 
            btnFtpUpdate.Location = new Point(12, 11);
            btnFtpUpdate.Margin = new Padding(3, 2, 3, 2);
            btnFtpUpdate.Name = "btnFtpUpdate";
            btnFtpUpdate.Size = new Size(82, 22);
            btnFtpUpdate.TabIndex = 1;
            btnFtpUpdate.Text = "FTP Update";
            btnFtpUpdate.UseVisualStyleBackColor = true;
            btnFtpUpdate.Click += btnFtpUpdate_Click;
            // 
            // buttonDatabaseUpdate
            // 
            buttonDatabaseUpdate.Location = new Point(125, 10);
            buttonDatabaseUpdate.Name = "buttonDatabaseUpdate";
            buttonDatabaseUpdate.Size = new Size(113, 23);
            buttonDatabaseUpdate.TabIndex = 2;
            buttonDatabaseUpdate.Text = "Database update";
            buttonDatabaseUpdate.UseVisualStyleBackColor = true;
            buttonDatabaseUpdate.Click += buttonDatabaseUpdate_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InactiveCaption;
            ClientSize = new Size(365, 131);
            Controls.Add(buttonDatabaseUpdate);
            Controls.Add(btnFtpUpdate);
            Controls.Add(labelVersionStatus);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "MyDemoApp";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelVersionStatus;
        private Button btnFtpUpdate;
        private HelpProvider helpProvider1;
        private Button buttonDatabaseUpdate;
    }
}
