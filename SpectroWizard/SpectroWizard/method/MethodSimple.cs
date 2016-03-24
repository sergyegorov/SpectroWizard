using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using SpectroWizard.data;
using SpectroWizard.dev;
using System.IO;

namespace SpectroWizard.method
{
    public class MethodSimple : IDisposable
    {
        const string MLSConst = "MSimple";
        MethodSimpleTable Data;
        public MethodSimpleInfo CommonInformation = new MethodSimpleInfo();
        public SparkConditionTester SpCondTester = new SparkConditionTester();
        public ExtraLineTester ExtraLineTester = new ExtraLineTester();
        public long BaseTick = 0;
        //public MeasuringExtraPrameters MExtraParams = new MeasuringExtraPrameters();
        public Spectr SpSort
        {
            get
            {
                string path = Path + "sort_sp";
                if (Spectr.IsFileExists(path))
                    return Common.Db.LoadSpectr(path);// new Spectr(path);
                return null;
            }
            set
            {
                string path = Path + "sort_sp";
                value.SaveAs(path);
            }
        }

        public Element[] GetElementList()
        {
            int n = GetElementCount();
            if(n == 0)
                return null;
            Element[] ret = new Element[n];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = GetElHeader(i).Element;
            return ret;
        }

        public float[] GetCalcConsForAllMeasuringElement(int element,int formula, out bool[] enable)
        {
            int count = 0;
            int pr_count = GetProbCount();
            for (int pr = 0; pr < pr_count; pr++)
            {
                MethodSimpleProb msp = GetProbHeader(pr);
                count += msp.MeasuredSpectrs.Count;
            }
            float[] cons = new float[count];
            enable = new bool[count];
            count = 0;
            for (int p = 0; p < pr_count; p++)
            {
                MethodSimpleProb pr = GetProbHeader(p);
                MethodSimpleCell msc = GetCell(element, p);//this[elemelemeent, GetProbHeader(p);
                //double sko,skog;
                for (int s = 0; s < pr.MeasuredSpectrs.Count; s++,count ++)
                {
                    MethodSimpleCellFormulaResult msf = msc.GetData(s, formula);
                    if (msf.ReCalcCon == null)
                    {
                        cons[count] = 0;
                        enable[count] = false;
                    }
                    else
                    {
                        cons[count] = (float)SpectroWizard.analit.Stat.GetEver(msf.ReCalcCon, SpectroWizard.analit.Stat.SpectrDataSKO);// (float)msc.CalcRealCon(out sko, out skog);
                        enable[count] = msf.Enabled;
                    }
                }
            }
            return cons;
        }

        public float[] GetConsForAllMeasuringElement(int element,out string[] prob_index)
        {
            int count = 0;
            int pr_count = GetProbCount();
            for(int pr = 0;pr<pr_count;pr++)
            {
                MethodSimpleProb msp = GetProbHeader(pr);
                count += msp.MeasuredSpectrs.Count;
            }
            float[] cons = new float[count];
            prob_index = new string[count];
            count = 0;
            for (int p = 0; p < pr_count; p++)
            {
                MethodSimpleProb pr = GetProbHeader(p);
                MethodSimpleCell msc = GetCell(element,p);//this[elemelemeent, GetProbHeader(p);
                for (int s = 0; s < pr.MeasuredSpectrs.Count; s++, count++)
                {
                    cons[count] = (float)msc.Con;
                    prob_index[count] = pr.Name;
                }
            }
            return cons;
        }

        public void Dispose()
        {
            if (SpCondTester != null)
                SpCondTester.Dispose();
            if (ExtraLineTester != null)
                ExtraLineTester.Dispose();
            Data = null;
            CommonInformation = null;
            SpCondTester = null;
            ExtraLineTester = null;
            SrcBuffer = null;
        }
        public void MethodMovedTo(string from,string to)
        {
            SetupPath(from);
            //SpLy.SaveAs(to + "lysp");
            if (SpSort != null)
                SpSort.SaveAs(to + "sort_sp");
            //Path = path;
            SetupPath(to);
        }

        byte[] SrcBuffer;
        public bool IsChanged()
        {
            if (SrcBuffer == null)
                return true;
            MemoryStream ms = new MemoryStream();
            try
            {
                BinaryWriter bw = new BinaryWriter(ms);
                Save(bw);
                bw.Flush();
                bw.Close();
                byte[] buf = ms.GetBuffer();
                if (buf.Length != SrcBuffer.Length)
                    return true;
                for (int i = 0; i < SrcBuffer.Length; i++)
                    if (SrcBuffer[i] != buf[i])
                        return true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                ms.Close();
            }
            return false;
        }

        public void SetupPath(string path)
        {
            FilePath = path;
            DataBase.CheckPath(ref FilePath);
            int ind = FilePath.LastIndexOf('\\');
            Path = FilePath.Substring(0, ind + 1);
        }

        public string FilePath;
        public string Path;
        void Bkp() // backup method
        {
            string base_path = DataBase.BasePath;
            string path_dst, path_src;
            for (int i = 1; i > 0; i--)
                try
                {
                    path_dst = base_path + Path + "method." + i;
                    path_src = base_path + Path + "method." + (i - 1);
                    if (File.Exists(path_dst))
                        File.Delete(path_dst);
                    if (File.Exists(path_src))
                        File.Move(path_src, path_dst);
                }
                catch
                {
                }
            path_dst = base_path + Path + "method.0";
            path_src = base_path + Path + "method";
            if (File.Exists(path_src))
                File.Copy(path_src, path_dst);

            PrevSavings = DateTime.Now.Ticks;
        }

        long PrevSavings = 0;
        public void SavePermited()
        {
            if ((DateTime.Now.Ticks - PrevSavings) > 10000000 * 60)
                try
                {
                    PrevSavings = DateTime.Now.Ticks;
                    Save();
                }
                catch
                {
                }
        }

