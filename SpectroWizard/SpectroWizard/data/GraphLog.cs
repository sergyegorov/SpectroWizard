using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.IO;
using SpectroWizard.util;

namespace SpectroWizard.data
{
    public partial class GraphLog : UserControl
    {
        public const string MLSConst = "GrLog";

        DoubleBufferedGraphics DBGr;
        public GraphLog()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            DBGr = new DoubleBufferedGraphics(SpectroWizard.gui.MainForm.MForm, 
                DrawPanel, new PaintDel(DrawPanel_Paint));
        }

        public void ReDraw()
        {
            if(DBGr != null)
                DBGr.ReDraw();
        }

        public bool ShowTxtChb
        {
            get
            {
                return chbTextLogShow.Visible;
            }
            set
            {
                chbTextLogShow.Visible = value;
            }
        }

        bool ShowPalitraPriv = false;
        public bool ShowPalitra
        {
            get
            {
                return ShowPalitraPriv;
            }
            set
            {
                ShowPalitraPriv = value;
            }
        }

        bool ShowSumDefaultValuePriv = false;
        public bool ShowSumDefaultValue
        {
            get
            {
                return ShowSumDefaultValuePriv;
            }
            set
            {
                ShowSumDefaultValuePriv = value;
                //chbSum.Checked = value;
                if(value)
                    chbSum.SelectedIndex = (int)GLogRecord.DrawSumType.Sum;
                else
                    chbSum.SelectedIndex = (int)GLogRecord.DrawSumType.Full;
            }
        }

        public void setSumAsProbSums()
        {
            chbSum.SelectedIndex = (int)GLogRecord.DrawSumType.MultySum;
        }

        public bool ShowSumCheckBox
        {
            get
            {
                return chbSum.Visible;
            }
            set
            {
                chbSum.Visible = value;
            }
        }

        class WindowInfo
        {
            public string Name;
            public double Width;

            public double GFrom,GWidht;
            public WindowInfo(string name, double width)
            {
                Name = name;
                Width = width;
            }
        }

        List<WindowInfo> DrawWindows = new List<WindowInfo>();
        Hashtable DrawWindowIndex = new Hashtable();

        List<GLogRecord> DrawData = new List<GLogRecord>();
        List<GLogMsg> DrawLogTxt = new List<GLogMsg>();

        public void Setup(List<GLogRecord> data, string section_nam)
        {
            DrawData.Clear();
            DrawWindows.Clear();
            DrawWindowIndex.Clear();
            DrawLogTxt.Clear();
            if (data == null)
                return;
            for (int i = 0; i < data.Count; i++)
            {
                if(data[i].GetSectionName().Equals(section_nam) == false)
                    continue;
                GLogRecord spd = (GLogRecord)data[i];
                string vname = spd.GetViewName();
                if (vname != null)
                {
                    if (DrawWindowIndex[vname] == null)
                    {
                        DrawWindowIndex.Add(vname, DrawWindows.Count);
                        DrawWindows.Add(new WindowInfo(vname, spd.GetDataWidth()));
                    }
                }
                chbTextLogShow.Visible = false;
                if (data[i].GetRecordType() == GLogDataTypes.LogMsg)
                {
                    DrawLogTxt.Add((GLogMsg)data[i]);
                    chbTextLogShow.Visible = true;
                }
                else
                    DrawData.Add(data[i]);
            }
            //DrawPanel.Refresh();
            DBGr.ReDraw();
        }

        List<double> CrossLyFrom = new List<double>(), 
            CrossLyTo = new List<double>(), 
            CrossY = new List<double>(),
            CrossZ = new List<double>();
        public void SetupCross(double ly_from,double ly_to,double y,double z)
        {
            CrossLyFrom.Add(ly_from);
            CrossLyTo.Add(ly_to);
            CrossY.Add(y);
            CrossZ.Add(z);
            DBGr.ReDraw();
        }

        public void ClearCross()
        {
            CrossLyFrom.Clear();
            CrossLyTo.Clear();
            CrossY.Clear();
            CrossZ.Clear();
            DBGr.ReDraw();
        }

        public void ResetCross()
        {
            ClearCross();
            DBGr.ReDraw();//DrawPanel.Refresh();
        }

        public int SelectedId1, SelectedId2;
        public void SelectePointId(int id1, int id2)
        {
            SelectedId1 = id1;
            SelectedId2 = id2;
            DBGr.ReDraw();
        }

