using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

using SpectroWizard.data;
using SpectroWizard.util;
using SpectroWizard.analit.fk;

namespace SpectroWizard.gui.comp
{
    public delegate void CursorChanged(float cursor_x, float cursor_y);
    public delegate void SetLy(bool isAnalit,float ly);
    public partial class SpectrView : UserControl
    {
        public CursorChanged CursorListener = null;
        private SetLy SetLyListenerPriv = null;
        public SetLy SetLyListener
        {
            set
            {
                SetLyListenerPriv = value;
                mnMainRememberAs.Visible = (value != null);
            }
        }
        public SpectrView()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            DrawPanel.MouseWheel += new MouseEventHandler(DrawPanel_MouseWheel);
            if (Common.UserRole == Common.UserRoleTypes.Laborant)
                cbSpectrViewType.Visible = false;
            if (GPensD == null)
            {
                GPensD = new Pen[GColors.Length];
                GPensL = new Pen[GColors.Length];
                GBrushesD = new Brush[GColors.Length];
                GBrushesL = new Brush[GColors.Length];

                int cstep = 100;

                for (int i = 0; i < GColors.Length; i++)
                {
                    int r = GColors[i].R;
                    int g = GColors[i].G;
                    int b = GColors[i].B;
                    int lr = r + cstep;
                    if (lr > 255)   lr = 255;
                    int lg = g + cstep;
                    if (lg > 255)   lg = 255;
                    int lb = b + cstep;
                    if (lb > 255)   lb = 255;

                    Color c = Color.FromArgb(lr, lg, lb);

                    GPensD[i] = new Pen(GColors[i]);
                    GPensL[i] = new Pen(c);
                    GBrushesD[i] = new SolidBrush(GColors[i]);
                    GBrushesL[i] = new SolidBrush(c);
                }
            }
            DBGr = new DoubleBufferedGraphics(SpectroWizard.gui.MainForm.MForm, 
                DrawPanel, new PaintDel(ReDrawDelegateProc));
        }

        DoubleBufferedGraphics DBGr;

        bool DrawAutoZoomEnablePriv = false;
        public bool DrawAutoZoomEnable
        {
            get
            {
                return DrawAutoZoomEnablePriv;
            }
            set
            {
                DrawAutoZoomEnablePriv = value;
                chbAutoZoom.Visible = value;
            }
        }

        bool DrawAutoZoomYPriv = false;
        public bool DrawAutoZoomY
        {
            get
            {
                return DrawAutoZoomYPriv;
            }
            set
            {
                DrawAutoZoomYPriv = value;
            }
        }

        Dispers DispersToPaint = null;
        int DispersSn;
        public void SetupDispersToPaint(Dispers disp, int sn)
        {
            if (disp == null && DispersToPaint == null)
                return;
            DispersToPaint = disp;
            DispersSn = sn;
            ReDraw();
        }

        List<SpectrViewInfo> SpInfo = new List<SpectrViewInfo>();
        List<Spectr> FullSpInfo = new List<Spectr>();
        public float MinLy = float.MaxValue,
            MaxLy = -float.MaxValue,
            MinY = float.MaxValue,
            MaxY = -float.MaxValue;
        public float ViewMinLy = float.MaxValue,
                    ViewMaxLy = -float.MaxValue,
                    ViewMinY = float.MaxValue,
                    ViewMaxY = -float.MaxValue;

        public Spectr GetSpectr(int index)
        {
            if (index < 0 || index >= SpInfo.Count)
                return null;
            return SpInfo[index].Sp;
        }

        public bool IsDefaultDispers()
        {
            if (SpInfo.Count == 0)
                return true;
            return SpInfo[0].IsDispersDefault();
        }

        void InternalInit()
        {
            if (SpInfo.Count == 0)
                return;

            MinLy = SpInfo[0].MinLy;
            MaxLy = SpInfo[0].MaxLy;
            MinY = SpInfo[0].MinY;
            MaxY = SpInfo[0].MaxY;

            for (int i = 1; i < SpInfo.Count; i++)
            {
                if (MinLy > SpInfo[i].MinLy)
                    MinLy = SpInfo[i].MinLy;
                if (MaxLy < SpInfo[i].MaxLy)
                    MaxLy = SpInfo[i].MaxLy;
                if (MinY > SpInfo[i].MinY)
                    MinY = SpInfo[i].MinY;
                if (MaxY < SpInfo[i].MaxY)
                    MaxY = SpInfo[i].MaxY;
            }

            float dlt = (MaxY-MinY)/20;
            
            if (dlt == 0)
                dlt = 1;
            
            MaxY += dlt;
            MinY -= dlt;

            if (ViewMinLy == float.MaxValue)
            {
                ViewMinLy = MinLy;
                ViewMaxLy = MaxLy;
                ViewMinY = MinY;
                ViewMaxY = MaxY;
                if (ViewMaxY < Common.Conf.MaxLevel)
                    ViewMaxY = Common.Conf.MaxLevel;
            }
            else
            {
                if(ViewMinLy < MinLy)
                    ViewMinLy = MinLy;
                if(ViewMaxLy > MaxLy)
                    ViewMaxLy = MaxLy;
                /*if(ViewMinY < MinY)
                    ViewMinY = MinY;
                if (ViewMaxY > MaxY)
                    ViewMaxY = MaxY;*/

                /*if (ViewMinY > MaxY ||
                    ViewMaxY < MinY)
                {
                    ViewMinLy = MinLy;
                    ViewMaxLy = MaxLy;
                    ViewMinY = MinY;
                    ViewMaxY = MaxY;
                }*/
            }
            //ReDraw();
            CheckHScrollBar();
            CheckVScrollBar();
            CheckButtons();
        }

        const string MLSConst = "SpV";
        void InitViewListTypes()
        {
            if (cbSpectrViewType.Items.Count == 0)
            {
                cbSpectrViewType.Items.Add(Common.MLS.Get(MLSConst, "Cуммарный спектр"));
                cbSpectrViewType.Items.Add(Common.MLS.Get(MLSConst, "Все прожиги"));

                CheckViewListTypes(40);
                cbSpectrViewType.SelectedIndex = DefaultViewTypePriv;
            }
        }

        void CheckViewListTypes(int num)
        {
            if (NeedToReloadDefaultViewTypePriv == true)
            {
                cbSpectrViewType.Visible = false;
                return;
            }
            else
                cbSpectrViewType.Visible = true;
            if (Common.UserRole == Common.UserRoleTypes.Debuger ||
                    Common.UserRole == Common.UserRoleTypes.Sientist)
            {
                int i;
                for (i = 0; i < num; i++)
                    if (cbSpectrViewType.Items.Count-2 <= i)
                        cbSpectrViewType.Items.Add(Common.MLS.Get(MLSConst, "Кадр №") + (i + 1));

                if (cbSpectrViewType.SelectedIndex >= num)
                    cbSpectrViewType.SelectedIndex = num - 1;

                while (cbSpectrViewType.Items.Count-2 > num)
                    cbSpectrViewType.Items.RemoveAt(num - 1);
            }
        }

        int DefaultViewTypePriv = -1;
        bool NeedToReloadDefaultViewTypePriv = false;
        public bool NeedToReloadDefaultViewType
        {
            get
            {
                return NeedToReloadDefaultViewTypePriv;
            }
            set
            {
                NeedToReloadDefaultViewTypePriv = value;
                cbSpectrViewType.Visible = !value;
            }
        }
        public int DefaultViewType
        {
            get
            {
                return DefaultViewTypePriv;
            }
            set
            {
                DefaultViewTypePriv = value;
                //NeedToReloadDefaultViewType = true;
            }
        }

        public void ClearSpectrList()
        {
            SpInfo.Clear();
            FullSpInfo.Clear();
            InternalInit();
            InitViewListTypes();
        }

        public void ReLoadSpectr(Spectr view, int index)
        {
            if (view.GetDefultView() != null)
                SpInfo[index] = new SpectrViewInfo(view,SpInfo[index].Name);
            if(index == 0 && spDataView.Visible)
                spDataView.SetupSpectr(SpInfo[0].Sp);
            InternalInit();
            if (DrawAutoZoomY)
                ZoomY();
        }

