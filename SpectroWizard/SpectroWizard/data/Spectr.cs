using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Windows.Forms;
using SpectroWizard.analit.fk;
using SpectroWizard.analit;
using SpectroWizard.gui.tasks;

namespace SpectroWizard.data
{
    public class Spectr
    {
        // short datas....
        SpectrDataView DefaultView;
        public DateTime CreatedDate;
        SpectrCondition FullConditions;
	    Dispers Disp = new Dispers();
        public OpticFk OFk = new OpticFk();
        public string SpectrInfo = null;
        public string MeasuringLog = null;

	    public Dispers GetCommonDispers()
	    {
    		return new Dispers(Disp,true);
	    }

        public void ShiftDispers(float step)
        {
            for(int i = 0;i<Disp.GetSensorSizes().Length;i++)
                Disp.AddShift(step,i);
        }

        public bool IsEmpty()
        {
            if (DefaultView == null && Views == null)
                return true;
            return false;
        }

        public void SetDispers(Dispers disp)
        {
            Disp = new Dispers(disp,false);
        }

        public Dispers GetDispers()
        {
            return Disp;
        }

        public void ApplySifts(float[] shifts)
        {
            for (int s = 0; s < shifts.Length; s++)
                Disp.ApplyShifts(shifts[s], s);
        }

        public void ApplySifts(double[] shifts)
        {
            float[] sh = new float[shifts.Length];
            for (int s = 0; s < shifts.Length; s++)
            {
                Disp.ApplyShifts(shifts[s], s);
                sh[s] = (float)shifts[s];
            }
        }

        public void ApplyLineLevelK(double[] line_level_k)
        {
            int[] ss = Disp.GetSensorSizes();
            OFk.LineLevelAmplify = new float[ss.Length];
            for (int s = 0; s < ss.Length; s++)
                OFk.LineLevelAmplify[s] = (float)line_level_k[s];
        }

        public PlainSpectr GetPlainSpectr()
        {
            int ind = PathShort.LastIndexOf('\\');
            string path = PathShort.Substring(0,ind);
            path = Common.Db.GetFoladerPath(path);
            string name = PathShort.Substring(ind+1);
            path += "\\"+name +"pl";
            if (File.Exists(path) == true)
                try
                {
                    return new PlainSpectr(path);
                }
                catch
                {
                }
            List<SpectrDataView> sset = GetViewsSet();
            PlainSpectr ret = new PlainSpectr();
            int cur_fr;
            for (int f = 0; f < sset.Count; f++)
            {
                if(sset[f].GetCondition().Lines[0].IsActive == false)
                    continue;
                SpectrDataView sdv = sset[f];
                SpectrDataView nsdv = GetNullFor(f);
                float tick = sdv.GetCondition().Lines[0].Expositions[0];
                if (tick == 0)
                    tick = sdv.GetCondition().Tick;
                cur_fr = ret.AddFrame(tick);
                float[][] data = sdv.GetFullDataNoClone();
                float[][] nul = nsdv.GetFullDataNoClone();
                for (int sn = 0; sn < data.Length; sn++)
                {
                    for (int pix = 0; pix < data[sn].Length; pix++)
                        ret.AddPixel(cur_fr,
                            (float)Disp.GetLyByLocalPixel(sn, pix),
                            (float)Disp.GetLyByLocalPixel(sn, pix + 1),
                            data[sn][pix],nul[sn][pix],
                            sdv.MaxLinarLevel
                            //sdv.OverloadLevel
                            );
                }
            }
            ret.FinalCompile();
            ret.Save(path);
            return ret;
        }

        public SpectrCondition GetMeasuringCondition()
        {
            return FullConditions;
        }

        public void SetMeasuringCondition(SpectrCondition cond)
        {
            FullConditions = cond;
        }

        public void Rename(string new_name)
        {
            int ind = PathFull.LastIndexOf(Path.DirectorySeparatorChar);
            string base_path = (string)PathFull.Substring(0,ind+1);
            string new_path_full = base_path + new_name + ".sf";
            string new_path_short = base_path + new_name + ".ss";
            File.Move(DataBase.GetFullPath(PathFull), DataBase.GetFullPath(new_path_full));
            File.Move(DataBase.GetFullPath(PathShort), DataBase.GetFullPath(new_path_short));
            PathFull = new_path_full;
            PathShort = new_path_short;
            NeedToRewriteFull = true;
        }

