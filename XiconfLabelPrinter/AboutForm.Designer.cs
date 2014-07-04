namespace MSYS.Xiconf.LabelPrinter
{
    partial class AboutForm
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
            System.Windows.Forms.Panel middlePanel;
            System.Windows.Forms.Label licensedForLabel;
            System.Windows.Forms.Label rightsLabel;
            this.licenseeLabel = new System.Windows.Forms.Label();
            this.productLinkLabel = new System.Windows.Forms.LinkLabel();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.productLabel = new System.Windows.Forms.Label();
            this.topPanel = new System.Windows.Forms.Panel();
            this.productVersionLabel = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            middlePanel = new System.Windows.Forms.Panel();
            licensedForLabel = new System.Windows.Forms.Label();
            rightsLabel = new System.Windows.Forms.Label();
            middlePanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // middlePanel
            // 
            middlePanel.Controls.Add(this.licenseeLabel);
            middlePanel.Controls.Add(licensedForLabel);
            middlePanel.Controls.Add(this.productLinkLabel);
            middlePanel.Controls.Add(rightsLabel);
            middlePanel.Controls.Add(this.copyrightLabel);
            middlePanel.Controls.Add(this.versionLabel);
            middlePanel.Controls.Add(this.productLabel);
            middlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            middlePanel.Location = new System.Drawing.Point(0, 60);
            middlePanel.Margin = new System.Windows.Forms.Padding(0);
            middlePanel.Name = "middlePanel";
            middlePanel.Padding = new System.Windows.Forms.Padding(13);
            middlePanel.Size = new System.Drawing.Size(444, 142);
            middlePanel.TabIndex = 2;
            // 
            // licenseeLabel
            // 
            this.licenseeLabel.BackColor = System.Drawing.SystemColors.Control;
            this.licenseeLabel.Location = new System.Drawing.Point(16, 118);
            this.licenseeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.licenseeLabel.Name = "licenseeLabel";
            this.licenseeLabel.Size = new System.Drawing.Size(419, 13);
            this.licenseeLabel.TabIndex = 6;
            this.licenseeLabel.Text = "BRAK LICENCJI";
            this.licenseeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // licensedForLabel
            // 
            licensedForLabel.AutoSize = true;
            licensedForLabel.Location = new System.Drawing.Point(307, 101);
            licensedForLabel.Margin = new System.Windows.Forms.Padding(0);
            licensedForLabel.Name = "licensedForLabel";
            licensedForLabel.Size = new System.Drawing.Size(128, 13);
            licensedForLabel.TabIndex = 5;
            licensedForLabel.Text = "Licencja przydzielona dla:";
            licensedForLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // productLinkLabel
            // 
            this.productLinkLabel.AutoSize = true;
            this.productLinkLabel.Location = new System.Drawing.Point(13, 84);
            this.productLinkLabel.Name = "productLinkLabel";
            this.productLinkLabel.Size = new System.Drawing.Size(124, 13);
            this.productLinkLabel.TabIndex = 4;
            this.productLinkLabel.TabStop = true;
            this.productLinkLabel.Text = "http://miraclesystems.pl/";
            this.productLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.productLinkLabel_LinkClicked);
            // 
            // rightsLabel
            // 
            rightsLabel.AutoSize = true;
            rightsLabel.Location = new System.Drawing.Point(13, 67);
            rightsLabel.Name = "rightsLabel";
            rightsLabel.Size = new System.Drawing.Size(134, 13);
            rightsLabel.TabIndex = 3;
            rightsLabel.Text = "Pewne prawa zastrzeżone.";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(13, 49);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(223, 13);
            this.copyrightLabel.TabIndex = 2;
            this.copyrightLabel.Text = "© 2014 Miracle Systems Łukasz Walukiewicz";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(13, 31);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(79, 13);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Wersja: 0.0.0.0";
            // 
            // productLabel
            // 
            this.productLabel.AutoSize = true;
            this.productLabel.Location = new System.Drawing.Point(13, 13);
            this.productLabel.Name = "productLabel";
            this.productLabel.Size = new System.Drawing.Size(99, 13);
            this.productLabel.TabIndex = 0;
            this.productLabel.Text = "Xiconf Label Printer";
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.topPanel.Controls.Add(this.productVersionLabel);
            this.topPanel.Controls.Add(this.logoPictureBox);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(444, 60);
            this.topPanel.TabIndex = 0;
            // 
            // productVersionLabel
            // 
            this.productVersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productVersionLabel.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.productVersionLabel.Location = new System.Drawing.Point(80, 0);
            this.productVersionLabel.Name = "productVersionLabel";
            this.productVersionLabel.Size = new System.Drawing.Size(364, 60);
            this.productVersionLabel.TabIndex = 1;
            this.productVersionLabel.Text = "Xiconf Label Printer v0.0.0.0";
            this.productVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.logoPictureBox.Image = global::MSYS.Xiconf.LabelPrinter.Properties.Resources.printerLogo;
            this.logoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(80, 60);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.bottomPanel.Controls.Add(this.closeButton);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 202);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(444, 50);
            this.bottomPanel.TabIndex = 1;
            this.bottomPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bottomPanel_Paint);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(357, 15);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "OK";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // AboutForm
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 252);
            this.Controls.Add(middlePanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "O programie";
            middlePanel.ResumeLayout(false);
            middlePanel.PerformLayout();
            this.topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label productVersionLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Label productLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.LinkLabel productLinkLabel;
        private System.Windows.Forms.Label licenseeLabel;

    }
}