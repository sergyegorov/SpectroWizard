using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using SpectroWizard.analit.fk;
using SpectroWizard.gui.tasks;

namespace SpectroWizard.data
{
    public class Dispers : TestInterf
    {
        public const string MLSConst = "Dispers";
        Function[] Functions = null;
        int[] SSizes = null;

        public double[] GetK(int function)
        {
            return Functions[function].GetK();
        }

        public bool IsDefaultDisp()
        {
            for (int i = 0; i < Functions.Length; i++)
                if (Functions[i].IsDefault() == false)
                    return false;
            return true;
        }

        public override ulong GetType()
        {
            return TestInterf.IsFunctionalTest;
        }

        public List<int> FindSensors(double ly)
        {
            //Common.ProfileStart("FS");    
            List<int> ret = new List<int>();
            for (int s = 0; s < SSizes.Length; s++)
                try
                {
                    double from = Functions[s].CalcY(0 + BasePixels[s]);
                    double to = Functions[s].CalcY(SSizes[s] + BasePixels[s]);
                    if (ly < from || 
                        ly > to)
                        continue;
                    double val = Functions[s].CalcX(ly) - BasePixels[s];
                    if (val >= 0 && val < SSizes[s])
                        ret.Add(s);
                }
                catch{}

            //Common.ProfileEnd("FS");
            return ret;
        }


        public double GetPixelThr(double ly)
        {
            List<int> sn = FindSensors(ly);
            if (sn == null || sn.Count == 0)
                return -1;
            double pixel = GetGlobalPixelByLy(sn[0], ly);
            pixel -= BasePixelsTh[sn[0]];
            return pixel;
        }

        public double GetShiftForSensor(int num)
        {
            return Functions[num].GetShift();
        }

        public override string GetName()
        {
            return "Dispers prsing";
        }

        public void ApplyShifts(double sh,int sn)
        {
            if (ReadOnly)
                throw new Exception("Dispers Read Only");
            Functions[sn].ApplyShift(sh);
        }

        public void AddShift(double sh, int sn)
        {
            if (ReadOnly)
                throw new Exception("Dispers Read Only");
            Functions[sn].ApplyShift(Functions[sn].GetShift() + sh);
        }

