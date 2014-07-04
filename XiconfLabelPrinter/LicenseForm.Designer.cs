namespace MSYS.Xiconf.LabelPrinter
{
    partial class LicenseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.licenseServerLabel = new System.Windows.Forms.Label();
            this.openLicenseButton = new System.Windows.Forms.Button();
            this.productLabel = new System.Windows.Forms.Label();
            this.productValue = new System.Windows.Forms.Label();
            this.versionValue = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.dateValue = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.licenseeValue = new System.Windows.Forms.Label();
            this.licenseeLabel = new System.Windows.Forms.Label();
            this.idValue = new System.Windows.Forms.Label();
            this.idLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.openLicenseDialog = new System.Windows.Forms.OpenFileDialog();
            this.licenseServerTextBox = new System.Windows.Forms.TextBox();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.bottomPanel.Controls.Add(this.closeButton);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 252);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(444, 50);
            this.bottomPanel.TabIndex = 0;
            this.bottomPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bottomPanel_Paint);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(357, 15);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 102;
            this.closeButton.Text = "OK";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // licenseServerLabel
            // 
            this.licenseServerLabel.Location = new System.Drawing.Point(12, 171);
            this.licenseServerLabel.Name = "licenseServerLabel";
            this.licenseServerLabel.Size = new System.Drawing.Size(100, 13);
            this.licenseServerLabel.TabIndex = 0;
            this.licenseServerLabel.Text = "Serwer licencji:";
            this.licenseServerLabel.Click += new System.EventHandler(this.licenseServerLabel_Click);
            // 
            // openLicenseButton
            // 
            this.openLicenseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.openLicenseButton.Location = new System.Drawing.Point(116, 194);
            this.openLicenseButton.Name = "openLicenseButton";
            this.openLicenseButton.Size = new System.Drawing.Size(316, 46);
            this.openLicenseButton.TabIndex = 101;
            this.openLicenseButton.Text = "Wybierz nowy klucz licencyjny";
            this.openLicenseButton.UseVisualStyleBackColor = true;
            this.openLicenseButton.Click += new System.EventHandler(this.openLicenseButton_Click);
            // 
            // productLabel
            // 
            this.productLabel.Location = new System.Drawing.Point(12, 83);
            this.productLabel.Name = "productLabel";
            this.productLabel.Size = new System.Drawing.Size(100, 13);
            this.productLabel.TabIndex = 0;
            this.productLabel.Text = "Aplikacja:";
            // 
            // productValue
            // 
            this.productValue.AutoSize = true;
            this.productValue.Location = new System.Drawing.Point(113, 83);
            this.productValue.Name = "productValue";
            this.productValue.Size = new System.Drawing.Size(10, 13);
            this.productValue.TabIndex = 0;
            this.productValue.Text = "-";
            // 
            // versionValue
            // 
            this.versionValue.AutoSize = true;
            this.versionValue.Location = new System.Drawing.Point(113, 105);
            this.versionValue.Name = "versionValue";
            this.versionValue.Size = new System.Drawing.Size(10, 13);
            this.versionValue.TabIndex = 0;
            this.versionValue.Text = "-";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(12, 105);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(100, 13);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.Text = "Wersja:";
            // 
            // dateValue
            // 
            this.dateValue.AutoSize = true;
            this.dateValue.Location = new System.Drawing.Point(113, 127);
            this.dateValue.Name = "dateValue";
            this.dateValue.Size = new System.Drawing.Size(10, 13);
            this.dateValue.TabIndex = 0;
            this.dateValue.Text = "-";
            // 
            // dateLabel
            // 
            this.dateLabel.Location = new System.Drawing.Point(12, 127);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(100, 13);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "Data przydzielenia:";
            // 
            // licenseeValue
            // 
            this.licenseeValue.AutoSize = true;
            this.licenseeValue.Location = new System.Drawing.Point(113, 149);
            this.licenseeValue.Name = "licenseeValue";
            this.licenseeValue.Size = new System.Drawing.Size(10, 13);
            this.licenseeValue.TabIndex = 0;
            this.licenseeValue.Text = "-";
            // 
            // licenseeLabel
            // 
            this.licenseeLabel.Location = new System.Drawing.Point(12, 149);
            this.licenseeLabel.Name = "licenseeLabel";
            this.licenseeLabel.Size = new System.Drawing.Size(100, 13);
            this.licenseeLabel.TabIndex = 0;
            this.licenseeLabel.Text = "Przydzielona dla:";
            // 
            // idValue
            // 
            this.idValue.AutoSize = true;
            this.idValue.Location = new System.Drawing.Point(113, 61);
            this.idValue.Name = "idValue";
            this.idValue.Size = new System.Drawing.Size(10, 13);
            this.idValue.TabIndex = 0;
            this.idValue.Text = "-";
            // 
            // idLabel
            // 
            this.idLabel.Location = new System.Drawing.Point(12, 61);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(100, 13);
            this.idLabel.TabIndex = 0;
            this.idLabel.Text = "Identyfikator:";
            // 
            // errorLabel
            // 
            this.errorLabel.BackColor = System.Drawing.Color.LimeGreen;
            this.errorLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.errorLabel.ForeColor = System.Drawing.Color.White;
            this.errorLabel.Location = new System.Drawing.Point(0, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Padding = new System.Windows.Forms.Padding(13, 0, 13, 0);
            this.errorLabel.Size = new System.Drawing.Size(444, 50);
            this.errorLabel.TabIndex = 0;
            this.errorLabel.Text = "Brak klucza licencyjnego.";
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openLicenseDialog
            // 
            this.openLicenseDialog.DefaultExt = "txt";
            this.openLicenseDialog.FileName = "msys_xiconf-lp_license_01.txt";
            this.openLicenseDialog.Filter = "Pliki tekstowe |*.txt";
            this.openLicenseDialog.Title = "Wybór nowego klucza licencyjnego";
            this.openLicenseDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openLicenseDialog_FileOk);
            // 
            // licenseServerTextBox
            // 
            this.licenseServerTextBox.Location = new System.Drawing.Point(116, 168);
            this.licenseServerTextBox.Name = "licenseServerTextBox";
            this.licenseServerTextBox.Size = new System.Drawing.Size(316, 20);
            this.licenseServerTextBox.TabIndex = 100;
            this.licenseServerTextBox.Text = global::MSYS.Xiconf.LabelPrinter.Properties.Settings.Default.LicenseServer;
            this.licenseServerTextBox.TextChanged += new System.EventHandler(this.licenseServerTextBox_TextChanged);
            // 
            // LicenseForm
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 302);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.idValue);
            this.Controls.Add(this.productLabel);
            this.Controls.Add(this.productValue);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.versionValue);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.dateValue);
            this.Controls.Add(this.licenseeLabel);
            this.Controls.Add(this.licenseeValue);
            this.Controls.Add(this.licenseServerLabel);
            this.Controls.Add(this.licenseServerTextBox);
            this.Controls.Add(this.openLicenseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Licencja";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LicenseForm_FormClosing);
            this.Shown += new System.EventHandler(this.LicenseForm_Shown);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button openLicenseButton;
        private System.Windows.Forms.Label licenseServerLabel;
        private System.Windows.Forms.TextBox licenseServerTextBox;
        private System.Windows.Forms.Label productLabel;
        private System.Windows.Forms.Label productValue;
        private System.Windows.Forms.Label versionValue;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label dateValue;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label licenseeValue;
        private System.Windows.Forms.Label licenseeLabel;
        private System.Windows.Forms.Label idValue;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.OpenFileDialog openLicenseDialog;
    }
}