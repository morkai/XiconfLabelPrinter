// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MSYS.Xiconf.LabelPrinter
{
    public partial class LicenseForm : Form
    {
        public LicenseForm()
        {
            InitializeComponent();
            ReloadLicenseInfo();
        }

        private void LicenseForm_Shown(object sender, EventArgs e)
        {
            if (openLicenseButton.Enabled)
            {
                openLicenseButton.Focus();
            }
            else
            {
                licenseServerTextBox.Focus();
            }
        }

        private void LicenseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!LicenseInfo.IsValid())
            {
                Properties.Settings.Default.LicenseKey = "";
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void licenseServerLabel_Click(object sender, EventArgs e)
        {
            licenseServerTextBox.Focus();
        }

        private void openLicenseButton_Click(object sender, EventArgs e)
        {
            openLicenseDialog.ShowDialog(this);
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

        private void openLicenseDialog_FileOk(object sender, CancelEventArgs e)
        {
            var timer = new Timer() { Interval = 1 };

            timer.Tick += (sender_, e_) =>
            {
                timer.Stop();

                try
                {
                    LicenseInfo.ReadFromFile(openLicenseDialog.FileName);

                    if (LicenseInfo.IsValid())
                    {
                        ReloadLicenseInfo();
                        ValidateRemotely();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(this, x.Message, "Błąd wczytywania klucza licencyjnego", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ReloadLicenseInfo();
                }
            };

            timer.Start();
        }

        private void licenseServerTextBox_TextChanged(object sender, EventArgs e)
        {
            Uri uri = null;

            try
            {
                uri = new Uri(licenseServerTextBox.Text, UriKind.Absolute);

                if (uri.Scheme != "http")
                {
                    throw new Exception();
                }

                Properties.Settings.Default.LicenseServer = "http://" + uri.Authority;
            }
            catch (Exception)
            {
                Properties.Settings.Default.LicenseServer = "";
            }

            LicenseInfo.Reset();
            ReloadLicenseInfo();
        }

        private void ShowErrorMessage(bool state, string message)
        {
            errorLabel.BackColor = state ? Color.LimeGreen : Color.Red;
            errorLabel.Text = message;
        }

        private void ReloadLicenseInfo()
        {
            openLicenseButton.Enabled = LicenseInfo.Error != "NO_SERVER";

            idValue.Text = LicenseInfo.Id;
            productValue.Text = LicenseInfo.Product;
            versionValue.Text = LicenseInfo.Version;
            dateValue.Text = LicenseInfo.Date;
            licenseeValue.Text = LicenseInfo.Licensee;

            if (LicenseInfo.IsValid())
            {
                ShowErrorMessage(true, "OK!");
            }
            else
            {
                ShowErrorMessage(false, LicenseInfo.ErrorMessage);
            }
        }

        private void ValidateRemotely()
        {
            errorLabel.BackColor = Color.Orange;
            errorLabel.Text = "Trwa walidacja licencji...";

            Enabled = false;

            try
            {
                LicenseInfo.ValidateRemotely();
            }
            catch (Exception x)
            {
                if (LicenseInfo.IsValid())
                {
                    LicenseInfo.Error = "VALIDATION";
                }

                throw x;
            }
            finally
            {
                Enabled = true;
            }
        }
    }
}
