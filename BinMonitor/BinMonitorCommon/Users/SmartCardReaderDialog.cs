using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GS.Apdu;
using GS.PCSC;
using GS.SCard;
using GS.Util.Hex;
using System.Diagnostics;
using System.Media;

namespace BinMonitor.Common
{
    public partial class SmartCardReaderDialog : Form
    {
        private Timer PollingTimer;

        private static string DefaultReaderName
        { get { return Settings.SmartCardReader.Default.ReaderName; } }

        private static int DefaultTimeout
        { get { return Settings.SmartCardReader.Default.Timeout; } }

        private static int DefaultPollingFrequency
        { get { return Settings.SmartCardReader.Default.PollingFrequency; } }

        public int PollingFrequency
        {
            get { return PollingTimer.Interval; }
            set { PollingTimer.Interval = value; }
        }

        public const int DEFAULT_TIMEOUT = 10;
        private int _Timeout = DEFAULT_TIMEOUT;
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }


        private string _ReaderName;
        public string ReaderName
        {
            get { return _ReaderName; }
            set { _ReaderName = value; }
        }

        private DateTime PollingStartedAt;
        private DateTime PollingTimesOutAt
        {
            get { return PollingStartedAt.AddSeconds(this.Timeout); }
        }

        [Browsable(false)]
        public string Uid { get; protected set; }


        public SmartCardReaderDialog()
        {
            InitializeComponent();
            PollingTimer = new Timer() { Interval = DefaultPollingFrequency };
            PollingTimer.Tick += PollingTimer_Tick;
        }

        void PollingTimer_Tick(object sender, EventArgs e)
        {
            PCSCReader reader = new PCSCReader();
            try
            {
                Trace.TraceInformation("Connecting to reader");
                reader.Connect(this.ReaderName);
                Trace.TraceInformation("Checking reader state");
                if (reader.SCard.GetCardPresentState(this.ReaderName))
                {
                    reader.SCard.Connect(this.ReaderName, GS.SCard.Const.SCARD_SHARE_MODE.Shared, GS.SCard.Const.SCARD_PROTOCOL.Tx);
                    Trace.TraceInformation("Requesting UID");
                    RespApdu uidResponse = reader.Exchange("FF CA 00 00 00"); //Get Card UID
                    if (uidResponse.Data == null)
                    { throw new Exception("UID could not be read " + uidResponse.SW1SW2.ToString()); }

                    string uid = HexFormatting.ToHexString(uidResponse.Data);

                    this.Uid = uid;
                    SystemSounds.Beep.Play();
                    this.PollingTimer.Stop();
                    this.DialogResult = DialogResult.OK;
                }
                else if (DateTime.Now > this.PollingTimesOutAt)
                {
                    this.PollingTimer.Stop();
                    throw new TimeoutException("Timeout");
                }
            }
            catch (WinSCardException ex)
            {
                lblCaption.BackColor = Color.Red;
                lblCaption.Text = ex.WinSCardFunctionName + " Error 0x" +
                                   ex.Status.ToString("X08") + ": " + ex.Message;
                this.PollingTimer.Stop();
            }
            catch (Exception ex)
            {
                lblCaption.BackColor = Color.Red;
                lblCaption.Text = ex.Message;
                this.PollingTimer.Stop();
            }
            finally
            {
                reader.Disconnect();
            }
        }

        private void SmartCardReaderDialog_Shown(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            this.Uid = null;
            this.PollingStartedAt = DateTime.Now;
            lblCaption.BackColor = Color.LightGreen;
            lblCaption.Text = "Listening to " + this.ReaderName;
            this.PollingTimer.Start();
        }

        private void SmartCardReaderDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel == false)
            { this.PollingTimer.Stop(); }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public static string PromptForUid(IWin32Window owner, string readerName, int timeout, int pollingFrequency)
        {
            SmartCardReaderDialog dlg = new SmartCardReaderDialog()
            {
                ReaderName = readerName,
                Timeout = timeout,
                PollingFrequency = pollingFrequency,
                StartPosition = FormStartPosition.CenterParent
            };
            if (dlg.ShowDialog(owner) != DialogResult.OK)
            { throw new OperationCanceledException(); }

            if (string.IsNullOrWhiteSpace(dlg.Uid))
            { throw new Exception("UID was not read"); }

            return dlg.Uid;
        }

        public static string PromptForUid(IWin32Window owner)
        { return PromptForUid(owner, DefaultReaderName, DefaultTimeout, DefaultPollingFrequency); }

        


    }
}
