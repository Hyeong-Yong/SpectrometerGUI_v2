using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spectrometer;

namespace Spectrometer
{
    public partial class SerialForm : Form
    {
        
        public SerialForm()
        {
            InitializeComponent();
        }

        private void txtPortname_DropDown(object sender, EventArgs e)
        {
            txtPortname.Items.Clear();
            string[] COMs = SerialPort.GetPortNames();
            foreach (var item in COMs)
            {
                txtPortname.Items.Add(item);
            }
        }

        private void SerialForm_Load(object sender, EventArgs e)
        {
            LoadConfigurationSetting();
        }
        private void LoadConfigurationSetting()
        {
            txtPortname.Text = ConfigurationManager.AppSettings["comname"];
        }

        private void btnSerial_Click(object sender, EventArgs e)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("comname");

                config.AppSettings.Settings.Add("comname", txtPortname.Text);

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                flag.loading_Flag = true;
                this.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show("saving Error," + exc.Message);
            }
        }
        
    }
}
