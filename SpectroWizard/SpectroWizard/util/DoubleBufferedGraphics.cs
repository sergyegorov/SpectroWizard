using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace SpectroWizard.util
{
    public delegate void PaintDel(Graphics g);
    public class DoubleBufferedGraphics
    {
        Panel P;
        Thread Th;
        PaintDel PDel;
        Control IForm;
        public DoubleBufferedGraphics(Control invoke_form,Panel p,PaintDel pdel)
        {
            P = p;
            PDel = pdel;
            IForm = invoke_form;
            P.SizeChanged += new EventHandler(P_SizeChanged);
            P.VisibleChanged += new EventHandler(P_VisibleChanged);
            CheckBmps();
            Th = new Thread(new ThreadStart(ReDrawThread));
            Th.Start();
            //P.Paint += new PaintEventHandler(P_Paint);
        }

        Graphics G1 = null,G2 = null;
        Bitmap Bmp1 = null;
        Bitmap Bmp2 = null;
        int Cycle;
        Bitmap Bmp
        {
            get{
                if((Cycle&1) == 0)
                    return Bmp1;
                else
                    return Bmp2;
            }
        }
        Graphics G
        {
            get{
                if((Cycle&1) == 0)
                    return G2;
                else
                    return G1;
            }
        }

        void CheckBmps()
        {
            if (Bmp1 == null || Bmp1.Width != P.Width || Bmp1.Height != P.Height)
            {
                int w = P.Width;
                int h = P.Height;
                Bmp1 = new Bitmap(w, h);
                Bmp2 = new Bitmap(w, h);
                G1 = Graphics.FromImage(Bmp1);
                G2 = Graphics.FromImage(Bmp2);
            }
        }

        long LastReDrawReQuest = 0;
        long LastReDraw = 0;
        void ReDrawThread()
        {
            while (P.IsDisposed == false && Common.IsRunning)
            try
            {
                while (P.IsDisposed == false && LastReDrawReQuest == 0 &&
                    Common.IsRunning)
                    Thread.Sleep(50);
                while (P.IsDisposed == false && 
                    (DateTime.Now.Ticks - LastReDrawReQuest) < 2000000 && 
                    Common.IsRunning &&
                    (DateTime.Now.Ticks - LastReDraw) < 10000000)
                    Thread.Sleep(10);
                if (P.IsDisposed == false && Common.IsRunning == false)
                    return;
                LastReDrawReQuest = 0;
                try
                {
                    LastReDrawReQuest = 0;
                    Graphics g = G;
                    while (P.IsDisposed == false &&
                        Common.IsRunning == false &&
                        P.IsAccessible == false)
                        Thread.Sleep(1);
                    g.ResetClip();
                    g.ResetTransform();
                    object[] prams = { g };
                    if (IForm.IsHandleCreated)
                        IForm.Invoke(PDel, prams);
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
                finally
                {
                    LastReDraw = DateTime.Now.Ticks;
                }
                try
                {
                    Cycle ++;
                    P.BackgroundImage = Bmp;
                    P.Invalidate();
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public void ReDraw()
        {
            ReDraw(false);
        }

        public void ReDraw(bool now)
        {
            if (LastReDrawReQuest == 0)
                LastReDraw = DateTime.Now.Ticks;
            if (now)
                LastReDraw = 0;
            LastReDrawReQuest = DateTime.Now.Ticks;
        }

        void P_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (P.Visible == false)
                    return;
                CheckBmps();
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void P_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (P.Width == 0 || P.Height == 0 ||
                    P.Visible == false)
                    return;
                CheckBmps();
                ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
