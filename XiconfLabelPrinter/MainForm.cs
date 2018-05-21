// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public partial class MainForm : Form
    {
        private Properties.Settings settings = Properties.Settings.Default;

        private Engine engine = null;

        private LabelFormatDocument orderFormat = null;

        private LabelFormatDocument programFormat = null;

        private LabelFormatDocument serviceTagFormat = null;

        private LabelFormatDocument resistFormat = null;

        private string openFormatFileType = null;

        private string openXlsxFileType = null;

        private OrderList orderList = new OrderList();

        private WorkCenterList workCenterList = new WorkCenterList();

        public MainForm()
        {
            InitializeComponent();
            ReorderFormatControls();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                engine = new Engine(true);
            }
            catch (Exception x)
            {
                ShowError("Błąd silnika BarTender", x.Message);
                Close();
            }

            ToggleLicenseError();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            printButton.Enabled = false;

            var timer = new Timer() { Interval = 1 };

            timer.Tick += (sender_, e_) =>
            {
                timer.Stop();

                SetUpOrderFormat();
                SetUpProgramFormat();
                SetUpServiceTagFormat();
                SetUpResistFormat();
                SetUpOrderXlsx();
                SetUpWorkCenterXlsx();
                SetUpPrinters();

                printButton.Enabled = true;
            };

            timer.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.Save();

            if (engine != null)
            {
                engine.Stop(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                engine.Dispose();
            }
        }

        private void formatCheckBox_MouseDown(object sender, MouseEventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (Control.ModifierKeys == Keys.Control)
            {
                checkBox.DoDragDrop(checkBox.Name, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void formatCheckBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = sender is CheckBox ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void formatCheckBox_DragDrop(object sender, DragEventArgs e)
        {
            var source = e.Data.GetData(DataFormats.Text).ToString().Replace("FormatCheckBox", "");
            var target = GetFormatTypeFromCheckBoxName(sender as CheckBox);

            SwapConfigControls(source, target, "FormatCheckBox");
            SwapConfigControls(source, target, "FormatLinkLabel");
            SwapConfigControls(source, target, "PrinterComboBox");

            RestoreFormatStyles();
            UpdateLabelOrder();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                orderFormatCheckBox.Font = new Font(orderFormatCheckBox.Font.FontFamily, orderFormatCheckBox.Font.Size, FontStyle.Italic);
                orderFormatCheckBox.Cursor = Cursors.SizeNS;
                programFormatCheckBox.Font = new Font(programFormatCheckBox.Font.FontFamily, programFormatCheckBox.Font.Size, FontStyle.Italic);
                programFormatCheckBox.Cursor = Cursors.SizeNS;
                serviceTagFormatCheckBox.Font = new Font(serviceTagFormatCheckBox.Font.FontFamily, serviceTagFormatCheckBox.Font.Size, FontStyle.Italic);
                serviceTagFormatCheckBox.Cursor = Cursors.SizeNS;
                resistFormatCheckBox.Font = new Font(resistFormatCheckBox.Font.FontFamily, resistFormatCheckBox.Font.Size, FontStyle.Italic);
                resistFormatCheckBox.Cursor = Cursors.SizeNS;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                RestoreFormatStyles();
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void licenseMenuItem_Click(object sender, EventArgs e)
        {
            ShowLicenseForm();
        }

        private void licenseErrorLabel_Click(object sender, EventArgs e)
        {
            ShowLicenseForm();
        }

        private void orderPrinterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.OrderPrinterName = orderPrinterComboBox.SelectedItem as string;
        }

        private void programPrinterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.ProgramPrinterName = programPrinterComboBox.SelectedItem as string;
        }

        private void serviceTagPrinterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.ServiceTagPrinterName = serviceTagPrinterComboBox.SelectedItem as string;
        }

        private void resistPrinterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.ResistPrinterName = resistPrinterComboBox.SelectedItem as string;
        }

        private void openOrderFormatMenuItem_Click(object sender, EventArgs e)
        {
            OpenOrderFormatFileDialog();
        }

        private void orderFormatLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenOrderFormatFileDialog();
        }

        private void orderFormatLinkLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                settings.OrderFormatFile = "";
                SetUpOrderFormat();
            }
        }

        private void openProgramFormatMenuItem_Click(object sender, EventArgs e)
        {
            OpenProgramFormatFileDialog();
        }

        private void programFormatLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProgramFormatFileDialog();
        }

        private void programFormatLinkLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                settings.ProgramFormatFile = "";
                SetUpProgramFormat();
            }
        }

        private void openServiceTagFormatMenuItem_Click(object sender, EventArgs e)
        {
            OpenServiceTagFormatFileDialog();
        }

        private void serviceTagFormatLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenServiceTagFormatFileDialog();
        }

        private void serviceTagFormatLinkLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                settings.ServiceTagFormatFile = "";
                SetUpServiceTagFormat();
            }
        }

        private void resistFormatLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenResistFormatFileDialog();
        }

        private void resistFormatLinkLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                settings.ResistFormatFile = "";
                SetUpResistFormat();
            }
        }

        private void openFormatFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var timer = new Timer() { Interval = 1 };

            timer.Tick += (sender_, e_) =>
            {
                timer.Stop();

                var formatFile = openFormatFileDialog.FileName;

                CloseOpenFormat(openFormatFileType, formatFile);

                switch (openFormatFileType)
                {
                    case "order":
                        settings.OrderFormatFile = formatFile;
                        SetUpOrderFormat();
                        break;

                    case "program":
                        settings.ProgramFormatFile = formatFile;
                        SetUpProgramFormat();
                        break;

                    case "serviceTag":
                        settings.ServiceTagFormatFile = formatFile;
                        SetUpServiceTagFormat();
                        break;

                    case "resist":
                        settings.ResistFormatFile = formatFile;
                        SetUpResistFormat();
                        break;
                }

                openFormatFileType = null;
            };

            timer.Start();
        }

        private void openOrderXlsxMenuItem_Click(object sender, EventArgs e)
        {
            OpenOrderXlsxFileDialog();
        }

        private void orderXlsxLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenOrderXlsxFileDialog();
        }

        private void orderXlsxLinkLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                settings.OrderXlsxFile = "";
                SetUpOrderXlsx();
            }
        }

        private void openWorkCenterXlsxMenuItem_Click(object sender, EventArgs e)
        {
            OpenWorkCenterXlsxFileDialog();
        }

        private void workCenterXlsxLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWorkCenterXlsxFileDialog();
        }

        private void workCenterXlsxLinkLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                settings.WorkCenterXlsxFile = "";
                SetUpWorkCenterXlsx();
            }
        }

        private void openXlsxFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var timer = new Timer() { Interval = 1 };

            timer.Tick += (sender_, e_) =>
            {
                timer.Stop();

                var xlsxFile = openXlsxFileDialog.FileName;

                switch (openXlsxFileType)
                {
                    case "order":
                        settings.OrderXlsxFile = xlsxFile;
                        SetUpOrderXlsx();
                        break;

                    case "workCenter":
                        settings.WorkCenterXlsxFile = xlsxFile;
                        SetUpWorkCenterXlsx();
                        break;
                }

                openXlsxFileType = null;
            };

            timer.Start();
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            if (orderFormatCheckBox.Checked && orderFormat == null)
            {
                ShowError("Nieprawidłowa konfiguracja", "Zaznaczono do druku etykietę zlecenia, ale nie wybrano szablonu.");
                return;
            }

            if (programFormatCheckBox.Checked && programFormat == null)
            {
                ShowError("Nieprawidłowa konfiguracja", "Zaznaczono do druku etykietę programową, ale nie wybrano szablonu.");
                return;
            }

            if (serviceTagFormatCheckBox.Checked && serviceTagFormat == null)
            {
                ShowError("Nieprawidłowa konfiguracja", "Zaznaczono do druku etykietę Service Tag, ale nie wybrano szablonu.");
                return;
            }

            if (resistFormatCheckBox.Checked && resistFormat == null)
            {
                ShowError("Nieprawidłowa konfiguracja", "Zaznaczono do druku etykietę wodo/UV, ale nie wybrano szablonu.");
                return;
            }

            if (!orderFormatCheckBox.Checked && !programFormatCheckBox.Checked && !serviceTagFormatCheckBox.Checked && !resistFormatCheckBox.Checked)
            {
                ShowError("Nieprawidłowa konfiguracja", "Nie zaznaczono żadnej etykiety.");
                return;
            }

            if (orderList.Count == 0)
            {
                ShowError("Nieprawidłowa konfiguracja", "Brak zleceń.");
                return;
            }

            var no = orderNoTextBox.Text.Trim();

            if (!Regex.IsMatch(no, "^[0-9]{9}$"))
            {
                ShowError("Nieprawidłowy nr zlecenia", "Nr zlecenia musi składać się z dziewięciu cyfr.");
                return;
            }

            var orders = orderList.FindByNo(no);

            if (orders.Count == 0)
            {
                ShowError("Brak zleceń", "Nie znaleziono zleceń pasujących do podanego nr zlecenia.");
                return;
            }

            var printJobs = new List<IPrintJob>();

            for (var i = 0; i < orders.Count; ++i)
            {
                CreatePrintJobs(printJobs, orders[i], i + 1);
            }

            if (printJobs.Count == 0)
            {
                ShowError("Brak etykiet", "Nie ma etykiet dla wybranej konfiguracji.");
                return;
            }

            ShowPrintForm(printJobs);
        }

        private void reprintOrderButton_Click(object sender, EventArgs e)
        {
            if (orderFormat == null)
            {
                ShowError("Błąd konfiguracji", "Nie wybrano szablonu etykiety zlecenia.");
                orderFormatLinkLabel.Focus();
                return;
            }

            var orderNo = reprintOrderNoValue.Text;

            if (orderNo.Length == 0)
            {
                ShowError("Nieprawidłowy nr zlecenia", "Nr zlecenia jest wymagany.");
                reprintOrderNoValue.Focus();
                return;
            }

            var orderQuantity = (int)reprintOrderQuantityValue.Value;

            if (orderQuantity == 0)
            {
                var order = orderList.Find(o => o.No == orderNo);

                if (order == null)
                {
                    ShowError("Nieprawidłowy nr zlecenia", "Nie znaleziono ilości dla podanego zlecenia.");
                    reprintOrderQuantityValue.Focus();
                    return;
                }

                orderQuantity = order.Quantity;
            }

            var printJob = new OrderPrintJob(orderNo, orderQuantity)
            {
                LabelQuantity = (int)reprintOrderCountValue.Value,
                PrinterName = settings.OrderPrinterName
            };

            ShowPrintForm(new List<IPrintJob>(1) { printJob });
        }

        private void reprintProgramButton_Click(object sender, EventArgs e)
        {
            if (programFormat == null)
            {
                ShowError("Błąd konfiguracji", "Nie wybrano szablonu etykiety programowej.");
                programFormatLinkLabel.Focus();
                return;
            }

            var nc12 = reprintProgramNc12Value.Text;
            var no = reprintProgramNoValue.Text;

            if (nc12.Length != 12 && no.Length != 9)
            {
                ShowError("Nieprawidłowe 12NC/nr zlecenia", "12NC lub Nr zlecenia jest wymagany.");
                reprintProgramNc12Value.Focus();
                return;
            }

            var printJobs = new List<IPrintJob>();
            IList<Order> orders;

            var quantity = (int)reprintProgramCountValue.Value;

            if (no.Length > 0)
            {
                orders = orderList.FindByNo(no);

                foreach (var order in orders)
                {
                    printJobs.Add(new ProgramPrintJob(order.Nc12, order.ProgramName)
                    {
                        LabelQuantity = quantity,
                        PrinterName = settings.ProgramPrinterName
                    });
                }

                if (printJobs.Count == 0)
                {
                    ShowError("Nieprawidłowy nr zlecenia", "Nie znaleziono programu dla podanego nr zlecenia.");
                    reprintProgramNoValue.Focus();
                    return;
                }
            }
            else
            {
                var programName = reprintProgramNameValue.Text.Trim();

                if (programName.Length == 0)
                {
                    orders = orderList.FindByNc12(nc12);

                    if (orders.Count > 0)
                    {
                        printJobs.Add(new ProgramPrintJob(orders[0].Nc12, orders[0].ProgramName)
                        {
                            LabelQuantity = quantity,
                            PrinterName = settings.ProgramPrinterName
                        });
                    }
                    else
                    {
                        ShowError("Nieprawidłowe 12NC", "Nie znaleziono nazwy programu dla podanego 12NC.");
                        reprintProgramNc12Value.Focus();
                        return;
                    }
                }
                else
                {
                    printJobs.Add(new ProgramPrintJob(nc12, programName)
                    {
                        LabelQuantity = quantity,
                        PrinterName = settings.ProgramPrinterName
                    });
                }
            }

            ShowPrintForm(printJobs);
        }

        private void reprintServiceTagButton_Click(object sender, EventArgs e)
        {
            if (serviceTagFormat == null)
            {
                ShowError("Błąd konfiguracji", "Nie wybrano szablonu etykiety Service Tag.");
                serviceTagFormatLinkLabel.Focus();
                return;
            }

            var orderNo = reprintServiceTagNoValue.Text;

            if (orderNo.Length == 0)
            {
                ShowError("Nieprawidłowy nr zlecenia", "Nr zlecenia jest wymagany.");
                reprintServiceTagNoValue.Focus();
                return;
            }

            var firstSerialNo = (int)reprintServiceTagStartNoValue.Value;

            var printJob = new ServiceTagPrintJob(orderNo, firstSerialNo)
            {
                LabelQuantity = (int)reprintServiceTagCountValue.Value,
                PrinterName = settings.ServiceTagPrinterName
            };

            ShowPrintForm(new List<IPrintJob>(1) { printJob });
        }

        private void reprintResistButton_Click(object sender, EventArgs e)
        {
            if (resistFormat == null)
            {
                ShowError("Błąd konfiguracji", "Nie wybrano szablonu etykiety wodo/UV odpornej.");
                resistFormatLinkLabel.Focus();
                return;
            }

            var orderNo = reprintResistNoValue.Text;

            if (orderNo.Length == 0)
            {
                ShowError("Nieprawidłowy nr zlecenia", "Nr zlecenia jest wymagany.");
                reprintResistNoValue.Focus();
                return;
            }

            var text = reprintResistTextValue.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                var order = orderList.Find(o => o.No == orderNo);

                if (order == null || string.IsNullOrEmpty(order.ResistText))
                {
                    ShowError("Nieprawidłowy nr zlecenia", "Nie znaleziono tekstu dla podanego zlecenia.");
                    reprintResistTextValue.Focus();
                    return;
                }

                text = order.ResistText;
            }

            var printJob = new ResistPrintJob(orderNo, text)
            {
                LabelQuantity = (int)reprintResistCountValue.Value,
                PrinterName = settings.ResistPrinterName
            };

            ShowPrintForm(new List<IPrintJob>(1) { printJob });
        }

        private void reloadOrderXlsxButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(settings.OrderXlsxFile))
            {
                SetUpOrderXlsx();
                FocusAcceptButton();
            }
        }

        private void editOrderXlsxButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(settings.OrderXlsxFile))
            {
                Process.Start(settings.OrderXlsxFile);
            }
        }

        private void reloadWorkCenterXlsxButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(settings.WorkCenterXlsxFile))
            {
                SetUpWorkCenterXlsx();
                FocusAcceptButton();
            }
        }

        private void editWorkCenterXlsxButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(settings.WorkCenterXlsxFile))
            {
                Process.Start(settings.WorkCenterXlsxFile);
            }
        }

        private void printTabControl_Selected(object sender, TabControlEventArgs e)
        {
            foreach (Control control in e.TabPage.Controls)
            {
                if (control is IButtonControl)
                {
                    AcceptButton = control as IButtonControl;
                }
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                (sender as TextBoxBase).SelectAll();
            });
        }

        private void reprintProgramNoValue_KeyDown(object sender, KeyEventArgs e)
        {
            reprintProgramNc12Value.Text = "";
            reprintProgramNameValue.Text = "";
        }

        private void reprintProgramNc12Value_KeyDown(object sender, KeyEventArgs e)
        {
            reprintProgramNoValue.Text = "";
        }

        private void numericUpDown_Enter(object sender, EventArgs e)
        {
            var upDown = sender as NumericUpDown;

            BeginInvoke((Action)delegate
            {
                upDown.Select(0, upDown.Text.Length);
            });
        }

        private void ReorderFormatControls()
        {
            var checkBoxTop = new int[] { orderFormatCheckBox.Top, programFormatCheckBox.Top, serviceTagFormatCheckBox.Top, resistFormatCheckBox.Top };
            var checkBoxTabIndex = new int[] { orderFormatCheckBox.TabIndex, programFormatCheckBox.TabIndex, serviceTagFormatCheckBox.TabIndex, resistFormatCheckBox.TabIndex };
            var linkLabelTop = new int[] { orderFormatLinkLabel.Top, programFormatLinkLabel.Top, serviceTagFormatLinkLabel.Top, resistFormatLinkLabel.Top };
            var linkLabelTabIndex = new int[] { orderFormatLinkLabel.TabIndex, programFormatLinkLabel.TabIndex, serviceTagFormatLinkLabel.TabIndex, resistFormatLinkLabel.TabIndex };
            var comboBoxTop = new int[] { orderPrinterComboBox.Top, programPrinterComboBox.Top, serviceTagPrinterComboBox.Top, resistPrinterComboBox.Top };
            var comboBoxTabIndex = new int[] { orderPrinterComboBox.TabIndex, programPrinterComboBox.TabIndex, serviceTagPrinterComboBox.TabIndex, resistPrinterComboBox.TabIndex };

            for (var i = 0; i < settings.LabelOrder.Count; ++i)
            {
                var controls = GetFormatControls(settings.LabelOrder[i]);

                if (controls == null)
                {
                    continue;
                }

                controls[0].Top = checkBoxTop[i];
                controls[0].TabIndex = checkBoxTabIndex[i];
                controls[1].Top = linkLabelTop[i];
                controls[1].TabIndex = linkLabelTabIndex[i];
                controls[2].Top = comboBoxTop[i];
                controls[2].TabIndex = comboBoxTabIndex[i];
            }
        }

        private Control[] GetFormatControls(string type)
        {
            switch (type)
            {
                case "order":
                    return new Control[] { orderFormatCheckBox, orderFormatLinkLabel, orderPrinterComboBox };

                case "program":
                    return new Control[] { programFormatCheckBox, programFormatLinkLabel, programPrinterComboBox };

                case "serviceTag":
                    return new Control[] { serviceTagFormatCheckBox, serviceTagFormatLinkLabel, serviceTagPrinterComboBox };

                case "resist":
                    return new Control[] { resistFormatCheckBox, resistFormatLinkLabel, resistPrinterComboBox };

                default:
                    return null;
            }
        }

        private void UpdateLabelOrder()
        {
            var sortedFormatTypes = new SortedList<int, string>(4);

            sortedFormatTypes.Add(orderFormatCheckBox.Top, GetFormatTypeFromCheckBoxName(orderFormatCheckBox));
            sortedFormatTypes.Add(programFormatCheckBox.Top, GetFormatTypeFromCheckBoxName(programFormatCheckBox));
            sortedFormatTypes.Add(serviceTagFormatCheckBox.Top, GetFormatTypeFromCheckBoxName(serviceTagFormatCheckBox));
            sortedFormatTypes.Add(resistFormatCheckBox.Top, GetFormatTypeFromCheckBoxName(resistFormatCheckBox));

            settings.LabelOrder.Clear();

            foreach (var formatType in sortedFormatTypes)
            {
                settings.LabelOrder.Add(formatType.Value);
            }
        }

        private string GetFormatTypeFromCheckBoxName(CheckBox formatCheckBox)
        {
            return formatCheckBox.Name.Replace("FormatCheckBox", "");
        }

        private void SwapConfigControls(string source, string target, string suffix)
        {
            var sourceControl = configGroupBox.Controls.Find(source + suffix, false)[0];
            var targetControl = configGroupBox.Controls.Find(target + suffix, false)[0];
            var targetTop = targetControl.Top;
            var targetLeft = targetControl.Left;
            var targetTabIndex = targetControl.TabIndex;

            targetControl.Top = sourceControl.Top;
            targetControl.Left = sourceControl.Left;
            targetControl.TabIndex = sourceControl.TabIndex;
            sourceControl.Top = targetTop;
            sourceControl.Left = targetLeft;
            sourceControl.TabIndex = targetTabIndex;
        }

        private void RestoreFormatStyles()
        {
            orderFormatCheckBox.Font = new Font(orderFormatCheckBox.Font.FontFamily, orderFormatCheckBox.Font.Size);
            orderFormatCheckBox.Cursor = Cursors.Default;
            programFormatCheckBox.Font = new Font(programFormatCheckBox.Font.FontFamily, programFormatCheckBox.Font.Size);
            programFormatCheckBox.Cursor = Cursors.Default;
            serviceTagFormatCheckBox.Font = new Font(serviceTagFormatCheckBox.Font.FontFamily, serviceTagFormatCheckBox.Font.Size);
            serviceTagFormatCheckBox.Cursor = Cursors.Default;
            resistFormatCheckBox.Font = new Font(resistFormatCheckBox.Font.FontFamily, resistFormatCheckBox.Font.Size);
            resistFormatCheckBox.Cursor = Cursors.Default;
        }

        private void ToggleLicenseError()
        {
            LicenseInfo.ReadFromSettings();

            if (LicenseInfo.IsValid())
            {
                licenseErrorLabel.Hide();
            }
            else
            {
                licenseErrorLabel.Show();
            }
        }

        private void ShowLicenseForm()
        {
            var licenseForm = new LicenseForm();
            licenseForm.FormClosed += (sender, e) => ToggleLicenseError();
            licenseForm.ShowDialog(this);
        }

        private void ShowPrintForm(IList<IPrintJob> printJobs)
        {
            var printForm = new PrintForm(
                printJobs.Where(printJob => printJob.LabelQuantity > 0).ToList(),
                engine,
                orderFormat,
                programFormat,
                serviceTagFormat,
                resistFormat
            );

            printForm.ShowDialog(this);
        }

        private void ShowError(string caption, string message)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SetUpPrinters()
        {
            Printers printers;

            try
            {
                printers = new Printers();
            }
            catch (Exception)
            {
                return;
            }

            foreach (var printer in printers)
            {
                orderPrinterComboBox.Items.Add(printer.PrinterName);
                programPrinterComboBox.Items.Add(printer.PrinterName);
                serviceTagPrinterComboBox.Items.Add(printer.PrinterName);
                resistPrinterComboBox.Items.Add(printer.PrinterName);
            }

            if (printers.Count > 0)
            {
                orderPrinterComboBox.SelectedItem = printers.Any(printer => printer.PrinterName == settings.OrderPrinterName)
                    ? settings.OrderPrinterName
                    : printers.Default.PrinterName;

                programPrinterComboBox.SelectedItem = printers.Any(printer => printer.PrinterName == settings.ProgramPrinterName)
                    ? settings.ProgramPrinterName
                    : printers.Default.PrinterName;

                serviceTagPrinterComboBox.SelectedItem = printers.Any(printer => printer.PrinterName == settings.ServiceTagPrinterName)
                    ? settings.ServiceTagPrinterName
                    : printers.Default.PrinterName;

                resistPrinterComboBox.SelectedItem = printers.Any(printer => printer.PrinterName == settings.ResistPrinterName)
                    ? settings.ResistPrinterName
                    : printers.Default.PrinterName;
            }
        }

        private void SetUpOrderFormat()
        {
            orderFormatLinkLabel.Enabled = false;
            orderFormatLinkLabel.Text = "ładowanie...";

            if (orderFormat != null)
            {
                orderFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                orderFormat = null;
            }

            if (!string.IsNullOrEmpty(settings.OrderFormatFile))
            {
                try
                {
                    orderFormat = engine.Documents.Open(settings.OrderFormatFile);
                }
                catch (Exception x)
                {
                    settings.OrderFormatFile = "";

                    ShowError("Błąd otwierania etykiety zlecenia", x.Message);
                }
            }

            orderFormatLinkLabel.Text = orderFormat == null ? "wybierz..." : Path.GetFileNameWithoutExtension(settings.OrderFormatFile);
            orderFormatLinkLabel.Enabled = true;
        }

        private void SetUpProgramFormat()
        {
            programFormatLinkLabel.Enabled = false;
            programFormatLinkLabel.Text = "ładowanie...";

            if (programFormat != null)
            {
                programFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                programFormat = null;
            }

            if (!string.IsNullOrEmpty(settings.ProgramFormatFile))
            {
                try
                {
                    programFormat = engine.Documents.Open(settings.ProgramFormatFile);
                }
                catch (Exception x)
                {
                    settings.ProgramFormatFile = "";

                    ShowError("Błąd otwierania etykiety programowej", x.Message);
                }
            }

            programFormatLinkLabel.Text = programFormat == null ? "wybierz..." : Path.GetFileNameWithoutExtension(settings.ProgramFormatFile);
            programFormatLinkLabel.Enabled = true;
        }

        private void SetUpServiceTagFormat()
        {
            serviceTagFormatLinkLabel.Enabled = false;
            serviceTagFormatLinkLabel.Text = "ładowanie...";

            if (serviceTagFormat != null)
            {
                serviceTagFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                serviceTagFormat = null;
            }

            if (!string.IsNullOrEmpty(settings.ServiceTagFormatFile))
            {
                try
                {
                    serviceTagFormat = engine.Documents.Open(settings.ServiceTagFormatFile);
                }
                catch (Exception x)
                {
                    settings.ServiceTagFormatFile = "";

                    ShowError("Błąd otwierania etykiety Service Tag", x.Message);
                }
            }

            serviceTagFormatLinkLabel.Text = serviceTagFormat == null ? "wybierz..." : Path.GetFileNameWithoutExtension(settings.ServiceTagFormatFile);
            serviceTagFormatLinkLabel.Enabled = true;
        }

        private void SetUpResistFormat()
        {
            resistFormatLinkLabel.Enabled = false;
            resistFormatLinkLabel.Text = "ładowanie...";

            if (resistFormat != null)
            {
                resistFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                resistFormat = null;
            }

            if (!string.IsNullOrEmpty(settings.ResistFormatFile))
            {
                try
                {
                    resistFormat = engine.Documents.Open(settings.ResistFormatFile);
                }
                catch (Exception x)
                {
                    settings.ResistFormatFile = "";

                    ShowError("Błąd otwierania etykiety wodo/UV odpornej", x.Message);
                }
            }

            resistFormatLinkLabel.Text = resistFormat == null ? "wybierz..." : Path.GetFileNameWithoutExtension(settings.ResistFormatFile);
            resistFormatLinkLabel.Enabled = true;
        }

        private void SetUpOrderXlsx()
        {
            reloadOrderXlsxButton.Enabled = false;
            editOrderXlsxButton.Enabled = false;
            orderXlsxLinkLabel.Enabled = false;
            orderXlsxLinkLabel.Text = "ładowanie...";

            orderList.Clear();

            if (!string.IsNullOrEmpty(settings.OrderXlsxFile))
            {
                try
                {
                    orderList.ReadFromXlsxFile(settings.OrderXlsxFile);
                }
                catch (Exception x)
                {
                    settings.OrderXlsxFile = "";

                    ShowError("Błąd wczytywania zleceń", x.Message);
                }
            }

            orderXlsxLinkLabel.Text = orderList.Count == 0 ? "wybierz..." : (Path.GetFileNameWithoutExtension(settings.OrderXlsxFile) + " (" + orderList.Count + ")");
            orderXlsxLinkLabel.Enabled = true;
            reloadOrderXlsxButton.Enabled = orderList.Count > 0;
            editOrderXlsxButton.Enabled = orderList.Count > 0;
        }

        private void SetUpWorkCenterXlsx()
        {
            reloadWorkCenterXlsxButton.Enabled = false;
            editWorkCenterXlsxButton.Enabled = false;
            workCenterXlsxLinkLabel.Enabled = false;
            workCenterXlsxLinkLabel.Text = "ładowanie...";

            workCenterList.Clear();

            if (!string.IsNullOrEmpty(settings.WorkCenterXlsxFile))
            {
                try
                {
                    workCenterList.ReadFromXlsxFile(settings.WorkCenterXlsxFile);
                }
                catch (Exception x)
                {
                    settings.WorkCenterXlsxFile = "";

                    ShowError("Błąd wczytywania WorkCentrów", x.Message);
                }
            }

            workCenterXlsxLinkLabel.Text = workCenterList.Count == 0 ? "wybierz..." : (Path.GetFileNameWithoutExtension(settings.WorkCenterXlsxFile) + " (" + workCenterList.Count + ")");
            workCenterXlsxLinkLabel.Enabled = true;
            reloadWorkCenterXlsxButton.Enabled = workCenterList.Count > 0;
            editWorkCenterXlsxButton.Enabled = workCenterList.Count > 0;
        }

        private void CloseOpenFormat(string type, string formatFile)
        {
            if (orderFormat != null && orderFormat.FileName == formatFile)
            {
                settings.OrderFormatFile = "";
                SetUpOrderFormat();
            }
            
            if (programFormat != null && programFormat.FileName == formatFile)
            {
                settings.ProgramFormatFile = "";
                SetUpProgramFormat();
            }

            if (serviceTagFormat != null && serviceTagFormat.FileName == formatFile)
            {
                settings.ServiceTagFormatFile = "";
                SetUpServiceTagFormat();
            }
        }

        private void OpenOrderFormatFileDialog()
        {
            openFormatFileType = "order";
            openFormatFileDialog.Title = "Wybierz szablon etykiety zlecenia";
            openFormatFileDialog.ShowDialog(this);
        }

        private void OpenProgramFormatFileDialog()
        {
            openFormatFileType = "program";
            openFormatFileDialog.Title = "Wybierz szablon etykiety programowej";
            openFormatFileDialog.ShowDialog(this);
        }

        private void OpenServiceTagFormatFileDialog()
        {
            openFormatFileType = "serviceTag";
            openFormatFileDialog.Title = "Wybierz szablon etykiety Service Tag";
            openFormatFileDialog.ShowDialog(this);
        }

        private void OpenResistFormatFileDialog()
        {
            openFormatFileType = "resist";
            openFormatFileDialog.Title = "Wybierz szablon etykiety wodo/UV odpornej";
            openFormatFileDialog.ShowDialog(this);
        }

        private void OpenOrderXlsxFileDialog()
        {
            openXlsxFileType = "order";
            openXlsxFileDialog.Title = "Wybierz arkusz danych ze zleceniami";
            openXlsxFileDialog.ShowDialog(this);
        }

        private void OpenWorkCenterXlsxFileDialog()
        {
            openXlsxFileType = "workCenter";
            openXlsxFileDialog.Title = "Wybierz arkusz danych z WorkCentrami";
            openXlsxFileDialog.ShowDialog(this);
        }

        private IList<int> CreateSeriesQuantities(Order order)
        {
            var workCenter = workCenterList.Find(wc => wc.Id == order.WorkCenter);
            var minDivQuantity = workCenter == null ? 1 : workCenter.MinDivQuantity;
            var prodLineCount = workCenter == null ? 1 : workCenter.ProdLineCount;
            var orderQuantity = (int)(order.Quantity + orderExtraValue.Value);
            var orderDivMinQuantity = orderQuantity / minDivQuantity;
            int seriesQuantity;
            int lastSeriesQuantity;
            int seriesCount;

            if (orderDivMinQuantity < prodLineCount)
            {
                seriesQuantity = minDivQuantity;
            }
            else
            {
                seriesQuantity = (int)Math.Floor((float)orderDivMinQuantity / prodLineCount * minDivQuantity);
            }

            seriesCount = (int)Math.Ceiling((float)orderQuantity / seriesQuantity);
            lastSeriesQuantity = orderQuantity % seriesQuantity;

            var seriesQuantities = new List<int>(seriesCount);

            if (lastSeriesQuantity > 0)
            {
                --seriesCount;
            }

            for (var i = 0; i < seriesCount; ++i)
            {
                seriesQuantities.Add(seriesQuantity);
            }

            if (lastSeriesQuantity > 0)
            {
                seriesQuantities.Add(lastSeriesQuantity);
            }

            return seriesQuantities;
        }

        private void CreatePrintJobs(List<IPrintJob> printJobs, Order order, int seriesGroup)
        {
            var seriesQuantities = CreateSeriesQuantities(order);

            for (var i = 0; i < seriesQuantities.Count; ++i)
            {
                CreatePrintJobs(printJobs, order, seriesGroup, seriesQuantities, i);
            }
        }

        private void CreatePrintJobs(List<IPrintJob> printJobs, Order order, int seriesGroup, IList<int> seriesQuantities, int seriesIndex)
        {
            var seriesNo = seriesQuantities.Count > 1 ? (seriesIndex + 1) : 0;

            foreach (var formatType in settings.LabelOrder)
            {
                switch (formatType)
                {
                    case "order":
                        if (orderFormatCheckBox.Checked)
                        {
                            CreateOrderPrintJobs(printJobs, order, seriesGroup, seriesNo);
                        }
                        break;

                    case "program":
                        if (programFormatCheckBox.Checked)
                        {
                            CreateProgramPrintJobs(printJobs, order, seriesGroup, seriesNo, seriesQuantities[seriesIndex]);
                        }
                        break;

                    case "serviceTag":
                        if (serviceTagFormatCheckBox.Checked)
                        {
                            CreateServiceTagPrintJobs(printJobs, order, seriesGroup, seriesNo, seriesQuantities, seriesIndex);
                        }
                        break;

                    case "resist":
                        if (resistFormatCheckBox.Checked)
                        {
                            CreateResistPrintJobs(printJobs, order, seriesGroup, seriesNo, seriesQuantities[seriesIndex]);
                        }
                        break;
                }
            }
        }

        private void CreateOrderPrintJobs(List<IPrintJob> printJobs, Order order, int seriesGroup, int seriesNo)
        {
            printJobs.Add(new OrderPrintJob(order.No, order.Quantity)
            {
                PrinterName = settings.OrderPrinterName,
                LabelQuantity = 1,
                SeriesNo = seriesNo,
                SeriesGroup = seriesGroup
            });
        }

        private void CreateProgramPrintJobs(List<IPrintJob> printJobs, Order order, int seriesGroup, int seriesNo, int labelQuantity)
        {
            if (string.IsNullOrEmpty(order.ProgramName))
            {
                return;
            }

            printJobs.Add(new ProgramPrintJob(order.Nc12, order.ProgramName)
            {
                PrinterName = settings.ProgramPrinterName,
                LabelQuantity = labelQuantity,
                SeriesNo = seriesNo,
                SeriesGroup = seriesGroup
            });
        }

        private void CreateServiceTagPrintJobs(List<IPrintJob> printJobs, Order order, int seriesGroup, int series, IList<int> seriesQuantities, int seriesIndex)
        {
            var firstSerialNo = 1;

            for (var i = 0; i < seriesIndex; ++i)
            {
                firstSerialNo += seriesQuantities[i];
            }

            printJobs.Add(new ServiceTagPrintJob(order.No, firstSerialNo)
            {
                PrinterName = settings.ServiceTagPrinterName,
                LabelQuantity = seriesQuantities[seriesIndex],
                SeriesNo = series,
                SeriesGroup = seriesGroup
            });
        }

        private void CreateResistPrintJobs(List<IPrintJob> printJobs, Order order, int seriesGroup, int series, int labelQuantity)
        {
            if (string.IsNullOrEmpty(order.ResistText))
            {
                return;
            }

            printJobs.Add(new ResistPrintJob(order.No, order.ResistText)
            {
                PrinterName = settings.ResistPrinterName,
                LabelQuantity = labelQuantity,
                SeriesNo = series,
                SeriesGroup = seriesGroup
            });
        }

        private void FocusAcceptButton()
        {
            if (AcceptButton is Button)
            {
                (AcceptButton as Button).Focus();
            }
        }
    }
}
