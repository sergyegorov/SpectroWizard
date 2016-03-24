using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SpectroWizard.util;
using SpectroWizard.data;

namespace SpectroWizard.data
{
    public partial class LyDb : Form
    {
        CSV Datas = new CSV();
        public LyDb()
        {
            InitializeComponent();
            string[] names = Directory.GetFiles(Common.DBBaseEnv + Common.DbNameLyDb,"*.csv");
            foreach (string name in names)
                Datas.AddFile(name);
            for (int i = 0; i < Datas.Data.Count; i++)
                try
                {
                    if (ConvertLine(Datas.Data[i]) == false)
                    {
                        Datas.Data.RemoveAt(i);
                        i--;
                    }
                }
                catch (Exception ex)
                {
                    //Common.Log(ex);
                    Datas.Data.RemoveAt(i);
                    i--;
                }
        }

        bool ConvertLine(List<object> line)
        {
            line[0] = ElementTable.FindIndex((string)line[0]);
            line[1] = ElementTable.FindIndex((string)line[1]);
            if ((int)line[1] < 0)
                return false;
            line[2] = serv.ParseDouble((string)line[2]);
            line[3] = serv.ParseDouble((string)line[3]);
            line[4] = serv.ParseDouble((string)line[4]);
            line[5] = serv.ParseDouble((string)line[5]);
            return true;
        }

        List<int> CSV_Indexes = new List<int>();
        public double Show(int element_base,int element,int line_num,Control f)
        {
            CSV_Indexes.Clear();
            lbRecomendations.Items.Clear();
            for (int i = 0; i < Datas.Data.Count; i++)
            {
                if (element_base > 0 && (int)Datas.Data[i][0] != element_base)
                    continue;
                if (element != (int)Datas.Data[i][1]) 
                    continue;
                CSV_Indexes.Add(i);
                lbRecomendations.Items.Add("" + Datas.Data[i][2 + line_num] + "   " + Datas.Data[i][6] + "    (" + Datas.Data[i][2] + "/" + Datas.Data[i][3]+")");
            }
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            ShowDialog(f);
            if(lbRecomendations.SelectedIndex < 0 || DialogResult != DialogResult.OK)
                return -1;
            return (double)Datas.Data[CSV_Indexes[lbRecomendations.SelectedIndex]][2 + line_num];
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                Visible = false;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Visible = false;
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void lbRecomendations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btOk.Enabled = lbRecomendations.SelectedIndex >= 0;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void lbRecomendations_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lbRecomendations.SelectedIndex >= 0)
                    btOk_Click(sender, e);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
