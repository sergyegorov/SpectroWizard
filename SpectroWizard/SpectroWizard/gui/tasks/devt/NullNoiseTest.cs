using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.util;
using SpectroWizard.data;
using System.IO;

namespace SpectroWizard.gui.tasks.devt
{
    public partial class NullNoiseTest : UserControl,DevTestInterface
    {
        public NullNoiseTest()
        {
            InitializeComponent();
            Csv = new CSV();
            ReloadData();
        }

        Spectr[] Sp = new Spectr[7];
        void ReloadData()
        {
            try
            {
                string path = Common.DbNameSienceSensFolder + "\\null_noise_test.csv";
                DataBase.CheckPath(ref path);
                if (File.Exists(path))
                    Csv.AddFile(path);
                dgvData.DataSource = GetData();
                for (int i = 0; i < Times.Length; i++)
                {
                    if (i >= dgvData.Rows.Count)
                        break;
                    dgvData.Rows[i].HeaderCell.Value = "" + Times[i];
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            for (int i = 0; i < Times.Length; i++)
                try
                {
                    string path = Common.DbNameSienceSensFolder + "\\null_noise_test" + Times[i] + ".csv";
                    if(Spectr.IsFileExists(path) == false)
                        continue;
                    Sp[i] = new Spectr(path);
                    spView.AddSpectr(Sp[i], ""+Times[i]);
                }
                catch(Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
        }

        CSV Csv;
        double[] Times = { 0, 0.1, 0.5, 1, 2, 4, 8 };
        public DataTable GetData()
        {
            DataTable ret = new DataTable();
            ret.Columns.Clear();
            int[] sizes = Common.Dev.Reg.GetSensorSizes();
            for (int s = 0; s < sizes.Length; s++)
                ret.Columns.Add("Sn" + (s + 1),typeof(String));
            for (int i = 0; i < Csv.Data.Count; i++)
                ret.Rows.Add(Csv.Data[i]);
            return ret;
        }

        string Report = "";
        public bool Run()
        {
            //report = "";
            return true;
        }//*/

        public string GetReport()
        {
            return Report;
        }
    }
}