        public int DrawCount, CurrentDrawIndex;
        public List<GLogRecord.ActivePoint> ActivePoints = new List<GLogRecord.ActivePoint>();
        private void DrawPanel_Paint(Graphics g)
        {
            try
            {
                g.FillRectangle(Brushes.White, 0, 0,
                    DrawPanel.Width, DrawPanel.Height);

                ActivePoints.Clear();

                if (DrawData.Count == 0 && DrawLogTxt.Count == 0)
                {
                    string msg;
                    if (Common.MLS != null)
                        msg = Common.MLS.Get(MLSConst, "No Data");
                    else
                        msg = "No Data";
                    SizeF s = g.MeasureString(msg, Common.GraphBigFont);
                    g.DrawString(msg, 
                        Common.GraphBigFont, Brushes.LightGray, 
                        DrawPanel.Width/2-s.Width/2, 
                        DrawPanel.Height/2-s.Height/2);
                    return;
                }

                RectangleF tmp_s = new RectangleF();
                if (DrawData.Count != 0)
                {
                    double sum_width = 0;
                    for (int i = 0; i < DrawWindows.Count; i++)
                        sum_width += DrawWindows[i].Width;

                    double k = DrawPanel.Width / (double)sum_width;
                    sum_width = 0;
                    for (int i = 0; i < DrawWindows.Count; i++)
                    {
                        DrawWindows[i].GFrom = sum_width;
                        DrawWindows[i].GWidht = (int)(DrawWindows[i].Width * k);
                        sum_width += DrawWindows[i].GWidht;
                        g.DrawRectangle(Pens.Black, (int)DrawWindows[i].GFrom, 0,
                            (int)DrawWindows[i].GWidht, DrawPanel.Height);
                    }

                    bool is_the_same = true;
                    GLogDataTypes type = DrawData[0].GetRecordType();
                    for (int i = 1; i < DrawData.Count; i++)
                    {
                        if (DrawData[i].GetRecordType() != type)
                            is_the_same = false;
                    }

                    if (is_the_same)
                    {
                        int[] draw_index = new int[DrawWindowIndex.Count];
                        int[] draw_index_max = new int[DrawWindowIndex.Count];
                        for (int i = DrawData.Count - 1; i >= 0; i--)
                        {
                            GLogRecord spd = (GLogRecord)DrawData[i];
                            int w_index = (int)DrawWindowIndex[spd.GetViewName()];
                            draw_index_max[w_index]++;
                        }
                        RectangleF size = new RectangleF();
                        if (CrossLyFrom.Count > 0)
                        {
                            double ly_from = double.MaxValue;
                            double ly_to = -double.MaxValue;
                            double y_from = double.MaxValue;
                            double y_to = -double.MaxValue;
                            for (int i = 0; i < CrossLyFrom.Count; i++)
                            {
                                if (ly_from > CrossLyFrom[i])
                                    ly_from = CrossLyFrom[i];
                                if (ly_from > CrossLyTo[i])
                                    ly_from = CrossLyTo[i];
                                if (ly_to < CrossLyFrom[i])
                                    ly_to = CrossLyFrom[i];
                                if (ly_to < CrossLyTo[i])
                                    ly_to = CrossLyTo[i];
                                if (y_from > CrossY[i])
                                    y_from = CrossY[i];
                                if (y_to < CrossY[i])
                                    y_to = CrossY[i];
                            }
                            size.X = (float)ly_from;
                            size.Y = (float)y_from;
                            size.Width = (float)(ly_to - ly_from);
                            size.Height = (float)(y_to - y_from);
                        }
                        for (int i = 0; i < DrawData.Count; i++)
                            DrawData[i].GetMinMaxVal(ref size);
                        if (size.Height == 0)
                        {
                            size.Y -= 0.5F;
                            size.Height++;
                        }
                        float dlt = size.Height;
                        dlt /= 30;
                        if (dlt < 0.00001)
                            dlt = 0.00001F;
                        size.Height += dlt*2;
                        size.Y -= dlt;
                        tmp_s = new RectangleF(size.X, size.Y, size.Width, size.Height);
                        DrawCount = DrawData.Count; 
                        for (int i = DrawData.Count - 1; i >= 0; i--)
                        {
                            CurrentDrawIndex = i;
                            GLogRecord spd = DrawData[i];
                            string vname = spd.GetViewName();
                            int w_index = (int)DrawWindowIndex[vname];
                            DrawData[i].Draw(g,
                                    new Rectangle((int)DrawWindows[w_index].GFrom,
                                        0,
                                        (int)DrawWindows[w_index].GWidht,
                                        DrawPanel.Height),
                                    tmp_s,
                                    draw_index_max[w_index]-draw_index[w_index]-1,
                                    draw_index_max[w_index]-1,
                                    (GLogRecord.DrawSumType)chbSum.SelectedIndex,
                                    this);
                            draw_index[w_index]++;
                            DrawData[i].DrawCrosses(g,CrossLyFrom, CrossLyTo, CrossY, CrossZ);
                        }
                    }
                    else
                    {
                        DrawCount = DrawData.Count; 
                        for (int i = DrawData.Count - 1; i >= 0; i--)
                        {
                            CurrentDrawIndex = i;
                            GLogSpData spd = (GLogSpData)DrawData[i];
                            int w_index = (int)DrawWindowIndex[spd.ViewName];
                            DrawData[i].Draw(g,
                                    new Rectangle((int)DrawWindows[w_index].GFrom,
                                        0,
                                        (int)DrawWindows[w_index].GWidht,
                                        DrawPanel.Height),
                                    tmp_s,
                                    0,
                                    0, 
                                    (GLogRecord.DrawSumType)chbSum.SelectedIndex, 
                                    this);
                        }
                    }

                    for (int i = 0; i < DrawWindows.Count; i++)
                    {
                        g.DrawRectangle(Pens.Black, (int)DrawWindows[i].GFrom, 0,
                            (int)DrawWindows[i].GWidht, DrawPanel.Height);
                        g.DrawString(DrawWindows[i].Name,
                            Common.GraphNormalFont, Brushes.Black,
                            (int)DrawWindows[i].GFrom + 1, 1);
                    }
                }

                if (chbTextLogShow.Checked)
                {
                    int step = (int)Common.GraphNormalFont.SizeInPoints * 4 / 3;
                    int h = (int)1 + step;
                    for (int i = 0; i < DrawLogTxt.Count; i++)
                    {
                        DrawLogTxt[i].Draw(g, new Rectangle(3, h, 100, 15), tmp_s, 0, 0, 
                            (GLogRecord.DrawSumType)chbSum.SelectedIndex,//chbSum.Checked, 
                            this);
                        h += DrawLogTxt[i].GetMinHeight();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
                try
                {
                    g.DrawString("Ошибка прорисовки...", Common.GraphNormalFont, Brushes.Red, 50, 50);
                }
                catch
                {
                }
            }
        }

        private void DrawPanel_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbTextLogShow_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbSum_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        bool IsClick = false;
        private void DrawPanel_Click(object sender, EventArgs e)
        {
            try
            {
                IsClick = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public delegate void SelectActivePointListener(int id1,int id2);

        public SelectActivePointListener SelectingActivePoint = null;
        //int SelectedActivePoint = -1;
        private void DrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    CM.Show(this, e.X, e.Y);
                    return;
                }
                if (IsClick == false || SelectingActivePoint == null)
                    return;
                IsClick = false;
                int len = int.MaxValue;
                int con = -1;
                for (int i = 0; i < ActivePoints.Count; i++)
                {
                    int dx = e.X - ActivePoints[i].X;
                    int dy = e.Y - ActivePoints[i].Y;
                    int l = (int)Math.Sqrt(dx * dx + dy * dy);
                    if (l > 10)
                        continue;
                    if (len > l)
                    {
                        con = i;
                        len = l;
                    }
                }
                if(con >= 0)
                    SelectingActivePoint(ActivePoints[con].PointId1, ActivePoints[con].PointId2);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    public enum GLogDataTypes
    {
        Null,
        LogMsg,
        SpData,
        Gr2Data,
        Gr3Data
    }

    public abstract class GLogRecord
    {
        public const string MLSConst = "GLogRecord";
        static Pen[] GreenPal = null;
        static Pen[] RedPal = null;
        static Pen[] GrayPal = null;

        protected Pen GetGreenPen(int i, int max)
        {
            return GreenPal[i * (GreenPal.Length-1) / max];
        }

        protected Pen GetGrayPen(int i, int max)
        {
            return GrayPal[i * (GreenPal.Length - 1) / max];
        }

        protected Pen GetRedPen(int i, int max)
        {
            return RedPal[i * (RedPal.Length - 1) / max];
        }

        GLogDataTypes Type = GLogDataTypes.Null;
        protected string SectionName;

        public GLogDataTypes GetRecordType()
        {
            return Type;
        }

        public string GetSectionName()
        {
            return SectionName;
        }

        public GLogRecord(GLogDataTypes type, string section_name)
        {
            SectionName = section_name;
            Type = type;
            if(GreenPal == null)
            {
                GreenPal = new Pen[128];
                RedPal = new Pen[128];
                GrayPal = new Pen[128];
                for (int i = 0; i < GreenPal.Length; i++)
                {
                    Color c = Color.FromArgb(i, 255 - i, 0);
                    GreenPal[i] = new Pen(c);
                    c = Color.FromArgb(255 - i*2, 0, i*2);
                    RedPal[i] = new Pen(c);
                    c = Color.FromArgb(255 - i, 255 - i, 255 - i);
                    GrayPal[i] = new Pen(c);
                } 
            }
        }

        static public GLogRecord Load(BinaryReader br)
        {
            GLogRecord ret = null;
            char type = (char)br.ReadByte();
            if (type != 'g')
                throw new Exception("GLogRecord.Load: Wrong prefix.");

            int ver = br.ReadByte();
            if(ver != 0)
                throw new Exception("GLogRecord.Load: Unsupported version.");

            GLogDataTypes glt = (GLogDataTypes)br.ReadByte();
            string section_name = br.ReadString();
            switch (glt)
            {
                case GLogDataTypes.Null: ret = null; break;
                case GLogDataTypes.LogMsg: ret = new GLogMsg(br,section_name); break;
                case GLogDataTypes.SpData: ret = new GLogSpData(br, section_name); break;
                case GLogDataTypes.Gr2Data: ret = new GLogGr2Data(br, section_name); break;
                case GLogDataTypes.Gr3Data: ret = new GLogGr3Data(br, section_name); break;
                default:
                    throw new Exception("GLogRecord.Load: Unsupported data type.");
            }

            ret.LoadProc(br);

            ver = br.ReadByte();
            if (ver != 0)
                throw new Exception("GLogRecord.Load: Unexpected final.");

            return ret;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)'g');
            bw.Write((byte)0);
            bw.Write((byte)Type);
            bw.Write(SectionName);

            SaveProc(bw);

            bw.Write((byte)0);
        }

        protected Color GetColor(Color col, int level, int max_level)
        {
            if (max_level == 0)
                return col;

            int[] c = {col.R,col.G,col.B};

            for(int i = 0;i<c.Length;i++)
            {
                if (c[i] < 170)
                    c[i] = c[i] + (170-c[i]) * level / max_level;
                /*else
                {
                    if (c[i] > 150)
                        c[i] = c[i] - 100 * level / max_level;
                }*/
            }
            Color rc = Color.FromArgb(c[0], c[1], c[2]);
            return rc;
        }

        public class ActivePoint
        {
            public int X, Y, PointId1, PointId2;
            public ActivePoint(int x, int y, int id1, int id2)
            {
                X = x;
                Y = y;
                PointId1 = id1;
                PointId2 = id2;
            }
        }

        public enum DrawSumType
        {
            Full,
            Sum,
            MultySum
        }

        abstract public void LoadProc(BinaryReader br);
        abstract public void SaveProc(BinaryWriter bw);
        abstract public void GetMinMaxVal(ref RectangleF size);
        abstract public void Draw(Graphics g,Rectangle client,RectangleF y_size,
            int level,int level_max,
            DrawSumType draw_sum,
            GraphLog master);
        //abstract public int ConvertXToPaint(double x);
        //abstract public int ConvertYToPaint(double y);
        abstract public void DrawCrosses(Graphics g, List<double> ly_from,
            List<double> ly_to,List<double> y,List<double> z);
        abstract public int GetMinHeight();
        abstract public string GetViewName();
        abstract public double GetDataWidth();

        public Color LoadColor(BinaryReader br)
        {
            return Color.FromArgb(br.ReadByte(),
                br.ReadByte(),
                br.ReadByte(),
                br.ReadByte());
        }

        public void SaveColor(BinaryWriter bw, Color col)
        {
            bw.Write(col.A);
            bw.Write(col.R);
            bw.Write(col.G);
            bw.Write(col.B);
        }
    }

    public class GLogGr3Data : GLogRecord
    {
        public double[,] XVals;
        public double[,] YVals;
        public double[,] ZVals;
        public double[] XPoints;
        public double[] YPoints;
        public double[] ZPoints;
        public int[] PointId1;
        public int[] PointId2;
        public bool[] PointEnabled;
        public Color ColSp;
        public Color ColMark;
        public string ViewName;

        override public string GetViewName()
        {
            return ViewName;
        }

        double Kx, Ky, Kz, X0, Y0, Z0;
        double MaxZ;
        int PanelHeight;
        void ConvertXYZ(double x, double y, double z, out int xg, out int yg)
        {
            int dx = (int)((z - Z0) * Kz);
            int dy = dx;
            xg = 5 + (int)((x - X0) * Kx) + dx;
            yg = (int)(PanelHeight - (y - Y0) * Ky) - dy - 5;
        }

        public override void DrawCrosses(Graphics g, List<double> ly_from,
            List<double> ly_to, List<double> yi, List<double> zi)
        {
            int size = 3;
            for (int i = 0; i < ly_from.Count; i++)
            {
                int x1, y1, x2, y2;
                ConvertXYZ(ly_from[i], yi[i], zi[i], out x1, out y1);
                ConvertXYZ(ly_to[i], yi[i], zi[i], out x2, out y2);
                g.DrawLine(Pens.Green, x1, y1, x2, y2);
                g.DrawLine(Pens.Green, x1 - size, y1 + size, x1 + size, y1 - size);
                g.DrawLine(Pens.Green, x1 - size, y1 - size, x1 + size, y1 + size);
            }
        }

        public override void Draw(Graphics g, Rectangle client, RectangleF y_size,
            int level, int level_max,
            DrawSumType draw_sum,
            GraphLog master)
        {
            double xmin, xmax, ymin;
            double ymax, zmin, zmax;
            
            GetMinMaxVals(out xmin, out ymin, out zmin, out xmax, out ymax, out zmax);
            PanelHeight = client.Height;

            int z_fild_size = client.Width / 10;

            X0 = xmin;
            Y0 = ymin;
            Z0 = zmin;
            MaxZ = zmax;

            Kx = (client.Width - z_fild_size - 20) / (xmax - xmin);
            Ky = (client.Height - z_fild_size - 20) / (ymax - ymin);
            Kz = (z_fild_size + 10) / (zmax - zmin);

            DrawGreed(g, xmin, xmax, ymin, ymax, zmin, zmax);
            DrawCut(g, xmin, xmax, ymin, ymax, zmin, zmax, master.SelectedId1, master.SelectedId2);
            DrawSf(g);
            DrawPoints(g,master.SelectedId1,master.SelectedId2);
        }

        void DrawCut(Graphics g,
            double xmin, double xmax,
            double ymin, double ymax,
            double zmin, double zmax,
            int selected_id1, int selected_id2)
        {
            double xv = 0;
            for (int i = 0; i < XPoints.Length; i++)
            {
                int x, y;
                int size = 2;
                if (PointId1[i] != selected_id1)
                    continue;
                xv = XPoints[i];
                ConvertXYZ(xmin, YPoints[i], ZPoints[i], out x, out y);
                Pen p;
                // SelectedId1 SelectedId2
                if (PointId1[i] == selected_id1 && PointId2[i] == selected_id2)
                {
                    p = new Pen(GetColor(Color.LightGreen, ZPoints[i]));
                    size = 10;
                }
                else
                    p = new Pen(GetColor(Color.Pink, ZPoints[i]));
                if (PointEnabled[i])
                {
                    g.DrawLine(p, x - size + 1, y + 1, x + size - 1, y - 1);
                    g.DrawLine(p, x, y - size, x, y + size);
                }
                else
                {
                    p = new Pen(GetColor(Color.LightGray, ZPoints[i]));
                    g.DrawEllipse(p, x - size+1, y - size, size * 2 - 1, size * 2 + 1);
                }
            }
            double dist = double.MaxValue;
            int dist_inded = 0;
            for (int i = 0; i < XVals.GetLength(0); i++)
            {
                double tmp = Math.Abs(XVals[i,0] - xv);
                if (tmp < dist)
                {
                    dist = tmp;
                    dist_inded = i;
                }
            }

            int xlen = XVals.GetLength(1);
            for (int i = 1; i < xlen; i++)
            {
                int x1, y1;
                int x2, y2;
                ConvertXYZ(xmin, YVals[dist_inded, i-1], ZVals[dist_inded, i-1], out x1, out y1);
                ConvertXYZ(xmin, YVals[dist_inded, i], ZVals[dist_inded, i], out x2, out y2);
                g.DrawLine(Pens.LightBlue, x1, y1, x2, y2);
            }
        }

        void DrawGreed(Graphics g,
            double xmin, double xmax,
            double ymin, double ymax, 
            double zmin, double zmax)
        {
            int x1, y1;
            int x2, y2;
            ConvertXYZ(xmin, ymin, zmin, out x1, out y1);
            ConvertXYZ(xmax, ymin, zmin, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
            ConvertXYZ(xmin, ymax, zmin, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
            ConvertXYZ(xmin, ymin, zmax, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);

            ConvertXYZ(xmin, ymax, zmax, out x1, out y1);
            ConvertXYZ(xmin, ymin, zmax, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
            ConvertXYZ(xmin, ymax, zmin, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
            ConvertXYZ(xmax, ymax, zmax, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);

            ConvertXYZ(xmax, ymin, zmax, out x1, out y1);
            ConvertXYZ(xmin, ymin, zmax, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
            ConvertXYZ(xmax, ymax, zmax, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
            ConvertXYZ(xmax, ymin, zmin, out x2, out y2);
            g.DrawLine(Pens.Black, x1, y1, x2, y2);

            /*double[] xvals = serv.GetGoodValues(xmin,xmax,10);
            double[] yvals = serv.GetGoodValues(ymin, ymax, 10);
            double[] zvals = serv.GetGoodValues(zmin, zmax, 5);
            if (xvals == null || yvals == null || zvals == null)
                return;
            //XY
            double z = zvals[zvals.Length - 1];
            for (int i = 0; i < xvals.Length; i++)
            {
                ConvertXYZ(xvals[i], yvals[0], z, out x1, out y1);
                ConvertXYZ(xvals[i], yvals[yvals.Length - 1], z, out x2, out y2);
                g.DrawLine(Pens.LightGray, x1, y1, x2, y2);
            }
            for (int i = 0; i < yvals.Length; i++)
            {
                ConvertXYZ(xvals[0], yvals[i], z, out x1, out y1);
                ConvertXYZ(xvals[yvals.Length - 1], yvals[i], z, out x2, out y2);
                g.DrawLine(Pens.LightGray, x1, y1, x2, y2);
            }

            //ZY
            double x = xvals[0];
            for (int i = 0; i < zvals.Length; i++)
            {
                ConvertXYZ(x, yvals[0], zvals[i], out x1, out y1);
                ConvertXYZ(x, yvals[yvals.Length - 1], zvals[i], out x2, out y2);
                g.DrawLine(Pens.LightGray, x1, y1, x2, y2);
            }
            for (int i = 0; i < yvals.Length; i++)
            {
                ConvertXYZ(x, yvals[i], zvals[0], out x1, out y1);
                ConvertXYZ(x, yvals[i], zvals[zvals.Length - 1], out x2, out y2);
                g.DrawLine(Pens.LightGray, x1, y1, x2, y2);
            }//*/
        }

        Color GetColor(Color base_color, double z)
        {
            double k = 0.6 - (z - Z0) * 0.6 / (MaxZ - Z0);
            if (k > 0.6)
                k = 0.6;
            if (k < 0)
                k = 0;
            k += 0.4;
            return Color.FromArgb((int)(base_color.R * k),
                (int)(base_color.G * k),
                (int)(base_color.B * k));
        }

        void DrawPoints(Graphics g,int selected_id1,int selected_id2)
        {
            for (int i = 0; i < XPoints.Length; i++)
            {
                int x, y;
                int size = 3;
                ConvertXYZ(XPoints[i], YPoints[i], ZPoints[i], out x, out y);
                Pen p;
                // SelectedId1 SelectedId2
                if (PointId1[i] == selected_id1 && PointId2[i] == selected_id2)
                {
                    p = new Pen(GetColor(Color.Green, ZPoints[i]));
                    size = 10;
                }
                else
                    p = new Pen(GetColor(Color.Red, ZPoints[i]));
                if (PointEnabled[i])
                {
                    g.DrawLine(p, x - size, y, x + size, y);
                    g.DrawLine(p, x, y - size, x, y + size);
                }
                else
                {
                    p = new Pen(GetColor(Color.Gray, ZPoints[i]));
                    g.DrawEllipse(p, x - size, y - size, size * 2 + 1, size * 2 + 1);
                }
            }
        }

        void DrawSf(Graphics g)
        {
            for (int i = 1; i < XVals.GetLength(0); i++)
            {
                for (int j = 1; j < XVals.GetLength(1); j++)
                {
                    int x1, y1;
                    int x2, y2;
                    int x3, y3;
                    Pen p = new Pen(GetColor(Color.LightBlue, ZVals[i - 1, j - 1]));
                    ConvertXYZ(XVals[i-1, j-1], YVals[i-1, j-1], ZVals[i-1, j-1], out x1, out y1);
                    ConvertXYZ(XVals[i, j - 1], YVals[i, j - 1], ZVals[i, j - 1], out x2, out y2);
                    ConvertXYZ(XVals[i, j], YVals[i, j], ZVals[i, j], out x3, out y3);
                    g.DrawLine(p, x1, y1, x2, y2);
                    g.DrawLine(p, x2, y2, x3, y3);
                }
            }
            for (int i = 0; i < XVals.GetLength(0); i++)
            {
                for (int j = 0; j < XVals.GetLength(1); j++)
                {
                    int x, y;
                    int size = 1;
                    ConvertXYZ(XVals[i, j], YVals[i, j], ZVals[i, j], out x, out y);
                    Pen p = new Pen(GetColor(Color.Blue, ZVals[i, j]));
                    g.DrawLine(p, x - size, y, x + size, y);
                    g.DrawLine(p, x, y - size, x, y + size);
                }
            }
        }

        override public double GetDataWidth()
        {
            return XVals[XVals.GetLength(0) - 1, XVals.GetLength(1) - 1] - XVals[0,0];
        }

        public override int GetMinHeight()
        {
            return 0;
        }

        public override void GetMinMaxVal(ref RectangleF size)//(ref double min, ref double max)
        {
            double minx;
            double maxx;
            double miny;
            double maxy;
            if (size.Width == 0 && size.Height == 0)
            {
                minx = double.MaxValue;
                maxx = -double.MaxValue;
                miny = double.MaxValue;
                maxy = -double.MaxValue;
            }
            else
            {
                minx = size.X;
                miny = size.Y;
                maxx = minx + size.Width;
                maxy = miny + size.Height;
            }

            for (int i = 0; i < XVals.GetLength(0); i++)
                for (int j = 0; j < XVals.GetLength(1); j++)
            {
                if (XVals[i,j] < minx)
                    minx = XVals[i, j];
                if (XVals[i, j] > maxx)
                    maxx = XVals[i, j];
                if (YVals[i, j] < miny)
                    miny = YVals[i, j];
                if (YVals[i, j] > maxy)
                    maxy = YVals[i, j];
            }

            for (int i = 0; i < XPoints.Length; i++)
            {
                if (XPoints[i] < minx)
                    minx = XPoints[i];
                if (XPoints[i] > maxx)
                    maxx = XPoints[i];
                if (YPoints[i] < miny)
                    miny = YPoints[i];
                if (YPoints[i] > maxy)
                    maxy = YPoints[i];
            }

            size.X = (float)minx;
            size.Y = (float)miny;
            size.Width = (float)(maxx - minx);
            size.Height = (float)(maxy - miny);
        }

        public GLogGr3Data(BinaryReader br, string section_name)
            : base(GLogDataTypes.Gr3Data, section_name)
        {
        }

        public GLogGr3Data(string section_name, string view_name,
            double[,] xvals, double[,] yvals,double[,] zvals,
            double[] xpoints, double[] ypoints, double[] zpoints,
            bool[] en_points, int[] point_ids1,
            int[] point_ids2,
            Color color_draw, Color mark)
            : base(GLogDataTypes.Gr3Data, section_name)
        {
            XVals = xvals;
            YVals = yvals;
            ZVals = zvals;
            XPoints = xpoints;
            YPoints = ypoints;
            ZPoints = zpoints;
            PointEnabled = en_points;
            PointId1 = point_ids1;
            PointId2 = point_ids2;
            ViewName = view_name;
            ColSp = color_draw;
            ColMark = mark;
        }

        public override void LoadProc(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver < 1 || ver > 1)
                throw new Exception("Wrong version of GLog3DData");

            ViewName = br.ReadString();
            int count1 = br.ReadInt32();
            int count2 = br.ReadInt32();
            XVals = new double[count1, count2];
            YVals = new double[count1, count2];
            ZVals = new double[count1, count2];
            double xmin = 0, lx = 0;
            double ymin = 0, ly = 0;
            double zmin = 0, lz = 0;
            xmin = br.ReadDouble();
            ymin = br.ReadDouble();
            zmin = br.ReadDouble();
            lx = br.ReadDouble();
            ly = br.ReadDouble();
            lz = br.ReadDouble();
            for (int i = 0; i < XVals.GetLength(0); i++)
            {
                for (int j = 0; j < XVals.GetLength(1); j++)
                {
                    double tmp = br.ReadUInt16();
                    XVals[i, j] = tmp * lx / ushort.MaxValue + xmin;
                    tmp = br.ReadUInt16();
                    YVals[i, j] = tmp * ly / ushort.MaxValue + ymin;
                    tmp = br.ReadUInt16();
                    ZVals[i, j] = tmp * lz / ushort.MaxValue + zmin;
                }
            }

            int count = br.ReadInt32();
            XPoints = new double[count];
            YPoints = new double[count];
            ZPoints = new double[count];
            PointEnabled = new bool[count];
            PointId1 = new int[count];
            PointId2 = new int[count];
            for (int i = 0; i < XPoints.Length; i++)
            {
                double tmp = br.ReadUInt16();
                XPoints[i] = tmp * lx / ushort.MaxValue + xmin;
                tmp = br.ReadUInt16();
                YPoints[i] = tmp * ly / ushort.MaxValue + ymin;
                tmp = br.ReadUInt16();
                ZPoints[i] = tmp * lz / ushort.MaxValue + zmin;
                PointEnabled[i] = br.ReadBoolean();
                PointId1[i] = br.ReadInt32();
                PointId2[i] = br.ReadInt32();
            }

            ColSp = LoadColor(br);
            ColMark = LoadColor(br);

            ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Wrong end of GLog3Data");
        }

        void GetMinMaxVals(out double xmin,
            out double ymin,
            out double zmin,
            out double xmax,
            out double ymax,
            out double zmax)
        {
            xmin = XVals[0, 0];
            xmax = XVals[0, 0];
            ymin = YVals[0, 0];
            ymax = YVals[0, 0];
            zmin = ZVals[0, 0];
            zmax = ZVals[0, 0];
            for (int i = 0; i < XVals.GetLength(0); i++)
            {
                for (int j = 0; j < XVals.GetLength(1); j++)
                {
                    if (xmin > XVals[i, j])
                        xmin = XVals[i, j];
                    if (xmax < XVals[i, j])
                        xmax = XVals[i, j];
                    if (ymin > YVals[i, j])
                        ymin = YVals[i, j];
                    if (ymax < YVals[i, j])
                        ymax = YVals[i, j];
                    if (zmin > ZVals[i, j])
                        zmin = ZVals[i, j];
                    if (zmax < ZVals[i, j])
                        zmax = ZVals[i, j];
                }
            }
            for (int i = 0; i < XPoints.Length; i++)
            {
                if (xmin > XPoints[i])
                    xmin = XPoints[i];
                if (xmax < XPoints[i])
                    xmax = XPoints[i];
                if (ymin > YPoints[i])
                    ymin = YPoints[i];
                if (ymax < YPoints[i])
                    ymax = YPoints[i];
                if (zmin > ZPoints[i])
                    zmin = ZPoints[i];
                if (zmax < ZPoints[i])
                    zmax = ZPoints[i];
            }
        }

        public override void SaveProc(BinaryWriter bw)
        {
            bw.Write((byte)1);

            bw.Write(ViewName);
            bw.Write(XVals.GetLength(0));
            bw.Write(XVals.GetLength(1));

            double xmin,xmax,ymin;
            double ymax,zmin,zmax;
            GetMinMaxVals(out xmin, out ymin, out zmin, out xmax, out ymax, out zmax);

            double lx = xmax - xmin;
            if (lx == 0)
                lx = 1;
            double ly = ymax - ymin;
            if (ly == 0)
                ly = 1;
            double lz = zmax - zmin;
            if (lz == 0)
                lz = 1;
            bw.Write(xmin);
            bw.Write(ymin);
            bw.Write(zmin);
            bw.Write(lx);
            bw.Write(ly);
            bw.Write(lz);
            for (int i = 0; i < XVals.GetLength(0); i++)
                for (int j = 0; j < XVals.GetLength(1); j++)
                {
                    bw.Write((ushort)(((XVals[i, j] - xmin) * ushort.MaxValue) / lx));
                    bw.Write((ushort)(((YVals[i, j] - ymin) * ushort.MaxValue) / ly));
                    bw.Write((ushort)(((ZVals[i, j] - zmin) * ushort.MaxValue) / lz));
                }
            bw.Write(XPoints.Length);
            for (int i = 0; i < XPoints.Length; i++)
            {
                bw.Write((ushort)(((XPoints[i] - xmin) * ushort.MaxValue) / lx));
                bw.Write((ushort)(((YPoints[i] - ymin) * ushort.MaxValue) / ly));
                bw.Write((ushort)(((ZPoints[i] - zmin) * ushort.MaxValue) / lz));
                bw.Write(PointEnabled[i]);
                bw.Write(PointId1[i]);
                bw.Write(PointId2[i]);
            }
            SaveColor(bw, ColSp);
            SaveColor(bw, ColMark);

            bw.Write((byte)1);
        }
    }

    public class GLogGr2Data : GLogRecord
    {
        public double[] XVals;
        public double[] YVals;
        public double[] XPoints;
        public double[] YPoints;
        public int[] PointId1;
        public int[] PointId2;
        public bool[] PointEnabled;
        public Color ColSp;
        public Color ColMark;
        public string ViewName;

        override public string GetViewName()
        {
            return ViewName;
        }

        double Kx, Ky, X0, Y0;
        int PanelHeight;
        //override public 
        int ConvertXToPaint(double x)
        {
            return (int)((x - X0) * Kx);
        }

        //override public 
        int ConvertYToPaint(double y)
        {
            return (int)(PanelHeight - (y - Y0) * Ky);
        }

        public override void DrawCrosses(Graphics g,List<double> ly_from,
                List<double> ly_to, List<double> y, List<double> z)
        {
            for (int ci = 0; ci < y.Count; ci++)
            {
                int cx_f = ConvertXToPaint(ly_from[ci]);
                int cx_t = ConvertXToPaint(ly_to[ci]);
                int cy = ConvertYToPaint(y[ci]);
                int csx = 20;
                int csy = 10;
                bool rev;
                if (cx_f > cx_t)
                {
                    int tmp = cx_f;
                    cx_f = cx_t;
                    cx_t = tmp;
                    rev = true;
                }
                else
                    rev = false;
                Pen p = GetGreenPen(ci, y.Count);
                g.DrawLine(p, cx_f - csx, cy, cx_t + csx, cy);
                if (rev == false)
                    g.DrawLine(p, cx_f, cy - csy, cx_f, cy + csy);
                else
                    g.DrawLine(p, cx_t, cy - csy, cx_t, cy + csy);
            }
        }

        public override void Draw(Graphics g, Rectangle client, RectangleF y_size,
            int level, int level_max,
            DrawSumType draw_sum,
            GraphLog master)
        {
            //y_size = new RectangleF();
            //GetMinMaxVal(ref y_size);
            if (y_size.Width == 0 || y_size.Height == 0)
                return;
            
            /*int[] ind = new int[XPoints.Length];
            int[] max = new int[YPoints.Length];

            for (int i = 0; i < ind.Length; )
            {
                double x = XPoints[i];
                int from = i;
                for (; i < ind.Length && x == XPoints[i]; i++) ;
                for (int j = from; j < i; j++)
                {
                    ind[j] = j - from;
                    max[j] = i - from;
                }
            }*/

            if(master.ShowPalitra)
                for (int i = 0; i < 128; i++)
                {
                    g.DrawLine(GetGreenPen(i, 128), 20 + i, 20, 20 + i, 22);
                    g.DrawLine(GetRedPen(i, 128), 20 + i, 23, 20 + i, 25);
                    g.DrawLine(GetGrayPen(i, 128), 20 + i, 26, 20 + i, 28);
                }

            double kx = client.Width / y_size.Width;
            double ky = client.Height / y_size.Height;

            Kx = kx;
            Ky = ky;
            X0 = y_size.X;
            Y0 = y_size.Y;
            PanelHeight = client.Height;
            if (level == level_max)
            {
                double[] xval = serv.GetGoodValues(y_size.X, y_size.X + y_size.Width, client.Width / 70);
                double[] yval = serv.GetGoodValues(y_size.Y, y_size.Y + y_size.Height, client.Height / 50);
                if (xval != null)
                {
                    for (int i = 0; i < xval.Length; i++)
                    {
                        int x = (int)((xval[i] - y_size.X) * kx);
                        g.DrawLine(Pens.LightGray, x, 0, x, client.Height);
                    }
                }
                if (yval != null)
                {
                    for (int i = 0; i < yval.Length; i++)
                    {
                        int y = (int)(client.Height - (yval[i] - y_size.Y) * ky);
                        g.DrawLine(Pens.LightGray, 0, y, client.Width, y);
                    }
                }
                if (xval != null)
                {
                    for (int i = 0; i < xval.Length; i++)
                    {
                        int x = (int)((xval[i] - y_size.X) * kx);
                        string tval = serv.GetGoodValue(xval[i], 3);
                        if (i < xval.Length / 2)
                            g.DrawString(tval,
                                Common.GraphLitleFont, Brushes.Gray, x, 0);
                        else
                        {
                            int h = (int)g.MeasureString(tval, Common.GraphLitleFont).Height;
                            g.DrawString(tval,
                                Common.GraphLitleFont, Brushes.Gray, x, client.Height-h);
                        }
                    }
                }
                if (yval != null)
                {
                    for (int i = 0; i < yval.Length; i++)
                    {
                        int y = (int)(client.Height - (yval[i] - y_size.Y) * ky);
                        string tval = serv.GetGoodValue(yval[i], 3);
                        if (i >= yval.Length / 2)
                            g.DrawString(tval,
                                Common.GraphLitleFont, Brushes.Gray, 1, y);
                        else
                        {
                            int w = (int)g.MeasureString(tval, Common.GraphLitleFont).Width;
                            g.DrawString(tval,
                                Common.GraphLitleFont, Brushes.Gray, client.Width - w-1,y);
                        }
                    }
                }
            }

            Pen p = new Pen(GetColor(ColSp,level,level_max));
            for (int i = 1; i < XVals.Length; i++)
            {
                int x1 = (int)((XVals[i-1] - y_size.X)*kx);
                int y1 = (int)(client.Height - (YVals[i-1] - y_size.Y)*ky);
                int x2 = (int)((XVals[i] - y_size.X)*kx);
                int y2 = (int)(client.Height - (YVals[i] - y_size.Y)*ky);
                g.DrawLine(p, x1, y1, x2, y2);
            }

            int con_from = 0;
            int con_to = 0;
            //double prev_x = XPoints[0];
            double[] xp = null;
            double[] yp = null;
            bool[] ep = null;
            int[] pid1 = null;
            int[] pid2 = null;
            for (int c = 0; c < XPoints.Length;)
            {
                con_from = c;
                con_to = c;
                for (c++ ; c < XPoints.Length; c++)
                    if (XPoints[c] == XPoints[con_from])
                        con_to = c;
                    else
                        break;

                //if (c == XPoints.Length - 1)
                //    con_to = c;

                if (xp == null || xp.Length != con_to - con_from + 1)
                {
                    xp = new double[con_to - con_from + 1];
                    yp = new double[xp.Length];
                    ep = new bool[xp.Length];
                    pid1 = new int[xp.Length];
                    pid2 = new int[xp.Length];
                }

                for (int i = con_from; i <= con_to; i++)
                {
                    xp[i - con_from] = XPoints[i];
                    yp[i - con_from] = YPoints[i];
                    ep[i - con_from] = PointEnabled[i];
                    pid1[i - con_from] = PointId1[i];
                    pid2[i - con_from] = PointId2[i];
                }

                double ever;// = SpectroWizard.analit.Stat.GetEver(yp, ep);
                double ever1;// = SpectroWizard.analit.Stat.GetEver(yp);
                Pen act_p = new Pen(GetColor(ColMark, level, level_max));
                Pen pas_p = Pens.Gray;
                int size,x,y;
                //int cur_pid;
                //int dfrom, dto;
                ever = SpectroWizard.analit.Stat.GetEver(yp, ep);
                ever1 = SpectroWizard.analit.Stat.GetEver(yp);
                switch (draw_sum)
                {
                    case DrawSumType.Sum:
                        size = 6;
                        x = (int)((xp[0] - y_size.X) * kx);
                        if (IsValid(ever))
                        {
                            y = (int)(client.Height - (ever - y_size.Y) * ky);
                            g.DrawLine(act_p, x - size, y, x + size, y);
                            g.DrawLine(act_p, x, y - size, x, y + size);
                        }
                        else
                        {
                            if (IsValid(ever1))
                            {
                                size /= 2;
                                y = (int)(client.Height - (ever1 - y_size.Y) * ky);
                                g.DrawEllipse(pas_p, x - size, y - size, size * 2 + 1, size * 2 + 1);
                            }
                        }
                        break;
                    case DrawSumType.Full:
                        for (int i = 0; i < xp.Length; i++)
                        {
                            if (IsValid(xp[i]) == false ||
                                IsValid(yp[i]) == false)
                                continue;
                            x = (int)((xp[i] - y_size.X) * kx);
                            y = (int)(client.Height - (yp[i] - y_size.Y) * ky);
                            master.ActivePoints.Add(new ActivePoint(x, y, PointId1[con_from + i], PointId2[con_from + i]));
                            const int size_low = 3;
                            const int size_hi = 5;
                            //int size;
                            if (pid1[i] == master.SelectedId1 &&
                                pid2[i] == master.SelectedId2)
                                size = size_hi;
                            else
                                size = size_low;
                            if (ep[i] == true)//SpectroWizard.analit.Stat.Used[i])
                            {
                                if (SpectroWizard.analit.Stat.Used[i])
                                    p = GetRedPen(i,xp.Length);// act_p;
                                else
                                    p = GetGrayPen(i, xp.Length);// pas_p;
                                if (size == size_low)
                                {
                                    g.DrawLine(p, x - size, y, x + size, y);
                                    g.DrawLine(p, x, y - size, x, y + size);
                                }
                                else
                                {
                                    g.DrawLine(p, x - size, y - size, x + size, y + size);
                                    g.DrawLine(p, x + size, y - size, x - size, y + size);
                                }
                            }
                            else
                                g.DrawEllipse(pas_p, x - 3, y - 3, 7, 7);
                        }
                        break;
                    case DrawSumType.MultySum:
                        for (int i = 0; i < xp.Length; )
                        {
                            int cur_pid = pid2[i];
                            int dfrom = i;
                            int dto = i;
                            bool en_p = false;
                            for (; i < xp.Length && pid2[i] == cur_pid; i++)
                            {
                                dto = i;
                                if (ep[i] == true)
                                    en_p = true;
                            }

                            double[] yy = new double[dto - dfrom + 1];
                            for (int j = dfrom; j <= dto; j++)
                                yy[j - dfrom] = yp[j];

                            ever = SpectroWizard.analit.Stat.GetEver(yy);

                            size = 6;
                            x = (int)((xp[0] - y_size.X) * kx);
                            y = (int)(client.Height - (ever - y_size.Y) * ky);
                            if (IsValid(ever) && en_p)
                            {
                                g.DrawLine(act_p, x - size, y, x + size, y);
                                g.DrawLine(act_p, x, y - size, x, y + size);
                            }
                            else
                            {
                                size /= 2;
                                g.DrawEllipse(pas_p, x - size, y - size, size * 2 + 1, size * 2 + 1);
                            }
                        }
                        break;
                }
            }
        }

        override public double GetDataWidth()
        {
            return XVals[XVals.Length - 1] - XVals[0];
        }

        public override int GetMinHeight()
        {
            return 0;
        }

        Boolean IsValid(double val)
        {
            if (serv.IsValid(val) == false || serv.IsValid(val) == false ||
                    val > 1000000000 || val < -1000000000 ||
                    val > 1000000000 || val < -1000000000)
                return false;
            return true;
        }

        public override void GetMinMaxVal(ref RectangleF size)//(ref double min, ref double max)
        {
            double minx;
            double maxx;
            double miny;
            double maxy;
            if (size.Width == 0 && size.Height == 0)
            {
                minx = double.MaxValue;
                maxx = -double.MaxValue;
                miny = double.MaxValue;
                maxy = -double.MaxValue;
            }
            else
            {
                minx = size.X;
                miny = size.Y;
                maxx = minx + size.Width;
                maxy = miny + size.Height;
            }

            for (int i = 0; i < XVals.Length; i++)
            {
                if (IsValid(XVals[i]) == false || IsValid(YVals[i]) == false)
                    continue;
                if(XVals[i] < minx)
                    minx = XVals[i];
                if (XVals[i] > maxx)
                    maxx = XVals[i];
                if (YVals[i] < miny)
                    miny = YVals[i];
                if (YVals[i] > maxy)
                    maxy = YVals[i];
            }

            //SpectroWizard.analit.Stat.Used[i]
            for (int p = 0; p < XPoints.Length;)
            {
                int p_from = p;
                double x = XPoints[p];
                for (; p < XPoints.Length && x == XPoints[p]; p++) ;

                double[] ys = new double[p - p_from];
                bool[] ye = new bool[ys.Length];
                for (int i = p_from, j = 0; i < p; i++, j++)
                {
                    ys[j] = YPoints[i];
                    ye[j] = PointEnabled[i];
                }

                SpectroWizard.analit.Stat.GetEver(ys,ye);

                for (int i = p_from; i < p; i++)
                {
                    if (PointEnabled[i] == false || SpectroWizard.analit.Stat.Used[i-p_from] == false)
                        continue;
                    if (IsValid(XPoints[i]) == false || IsValid(YPoints[i]) == false)
                        continue;
                    if (XPoints[i] < minx)
                        minx = XPoints[i];
                    if (XPoints[i] > maxx)
                        maxx = XPoints[i];
                    if (YPoints[i] < miny)
                        miny = YPoints[i];
                    if (YPoints[i] > maxy)
                        maxy = YPoints[i];
                }
            }
            /*for (int i = 0; i < XPoints.Length; i++)
            {
                if (PointEnabled[i] == false)
                    continue;
                if (XPoints[i] < minx)
                    minx = XPoints[i];
                if (XPoints[i] > maxx)
                    maxx = XPoints[i];
                if (YPoints[i] < miny)
                    miny = YPoints[i];
                if (YPoints[i] > maxy)
                    maxy = YPoints[i];
            }//*/

            size.X = (float)minx;
            size.Y = (float)miny;
            size.Width = (float)(maxx - minx);
            size.Height = (float)(maxy - miny);
        }

        public GLogGr2Data(BinaryReader br, string section_name)
            : base(GLogDataTypes.Gr2Data, section_name)
        {
        }

        public GLogGr2Data(string section_name, string view_name,
            double[] xvals,double[] yvals,
            double[] xpoints,double[] ypoints,
            bool[] en_points,int[] point_ids1,
            int[] point_ids2,
            Color color_draw, Color mark)
            : base(GLogDataTypes.Gr2Data, section_name)
        {
            XVals = xvals;
            YVals = yvals;
            XPoints = xpoints;
            YPoints = ypoints;
            PointEnabled = en_points;
            PointId1 = point_ids1;
            PointId2 = point_ids2;
            ViewName = view_name;
            ColSp = color_draw;
            ColMark = mark;
        }

        public override void LoadProc(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver < 1 || ver > 4)
                throw new Exception("Wrong version of GLog2Data");

            ViewName = br.ReadString();
            int count = br.ReadInt32();
            XVals = new double[count];
            YVals = new double[count];
            double xmin = 0, lx = 0;
            double ymin = 0, ly = 0;
            if (ver < 3)
            {
                for (int i = 0; i < XVals.Length; i++)
                {
                    XVals[i] = br.ReadDouble();
                    YVals[i] = br.ReadDouble();
                }
            }
            else
            {
                xmin = br.ReadDouble();
                ymin = br.ReadDouble();
                lx = br.ReadDouble();
                ly = br.ReadDouble();
                if (ver == 3)
                {
                    for (int i = 0; i < XVals.Length; i++)
                    {
                        double tmp = br.ReadUInt16();
                        XVals[i] = tmp * lx / ushort.MaxValue + xmin;
                        tmp = br.ReadUInt16();
                        YVals[i] = tmp * ly / ushort.MaxValue + ymin;
                    }
                }
                else
                {
                    for (int i = 0; i < XVals.Length; i++)
                    {
                        ushort tmpus = br.ReadUInt16();
                        if (tmpus < ushort.MaxValue)
                            XVals[i] = (double)(tmpus) * lx / (ushort.MaxValue - 1) + xmin;
                        else
                            XVals[i] = double.MaxValue;
                        tmpus = br.ReadUInt16();
                        if (tmpus < ushort.MaxValue)
                            YVals[i] = (double)(tmpus) * ly / (ushort.MaxValue - 1) + ymin;
                        else
                            YVals[i] = double.MaxValue;
                    }
                }
            }

            count = br.ReadInt32();
            XPoints = new double[count];
            YPoints = new double[count];
            PointEnabled = new bool[count];
            PointId1 = new int[count];
            PointId2 = new int[count];
            for (int i = 0; i < XPoints.Length; i++)
            {
                if (ver < 3)
                {
                    XPoints[i] = br.ReadDouble();
                    YPoints[i] = br.ReadDouble();
                }
                else
                {
                    if (ver == 3)
                    {
                        double tmp = br.ReadUInt16();
                        XPoints[i] = tmp * lx / ushort.MaxValue + xmin;
                        tmp = br.ReadUInt16();
                        YPoints[i] = tmp * ly / ushort.MaxValue + ymin;
                    }
                    else
                    {
                        ushort tmpus = br.ReadUInt16();
                        if (tmpus < ushort.MaxValue)
                            XPoints[i] = (double)(tmpus) * lx / (ushort.MaxValue-1) + xmin;
                        else
                            XPoints[i] = double.MaxValue;
                        tmpus = br.ReadUInt16();
                        if (tmpus < ushort.MaxValue)
                            YPoints[i] = (double)(tmpus) * ly / (ushort.MaxValue-1) + ymin;
                        else
                            YPoints[i] = double.MaxValue;
                    }
                }
                PointEnabled[i] = br.ReadBoolean();
                if (ver >= 2)
                {
                    PointId1[i] = br.ReadInt32();
                    PointId2[i] = br.ReadInt32();
                }
                else
                {
                    PointId1[i] = -1;
                    PointId2[i] = -1;
                }
            }

            ColSp = LoadColor(br);
            ColMark = LoadColor(br);

            ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Wrong end of GLog2Data");
        }

        public override void SaveProc(BinaryWriter bw)
        {
            bw.Write((byte)4);

            bw.Write(ViewName);
            bw.Write(XVals.Length);
            double xmin = XVals[0];
            double xmax = XVals[0];
            double ymin = YVals[0];
            double ymax = YVals[0];
            for (int i = 0; i < XVals.Length; i++)
            {
                if (IsValid(XVals[i]) == false ||
                    IsValid(YVals[i]) == false)
                    continue;

                if (xmin > XVals[i])
                    xmin = XVals[i];
                if (xmax < XVals[i])
                    xmax = XVals[i];
                if (ymin > YVals[i])
                    ymin = YVals[i];
                if (ymax < YVals[i])
                    ymax = YVals[i];
            }
            for (int i = 0; i < XPoints.Length; i++)
            {
                if (IsValid(XPoints[i]) == false ||
                    IsValid(YPoints[i]) == false)
                    continue;

                if (xmin > XPoints[i])
                    xmin = XPoints[i];
                if (xmax < XPoints[i])
                    xmax = XPoints[i];
                if (ymin > YPoints[i])
                    ymin = YPoints[i];
                if (ymax < YPoints[i])
                    ymax = YPoints[i];
            }
            double lx = xmax - xmin;
            if (lx == 0)
                lx = 1;
            double ly = ymax - ymin;
            if (ly == 0)
                ly = 1;
            bw.Write(xmin);
            bw.Write(ymin);
            bw.Write(lx);
            bw.Write(ly);
            for (int i = 0; i < XVals.Length; i++)
            {
                if (IsValid(XVals[i]) == true)
                    bw.Write((ushort)(((XVals[i] - xmin) * (ushort.MaxValue - 1)) / lx));
                else
                    bw.Write(ushort.MaxValue);
                if(IsValid(YVals[i]) == true)
                    bw.Write((ushort)(((YVals[i] - ymin) * (ushort.MaxValue-1)) / ly));
                else
                    bw.Write(ushort.MaxValue);
            }
            bw.Write(XPoints.Length);
            for (int i = 0; i < XPoints.Length; i++)
            {
                if (IsValid(XPoints[i]) == true)
                    bw.Write((ushort)(((XPoints[i] - xmin) * (ushort.MaxValue-1)) / lx));
                else
                    bw.Write(ushort.MaxValue);
                if (IsValid(YPoints[i]) == true)
                    bw.Write((ushort)(((YPoints[i] - ymin) * (ushort.MaxValue-1)) / ly));
                else
                    bw.Write(ushort.MaxValue);
                bw.Write(PointEnabled[i]);
                bw.Write(PointId1[i]);
                bw.Write(PointId2[i]);
            }
            SaveColor(bw, ColSp);
            SaveColor(bw, ColMark);

            bw.Write((byte)1);
        }
    }

    public class GLogSpData : GLogRecord
    {
        public double[] Vals;
        public int SelectedFrom;
        public int SelectedTo;
        public int MarkAt;
        public double MarkLy;
        public Color ColSp;
        public Color ColMark;
        public string ViewName;

        double Kx, Ky, X0, Y0;
        int PanelHeight;
        /*override public int ConvertXToPaint(double x)
        {
            return (int)((x - X0) * Kx);
        }

        override public int ConvertYToPaint(double y)
        {
            return (int)(PanelHeight - (y - Y0) * Ky);
        }*/

        override public double GetDataWidth()
        {
            return Vals.Length;
        }

        override public string GetViewName()
        {
            return ViewName;
        }

        public override void DrawCrosses(Graphics g, List<double> ly_from,
                List<double> ly_to, List<double> y, List<double> z)
        {
        }

        public override void Draw(Graphics g, Rectangle client, 
            RectangleF y_size,
            int level, int level_max,
            DrawSumType draw_sum,
            GraphLog master)
        {
            if (y_size.Height == 0 && y_size.Width == 0)
            {
                double min = Vals[0];
                double max = Vals[0];
                for (int i = 1; i < Vals.Length; i++)
                {
                    if (min > Vals[i])
                        min = Vals[i];
                    if (max < Vals[i])
                        max = Vals[i];
                }
                double dlt = max-min;
                dlt /= 20;
                if(dlt < 1)
                    dlt = 1;
                max += dlt;
                min -= dlt;
                y_size = new RectangleF(0, (float)min, 0, (float)(max - min));
            }
            int z_step = 3;
            int width = client.Width - master.DrawCount*z_step;
            double kx = width / (double)Vals.Length;
            double ky = (client.Height - master.DrawCount*z_step) / y_size.Height;
            int clientX = client.X + master.CurrentDrawIndex * z_step;
            int clientY = client.Y - master.CurrentDrawIndex * z_step;
            Pen oc = new Pen(GetColor(ColSp,level,level_max));
            Brush mc = new SolidBrush(GetColor(ColMark,level,level_max));
            int px = clientX, py = 2000;
            int y0 = clientY + client.Height;
            Kx = kx;
            Ky = ky;
            X0 = 0;
            Y0 = y_size.Y;
            PanelHeight = y0;
            for (int i = 0; i < Vals.Length; i++)
            {
                int x = (int)(i * kx) + clientX;
                int x1 = (int)(x + kx);
                if (double.IsNaN(Vals[i]))
                {
                    px = x;
                    continue;
                }
                int y1;
                if (Vals[i] > 2000000000)
                    y1 = py;
                else
                {
                    y1 = (int)(y0 - (Vals[i] - y_size.Y) * ky);
                    if (i == 0)
                        py = y1;
                    if (SelectedFrom <= i && i <= SelectedTo)
                    {
                        g.DrawLine(Pens.Gray, clientX + width * 7 / 20, y0,
                            clientX + width * 6 / 10, y0);
                        g.FillRectangle(mc, x - 1, y1, x1 - x + 2, y0 - y1);
                    }
                    else
                    {
                        g.DrawLine(oc, px, py, px, y1);
                        g.DrawLine(oc, px, y1, x, y1);
                    }
                }
                px = x;
                py = y1;
            }
        }

        public override int GetMinHeight()
        {
            return 0;
        }

        //public override void GetMinMaxVal(ref double min, ref double max)
        public override void GetMinMaxVal(ref RectangleF size)
        {
            double min,max;
            if (size.Width == 0 && size.Height == 0)
            {
                min = Vals[0];
                max = Vals[0];
            }
            else
            {
                min = size.Y;
                max = size.Y + size.Height;
            }
            for (int i = 1; i < Vals.Length; i++)
            {
                if (Vals[i] < min)
                    min = Vals[i];
                if (Vals[i] > max && Vals[i] < 2000000000)
                    max = Vals[i];
            }
            size.Y = (float)min;
            size.Height = (float)(max - min);
        }

        public GLogSpData(BinaryReader br,string section_name)
            : base(GLogDataTypes.SpData, section_name)
        {
            //LoadProc(br);
        }

        public GLogSpData(string section_name, string view_name,
            double[] data_view,
            int selected_from,int selected_to,
            int mark_at_pixel,double mark_ly,
            Color color_draw,Color mark)
            : base(GLogDataTypes.SpData, section_name)
        {
            ViewName = view_name;
            Vals = data_view;
            SelectedFrom = selected_from;
            SelectedTo = selected_to;
            MarkAt = mark_at_pixel;
            MarkLy = mark_ly;
            ColSp = color_draw;
            ColMark = mark;
        }

        public override void LoadProc(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver < 1 || ver > 2)
                throw new Exception("Wrong version of GLogSpData");

            ViewName = br.ReadString();
            Vals = new double[br.ReadInt32()];
            if (ver == 1)
            {
                for (int i = 0; i < Vals.Length; i++)
                    Vals[i] = br.ReadDouble();
            }
            else
            {
                double min = br.ReadDouble();
                double l = br.ReadDouble();
                for (int i = 0; i < Vals.Length; i++)
                    Vals[i] = br.ReadByte()*l/255+min;
            }
            SelectedFrom = br.ReadInt32();
            SelectedTo = br.ReadInt32();
            MarkAt = br.ReadInt32();
            MarkLy = br.ReadDouble();
            ColSp = LoadColor(br);
            ColMark = LoadColor(br);

            ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Wrong end of GLogSpData");
        }

        public override void SaveProc(BinaryWriter bw)
        {
            bw.Write((byte)2);

            bw.Write(ViewName);
            bw.Write(Vals.Length);
            double min = Vals[0];
            double max = Vals[0];
            for (int i = 0; i < Vals.Length; i++)
            {
                if (min > Vals[i])
                    min = Vals[i];
                if (max < Vals[i])
                    max = Vals[i];
            }
            bw.Write(min);
            double l = max-min;
            if (l == 0)
                l = 1;
            bw.Write(l);
            for (int i = 0; i < Vals.Length; i++)
                bw.Write((byte)(255*(Vals[i]-min)/l));
            bw.Write(SelectedFrom);
            bw.Write(SelectedTo);
            bw.Write(MarkAt);
            bw.Write(MarkLy);
            SaveColor(bw, ColSp);
            SaveColor(bw, ColMark);

            bw.Write((byte)1);
        }
    }

    public class GLogMsg : GLogRecord
    {
        public string Txt;
        public Color Col;

        double Kx = 0, Ky = 0, X0 = 0, Y0 = 0;
        int PanelHeight = 0;
        /*override public int ConvertXToPaint(double x)
        {
            return (int)((x - X0) * Kx);
        }

        override public int ConvertYToPaint(double y)
        {
            return (int)(PanelHeight - (y - Y0) * Ky);
        }*/

        override public string GetViewName()
        {
            return null;
        }

        override public double GetDataWidth()
        {
            return -1;
        }

        public override void DrawCrosses(Graphics g, List<double> ly_from,
            List<double> ly_to, List<double> y, List<double> z)
        {
        }

        Brush ColB = null;
        public override void Draw(Graphics g, Rectangle client, RectangleF y_size,
            int level, int level_max,
            DrawSumType draw_sum,
            GraphLog master)
        {
            if(ColB == null)
                ColB = new SolidBrush(Col);
            g.DrawString(Txt,Common.GraphLitleFont,ColB,client.X,client.Y);
        }

        public override void GetMinMaxVal(ref RectangleF size)
        {
        }

        public override int GetMinHeight()
        {
            return (int)Common.GraphLitleFont.SizeInPoints * 4 / 3;
        }

        public GLogMsg(BinaryReader br,string section_name)
            : base(GLogDataTypes.LogMsg,section_name)
        {
            //LoadProc(br);
        }

        public GLogMsg(string section_name,string txt, Color color) : base(GLogDataTypes.LogMsg,section_name)
        {
            if (txt != null)
                Txt = txt;
            else
                Txt = "";
            Col = color;
        }

        public override void LoadProc(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Wrong version of GLogMsg");
            
            Txt = br.ReadString();
            Col = LoadColor(br);

            ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Wrong end of GLogMsg");
        }

        public override void SaveProc(BinaryWriter bw)
        {
            bw.Write((byte)1);

            bw.Write(Txt);
            SaveColor(bw, Col);

            bw.Write((byte)1);
        }
    }
}
