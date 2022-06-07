using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrometer
{
    public partial class MainForm : Form
    {
        SerialComm serialComm = new SerialComm();

        const string strVersion = "1.1";
        public MainForm()
        {
            InitializeComponent();
            LoadConfigurationSetting();

        }

        private void LoadConfigurationSetting()
        {
            lblPortname.Text = ConfigurationManager.AppSettings["comname"];
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Version oVersion = Assembly.GetEntryAssembly().GetName().Version;
            this.Text = string.Format("{0} Ver.{1}.{2} / Build Time {3} - {4}", "HY Spectrometer ", oVersion.Major, oVersion.Minor, GetBuildDataTime(oVersion), "Status");

            GetBuildDataTime(oVersion);

            serialPort.PortName = lblPortname.Text;
            serialPort.BaudRate = 57600;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;

            serialPort.Open();
            if (serialPort.IsOpen)
            { 
            btnConnect.Enabled = false;
            }


            int[] plotdata = { 1, 2, 3, 4 };

        }

        public DateTime GetBuildDataTime(Version oVersion)
        {
            string strVersion = oVersion.ToString();

            //날짜 등록
            int iDays = Convert.ToInt32(strVersion.Split('.')[2]);
            DateTime refData = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refData.AddDays(iDays);

            //초 등록
            int iSeconds = Convert.ToInt32(strVersion.Split('.')[3]);
            iSeconds = iSeconds * 2;
            dtBuildDate = dtBuildDate.AddSeconds(iSeconds);

            //시차 조정
            DaylightTime daylighttime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);

            if (TimeZone.IsDaylightSavingTime(dtBuildDate, daylighttime))
            {
                dtBuildDate = dtBuildDate.Add(daylighttime.Delta);
            }


            return dtBuildDate;

        }


        private int rx_data;

        private void SerialThread()
        {
            try
            {
                Thread serailThread = new Thread(SerialMethod);
                serailThread.Start();
            }
            catch
            {

            }
        }

        private void SerialMethod()
        {
            try
            {
                while (serialPort.IsOpen) //SerialThread 객체에서 SerialMethod 메소드를 사용하여 계속 데이터를 취득함.
                {
                    rx_data = serialPort.ReadByte();
                    showRxData(rx_data);
                    Thread.Sleep(1);
                }
            }
            catch
            {
            }
        }

        private bool rxLogFlag = true;

        public delegate void ShowSerialDataDelegate(int r);
        private void showRxData(int rx_data) //Serial 스레드에서 UI 스레드 접근 시, Delegate로 접근
        {
            if (chart.InvokeRequired) //입력받을 데이터 넣는 곳
            {
                ShowSerialDataDelegate SSDD = showRxData;
                Invoke(SSDD, rx_data);
            }//기다림
            else
            {//기다림 후 실행

                if (serialComm.msgParse(((byte)rx_data), 100) == true & rxLogFlag == true)
                {
                    /* 입력 받을 데이터를 차트로 넣기 */
                    //chart.Text += serialComm.PrintData;
                    //txtRead.Text += Environment.NewLine + "------------------" + Environment.NewLine;
                    //txtRead.Text += "Packet Length : " + serialComm.rxPKT.Length.ToString() + Environment.NewLine;
                    //txtRead.Text += "Instruction : " + serialComm.rxPKT.Inst.ToString() + Environment.NewLine;
                    //txtRead.Text += "Parameter :" + serialComm.rxPKT.Param.ToString() + Environment.NewLine;
                    //txtRead.ScrollToCaret();
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;

            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serialPort.Open();
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
        }
    }
}
