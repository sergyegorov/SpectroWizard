using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;
using System.IO;

namespace SpectroWizard.method
{
    public class StandartHistory
    {
        List<StandartHistryRecord> Records = new List<StandartHistryRecord>();
        public StandartHistryRecord this[long date,string element]
        {
            get
            {
                foreach(StandartHistryRecord rec in Records)
                    if(rec.Time.Ticks == date && rec.Element.Equals(element))
                        return rec;
                foreach (StandartHistryRecord rec in Records)
                    if (rec.Time.Ticks == date && (rec.Element+"["+rec.FormulaName+"]").Equals(element))
                        return rec;
                return null;
            }
        }

        public int Count
        {
            get
            {
                return Records.Count;
            }
        }

        public List<string> ExtraNames;
        public List<long> GetMeasuringList()
        {
            List<long> ret = new List<long>();
            ExtraNames = new List<string>();
            foreach (StandartHistryRecord rec in Records)
            {
                long name = rec.Time.Ticks;
                if (ret.IndexOf(name) >= 0)
                    continue;
                ret.Add(name);
                ExtraNames.Add(Math.Round(rec.Con,3).ToString());
            }
            return ret;
        }

        public void RemoveAt(long date)
        {
            for (int i = 0; i < Records.Count;i++ )
            {
                if (Records[i].Time.Ticks == date)
                {
                    Records.RemoveAt(i);
                    i--;
                }
            }
            Save();
            Load();
        }

        public List<string> GetElementList()
        {
            List<string> ret = new List<string>();
            foreach (StandartHistryRecord rec in Records)
            {
                string name = rec.GetKeyName();
                if (ret.IndexOf(name) >= 0)
                    continue;
                ret.Add(name);
            }
            return ret;
        }

        string Path;
        public StandartHistory(Spectr s)
        {
            string path = s.GetPath();
            path = path.Substring(0,path.LastIndexOf('_'));
            Path = path + ".history";
            //DataBase.CheckPath(ref path);
            Load();
        }

        void Load()
        {
            if (DataBase.FileExists(ref Path) == false)// File.Exists(Path) == false)
                return;
            //FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
            Records.Clear();
            FileStream fs = DataBase.OpenFile(ref Path, FileMode.Open, FileAccess.Read);
            try
            {
                BinaryReader br = new BinaryReader(fs);
                int ver = br.ReadInt32();
                if (ver != 1)
                    throw new Exception("Unsupported version of StandartHistory");
                int l = br.ReadInt32();
                for (int i = 0; i < l; i++)
                    Records.Add(new StandartHistryRecord(br));
            }
            finally
            {
                fs.Close();
            }
        }

        void Save()
        {
            FileStream fs = DataBase.OpenFile(ref Path, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                fs.SetLength(0);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(1);
                bw.Write(Records.Count);
                foreach (StandartHistryRecord rec in Records)
                    rec.Save(bw);
                bw.Flush();
            }
            finally
            {
                fs.Close();
            }
        }

        public void Add(MethodSimple method, int prob, int sub_prob)
        {
            DateTime now = DateTime.Now;
            Element[] elem = method.GetElementList();
            MethodSimpleProb msp = method.GetProbHeader(prob);
            for(int el = 0;el<elem.Length;el++)
            {
                MethodSimpleCell cell = method.GetCell(el,prob);
                MethodSimpleElement mse = method.GetElHeader(el);
                for (int f = 0; f < mse.Formula.Count; f++)
                {
                    MethodSimpleElementFormula formula = mse.Formula[f];
                    MethodSimpleCellFormulaResult mscfr = cell.GetData(sub_prob, f);
                    Records.Add(new StandartHistryRecord(mse, f, formula, mscfr, now));
                }
            }
            Save();
        }
    }

    public class StandartHistryRecord
    {
        public string Element;
        public int Formula;
        public string FormulaName;
        public List<GLogRecord> LogData = new List<GLogRecord>();
        public double[] Cons;
        public DateTime Time;

        public double Con
        {
            get
            {
                return SpectroWizard.analit.Stat.GetEver(Cons);
            }
        }

        public string GetKeyName()
        {
            if (Formula == 0)
                return Element;
            else
                return Element + "[" + FormulaName + "]";
        }

        public StandartHistryRecord(MethodSimpleElement mse,int formula,
            MethodSimpleElementFormula msef,
            MethodSimpleCellFormulaResult mscfr,
            DateTime measured)
        {
            Time = new DateTime(DateTime.Now.Ticks);
            Element = mse.Element.Name;
            Formula = formula;
            FormulaName = msef.Name;
            LogData = mscfr.LogData;
            Cons = mscfr.ReCalcCon;
            Time = new DateTime(measured.Ticks);
        }

        public StandartHistryRecord(BinaryReader br)
        {
            int ver = br.ReadInt32();
            if (ver < 1 || ver > 1)
                throw new Exception("Unsupported st history record version...");
            Element = br.ReadString();
            Formula = br.ReadInt32();
            FormulaName = br.ReadString();
            int l = br.ReadInt32();
            for (int i = 0; i < l; i++)
                LogData.Add(GLogRecord.Load(br));
            l = br.ReadInt32();
            Cons = new double[l];
            for (int i = 0; i < Cons.Length; i++)
                Cons[i] = br.ReadDouble();
            Time = new DateTime(br.ReadInt64());
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);
            bw.Write(Element);
            bw.Write(Formula);
            bw.Write(FormulaName);
            bw.Write(LogData.Count);
            foreach (GLogRecord rec in LogData)
                rec.Save(bw);
            if (Cons != null)
            {
                bw.Write(Cons.Length);
                foreach (double val in Cons)
                    bw.Write(val);
            }
            else
                bw.Write(0);
            bw.Write(Time.Ticks);
        }
    }
}
