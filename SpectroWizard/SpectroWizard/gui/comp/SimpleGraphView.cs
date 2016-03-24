using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.gui.comp
{
    public partial class SimpleGraphView : UserControl
    {
        public SimpleGraphView()
        {
            InitializeComponent();
        }

        class Line
        {
            public float X1, Y1, X2, Y2;
            public Line(float x1, float y1, float x2, float y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }
        }
        List<Line> Lines = new List<Line>();
        List<Pen> LinesPen = new List<Pen>();
        List<PointF> Points = new List<PointF>();
        List<Pen> PointsPen = new List<Pen>();
        float MinX = float.MaxValue, MaxX = -float.MaxValue,
            MinY = float.MaxValue, MaxY = -float.MaxValue;
        public void Reset()
        {
            Lines.Clear();
            LinesPen.Clear();
            Points.Clear();
            PointsPen.Clear();

            MinX = float.MaxValue;
            MinY = float.MaxValue;
            MaxX = -float.MaxValue;
            MaxY = -float.MaxValue;
        }

        public void AddLine(Pen pen,float x1, float y1, float x2, float y2)
        {
            Lines.Add(new Line(x1, y1, x2, y2));
            LinesPen.Add(pen);
            CheckPoint(x1, y1);
            CheckPoint(x2, y2);
        }

        void CheckPoint(float x, float y)
        {
            if (MinX > x)
                MinX = x;
            if (MinY > y)
                MinY = y;
            if (MaxX < x)
                MaxX = x;
            if (MaxY < y)
                MaxY = y;
        }

        public void AddPoint(Pen pen, float x, float y)
        {
            Points.Add(new PointF(x, y));
            PointsPen.Add(pen);
            CheckPoint(x, y);
        }

        private void SimpleGraphView_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.FillRectangle(Brushes.White,0,0,Width,Height);

                if (MinX == double.MaxValue || MinX == MaxX ||
                    MinY == MaxY)
                    return;

                float dlt = (MaxX - MinX) / 20;
                float kx = Width / (MaxX - MinX+dlt);
                dlt = (MaxY - MinY) / 20;
                float ky = Height / (MaxY - MinY+dlt);

                int x0 = 5;
                int y0 = -5;
                double[] xvals = serv.GetGoodValues(MinX, MaxX, Width / 60);
                if (xvals != null)
                {
                    for (int i = 0; i < xvals.Length; i++)
                    {
                        int x = (int)((xvals[i] - MinX) * kx)+x0;
                        e.Graphics.DrawLine(Pens.LightGray, x, 0, x, Height);
                        e.Graphics.DrawString(serv.GetGoodValue(xvals[i],2),
                            Common.GraphLitleFont, Brushes.Gray, x, 5);
                    }
                }

                double[] yvals = serv.GetGoodValues(MinY, MaxY, Height / 50);
                if (yvals != null)
                {
                    for (int i = 0; i < yvals.Length; i++)
                    {
                        int y = Height - (int)((yvals[i] - MinY) * ky)+y0;
                        e.Graphics.DrawLine(Pens.LightGray, 0, y, Width, y);
                        e.Graphics.DrawString(serv.GetGoodValue(yvals[i], 2),
                            Common.GraphLitleFont, Brushes.Gray, 5, y);
                    }
                }

                for (int i = 0; i < Lines.Count; i++)
                {
                    int x1 = (int)((Lines[i].X1 - MinX) * kx) + x0;
                    int x2 = (int)((Lines[i].X2 - MinX) * kx) + x0;
                    int y1 = Height - (int)((Lines[i].Y1 - MinY) * ky) + y0;
                    int y2 = Height - (int)((Lines[i].Y2 - MinY) * ky) + y0;
                    e.Graphics.DrawLine(LinesPen[i],x1, y1, x2, y2);
                }

                int ps = 3;
                for (int i = 0; i < Points.Count; i++)
                {
                    int x1 = (int)((Points[i].X - MinX) * kx) + x0;
                    int y1 = Height - (int)((Points[i].Y - MinY) * ky) + y0;
                    e.Graphics.DrawLine(PointsPen[i], x1-ps, y1, x1+ps, y1);
                    e.Graphics.DrawLine(PointsPen[i], x1, y1 - ps, x1, y1 + ps);
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
    }
}
