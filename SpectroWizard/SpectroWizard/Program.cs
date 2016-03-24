using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Threading;
using System.IO;

namespace SpectroWizard
{
    static class Program
    {
        static string GlobalExceptionFileName = "GlobalException.txt";
        static string GlobalExceptionFileBkpName = "GlobalException.txt.bkp";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (File.Exists(GlobalExceptionFileName))
                {
                    File.Copy(GlobalExceptionFileName, GlobalExceptionFileBkpName, true);
                    File.Delete(GlobalExceptionFileName);
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Login());
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("GlobalException.txt", ex.ToString());
            }
        }
    }
}