        public MethodSimple(string tmp)
        {
            BaseTick = DateTime.Now.Ticks;
            string path = (string)tmp.Clone();
            DataBase.CheckPath(ref path);
            SetupPath(path);
            Bkp();
            Data = new MethodSimpleTable(this);
            if (DataBase.FileExists(ref FilePath) == true)// File.Exists(FilePath) == true)
            {
                FileStream fs = DataBase.OpenFile(ref FilePath, FileMode.Open, FileAccess.Read);
                SrcBuffer = new byte[(int)fs.Length];
                fs.Read(SrcBuffer,0,SrcBuffer.Length);
                fs.Seek(0,SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(fs);
                try
                {
                    Load(br);
                }
                finally
                {
                    br.Close();
                }
            }
            Save();
        }

        public MethodSimple(BinaryReader br)
        {
            Load(br);
        }

        #region Load/Save methods
        public void Save()
        {
            PrevSavings = DateTime.Now.Ticks;
            string fp = DataBase.GetFullPath(FilePath);
            string fp_ = null;
            if (File.Exists(fp))
            {
                fp_ = fp + "_";
                File.Copy(fp, fp_);
            }
            FileStream fs = DataBase.OpenFile(ref FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                Save(bw);
                bw.Flush();
                bw.Close();
            }
            catch (Exception ex)
            {
                bw.Close();
                Common.Log(ex);
                if (fp_ != null)
                {
                    File.Delete(fp);
                    File.Copy(fp_, fp);
                }
            }
            finally
            {
                if(fp_ != null)
                    File.Delete(fp_);
            }
        }

        public void Load(BinaryReader br)
        {
            for (int i = 99; i > 0;i-- )
                GC.Collect(i, GCCollectionMode.Forced);//.CompactLargeObjectHeap();

            string tmp = br.ReadString();
            if (tmp.Equals("ms") == false)
                throw new Exception("MethodSimple:Wrong record type");
            int ver = br.ReadInt32();
            if(ver < 0 || ver > 3)
                throw new Exception("MethodSimple:Unsupported version");

            //MethodSimpleTable Data;
            if (ver >= 2)
                CommonInformation = new MethodSimpleInfo(br);
            Data = new MethodSimpleTable(br,this);
            //public MethodSimpleInfo CommonInformation;
            if (ver < 2)
                CommonInformation = new MethodSimpleInfo(br);

            if (ver >= 1)
            {
                SpCondTester.LoadTech(br, this);
                ExtraLineTester.LoadTech(br, this);
            }
            else
                ExtraLineTester.Setup(this);

            if (ver >= 3)
                BaseTick = br.ReadInt64();
            else
            {
                string path = Common.Db.GetFoladerPath(Path);
                string[] files = Directory.GetFiles(path, "*.ss");
                BaseTick = long.MaxValue;
                for (int i = 0; i < files.Length; i++)
                {
                    DateTime dt = File.GetCreationTime(files[i]);
                    if (BaseTick > dt.Ticks)
                        BaseTick = dt.Ticks;
                }
                files = Directory.GetFiles(path, "method");
                for (int i = 0; i < files.Length; i++)
                {
                    DateTime dt = File.GetCreationTime(files[i]);
                    if (BaseTick > dt.Ticks)
                        BaseTick = dt.Ticks;
                }
                BaseTick -= 600 * 10000000L;
            }

            /*if (ver >= 4)
            {
                MExtraParams.load(br);
            }//*/

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimple:Unsupported end version");

            //CheckLyEtalon();
        }


        public void Save(BinaryWriter bw)
        {
            bw.Write("ms");
            bw.Write(3);
            CommonInformation.Save(bw);
            Data.Save(bw);
            SpCondTester.Save(bw);
            ExtraLineTester.Save(bw);
            bw.Write(BaseTick);
            //MExtraParams.save(bw);
            bw.Write(0);
        }
        #endregion

        public void ClearUnusdResults()
        {
#warning !!!!!!!  to be implement.... !!!!!!!!!!!!!!!
            /*int cc = Data.GetColumnCount();
            int pc = Data.GetRowCount();
            for (int p = 0; p < pc; p++)
            {
                MethodSimpleProb mp = GetProbHeader(p);
                for (int e = 0; e < cc; e++)
                {
                    MethodSimpleElement me = GetElHeader(e);
                    MethodSimpleCell msc = Data[e, p];
                    if (msc != null)
                        msc.ClearCons(me);
                }
            }*/
        }

        public void ReloadAllCons()
        {
            int cc = Data.GetColumnCount();
            int pc = Data.GetRowCount();
            for (int p = 0; p < pc; p++)
            {
                MethodSimpleProb mp = GetProbHeader(p);
                StLibStandart stand;
                try { stand = mp.Standart; }
                catch { continue; }
                for (int e = 0; e < cc; e++)
                {
                    MethodSimpleElement me = GetElHeader(e);
                    StLibElement elem = stand.FindByElementIndex(me.ElementIndex);
                    MethodSimpleCell msc = Data[e, p];
                    if (msc == null)
                    {
                        msc = new MethodSimpleCell();
                        Data[e, p] = msc;
                    }
                    msc.Prefix = "";
                    msc.Sufix = "";
                    if (elem != null)
                    {
                        msc.Con = elem.Con;
                        if (elem.IsAproxim)
                            msc.Prefix = "~";
                    }
                    else
                    {
                        msc.Con = -1;
                        msc.Prefix = "?";
                    }
                }
            }
        }

        void ClearData()
        {
            int cc = Data.GetColumnCount();
            int pc = Data.GetRowCount();
            for (int e = 0; e < cc; e++)
            {
                for (int p = 0; p < pc; p++)
                    Data[e, p].Clear();
            }
        }

        public void RemoveElement(int ind)
        {
            if (Data.GetColumnCount() == 1)
                throw new Exception("Невозможно удалить последний элемент.");
            Data.RemoveElement(ind);
        }

        public bool IsProbExists(string name)
        {
            for (int i = 0; i < GetProbCount(); i++)
            {
                MethodSimpleProb fpr = GetProbHeader(i);
                if (fpr.Name.Equals(name) == true)
                    return true;
            }
            return false;
        }

        public void AddStandart(MethodSimpleProb msp)
        {
            int ret = Data.AddRow(msp,null);

            for (int e = 0; e < Data.GetColumnCount(); e++)
                Data[e, ret] = new MethodSimpleCell();
        }

        public int AddStandart(string st_name, int st_index, StLib lib,string prob_name)
        {
            if (st_index == 0)            
            {
                if(IsProbExists(st_name))
                    throw new Exception(Common.MLS.Get(MLSConst,"Добавляетмая проба уже существует: ")+st_name);
            }
            string name;
            if (lib != null)
            {
                if (prob_name == null)
                {
                    int ind = st_name.LastIndexOf('\\');
                    name = st_name.Substring(ind + 1);// +"[" + st_index + "]";
                    ind = name.LastIndexOf('.');
                    name = name.Substring(0, ind) + "[" + (st_index + 1) + "]";
                }
                else
                    name = prob_name;
            }
            else
                name = st_name;
            
            int ret;
            if (lib != null)
                ret = Data.AddRow(new MethodSimpleProb(name, "", st_name, st_index, this),
                                    null);
            else
                ret = Data.InsetRowInto(0,new MethodSimpleProb(name, "", st_name, st_index, this),
                                    null);

            MethodSimpleProb pr = Data.GetRowHeader(ret);
            pr.MeasuredSpectrs.Add(new MethodSimpleProbMeasuring(pr));
            if (lib != null)
                ReloadAllCons();
            else
            {
                for (int e = 0; e < Data.GetColumnCount(); e++)
                    Data[e, ret] = new MethodSimpleCell();
            }
            return ret;
        }

        public int AddStandartCopy(int prob)
        {
            MethodSimpleProb pr = Data.GetRowHeader(prob);
            int ret = pr.MeasuredSpectrs.Count;
            pr.MeasuredSpectrs.Add(new MethodSimpleProbMeasuring(pr));
            return ret;
        }

        public int AddElement(int elem_index)
        {
            for (int el = 0; el < Data.GetColumnCount(); el++)
                if (Data.GetColHeader(el).ElementIndex == elem_index)
                    throw new Exception("Невозможно добавить уже существующий элемент.");
            MethodSimpleElement e = new MethodSimpleElement(elem_index);
            int ret = Data.AddCol(e, new MethodSimpleCell());
            
            int pc = Data.GetRowCount();
            for (int p = 0; p < pc; p++)
            {
                MethodSimpleProb mp = GetProbHeader(p);
                StLibStandart stand = mp.Standart;
                Data[ret, p] = new MethodSimpleCell();
            }

            AddFormula(ret, Common.MLS.Get(MLSConst, "BaseF"));


            return ret;
        }

        public int AddFormula(int element,string name)
        {
            int pr_n = Data.GetRowCount();
            MethodSimpleElement ms = Data.GetColHeader(element);
            int index = ms.Formula.Count;
            ms.Formula.Add(new MethodSimpleElementFormula(name,this,ms));
            ReloadAllCons();
            return index;
        }

        public int GetProbCount()
        {
            return Data.GetRowCount();
        }

        public int GetSubProbCount(int prob)
        {
            return Data.GetRowHeader(prob).MeasuredSpectrs.Count;
        }

        public int GetElementCount()
        {
            if (Data == null)
                return 0;
            return Data.GetColumnCount();
        }

        public int GetFormulaCount(int col)
        {
            return Data.GetColHeader(col).Formula.Count;
        }

        public MethodSimpleCell GetCell(int col, int row)
        {
            return Data[col, row];
        }

        public MethodSimpleElement GetElHeader(int el)
        {
            return Data.GetColHeader(el);
        }

        public MethodSimpleProb GetProbHeader(int pr)
        {
            return Data.GetRowHeader(pr);
        }

        public void ClearProbRecords()
        {
            while(Data.GetRowCount() > 0)
                Data.RemoveRow(0);
        }

        public void RemoveProb(int pr)
        {
            MethodSimpleProb prob = Data.GetRowHeader(pr);
            Data.RemoveRow(pr);
            for (int m = 0; m < prob.MeasuredSpectrs.Count; m++)
                prob.MeasuredSpectrs[m].DeleteSpectr();
                //if (prob.MeasuredSpectrs[m].Sp != null)
                //    prob.MeasuredSpectrs[m].Sp.Remove();
        }

        public void RemoveSubProb(int pr, int sub_pr, int sub_pr_count)
        {
            int elc = Data.GetColumnCount();
            for (int el = 0; el < elc; el++)
            {
                int fc = Data.GetColHeader(el).Formula.Count;
                for (int f = 0; f < fc; f++)
                {
                    int pc = Data.GetRowCount();
                    this.GetCell(el, pr).ClearCons(sub_pr_count);
                }
            }
            MethodSimpleProb msp = Data.GetRowHeader(pr);
            MethodSimpleProbMeasuring sp = msp.MeasuredSpectrs[sub_pr];
            msp.MeasuredSpectrs.RemoveAt(sub_pr);
            try
            {
                sp.Sp.Remove();
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }
    }

    public class MethodSimpleTable : DataTable<MethodSimpleInfo, MethodSimpleProb, MethodSimpleElement, MethodSimpleCell>
    {
        MethodSimple Method;
        public MethodSimpleTable(MethodSimple method)
        {
            Method = method;
        }

        public MethodSimpleTable(BinaryReader br,MethodSimple method)
        {
            Method = method;
            Load(br);
        }

        #region saving methods
        protected override MethodSimpleElement LoadCol(BinaryReader br)
        {
            return new MethodSimpleElement(br,Method);
        }

        protected override MethodSimpleCell LoadData(BinaryReader br)
        {
            return new MethodSimpleCell(br);
        }

        protected override MethodSimpleInfo LoadHeader(BinaryReader br)
        {
            byte val = br.ReadByte();
            if (val == 0)
                return null;
            return new MethodSimpleInfo(br);
        }

        protected override MethodSimpleProb LoadRow(BinaryReader br)
        {
            return new MethodSimpleProb(br,Method);
        }

        protected override void SaveCol(BinaryWriter bw, MethodSimpleElement data)
        {
            data.Save(bw);
        }

        protected override void SaveData(BinaryWriter bw, MethodSimpleCell data)
        {
            data.Save(bw);
        }

        protected override void SaveHeader(BinaryWriter bw, MethodSimpleInfo data)
        {
            if (data == null)
                bw.Write((byte)0);
            else
            {
                bw.Write((byte)1);
                data.Save(bw);
            }
        }

        protected override void SaveRow(BinaryWriter bw, MethodSimpleProb data)
        {
            data.Save(bw);
        }
        #endregion
    }

    public class MethodSimpleInfo
    {
        public string Description = "";
        public string Caution = "";
        public SpectrCondition MatrixCond = new SpectrCondition();
        public SpectrCondition WorkingCond = new SpectrCondition();

        public MethodSimpleInfo()
        {
            //MatrixCond = new SpectrCondition();
            //WorkingCond = new SpectrCondition();
            //MatrixCond.Compile(SpectrCondition.GetDefaultCondition());
            //WorkingCond.Compile(SpectrCondition.GetDefaultCondition());
        }

        public MethodSimpleInfo(BinaryReader br)
        {
            Load(br);
        }

        #region Save/Load methods
        public void Load(BinaryReader br)
        {
            //long from = br.BaseStream.Position;
            string tmp = br.ReadString();
            if (tmp.Equals("msi") == false)
                throw new Exception("MethodSimpleInfo:Wrong record type");
            int ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleInfo:Unsupported version");
            
            //public string Description;
            Description = br.ReadString();
            //public string Caution;
            Caution = br.ReadString();
            //public SpectrCondition MatrixCond;
            MatrixCond = new SpectrCondition(br);
            WorkingCond = new SpectrCondition(br);

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleInfo:Unsupported end version");
            //Common.Log("MethodSimpleInfo=" + (br.BaseStream.Position - from));
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write("msi");
            bw.Write(0);

            //public string Description;
            bw.Write(Description);
            //public string Caution;
            bw.Write(Caution);
            //public SpectrCondition MatrixCond;
            MatrixCond.Save(bw);
            WorkingCond.Save(bw);

            bw.Write(0);
        }
        #endregion
    }

    public class MethodSimpleProb
    {
        public string Name = "";
        public string Comments = "";
        public List<MethodSimpleProbMeasuring> MeasuredSpectrs = new List<MethodSimpleProbMeasuring>();
        public string StLibPath = null;
        public int StIndex = -1;
        public int SpectrGlobalIndex = -1; //Global index for spectrs
        //public string StLibPath = null;
        //public int StLibIndex = 0;

        public bool IsStandart
        {
            get
            {
                return StIndex >= 0 && StLibPath != null && StLibPath.Length > 0 && StLibPath.IndexOf(Path.DirectorySeparatorChar) > 0;
            }
        }

        public string GetDebugReport()
        {
            string endl = "" + (char)0xD + (char)0xA;
            string ret = "";
            ret += "--------Проба '" + Name + "' '" + Comments + "'"+endl+
                "Промеров:" + MeasuredSpectrs.Count;
            ret += " реально промерянных: ";
            int tmp = 0;
            for (int i = 0; i < MeasuredSpectrs.Count; i++)
                if (MeasuredSpectrs[i].Sp != null)
                    tmp++;
            ret += tmp;
            ret += endl;
            for (int i = 0; i < MeasuredSpectrs.Count; i++)
            {
                if (MeasuredSpectrs[i].Sp != null)
                    ret += " " + MeasuredSpectrs[i].Sp.GetPath();
                else
                    ret += "-";
                ret += endl;
            }
            ret += endl;
            ret += "StLibP='" + StLibPath + "' StIndex=" + StIndex + " SpGlobIndex=" + SpectrGlobalIndex+endl;
            return ret;
        }

        public string GetNewSpectrName()
        {
            SpectrGlobalIndex++;
            return Method.Path + "prob_" + Name + "_" + SpectrGlobalIndex;
        }

        public StLibStandart Standart
        {
            get
            {
                return new StLib(StLibPath)[StIndex];
            }
        }

        public void Rename(string new_name)
        {
            Name = new_name;
            for (int sp = 0; sp < MeasuredSpectrs.Count; sp++)
                MeasuredSpectrs[sp].Rename(new_name,sp);
        }

        public MethodSimple Method;
        public MethodSimpleProb(string name,string comments,string path,int index,
            MethodSimple method)
        {
            Name = name;
            Comments = comments;
            StLibPath = path;
            StIndex = index;
            Method = method;
        }

        public MethodSimpleProb(BinaryReader br, MethodSimple method)
        {
            Load(br);
            Method = method;
        }

        #region Save/Load methods
        public void Load(BinaryReader br)
        {
            //long from = br.BaseStream.Position;
            string tmp = br.ReadString();
            if (tmp.Equals("msp") == false)
                throw new Exception("MethodSimpleProb:Wrong record type");
            int ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleProb:Unsupported version");
            
            //public string Name;
            Name = br.ReadString();
            //public string Comments;
            Comments = br.ReadString();
            StLibPath = br.ReadString();
            StIndex = br.ReadInt32();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                MeasuredSpectrs.Add(new MethodSimpleProbMeasuring(this,br));
            //SpectrGlobalIndex
            SpectrGlobalIndex = br.ReadInt32();

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleProb:Unsupported end version");
            //Common.Log("MethodSimpleProb=" + (br.BaseStream.Position - from));
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write("msp");
            bw.Write(0);
            
            //public string Name;
            bw.Write(Name);
            //public string Comments;
            bw.Write(Comments);
            bw.Write(StLibPath);
            bw.Write(StIndex);
            bw.Write(MeasuredSpectrs.Count);
            for (int i = 0; i < MeasuredSpectrs.Count; i++)
                MeasuredSpectrs[i].Save(bw);
            bw.Write(SpectrGlobalIndex);

            bw.Write(0);
        }
        #endregion
    }

    public class MethodSimpleProbMeasuring
    {
        MethodSimpleProb Prob;
        public double[] SpRates;
        long SpectrTicks = 0;
        
        public DateTime SpDateTime
        {
            get
            {
                if (SpectrTicks == 0)
                {
                    SpectroWizard.data.Spectr sp = Sp;
                }
                return new DateTime(SpectrTicks);
            }
        }

        public double CalcSpRate()
        {
            if (SpRates == null || SpRates.Length == 0)
                return -1;
            double ret = analit.Stat.GetEver(SpRates, SpectroWizard.analit.Stat.SpectrDataSKO);
            ret = 2 - ret;
            if (ret < 0.1)
                ret = 0.1;
            return ret;
        }

        List<ExtraLineTester.ExtraLineInfo> ExtraLines = new List<ExtraLineTester.ExtraLineInfo>();
        public void CalcSpExtraLines()
        {
            if (Prob.Method.ExtraLineTester != null)
                ExtraLines = Prob.Method.ExtraLineTester.CheckSpectr(Sp);
        }

        public string GetExtraLineInfo()
        {
            if (ExtraLines.Count == 0)
                return "";
            return "нл."+ExtraLines.Count;
        }

        public string CalcSpRateStr()
        {
            if (SpRates == null || SpRates.Length == 0)
                return "-";
            double ret = analit.Stat.GetEver(SpRates, SpectroWizard.analit.Stat.SpectrDataSKO);
            if (ret == -1 || serv.IsValid(ret) == false)
                return "-";
            switch ((int)(ret*4))
            {
                case 0: return "5+";
                case 1: return "5";
                case 2: return "5-";
                case 3: return "4+";
                case 4: return "4";
                case 5: return "4-";
                case 6: return "3+";
                case 7: return "3";
                case 8: return "3-";
                case 9: return "2+";
                case 10: return "2";
                case 11: return "2-";
                case 12: return "1+";
                case 13: return "1";
                case 14: return "1-";
            }
            return "X";
        }
        public MethodSimpleProbMeasuring(MethodSimpleProb prob)
        {
            Prob = prob;
        }

        public MethodSimpleProbMeasuring(MethodSimpleProb prob,BinaryReader br)
        {
            Prob = prob;
            Load(br);
        }

        public string GetDebugReport()
        {
            string endl = "" + (char)0xD + (char)0xA;
            string ret = "";
            ret += "--------Промерянный спектр:" + endl + "Путь: '" + SpectrPath + "'" + endl;
            ret += "Оценки прожега: ";
            if (SpRates == null || SpRates.Length == 0)
                ret += "Механизм оценок не применялся...";
            else
            {
                for (int i = 0; i < SpRates.Length; i++)
                    ret += serv.GetGoodValue(SpRates[i],3) + " ";
            }
            ret += serv.Endl;
            return ret;
        }

        public void Rename(string new_name,int sp_ind)
        {
            Spectr sp = Sp;
            string name = "prob_" + new_name + "_" + sp_ind;
            sp.Rename(name);
            //int ind = SpectrLocalPath.IndexOf(Path.DirectorySeparatorChar);
            //string path = SpectrLocalPath.Substring(0, ind+1);
            //path += new_name;
            SpectrLocalPath = name;
        }

        public string SpectrPath
        {
            get
            {
                //DataBase.CheckPath(ref SpectrLocalPath);
                //CheckLocalPath();
                //return Common.Db.GetFoladerPath(SpectrLocalPath);
                return SpectrLocalPath;
            }
            set
            {
                SpectrLocalPath = (string)value.Clone();
                DataBase.CheckPath(ref SpectrLocalPath);
                //CheckLocalPath();
            }
        }

        string ExtractPath(string path)
        {
            if (path == null)
                return null;
            int i = path.LastIndexOf("\\");
            if (i < 0)
                return path;
            return path.Substring(i+1);
        }

        string GetSpectrPath()
        {
            if (SpectrPath == null)
                return null;
            return Prob.Method.Path + ExtractPath(SpectrPath);
        }

        public void DeleteSpectr()
        {
            try
            {
                string sp_path = GetSpectrPath();
                Spectr.RemoveSpectr(sp_path);
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        public string SpectrLocalPath = null;
        Spectr SpPriv = null;
        string SpPrivDateTime = "";
        public Spectr Sp
        {
            get
            {
                if (SpPriv == null || SpPrivDateTime == null)
                {
                    string sp_path = GetSpectrPath();//SpectrPath;
                    if (sp_path == null || Spectr.IsFileExists(sp_path) == false)
                        return null;
                    SpPriv = Common.Db.LoadSpectr(sp_path);// new Spectr(SpectrPath);
                    SpPrivDateTime = Spectr.GetFileDateTime(sp_path);
                    SpectrTicks = Spectr.GetFileDateTimeTmp.Ticks;
                }
                else
                {
                    string sp_path = GetSpectrPath();//SpectrPath;
                    if (SpPrivDateTime.Equals(Spectr.GetFileDateTime(sp_path)) == false)
                    {
                        SpPriv = Common.Db.LoadSpectr(sp_path);////new Spectr(SpectrPath);
                        SpPrivDateTime = Spectr.GetFileDateTime(sp_path);
                        SpectrTicks = Spectr.GetFileDateTimeTmp.Ticks;
                    }
                }
                SpectrTicks = SpPriv.CreatedDate.Ticks;
                return SpPriv;
            }
        }

        static Random Rnd = new Random();
        void AddNoise(Spectr sp)
        {
            List<SpectrDataView> list = sp.GetViewsSet();
            for (int v = 0; v < list.Count; v++)
            {
                SpectrDataView view = list[v];
                float[][] data = view.GetFullDataNoClone();
                for (int s = 0; s < data.Length; s++)
                {
                    for (int pixel = 0; pixel < data[s].Length; pixel++)
                    {
                        data[s][pixel] += Rnd.Next(20);
                    }
                }
            }
        }

        String FindNearest(String path, String name, int measuring)
        {
            for (int i = measuring; i >= 0; i--)
            {
                String file = Common.Db.GetFoladerPath(path + "debug\\d" + name + "_" + i + ".ss");
                if (File.Exists(file))
                    return file;
            }
            return null;
        }

        public Spectr getDebugSpectr(String spectrName,int measuring)
        {
            try
            {
                String path = Prob.GetNewSpectrName();
                int ind = path.LastIndexOf("\\") + 1;
                path = path.Substring(0, ind);
                //String file = Common.Db.GetFoladerPath(path+"debug\\d" + spectrName[spectrName.Length - 1] + "_"+measuring+".ss");
                String file = FindNearest(path, "" + spectrName[spectrName.Length - 1], measuring);
                if (file != null)
                {
                }
                else
                {
                    int measuring_count = 0;
                    //while (File.Exists(file = Common.Db.GetFoladerPath(path + "debug\\d" + measuring_count + "_" + measuring + ".ss")))
                    while(FindNearest(path,""+measuring_count,measuring) != null)
                        measuring_count++;
                    if (measuring_count == 0)
                        return null;
                    int spectr = spectrName.GetHashCode() % measuring_count;
                    if (spectr == measuring_count)
                        spectr = 0;
                    file = FindNearest(path, "" + spectr, measuring); //Common.Db.GetFoladerPath(path + "debug\\d" + spectr + "_" + measuring);
                }
                Spectr sp =  Common.Db.LoadSpectr(file);
                AddNoise(sp);
                return sp;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public void SetSp(Spectr sp,bool need_save)//,bool correct_ly)
        {
            if (SpPriv != null)
                SpectrPath = ExtractPath(SpPriv.GetPath());
            else
                SpectrPath = ExtractPath(Prob.GetNewSpectrName());
            SpPriv = sp;
            /*if (correct_ly == true)
            {
                SpPriv.MakeLyCorrection(Prob.Method.SpLy);
                SpPriv.SaveAs(SpectrPath);
            }*/
            //CheckLocalPath();
            SpectrTicks = sp.CreatedDate.Ticks;
            if(need_save)
                SpPriv.SaveAs(GetSpectrPath());
        }

        #region Save/Load methods
        public void Load(BinaryReader br)
        {
            //long from = br.BaseStream.Position;
            string tmp = br.ReadString();
            if (tmp.Equals("mspm") == false)
                throw new Exception("MethodSimpleProbMeasuring:Wrong record type");
            int ver = br.ReadInt32();
            if (ver < 0 && ver > 2)
                throw new Exception("MethodSimpleProbMeasuring:Unsupported version");

            //public string SpectrPath;
            if (br.ReadByte() == 1)
            {
                SpectrLocalPath = ExtractPath(br.ReadString());
                //DataBase.CheckPath(ref SpectrLocalPath);
            }
            else
                SpectrLocalPath = null;

            if (ver >= 1)
            {
                int c = br.ReadInt32();
                if (c != 0)
                {
                    SpRates = new double[c];
                    for (int i = 0; i < c; i++)
                        SpRates[i] = br.ReadDouble();
                }
                c = br.ReadInt32();
                if (c != 0)
                {
                    ExtraLines.Clear();
                    for (int i = 0; i < c; i++)
                        ExtraLines.Add(new ExtraLineTester.ExtraLineInfo(br));// [i] = br.ReadDouble();
                }
            }
            else
            {
                SpRates = null;
                ExtraLines = new List<ExtraLineTester.ExtraLineInfo>();
            }

            if(ver >= 2)
                SpectrTicks = br.ReadInt64();

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleProbMeasuring:Unsupported end version");
            //Common.Log("MethodSimpleProbMeasuring=" + (br.BaseStream.Position - from));
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write("mspm");
            bw.Write(2);

            //public string SpectrPath;
            if (SpectrLocalPath == null)
                bw.Write((byte)0);
            else
            {
                bw.Write((byte)1);
                bw.Write(SpectrLocalPath);
            }

            if (SpRates != null && SpRates.Length != 0)
            {
                bw.Write(SpRates.Length);
                foreach (double tmp in SpRates)
                    bw.Write(tmp);
            }
            else
                bw.Write(0);

            if (ExtraLines != null)
            {
                bw.Write(ExtraLines.Count);
                foreach (ExtraLineTester.ExtraLineInfo li in ExtraLines)
                    li.Save(bw);
            }
            else
                bw.Write(0);

            bw.Write(SpectrTicks);
            bw.Write(0);
        }
        #endregion
    }

    public class MethodSimpleElement
    {
        public int ElementIndex = -1;
        public Element Element
        {
            get
            {
                return ElementTable.Elements[ElementIndex];
            }
        }
        public List<MethodSimpleElementFormula> Formula = new List<MethodSimpleElementFormula>();

        public int FindFreeFormulaIndex()
        {
            for (int ind = 0; ind < 1000; ind++)
            {
                bool found = false;
                for (int i = 0; i < Formula.Count; i++)
                {
                    if (Formula[i].FormulaIndex == ind)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                    return ind;
            }
            throw new Exception("Can't find free formulf index");
        }

        public string GetDebugReport()
        {
            string endl = "" + (char)0xD + (char)0xA;
            string ret = "";
            ret += "--------Химический элемент: " + Element.Name + " ("+Element.FullName+")";
            //ret += "    ";
            ret += "Количество формул: " + Formula.Count + endl + "Перечень: ";
            for (int i = 0; i < Formula.Count; i++)
                ret += Formula[i].Name+" ";
            //ret += endl;
            return ret;
        }

        public MethodSimpleElement(int element_index)
        {
            ElementIndex = element_index;
        }

        public MethodSimpleElement(BinaryReader br,MethodSimple method)
        {
            Load(br,method);
        }

        #region Save/Load methods
        public void Load(BinaryReader br,MethodSimple method)
        {
            //long from = br.BaseStream.Position;
            string tmp = br.ReadString();
            if (tmp.Equals("mse") == false)
                throw new Exception("MethodSimpleElement:Wrong record type");
            int ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleElement:Unsupported version");
            
            //public int ElementIndex;
            ElementIndex = br.ReadInt32();
            //public List<SimpleFormula> Formula
            int count = br.ReadInt32();
            for(int i = 0 ; i < count ; i++)
                Formula.Add(new MethodSimpleElementFormula(br, method, this));

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleElement:Unsupported end version");
            //Common.Log("MethodSimpleElement=" + (br.BaseStream.Position - from));
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write("mse");
            bw.Write(0);
            
            //public int ElementIndex;
            bw.Write(ElementIndex);
            //public List<SimpleFormula> Formula = new List<SimpleFormula>()
            bw.Write(Formula.Count);
            for (int i = 0; i < Formula.Count; i++)
                Formula[i].Save(bw);

            bw.Write(0);
        }
        #endregion
    }

    public class MethodSimpleElementFormula
    {
        public string Name = "";
        public int FormulaIndex = 0;
        public SimpleFormula Formula;
        public List<GLogRecord> CalibGraphicsLog = new List<GLogRecord>();
        //int Prob = 0, SubProb = 0;
        public double ConMin = 0,ConMax = 30;


        //public double Base100MinusCon = 0;

        public string GetDebugReport()
        {
            string endl = "" + (char)0xD + (char)0xA;
            string ret = "";
            ret += "--------Формула элемента: '" + Name + "' fi = " + FormulaIndex + endl;
            ret += Formula.GetDebugReport() + endl;
            return ret;
        }

        MethodSimpleElement Parent;
        public MethodSimpleElementFormula(string name, MethodSimple method, MethodSimpleElement el)
        {
            Parent = el;
            Formula = new SimpleFormula(method);
            FormulaIndex = el.FindFreeFormulaIndex();
            Name = name;
            Formula.Element = el.ElementIndex;
            if (Formula.Element <= 0)
                throw new Exception("Wrong element index...");
            if (method == null)
                throw new Exception("Method must not be NULL!");
        }

        public MethodSimpleElementFormula(BinaryReader br, MethodSimple method, MethodSimpleElement el)
        {
            Parent = el;
            Formula = new SimpleFormula(method);
            Load(br,method);
            Formula.Element = el.ElementIndex;
            if (Formula.Element <= 0)
                throw new Exception("Wrong element index...");
            if (method == null)
                throw new Exception("Method must not be NULL!");
        }

        #region Save/Load methods
        public void Load(BinaryReader br,MethodSimple method)
        {
            //long from = br.BaseStream.Position;
            string tmp = br.ReadString();
            if (tmp.Equals("msef") == false)
                throw new Exception("MethodSimpleElementFormula:Wrong record type");
            int ver = br.ReadInt32();
            if (ver < 0 || ver > 3)
                throw new Exception("MethodSimpleElementFormula:Unsupported version");

            //public string Name = "";
            Name = br.ReadString();
            if (ver >= 1)
                FormulaIndex = br.ReadInt32();
            //public SimpleFormula Formula = new SimpleFormula();
            Formula = new SimpleFormula(br, method);
            //List<GLogRecord> CalibGraphics = new List<GLogRecord>();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                CalibGraphicsLog.Add(GLogRecord.Load(br));

            if (ver >= 2)
            {
                ConMin = br.ReadDouble();
                ConMax = br.ReadDouble();
            }

            if (ver >= 3)
                br.ReadDouble();

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleElementFormula:Unsupported end version");
            //Common.Log("MethodSimpleElementFormula=" + (br.BaseStream.Position - from));
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write("msef");
            bw.Write(2);

            //public string Name = "";
            bw.Write(Name);
            bw.Write(FormulaIndex);
            //public SimpleFormula Formula = new SimpleFormula();
            Formula.Save(bw);
            //List<GLogRecord> CalibGraphics = new List<GLogRecord>();
            bw.Write(CalibGraphicsLog.Count);
            for (int i = 0; i < CalibGraphicsLog.Count; i++)
                CalibGraphicsLog[i].Save(bw);

            bw.Write(ConMin);
            bw.Write(ConMax);

            //bw.Write(Base100MinusCon); //ver=3

            bw.Write(0);
        }
        #endregion
    }

    public class MethodSimpleCell
    {
        public double Con100Minus = -1;
        public int Con100MinuFormulaIndex = -1;
        public double Con = 0;
        List<double[]> SpRates_ = new List<double[]>();
        public bool Enabled = true;
        List<MethodSimpleCellFormulaResult> Results =
            new List<MethodSimpleCellFormulaResult>();
        public string Prefix = "", Sufix = "";

        
        public void ClearAllCons()
        {
            Results.Clear();
        }

        public void ResetSpRates()
        {
            foreach (MethodSimpleCellFormulaResult r in Results)
                r.SparkRates = null;
        }

        public void applyDlt(double dlt)
        {
            foreach (MethodSimpleCellFormulaResult r in Results)
            {
                for (int i = 0; i < r.ReCalcCon.Length; i++)
                    r.ReCalcCon[i] += dlt;
            }
        }

        public void applyCon(double con,int measuring_count)
        {
            double sko, gsko;
            double ever = CalcRealCon(out sko, out gsko);
            //int step;
            foreach (MethodSimpleCellFormulaResult r in Results)
            {
                double ever_r = SpectroWizard.analit.Stat.GetEver(r.ReCalcCon, SpectroWizard.analit.Stat.SpectrDataSKO);
                sko = 0;
                foreach (double val in r.ReCalcCon)
                    sko += Math.Abs(ever_r - val);
                sko /= r.ReCalcCon.Length;
                Random rnd = new Random(r.AnalitValue[0].GetHashCode());
                double dlt = con - ever_r;
                if (sko > con / 10)
                    sko = con / 10;
                if (sko < con / 20)
                    sko = con / 20;
                dlt += (rnd.NextDouble() - 0.5) * sko;
                for (int i = 0; i < r.ReCalcCon.Length; i++)
                    r.ReCalcCon[i] += dlt;
            }
        }

        public double CalcRealConWithRates(out double sko, out double good_sko)
        {
            if (Con100Minus >= 0)
            {
                sko = 0;
                good_sko = 0;
                return Con100Minus;
            }//*/
            double[] cons = GetEnabledCons(-1);
            double ever = SpectroWizard.analit.Stat.GetEver(cons, SpRates, SpectroWizard.analit.Stat.SpectrDataSKO);
            sko = SpectroWizard.analit.Stat.LastSKO;
            good_sko = SpectroWizard.analit.Stat.LastGoodSKO;
            return ever;
        }

        public double CalcRealCon(out double sko, out double good_sko)
        {
            if (Con100Minus >= 0)
            {
                sko = 0;
                good_sko = 0;
                return Con100Minus;
            }//*/
            double ever = SpectroWizard.analit.Stat.GetEver(GetEnabledCons(-1), SpectroWizard.analit.Stat.SpectrDataSKO);
            sko = SpectroWizard.analit.Stat.LastSKO;
            good_sko = SpectroWizard.analit.Stat.LastGoodSKO;
            return ever;
        }

        public double CalcRealCon(int formula,out double sko, out double good_sko)
        {
            if (Con100Minus >= 0)
            {
                sko = 0;
                good_sko = 0;
                return Con100Minus;
            }//*/
            double ever = SpectroWizard.analit.Stat.GetEver(GetEnabledCons(formula), SpectroWizard.analit.Stat.SpectrDataSKO);
            sko = SpectroWizard.analit.Stat.LastSKO;
            good_sko = SpectroWizard.analit.Stat.LastGoodSKO;
            return ever;
        }

        public double CalcRealPrelimConWithRates(out double sko)
        {
            double[] cons = GetCons();
            double ever = SpectroWizard.analit.Stat.GetEver(cons, SpRates, SpectroWizard.analit.Stat.SpectrDataSKO);
            sko = SpectroWizard.analit.Stat.LastSKO;
            return ever;
        }

        public double CalcRealPrelimCon(out double sko)
        {
            double ever = SpectroWizard.analit.Stat.GetEver(GetCons(), SpectroWizard.analit.Stat.SpectrDataSKO);
            sko = SpectroWizard.analit.Stat.LastSKO;
            return ever;
        }

        public string GetDebugReport()
        {
            string endl = "" + (char)0xD + (char)0xA;
            string ret = "";
            ret += "--------Сумма всех промеров: " + endl;
            double[] vals = GetEnabledCons(-1);
            if (vals != null && vals.Length > 0)
            {
                double sko, good_sko;
                double ever = CalcRealCon(out sko, out good_sko);
                ret += "Заданная концентрация = " + Con + endl;
                ret += "Исходные данные:"+endl;
                for (int i = 0; i < vals.Length; i++)
                {
                    ret += "    " + vals[i];
                    if (SpRates == null || i >= SpRates.Length)
                        ret += " - ";
                    else
                        ret += " " + serv.GetGoodValue(SpRates[i], 3)+" "+serv.GetGoodValue(1/(1+SpRates[i]),3);
                    if(analit.Stat.Used[i] == true)
                        ret += " + ";
                    else
                        ret += " неисп. ";
                    ret += serv.Endl;
                }
                ret += " Среднее = " + ever+endl;
                ret += " СКО = " + sko + " " + (sko * 100 / ever) + "%";
                if(Con != 0)
                    ret += " к заданному " + (sko * 100 / Con) + "%";
                ret += endl;
                ret += " Хор.СКО = " + good_sko + " " + (good_sko * 100 / ever) + "%";
                if (Con != 0)
                    ret += " к заданному " + (good_sko * 100 / Con) + "%" + endl;
                ret += endl;
            }
            ret += serv.Endl;
            return ret;
        }

        public void ClearCons(MethodSimpleElement el)
        {
            for (int r = Results.Count - 1; r >= 0; r--)
            {
                MethodSimpleCellFormulaResult msfr = Results[r];
                bool found = false;
                for (int f = 0; f < el.Formula.Count; f++)
                {
                    string tmp = msfr.Key.Substring(msfr.Key.IndexOf("-") + 1);
                    if (tmp.Equals(el.Formula[f].Name))
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                    Results.RemoveAt(r);
            }
        }

        public void ClearCons(int sub_pr_count)
        {
            //string strk = sub_pr_count.ToString();
            for (int r = Results.Count - 1; r >= 0; r--)
            {

                MethodSimpleCellFormulaResult msfr = Results[r];
                string tmp = msfr.Key.Substring(0,msfr.Key.IndexOf("-"));
                int ind = int.Parse(tmp);
                if (ind >= sub_pr_count)
                    Results.RemoveAt(r);
            }
        }

        double[] SpRates;
        double[] GetCons()
        {
            int r_count = 0;
            for (int r = 0; r < Results.Count; r++)
            {
                if (Results[r].ReCalcCon == null)
                    continue;
                int n = Results[r].ReCalcCon.Length;
                for (int ri = 0; ri < n; ri++)
                {
                    if (Results[r].ReCalcCon[ri] < Results[r].ConFrom ||
                        Results[r].ReCalcCon[ri] > Results[r].ConTo)
                        continue;
                    r_count++;
                }
            }
            SpRates = new double[r_count];
            double[] ret = new double[r_count];
            r_count = 0;
            for (int r = 0; r < Results.Count; r++)
            {
                if (Results[r].ReCalcCon == null)
                    continue;
                int n = Results[r].ReCalcCon.Length;
                for (int ri = 0; ri < n; ri++)
                {
                    if (Results[r].ReCalcCon[ri] < Results[r].ConFrom ||
                        Results[r].ReCalcCon[ri] > Results[r].ConTo)
                        continue;
                    ret[r_count] = Results[r].ReCalcCon[ri];
                    if (Results[r].SparkRates != null && Results[r].SparkRates.Length > ri)
                        SpRates[r_count] = Results[r].SparkRates[ri];
                    else
                        SpRates[r_count] = 1;
                    r_count++;
                }
            }
            return ret;
        }

        /*public double getConFor(int prob,int shortIndex,int formula)
        {
            String keyCand = "" + prob + "-" + formula;
            for (int r = 0; r < Results.Count; r++)
            {
                String key = Results[r].Key;
                if (keyCand.Equals(key))
                {
                    if(shortIndex == 0)
                        return Results[r].AnalitAq[]
                }
            }
            return 0;
        }*/

        double[] GetEnabledCons(int formula)
        {
            int r_count = 0;
            for (int r = 0; r < Results.Count; r++)
            {
                if (formula >= 0)
                {
                    string p = "-" + formula;
                    if (Results[r].Key.IndexOf(p) < 0)
                        continue;
                }
                if (Results[r].ReCalcCon == null || Results[r].Enabled == false)
                    continue;
                int n = Results[r].ReCalcCon.Length;
                for (int ri = 0; ri < n; ri++)
                {
                    if (Results[r].ReCalcCon[ri] < Results[r].ConFrom ||
                        Results[r].ReCalcCon[ri] > Results[r].ConTo)
                        continue;
                    r_count++;
                }
            }
            SpRates =  new double[r_count];
            double[] ret = new double[r_count];
            r_count = 0;
            for (int r = 0; r < Results.Count; r++)
            {
                if (formula >= 0)
                {
                    string p = "-" + formula;
                    if (Results[r].Key.IndexOf(p) < 0)
                        continue;
                }
                if (Results[r].ReCalcCon == null || Results[r].Enabled == false)
                    continue;
                int n = Results[r].ReCalcCon.Length;
                for (int ri = 0; ri < n; ri++)
                {
                    if (Results[r].ReCalcCon[ri] < Results[r].ConFrom ||
                        Results[r].ReCalcCon[ri] > Results[r].ConTo)
                        continue;
                    ret[r_count] = Results[r].ReCalcCon[ri];
                    if (Results[r].SparkRates != null && Results[r].SparkRates.Length > ri)
                        SpRates[r_count] = Results[r].SparkRates[ri];
                    else
                        SpRates[r_count] = 1;
                    r_count++;
                }
            }
            return ret;
        }

        public void Clear()
        {
            Results.Clear();
        }

        public MethodSimpleCellFormulaResult GetData(int sub_prob, int formula_index)
        {
            string key = "" + sub_prob + "-" + formula_index;
            for (int i = 0; i < Results.Count; i++)
                if (key.Equals(Results[i].Key))
                    return Results[i];
            MethodSimpleCellFormulaResult fr = new MethodSimpleCellFormulaResult(key,this);
            Results.Add(fr);
            return fr;
        }

        public MethodSimpleCell()
        {
        }

        public MethodSimpleCell(BinaryReader br)
        {
            Load(br);
        }

        #region Save/Load methods
        public void Load(BinaryReader br)
        {
            //long from = br.BaseStream.Position;
            string tmp = br.ReadString();
            if (tmp.Equals("msc") == false)
                throw new Exception("MethodSimpleCalc:Wrong record type");
            int ver = br.ReadInt32();
            if (ver < 0 || ver > 3)
                throw new Exception("MethodSimpleCalc:Unsupported version");

            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                Results.Add(new MethodSimpleCellFormulaResult(br,this));
            Con = br.ReadDouble();
            //Sko = br.ReadDouble();
            Enabled = br.ReadBoolean();

            if (ver == 1)
            {
                count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int len = br.ReadInt32();
                    if (len > 0)
                    {
                        double[] dtmp = new double[len];
                        for (int j = 0; j < len; j++)
                            dtmp[j] = br.ReadDouble();
                        SpRates_.Add(dtmp);
                    }
                    else
                        SpRates_.Add(null);
                }
            }

            if (ver >= 2)
            {
                Prefix = br.ReadString();
                Sufix = br.ReadString();
            }

            if (ver >= 3)
            {
                Con100Minus = br.ReadDouble();
            }

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleCalc:Unsupported end version");
            //Common.Log("MethodSimpleCalc=" + (br.BaseStream.Position - from));
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write("msc");
            bw.Write(3);

            //public List<MethodSimpleFormulaCalcDetails> FormulaRes = new List<MethodSimpleFormulaCalcDetails>();
            bw.Write(Results.Count);
            for (int i = 0; i < Results.Count; i++)
                Results[i].Save(bw);
            bw.Write(Con);
            //bw.Write(Sko);
            bw.Write(Enabled);

            bw.Write(Prefix);
            bw.Write(Sufix);
            bw.Write(Con100Minus);

            bw.Write(0);
        }
        #endregion
    }

    public class CalcLineAtrib
    {
        const string MLSConst = "CalcLAtrib";

        public bool IsTooLow = false;
        public bool IsTooHi = false;
        public bool IsNonLinar = false;

        public void ReSet()
        {
            IsTooLow = false;
            IsTooHi = false;
            IsNonLinar = false;
        }

        public bool IsError
        {
            get
            {
                return IsTooHi | IsTooLow | IsNonLinar;
            }
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);
            bw.Write(IsTooLow);
            bw.Write(IsTooHi);
            bw.Write(IsNonLinar);
            bw.Write(32412);
        }

        public void Load(BinaryReader br)
        {
            int ver = br.ReadInt32();
            if (ver != 1)
                throw new Exception("Неверная весия атрибута вычислений");
            IsTooLow = br.ReadBoolean();
            IsTooHi = br.ReadBoolean();
            IsNonLinar = br.ReadBoolean();
            ver = br.ReadInt32();
            if(ver != 32412)
                throw new Exception("Неверное окончание атрибута вычислений");
        }

        public string GetShortDescription()
        {
            if (IsError == false)
                return null;
            if (IsTooLow)
                return Common.MLS.Get(MLSConst, "Слишком малая интенс.");
            if (IsTooHi)
                return Common.MLS.Get(MLSConst, "Слишком большая интенс.");
            return Common.MLS.Get(MLSConst, "Линия зашкалила");
        }

        public override string ToString()
        {
            string ret = "";
            if (IsTooLow)
                ret += "Ошибка: слишком малая интенсивность линии!!!! " + serv.Endl;
            if (IsTooHi)
                ret += "Ошибка: слишком большая интенсивность линии!!!! " + serv.Endl;
            if (IsNonLinar)
                ret += "Ошибка: линия вышла за допустимый диапазон!!!! " + serv.Endl;
            if(ret.Length == 0)
                ret += "Замечаний к линии нет." + serv.Endl;
            return ret;
        }
    }

    public class MethodSimpleCellFormulaResult
    {
        public double[] ReCalcCon = null;
        public double[] AnalitValue = null;
        public double[] AnalitAq = null;
        public double[] AnalitCorrValue = null;
        public double[] AnalitCorrAq = null;
        public double[] SparkRates = null;
        public float ConFrom = 0;
        public float ConTo = 100;
        public CalcLineAtrib ResultAttrib = new CalcLineAtrib();
        //public double Error = -1;
        //public double ErrorPs = -1;
        bool EnabledPriv = true;
        public byte FormulaType = 0;
        public string Key = "";
        public bool Enabled
        {
            get
            {
                if (AnalitValue == null)// && Error == -1 &&
                    //ErrorPs == -1)
                    return false;
                if (Master.Enabled == false || 
                    FormulaType != 0 ||
                    ResultAttrib.IsError)
                    return false;
                return EnabledPriv;
            }
            set
            {
                EnabledPriv = value;
            }
        }
        public string Msgs = "";
        public List<GLogRecord> LogData = new List<GLogRecord>();
        MethodSimpleCell Master;

        public string GetDebugReport()
        {
            string endl = "" + (char)0xD + (char)0xA;
            string ret = "";
            ret += "--------Результат анализа спектра: " + endl;
            ret += "Флаг использования = " + Enabled + " (" + EnabledPriv + ")" + endl;
            ret += ResultAttrib.ToString();
            if (ReCalcCon == null)
                ret += " Нет данных" + endl;
            else
            {
                double sko, good_sko;
                double ever = GetEver(out sko,out good_sko);
                ret += " Среднее значение = " + ever + serv.Endl;
                ret += " СКО = " + sko + " " + (sko * 100 / ever) + "%" + endl;
                ret += " Хор.СКО = " + good_sko + " " + (good_sko * 100 / ever) + "%" + endl;
                ret += "   Кон." + "   Аналит." + "  Оценка прожега" + endl;
                for (int i = 0; i < ReCalcCon.Length; i++)
                {
                    ret += "   " + ReCalcCon[i] + "  " + AnalitValue[i];// +endl;
                    if(SparkRates == null || i >= SparkRates.Length)
                        ret += "    -  ";
                    else
                        ret += "   "+serv.GetGoodValue(SparkRates[i],2);
                    if (ReCalcCon[i] < ConFrom)
                        ret += "Концентрация меньше допустимой:"+ConFrom;
                    if (ReCalcCon[i] > ConTo)
                        ret += "Концентрация выше допостимой:"+ConTo;
                    ret += serv.Endl;
                }
            }
            return ret;
        }

        public double GetEver(out double sko,out double good_sko)
        {
            if (ReCalcCon == null)
            {
                sko = 0;
                good_sko = 0;
                return 0;
            }
            double ret = SpectroWizard.analit.Stat.GetEver(ReCalcCon, SpectroWizard.analit.Stat.SpectrDataSKO);
            sko = SpectroWizard.analit.Stat.LastSKO;
            good_sko = SpectroWizard.analit.Stat.LastGoodSKO;
            return ret;
        }

        public MethodSimpleCellFormulaResult(string key, MethodSimpleCell master)
        {
            Master = master;
            Key = key;
        }

        public MethodSimpleCellFormulaResult(BinaryReader br, MethodSimpleCell master)
        {
            Master = master;
            Load(br);
        }

        #region Save/Load methods
        public void Load(BinaryReader br)
        {
            //long from = br.BaseStream.Position;
            string tmp = br.ReadString();
            if (tmp.Equals("mefcd") == false)
                throw new Exception("MethodSimpleFormulaCalcDetails:Wrong record type");
            int ver = br.ReadInt32();
            if (ver < 0 || ver > 4)
                throw new Exception("MethodSimpleFormulaCalcDetails:Unsupported version");

            //public double ReCalcValue;
            int count = br.ReadInt32();
            if (count > 0)
            {
                byte cor_flag = br.ReadByte();
                ReCalcCon = new double[count];
                AnalitValue = new double[count];
                AnalitAq = new double[count];
                /*if (cor_flag != 0)
                {
                    AnalitCorrValue = new double[count];
                    AnalitCorrAq = new double[count];
                }
                else
                {
                    AnalitCorrValue = null;
                    AnalitCorrAq = null;
                }*/
                AnalitCorrValue = new double[count];
                AnalitCorrAq = new double[count];
                for (int i = 0; i < count; i++)
                {
                    ReCalcCon[i] = br.ReadDouble();
                    AnalitValue[i] = br.ReadDouble();
                    AnalitAq[i] = br.ReadDouble();
                    if (cor_flag != 0)
                    {
                        AnalitCorrValue[i] = br.ReadDouble();
                        AnalitCorrAq[i] = br.ReadDouble();
                    }
                }
            }
            else
            {
                ReCalcCon = null;
                AnalitValue = null;
                AnalitAq = null;
                AnalitCorrValue = null;
                AnalitCorrAq = null;
            }
            //public double Error;
            //Error = br.ReadDouble();
            //public double ErrorPs;
            //ErrorPs = br.ReadDouble();
            //public bool Enabled;
            EnabledPriv = br.ReadBoolean();
            //public string Msgs;
            Msgs = br.ReadString();
            //public List<GLogRecord> Data = new List<GLogRecord>();
            count = br.ReadInt32();
            for (int i = 0; i < count; i++)
                LogData.Add(GLogRecord.Load(br));
            Key = br.ReadString();
            if (ver >= 1)
                FormulaType = br.ReadByte();

            if (ver >= 2 && ver < 4)
                ResultAttrib.IsTooLow = br.ReadBoolean();

            if (ver >= 3)
            {
                count = br.ReadInt32();
                SparkRates = new double[count];
                for (int i = 0; i < count; i++)
                    SparkRates[i] = br.ReadDouble();
                ConFrom = br.ReadSingle();
                ConTo = br.ReadSingle();
            }

            if(ver >= 4)
                ResultAttrib.Load(br);

            ver = br.ReadInt32();
            if (ver != 0)
                throw new Exception("MethodSimpleFormulaCalcDetails:Unsupported end version");
            //Common.Log("MethodSimpleFormulaCalcDetails=" + (br.BaseStream.Position - from));
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write("mefcd");
            bw.Write(4);
            
            //public double ReCalcValue;
            if (ReCalcCon != null &&
                ReCalcCon.Length > 0)
            {
                bw.Write(ReCalcCon.Length);
                if(AnalitCorrValue != null)
                    bw.Write((byte)1);
                else
                    bw.Write((byte)0);
                for (int i = 0; i < ReCalcCon.Length; i++)
                {
                    bw.Write(ReCalcCon[i]);
                    try { bw.Write(AnalitValue[i]); }
                    catch { bw.Write((double)0); }
                    try { bw.Write(AnalitAq[i]); }
                    catch { bw.Write((double)0); }
                    if (AnalitCorrValue != null)
                    {
                        try {bw.Write(AnalitCorrValue[i]);}
                        catch { bw.Write((double)0); }
                        try { bw.Write(AnalitCorrAq[i]); }
                        catch { bw.Write((double)0); }
                    }
                }
            }
            else
                bw.Write(0);
            //public double Error;
            //bw.Write(Error);
            //public double ErrorPs;
            //bw.Write(ErrorPs);
            //public bool Enabled;
            bw.Write(EnabledPriv);
            //public string Msgs;
            bw.Write(Msgs);
            //public List<GLogRecord> Data = new List<GLogRecord>();
            bw.Write(LogData.Count);
            for (int i = 0; i < LogData.Count; i++)
                LogData[i].Save(bw);
            bw.Write(Key);

            // version 1
            bw.Write(FormulaType);

            // version 2
            //bw.Write(IsTooLow);

            // version 3
            if (SparkRates == null)
                bw.Write(0);
            else
            {
                bw.Write(SparkRates.Length);
                foreach (double val in SparkRates)
                    bw.Write(val);
            }
            bw.Write(ConFrom);
            bw.Write(ConTo);

            // version 4
            ResultAttrib.Save(bw);

            bw.Write(0);
        }
        #endregion
    }
}
