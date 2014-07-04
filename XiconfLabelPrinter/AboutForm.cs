// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Deployment.Application;
using System.Drawing;
using System.Windows.Forms;

namespace MSYS.Xiconf.LabelPrinter
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            var version = ApplicationDeployment.IsNetworkDeployed
                ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                : Application.ProductVersion;

            productVersionLabel.Text = Application.ProductName + " v" + version;
            productLabel.Text = Application.ProductName;
            versionLabel.Text = "Wersja: " + version;
            copyrightLabel.Text = "© " + DateTime.Today.Year + " " + Application.CompanyName;
            licenseeLabel.Text = LicenseInfo.IsValid() ? LicenseInfo.Licensee : "BRAK LICENCJI";
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bottomPanel_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(
                e.Graphics,
                bottomPanel.ClientRectangle,
                Color.Black, 0, ButtonBorderStyle.None,
                Color.FromKnownColor(KnownColor.ControlDark), 1, ButtonBorderStyle.Solid,
                Color.Black, 0, ButtonBorderStyle.None,
                Color.Black, 0, ButtonBorderStyle.None
            );
        }

        private void productLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(productLinkLabel.Text);
        }
    }
}