        public void AddSpectr(Spectr view, string name)
        {
            if (SpInfo.Count == GColors.Length)
                return;
            //CheckViewListTypes(view.GetViewsSet().Count);
            if (view.GetDefultView() != null)
            {
                SpInfo.Add(new SpectrViewInfo(view, name));
                FullSpInfo.Add(view);
            }
            InternalInit();
            InitViewListTypes();
            if (SpInfo.Count == 1 && spDataView.Visible)
                spDataView.SetupSpectr(SpInfo[0].Sp);

            if (DrawAutoZoomYPriv)
                ZoomY();
        }

        public void ZoomY()
        {
            if(SpInfo.Count == 0)
                return;
            //ViewMinY = SpInfo[0].MinY;
            //ViewMaxY = SpInfo[0].MaxY;
            for (int i = 0; i < SpInfo.Count; i++)
            {
                if (SpInfo[i].MinY < ViewMinY)
                    ViewMinY = SpInfo[i].MinY;
                if (SpInfo[i].MaxY > ViewMaxY)
                    ViewMaxY = SpInfo[i].MaxY;
            }//*/
            //ViewMinY = MinY;
            //ViewMaxY = MaxY;
        }

        public void ReDraw()
        {
            ReDraw(false);
        }

        void ReDraw(bool flag)
        {
            if (DefaultViewTypePriv >= 0 && DefaultViewTypePriv < cbSpectrViewType.Items.Count &&
                NeedToReloadDefaultViewType == true)
                cbSpectrViewType.SelectedIndex = DefaultViewTypePriv;
            CheckView();
            DBGr.ReDraw(flag);
        }

        public void ReDrawNow()
        {
            ReDraw(true);
        }