        public override bool Run(out string log)
        {
            log = "";
            try
            {
                #region testing...
                Dispers disp = new Dispers();
                int test = 1;
                string tmps;
                int tmpi;
                int[] ss = Common.Dev.Reg.GetSensorSizes();
                bool result;

                log += "Test#" + test + " Comments and empty strings...";
                disp.Compile("#comment", false, out result);
                if (disp.Errors != null)
                {
                    log += Common.MLS.Get(MLSConst, "Comment error:") + disp.Errors + serv.Endl;
                    return false;
                }
                log += "Ok" + serv.Endl;
                test++;

                log += "Test#" + test + " Empty strings...";
                disp.Compile("", false, out result);
                if (disp.Errors != null)
                {
                    log += Common.MLS.Get(MLSConst, "Empty string error:") + disp.Errors + serv.Endl;
                    return false;
                }
                log += "Ok" + serv.Endl;
                test++;

                for (int i = 0; i < 8; i++)
                {
                    log += "Test#" + test + " Comments and empty strings...";
                    tmps = "";
                    tmpi = i;
                    for (int j = 0; j < 3; j++, tmpi >>= 1)
                        if ((tmpi & 1) == 0)
                            tmps += serv.Endl;
                        else
                            tmps += "#comments" + serv.Endl;
                    disp.Compile(tmps, false, out result);
                    if (disp.Errors != null)
                    {
                        log += Common.MLS.Get(MLSConst, "Comment and empty string error:") + disp.Errors + serv.Endl;
                        return false;
                    }
                    log += "Ok" + serv.Endl;
                    test++;
                }

                log += "Test#" + test + " Simple setup 1 sensor";
                tmps = "#comment" + serv.Endl+
                    "s1:1" + serv.Endl+
                    "10-100" + serv.Endl+
                    "  20 - 200 " + serv.Endl;
                disp.Compile(tmps, false, out result);
                if (disp.Errors != null)
                {
                    log += Common.MLS.Get(MLSConst, "Parsing erros") + disp.Errors + serv.Endl;
                    return false;
                }
                log += "Ok" + serv.Endl;
                test++;

                log += "Test#" + test + " Simple setup 2 sensor";
                tmps = "#comment" + serv.Endl +
                    "s1:1" + serv.Endl +
                    "10-10" + serv.Endl +
                    "  20 - 20 " + serv.Endl + serv.Endl +
                    "s2:2" + serv.Endl +
                    " " + (ss[0] + 2) + "-" + (ss[0] + 2) + serv.Endl +
                    " " + (ss[0] + 200) + "-" + (ss[0] + 200) + serv.Endl;
                disp.Compile(tmps, false, out result);
                if (disp.Errors != null)
                {
                    log += Common.MLS.Get(MLSConst, "Parsing erros") + disp.Errors + serv.Endl;
                    return false;
                }
                log += "Ok" + serv.Endl;
                test++;

                log += "Test#" + test + " Simple setup 3 sensor";
                tmps = "#comment" + serv.Endl +
                    "s1:1" + serv.Endl +
                    "10-10" + serv.Endl +
                    "  20 - 20 " + serv.Endl + serv.Endl +
                    "s2:2" + serv.Endl +
                    " " + (ss[0] + 2) + "-" + (ss[0] + 2) + serv.Endl +
                    " " + (ss[0] + 200) + "-" + (ss[0] + 200) + serv.Endl+
                    "s3:3" + serv.Endl +
                    " " + (ss[1] + ss[0] + 3) + "-" + (ss[1] + ss[0] + 3) + serv.Endl +
                    " " + (ss[1] + ss[0] + 300) + "-" + (ss[1] + ss[0] + 300) + serv.Endl;
                disp.Compile(tmps, false, out result);
                if (disp.Errors != null)
                {
                    log += Common.MLS.Get(MLSConst, "Parsing erros:")+ disp.Errors + serv.Endl;
                    return false;
                }
                log += "Ok"+serv.Endl;
                test++;

                log += "Test#" + test + " Wrong interpolation type";
                tmps = "#comment" + serv.Endl +
                    "s1:4" + serv.Endl +
                    "10-10" + serv.Endl +
                    "  20 - 20 " + serv.Endl + serv.Endl +
                    "s2:2" + serv.Endl +
                    " " + (ss[0] + 2) + "-" + (ss[0] + 2) + serv.Endl +
                    " " + (ss[0] + 200) + "-" + (ss[0] + 200) + serv.Endl +
                    "s3:3" + serv.Endl +
                    " " + (ss[1] + ss[0] + 3) + "-" + (ss[1] + ss[0] + 3) + serv.Endl +
                    " " + (ss[1] + ss[0] + 300) + "-" + (ss[1] + ss[0] + 300) + serv.Endl;
                disp.Compile(tmps, false, out result);
                if (disp.Errors == null)
                {
                    log += Common.MLS.Get(MLSConst, "No error found.") + serv.Endl;
                    return false;
                }
                log += " Found problem: '" + disp.Errors + "' - Ok" + serv.Endl;
                test++;

                log += "Test#" + test + " Wrong pixel interval";
                tmps = "#comment" + serv.Endl +
                    "s1:1" + serv.Endl +
                    "10-10" + serv.Endl +
                    "  20 - 20 " + serv.Endl + serv.Endl +
                    "s2:2" + serv.Endl +
                    " " + (ss[0] - 2) + "-" + (ss[0] - 2) + serv.Endl +
                    " " + (ss[0] + 200) + "-" + (ss[0] + 200) + serv.Endl +
                    "s3:3" + serv.Endl +
                    " " + (ss[1] + ss[0] + 3) + "-" + (ss[1] + ss[0] + 3) + serv.Endl +
                    " " + (ss[1] + ss[0] + 300) + "-" + (ss[1] + ss[0] + 300) + serv.Endl;
                disp.Compile(tmps, false, out result);
                if (disp.Errors == null)
                {
                    log += Common.MLS.Get(MLSConst, "No error found.") + serv.Endl;
                    return false;
                }
                log += " Found problem: '" + disp.Errors + "' - Ok" + serv.Endl;
                test++;

                log += "Test#" + test + " Links is to close";
                tmps = "#comment" + serv.Endl +
                    "s1:1" + serv.Endl +
                    "10-10" + serv.Endl +
                    "  20 - 20 " + serv.Endl + serv.Endl +
                    "s2:2" + serv.Endl +
                    " " + (ss[0] + 2) + "-" + (ss[0] + 2) + serv.Endl +
                    " " + (ss[0] + 3) + "-" + (ss[0] + 3) + serv.Endl +
                    "s3:3" + serv.Endl +
                    " " + (ss[1] + ss[0] + 3) + "-" + (ss[1] + ss[0] + 3) + serv.Endl +
                    " " + (ss[1] + ss[0] + 300) + "-" + (ss[1] + ss[0] + 300) + serv.Endl;
                disp.Compile(tmps, false, out result);
                if (disp.Errors == null)
                {
                    log += Common.MLS.Get(MLSConst, "No error found.") + serv.Endl;
                    return false;
                }
                log += " Found problem: '" + disp.Errors + "' - Ok" + serv.Endl;
                test++;

                log += "Test#" + test + " Wrong sensor number";
                tmps = "#comment" + serv.Endl +
                    "s1:1" + serv.Endl +
                    "10-10" + serv.Endl +
                    "  20 - 20 " + serv.Endl + serv.Endl +
                    "s2:2" + serv.Endl +
                    " " + (ss[0] + 2) + "-" + (ss[0] + 2) + serv.Endl +
                    " " + (ss[0] + 200) + "-" + (ss[0] + 200) + serv.Endl +
                    "s3:3" + serv.Endl +
                    " " + (ss[1] + ss[0] + 3) + "-" + (ss[1] + ss[0] + 3) + serv.Endl +
                    " " + (ss[1] + ss[0] + 300) + "-" + (ss[1] + ss[0] + 300) + serv.Endl+
                    "s100000:3" + serv.Endl +
                    " " + (ss[1] + ss[0] + 3) + "-" + (ss[1] + ss[0] + 3) + serv.Endl +
                    " " + (ss[1] + ss[0] + 300) + "-" + (ss[1] + ss[0] + 300) + serv.Endl;
                disp.Compile(tmps, false, out result);
                if (disp.Errors == null)
                {
                    log += Common.MLS.Get(MLSConst, "No error found.") + serv.Endl;
                    return false;
                }
                log += " Found problem: '" + disp.Errors + "' - Ok" + serv.Endl;
                test++;

                string tt = "#c"+serv.Endl+"s1:1"+serv.Endl+"10-20";
                bool[] ttf = {true, true, true, true, true,
                             false, false, false, true, true, true,
                             false, false, false, true, true, true, true};
                tmps = "";
                for (int i = 0; i < tt.Length; i++)
                {
                    log += "Test#" + test + " Typing test...";
                    disp.Compile(tmps, false, out result);
                    if (ttf[i])
                    {
                        if (disp.Errors != null)
                        {
                            log += Common.MLS.Get(MLSConst, "Error found:")+disp.Errors + serv.Endl;
                            return false;
                        }
                        log += "  - Ok" + serv.Endl;
                    }
                    else
                    {
                        if (disp.Errors == null)
                        {
                            log += Common.MLS.Get(MLSConst, "No error found.") + serv.Endl;
                            return false;
                        }
                        log += " Found problem: '" + disp.Errors + "' - Ok" + serv.Endl;
                    }
                    test++;
                    tmps += tt[i];
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                log += "Fatal Error: " + ex + serv.Endl;
                return false;
            }
        }

        public int[] GetSensorSizes()
        {
            return (int[])SSizes.Clone();
        }

        void InitBasePixels()
        {
            BasePixels = new int[SSizes.Length];
            int bp = 0;
            for (int i = 0; i < SSizes.Length; i++)
            {
                BasePixels[i] = bp;
                bp += SSizes[i];
            }

            BasePixelsTh = new float[SSizes.Length];
            float bpf = 0;
            for (int i = 1; i < SSizes.Length; i++)
            {
                float ly = (float)GetLyByLocalPixel(i - 1, SSizes[i - 1]);
                bpf += (float)GetLocalPixelByLy(i, ly);
                BasePixelsTh[i] = bpf;
            }
        }
        int[] BasePixels = null;
        float[] BasePixelsTh = null;

        public void SetupDefaultDisp()
        {
            if (ReadOnly)
                throw new Exception("Dispers Read Only");
            int[] ss = Common.Dev.Reg.GetSensorSizes();
            Functions = new Function[ss.Length];
            SSizes = (int[])ss.Clone();
            int bs = 0;
            for (int i = 0; i < Functions.Length; i++)
            {
                double[] n = {bs,bs+1000};
                bool[] en = {true,true};
                Functions[i] = new Function(Function.Types.Line, n, n, en, false, false, 0.3);
                bs += ss[i];
            }
            InitBasePixels();
        }

        public Dispers()
        {
            SetupDefaultDisp();
        }

        public Dispers(string links)
        {
            SetupDefaultDisp();
            bool rez;
            Compile(links,true,out rez);
        }

        bool ReadOnly = false;
        public Dispers(Dispers disp,bool read_only)
        {
            ReadOnly = read_only;
            Functions = new Function[disp.Functions.Length];
            SSizes = (int[])disp.SSizes.Clone();
            for (int i = 0; i < Functions.Length; i++)
                Functions[i] = new Function(disp.Functions[i]);
            InitBasePixels();
        }

        public double GetGlobalPixelByLy(int sn, double ly)
        {
            return Functions[sn].CalcX(ly);
        }

        public double GetLocalPixelByLy(int sn, double ly)
        {
            return Functions[sn].CalcX(ly) - BasePixels[sn];
        }

        public double GetLyByGlobalPixel(int sn, double global_pixels)
        {
            return Functions[sn].CalcY(global_pixels);
        }

        public double GetLyByLocalPixel(int sn, double local_pixels)
        {
            return Functions[sn].CalcY(local_pixels+BasePixels[sn]);
        }

        public string Errors;
        public class LinkInfo
        {
            public float Pixel, Ly;
            public int Sn;
            public float Dlt = 0;
            public int SrcLine;
            public LinkInfo(double pixel, double ly, int sn,
                int src_line)
            {
                Pixel = (float)pixel;
                Ly = (float)ly;
                Sn = sn;
                SrcLine = src_line;
            }

            public ulong CalcHash()
            {
                ulong ret = (ulong)(Pixel * 10 + Ly * 100 + Sn + Pixel * 3 + Ly * 5 + Sn * 5);
                return ret;
            }
        }

        public List<LinkInfo> Compile(string text_src,bool apply, out bool rez)
        {
            return Compile(text_src, apply, out rez, true);
        }

        public List<LinkInfo> Compile(string text_src,bool apply, out bool rez,bool check_for_outpixel)
        {
            string text = text_src + "  \n   ";
            #region compilatoin proc
            List<LinkInfo> ret = new List<LinkInfo>();
            string line = "";
            rez = false;
            try
            {
                SetupDefaultDisp();
                Errors = null;
                int ind = 0;
                line = serv.GetLine(text, ref ind);
                int line_count = 1;
                int[] ss = Common.Dev.Reg.GetSensorSizes();
                List<double>[] links_pixel = new List<double>[ss.Length];
                List<double>[] links_ly = new List<double>[ss.Length];
                List<int>[] links_i = new List<int>[ss.Length];
                int[] polinom = new int[ss.Length];
                int[] base_pixels = new int[ss.Length];
                int base_cur = 0;
                for (int i = 0; i < ss.Length; i++)
                {
                    links_pixel[i] = new List<double>();
                    links_ly[i] = new List<double>();
                    links_i[i] = new List<int>();
                    base_pixels[i] = base_cur;
                    base_cur += ss[i];
                }
                int sn = 0;
                while (line != null)
                {
                    line = serv.RemoveAllAfter(line, '#');
                    line = serv.RemoveSpaces(line);
                    //line = serv.RemoveControlSym(line);
                    if (line.Length == 0)
                    {
                        line = serv.GetLine(text, ref ind);
                        line_count++;
                        continue;
                    }
                    if (line[0] == 's')
                    {
                        string num1 = "";
                        int j = 1;
                        for (; j < line.Length && char.IsDigit(line[j]); j++)
                            num1 += line[j];
                        try
                        {
                            sn = (int)(serv.ParseDouble(num1) - 1);
                        }
                        catch
                        {
                            Errors = Common.MLS.Get(MLSConst, "Can't parse sensor value. In line:") + " '" + line + "'";
                            return ret;
                        }
                        if (sn >= ss.Length)
                        {
                            Errors = Common.MLS.Get(MLSConst, "There is no sensor with the folloing index:") + " '" + line + "'";
                            return ret;
                        }
                        if (j == line.Length || line[j] != ':')
                        {
                            Errors = Common.MLS.Get(MLSConst, "No symbol ':' after sensor number definishion:") + " '" + line + "'";
                            return ret;
                        }
                        j++;
                        if (j >= line.Length)
                        {
                            Errors = Common.MLS.Get(MLSConst, "Can't find polinom type. In line:") + " '" + line + "'";
                            return ret;
                        }
                        num1 = "" + line[j];
                        try
                        {
                            polinom[sn] = (int)(serv.ParseDouble(num1));
                        }
                        catch
                        {
                            Errors = Common.MLS.Get(MLSConst, "Can't parse polinom number. In line:") + " '" + line + "'";
                            return ret;
                        }
                        if (polinom[sn] > 3)
                        {
                            Errors = Common.MLS.Get(MLSConst, "Unsuported polinom index. In line:") + " '" + line + "'";
                            return ret;
                        }
                    }
                    else
                    {
                        string num2 = "";
                        int j = 0;
                        for (; j < line.Length && (char.IsDigit(line[j]) || line[j] == ',' || line[j] == '.'); j++)
                            num2 += line[j];
                        double pixel, ly;
                        try
                        {
                            pixel = serv.ParseDouble(num2);
                        }
                        catch
                        {
                            Errors = Common.MLS.Get(MLSConst, "Can't parse pixel value. In line:") + " '" + line + "'";
                            return ret;
                        }
                        if (check_for_outpixel)
                        try
                        {
                            if (pixel < base_pixels[sn] || pixel >= base_pixels[sn] + ss[sn])
                            {
                                Errors = Common.MLS.Get(MLSConst, "Pixels value out of range! In line:") + " '" + line + "' " +
                                    Common.MLS.Get(MLSConst, "Must to be:") + base_pixels[sn] + " " + (base_pixels[sn] + ss[sn]);
                                return ret;
                            }
                        }
                        catch
                        {
                            Errors = Common.MLS.Get(MLSConst, "Can't parse pixel value. In line:") + " '" + line + "'";
                            return ret;
                        }
                        if (j == line.Length || line[j] != '-')
                        {
                            Errors = Common.MLS.Get(MLSConst, "No symbol '-' after sensor number definishion:") + " '" + line + "'";
                            return ret;
                        }
                        j++;
                        num2 = "";
                        for (; j < line.Length && (char.IsDigit(line[j]) || line[j] == ',' || line[j] == '.'); j++)
                            num2 += line[j];
                        try
                        {
                            ly = serv.ParseDouble(num2);
                        }
                        catch
                        {
                            Errors = Common.MLS.Get(MLSConst, "Can't parse ly value. In line:") + " '" + line + "'";
                            return ret;
                        }
                        links_pixel[sn].Add(pixel);
                        links_ly[sn].Add(ly);
                        links_i[sn].Add(ret.Count);
                        ret.Add(new LinkInfo(pixel, ly, sn, line_count));
                    }
                    line = serv.GetLine(text, ref ind);
                    line_count++;
                }
                Function[] fks = new Function[ss.Length];
                for (int s = 0; s < ss.Length; s++)
                {
                    if (links_ly[s].Count < 2)
                        continue;
                    double[] n = new double[links_ly[s].Count];
                    double[] ly = new double[links_ly[s].Count];
                    bool[] en = new bool[links_ly[s].Count];
                    for (int i = 0; i < n.Length; i++)
                    {
                        n[i] = links_pixel[s][i];
                        ly[i] = links_ly[s][i];
                        en[i] = true;
                    }
                    Function.Types type;
                    switch (polinom[s])
                    {
                        case 2: type = Function.Types.Polinom2; break;
                        case 3: type = Function.Types.Polinom3; break;
                        default: type = Function.Types.Line; break;
                    }

                    for (int i = 0; i < n.Length; i++)
                    {
                        for (int j = i+1; j < n.Length; j++)
                        {
                            if (Math.Abs(n[i] - n[j]) <= 1)
                            {
                                Errors = Common.MLS.Get(MLSConst, "Links is too close... ")+n[i]+"-"+n[j];
                                return ret;
                            }
                        }
                    }

                    fks[s] = new Function(type, n, ly, en, false, false, 0.1);
                    for (int i = 0; i < ly.Length; i++)
                    {
                        int ret_i = links_i[s][i];
                        double rly = fks[s].CalcY(n[i]);
                        ret[ret_i].Dlt = (float)Math.Abs(rly - ly[i]);
                        if(ret[ret_i].Dlt > 10)
                            rly = fks[s].CalcY(n[i]);
                    }
                }
                for (int sc = 0; sc < ss.Length - 1 && fks[0] == null; sc++)
                {
                    for (int s = 0; s < ss.Length - 1 && fks[s] == null; s++)
                    {
                        if (fks[s+1] == null)//(links_ly[s + 1].Count < 2)
                            continue;
                        double[] n = { base_pixels[s] + ss[s] - 10, base_pixels[s] + ss[s] - 1 };
                        double[] ly = {fks[s+1].CalcY(n[0]),fks[s+1].CalcY(n[1])};
                        fks[s] = new Function(Function.Types.Line, n, ly, false, false, 0.1);
                        break;
                    }
                }
                for (int sc = 0; sc < ss.Length - 1 && fks[fks.Length - 1] == null; sc++)
                {
                    for (int s = ss.Length - 1; s > 0 && fks[s] == null; s--)
                    {
                        if (fks[s - 1] == null)
                            continue;
                        double[] n = { base_pixels[s], base_pixels[s] + 10 };
                        double[] ly = { fks[s - 1].CalcY(n[0]), fks[s - 1].CalcY(n[1]) };
                        fks[s] = new Function(Function.Types.Line, n, ly, false, false, 0.1);
                        break;
                    }
                }
                for (int sc = 0; sc < ss.Length - 1; sc++)
                {
                    for (int s = 1; s < ss.Length-1 && fks[s] == null; s++)
                    {
                        if (fks[s] != null)
                            continue;
                        List<double> n = new List<double>();
                        List<double> ly = new List<double>();
                        if (fks[s - 1] != null)
                        {
                            double tmp = base_pixels[s-1]+ss[s-1];
                            n.Add(tmp);
                            ly.Add(fks[s - 1].CalcY(tmp));
                            tmp = base_pixels[s - 1] + ss[s - 1] + 20;
                            n.Add(tmp);
                            ly.Add(fks[s - 1].CalcY(tmp));
                        }
                        if (fks[s + 1] != null)
                        {
                            double tmp = base_pixels[s + 1];
                            n.Add(tmp);
                            ly.Add(fks[s + 1].CalcY(tmp));
                            tmp = base_pixels[s + 1] + 20;
                            n.Add(tmp);
                            ly.Add(fks[s + 1].CalcY(tmp));
                        }
                        if (n.Count < 2)
                            break;
                        fks[s] = new Function(Function.Types.Line, n, ly, false, false, 0.1);
                        break;
                    }
                }
                /*if (fks[0] != null)
                {
                    for (int s = 1; s < ss.Length - 1; s++)
                    {
                        if (links_ly[s].Count > 2)
                            continue;
                        double[] n = { base_pixels[s] + ss[s] - 10, base_pixels[s] + ss[s] - 1 };
                        double[] ly = { fks[s + 1].CalcY(n[0]), fks[s + 1].CalcY(n[1]) };
                        fks[s] = new Function(Function.Types.Line, n, ly, false, false, 0.1);
                        break;
                    }
                }*/
                for (int s = 0; s < ss.Length; s++)
                {
                    if (fks[s] != null)
                        continue;
                    double[] n = { base_pixels[s], base_pixels[s] + ss[s] - 1 };
                    double[] ly = { n[0], n[1] };
                    fks[s] = new Function(Function.Types.Line, n, ly, false, false, 0.1);
                }
                if (apply)
                {
                    if (ReadOnly)
                        throw new Exception("Dispers Read Only");
                    for (int s = 0; s < ss.Length; s++)
                        Functions[s] = fks[s];
                }
                rez = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                Errors = Common.MLS.Get(MLSConst, "Faltal error. In line:") + " '" + line + "' "+ex;
            }
            #endregion
            return ret;
        }

        public Dispers(BinaryReader br)
        {
            SetupDefaultDisp();
            int ver = br.ReadInt32();
            if (ver != 1)
                throw new Exception("Wrong version of the Disprs");
            int n = br.ReadInt32();
            Functions = new Function[n];
            SSizes = new int[n];
            for (int i = 0; i < n; i++)
            {
                Functions[i] = new Function(br);
                SSizes[i] = br.ReadInt32();
                if (SSizes[i] == 4096 && (Common.Conf.BlakPixelStart != 0 || Common.Conf.BlakPixelEnd != 0))
                    SSizes[i] = 4096 - Common.Conf.BlakPixelStart - Common.Conf.BlakPixelEnd;
            }
            InitBasePixels();
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);
            bw.Write(Functions.Length);
            for (int i = 0; i < Functions.Length; i++)
            {
                Functions[i].Save(bw);
                bw.Write(SSizes[i]);
            }
        }
    }
}
