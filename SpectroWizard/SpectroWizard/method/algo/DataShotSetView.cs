using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectroWizard.method.algo
{
    public partial class DataShotSetView : UserControl
    {
        public DataShotSetView()
        {
            InitializeComponent();
        }

        MethodSimple Method;
        List<DataShot> Data;
        const int WindowSize = 40;
        float MinValue, MaxValue;
        List<Point> points = new List<Point>();
        public void update(MethodSimple method,string element,int formula,double ly,bool isConDlt,bool isRel)
        {
            //points = new List<Point>();
            points.Clear();
            Method = method;
            if (Method == null)
            {
                Data = null;
            }
            else
            {
                Data = DataShotExtractor.extract(method, element, formula, ly, WindowSize, isConDlt, 0, 20000, isRel);
                if (Data == null || Data.Count == 0)
                    Data = null;
                else
                {
                    MinValue = float.MaxValue;
                    MaxValue = -float.MaxValue;
                    for (int i = 0; i < Data.Count; i++)
                        Data[i].checkMinMax(ref MinValue, ref MaxValue);
                }
            }
            Invalidate();
        }

        private void DataShotSetView_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.FillRectangle(Brushes.White,0,0,Size.Width,Size.Height);
                if (Data == null)
                    return;
                int xl = WindowSize * 2 + 1;
                double kx = Size.Width / (double)xl;
                double ky = Size.Height / (MaxValue - MinValue);
                int x0 = (int)(WindowSize*kx);
                Pen pen;
                if (Common.Dev.Reg.GetMaxLinarValue() < MaxValue)
                    pen = Pens.Red;
                else
                    pen = Pens.Blue;

                if (points.Count == 0)
                {
                    for (int d = 0; d < Data.Count; d++)
                    {
                        DataShot ds = Data[d];
                        int prevX = 0;
                        //int prevY = Size.Height - (int)((ds.Data[0]-MinValue)*ky);
                        for (int i = 1; i < ds.Data.Length; i++)
                        {
                            int x = (int)(i * kx);
                            int y = Size.Height - (int)((ds.Data[i] - MinValue) * ky);
                            //g.DrawLine(pen, prevX, prevY, x, y);
                            prevX = x;
                            //prevY = y;
                            points.Add(new Point(x, y));
                        }
                        points.Add(new Point(prevX + Size.Height, -Size.Height));
                        points.Add(new Point(-10000, 0));
                    }
                }
                g.DrawLines(pen, points.ToArray());

                g.DrawLine(Pens.Red, x0, 20, x0, Size.Height);
                g.DrawString("" + MaxValue, DefaultFont, Brushes.Blue, 10, 10);
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }
    }
}
