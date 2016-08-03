using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using SpectroWizard.dev.interf;
using SpectroWizard.data;
using SpectroWizard.gui;
using SpectroWizard.dev.devices;
using SpectroWizard.util;

namespace SpectroWizard.dev
{
    abstract public class Dev
    {
        const string MlsConst = "Dev";
        /*public const uint IdRegDebugDevice = 0;
        public const uint IdGenDebugDevice = 0;
        public const uint IdRegMLDevice = 3;
        public const uint IdGenStandartDevice = 4;*/

        public byte IP1, IP2, IP3, IP4;
        public ushort Port;
        public float Tick
        {
            get{
                return Common.Conf.MinTick;
            }
        }

        abstract public string GetMeasuringLog();
        abstract public string GetName();

        public DevReg Reg;
        public DevGen Gen;
        public DevFillLight Fill;
        public DevGas Gas;

        public abstract string DefaultDipsers();

        public Dev(DevReg reg, DevGen gen, 
            DevFillLight fill,
            DevGas gas)//, CONInterf interf)
        {
            Reg = reg;
            reg.Init(this);//, interf);
            Gen = gen;
            gen.Init(this);//, interf);
            Fill = fill;
            Fill.Init(this);
            Gas = gas;
            Gas.Init(this);
            //Interf = interf;
        }

        public void CorrectExposition(SpectrConditionCompiledLine line,
            out int common_i, out int[] exps_i)
        {
            DevReg.SimpleTimeCorrection(this, line.CommonTime, line.Expositions, out common_i, out exps_i);
        }

        public void CorrectExposition(float common_time, float[] exps,
            out int common_i, out int[] exps_i)
        {
            DevReg.SimpleTimeCorrection(this, common_time, exps, out common_i, out exps_i);
        }

        abstract public void AfterMeasuring();
        abstract public void BeforeMeasuring();
        abstract public bool IsUSBConsole();

        public void CheckConnection()
        {
            if(Reg.IsConnected() == false)
                Reg.Connect();
            if(Gen.IsConnected() == false)
                Gen.Connect();
        }

        public delegate void MeasuringResultFinalCall(List<SpectrDataView> data, SpectrCondition cond);
        MeasuringResultFinalCall FinalCall = null;
        SpectrCondition Cond;
        Thread MeasuringThread;
        public static bool Aborted = false;
        public void Measuring(SpectrCondition cond, MeasuringResultFinalCall final_call)
        {
            if (File.Exists(USBConDev.MLogFileName))
                File.Delete(USBConDev.MLogFileName);
            if(FinalCall != null)
                throw new Exception(Common.MLS.Get("Dev","Start new measuring before previous finished."));
            WaitTimeOutDlg.checkTimeOut();
            Aborted = false;
            FinalCall = final_call;
            Cond = cond;
            if (final_call != null)
            {
                MeasuringThread = new Thread(new ThreadStart(MeasuringTech));
                MeasuringThread.Start();
                //gui.MainForm.MForm.Enabled = false;
                gui.MainForm.MForm.EnableToolExit(false,
                    Common.MLS.Get(MlsConst,"Измерения..."));
            }
            else
                MeasuringTech();
        }

        public Spectr GetLetestDataAsSpectr()
        {
            Spectr sp = new Spectr(Cond, Common.Env.DefaultDisp, Common.Env.DefaultOpticFk,Common.Dev.GetMeasuringLog());
            for (int i = 0; i < LetestResult.Count; i++)
                sp.Add(LetestResult[i]);
            return sp;
        }

