using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Fb2Kindle.Localization;

namespace Fb2Kindle
{
    public partial class FormMain : Form
    {
        private LocaleManager _locale;
        private KindleFinder _kindle;
        private ConverterHelper _converter;
        private ConcurrentQueue<string> _files;
        private Thread _converterThread;
        private bool _converterThreadRunning;
        private int _success;

        public FormMain()
        {
            InitializeComponent();
            _locale = new LocaleManager();
            _files = new ConcurrentQueue<string>();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            lblStatus2.Text = "";
            _kindle = new KindleFinder();
            if (!_kindle.IsFound)
            {
                ShowError(_locale.GetString("DeviceNotFound"));
            }
            else
            {
                lblStatus.Text = _locale.GetString("DeviceFound", _kindle.Mount);
                lblStatus.ForeColor = Color.DarkSlateGray;
            }

            _converter = new ConverterHelper();
            if (!_converter.IsFound)
            {
                ShowError(_locale.GetString("ConverterNotFound"));
            }

            if (_kindle.IsFound && _converter.IsFound)
            {
                AllowDrop = true;
                _converterThreadRunning = true;
                _converterThread = new Thread(new ThreadStart(ConverterThreadProc));
                _converterThread.Start();
            }
        }

        private void ShowError(string text, string log = null)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => ShowError(text, log)));
            }
            else
            {
                picWorking.Visible = false;
                lblStatus.Text = text;
                lblStatus.ForeColor = Color.OrangeRed;
                lblStatus2.Text = "";
                AllowDrop = false;

                if (!string.IsNullOrEmpty(log))
                {
                    var formLog = new FormLog();
                    formLog.SetText(log);
                    formLog.Show();
                }
            }
        }

        private void ShowBusy(string file)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => ShowBusy(file)));
            }
            else
            {
                picWorking.Visible = true;
                lblStatus2.Text = _locale.GetString("Working");
                lblStatus2.ForeColor = Color.DarkSlateGray;
                lblStatus.Text = GetCurrentFileStatus(file);
                lblStatus.ForeColor = Color.DarkGray;
            }
        }

        private string GetCurrentFileStatus(string file)
        {
            const int MaxLen = 28;
            if (file.Length <= MaxLen) return file;

            var parts = file.Split('\\').Reverse().ToArray();
            string name = parts[0];
            for (var i = 1; i < parts.Length; i++)
            {
                name = parts[i] + "\\" + name;
                if (name.Length >= MaxLen) break;
            }

            return "..." + name.Substring(Math.Max(0, name.Length - MaxLen - 3));
        }

        private void SuccessfullyConverted()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => SuccessfullyConverted()));
            }
            else
            {
                picWorking.Visible = false;
                lblStatus.Text = _locale.GetString("Success", ++_success);
                lblStatus.ForeColor = Color.LimeGreen;
                lblStatus2.Text = "";
            }
        }

        private void ConverterThreadProc()
        {
            while (_converterThreadRunning)
            {
                if (_files.TryDequeue(out string file))
                {
                    ShowBusy(file);

                    if (!_converter.Run(file, _kindle.Documents))
                    {
                        ShowError(_locale.GetString("ConverterError"), _converter.Log);
                    }
                    else
                    {
                        SuccessfullyConverted();
                    }
                }
                Thread.Sleep(0);
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                if (file.ToLowerInvariant().EndsWith(".epub") ||
                    file.ToLowerInvariant().EndsWith(".fb2") ||
                    file.ToLowerInvariant().EndsWith(".fbz") ||
                    file.ToLowerInvariant().EndsWith(".fb2.zip") ||
                    Convert.ToBoolean(File.GetAttributes(file) & FileAttributes.Directory))
                {
                    _files.Enqueue(file);
                }
            }
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            // Arrow
            var bigArrow = new AdjustableArrowCap(2, 1);
            var p = new Pen(Color.LightGray, 85)
            {
                CustomEndCap = bigArrow
            };
            e.Graphics.DrawLine(p, 140, 40, 140, 200);

            // Border
            const int BorderWidth = 7;
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                Color.LightGray, BorderWidth, ButtonBorderStyle.Dotted,
                Color.LightGray, BorderWidth, ButtonBorderStyle.Dotted,
                Color.LightGray, BorderWidth, ButtonBorderStyle.Dotted,
                Color.LightGray, BorderWidth, ButtonBorderStyle.Dotted);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _converterThreadRunning = false;
            if (_converterThread != null) _converterThread.Join();
        }
    }
}