        double KLy;
        double KY;
        void ReDrawDelegateProc(Graphics g)
        {
            try
            {
                //Graphics g = GBmp[Bmp2Draw];
                g.FillRectangle(Brushes.White, 0, 0, DrawPanel.Width, DrawPanel.Height);
                if (SpInfo.Count == 0)
                {
                    string str;
                    if (Common.MLS != null)
                        str = Common.MLS.Get("SpView", "No Data");
                    else
                        str = "No Data";
                    SizeF size = g.MeasureString(str, Common.GraphBigFont);
                    g.DrawString(str, Common.GraphBigFont, Brushes.LightGray,
                        DrawPanel.Width / 2 - (int)size.Width / 2,
                        DrawPanel.Height / 2 - (int)size.Height / 2);
                }
                else
                {
                    KLy = DrawPanel.Width / (ViewMaxLy - ViewMinLy);
                    KY = DrawPanel.Height / (ViewMaxY - ViewMinY);
                    PaintGreedX(g);
                    PaintGreedY(g);

                    if (DrawSelection == false)
                    {
                        PaintSpLines(g, SpLQuery, SpLSelected, true);
                        PaintSpLines(g, SpLLineQuery, SpLLineSelected, false);
                    }

                    for (int i = SpInfo.Count - 1; i >= 0; i--)
                        PaintGraph(SpInfo[i], g, i);
                    
                    if (SpAddone != null)
                        PaintGraph(SpAddoneView, g, SpInfo.Count);

                    if (DrawSelection == false)
                    {
                        PaintSpLineMarkers(g);
                        PaintAnalitMarkers(g);
                        PaintCursor(g);
                    }
                    else
                        PaintSelection(g);
                    
                    for (int i = SpInfo.Count-1; i >= 0; i--)
                        PaintSpNames(SpInfo[i], g, i);
                    
                    if(SpAddoneView != null)
                        PaintSpNames(SpAddoneView, g, SpInfo.Count);

                    //if (DrawDispersPriv)
                    if(DispersToPaint != null)
                        PaintDispers(g);
                    
                    for (int i = SpInfo.Count - 1; i >= 0; i--)
                        if (SpInfo[i].Sp.SpectrInfo != null && SpInfo[i].Sp.SpectrInfo.Length > 0)
                        {
                            g.DrawString("?!", Common.GraphBigFont, Brushes.Red, 10, 10);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        #region Marker functions
        public class Marker
        {
            public double Ly;
            public string Name;
            public bool Selected = false;
            public bool Visible = true;
            public Pen Pn;
            public Brush Br;

            public Marker(double ly, string name, Color col,
                bool selected)
            {
                Ly = ly;
                Name = name;
                Pn = new Pen(col);
                Br = new SolidBrush(col);
                Selected = selected;
            }
        }

        public void ZoomAnalitMarkers(float ampl_level)
        {
            if (AnalitMarkers.Count == 0 || SpInfo.Count == 0 || chbAutoZoom.Checked == false)
                return;
            double ly_from = double.MaxValue;
            double ly_to = -double.MaxValue;
            int count = 0;
            for (int i = 0; i < AnalitMarkers.Count; i++)
            {
                if (AnalitMarkers[i].Ly <= MinLy ||
                    AnalitMarkers[i].Ly >= MaxLy)
                    continue;
                if (ly_from > AnalitMarkers[i].Ly)
                    ly_from = AnalitMarkers[i].Ly;
                if (ly_to < AnalitMarkers[i].Ly)
                    ly_to = AnalitMarkers[i].Ly;
                count++;
            }
            if (count <= 0)
                return;
            double pixel_size_from = SpInfo[0].GetPixelSize(ly_from);
            double pixel_size_to = SpInfo[0].GetPixelSize(ly_to);
            const int size = 30;
            ly_from -= pixel_size_from * size;
            if (ly_from < MinLy)
                ly_from = MinLy;
            ly_to += pixel_size_to * size;
            if (ly_to > MaxLy)
                ly_to = MaxLy;
            ViewMinLy = (float)ly_from;
            ViewMaxLy = (float)ly_to;
            if (ampl_level > 0)
            {
                ViewMaxY = ampl_level * 1.1F;
                ViewMinY = -10;
            }
            
            ReDraw();

            CheckVScrollBar();
            CheckHScrollBar();
        }

        List<Marker> AnalitMarkers = new List<Marker>();
        List<Marker> SpLineMarkers = new List<Marker>();
        public void ClearAnalitMarkers()
        {
            AnalitMarkers.Clear();
        }

        public void ClearSpLineMarkers()
        {
            SpLineMarkers.Clear();
        }

        public Marker AddAnalitMarker(float ly, string name,Color col,bool selected)
        {
            Marker mk = new Marker(ly, name,col,selected);
            AnalitMarkers.Add(mk);
            return mk;
        }

        /*public Marker AddMonoColorMarker(float ly, string name)
        {
            Marker mk = new Marker(ly, name, false);
            AnalitMarkers.Add(mk);
            return mk;
        }*/

        public void AddSpLineMarker(float ly, string name)
        {
            if (ly < MinLy || ly > MaxLy)
                return;
            SpLineMarkers.Add(new Marker(ly, name,Color.Red,false));
        }

        void PaintAnalitMarkers(Graphics g)
        {
            int y2 = DrawPanel.Height;
            int font_step = (int)(Common.GraphNormalFont.SizeInPoints * 4/3);
            int y1 = 50;
            for (int i = 0; i < AnalitMarkers.Count; i++)
            {
                if (AnalitMarkers[i].Visible == false)
                    continue;
                int x = (int)((AnalitMarkers[i].Ly - ViewMinLy) * KLy);
                if (x < -50 || x > DrawPanel.Width)
                    continue;
                y1 += font_step;
                if (y1 > y2 / 2)
                    y1 = 50;
                if (AnalitMarkers[i].Selected == false)
                {
                    g.DrawLine(AnalitMarkers[i].Pn, x, y1, x, y2);
                    g.DrawString(AnalitMarkers[i].Name, Common.GraphNormalFont,
                        AnalitMarkers[i].Br, x, y1 - font_step);
                }
                else
                {
                    g.DrawLine(AnalitMarkers[i].Pn, x, 30, x, y2);
                    g.DrawLine(AnalitMarkers[i].Pn, x - 1, 60, x - 1, y2);
                    g.DrawLine(AnalitMarkers[i].Pn, x + 1, 60, x + 1, y2);
                    int width = (int)(g.MeasureString(AnalitMarkers[i].Name, Common.GraphBigFont).Width);
                    if (x + width > DrawPanel.Width - 5)
                        x = DrawPanel.Width - 5 - width;
                    g.DrawString(AnalitMarkers[i].Name, Common.GraphBigFont,
                        AnalitMarkers[i].Br, x+2, 20);
                }
            }
        }

        void PaintSpLineMarkers(Graphics g)
        {
        }
        #endregion

        #region Draw function
        static Color[] GColors = { Color.Black, Color.Red, Color.Blue, Color.Brown, Color.Green, Color.Purple,
                          Color.Aqua, Color.BlueViolet, Color.Coral, Color.Cyan, Color.LightGray};
        static Pen[] GPensD, GPensL;// = { Pens.Black, Pens.Green, Pens.Blue, Pens.Purple, Pens.Brown,
                      //Pens.Aqua, Pens.BlueViolet, Pens.Coral, Pens.Cyan, Pens.LightGray};
        static Brush[] GBrushesD,GBrushesL;// = { Brushes.Black, Brushes.Green, Brushes.Blue, Brushes.Purple, Brushes.Brown,
                           //Brushes.Aqua, Brushes.BlueViolet, Brushes.Coral, Brushes.Cyan, Brushes.LightGray};

        void PaintDispers(Graphics g)
        {
            if(DispersToPaint == null || DispersSn < 0)
                return;
            //double dlt_view = ViewMaxLy - ViewMinLy;
            int[] size = DispersToPaint.GetSensorSizes();
            double dlt_size = size[DispersSn]*0.2;
            double kx = DrawPanel.Width/
                (size[DispersSn] + dlt_size + dlt_size);
            /*int base_pixel = 0;
            for(int s = 0;s<DispersSn;s++)
                base_pixel += size[s];*/
            double min_ly = double.MaxValue;
            double max_ly = -double.MaxValue;
            if (DispersSn > 0)
            {
                for (int i = (int)(size[DispersSn - 1] * 0.8); i < size[DispersSn - 1]; i++)
                {
                    double ly1 = DispersToPaint.GetLyByLocalPixel(DispersSn - 1, i);
                    double p = DispersToPaint.GetLocalPixelByLy(DispersSn - 1, ly1);
                    double ly2 = DispersToPaint.GetLyByLocalPixel(DispersSn - 1, p);
                    if (ly1 > 10000 || ly2 > 10000 ||
                        ly1 < 0 || ly2 < 0)
                        continue;
                    if (ly1 < min_ly)
                        min_ly = ly1;
                    if (ly2 < min_ly)
                        min_ly = ly2;
                    if (ly1 > max_ly)
                        max_ly = ly1;
                    if (ly2 > max_ly)
                        max_ly = ly2;
                }
            }
            for(int i = 0;i<size[DispersSn];i++)
            {
                double ly1 = DispersToPaint.GetLyByLocalPixel(DispersSn,i);
                double p = DispersToPaint.GetLocalPixelByLy(DispersSn,ly1);
                double ly2 = DispersToPaint.GetLyByLocalPixel(DispersSn,p);
                if (ly1 > 10000 || ly2 > 10000 ||
                    ly1 < 0 || ly2 < 0)
                    continue;
                if(ly1 < min_ly)
                    min_ly = ly1;
                if(ly2 < min_ly)
                    min_ly = ly2;
                if(ly1 > max_ly)
                    max_ly = ly1;
                if(ly2 > max_ly)
                    max_ly = ly2;
            }
            if (DispersSn < size.Length - 1)
            {
                for (int i = 0; i < (int)(size[DispersSn+1] * 0.2); i++)
                {
                    double ly1 = DispersToPaint.GetLyByLocalPixel(DispersSn + 1, i);
                    double p = DispersToPaint.GetLocalPixelByLy(DispersSn + 1, ly1);
                    double ly2 = DispersToPaint.GetLyByLocalPixel(DispersSn + 1, p);
                    if (ly1 > 10000 || ly2 > 10000 ||
                        ly1 < 0 || ly2 < 0)
                        continue;
                    if (ly1 < min_ly)
                        min_ly = ly1;
                    if (ly2 < min_ly)
                        min_ly = ly2;
                    if (ly1 > max_ly)
                        max_ly = ly1;
                    if (ly2 > max_ly)
                        max_ly = ly2;
                }
            }
            double ky = DrawPanel.Height/(double)(max_ly-min_ly);
            int px = -10000;
            int py1 = 1000;
            int py2 = 1000;
            double xd = 0;
            if (DispersSn > 0)
            {
                for (int i = (int)(size[DispersSn-1] * 0.8); i < size[DispersSn-1]; i++, xd += kx)
                {
                    double ly1 = DispersToPaint.GetLyByLocalPixel(DispersSn-1, i);
                    double p = DispersToPaint.GetLocalPixelByLy(DispersSn-1, ly1);
                    double ly2 = DispersToPaint.GetLyByLocalPixel(DispersSn-1, p);
                    int x = (int)(xd);
                    int y1 = DrawPanel.Height - (int)((ly1 - min_ly) * ky);
                    int y2 = DrawPanel.Height - (int)((ly2 - min_ly) * ky);
                    if (y1 < 0) y1 = 0;
                    if (y2 < 0) y2 = 0;
                    if (y1 > DrawPanel.Height) y1 = DrawPanel.Height;
                    if (y2 > DrawPanel.Height) y2 = DrawPanel.Height;
                    g.DrawLine(Pens.Pink, px, py1, x, y1);
                    g.DrawLine(Pens.Pink, px, py2, x, y2);
                    px = x;
                    py1 = y1;
                    py2 = y2;
                }
            }
            for (int i = 0; i < size[DispersSn]; i++, xd += kx)
            {
                double ly1 = DispersToPaint.GetLyByLocalPixel(DispersSn,i);
                double p = DispersToPaint.GetLocalPixelByLy(DispersSn,ly1);
                double ly2 = DispersToPaint.GetLyByLocalPixel(DispersSn,p);
                int x = (int)(xd);
                int y1 = DrawPanel.Height - (int)((ly1 - min_ly) * ky);
                int y2 = DrawPanel.Height - (int)((ly2 - min_ly) * ky);
                if (y1 < 0) y1 = 0;
                if (y2 < 0) y2 = 0;
                if (y1 > DrawPanel.Height) y1 = DrawPanel.Height;
                if (y2 > DrawPanel.Height) y2 = DrawPanel.Height;
                g.DrawLine(Pens.Red, px, py1, x, y1);
                g.DrawLine(Pens.Red, px, py2, x, y2);
                px = x;
                py1 = y1;
                py2 = y2;
            }
            if (DispersSn < size.Length - 1)
            {
                for (int i = 0; i < (int)(size[DispersSn+1] * 0.2); i++, xd += kx)
                {
                    double ly1 = DispersToPaint.GetLyByLocalPixel(DispersSn + 1, i);
                    double p = DispersToPaint.GetLocalPixelByLy(DispersSn + 1, ly1);
                    double ly2 = DispersToPaint.GetLyByLocalPixel(DispersSn + 1, p);
                    int x = (int)(xd);
                    int y1 = DrawPanel.Height - (int)((ly1 - min_ly) * ky);
                    int y2 = DrawPanel.Height - (int)((ly2 - min_ly) * ky);
                    if (y1 < 0) y1 = 0;
                    if (y2 < 0) y2 = 0;
                    if (y1 > DrawPanel.Height) y1 = DrawPanel.Height;
                    if (y2 > DrawPanel.Height) y2 = DrawPanel.Height;
                    g.DrawLine(Pens.Pink, px, py1, x, y1);
                    g.DrawLine(Pens.Pink, px, py2, x, y2);
                    px = x;
                    py1 = y1;
                    py2 = y2;
                }
            }
        }

        void PaintSpNames(SpectrViewInfo view, Graphics g, int color)
        {
            g.DrawString(view.Name, Common.GraphNormalFont, GBrushesD[color], 100, 15 + color * Common.GraphNormalFont.SizeInPoints);
        }

        //const int KLineBase = 20;
        int SelectedSpLine = 0;
        public LineDbRecord SelSpLine
        {
            get
            {
                try
                {
                    LineDbRecord lr = Common.LDb.Data[SpLSelected[SelectedSpLine]];
                    return lr;
                }
                catch
                {
                    return null;
                }
            }
        }
        void PaintSpLines(Graphics g,LDbQuery ldbq,List<int> sel,bool paint_sel)
        {
            if (sel == null ||
                sel.Count == 0)
                return;

            int KLine;
            if (paint_sel)
                KLine = 15;
            else
                KLine = 20;

            int[] h = new int[DrawPanel.Width];
            string[] names = new string[DrawPanel.Width];
            int found = 0;
            int i_from = 0;
            int i_to = 0;
            for (int i = 0; i < sel.Count; i++)
            {
                if (i == SelectedSpLine)
                    continue;
                LineDbRecord lr = Common.LDb.Data[sel[i]];
                int xp = (int)((lr.Ly - ViewMinLy) * KLy);
                if (xp < 0)
                {
                    i_from = i + 1;
                    continue;
                }
                if (xp >= DrawPanel.Width)
                    break;
                i_to = i;
                found++;
                int intens = (int)(Math.Sqrt(ldbq.GetQIntens(lr)+2) * KLine);
                if(intens > h[xp])
                {
                    h[xp] = intens;
                    names[xp] = lr.ElementName + lr.IonLevel;// +" " + Math.Round(lr.Ly, 2) + " " + lr.QIntens;
                }
            }
            Pen p;
            Brush br;
            if (paint_sel)
            {
                p = Pens.Blue;
                br = Brushes.Black;
            }
            else
            {
                p = Pens.Green;
                br = Brushes.Blue;
            }
            if(found < 1000)
            {
                for (int i = i_from; i <= i_to; i++)
                {
                    if (i == SelectedSpLine)
                        continue;
                    LineDbRecord lr = Common.LDb.Data[sel[i]];
                    int xp = (int)((lr.Ly - ViewMinLy) * KLy);
                    int intens = (int)(Math.Sqrt(ldbq.GetQIntens(lr) + 2) * KLine);
                    string name = lr.ElementName + lr.IonLevel;// +" " + Math.Round(lr.Ly, 2) + " " + lr.QIntens;
                    int hl = intens;
                    if (hl > DrawPanel.Height - 20)
                        hl = DrawPanel.Height - 20;
                    int y = DrawPanel.Height - hl;
                    g.DrawLine(p, xp, DrawPanel.Height,
                        xp, y);
                    g.DrawString(name, Common.GraphLitleFont, br,
                        xp - 10, y - 15);
                }
            }
            else
            {
                for (int i = 0; i < h.Length; i++)
                {
                    if (names[i] == null)
                        continue;
                    int hl = h[i];
                    if(hl > DrawPanel.Height-20)
                        hl = DrawPanel.Height-20;
                    int y = DrawPanel.Height - hl;
                    g.DrawLine(Pens.Red, i, DrawPanel.Height, 
                        i, y);
                    g.DrawString(names[i], Common.GraphLitleFont, br, i - 10, y - 15);
                }
            }
            if (SelectedSpLine >= 0 && sel.Count > SelectedSpLine && paint_sel)
                try
                {
                    LineDbRecord lr = Common.LDb.Data[sel[SelectedSpLine]];
                    int xp = (int)((lr.Ly - ViewMinLy) * KLy);
                    int intens = (int)(Math.Sqrt(ldbq.GetQIntens(lr) + 2) * KLine);
                    string name = lr.ElementName + lr.IonLevel + " " + Math.Round(lr.Ly, 2) + " " + ldbq.GetQIntens(lr);// lr.QIntens;
                    int hl = intens;
                    if (hl > DrawPanel.Height - 20)
                        hl = DrawPanel.Height - 20;
                    int y = DrawPanel.Height - hl;
                    g.DrawLine(Pens.Red, xp, DrawPanel.Height,
                        xp, 5);
                    g.DrawLine(Pens.Red, xp + 1, y,
                        xp + 1, DrawPanel.Height-5);
                    g.DrawLine(Pens.Red, xp - 1, y,
                        xp - 1, DrawPanel.Height-5);
                    g.DrawString(name, Common.GraphNormalFont, Brushes.Red,
                        xp + 10, 15);
                }
                catch
                {
                }
        }

        void PaintGraph(SpectrViewInfo view, Graphics g,int index)
        {
            if (PaintGraph(view, g, index, false) == true &&
                cmMainSmooth.Checked)
                PaintGraph(view, g, index, true);
        }

        bool PaintGraph(SpectrViewInfo view, Graphics g, int index,bool smooth)
        {
            bool need_smooth = false;
            int color = index;
            Pen pl = GPensL[color];
            Pen pd = GPensD[color];
            if (cbSpectrViewType.SelectedIndex <= 0)
            {
                for (int s = 0; s < view.SpData.Length; s++)
                {
                    int xp = (int)((view.Lys[s][0] - ViewMinLy) * KLy);
                    int yp = DrawPanel.Height - (int)((view.SpData[s][0] - ViewMinY) * KY);

                    Pen p;
                    if ((s & 1) == 1)
                        p = pl;
                    else
                        p = pd;

                    try
                    {
                        g.DrawLine(Pens.Yellow, xp, yp, xp, 0);
                    }
                    catch
                    {
                    }
                    int ln = view.SpData[s].Length;
                    int dlt_x = 0;
                    for (int i = 1; i < ln; i++)
                    {
                        int x = (int)((view.Lys[s][i] - ViewMinLy) * KLy);
                        int y;
                        if (view.SpData[s][i] < int.MaxValue)
                        {
                            //continue;
                            y = DrawPanel.Height - (int)((view.SpData[s][i] - ViewMinY) * KY);
                            g.DrawLine(p, xp, yp, x, yp);
                            g.DrawLine(p, x, yp, x, y);
                            dlt_x = x - xp;
                        }
                        else
                        {
                            g.DrawLine(Pens.Red, x, 0, x, 3);
                            y = yp;
                        }
                        xp = x;
                        yp = y;
                    }
                    try
                    {
                        g.DrawLine(p, xp, yp, xp + dlt_x, yp);
                        g.DrawLine(Pens.Yellow, xp + dlt_x, yp, xp + dlt_x, DrawPanel.Height);
                    }
                    catch
                    {
                    }
                }
                return need_smooth;
            }
            int[][] data;
            if (cbSpectrViewType.SelectedIndex == 1)
            {
                int col = 0;
                int c_from = pd.Color.R;
                if (c_from > pd.Color.G)
                {
                    col = 1;
                    c_from = pd.Color.G;
                }
                if (c_from > pd.Color.B)
                {
                    col = 2;
                    c_from = pd.Color.B;
                }
                int c_to = (c_from + 255) / 2;
                int dlt = c_to - c_from;
                Pen ppd = pd;
                Pen ppl = pl;
                bool draw_smooth = false;
                for (int v = 0; v < view.GetShotCount(); v++)
                {
                    int nc;
                    if (view.GetShotCount() > 1)
                        nc = c_from + dlt * v / (view.GetShotCount() - 1);
                    else
                        nc = c_from;
                    data = view.GetShotData(v);
                    for (int s = 0; s < data.Length; s++)
                    {
                        Pen p;
                        if ((s & 1) == 1)
                        {
                            switch (col)
                            {
                                case 0: p = new Pen(Color.FromArgb(nc, ppd.Color.G, ppd.Color.B)); break;
                                case 1: p = new Pen(Color.FromArgb(ppd.Color.R, nc, ppd.Color.B)); break;
                                default: p = new Pen(Color.FromArgb(ppd.Color.R, ppd.Color.G, nc)); break;
                            }
                        }
                        else
                        {
                            switch (col)
                            {
                                case 0: p = new Pen(Color.FromArgb(nc, ppl.Color.G, ppl.Color.B)); break;
                                case 1: p = new Pen(Color.FromArgb(ppl.Color.R, nc, ppl.Color.B)); break;
                                default: p = new Pen(Color.FromArgb(ppl.Color.R, ppl.Color.G, nc)); break;
                            }
                        }

                        int xp = (int)((view.Lys[s][0] - ViewMinLy) * KLy);
                        int yp = DrawPanel.Height - (int)((data[s][0] - ViewMinY) * KY);
                        //int xpp = xp-1, ypp = yp-1;
                        g.DrawLine(Pens.Yellow, xp, yp, xp, 0);

                        int ln = data[s].Length;
                        int dlt_x = 0;
                        
                        for (int i = 1; i < ln; i++)
                        {
                            int x = (int)((view.Lys[s][i] - ViewMinLy) * KLy);
                            int y;
                            if (data[s][i] < int.MaxValue)
                            {
                                y = DrawPanel.Height - (int)((data[s][i] - ViewMinY) * KY);
                                if (x - xp > 3)
                                    need_smooth = true;
                                if (x >= 0 && x < DrawPanel.Width)
                                {
                                    if (smooth && x - xp > 3)
                                        draw_smooth = true;
                                    if (draw_smooth == false)
                                    {
                                        g.DrawLine(p, xp, yp, x, yp);
                                        g.DrawLine(p, x, yp, x, y);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int xn = (int)((view.Lys[s][i + 1] - ViewMinLy) * KLy);
                                            int yn = DrawPanel.Height - (int)((data[s][i + 1] - ViewMinY) * KY);
                                            int xnn = (int)((view.Lys[s][i + 2] - ViewMinLy) * KLy);
                                            int ynn = DrawPanel.Height - (int)((data[s][i + 2] - ViewMinY) * KY);
                                            double[] xt = { xp, x, xn, xnn }, yt = { yp, y, yn, ynn };
                                            double[] k = Interpolation.mInterpol3(xt, yt);
                                            for (double t = 0; t < 1; t += 0.2)
                                            {
                                                double x1 = x + (xn - x) * t;
                                                double x2 = x + (xn - x) * (t + 0.2);
                                                int from_x = (int)(x1);
                                                int from_y = (int)(k[0] + k[1] * x1 + k[2] * x1 * x1 + k[3] * x1 * x1 * x1);
                                                int to_x = (int)(x2);
                                                int to_y = (int)(k[0] + k[1] * x2 + k[2] * x2 * x2 + k[3] * x2 * x2 * x2);
                                                g.DrawLine(p, from_x, from_y, to_x, to_y);
                                            }
                                        }
                                        catch
                                        {
                                            g.DrawLine(p, xp, yp, x, y);
                                        }
                                    }
                                }
                                dlt_x = x - xp;
                            }
                            else
                            {
                                g.DrawLine(Pens.Red, x, 0, x, 3);
                                y = yp;
                            }
                            //xpp = xp;
                            //ypp = yp;
                            xp = x;
                            yp = y;
                        }
                        g.DrawLine(p, xp, yp, xp + dlt_x, yp);
                        g.DrawLine(Pens.Yellow, xp + dlt_x, yp, xp + dlt_x, DrawPanel.Height);
                    }
                }
                return need_smooth;
            }
            data = view.GetViewData(cbSpectrViewType.SelectedIndex - 2);
            if (data == null)
                return need_smooth;
            for (int s = 0; s < data.Length; s++)
            {
                Pen p;
                if ((s & 1) == 1)
                    p = pl;
                else
                    p = pd;

                int xp = (int)((view.Lys[s][0] - ViewMinLy) * KLy);
                int yp = DrawPanel.Height - (int)((data[s][0] - ViewMinY) * KY);

                g.DrawLine(Pens.Yellow, xp, yp, xp, 0);

                int ln = data[s].Length;
                int dlt_x = 0;
                for (int i = 1; i < ln; i++)
                {
                    int x = (int)((view.Lys[s][i] - ViewMinLy) * KLy);
                    int y;
                    if (data[s][i] < int.MaxValue)
                    {
                        y = DrawPanel.Height - (int)((data[s][i] - ViewMinY) * KY);
                        g.DrawLine(p, xp, yp, x, yp);
                        g.DrawLine(p, x, yp, x, y);
                        dlt_x = x - xp;
                    }
                    else
                    {
                        g.DrawLine(Pens.Red, x, 0, x, 3);
                        y = yp;
                    }
                    xp = x;
                    yp = y;
                }
                g.DrawLine(p, xp, yp, xp + dlt_x, yp);
                g.DrawLine(Pens.Yellow, xp + dlt_x, yp, xp + dlt_x, DrawPanel.Height);
            }
            return need_smooth;
        }

        void PaintSelection(Graphics g)
        {
            float minx = PrevMouseX;
            float maxx = CurrentMouseX;
            float miny = PrevMouseY;
            float maxy = CurrentMouseY;

            if (minx > maxx)
            {
                float tmp = maxx;
                maxx = minx;
                minx = tmp;
            }
            if (miny > maxy)
            {
                float tmp = maxy;
                maxy = miny;
                miny = tmp;
            }

            int minxi = (int)((minx - ViewMinLy) * KLy);
            int minyi = DrawPanel.Height - (int)((miny - ViewMinY) * KY);
            int maxxi = (int)((maxx - ViewMinLy) * KLy);
            int maxyi = DrawPanel.Height - (int)((maxy - ViewMinY) * KY);

            g.DrawRectangle(CursorPen, minxi, maxyi, maxxi - minxi, minyi - maxyi);
        }

        void PaintGreedX(Graphics g)
        {
            double[] vals = serv.GetGoodValues(ViewMinLy, ViewMaxLy, DrawPanel.Width / 100);
            if (vals == null)
                return;
            for (int i = 0; i < vals.Length; i++)
            {
                int x = (int)((vals[i] - ViewMinLy) * KLy);
                g.DrawLine(Pens.LightGray, x, 0, x, DrawPanel.Height);
                g.DrawString(vals[i].ToString(), Common.GraphNormalFont, Brushes.Gray, x + 1, 2);
            }
        }

        void PaintGreedY(Graphics g)
        {
            double[] vals = serv.GetGoodValues(ViewMinY, ViewMaxY, DrawPanel.Height / 100);
            if (vals == null)
                return;
            for (int i = 0; i < vals.Length; i++)
            {
                int y = DrawPanel.Height - (int)((vals[i] - ViewMinY) * KY);
                g.DrawLine(Pens.LightGray, 0, y, DrawPanel.Width, y);
                g.DrawString(vals[i].ToString(), Common.GraphNormalFont, Brushes.Gray, 2, y + 1);
            }
        }

        float CursorX = 0;
        float CursorY = 0;
        List<float> CursorPixel = new List<float>();
        List<int> CursorSn = new List<int>();
        public void GetCursorPosition(out float x, out float y, out List<float> pix,out List<int> sn)
        {
            x = CursorX;
            y = CursorY;
            pix = CursorPixel;
            sn = CursorSn;
        }

        Pen CursorPen = new Pen(Color.FromArgb(128,255,0,0));
        void PaintCursor(Graphics g)
        {
            if (CursorX == 0 &&
                CursorY == 0)
            {
                if (btZ.Enabled != false)
                    btZ.Enabled = false;
                return;
            }

            int x = (int)((CursorX - ViewMinLy) * KLy);
            int y = DrawPanel.Height - (int)((CursorY - ViewMinY) * KY);

            g.DrawLine(CursorPen, x, 0, x, DrawPanel.Height);
            int yi = 50;
            g.DrawString(Math.Round(CursorX, 1).ToString()+(char)0xC5, Common.GraphLitleFont, Brushes.Red, x, yi);
            yi += (int)Common.GraphLitleFont.SizeInPoints+4;
            for (int i = 0; i < CursorPixel.Count; i++)
            {
                g.DrawString(" n:"+Math.Round(CursorPixel[i], 1).ToString(), Common.GraphLitleFont, Brushes.Red, x, yi);
                yi += (int)Common.GraphLitleFont.SizeInPoints;
            }
            g.DrawLine(CursorPen, 0, y, DrawPanel.Width, y);

            if (btZ.Enabled != true)
                btZ.Enabled = true;
        }
        #endregion

        #region Zoom button section....
        private void btZ_Click(object sender, EventArgs e)
        {
            try
            {
                if(SpInfo.Count == 0)
                    return;
                float dlt = (float)SpInfo[0].GetPixelSize((ViewMinLy + ViewMaxLy) / 2);
                dlt *= DrawPanel.Width / 4;
                ViewMinLy = CursorX - dlt;
                ViewMaxLy = CursorX + dlt;
                CheckVScrollBar();
                CheckHScrollBar();
                ReDraw();
                CheckButtons();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btXPlus_Click(object sender, EventArgs e)
        {
            try
            {
                float dlt = ViewMaxLy - ViewMinLy;
                dlt /= 10;
                if (CursorX > ViewMinLy && CursorX < ViewMaxLy)
                {
                    float w = (ViewMaxLy - ViewMinLy)/2;
                    w -= dlt;
                    ViewMinLy = CursorX - w;
                    ViewMaxLy = CursorX + w;
                    if (ViewMinLy < MinLy)
                        ViewMinLy = MinLy;
                    if (ViewMaxLy > MaxLy)
                        ViewMaxLy = MaxLy;
                }
                else
                {
                    ViewMaxLy -= dlt;
                    ViewMinLy += dlt;
                }
                CheckHScrollBar();
                CheckButtons();
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btYPlus_Click(object sender, EventArgs e)
        {
            try
            {
                float dlt = ViewMaxY - ViewMinY;
                dlt /= 10;
                if (CursorY > ViewMinY && CursorY < ViewMaxY)
                {
                    float w = (ViewMaxY - ViewMinY) / 2;
                    w -= dlt;
                    ViewMinY = CursorY - w;
                    ViewMaxY = CursorY + w;
                    if (ViewMinY < MinY)
                        ViewMinY = MinY;
                    if (ViewMaxY > MaxY)
                        ViewMaxY = MaxY;
                }
                else
                    ViewMaxY -= dlt;
                ReDraw();
                CheckVScrollBar();
                CheckButtons();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btLyMinus_Click(object sender, EventArgs e)
        {
            try
            {
                float dlt = ViewMaxLy - ViewMinLy;
                dlt /= 10;
                if (CursorX > ViewMinLy && CursorX < ViewMaxLy)
                {
                    float w = (ViewMaxLy - ViewMinLy) / 2;
                    w += dlt;
                    ViewMinLy = CursorX - w;
                    ViewMaxLy = CursorX + w;
                    if (ViewMinLy < MinLy)
                        ViewMinLy = MinLy;
                    if (ViewMaxLy > MaxLy)
                        ViewMaxLy = MaxLy;
                }
                else
                {
                    ViewMaxLy += dlt;
                    if (ViewMaxLy > MaxLy)
                        ViewMaxLy = MaxLy;
                    ViewMinLy -= dlt;
                    if (ViewMinLy < MinLy)
                        ViewMinLy = MinLy;
                }
                CheckHScrollBar();
                CheckButtons();
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btYMinus_Click(object sender, EventArgs e)
        {
            try
            {
                float dlt = ViewMaxY - ViewMinY;
                dlt /= 10;
                if (CursorY > ViewMinY && CursorY < ViewMaxY)
                {
                    float w = (ViewMaxY - ViewMinY) / 2;
                    w += dlt;
                    ViewMinY = CursorY - w;
                    ViewMaxY = CursorY + w;
                    if (ViewMinY < MinY)
                        ViewMinY = MinY;
                    if (ViewMaxY > MaxY)
                        ViewMaxY = MaxY;
                }
                else
                {
                    ViewMaxY += dlt;
                    if (ViewMaxY > MaxY)
                    {
                        dlt = ViewMaxY - MaxY;
                        ViewMaxY = MaxY;
                        ViewMinY -= dlt;
                        if (ViewMinY < MinY)
                            ViewMinY = MinY;
                    }
                }
                ReDraw();
                CheckVScrollBar();
                CheckButtons();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void ShowAll()
        {
            btAll_Click(null, null);
        }

        private void btAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (MaxY == MaxLy && MinY == MinLy)
                    return;
                ViewMaxY = MaxY;
                ViewMaxLy = MaxLy;
                ViewMinY = MinY;
                ViewMinLy = MinLy;
                ReDraw();
                CheckVScrollBar();
                CheckHScrollBar();
                CheckButtons();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
        #endregion

        #region Scroll Bar functions....
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                float d = ViewMaxLy - ViewMinLy;
                ViewMinLy = hScrollBar1.Value;
                ViewMaxLy = hScrollBar1.Value + d;
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                float d = ViewMaxY - ViewMinY;
                ViewMinY = vScrollBar1.Maximum - vScrollBar1.LargeChange - vScrollBar1.Value + MinY;
                ViewMaxY = ViewMinY + d;
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void CheckView()
        {
            if (ViewMaxLy - ViewMinLy < 1)
                ViewMaxLy += 1;
            if (ViewMinLy < MinLy)
            {
                ViewMinLy = MinLy;
                if (ViewMaxLy - ViewMinLy < 1)
                    ViewMaxLy += 1;
            }
            if (ViewMaxLy > MaxLy)
            {
                ViewMaxLy = MaxLy;
                if (ViewMaxLy - ViewMinLy < 1)
                    ViewMinLy -= 1;
            }
        }

        void CheckHScrollBar()
        {
            try
            {
                CheckView();

                hScrollBar1.Minimum = (int)MinLy;
                hScrollBar1.Maximum = (int)MaxLy;
                hScrollBar1.Value = (int)ViewMinLy;

                hScrollBar1.LargeChange = (int)(ViewMaxLy - ViewMinLy);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
                ShowAll();
            }
            CheckViewSet();
        }

        void CheckButtons()
        {
            if (ViewMaxLy >= MaxLy && ViewMinLy <= MinLy)
                btLyMinus.Enabled = false;
            else
                btLyMinus.Enabled = true;

            /*if (ViewMaxY >= MaxY && ViewMinY <= MinY)
                btYMinus.Enabled = false;
            else
                btYMinus.Enabled = true;*/

            /*if (btYMinus.Enabled == false &&
                btLyMinus.Enabled == false)
                btAll.Enabled = false;
            else
                btAll.Enabled = true;*/
        }

        List<RectangleF> ViewHistory = new List<RectangleF>();
        int CurrentViewHistoryIndex = -1;
        void CheckViewSet()
        {
            try
            {
                if (ViewHistory.Count == 0)
                {
                    ViewHistory.Add(new RectangleF(ViewMinLy,ViewMinY,ViewMaxLy - ViewMinLy,ViewMaxY - ViewMinY));
                    CurrentViewHistoryIndex = 0;
                }
                else
                {
                    RectangleF cur = ViewHistory[CurrentViewHistoryIndex];
                    float eq_lim = 0.000001F;
                    if (Math.Abs(cur.X - ViewMinLy) > eq_lim && 
                        Math.Abs(cur.Y - ViewMinY) > eq_lim &&
                        Math.Abs(cur.X + cur.Width - ViewMaxLy) > eq_lim &&
                        Math.Abs(cur.Y + cur.Height - ViewMaxY) > eq_lim)
                    {
                        while (CurrentViewHistoryIndex != ViewHistory.Count - 1)
                            ViewHistory.RemoveAt(ViewHistory.Count - 1);
                        ViewHistory.Add(new RectangleF(ViewMinLy, ViewMinY, ViewMaxLy - ViewMinLy, ViewMaxY - ViewMinY));
                        CurrentViewHistoryIndex = ViewHistory.Count - 1;
                    }
                }
                btPrevView.Enabled = CurrentViewHistoryIndex > 0;
                btNextView.Enabled = CurrentViewHistoryIndex < (ViewHistory.Count - 1);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        void CheckVScrollBar()
        {
            try
            {
                vScrollBar1.Maximum = (int)(MaxY - MinY);
                vScrollBar1.Minimum = 0;
                vScrollBar1.LargeChange = (int)(ViewMaxY - ViewMinY);
                int val = (int)(vScrollBar1.Maximum - vScrollBar1.LargeChange - (ViewMinY - MinY));
                if (val < vScrollBar1.Minimum)
                    val = vScrollBar1.Minimum;
                if (val > vScrollBar1.Maximum)
                    val = vScrollBar1.Maximum;
                vScrollBar1.Value = val;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
                ShowAll();
            }
            CheckViewSet();
        }
        #endregion

        bool ShowGlobalPixelsPriv = false;
        public bool ShowGlobalPixels
        {
            get
            {
                return ShowGlobalPixelsPriv;
            }
            set
            {
                ShowGlobalPixelsPriv = value;
            }
        }
        float CurrentMouseX = 0, CurrentMouseY = 0;
        List<float> CurrentMousePixel = new List<float>();
        List<int> CurrentMouseSn = new List<int>();
        float PrevMouseX = 0, PrevMouseY = 0;
        bool DrawSelection = false;
        #region MouseFunctions
        private void DrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (KLy == 0 || KY == 0)
                    return;
                CurrentMouseX = (float)(e.X / KLy + ViewMinLy);
                CurrentMouseY = (float)((DrawPanel.Height - e.Y) / KY + ViewMinY);
                if (e.Button == 0)
                {
                    PrevMouseX = CurrentMouseX;
                    PrevMouseY = CurrentMouseY;
                    DrawSelection = false;
                    if (e.Y > DrawPanel.Height - 30 && SpLSelected != null &&
                        SpLSelected.Count != 0)
                    {
                        int selected = -1;
                        double selected_dlt = double.MaxValue;
                        int i = 0;
                        for (; i < SpLSelected.Count; i++)
                        {
                            LineDbRecord lr = Common.LDb.Data[SpLSelected[i]];
                            double xp = (lr.Ly - ViewMinLy) * KLy;
                            double dlt = Math.Abs(xp - e.X);
                            if (dlt < 5)
                                break;
                            {
                                if (selected_dlt > dlt)
                                {
                                    selected_dlt = dlt;
                                    selected = i;
                                }
                            }
                        }
                        for (; i < SpLSelected.Count; i++)
                        {
                            LineDbRecord lr = Common.LDb.Data[SpLSelected[i]];
                            double xp = (lr.Ly - ViewMinLy) * KLy;
                            double dlt = Math.Abs(xp - e.X);
                            if (dlt > 5)
                                break;
                            if (selected_dlt > dlt)
                            {
                                selected_dlt = dlt;
                                selected = i;
                            }
                        }

                        if (selected != SelectedSpLine)
                        {
                            SelectedSpLine = selected;
                            ReDraw();
                        }
                    }

                }
                else
                {
                    DrawSelection = true;
                    ReDraw();
                }
                CurLy = CurrentMouseX;//lbLyInfo.Text = ""+Math.Round(CurrentMouseX,2).ToString();
                lbYInfo.Text = "y"+serv.GetGoodValue(CurrentMouseY,1).ToString();
                CurrentMousePixel.Clear();
                CurrentMouseSn.Clear();
                if (SpInfo.Count > 0)
                {
                    Dispers d = SpInfo[0].Sp.GetCommonDispers();
                    List<int> sn = d.FindSensors(CurrentMouseX);
                    if (sn.Count > 0)
                    {
                        lbSNInfo.Text = "c№"+(sn[0] + 1);
                        //CurrentMousePixel.Add((float)d.GetGlobalPixelByLy(sn[0], CurrentMouseX));
                        if(ShowGlobalPixelsPriv)
                            CurrentMousePixel.Add((float)d.GetPixelThr(CurrentMouseX));// .GetGlobalPixelByLy(sn[0], CurrentMouseX));
                        else
                            CurrentMousePixel.Add((float)d.GetGlobalPixelByLy(sn[0], CurrentMouseX));
                        CurrentMouseSn.Add(sn[0]);
                        if(ShowGlobalPixelsPriv)
                            lbNInfo.Text = "g";
                        else
                            lbNInfo.Text = "n";
                        lbNInfo.Text += Math.Round(CurrentMousePixel[0], 1).ToString();
                        if (ShowGlobalPixelsPriv)
                            lbNInfo.Text += "(" + (int)d.GetGlobalPixelByLy(sn[0], CurrentMouseX) + ")";
                        else
                            lbNInfo.Text += "(" + (int)d.GetLocalPixelByLy(sn[0], CurrentMouseX) + ")";
                        for (int i = 1; i < sn.Count; i++)
                        {
                            CurrentMouseSn.Add(sn[i]);
                            CurrentMousePixel.Add((float)d.GetGlobalPixelByLy(sn[i], CurrentMouseX));
                            //CurrentMousePixel.Add((float)d.GetPixelThr(CurrentMouseX));
                            lbSNInfo.Text += " " + (sn[i] + 1).ToString();
                            lbNInfo.Text += " " + Math.Round(CurrentMousePixel[i], 0).ToString();
                        }
                    }
                    else
                    {
                        lbSNInfo.Text = "-";
                        lbNInfo.Text = "-";
                    }
                }
                else
                {
                    lbSNInfo.Text = "-";
                    lbNInfo.Text = "-";
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void DrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void DrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    cmMain.Show(this, e.X, e.Y);
                    return;
                }
                if (DrawSelection == false || Math.Abs(PrevMouseX - CurrentMouseX) < 0.01 ||
                    Math.Abs(PrevMouseY - CurrentMouseY) < 0.01)
                {
                    CursorX = PrevMouseX;
                    CursorY = PrevMouseY;
                    CursorPixel.Clear();
                    CursorSn.Clear();
                    for (int i = 0; i < CurrentMousePixel.Count; i++)
                    {
                        CursorPixel.Add(CurrentMousePixel[i]);
                        CursorSn.Add(CurrentMouseSn[i]);
                    }
                    Focus();
                    try
                    {
                        if (CursorListener != null)
                            CursorListener(CursorX, CursorY);
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                    }
                }
                else
                {
                    float minx = PrevMouseX;
                    float maxx = CurrentMouseX;
                    float miny = PrevMouseY;
                    float maxy = CurrentMouseY;

                    if (minx > maxx)
                    {
                        float tmp = maxx;
                        maxx = minx;
                        minx = tmp;
                    }
                    if (miny > maxy)
                    {
                        float tmp = maxy;
                        maxy = miny;
                        miny = tmp;
                    }

                    ViewMinLy = minx;
                    ViewMaxLy = maxx;
                    ViewMinY = miny;
                    ViewMaxY = maxy;

                    if (ViewMinLy < MinLy)
                        ViewMinLy = MinLy;
                    if (ViewMaxLy > MaxLy)
                        ViewMaxLy = MaxLy;
                    if (ViewMinY < MinY)
                        ViewMinY = MinY;
                    if (ViewMaxY > MaxY)
                        ViewMaxY = MaxY;

                    PrevMouseX = CurrentMouseX;
                    PrevMouseY = CurrentMouseY;
                    DrawSelection = false;

                    CheckVScrollBar();
                    CheckHScrollBar();
                    CheckButtons();
                }
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void DrawPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                float dx = ViewMaxLy - ViewMinLy;
                float dy = ViewMaxY - ViewMinLy;
                float dx_left = CurrentMouseX - ViewMinY;
                float dx_right = ViewMaxY - CurrentMouseX;

                if (e.Delta > 0)
                {
                    ViewMinLy += dx_left / 10;
                    ViewMaxLy -= dx_left / 10;
                }
                else
                {
                    ViewMinLy -= dx_left / 10;
                    ViewMaxLy += dx_left / 10;
                }

                if (ViewMinLy < MinLy)
                    ViewMinLy = MinLy;
                if (ViewMaxLy > MaxLy)
                    ViewMaxLy = MaxLy;
                if (ViewMinY < MinY)
                    ViewMinY = MinY;
                if (ViewMaxY > MaxY)
                    ViewMaxY = MaxY;

                CheckVScrollBar();
                CheckHScrollBar();
                CheckButtons();
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
        #endregion

        LDbQuery SpLQuery = null;
        List<int> SpLSelected;
        LDbQuery SpLLineQuery = null;
        List<int> SpLLineSelected;
        private void btShowLib_Click(object sender, EventArgs e)
        {
            try
            {
                if (SpLQuery == null)
                    SpLQuery = new LDbQuery();
                DialogResult dr = SpLQuery.ShowDialog(MainForm.MForm);
                if (dr == DialogResult.Cancel)
                    return;
                if (dr == DialogResult.OK)
                    SpLSelected = SpLQuery.GetSelection();
                else
                    SpLSelected = null;
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbAllSpectrs_CheckedChanged(object sender, EventArgs e)
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

        private void btAskLine_Click(object sender, EventArgs e)
        {
            try
            {
                if (CursorX <= 0 || SpInfo.Count == 0)
                {
                    MessageBox.Show(serv.FindParentForm(this),
                        Common.MLS.Get(MLSConst,"Не указано вокруг какой длины волны необходимо просмотреть линии."),
                        Common.MLS.Get(MLSConst,"Предупреждение"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    return;
                }
                if (SpLLineQuery == null)
                    SpLLineQuery = new LDbQuery();

                SpLLineQuery.SetupLyAsk(CursorX,SpInfo[0].GetPixelSize(CursorX)*2);
                DialogResult dr = SpLLineQuery.ShowDialog(MainForm.MForm);
                if (dr == DialogResult.Cancel)
                    return;
                if (dr == DialogResult.OK)
                    SpLLineSelected = SpLLineQuery.GetSelection();
                else
                    SpLLineSelected = null;
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void chbAutoZoom_CheckedChanged(object sender, EventArgs e)
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

        private void lbSNInfo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (SpInfo.Count == 0 || spDataView.Visible == true)
                {
                    spDataView.Visible = false;
                    return;
                }
                spDataView.Dock = DockStyle.Fill;
                spDataView.SetupSpectr(SpInfo[0].Sp);
                spDataView.Visible = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void Shift(float step)//int dir)
        {
            bool auto_zoom = DrawAutoZoomY;
            DrawAutoZoomY = false;
            try
            {
                if (FullSpInfo.Count == 0)
                    return;
                //FullSpInfo[0].ShiftDispers((float)(nmStepSize.Value)*dir);
                FullSpInfo[0].ShiftDispers(step);
                FullSpInfo[0].Save();
                ReLoadSpectr(FullSpInfo[0], 0);
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            DrawAutoZoomY = auto_zoom;
        }

        private void cmMainSmooth_Click(object sender, EventArgs e)
        {
            try
            {
                ReDraw();
            }
            catch
            {
            }
        }

        private void cmMainShiftLeft01_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(-0.1F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftRight01_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(0.1F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btPrevView_Click(object sender, EventArgs e)
        {
            try
            {
                if (MaxY == MaxLy && MinY == MinLy)
                    return;

                CurrentViewHistoryIndex--;

                RectangleF f = ViewHistory[CurrentViewHistoryIndex];
                ViewMaxY = f.Y+f.Height;
                ViewMaxLy = f.X+f.Width;
                ViewMinY = f.Y;
                ViewMinLy = f.X;
                ReDraw();
                CheckVScrollBar();
                CheckHScrollBar();
                CheckButtons();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void btNextView_Click(object sender, EventArgs e)
        {
            try
            {
                if (MaxY == MaxLy && MinY == MinLy)
                    return;

                CurrentViewHistoryIndex++;

                RectangleF f = ViewHistory[CurrentViewHistoryIndex];
                ViewMaxY = f.Y + f.Height;
                ViewMaxLy = f.X + f.Width;
                ViewMinY = f.Y;
                ViewMinLy = f.X;
                ReDraw();
                CheckVScrollBar();
                CheckHScrollBar();
                CheckButtons();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void cmMainShiftRight05_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(0.5F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftRight10_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(1F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftLeft05_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(-0.5F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftLeft10_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(-1F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        OpenFileDialog OFD = new OpenFileDialog();
        Spectr SpAddone;
        SpectrViewInfo SpAddoneView;
        private void cmMainAddSpectr_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = OFD.ShowDialog();
                if (dr != DialogResult.OK)
                    return;
                string file_name = OFD.FileName;
                if (file_name == null)
                    return;
                SpAddone = new Spectr(file_name);
                SpAddoneView = new SpectrViewInfo(SpAddone, "Эталон");
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainRemoveSpectr_Click(object sender, EventArgs e)
        {
            try
            {
                SpAddone = null;
                SpAddoneView = null;
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftRight20_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(2F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftRight50_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(5F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftLeft20_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(-2F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainShiftLeft50_Click(object sender, EventArgs e)
        {
            try
            {
                Shift(-5F);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        string LyTxt;
        private double CurLy
        {
            set
            {
                LyTxt = "" + Math.Round(value, 2).ToString();
                lbLyInfo.Text = LyTxt;
            }
        }

        private void lbLyInfo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    CursorX = (float)serv.ParseDouble(lbLyInfo.Text);
                    btZ_Click(this, null);
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainSetAnalit_Click(object sender, EventArgs e)
        {
            try
            {
                if (SetLyListenerPriv != null)
                    SetLyListenerPriv(true, CursorX);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void cmMainSetCompare_Click(object sender, EventArgs e)
        {
            try
            {
                if (SetLyListenerPriv != null)
                    SetLyListenerPriv(false, CursorX);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    class SpectrViewInfo
    {
        public int[][] SpData;
        public float[][] Lys;
        public string Name;
        //public float MinLy = 0,//float.MaxValue,
        //    MaxLy = 30000,//-float.MaxValue,
        public float MinLy = float.MaxValue,
            MaxLy = -float.MaxValue,
            MinY = float.MaxValue,
            MaxY = 3500;//-float.MaxValue;
        public Spectr Sp;

        public bool IsDispersDefault()
        {
            //SpectrDataView view = Sp.GetDefultView();
            Dispers disp = Sp.GetCommonDispers();// view.GetDispersRO();
            return disp.IsDefaultDisp();
        }

        public double GetPixelSize(double ly)
        {
            Dispers disp = Sp.GetCommonDispers();
            List<int> snl = disp.FindSensors(ly);
            if (snl.Count == 0)
                return 0.1;
            int sn = snl[0];
            double pix = disp.GetGlobalPixelByLy(sn, ly);
            double ly1 = disp.GetLyByGlobalPixel(sn, pix);
            double ly2 = disp.GetLyByGlobalPixel(sn, pix+1);
            return ly2 - ly1;
        }

        int[][][] FullViewData = null;
        int[][][] FullShotData = null;
        int[] FullShortIndexes;
        bool HasNull = false;
        public int GetShotCount()
        {
            if (FullShotData == null)
            {
                FullShortIndexes = Sp.GetShotIndexes();
                if (FullShortIndexes.Length == 0)
                {
                    FullShortIndexes = new int[Sp.GetViewsSet().Count];
                    for (int i = 0; i < FullShortIndexes.Length; i++)
                        FullShortIndexes[i] = i;
                    HasNull = false;
                }
                else
                    HasNull = true;
                FullShotData = new int[FullShortIndexes.Length][][];
                FullViewData = new int[Sp.GetViewsSet().Count][][];
            }
            return FullShotData.Length;
        }

        public int[][] GetViewData(int index)
        {
            if (index < 0 || index >= FullViewData.Length)
                return null;

            if (FullViewData == null)
                GetShotCount();

            if (FullViewData[index] == null)
            {
                SpectrDataView sig = Sp.GetViewsSet()[index];
                int sn = sig.GetSensorCount();
                int[][] ret = new int[sn][];
                for (int s = 0; s < sn; s++)
                {
                    int ss = sig.GetSensorSize(s);
                    ret[s] = new int[ss];
                    for (int i = 0; i < ss; i++)
                        ret[s][i] = (int)(sig[s, i]);
                }
                FullViewData[index] = ret;
            }

            return FullViewData[index];
        }

        public int[][] GetShotData(int index)
        {
            if (FullShotData[index] == null)
            {
                SpectrDataView sig = Sp.GetViewsSet()[FullShortIndexes[index]];
                SpectrDataView nul;
                if (HasNull)
                    nul = Sp.GetNullFor(FullShortIndexes[index]);
                else
                    nul = null;
                int sn = sig.GetSensorCount();
                int[][] ret = new int[sn][];
                for (int s = 0; s < sn; s++)
                {
                    int ss = sig.GetSensorSize(s);
                    ret[s] = new int[ss];
                    if(nul != null)
                        for (int i = 0; i < ss; i++)
                            ret[s][i] = (int)(sig[s, i] - nul[s, i]);
                    else
                        for (int i = 0; i < ss; i++)
                            ret[s][i] = (int)sig[s, i];
                }
                FullShotData[index] = ret;
            }

            return FullShotData[index];
        }

        public SpectrViewInfo(Spectr spectr, string name)
        {
            Sp = spectr;
            SpectrDataView view = spectr.GetDefultView();
            int sn = view.GetSensorCount();
            SpData = new int[sn][];
            Lys = new float[sn][];
            Name = (string)(name.Clone());
            Dispers disp = Sp.GetCommonDispers();
            for (int s = 0; s < sn; s++)
            {
                int ss = view.GetSensorSize(s);
                SpData[s] = new int[ss];
                Lys[s] = new float[ss];
                for (int i = 0; i < ss; i++)
                {
                    SpData[s][i] = (int)view[s, i];
                    Lys[s][i] = (float)disp.GetLyByLocalPixel(s, i);//( view.GetLy(s, i);
                    if (view[s, i] < float.MaxValue)
                    {
                        if (MinY > SpData[s][i]) MinY = SpData[s][i];
                        if (MaxY < SpData[s][i]) MaxY = SpData[s][i];
                    }
                    else
                        SpData[s][i] = int.MaxValue;
                    if (MinLy > Lys[s][i])      MinLy = Lys[s][i];
                    if (MaxLy < Lys[s][i])      MaxLy = Lys[s][i];
                }

                /*List<SpectrDataView> views = spectr.GetViewsSet();
                for (int v = 0; v < views.Count; v++)
                {
                    view = Sp.GetViewsSet()[v];
                    for (int i = 0; i < ss; i++)
                    {
                        float tmp = view[s, i];
                        if (MinY > tmp) 
                            MinY = tmp;
                        if (MaxY < tmp) 
                            MaxY = tmp;
                    }
                }*/
            }
        }
    }
}
