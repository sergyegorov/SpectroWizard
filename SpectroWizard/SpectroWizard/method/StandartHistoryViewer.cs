using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.method
{
    public partial class StandartHistoryViewer : Form
    {
        public StandartHistoryViewer()
        {
            InitializeComponent();
            //lbMeasuringList.Font = new Font(FontFamily.GenericMonospace, 10);
        }

        StandartHistory History;
        List<string> Tabs;
        List<long> Probs;
        public void Init(StandartHistory history)
        {
            History = history;
            Tabs = history.GetElementList();
            tbNames.TabPages.Clear();
            foreach (string tab in Tabs)
                tbNames.TabPages.Add(tab);
            Probs = history.GetMeasuringList();
            lbMeasuringList.Items.Clear();
            for (int i = 0; i < Probs.Count; i++)
                lbMeasuringList.Items.Add((new DateTime(Probs[i])).ToString() + "  " + history.ExtraNames[i]);
            CheckSelection();
        }

        double[] X;
        double[][] Y;
        //string[] Names;
        double MinX, MaxX, MinY, MaxY;
        Pen[] RedPens = null;
        public void CheckSelection()
        {
            if (tbNames.TabPages.Count == 0)
                return;
            if (SelectedProb >= 0)
            {
                StandartHistryRecord rec = History[Probs[SelectedProb], Tabs[tbNames.SelectedIndex]];
                gLog.Setup(rec.LogData, Common.LogCalcSectionName);
                pDrawPanel.Refresh();
            }

            if (RedPens == null)
            {
                RedPens = new Pen[256];
                for (int i = 0; i < RedPens.Length; i++)
                    RedPens[i] = new Pen(Color.FromArgb(i, 0, 0));
            }
            X = new double[Probs.Count];
            //Names = new string[Probs.Count];
            Y = new double[Probs.Count][];
            MinX = double.MaxValue;
            MaxX = -double.MaxValue;
            MinY = double.MaxValue;
            MaxY = -double.MaxValue;
            double min = double.MaxValue;
            for (int i = 0; i < Probs.Count; i++)
                min = Math.Min(Probs[i], min);
            for (int i = 0; i < Probs.Count; i++)
            {
                switch(cbXType.SelectedIndex)
                {
                    case 0:
                        X[i] = i;
                        break;
                    case 2:
                        if (i == 0)
                            X[i] = Probs[i]/600000000;
                        else
                        {
                            double dltt = (Probs[i] - Probs[i - 1])/600000000;
                            if (dltt < 0)
                                dltt = -dltt;
                            if (dltt < 1)
                                dltt = 1;
                            //X[i] = Math.Log(dltt) + X[i - 1];
                            X[i] = Math.Sqrt(dltt) + X[i - 1];
                        }
                        break;
                    default:
                        X[i] = Probs[i];
                        break;
                }
                if (X[i] < MinX)
                    MinX = X[i];
                if (X[i] > MaxX)
                    MaxX = X[i];
                
                StandartHistryRecord r = History[Probs[i], Tabs[tbNames.SelectedIndex]];
                Y[i] = (double[])r.Cons.Clone();
                //Names[i] = Math.Round(r.Con,3).ToString();
                for (int j = 0; j < Y[i].Length; j++)
                {
                    if (Y[i][j] < MinY)
                        MinY = Y[i][j];
                    if (Y[i][j] > MaxY)
                        MaxY = Y[i][j];
                }
            }

            double dlt = (MaxX - MinX) / 20;
            if (dlt == 0)
                dlt = 1;
            MaxX += dlt;
            MinX -= dlt;

            dlt = (MaxY - MinY) / 20;
            if (dlt == 0)
                dlt = 1;
            MaxY += dlt;
            MinY -= dlt;

            double dx = MaxX - MinX;
            double dy = MaxY - MinY;
            for (int i = 0; i < Probs.Count; i++)
            {
                X[i] = (X[i] - MinX) / dx;
                for (int j = 0; j < Y[i].Length; j++)
                    Y[i][j] = (Y[i][j] - MinY) / dy;
            }

            pDrawPanel.Refresh();
        }

        private void tbNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }


        private void pDrawPanel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                double[] yg = serv.GetGoodValues(MinY, MaxY, pDrawPanel.Height / 30);
                for (int i = 0; i < yg.Length; i++)
                {
                    int yy = pDrawPanel.Height - (int)(pDrawPanel.Height * yg[i]);
                    e.Graphics.DrawLine(Pens.LightGray, 0, yy, pDrawPanel.Width, yy);
                    e.Graphics.DrawString(serv.GetGoodValue(yg[i], 3), Common.GraphLitleFont, Brushes.Gray, 10, yy);
                }
                int size = 5;
                int xp = -10000, yp = 0;
                for (int i = 0; i < X.Length; i++)
                {
                    int x = (int)(pDrawPanel.Width * X[i]);
                    if (i == SelectedProb)
                        e.Graphics.DrawLine(Pens.Brown, x, 0, x, pDrawPanel.Height);
                    int y;
                    double[] ever_data = new double[Y[i].Length];
                    for (int j = 0; j < Y[i].Length; j++)
                    {
                        y = pDrawPanel.Height - (int)(pDrawPanel.Height * Y[i][j]);
                        int color_index = j * 256 / Y[i].Length;
                        if (color_index < 0)
                            color_index = 0;
                        if (color_index > 255)
                            color_index = 255;
                        e.Graphics.DrawLine(RedPens[color_index], x - size, y, x + size, y);
                        e.Graphics.DrawLine(RedPens[color_index], x, y - size, x, y + size);
                        ever_data[j] = Y[i][j];
                    }
                    double ever = SpectroWizard.analit.Stat.GetEver(ever_data);
                    y = pDrawPanel.Height - (int)(pDrawPanel.Height * ever);
                    e.Graphics.DrawLine(Pens.Green, x - size, y, x + size, y);
                    e.Graphics.DrawLine(Pens.Green, x, y - size, x, y + size);
                    if(i > 0)
                        e.Graphics.DrawLine(Pens.Blue, xp, yp, x, y);
                    xp = x;
                    yp = y;
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void pDrawPanel_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                pDrawPanel.Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cbXType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        int SelectedProb = -1;
        private void lbMeasuringList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedProb = lbMeasuringList.SelectedIndex;
                CheckSelection();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbMeasuringList.SelectedIndex < 0)
                    return;
                int i = lbMeasuringList.SelectedIndex;
                History.RemoveAt(Probs[i]);
                Init(History);
                if (lbMeasuringList.Items.Count > i)
                    lbMeasuringList.SelectedItem = i;
                else
                    if (i > 0)
                        lbMeasuringList.SelectedItem = i-1;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
