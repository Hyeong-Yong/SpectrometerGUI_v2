using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrometer
{

    public partial class frmloading : Form
    {
        System.Windows.Forms.Timer _tm;


        public frmloading()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _tm_Tick(object sender, EventArgs e)
        {
            bool bEndcheck = false;
            int iPer = pBar.Value;
            Random rd = new Random();

            iPer = iPer + rd.Next(30);

            if (iPer>100)
            {
                iPer = 100;
                bEndcheck = true;
            }

            pBar.Value = iPer;
            lblNow.Text = iPer.ToString() +"%";

            if (bEndcheck)
            {
                _tm.Stop();
                this.Close();
            }
        }

        private void frmloading_Load(object sender, EventArgs e)
        {
            _tm = new System.Windows.Forms.Timer();
            _tm.Interval = 100;
            _tm.Tick += _tm_Tick;
            _tm.Start();
        }
    }
}
