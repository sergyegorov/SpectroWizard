using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.util
{
    public partial class GraphPreview : Form
    {
        public GraphPreview()
        {
            InitializeComponent();
        }

        public int NoiseCenselationLevel
        {
            get
            {
                return (int)numNoiseCenseletionLevel.Value;
            }
        }

        bool IsOk;
        float[] Data;
        float MinData, MaxData;
        public bool check(Form form,float[] data)
        {
            IsOk = false;
            Data = data;
            MinData = Data[0];
            MaxData = Data[0];
            for (int i = 1; i < Data.Length; i++)
            {
                if (MinData > Data[i])
                    MinData = Data[i];
                if (MaxData < Data[i])
                    MaxData = Data[i];
            }
            float d = (MaxData - MinData)/10;
            MinData -= d;
            MaxData += d;
            this.ShowDialog(form);
            return IsOk;
        }

        private void pPreview_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.FillRectangle(Brushes.White, 0, 0, pPreview.Width, pPreview.Height);
                if (Data == null)
                    return;
                double kx = pPreview.Width / (double)Data.Length;
                double ky = pPreview.Height / (MaxData - MinData);
                int xp = 0;
                int yp = (int)(pPreview.Height - ky * (Data[0] - MinData));
                for (int i = 0; i < Data.Length; i++)
                {
                    int x = (int)(i * kx);
                    int y = (int)(pPreview.Height - ky * (Data[i] - MinData));
                    g.DrawLine(Pens.Blue, xp, yp, x, yp);
                    g.DrawLine(Pens.Blue, x, yp, x, y);
                    xp = x;
                    yp = y;
                }
                g.DrawLine(Pens.Red, pPreview.Width / 2, 0, pPreview.Width / 2, pPreview.Height);
            }
            catch (Exception ex)
            {
                Log.OutNoMsg(ex);
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            try
            {
                IsOk = true;
                Hide();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
