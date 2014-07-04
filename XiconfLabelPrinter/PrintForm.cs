// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Management;
using System.Media;
using System.Text;
using System.Windows.Forms;
using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public partial class PrintForm : Form
    {
        private int currentPrintJob = -1;

        private bool cancelled = false;

        private bool closing = false;

        private bool finished = true;

        private IList<IPrintJob> printJobs;

        private Engine engine;

        private LabelFormatDocument orderFormat;

        private LabelFormatDocument programFormat;

        private LabelFormatDocument serviceTagFormat;

        public PrintForm(IList<IPrintJob> printJobs, Engine engine, LabelFormatDocument orderFormat, LabelFormatDocument programFormat, LabelFormatDocument serviceTagFormat)
        {
            InitializeComponent();

            this.printJobs = printJobs;
            this.engine = engine;
            this.orderFormat = orderFormat;
            this.programFormat = programFormat;
            this.serviceTagFormat = serviceTagFormat;

            CreateSeriesGroups();
            CreatePrintJobItems();
        }

        private void PrintForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = e.CloseReason == CloseReason.UserClosing && backgroundWorker.IsBusy;

            if (e.Cancel)
            {
                closing = true;
                Cancel();
            }
            else if (!finished)
            {
                DeletePrintJobs();
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            if (currentPrintJob == -1)
            {
                foreach (ListViewItem listItem in printJobListView.Items)
                {
                    if (!listItem.Checked)
                    {
                        listItem.BackColor = Color.LightGray;
                    }
                }

                printJobListView.CheckBoxes = false;
            }

            finished = false;
            UseWaitCursor = true;

            AcceptButton = null;
            cancelButton.Show();
            printButton.Hide();

            RunNextPrintJob(currentPrintJob == -1);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var workerArgs = e.Argument as PrintWorkerArguments;
            
            lock (workerArgs.Engine)
            {
                workerArgs.PrintJob.Print(workerArgs.LabelFormat);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = (int)Math.Round((currentPrintJob + 1) * (100d / printJobs.Count));

            if (e.Error == null)
            {
                HandleSuccess();
            }
            else
            {
                HandleFailure(e.Error);
            }
        }

        private void CreateSeriesGroups()
        {
            if (printJobs[0].SeriesNo == 0)
            {
                printJobListView.ShowGroups = false;

                return;
            }

            printButton.Text = "DRUKUJ SERIĘ " + printJobs[0].SeriesId;
            printJobListView.ShowGroups = true;

            var lastSeriesId = "";

            foreach (var printJob in printJobs)
            {
                var seriesId = printJob.SeriesId;

                if (seriesId != lastSeriesId)
                {
                    printJobListView.Groups.Add(seriesId, "Seria " + seriesId);

                    lastSeriesId = seriesId;
                }
            }
        }

        private void CreatePrintJobItems()
        {
            foreach (var printJob in printJobs)
            {
                CreatePrintJobItem(printJob);
            }
        }

        private void CreatePrintJobItem(IPrintJob printJob)
        {
            var formatItem = new ListViewItem(printJob.LabelFormatType)
            {
                Checked = true,
                Group = printJob.SeriesNo == 0 ? null : printJobListView.Groups[printJob.SeriesId]
            };

            formatItem.SubItems.Add(printJob.LabelQuantity.ToString());
            formatItem.SubItems.Add(printJob.AdditionalInfo);
            formatItem.SubItems.Add(printJob.PrinterName);

            printJobListView.Items.Add(formatItem);
        }

        private bool IsLastPrintJob()
        {
            return currentPrintJob == printJobs.Count - 1;
        }

        private void RunNextPrintJob(bool checkSeries = true)
        {
            if (checkSeries && CheckSeriesChange())
            {
                HandleSeriesChange();
                return;
            }

            ++currentPrintJob;

            var singleStep = 100d / printJobs.Count;

            progressBar.Value = (int)Math.Round((double)currentPrintJob * singleStep + singleStep / 2d);

            if (printJobListView.Items[currentPrintJob].Checked)
            {
                printJobListView.Items[currentPrintJob].BackColor = Color.Orange;

                backgroundWorker.RunWorkerAsync(new PrintWorkerArguments(printJobs[currentPrintJob], engine, orderFormat, programFormat, serviceTagFormat));
            }
            else if (IsLastPrintJob())
            {
                Finish();
            }
            else
            {
                RunNextPrintJob();
            }
        }

        private void HandleSeriesChange()
        {
            UseWaitCursor = false;
            AcceptButton = printButton;

            var seriesId = printJobs[currentPrintJob + 1].SeriesId;

            printButton.Text = "DRUKUJ SERIĘ " + seriesId;
            printButton.Show();
            printButton.Focus();
            cancelButton.Hide();

            var group = printJobListView.Groups[seriesId];
            group.Items[group.Items.Count - 1].EnsureVisible();

            Activate();
            SystemSounds.Beep.Play();
        }

        private bool CheckSeriesChange()
        {
            return currentPrintJob != -1 && printJobs[currentPrintJob].SeriesId != printJobs[currentPrintJob + 1].SeriesId;
        }

        private void HandleSuccess()
        {
            HighlightPrintJob(Color.LightGreen);

            if (cancelled)
            {
                HandleCancellation();
            }
            else if (IsLastPrintJob())
            {
                Finish();
            }
            else
            {
                RunNextPrintJob();
            }
        }

        private void HandleFailure(Exception x)
        {
            HighlightPrintJob(Color.LightCoral);

            if (cancelled)
            {
                HandleCancellation();
                return;
            }

            var message = new StringBuilder();

            message.AppendLine("Błąd podczas drukowania:");
            message.AppendLine();
            message.AppendLine(x.Message.Length == 0 ? "?!?" : x.Message);

            if (IsLastPrintJob())
            {
                MessageBox.Show(this, message.ToString(), "Błąd drukowania", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                Finish();
            }
            else
            {
                message.AppendLine();
                message.AppendLine("Kontynuować drukowanie?");

                var dialogResult = MessageBox.Show(this, message.ToString(), "Błąd drukowania", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                {
                    RunNextPrintJob();
                }
                else
                {
                    MarkRemainingJobsAsFailures();
                    Finish();
                }
            }
        }

        private void HandleCancellation()
        {
            MarkRemainingJobsAsFailures();
            DeletePrintJobs();
            Finish();
        }

        private void Finish()
        {
            finished = true;
            UseWaitCursor = false;
            AcceptButton = closeButton;

            closeButton.Show();
            cancelButton.Hide();
            closeButton.Focus();

            if (closing)
            {
                Close();
            }
        }

        private void Cancel()
        {
            if (cancelled)
            {
                return;
            }

            cancelled = true;

            cancelButton.Enabled = false;
            cancelButton.Text = "Anulowanie...";
            cancelButton.Show();
            printButton.Hide();
        }

        private void HighlightPrintJob(Color color, int printJobIndex = -1)
        {
            if (printJobIndex == -1)
            {
                printJobIndex = currentPrintJob;
            }

            printJobListView.Items[printJobIndex].EnsureVisible();
            printJobListView.Items[printJobIndex].BackColor = color;
        }

        private void MarkRemainingJobsAsFailures()
        {
            for (int i = currentPrintJob + 1, l = printJobs.Count; i < l; ++i)
            {
                if (printJobListView.Items[i].Checked)
                {
                    HighlightPrintJob(Color.LightCoral, i);
                }
            }
        }

        private void DeletePrintJobs()
        {
            var printJobMap = new Dictionary<string, string>();

            foreach (var printJob in printJobs)
            {
                printJobMap[printJob.PrintJobName] = printJob.PrinterName;
            }

            using (ManagementObjectSearcher jobQuery = new ManagementObjectSearcher("SELECT * FROM Win32_PrintJob"))
            {
                using (ManagementObjectCollection jobs = jobQuery.Get())
                {
                    foreach (ManagementObject job in jobs)
                    {
                        TryDeletePrintJob(printJobMap, job);
                    }
                }
            }
        }

        private void TryDeletePrintJob(Dictionary<string, string> printJobMap, ManagementObject job)
        {
            try
            {
                var printJobName = (string)job["Document"];
                var jobId = (string)job["Name"];
                var commaIndex = jobId.LastIndexOf(',');
                var printerName = jobId.Substring(0, commaIndex);

                if (printJobMap.ContainsKey(printJobName) && printerName.Equals(printJobMap[printJobName]))
                {
                    job.Delete();
                }
            }
            catch (Exception) {}
        }
    }
}
