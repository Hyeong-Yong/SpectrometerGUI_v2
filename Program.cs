using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spectrometer;

namespace Spectrometer
{
    public class flag
    {
        public static bool loading_Flag = false;
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SerialForm());
            Application.Run(new frmloading());
            Application.Run(new MainForm());
        }
    }
}