        public List<SpectrDataView> LetestResult;
        void MeasuringTech()
        {
            try
            {
                Reg.NullMeasuringEnabled = false;
                while (Reg.NullMeasuring == true && Common.IsRunning)
                    Thread.Sleep(10);
                if (Common.IsRunning == false)
                    return;
                SpectrCondition cond = Cond;
                List<SpectrDataView> ret = new List<SpectrDataView>();
                CheckConnection();
                BeforeMeasuring();

                int common_time_i = 1;
                int[] exps_i;

                if (IsUSBConsole())
                {
                    int[] line_sizes = Reg.GetSensorSizes();
                    float time_step = Common.Conf.MinTick * (Common.Conf.Divider2 + 1);
                    string program = "n"+line_sizes.Length+";s4096;d"+Common.Conf.Divider2+";";
                    int measuring_index = 0;
                    cond.saveExtra(Common.Env.DefaultDisp);
                    int[] orientations = Common.Conf.USBOrientationVals;
                    for (int i = 0; i < cond.Lines.Count; i++)
                    {
                        switch (cond.Lines[i].Type)//SpectrCondition.ParseStringType(line))
                        {
                            case SpectrCondition.CondTypes.FillLight:
                                Fill.SetFillLight(cond.Lines[i].IsFillLight);
                                break;
                            case SpectrCondition.CondTypes.Comment: break;
                            case SpectrCondition.CondTypes.Empty: break;
                            case SpectrCondition.CondTypes.Unexpected: break;
                            case SpectrCondition.CondTypes.Prespark:
                                program += "p"+cond.Lines[i].CommonTime+";";
                                break;
                            case SpectrCondition.CondTypes.Exposition:
                                CorrectExposition(cond.Lines[i], out common_time_i, out exps_i);
                                cond.Lines[i].TmpInteger = measuring_index;
                                if(cond.Lines[i].IsGenOn)
                                    program += "E";
                                else
                                    program += "e";
                                program += cond.Lines[i].TmpInteger + "_" + common_time_i + "_";
                                int[] exps = new int[16];
                                for (int ei = 0; ei < exps.Length && ei < orientations.Length; ei++)
                                {
                                    if (exps_i[ei] > 255)
                                        throw new Exception("Exposition is too bit!");
                                    exps[(int)Math.Abs(orientations[ei]) - 1] = exps_i[ei];
                                }
                                for (int l = 0; l < 16; l++)
                                {
                                    program += exps[l] + " ";
                                    //if (l < exps_i.Length)
                                    //    program += exps_i[l] + " ";
                                    //else
                                    //    program += "0 ";
                                }
                                program += ";";
                                measuring_index ++;
                                break;
                        }
                    }
                    FileStream fs = new FileStream("cur_prog.txt", FileMode.Create);
                    byte[] array = Encoding.ASCII.GetBytes(program);
                    fs.Write(array, 0, array.Length);
                    fs.Flush();
                    fs.Close();

                    //Process proc = Process.Start("USBDataCollector.exe", "cur_prog.txt");
                    //Process proc = Process.Start("USBMiner.exe", "cur_prog.txt");
                    //Process proc = Process.Start("measuring.bat");
                    System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo("measuring.bat");

                    proc.RedirectStandardOutput = false;
                    proc.UseShellExecute = false;

                    // Do not create the black window.
                    proc.CreateNoWindow = true;
                    proc.WindowStyle = ProcessWindowStyle.Hidden;

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo = proc;
                    process.Start();

                    process.WaitForExit();
                    int exit_code = process.ExitCode;

                    if (exit_code != 0)
                        throw new Exception("Measuring error: "+exit_code);

                    //((USBConDev)this).CollectDataMiner(ret, cond); //uncomment if use USBDataCollector.exe
                    ((USBConDev)this).CollectDataMiner(ret, cond);
                }
                else
                {
                    string source = cond.SourceCode;
                    int i = 0;
                    //string line = SpectrCondition.GetLine(source, ref i);
                    int[] ssize = Reg.GetSensorSizes();
                    double[] exps = new double[ssize.Length];
                    float[][] data;
                    short[][] datas;
                    SpectrDataView cur_spview;
                    for (i = 0; i < cond.Lines.Count; i++)//(line != null)
                    {
                        if (Aborted || Common.CancelFlag == true)
                        {
                            try { Common.Dev.Gen.SetStatus(false); }
                            catch { }
                            return;
                        }
                        try
                        {
                            gui.MainForm.MForm.SetupPersents(i * 100 / cond.Lines.Count);
                            switch (cond.Lines[i].Type)//SpectrCondition.ParseStringType(line))
                            {
                                case SpectrCondition.CondTypes.FillLight:
                                    Fill.SetFillLight(cond.Lines[i].IsFillLight);
                                    break;
                                case SpectrCondition.CondTypes.Comment: break;
                                case SpectrCondition.CondTypes.Empty: break;
                                case SpectrCondition.CondTypes.Unexpected: break;
                                case SpectrCondition.CondTypes.Prespark:
                                    Common.Log(Common.MLS.Get("Dev", "Start prespark. ") + cond.Lines[i].CommonTime);
                                    //gui.MainForm.MForm.SetupTimeOut(cond.Lines[i].CommonTime);
                                    /*common_time_i = (int)(cond.Lines[i].CommonTime / Tick);
                                    exps_i = new int[exps.Length];
                                    for (int j = 0; j < exps.Length; j++)
                                        exps_i[j] = 1;*/
                                    exps_i = null;
                                    for (int t = 0; t < cond.Lines.Count; t++)
                                    {
                                        if (cond.Lines[t].IsActive && cond.Lines[t].Expositions != null)
                                        {
                                            CorrectExposition(cond.Lines[i].CommonTime, cond.Lines[t].Expositions,
                                                                out common_time_i, out exps_i);
                                            break;
                                        }
                                    }
                                    Gen.SetStatus(true);
                                    Reg.RegFrame(common_time_i, exps_i);
                                    break;
                                case SpectrCondition.CondTypes.Exposition:
                                    bool nul;
                                    CorrectExposition(cond.Lines[i], out common_time_i, out exps_i);
                                    string msg;
                                    if (cond.Lines[i].IsActive)
                                    {
                                        msg = Common.MLS.Get("Dev", "Spark. ");
                                        nul = false;
                                    }
                                    else
                                    {
                                        msg = Common.MLS.Get("Dev", "Null. ");
                                        nul = true;
                                    }
                                    msg += +common_time_i + " [ ";
                                    for (int j = 0; j < exps_i.Length; j++)
                                        msg += exps_i[j] + " ";
                                    msg += "] " + Math.Round(cond.Lines[i].CommonTime, 2);
                                    Common.Log(msg);

                                    //gui.MainForm.MForm.SetupTimeOut(cond.Lines[i].CommonTime);
                                    Gen.SetStatus(cond.Lines[i].IsGenOn);
                                    short[][] bb, ba;
                                    if (nul == false)
                                        datas = Reg.RegFrame(common_time_i, exps_i, out bb, out ba);
                                    else
                                        datas = Reg.GetNull(cond.Lines[i], out bb, out ba);

                                    data = new float[datas.Length][];
                                    for (int s = 0; s < datas.Length; s++)
                                    {
                                        int size = datas[s].Length;
                                        data[s] = new float[size];
                                        for (int j = 0; j < size; j++)
                                            data[s][j] = datas[s][j];
                                    }
                                    cur_spview = new SpectrDataView(new SpectrCondition(Tick, cond.Lines[i]), data, bb, ba,
                                            Common.Dev.Reg.GetMaxValue(),
                                            Common.Dev.Reg.GetMaxLinarValue());
                                    ret.Add(cur_spview);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Log(ex);
                            break;
                        }
                    }
                    Gen.SetStatus(false);
                }
                AfterMeasuring();
                LetestResult = ret;
                if (FinalCall != null)
                    FinalCall(ret, Cond);
                //gui.MainForm.MForm.SetupTimeOut(0);
                gui.MainForm.MForm.SetupPersents(-1);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                try
                {
                    Reg.NullMeasuringEnabled = true;
                    gui.MainForm.MForm.SetupTimeOut(0);
                }
                catch
                {
                }
                try
                {
                    FinalCall = null;
                    //gui.MainForm.MForm.Enabled = true;
                    gui.MainForm.MForm.EnableToolExit();
                }
                catch
                {
                }
                try
                {
                    Reg.Reset();
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
            }
        }
    }

    abstract public class DevReg
    {
        float GetTick()
        {
            return Common.Dev.Tick;
        }
        const string MlsConst = "DevReg";
        public short[][] LastBlankStart, LastBlankEnd;
        public short[][] RegFrame(int common, int[] exps)
        {
            return RegFrame(common,exps,out LastBlankStart,out LastBlankEnd);
        }

        public short[][] RegFrame(int common, int[] exps,out short[][] blank_start,out short[][] blank_end)
        {
            float tick = GetTick();
            short[][] ret = null;
            MainForm.MForm.SetupTimeOut(common * tick+1);
            try
            {
                ret = RegFrameProc(common, exps,out blank_start,out blank_end);
            }
            finally
            {
                try
                {
                    MainForm.MForm.SetupTimeOut(-1);
                }
                catch { }
            }
            return ret;
        }
        abstract protected short[][] RegFrameProc(int common, int[] exps,out short[][] blank_start,out short[][] blank_end);
        abstract public int[] GetSensorSizes();
        //abstract public void SetFillLightStatus(bool on_off);
        abstract public void Connect();
        abstract public void Disconnect();
        abstract public string GetName();
        abstract public bool IsConnected();
        abstract public short GetMaxValue();
        abstract public short GetMaxLinarValue();
        abstract public string Test();
        abstract public string TimeConstTest(int divider,out float val);
        abstract public string MaxLevelTest(out float level);
        abstract public string NoiseTest(int exp);
        abstract public void Reset();

        #region Find expositions....
        private static int mFindNearest(int c, int time)
        {
            int n = c / time;
            if (n <= 1)
                return c;
            int n1 = n;
            int n2 = n;
            while ((c / n1) * n1 != c && (c / n2) * n2 != c)
            {
                n1++;
                n2--;
            }
            if ((c / n2) * n2 == c)
                return c / n2;
            return c / n1;
        }

        public static void SimpleTimeCorrection(Dev dev,float common, float[] exps,
            out int common_i, out int[] exps_i)
        {
            double[] inpar = new double[exps.Length + 1];
            inpar[0] = common;
            for (int i = 0; i < exps.Length; i++)
                inpar[i + 1] = exps[i];
            int[] rez = mSimpleTimeCorrection(inpar,dev);
            common_i = rez[0];
            exps_i = new int [exps.Length];
            for(int i = 0;i<exps.Length;i++)
                exps_i[i] = rez[i+1];
        }

        /// <summary>
        /// This method converts from real time to supported time steps
        /// </summary>
        /// <param name="lTimes">real times</param>
        /// <returns></returns>
        private static int[] mSimpleTimeCorrection(double[] lTimes,Dev dev)
        {
            try
            {
                double step = dev.Tick;// *0.000001;
                int common = (int)(lTimes[0] / step);
                int[] times = new int[lTimes.Length - 1];
                for (int i = 0; i < times.Length; i++)
                    times[i] = (int)(lTimes[i + 1] / step);
                //int presp = (int)(lTimes[lTimes.Length - 1] / step);
                if (common <= 0)
                    common = 1;
                //if (presp < 0)
                //    presp = 0;
                for (int i = 0; i < times.Length; i++)
                {
                    if (times[i] <= 0)
                        times[i] = 1;
                    if (times[i] > common)
                        times[i] = common;
                }

                int common_from = (int)(common * 0.7) - 2;
                int common_to = (int)(common * 1.3) + 2;
                int best_common = common_from;
                double best_crit = double.MaxValue;
                if (common_from < 1)
                    common_from = 1;
                int c;
                
                int prev_common = -1;
                for (c = common_from; c <= common_to; c++)
                {
                    double tmp_crit = 0;// Math.Abs(common - c) * 0.1 / (double)common;
                    for (int i = 0; i < times.Length; i++)
                    {
                        int n = mFindNearest(c, times[i]);
                        tmp_crit += Math.Pow((double)(Math.Abs(times[i] - n))*100 / times[i], 2);
                    }
                    if (tmp_crit < best_crit)
                    {
                        best_crit = tmp_crit;
                        best_common = c;
                        prev_common = c;
                    }
                    else
                    {
                        if(tmp_crit == best_crit && Math.Abs(prev_common-common) > Math.Abs(prev_common-c))
                        {
                            best_crit = tmp_crit;
                            best_common = c;
                            prev_common = c;
                        }
                    }
                }

                c = best_common;
                int[] ret = new int[times.Length + 1];
                ret[0] = best_common;
                for (int i = 0; i < times.Length; i++)
                {
                    int n = mFindNearest(best_common, times[i]);
                    ret[i + 1] = n;// times[i];
                }
                return ret;
            }
            catch { }
            return null;
        }
        #endregion

        uint Id;
        public uint GetId()
        {
            return Id;
        }

        protected DevReg(uint id)
        {
            Id = id;
        }

        protected Dev D;
        //protected CONInterf Interf;
        public void Init(Dev dev)
        {
            D = dev;
            //Interf = interf;
            InitNullCalibrator();
        }

        #region Null Calibrator region
        //DbFolder NulFolder;
        class NullDataRecord
        {
            public short[][] Data;
            public short[][] BlankStart, BlankEnd;
            public float[] EverLevel;
            public long Time;

            public NullDataRecord(short[][] data,short[][] blank_start,short[][] blank_end)
            {
                Data = data;
                BlankStart = (short[][])blank_start;
                BlankEnd = (short[][])blank_end;
                short ever;
                if (Common.Conf.BlankSub && blank_start != null && blank_end != null)
                {
                    for (int s = 0; s < data.Length; s++)
                    {
                        ever = SpectroWizard.analit.Stat.GetEver(blank_start[s], blank_end[s]);
                        int len = data[s].Length;
                        for (int p = 0; p < len; p++)
                            data[s][p] -= ever;
                    }
                }
                EverLevel = new float[data.Length];
                for (int i = 0; i < EverLevel.Length; i++)
                    EverLevel[i] = (float)SpectroWizard.analit.Stat.GetEver(data[i]);
                Time = DateTime.Now.Ticks;
            }

            public NullDataRecord(BinaryReader br)
            {
                int ver = br.ReadInt32();
                if (ver < 2 || ver > 3)
                    throw new Exception("Wrong version of NullDataRecord");
                Time = br.ReadInt64();
                int sn = br.ReadInt32();
                Data = new short[sn][];
                EverLevel = new float[sn];
                for (int s = 0; s < sn; s++)
                {
                    EverLevel[s] = br.ReadSingle();
                    int len = br.ReadInt32();
                    Data[s] = new short[len];
                    for (int i = 0; i < len; i++)
                        Data[s][i] = br.ReadInt16();
                }
                if (ver >= 3)
                {
                    Load(br, ref BlankStart);
                    Load(br, ref BlankEnd);
                }
                ver = br.ReadInt32();
                if (ver != 52)
                    throw new Exception("Wrong version of NullDataRecord");
            }

            void Load(BinaryReader br,ref short[][] data)
            {
                int sn = br.ReadInt32();
                if(sn == 0)
                {
                    data = null;
                    return;
                }
                data = new short[sn][];
                for(int s = 0;s<sn;s++)
                {
                    int len = br.ReadInt32();
                    data[s] = new short[len];
                    for(int i = 0;i<len;i++)
                        data[s][i] = br.ReadInt16();
                }
            }

            public void Save(BinaryWriter bw)
            {
                bw.Write(3);
                bw.Write(Time);
                bw.Write(Data.Length);
                for (int s = 0; s < Data.Length; s++)
                {
                    bw.Write(EverLevel[s]);
                    bw.Write(Data[s].Length);
                    for (int i = 0; i < Data[s].Length; i++)
                        bw.Write(Data[s][i]);
                }
                Save(bw, ref BlankStart);
                Save(bw, ref BlankEnd);
                bw.Write(52);
            }

            void Save(BinaryWriter bw,ref short[][] data)
            {
                if (data == null || data[0] == null)
                    bw.Write(0);
                else
                {
                    bw.Write(data.Length);
                    for (int s = 0; s < data.Length; s++)
                    {
                        bw.Write(data[s].Length);
                        for(int i = 0;i<data[s].Length;i++)
                            bw.Write(data[s][i]);
                    }
                }
            }
        }

        public class NulDatas
        {
            List<NullDataRecord> Datas = new List<NullDataRecord>();

            public void ClearData()
            {
                Datas.Clear();
            }

            public int Count
            {
                get
                {
                    return Datas.Count;
                }
            }

            string Path;
            public SpectrConditionCompiledLine Line;
            int CommonTime;
            int[] Exps;
            public DevReg Dev = null;
            public NulDatas(SpectrConditionCompiledLine line)
            {
                Line = line;
                DevReg.SimpleTimeCorrection(Common.Dev, line.CommonTime,
                    line.Expositions, out CommonTime, out Exps);
                int common_time = 0;
                int n = 8;
                int max_exp = Exps[0];
                for(int i = 1;i<Exps.Length;i++)
                {
                    if(max_exp < Exps[i])
                        max_exp = Exps[i];
                }
                common_time = max_exp * n;
                while(common_time < CommonTime)
                {
                    bool is_good = true;
                    for (int i = 0; i < Exps.Length; i++)
                    {
                        if ((common_time % (Exps[i])) != 0)
                        {
                            is_good = false;
                            break;
                        }
                    }
                    if (is_good)
                    {
                        CommonTime = common_time;
                        break;
                    }
                    n++;
                    common_time = max_exp * n;
                }
                DbFolder folder = Common.Db.GetFolder(Common.DbNameNulFolder);
                string record_name = line.CommonTime+"[";
                for(int i = 0;i<line.Expositions.Length;i++)
                {
                    if(i != 0)
                        record_name += " ";
                    record_name += line.Expositions[i];
                }
                record_name += "]";
                Path = folder.GetRecordPath(record_name);
                Load();
            }

            void Load()
            {
                if (DataBase.FileExists(ref Path) == false)// File.Exists(Path) == false)
                    return;
                FileStream fs = DataBase.OpenFile(ref Path,FileMode.OpenOrCreate,FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                string type = br.ReadString();
                if (type.Equals("NulCalibrs") == false)
                    throw new Exception(Common.MLS.Get(MlsConst,"Wrong type of the Null Calibr record"));
                int ver = br.ReadInt32();
                if(ver != 1)
                    throw new Exception(Common.MLS.Get(MlsConst, "Wrong type version of the Null Calibr record"));

                ver = br.ReadInt32();
                Datas.Clear();
                for (int i = 0; i < ver; i++)
                    Datas.Add(new NullDataRecord(br));
                ver = br.ReadInt32();
                if (ver != 1)
                    throw new Exception(Common.MLS.Get(MlsConst, "Wrong final of the Null Calibr record"));
                br.Close();

                ClearOldData();//*/
            }

            public void Save()
            {
                FileStream fs = DataBase.OpenFile(ref Path, FileMode.OpenOrCreate, FileAccess.Write);
                try
                {
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Write("NulCalibrs");
                    bw.Write(1);
                    bw.Write(Datas.Count);
                    for (int i = 0; i < Datas.Count; i++)
                        Datas[i].Save(bw);
                    bw.Write(1);
                    bw.Flush();
                    fs.SetLength(fs.Position);
                }
                finally
                {
                    fs.Close();//*/
                }
            }

            void ClearOldData()
            {
                try
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        long life_time = (DateTime.Now.Ticks - Datas[i].Time) / 10000000L;
                        if (life_time > Common.Conf.NulHistory)
                        {
                            Datas.RemoveAt(i);
                            i--;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
            }

            long PrevNulSave = 0;
            public void Measuring()
            {
                if (Dev.IsConnected() != true)
                    return;
                //gui.MainForm.MForm.SetupTimeOut(Line.CommonTime);
                short[][] data, blank_start, blank_end;
                data = Common.Dev.Reg.RegFrame(CommonTime, Exps,out blank_start,out blank_end);
                Datas.Add(new NullDataRecord(data,blank_start,blank_end));
                ClearOldData();
                while (Datas.Count > 256)
                    Datas.RemoveAt(0);
                if (DateTime.Now.Ticks - PrevNulSave > 60 * 10000000L)
                {
                    Save();
                    PrevNulSave = DateTime.Now.Ticks;
                }
                //gui.MainForm.MForm.SetupTimeOut(0);
            }

            public void ForseSave()
            {
                Save();
            }

            public short[][] GetData(out short[][] blank_start,out short[][] blank_end)
            {
                double[] data = new double[Datas.Count];
                int sn = Datas[0].Data.Length;
                int last = Datas.Count - 1;
                short[][] ret = new short[sn][];
                if (Datas[0].BlankStart != null)
                    blank_start = new short[sn][];
                else
                    blank_start = null;
                if (Datas[0].BlankEnd != null)
                    blank_end = new short[sn][];
                else
                    blank_end = null;
                for (int s = 0; s < sn; s++)
                {
                    int len = Datas[0].Data[s].Length;
                    ret[s] = new short[len];
                    for (int p = 0; p < len; p++)
                    {
                        for (int m = 0; m < Datas.Count; m++)
                            data[m] = Datas[m].Data[s][p] + (Datas[last].EverLevel[s] - Datas[m].EverLevel[s]);
                        ret[s][p] = (short)(Math.Round(SpectroWizard.analit.Stat.GetEver(data),0));
                    }

                    if (Datas[0].BlankStart != null)
                    {
                        double[] data_bs = new double[Datas.Count];
                        len = Datas[0].BlankStart.Length;
                        blank_start[s] = new short[len];
                        for (int p = 0; p < len; p++)
                        {
                            for (int m = 0; m < Datas.Count; m++)
                                data_bs[m] = Datas[m].BlankStart[s][p] + (Datas[last].EverLevel[s] - Datas[m].EverLevel[s]);
                            ret[s][p] = (short)(Math.Round(SpectroWizard.analit.Stat.GetEver(data_bs), 0));
                        }
                    }

                    if (Datas[0].BlankEnd != null)
                    {
                        double[] data_be = new double[Datas.Count];
                        len = Datas[0].BlankEnd.Length;
                        blank_end[s] = new short[len];
                        for (int p = 0; p < len; p++)
                        {
                            for (int m = 0; m < Datas.Count; m++)
                                data_be[m] = Datas[m].BlankEnd[s][p] + (Datas[last].EverLevel[s] - Datas[m].EverLevel[s]);
                            ret[s][p] = (short)(Math.Round(SpectroWizard.analit.Stat.GetEver(data_be), 0));
                        }
                    }
                }
                return ret;
            }
        }

        List<NulDatas> NulStorage = null;
        public void InitNullCalibrator()
        {
            if (NulStorage != null)
                return;
            NulStorage = new List<NulDatas>();
            //NulFolder = Common.Db.GetFolder(Common.DbNameNulFolder);
            NullMeasuringThread = new Thread(NullMeasuringProc);
            NullMeasuringThread.Start();
            NullMeasuringEnabled = true;
        }

        public short[][] GetNull(SpectrConditionCompiledLine line,out short[][] blank_start,out short[][] blank_end)
        {
            if (Common.Conf.UseGoodNul != false)
            {
                blank_start = null;
                blank_end = null;
                for (int i = 0; i < NulStorage.Count; i++)
                {
                    if (NulStorage[i].Line.IsExpositionEqual(line,false))
                        return NulStorage[i].GetData(out blank_start,out blank_end);
                }
            }
            int common;
            int[] exps;
            DevReg.SimpleTimeCorrection(Common.Dev,line.CommonTime,
                line.Expositions,out common,out exps);
            return RegFrame(common, exps,out blank_start,out blank_end);
        }

        void WaitForMeasuringEnd()
        {
            for (int i = 0; i < 5000 && NullMeasuring == true && Common.IsRunning; i++ )
                Thread.Sleep(1);
            if(NullMeasuring && Common.IsRunning)
                try
                {
                    NullMeasuringThread.Abort();
                }
                catch
                {
                }
                finally
                {
                    NullMeasuringPause = false;
                    NullMeasuring = false;
                    InitNullCalibrator();
                }
        }

        public void AddNullMonitor(SpectrConditionCompiledLine line)
        {
            WaitForMeasuringEnd();
            NullMeasuringPause = true;

            NulDatas nd = new NulDatas(line);
            nd.Dev = this;
            NulStorage.Add(nd);
            NullMeasuringPause = false;
        }

        public void ClearNullMonitor()
        {
            while (NullMeasuring == true && Common.IsRunning)
                Thread.Sleep(1);
            NullMeasuringPause = true;
            foreach (NulDatas nul in NulStorage)
            {
                //nul.ClearData();
                nul.Save();
            }
            while (NullMeasuring == true && Common.IsRunning)
                Thread.Sleep(1);
            NulStorage.Clear();
            NullMeasuringPause = false;
        }

        public void ClearNullStorage()
        {
            NullMeasuringPause = true;
            foreach (NulDatas nul in NulStorage)
            {
                nul.ClearData();
                nul.Save();
            }
            NullMeasuringPause = false;
        }

        Thread NullMeasuringThread = null;
        public bool NullMeasuring = false;
        public bool NullMeasuringEnabled = false;
        string NulCountLb = "";
        void Update()
        {
            MainForm.MForm.StatusBarNulCount.Text = NulCountLb;
        }

        bool NullMeasuringPause = false;
        void NullMeasuringProc()
        {
            int nul_count = 0;
            int ex_count = 0;

            while(Common.IsRunning && IsConnected() == false)
                Thread.Sleep(1000);

            while (Common.IsRunning)
                try
                {
                    if (ex_count > 2)
                        Thread.Sleep(1000);
                    Thread.Sleep(500);
                    if (NullMeasuringPause)
                        continue;
                    if (Common.Conf.UseGoodNul == false ||
                        NulStorage.Count == 0 || 
                        NullMeasuringEnabled == false)
                        continue;
                    if (IsConnected() == false)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    NullMeasuring = true;
                    if (nul_count >= NulStorage.Count)
                        nul_count = 0;
                    NulStorage[nul_count].Measuring();
                    nul_count++;
                    ex_count = 0;
                    NulCountLb = "n";
                    for (int i = 0; i < NulStorage.Count;i++ )
                        NulCountLb += " " + NulStorage[i].Count;
                    if (Common.IsClosing == false && Common.IsRunning == true)
                        MainForm.MForm.Invoke(new MethodInvoker(Update));
                }
                catch (Exception ex)
                {
                    if (ex_count == 0)
                        Common.LogNoMsg(ex);
                    else
                    {
                        if(ex_count < 4)
                            Common.LogNoMsg(ex);
                    }
                    ex_count++;
                }
                finally
                {
                    NullMeasuring = false;
                }

            if (ex_count == 0)
            {
                for (int i = 0; i < nul_count; i++)
                    try
                    {
                        NulStorage[i].ForseSave();
                    }
                    catch
                    {
                        break;
                    }
            }
                
        }
        #endregion
    }

    abstract public class DevGen
    {
        public void SetStatus(bool on_off)
        {
            SetStatusProc(on_off);
            Common.GenForm.SetStatus(on_off);
        }
        abstract protected void SetStatusProc(bool on_off);
        abstract public void Connect();
        abstract public void Disconnect();
        abstract public string GetName();
        abstract public bool IsConnected();
        abstract public string Test();

        uint Id;
        public uint GetId()
        {
            return Id;
        }

        protected DevGen(uint id)
        {
            Id = id;
        }

        protected Dev D;
        //CONInterf Interf;
        public void Init(Dev dev)
        {
            D = dev;
            //Interf = interf;
        }
    }

    abstract public class DevFillLight
    {
        protected const string MLSConst = "DFLight";
        public abstract void SetFillLight(bool on_off);
        public abstract bool Has();
        public abstract string GetName();
        public abstract string Test();
        public abstract string Test(bool status);

        protected Dev Device;
        public void Init(Dev dev)
        {
            Device = dev;
        }
    }

    public class DevFillLightNull : DevFillLight
    {
        public override string Test()
        {
            return "Nothing to be tested...";
        }

        public override bool Has()
        {
            return false;
        }

        public override void SetFillLight(bool on_off)
        {
            throw new NotImplementedException();
        }

        public override string  Test(bool status)
        {
         	throw new NotImplementedException();
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst,"Null");
        }
    }

    public class DevFillLightHContr : DevFillLight
    {
        public override string Test()
        {
            string ret = "";
            try
            {
                ret += "Fillight On test: ";
                SetFillLight(true);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            Thread.Sleep(1000);
            try
            {
                ret += "Fillight Off test: ";
                SetFillLight(false);
                ret += " Ok" + serv.Endl;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                ret += ex;
                return ret;
            }
            return ret;
        }

        public override string  Test(bool status)
        {
         	SetFillLight(status);
            return "Ok";
        }

        public override bool Has()
        {
            return true;
        }

        bool FillLightStatus = false;
        public override void SetFillLight(bool on_off)
        {
            if (FillLightStatus == on_off)
                return;

            /*string msg;
            MessageBoxIcon icon;
            if (on_off)
            {
                msg = "Включить источник заливающего света";
                icon = MessageBoxIcon.Hand;
            }
            else
            {
                msg = "Выключить источник заливающего света";
                icon = MessageBoxIcon.Stop;
            }
            
            DialogResult dr = MessageBox.Show(gui.MainForm.MForm,
                Common.MLS.Get(MLSConst,msg),
                Common.MLS.Get(MLSConst,"Заливающий свет..."),
                MessageBoxButtons.OKCancel,icon);*/

            DialogResult dr;
            if (on_off)
            {
                dr = MessageBox.Show(gui.MainForm.MForm,
                        Common.MLS.Get(MLSConst,"Включить источник заливающего света"),
                        Common.MLS.Get(MLSConst,"Заливающий свет..."),
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
            }
            else
            {
                dr = MessageBox.Show(gui.MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выключить источник заливающего света"),
                        Common.MLS.Get(MLSConst, "Заливающий свет..."),
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
            }
            if (dr != DialogResult.OK)
                throw new Exception(Common.MLS.Get(MLSConst, "Прервано пользователем!"));
            
            FillLightStatus = on_off;
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst, "HandControled");
        }
    }

    abstract public class DevGas
    {
        protected const string MLSConst = "DFLight";
        public abstract bool Has();
        protected Dev Device;
        public abstract string GetName();
        public abstract string Test();

        public void Init(Dev dev)
        {
            Device = dev;
        }
    }

    public class DevGasNull : DevGas
    {
        public override string Test()
        {
            return "Nothing to be tested...";
        }

        public override bool Has()
        {
            return false;
        }

        public override string GetName()
        {
            return Common.MLS.Get(MLSConst, "Null");
        }
    }
}