        public static void RemoveSpectr(string record)
        {
            int ind = record.LastIndexOf(".ss");
            if (ind > 0)
                record = record.Substring(0, ind);
            try
            {
                DataBase.RemoveFile(record + ".ss");
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
            try
            {
                DataBase.RemoveFile(record + ".sf");
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
            try
            {
                DataBase.RemoveFile(record + ".sspl");
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        public static string[] GetSpectrList(string path)
        {
            string[] ret = Directory.GetFiles(DataBase.BasePath + path, "*.ss");
            long[] times = new long[ret.Length];

            for (int i = 0; i < ret.Length; i++)
                times[i] = File.GetLastWriteTime(ret[i]).Ticks;

            for (int j = 1; j < ret.Length; j++)
            {
                for (int i = 1; i < ret.Length; i++)
                {
                    if (times[i - 1] > times[i])
                    {
                        string tmp = ret[i - 1];
                        long tmpl = times[i - 1];

                        ret[i - 1] = ret[i];
                        times[i - 1] = times[i];

                        ret[i] = tmp;
                        times[i] = tmpl;
                    }
                }
            }
            return ret;
        }
        // full datas....
        List<SpectrDataView> Views = new List<SpectrDataView>();

        public float OverloadLevel
        {
            get
            {
                return Views[0].OverloadLevel;
            }
        }

        List<SpectrDataView> ViewNuls = new List<SpectrDataView>();
        public SpectrDataView GetNullFor(int spectr_view_index)
        {
            while (ViewNuls.Count <= spectr_view_index)
                ViewNuls.Add(null);
            if (ViewNuls[spectr_view_index] == null)
            {
                SpectrConditionCompiledLine line = Views[spectr_view_index].GetCondition().Lines[0];
                List<SpectrDataView> found_nuls = new List<SpectrDataView>();
                for (int i = 0; i < Views.Count; i++)
                    if (Views[i].GetCondition().Lines[0].IsExpositionEqual(line,false) &&
                        Views[i].GetCondition().Lines[0].IsActive == false)
                        found_nuls.Add(Views[i]);
                if (found_nuls.Count <= 0)
                    return null;
                if (found_nuls.Count == 1)
                    ViewNuls[spectr_view_index] = found_nuls[0];
                else
                    ViewNuls[spectr_view_index] = analit.SpectrFunctions.Ever(found_nuls);
            }
            return ViewNuls[spectr_view_index];
        }

        int ShotCount = -1;
        public int GetShotCount()
        {
            GetViewsSet();

            List<SpectrDataView> views = GetViewsSet();
            //int active_exps_num = 0;
            ShotCount = 0;
            // count number of exposition
            for (int i = 0; i < views.Count; i++)
            {
                SpectrCondition cond = views[i].GetCondition();
                SpectrCondition.CondTypes type = cond.Lines[0].Type;// SpectrCondition.ParseStringType(cond);
                if (type == SpectrCondition.CondTypes.Exposition)
                {
                    if (cond.Lines[0].IsActive)
                        ShotCount++;
                }
            }
            return ShotCount;
        }

        public int[] GetShotIndexes()
        {
            if (ShotCount <= 0)
                GetShotCount();
            List<SpectrDataView> views = GetViewsSet();
            int[] ret = new int[ShotCount];
            int ind = 0;
            for (int i = 0; i < ShotCount; i++)
            {
                SpectrCondition cond = views[i].GetCondition();
                SpectrCondition.CondTypes type = cond.Lines[0].Type;// SpectrCondition.ParseStringType(cond);
                if (type == SpectrCondition.CondTypes.Exposition)
                {
                    if (cond.Lines[0].IsActive)
                    {
                        ret[ind] = i;
                        ind++;
                    }
                }
            }
            return ret;
        }

        public void ResetDefaultView()
        {
            DefaultView = null;
            GetDefultView();
        }

        bool IsDataValid()
        {
            int good = 0;
            bool need_to_reinit = false;
            if (DefaultView == null)
                need_to_reinit = true;
            else
            {
                float[] data = DefaultView.GetSensorData(0);
                for (int i = 0; i < data.Length - 1; i++)
                {
                    if (serv.IsValid(data[i]) == false)
                        return true;
                    if (data[i] == float.MaxValue || data[i + 1] == float.MaxValue)
                        continue;
                    good++;
                    if (data[i] != data[i + 1])
                    {
                        need_to_reinit = false;
                        break;
                    }
                    else
                        need_to_reinit = true;
                }
            }
            if (good == 0)
                return true;
            return need_to_reinit;
        }

        public SpectrDataView GetDefultView()
        {
            //if (IsDataValid() || SpectrInfo == null)
            //if (SpectrInfo != null)
            {
                if(DefaultView != null)
                    Common.LogWar("Rebuild spectr file...'" + PathShort + "' '" + PathFull + "'");
                GetViewsSet();

                if (Views.Count == 1)
                {
                    DefaultView = Views[0];
                    return DefaultView;
                }
                List<SpectrDataView> views = GetViewsSet();
                int active_exps_num = GetShotCount();
                // count number of exposition
                if (active_exps_num == 0)
                {
                    if (Views.Count == 0)
                        return null;
                    else
                        return Views[0];
                }

                SpectrDataView[] nul = new SpectrDataView[active_exps_num];
                SpectrDataView[] sig = new SpectrDataView[active_exps_num];
                //OpticFk[] fk = new OpticFk[active_exps_num];
                int tmp_n = 0;
                // find all expositions with gen on
                for (int i = 0; i < views.Count && tmp_n < active_exps_num; i++)
                {
                    if (views[i].GetCondition().Lines[0].IsActive)// (is_on)
                    {
                        sig[tmp_n] = views[i];
                        tmp_n++;
                    }
                }

                // find all corresponded expositions
                for (int i = 0; i < active_exps_num; i++)
                {
                    for (int j = 0; j < views.Count; j++)
                    {
                        if (views[j].GetCondition().Lines[0].IsActive)//is_on)
                            continue;
                        if (views[j].GetCondition().Lines[0].IsExpositionEqual(sig[i].GetCondition().Lines[0],false) == true)
                        {
                            nul[i] = views[j];
                            break;
                        }
                    }
                }
                DefaultView = SpectroWizard.analit.SpectrMixer.Mix(sig, nul, OFk);
                Save();
            }
            return DefaultView;
        }

        public int GetViewCount(SpectrCondition cond,bool is_gen_on)
        {
            return 0;
        }

        public SpectrDataView this[int index]
        {
            get
            {
                return Views[index];
            }
        }

        bool FullLoaded = false;
        public List<SpectrDataView> GetViewsSet()
        {
            if (FullLoaded == false && PathFull != null)
                LoadFull();
            FullLoaded = true;
            return Views;
        }

        public void Set(SpectrDataView view, int index)
        {
            CreatedDate = new DateTime(DateTime.Now.Ticks);
            GetViewsSet()[index] = view;
            DefaultView = null;
            NeedToRewriteFull = true;
        }

        public void Add(SpectrDataView view)
        {
            CreatedDate = new DateTime(DateTime.Now.Ticks);
            GetViewsSet().Add(view);
            DefaultView = null;
            NeedToRewriteFull = true;
        }

        public void Clear()
        {
            CreatedDate = new DateTime(DateTime.Now.Ticks);
            GetViewsSet();
            Views.Clear();
            DefaultView = null;
            NeedToRewriteFull = true;
        }

        static public bool IsFileExists(string path)
        {
            return GetFileDateTime(path) != null;
        }

        static public DateTime GetFileDateTimeTmp = DateTime.Today;
        static public string GetFileDateTime(string path)
        {
            GetFileDateTimeTmp = DateTime.Today;
            int ind = path.LastIndexOf(".s");
            string pshort, pfull;
            if (ind > 0 && ind == path.Length - 3)
            {
                string base_name = path.Substring(0, ind);
                pshort = base_name + ".ss";
                pfull = base_name + ".sf";
            }
            else
            {
                pshort = path + ".ss";
                pfull = path + ".sf";
            }
            //if(File.Exists(pshort) && File.Exists(pfull))
            if (DataBase.FileExists(ref pshort) &&
                DataBase.FileExists(ref pfull))
            {
                DateTime dt = DataBase.GetFileDateTime(ref pshort);
                GetFileDateTimeTmp = dt;
                return "" + DataBase.GetFileDateTime(ref pshort) + " " + DataBase.GetFileDateTime(ref pfull);
            }
            //    return ""+File.GetCreationTime(pshort) + " "+ File.GetCreationTime(pfull);
            return null;
        }

        static public Spectr GetSpectr_Call_Only_Db_Method(string tmp)
        {
            string path = (string)tmp.Clone();
            return new Spectr(path);
        }

        public Spectr(string file_path)
        {
            string path = (string)file_path.Clone();
            DataBase.CheckPath(ref path);
            int ind = path.LastIndexOf(".s");
            if (ind > 0 && ind == path.Length - 3)
            {
                string base_name = path.Substring(0, ind);
                PathShort = base_name + ".ss";
                PathFull = base_name + ".sf";
            }
            else
            {
                PathShort = path + ".ss";
                PathFull = path + ".sf";
            }
            LoadShort();
        }

        public Spectr(SpectrCondition cond,Dispers disp,OpticFk ofk,String measuring_log)
        {
            MeasuringLog = measuring_log;
            CreatedDate = DateTime.Now;
            FullConditions = cond;
            Disp = disp;
            OFk = new OpticFk(ofk);
        }

        public string GetPath()
        {
            return PathShort.Substring(0, PathShort.Length - 3);
        }


        /*public void MakeLyCorrection(Spectr ly_etalon)
        {
            List<SpectrDataView> dv = GetViewsSet();
            for (int sp = 0; sp < dv.Count; sp++)
            {
                SpectrConditionCompiledLine cl = dv[sp].GetCondition().Lines[0];
                if (cl.Type == SpectrCondition.CondTypes.Exposition && cl.IsGenOn)
                    dv[sp].MakeLyCorrection(ly_etalon);
            }
        }*/

        string PathShort;
        string PathFull;

        const string MLSConst = "SpectrFk";
        public void Remove()
        {
            /*DialogResult dr = MessageBox.Show(SpectroWizard.gui.MainForm.MForm,
                Common.MLS.Get(MLSConst, "Удалить спектр: ") + PathShort, 
                Common.MLS.Get(MLSConst, "Удаление"), 
                MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes)
                throw new Exception("Прервано пользователем.");*/
            data.DataBase.RemoveFile(PathShort);
            //File.Delete(PathShort);
            data.DataBase.RemoveFile(PathFull);
            //File.Delete(PathFull);
        }

        public Spectr(DbFolder fld,string record_name)
        {
            int ind = record_name.LastIndexOf(".s");
            if (ind > 0 && ind == record_name.Length - 3)
            {
                string base_name = record_name.Substring(0, ind);
                PathShort = fld.CreateRecordPath(base_name + ".ss");
                DataBase.CheckPath(ref PathShort);
                PathFull = fld.CreateRecordPath(base_name + ".sf");
                DataBase.CheckPath(ref PathFull);
            }
            else
            {
                PathShort = fld.CreateRecordPath(record_name + ".ss");
                PathFull = fld.CreateRecordPath(record_name + ".sf");
            }
            LoadShort();
        }

        void LoadShort()
        {
            FileStream fs;
            fs = DataBase.OpenFile(ref PathShort, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            string tmp = br.ReadString();
            if (tmp.Equals("bss") == false)
                throw new Exception("Wrong file type.");
            int ver = br.ReadInt32();
            if (ver < 0 || ver > 4)
                throw new Exception("Wrong file version.");
            //int prev_ver = ver;
            CreatedDate = new DateTime(br.ReadInt64());
            FullConditions = new SpectrCondition(br);
            Disp = new Dispers(br);
            try
            {
                OFk = new OpticFk(br);
            }
            catch (Exception ex)
            {
                OFk = new OpticFk();
                Log.Out(ex);
            }
            int fl = br.ReadInt32();
            if (fl != 0)
                DefaultView = new SpectrDataView(br, this);
            if (ver >= 3)
                SpectrInfo = br.ReadString();
            if (ver >= 4)
                MeasuringLog = br.ReadString();
            br.Close();
            if (ver < 3)
            {
                LoadFull();
                Save();
            }
        }

        public void LoadFull()
        {
            FullLoaded = true;
            FileStream fs = DataBase.OpenFile(ref PathFull, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            string tmp = br.ReadString();
            if (tmp.Equals("bsf") == false)
                throw new Exception("Wrong file type.");
            int ver = br.ReadInt32();
            if (ver < 0 || ver > 2)
                throw new Exception("Wrong file version.");
            int n = br.ReadInt32();
            Views.Clear();
            for (int i = 0; i < n; i++)
                Views.Add(new SpectrDataView(br,this));
            br.Close();

            ViewNuls.Clear();
            for (int i = 0; i < n; i++)
                ViewNuls.Add(null);

            if (SpectrInfo == null)
            //#warning --------------- Remove comments above --------------------
                GenerateSpectrInfo();
        }

        void GenerateSpectrInfo()
        {
            SpectrInfo = "";
            Save();
        }

        public void Save()
        {
            try
            {
                if (DefaultView == null)
                    GetDefultView();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            Common.Db.SpectrSaved(this);
            //DataBase.CheckPath(ref PathShort);
            //DataBase.CheckPath(ref PathFull);
            SaveShort(PathShort);
            SaveFull(PathFull);
        }

        public void SaveAs(string path)
        {
            DataBase.CheckPath(ref path);
            if(PathShort != null)
                Common.Db.SpectrSaved(this);
            //if (Views.Count == 0)
            //    LoadFull();
            GetViewsSet();
            int ind = path.Length - 3;
            if (path[ind] == '.' && path[ind] == 's')
                path = path.Substring(0, ind - 1);
            PathShort = path + ".ss";
            PathFull = path + ".sf";
            NeedToRewriteFull = true;
            Save();
            Common.Db.SpectrSaved(this);
        }

        void SaveShort(string file_name)
        {
            FileStream fs = DataBase.OpenFile(ref file_name, FileMode.OpenOrCreate, FileAccess.Write);
            fs.SetLength(0);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write("bss");
            bw.Write(4);
            bw.Write(CreatedDate.Ticks);
            FullConditions.Save(bw);
            Disp.Save(bw);
            OFk.Save(bw);
            if (DefaultView == null)
                bw.Write(0);
            else
            {
                bw.Write(1);
                DefaultView.Save(bw);
            }
            if (SpectrInfo == null)
                bw.Write("");
            else
                bw.Write(SpectrInfo);
            
            if (MeasuringLog == null)
                bw.Write("No Info");
            else
                bw.Write(MeasuringLog);

            bw.Close();
        }

        bool NeedToRewriteFull = false;
        void SaveFull(string file_name)
        {
            if (Views.Count == 0 || NeedToRewriteFull == false)
                return;

            FileStream fs = DataBase.OpenFile(ref file_name, FileMode.OpenOrCreate, FileAccess.Write);
            fs.SetLength(0);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write("bsf");
            bw.Write(2);
            bw.Write(Views.Count);
            for (int i = 0; i < Views.Count; i++)
                Views[i].Save(bw);
            bw.Close();
        }
    }

    public class SpectrDataView
    {
        SpectrCondition CondPriv;
        float[][] Data;
        public short[][] BlankStart,
            BlankEnd;
        float OverloadVal;
        float MaxLinarVal;

        Function[] BackGroundCash;
        bool[][] Masks;
        public void SetBackGroundFunction(int sn, Function bkg_fk, bool[] mask)
        {
            BackGroundCash[sn] = bkg_fk;
            Masks[sn] = mask;
        }

        string CheckBuffer89(short[] buffer,bool is_dir,bool report_any_way,int sn)
        {
            int pix_shift;
            if (sn == 0)
                pix_shift = 0;
            else
                pix_shift = 1;
            double[] dlt = new double[buffer.Length-1];
            for (int i = 1; i < buffer.Length; i++)
                dlt[i-1] = (short)Math.Abs(buffer[i] - buffer[i - 1]);
            short ever_dlt = (short)Stat.GetEver(dlt);

            int trig_level = ever_dlt * 2;

            string ret = "";

            if (is_dir)
            {
                int found = 0;
                for (int start_pixel = 69; start_pixel < 79; start_pixel++)
                {
                    if ((buffer[start_pixel] - buffer[start_pixel - 1]) > trig_level &&
                        (buffer[start_pixel + 1] - buffer[start_pixel + 2]) > trig_level)
                    {
                        found = start_pixel;
                        break;
                    }
                }
                if ((found > 0 && found != 73 + pix_shift) || report_any_way)
                {
                    if (found > 0 && found != 73 + pix_shift)
                        ret = "Error ";
                    else
                        ret = "Ok    ";
                    ret += "Found pic at " + found + " data from 70pix[...";
                    for (int i = 69; i < 80; i++)
                    {
                        if (i == 73 + pix_shift || i == 74 + pix_shift)
                            ret += "*";
                        ret += buffer[i] + ",";
                    }
                    ret += "...]";
                }
            }
            else
            {
                int sh = 88;
                int found = 0;
                for (int start_pixel = 69; start_pixel < 79; start_pixel++)
                {
                    if ((buffer[sh - start_pixel] - buffer[sh - start_pixel - 1]) > trig_level &&
                        (buffer[sh - start_pixel + 1] - buffer[sh - start_pixel + 2]) > trig_level)
                    {
                        found = start_pixel;
                        break;
                    }
                }
                if ((found > 0 && found != 73 + pix_shift) || report_any_way)
                {
                    if (found > 0 && found != 73 + pix_shift)
                        ret = "Error ";
                    else
                        ret = "Ok    ";
                    ret += "Found pic at " + found + " data from 70pix reverce[...";
                    //for (int i = 69; i < 80; i++)
                    for (int i = 79; i >= 69; i--)
                    {
                        if (i == 73 + pix_shift || i == 74 + pix_shift)
                            ret += "*";
                        ret += buffer[sh-i] + ",";
                    }
                    ret += "...]";
                }
            }
            return ret;
        }
        
        public string GetInfo(string prefix)
        {
            string ret = "";
            if (CondPriv.Lines[0].IsActive != true)
                return ret;
            bool has_error = false;

            if (BlankStart != null)
            {
                for (int s = 0; s < BlankStart.Length; s++)
                    if (BlankStart[s].Length == 89)
                    {
                        string tmp = CheckBuffer89(BlankStart[s], true, false,s);
                        if(tmp.Length > 0)
                        {
                            has_error = true;
                            break;
                        }
                    }
            }
            if (BlankEnd != null)
            {
                for (int s = 0; s < BlankEnd.Length; s++)
                    if (BlankEnd[s].Length == 89)
                    {
                        string tmp = CheckBuffer89(BlankEnd[s], false, false, s);
                        if (tmp.Length > 0)
                        {
                            has_error = true;
                            break;
                        }
                    }
            }

            if (has_error)
            {
                ret += prefix+serv.Endl;
                if (BlankStart != null)
                {
                    for (int s = 0; s < BlankStart.Length; s++)
                        if (BlankStart[s].Length == 89)
                        {
                            string tmp = CheckBuffer89(BlankStart[s], true, true, s);
                            if (tmp.Length > 0)
                            {
                                has_error = true;
                                ret += "SN" + (s + 1) + " " + tmp;
                                ret += serv.Endl;
                            }
                        }
                }
                if (BlankEnd != null)
                {
                    for (int s = 0; s < BlankEnd.Length; s++)
                        if (BlankEnd[s].Length == 89)
                        {
                            string tmp = CheckBuffer89(BlankEnd[s], false, true, s);
                            if (tmp.Length > 0)
                            {
                                has_error = true;
                                ret += "SN" + (s + 1) + " " + tmp;
                                ret += serv.Endl;
                            }
                        }
                }//*/
            }
            
            return ret;
        }

        public void GetBackGroundFunction(int sn,out Function fk,out bool[] mask)
        {
            if (BackGroundCash == null)
            {
                BackGroundCash = new Function[Data.Length];
                Masks = new bool[Data.Length][];
                for (int i = 0; i < BackGroundCash.Length; i++)
                {
                    BackGroundCash[i] = null;
                    Masks[i] = null;
                }
            }
            fk = BackGroundCash[sn];
            mask = Masks[sn];
            //throw new NotImplementedException("Getting backgrund function not implemented");
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(5);
            CondPriv.Save(bw);
            bw.Write(OverloadVal);
            bw.Write(MaxLinarVal);
            bw.Write(Data.Length);
            float min = Data[0][0];
            float max = min;
            for (int s = 0; s < Data.Length; s++)
                for (int i = 0; i < Data[s].Length; i++)
                {
                    if (Data[s][i] < float.MaxValue && Data[s][i] > max)
                        max = Data[s][i];
                    if (Data[s][i] < min)
                        min = Data[s][i];
                }
            bw.Write(min);
            float l = max - min;
            if (l == 0)
                l = 1;
            bw.Write(l);
            for (int s = 0; s < Data.Length; s++)
            {
                bw.Write(Data[s].Length);
                for (int i = 0; i < Data[s].Length; i++)
                {
                    if (Data[s][i] < float.MaxValue)
                        bw.Write((ushort)((Data[s][i] - min) * (ushort.MaxValue - 1) / l));
                    else
                        bw.Write(ushort.MaxValue);
                }
                //bw.Write(SpectrShifts[s]);
            }
            SaveBlank(bw, ref BlankStart);
            SaveBlank(bw, ref BlankEnd);
        }

        void SaveBlank(BinaryWriter bw, ref short[][] blanks)
        {
            if (blanks == null || blanks[0] == null)
                bw.Write(0);
            else
            {
                bw.Write(blanks.Length);
                for (int s = 0; s < blanks.Length; s++)
                {
                    bw.Write(blanks[s].Length);
                    for (int p = 0; p < blanks[s].Length; p++)
                        bw.Write(blanks[s][p]);
                }
            }
        }

        public SpectrDataView(BinaryReader br,Spectr sp)
        {
            //ShiftedDisp = new Dispers(sp.GetCommonDispers(),false);
            int ver = br.ReadInt32();
            if (ver < 0 || ver > 5)
                throw new Exception(Common.MLS.Get("SpDataView", "Wrong version of SpectrDataView"));
            CondPriv = new SpectrCondition(br);
            OverloadVal = br.ReadSingle();
            MaxLinarVal = br.ReadSingle();
            int sn = br.ReadInt32();
            Data = new float[sn][];
            //SpectrShifts = new float[sn];
            float min = 0,l = 0;
            if (ver >= 3)
            {
                min = br.ReadSingle();
                l = br.ReadSingle();
                if (ver < 5 && l == float.MaxValue)
                    min = float.NaN;
            }
            for (int s = 0; s < sn; s++)
            {
                int size = br.ReadInt32();
                Data[s] = new float[size];
                if (ver < 3)
                {
                    for (int i = 0; i < size; i++)
                        Data[s][i] = br.ReadSingle();
                }
                else
                {
                    if(ver < 5)
                        for (int i = 0; i < size; i++)
                        {
                            float tmp = br.ReadUInt16();
                            Data[s][i] = tmp * l / ushort.MaxValue + min;
                        }
                    else
                        for (int i = 0; i < size; i++)
                        {
                            float tmp = br.ReadUInt16();
                            if (tmp == ushort.MaxValue)
                                Data[s][i] = float.MaxValue;
                            else
                                Data[s][i] = tmp * l / (ushort.MaxValue-1) + min;
                        }
                }
                if(ver == 1)
                    br.ReadSingle();//SpectrShifts[s] = br.ReadSingle();
                //ShiftedDisp.ApplyShifts(SpectrShifts[s], s);
            }
            if (ver >= 4)
            {
                LoadBlanks(br, ref BlankStart);
                LoadBlanks(br, ref BlankEnd);
            }
        }

        void LoadBlanks(BinaryReader br,ref short[][] data)
        {
            int sn = br.ReadInt32();
            if(sn == 0)
            {
                data = null;
                return;
            }
            data = new short[sn][];
            for (int s = 0; s < sn; s++)
            {
                int len = br.ReadInt32();
                data[s] = new short[len];
                for (int i = 0; i < len; i++)
                    data[s][i] = br.ReadInt16();
            }
        }

        public float OverloadLevel
        {
            get
            {
                return OverloadVal;
            }
        }

        public float MaxLinarLevel
        {
            get
            {
                return MaxLinarVal;
            }
        }

        public float this[int sn, int pixel]
        {
            get
            {
                if(sn < Data.Length && pixel < Data[sn].Length)
                    return Data[sn][pixel];
                return 0;
            }
        }

        public float[] GetSensorData(int sn)
        {
            return (float[])Data[sn].Clone();
        }

        public float[] GetSensorDataWithCorrection(int sn)
        {
            float[] ret = new float[Data[sn].Length];
            float ever = 0;
            if (Common.Conf.BlankSub && BlankEnd != null && BlankStart != null)
            {
                double[] tmp = new double[BlankEnd.Length + BlankStart.Length];
                int ind = 0;
                for (int i = 0; i < BlankStart.Length; i++, ind++)
                    tmp[ind] = BlankStart[sn][i];
                for (int i = 0; i < BlankEnd.Length; i++, ind++)
                    tmp[ind] = BlankEnd[sn][i];
                ever = (float)SpectroWizard.analit.Stat.GetEver(tmp);
            }
            //float[] ret = new float[Data[sn].Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = Data[sn][i] - ever;
            return ret;
        }

        public float[][] GetFullData()
        {
            float[][] ret = new float[Data.Length][];
            for (int s = 0; s < ret.Length; s++)
                ret[s] = GetSensorData(s);
            return ret;//(float[][])Data.Clone();
        }

        public float[][] GetFullDataNoClone()
        {
            return Data;
        }

        public SpectrCondition GetCondition()
        {
            return CondPriv;
        }

        public int GetSensorSize(int sn)
        {
            return Data[sn].Length;
        }

        public int GetSensorCount()
        {
            return Data.Length;
        }

        const string MLSPrefix = "SpDView";

        public SpectrDataView(SpectrCondition cond, short[][] data, short[][] blank_start, short[][] blank_end,
            short oversload_val, short max_linar)
        {
            Init(cond, data, blank_start, blank_end, oversload_val, max_linar);
        }

        public SpectrDataView(SpectrCondition cond, short[][] data,
            short oversload_val, short max_linar)
        {
            Init(cond, data, null, null, oversload_val, max_linar);
        }

        void Init(SpectrCondition cond, short[][] data, short[][] blank_start,short[][] blank_end,
            short oversload_val, short max_linar)//,Dispers disp)
        {
            //BlankStart = (short[][])blank_start.Clone();
            //BlankEnd = (short[][])blank_end.Clone();
            if (blank_start != null)
                BlankStart = (short[][])blank_start.Clone();
            if (blank_end != null)
                BlankEnd = (short[][])blank_end.Clone();
            float[][] dr = new float[data.Length][];
            for (int s = 0; s < dr.Length; s++)
            {
                dr[s] = new float[data[s].Length];
                for (int i = 0; i < data[s].Length; i++)
                    dr[s][i] = data[s][i];
            }
            Data = dr;
            CondPriv = cond;
            OverloadVal = oversload_val;
            MaxLinarVal = max_linar;
        }

        public SpectrDataView(SpectrCondition cond, float[][] data, short[][] blank_start, short[][] blank_end,
            short oversload_val, short max_linar)
        {
            Init(cond, data, blank_start, blank_end, oversload_val, max_linar);
        }

        public SpectrDataView(SpectrCondition cond, float[][] data,
            short oversload_val, short max_linar)
        {
            Init(cond, data, null, null, oversload_val, max_linar);
        }

        void Init(SpectrCondition cond, float[][] data, short[][] blank_start, short[][] blank_end,
            short oversload_val, short max_linar)//, Dispers disp)
        {
            if(blank_start != null)
                BlankStart = (short[][])blank_start.Clone();
            if(blank_end != null)
                BlankEnd = (short[][])blank_end.Clone();
            float[][] dr = new float[data.Length][];
            for (int s = 0; s < dr.Length; s++)
                dr[s] = (float[])data[s].Clone();
            Data = dr;
            CondPriv = cond;
            OverloadVal = oversload_val;
            MaxLinarVal = max_linar;
        }
    }

    public class SpectrCondition : SpectroWizard.gui.tasks.TestInterf
    {
        string SourceProgramText;
        public float Tick;
        public float PreSparkLy, PreSparkWidth, PreSparkLevel;
        public float PreSparkExp;
        public bool PreSparkEnable = false;

        public void saveExtra(Dispers disp)
        {
            File.Delete("extra_spark.csv");
            String toSave = "";
            if (PreSparkEnable)
            {
                List<int> snl = disp.FindSensors(PreSparkLy);
                int sn;
                int[] orientations = Common.Conf.USBOrientationVals;
                sn = orientations[snl[0]];
                int pixel;
                if (sn < 0)
                {
                    int[] line_sizes = Common.Dev.Reg.GetSensorSizes();
                    sn = -sn;
                    toSave = sn + ";";
                    pixel = (line_sizes[snl[0]] - (int)disp.GetLocalPixelByLy(snl[0], PreSparkLy)) + Common.Conf.BlakPixelStart;
                }
                else
                {
                    sn = sn;
                    toSave = sn + ";";
                    pixel = (int)disp.GetLocalPixelByLy(snl[0], PreSparkLy) + Common.Conf.BlakPixelStart;
                }
                toSave += pixel + ";";
                toSave += PreSparkWidth + ";";
                int exp = (int)(PreSparkExp / Common.Conf.MinTick);
                if (exp <= 1)
                    exp = 1;
                toSave += exp + ";";
                toSave += PreSparkLevel + ";";
                File.WriteAllText("extra_spark.csv", toSave);
            }
        }

        public override string GetName()
        {
            return "Проверка анализа условий измерений";//Spectr condition text parsing Test";
        }

        string CheckErrorStatus(bool check_war)
        {
            if (Warning != null && check_war)
                return "Предупреждение: "+Warning;
            for (int i = 0; i < Lines.Count; i++)
                if (Lines[i].CompilationError != null)
                    return "Ошибка анализа :" + Lines[i].CompilationError + " строка: "+(i+1);
            return null;
        }

        public override ulong GetType()
        {
            return TestInterf.IsFunctionalTest;
        }

        public override bool Run(out string log)
        {
            log = "Init default program";
            try
            {
                // test____________________
                int test = 1;
                SourceProgramText = GetDefaultCondition();
                log = "Test#" + test + ". Default task parsing.";
                Compile();
                string tmp = CheckErrorStatus(true);
                if (tmp != null)
                {
                    log += serv.Endl + tmp;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                //test____________________
                log += "Test#" + test + ". Nul test.";
                SourceProgramText = "";
                Compile();
                tmp = CheckErrorStatus(false);
                if (tmp != null)
                {
                    log += "Error: " + tmp + serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                //test____________________
                log += "Test#" + test + ". Nul multy line test.";
                SourceProgramText = "" + serv.Endl + serv.Endl + serv.Endl + serv.Endl;
                Compile();
                tmp = CheckErrorStatus(false);
                if (tmp != null)
                {
                    log += "Error: " + tmp + serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                //test____________________
                for (int i = 0; i < 8;i++ )
                {
                    log += "Test#" + test + ". Nul line and comment test. Mask"+i;
                    SourceProgramText = "";
                    int mask = i;
                    for (int j = 0; j < 3; j++, mask >>= 1)
                        if ((mask & 1) == 0)
                            SourceProgramText += serv.Endl;
                        else
                            SourceProgramText += "#comment"+serv.Endl;
                    Compile();
                    tmp = CheckErrorStatus(false);
                    if (tmp != null)
                    {
                        log += "Error: " + tmp + serv.Endl+
                            SourceProgramText + serv.Endl+serv.Endl;
                        return false;
                    }
                    log += " ... Ok" + serv.Endl; test++;
                }
                //test____________________
                log += "Test#" + test + ". Error prespark line. 'p:'";
                SourceProgramText = "#Test#" + test + " " + serv.Endl +
                "p:" + serv.Endl;
                Compile();
                tmp = CheckErrorStatus(false);
                if (tmp == null)
                {
                    log += "No report error" + serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                //test____________________
                log += "Test#" + test + ". Error prespark line.'p:'";
                SourceProgramText = "#Test #" + test + " " + serv.Endl +
                "p:1" + serv.Endl;
                Compile();
                tmp = CheckErrorStatus(false);
                if (tmp == null)
                {
                    log += "No report error" + serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                //test____________________
                log += "Test#" + test + ". Unknown line error.";
                SourceProgramText = "pfadsfasdf" + serv.Endl +
                "p:1" + serv.Endl;
                Compile();
                tmp = CheckErrorStatus(false);
                if (tmp == null)
                {
                    log += "No report error" + serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                //test____________________
                log += "Test#" + test + ". Normal prespark.";
                SourceProgramText = "#Test " + test + " " + serv.Endl +
                "p:1" + serv.Endl;
                Compile();
                tmp = CheckErrorStatus(false);
                if (tmp != null)
                {
                    log += "Error: " + tmp + serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                //test____________________
                log += "Test#" + test + ". Multi exposition.";
                SourceProgramText = "#Test " + test + " " + serv.Endl +
                " p : 1" + serv.Endl+
                "\te: 1.4(0.1; 0,2; 0.4)On" + serv.Endl+
                "e :1.5 (0.3; 0,2; 0.4)On" + serv.Endl +
                "e\t: 1,4  (  0.1;  0,2; 0.4    ) OFF " + serv.Endl;
                Compile();
                tmp = CheckErrorStatus(false);
                if (tmp != null)
                {
                    log += "Error: " + tmp + serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                log += "Test#" + test + ". Multi exposition check corresponded exposions.";
                Compile();
                tmp = CheckErrorStatus(true);
                if (tmp == null)
                {
                    log += "No error reported "+ serv.Endl;
                    return false;
                }
                log += " ... Ok" + serv.Endl; test++;

                return true;
            }
            catch (Exception ex)
            {
                log += serv.Endl + serv.Endl + "Fatal error!!!" + serv.Endl + serv.Endl + ex.ToString();
            }
            return false;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(3);
            bw.Write(Tick);
            bw.Write(SourceProgramText);
            if (LinesPriv == null)
            {
                LinesPriv = new List<SpectrConditionCompiledLine>();
                Compile();
            }
            bw.Write(LinesPriv.Count);
            for (int i = 0; i < LinesPriv.Count; i++)
                LinesPriv[i].Save(bw);

            bw.Write(PreSparkEnable);
            bw.Write(PreSparkLy);
            bw.Write(PreSparkWidth);
            bw.Write(PreSparkLevel);
            bw.Write(PreSparkExp);

            bw.Write(32423);
        }

        public void Load(BinaryReader br)
        {
            int ver = br.ReadInt32();
            if (ver < 1 || ver > 3)
                throw new Exception(Common.MLS.Get("Spectr", "Wrong version of SpectrCondition"));
            Tick = br.ReadSingle();
            SourceProgramText = br.ReadString();
            if (ver >= 2)
            {
                int count = br.ReadInt32();
                LinesPriv = new List<SpectrConditionCompiledLine>();
                for (int i = 0; i < count; i++)
                    Lines.Add(new SpectrConditionCompiledLine(br));
                if (ver >= 3)
                {
                    PreSparkEnable = br.ReadBoolean();
                    PreSparkLy = br.ReadSingle();
                    PreSparkWidth = br.ReadSingle();
                    PreSparkLevel = br.ReadSingle();
                    PreSparkExp = br.ReadSingle();
                }
            }
            else
                return;
            ver = br.ReadInt32();
            if (ver != 32423)
                throw new Exception(Common.MLS.Get("Spectr", "Wrong finish of SpectrCondition"));
        }

        public SpectrCondition(BinaryReader br)
        {
            Load(br);
        }

        public string SourceCode
        {
            get
            {
                return (string)SourceProgramText.Clone();
            }
        }

        /*static string GetDefaultGen()
        {
            return "()";
        }*/

        static public string GetDefaultCondition()
        {
            string endl = "" + (char)0xD + (char)0xA;
            string tmp = "#комментарии......" + endl;
            tmp += "О:1 " + "   # Обыскривание 1s" + endl;
            float exp = Common.Conf.DefaultExposition;
            float cexp = exp*2;
            for (int i = 0; i < 100 && cexp < 1.5; i++)
                cexp += exp;
            tmp += "Э:"+cexp+" (";
            int[] ss = Common.Dev.Reg.GetSensorSizes();
            for (int i = 0; i < ss.Length; i++)
            {
                tmp += exp;// "0.1";
                if (ss.Length > (i + 1))
                    tmp += ";";
            }
            tmp += ")ГВкл " + endl;
            tmp += "Э:" + cexp + " (";
            for (int i = 0; i < ss.Length; i++)
            {
                tmp += exp;// "0.1";
                if (ss.Length > (i + 1))
                    tmp += ";";
            }
            tmp += ")ГВыкл " + endl;
            return tmp;
        }

        static public string GetDefaultCondition(bool prespark, bool start_gen, float exp, float common, int n)
        {
            return GetDefaultCondition(prespark, start_gen, exp, exp, common, n);
        }

        static public string GetDefaultCondition(bool prespark,bool start_gen,float exp_from,float exp_to,float common,int n)
        {
            string endl = "" + (char)0xD + (char)0xA;
            string tmp;
            if (prespark)
            {
                tmp = "#стандартная программа ген.старт=" + start_gen +
                        " exp_from=" + exp_from + " exp_to=" + exp_to + " common=" + common + " cycles=" + n + endl;
                if (start_gen == true)
                    tmp += "О:1 " + "   # Обыскривание 1s" + endl;
            }
            else
                tmp = "";
            
            float exp_dlt = exp_to - exp_from;
            int[] ss = Common.Dev.Reg.GetSensorSizes();
            for (int cycle = 0; cycle < n; cycle++)
            {
                tmp += "Э:" + common + " (";
                for (int i = 0; i < ss.Length; i++)
                {
                    tmp += "" + Math.Round(exp_from + exp_dlt * i / (ss.Length - 1),2);
                    if (ss.Length > (i + 1))
                        tmp += ";";
                }
                if(start_gen)
                    tmp += ")ГВкл " + endl;
                else
                    tmp += ")ГВыкл " + endl;
            }
            tmp += "Э:"+common+" (";
            for (int i = 0; i < ss.Length; i++)
            {
                tmp += "" + Math.Round(exp_from + exp_dlt * i / (ss.Length - 1),2);
                if (ss.Length > (i + 1))
                    tmp += ";";
            }
            tmp += ")ГВыкл " + endl;
            return tmp;
        }

        public SpectrCondition()
        {
            Tick = Common.Dev.Tick;
            SourceProgramText = GetDefaultCondition();
        }


        public SpectrCondition(float tick, string program_text)
        {
            Tick = tick;
            SourceProgramText = program_text;
            LinesPriv = new List<SpectrConditionCompiledLine>();
            Compile();
        }

        public SpectrCondition(float tick, SpectrConditionCompiledLine line)
        {
            Tick = tick;
            SourceProgramText = line.SourceCode;
            LinesPriv = new List<SpectrConditionCompiledLine>();
            LinesPriv.Add(line);
        }

        public enum CondTypes
        {
            Unexpected,
            Comment,
            Empty,
            Prespark,
            Exposition,
            FillLight
        };

        static CondTypes ParseStringType(string line)
        {
            if (line.Length == 0)
                return CondTypes.Empty;
            if (line[0] == '#')
                return CondTypes.Comment;
            if (line[0] == 'p' || line[0] == 'о')
                return CondTypes.Prespark;
            if (line[0] == 'e' || line[0] == 'э')
                return CondTypes.Exposition;
            if (line[0] == 'e' || line[0] == 'э')
                return CondTypes.Exposition;
            if (line[0] == 'f' || line[0] == 'з')
                return CondTypes.FillLight;
            return CondTypes.Unexpected;
        }

        //public string CompiledText = null;
        public List<SpectrConditionCompiledLine> Lines
        {
            get
            {
                if (LinesPriv == null)
                {
                    LinesPriv = new List<SpectrConditionCompiledLine>();
                    Compile();
                }
                return LinesPriv;
            }
        }

        public bool IsEqual(SpectrCondition cond)
        {
            if (Lines.Count != cond.Lines.Count)
                return false;
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].SourceCode.Equals(cond.Lines[i].SourceCode) == false)
                    return false;
            }
            return true;
        }

        public List<SpectrConditionCompiledLine> LinesPriv;// = 
            //new List<SpectrConditionCompiledLine>();
        public string Warning = null;
        public void Compile(string txt)
        {
            SourceProgramText = txt;
            Compile();
        }

        public void Compile()
        {
            Warning = null;
            LinesPriv = new List<SpectrConditionCompiledLine>();
            string endl = "" + (char)0xD + (char)0xA;
            int ind = 0;
            int line_num = 1;
            string line = serv.GetLine(SourceProgramText, ref ind);
            SpectrConditionCompiledLine tmp;
            int sp_view_index = 0;
            bool fill_light = false;
            while (line != null)
            {
                line = line.Trim().ToLower();
                line = serv.RemoveSpaces(line);
                line = serv.RemoveControlSym(line);
                CondTypes ct = ParseStringType(line);
                switch (ct)
                {
                    case CondTypes.Prespark:
                        tmp = new SpectrConditionCompiledLine(
                                    ct, line, line_num, sp_view_index, ref fill_light);
                        LinesPriv.Add(tmp);
                        break;
                    case CondTypes.Exposition:
                        tmp = new SpectrConditionCompiledLine(ct, line, line_num, sp_view_index, ref fill_light);
                        if (tmp.IsActive)
                            sp_view_index++;
                        LinesPriv.Add(tmp);
                        break;
                    case CondTypes.FillLight:
                        tmp = new SpectrConditionCompiledLine(ct, line, line_num, sp_view_index, ref fill_light);
                        if (tmp.IsActive)
                            sp_view_index++;
                        LinesPriv.Add(tmp);
                        break;
                    default:
                        break;
                }
                line = serv.GetLine(SourceProgramText, ref ind);
                line_num++;
            }
            if (sp_view_index == 0)
                Warning = Common.MLS.Get("Spectr", "Нет измерений спектров со включенным генератором!")+serv.Endl;
            for (int i = 0; i < LinesPriv.Count; i++)
            {
                if (LinesPriv[i].IsActive && LinesPriv[i].Type == CondTypes.Exposition)
                {
                    bool found = false;
                    for (int j = 0; j < LinesPriv.Count; j++)
                    {
                        if (LinesPriv[j].IsActive == false &&
                            LinesPriv[j].IsExpositionEqual(LinesPriv[i],false))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                        Warning = Common.MLS.Get("Spectr", "У измеренного сиграла нет соответствующей калибровки фона:") + LinesPriv[i].ToString();
                }
            }
        }

        public override string ToString()
        {
            return SourceProgramText;
        }
    }

    public class SpectrConditionGenCompiledLine
    {
        public string CompilationError;
        public SpectrConditionGenCompiledLine(string line, int ind)
        {
        }

        /*public bool IsEqual(SpectrConditionGenCompiledLine line)
        {
            return true;
        }*/

        public SpectrConditionGenCompiledLine(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver != 0)
                throw new Exception("Unsupported SpectrConditionGenCompiledLine version");

            ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Unsupported SpectrConditionGenCompiledLine finish");
        }

        /*public override string ToString()
        {
            return "()";
        }*/

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)0);
            bw.Write((byte)1);
        }
    }

    public class SpectrConditionCompiledLine
    {
        public string SourceCode;
        public int LineNum;
        public SpectrCondition.CondTypes Type;
        public float CommonTime;
        public float[] Expositions;
        public bool IsActive
        {
            get
            {
                return IsFillLight || IsGenOn;
            }
        }
        public bool IsGenOn;
        public bool IsFillLight;
        public int TmpInteger;
        //public SpectrConditionGenCompiledLine GenCond;
        public int SpectrViewIndex;

        public bool IsEqual(SpectrConditionCompiledLine line)
        {
            bool flag = IsExpositionEqual(line,true);
            //if (flag == false)
                //return false;
            //flag = GenCond.IsEqual(line.GenCond);
            return flag;
        }

        public bool IsExpositionEqual(SpectrConditionCompiledLine line, bool and_common_time)
        //public bool IsExpositionEqual(SpectrConditionCompiledLine line)
        {
            //if (and_common_time && CommonTime != line.CommonTime)
            if (and_common_time == true && CommonTime != line.CommonTime)
                return false;
            if (Expositions == null || line.Expositions == null)
                return false;
            if (Expositions.Length != line.Expositions.Length)
                return false;
            for (int i = 0; i < Expositions.Length; i++)
                if (Expositions[i] != line.Expositions[i])
                    return false;
            return true;
        }

        public bool IsExpositionAndGenEqual(SpectrConditionCompiledLine line, bool and_common_time)
        {
            if (and_common_time && CommonTime != line.CommonTime)
                return false;
            if (Expositions == null || line.Expositions == null)
                return false;
            if (Expositions.Length != line.Expositions.Length)
                return false;
            for (int i = 0; i < Expositions.Length; i++)
                if (Expositions[i] != line.Expositions[i])
                    return false;
            return true;//GenCond.IsEqual(line.GenCond);
        }

        public string CompilationError;

        public SpectrConditionCompiledLine(SpectrCondition.CondTypes type,
            string line,int line_num,int sp_view_index, 
            ref bool fill_light_status)
        {
            SpectrViewIndex = sp_view_index;
            Type = type;
            SourceCode = line;
            LineNum = line_num;
            CompilationError = null;
            IsFillLight = fill_light_status;
            switch (type)
            {
                case SpectrCondition.CondTypes.Prespark:
                    ParsePrespark();
                    break;
                case SpectrCondition.CondTypes.Exposition:
                    ParseExposition();
                    break;
                case SpectrCondition.CondTypes.FillLight:
                    IsFillLight = ParseOnOf();
                    fill_light_status = IsFillLight;
                    break;
            }
        }

        public SpectrConditionCompiledLine(SpectrConditionCompiledLine com)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            com.Save(bw);
            bw.Flush();
            bw.Close();

            ms = new MemoryStream(ms.GetBuffer());
            BinaryReader br = new BinaryReader(ms);
            Load(br);
        }

        public SpectrConditionCompiledLine(BinaryReader br)
        {
            Load(br);
        }

        #region Save / Load
        void Load(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver < 0 || ver > 3)
                throw new Exception("Unsupported SpectrConditionCompiledLine version");

            //public string SourceCode;
            SourceCode = br.ReadString();
            //public int LineNum;
            LineNum = br.ReadInt32();
            //public SpectrCondition.CondTypes Type;
            Type = (SpectrCondition.CondTypes)br.ReadByte();
            //public float CommonTime;
            CommonTime = br.ReadSingle();
            //public float[] Expositions;
            int num = br.ReadInt32();
            if (num >= 0)
            {
                Expositions = new float[num];
                for (int i = 0; i < num; i++)
                    Expositions[i] = br.ReadSingle();
            }
            else
                Expositions = null;
            //public bool GenOn;
            IsGenOn = br.ReadBoolean();
            if (ver >= 2)
                IsFillLight = br.ReadBoolean();
            else
                IsFillLight = false;
            //public SpectrConditionGenCompiledLine GenCond;
            if(ver <= 2)
                new SpectrConditionGenCompiledLine(br);
            //public int SpectrViewIndex;
            SpectrViewIndex = br.ReadInt32();

            ver = br.ReadByte();
            if (ver != 91)
                throw new Exception("Unsupported SpectrConditionCompiledLine finish");
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)3);
            //public string SourceCode;
            bw.Write(SourceCode);
            //public int LineNum;
            bw.Write(LineNum);
            //public SpectrCondition.CondTypes Type;
            bw.Write((byte)Type);
            //public float CommonTime;
            bw.Write(CommonTime);
            //public float[] Expositions;
            //if (Expositions == null)
            //    ParseExposition();
            if (Expositions != null)
            {
                bw.Write(Expositions.Length);
                for (int i = 0; i < Expositions.Length; i++)
                    bw.Write(Expositions[i]);
            }
            else
                bw.Write(-1);
            //public bool GenOn;
            bw.Write(IsGenOn);
            bw.Write(IsFillLight);
            //public SpectrConditionGenCompiledLine GenCond;
            //GenCond.Save(bw);
            //public int SpectrViewIndex;
            bw.Write(SpectrViewIndex);
            bw.Write((byte)91);
        }
        #endregion

        string LoadValue(ref int index)
        {
            int from = index;
            while (index < SourceCode.Length && 
                (SourceCode[index] == ',' || SourceCode[index] == '.' || SourceCode[index] == ' ' ||
                char.IsDigit(SourceCode[index]) == true))
                index++;
            if (index == from)
                return null;
            return SourceCode.Substring(from, index - from).Trim();
        }

        bool ParseOnOf()
        {
            try
            {
                int ind = 1;
                string source_code = SourceCode.Trim().ToLower();
                if (source_code[ind] != ':')
                {
                    CompilationError = Common.MLS.Get("SpCond", "Не могу найти ':' после символа типа. В линии:") + SourceCode;
                    return false;
                }
                ind++;
                while (ind < source_code.Length && source_code[ind] == ' ')
                    ind ++;
                if (ind == source_code.Length)
                {
                    CompilationError = Common.MLS.Get("SpCond", "Не могу найти статус после ':'. В линии:") + SourceCode;
                    return false;
                }

                if ((source_code[ind] == 'в' && source_code[ind + 1] == 'к') ||
                    (source_code[ind] == 'o' && source_code[ind + 1] == 'n'))
                    return true;

                if ((source_code[ind] == 'в' && source_code[ind + 1] == 'ы') ||
                    (source_code[ind] == 'o' && source_code[ind + 1] == 'f'))
                    return false;

                CompilationError = Common.MLS.Get("SpCond", "Не известный после ':'. В линии:") + SourceCode;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            return false;
        }

        void ParsePrespark()
        {
            try
            {
                int ind = 0;
                if (SourceCode[ind] != 'p' && SourceCode[ind] != 'о')
                {
                    CompilationError = Common.MLS.Get("SpCond", "Это не строка обыскривания:") + SourceCode;
                    return;
                }
                ind++;
                if (SourceCode[ind] != ':')
                {
                    CompilationError = Common.MLS.Get("SpCond", "Не могу найти ':' после символа типа. В линии:") + SourceCode;
                    return;
                }
                ind++;
                string tmp = LoadValue(ref ind);
                if(tmp == null)
                {
                    CompilationError = Common.MLS.Get("SpCond", "Немогу распознать значение общего времени в строке:") + SourceCode;
                    return;
                }
                try
                {
                    CommonTime = (float)serv.ParseDouble(tmp);
                }
                catch
                {
                    CompilationError = Common.MLS.Get("SpCond", "Немогу распознать значение общего времени в строке:") + SourceCode + " " + tmp;
                    return;
                }

                //GenCond = new SpectrConditionGenCompiledLine(SourceCode, ind);
                //CompilationError = GenCond.CompilationError;
            }
            catch (Exception ex)
            {
                CompilationError = Common.MLS.Get("SpCond", "Ошибка при разборе строки:") + SourceCode;
                Common.Log(ex);
            }
        }

        void ParseExposition()
        {
            try
            {
                int ind = 0;
                if (SourceCode[ind] != 'e' && SourceCode[ind] != 'э')
                {
                    CompilationError = Common.MLS.Get("SpCond", "Это не строка экспозиции:") + SourceCode;
                    return;
                }
                ind++;
                if (SourceCode[ind] != ':')
                {
                    CompilationError = Common.MLS.Get("SpCond", "Не могу найти ':' после символа типа. В линии:") + SourceCode;
                    return;
                }
                ind++;
                string tmp = LoadValue(ref ind);
                if (tmp == null)
                {
                    CompilationError = Common.MLS.Get("SpCond", "Немогу распознать значение общего времени в строке:") + SourceCode;
                    return;
                }
                try
                {
                    CommonTime = (float)serv.ParseDouble(tmp);
                }
                catch
                {
                    CompilationError = Common.MLS.Get("SpCond", "Немогу распознать значение общего времени в строке:") + SourceCode + " " + tmp;
                    return;
                }
                if (SourceCode[ind] != '(')
                {
                    CompilationError = Common.MLS.Get("SpCond", "Немогу найти '(' после общего времени. В строке:") + SourceCode;
                    return;
                }
                ind++;
                List<float> exp = new List<float>();
                tmp = "";
                for (; ind < SourceCode.Length; ind++)
                {
                    tmp = LoadValue(ref ind);
                    try
                    {
                        exp.Add((float)serv.ParseDouble(tmp));
                    }
                    catch
                    {
                        CompilationError = Common.MLS.Get("SpCond", "Немогу распознать значение экспозиции в строке:") + SourceCode + " " + tmp;
                    }
                    if (SourceCode[ind] == ')')
                        break;
                }
                Expositions = new float[exp.Count];
                for (int i = 0; i < Expositions.Length; i++)
                    Expositions[i] = exp[i];
                if (SourceCode[ind] != ')')
                {
                    CompilationError = Common.MLS.Get("SpCond", "Немогу найти ')' описания экспозиций. В строке:") + SourceCode;
                    return;
                }
                ind++;
                if (SourceCode[ind] == 'o' && SourceCode[ind + 1] == 'n')
                {
                    IsGenOn = true;
                    ind += 2;
                }
                else
                {
                    if (SourceCode[ind] == 'г' && SourceCode[ind + 1] == 'в'
                         && SourceCode[ind + 2] == 'к' && SourceCode[ind + 3] == 'л')
                    {
                        IsGenOn = true;
                        ind += 4;
                    }
                    else
                    {
                        if (SourceCode[ind] == 'o' && SourceCode[ind + 1] == 'f'
                             && SourceCode[ind + 2] == 'f')
                        {
                            IsGenOn = false;
                            ind += 3;
                        }
                        else
                        {
                            if (SourceCode[ind] == 'г' && SourceCode[ind + 1] == 'в' &&
                                 SourceCode[ind + 2] == 'ы' && SourceCode[ind + 3] == 'к' && 
                                 SourceCode[ind + 4] == 'л')
                            {
                                IsGenOn = false;
                                ind += 5;
                            }
                            else
                            {
                                CompilationError = Common.MLS.Get("SpCond", "Немогу распознать состояние генератора в строке:") + SourceCode;
                                return;
                            }
                        }
                    }
                }
                //GenCond = new SpectrConditionGenCompiledLine(SourceCode, ind);
                //CompilationError = GenCond.CompilationError;
            }
            catch (Exception ex)
            {
                CompilationError = Common.MLS.Get("SpCond", "Немогу разобрать строку:") + SourceCode;
                Common.Log(ex);
            }
        }

        public override string ToString()
        {
            string ret = "";
            switch (Type)
            {
                case SpectrCondition.CondTypes.Prespark:
                    ret += Common.MLS.Get("SpCondLine","Обыскривание:")+CommonTime;
                    break;
                case SpectrCondition.CondTypes.Exposition:
                    if(IsGenOn)
                        ret += Common.MLS.Get("SpCondLine", " Измерение сигнала:");
                    else
                        ret += Common.MLS.Get("SpCondLine", " Калибровка фона:");
                    int com_i;
                    int[] exp_i;
                    SpectroWizard.dev.DevReg.SimpleTimeCorrection(Common.Dev, CommonTime, Expositions, out com_i, out exp_i);
                    ret += Math.Round(Common.Dev.Tick * com_i,2);
                    if(Expositions != null)
                    {
                        ret += "( ";
                        for (int i = 0; i < Expositions.Length; i++)
                            ret += Math.Round(exp_i[i]*Common.Dev.Tick,3) + " ";
                        ret += ")";
                    }
                    //ret += GenCond;
                    break;
                case SpectrCondition.CondTypes.FillLight:
                    if (IsFillLight)
                        ret += Common.MLS.Get("SpCondLine", " Включить заливающий свет");
                    else
                        ret += Common.MLS.Get("SpCondLine", " Выключить заливающий свет");
                    break;
                default:
                    return SourceCode;
            }
            return ret;
        }
    }
}
