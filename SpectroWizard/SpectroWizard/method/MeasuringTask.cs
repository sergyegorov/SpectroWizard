using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;
using SpectroWizard.dev;
using System.IO;

using SpectroWizard.data;
using SpectroWizard.method;
using SpectroWizard.dev;
using SpectroWizard.analit;

namespace SpectroWizard.method
{
    public class MeasuringSimpleTask : IDisposable
    {
        const string MLSConst = "MTSimple";
        MethodSimple DataPriv = null;
        public MethodSimple Data
        {
            get
            {
                return DataPriv;
            }
            set
            {
                if (DataPriv == value)
                    return;
                if (DataPriv != null)
                    DataPriv.Dispose();
                DataPriv = value;
            }
        }
        bool Created = false;
        public string SrcMethodPath = "";

        public string TaskPath
        {
            get
            {
                return (string)Path.Clone();
            }
        }

        public void Dispose()
        {
            Data.Dispose();
            Data = null;
        }

        public bool IsChanged()
        {
            if (Created)
                return true;
            return Data.IsChanged();
        }

        public MeasuringSimpleTask(string path,MethodSimple method,string method_path)
        {
            Created = true;
            Path = path;
            SrcMethodPath = method_path;
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            method.Save(bw);
            bw.Flush();

            ms.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(ms);
            Data = new MethodSimple(br);

            Data.ClearProbRecords();

            Data.MethodMovedTo(SrcMethodPath,Path);
            br.Close();
            bw.Close();
            try { ms.Close(); }
            catch { }
            try { ms.Dispose(); }
            catch { }
        }

        string Path;
        public MeasuringSimpleTask(string path)
        {
            Path = (string)path.Clone();
            path += "method";
            FileStream fs = DataBase.OpenFile(ref path,FileMode.Open,FileAccess.Read);
            try
            {
                BinaryReader br = new BinaryReader(fs);
                string type = br.ReadString();
                if (type.Equals("mt1") == false)
                    throw new Exception("Wrong file type");
                int ver = br.ReadInt32();
                if (ver != 1 && ver != 2)
                    throw new Exception("Unsupported version of file");

                Data = new MethodSimple(br);
                Data.SetupPath(path);

                if (ver > 1)
                    SrcMethodPath = br.ReadString();
                else
                    SrcMethodPath = "";

                ver = br.ReadInt32();
                if (ver != 234324)
                    throw new Exception("Wrong end of file.");
            }
            finally
            {
                fs.Close();
            }
        }

        public List<SpectrConditionCompiledLine> GetNullList()
        {
            List<SpectrConditionCompiledLine> ret = new List<data.SpectrConditionCompiledLine>();
            for (int i = 0; i < Data.CommonInformation.WorkingCond.Lines.Count; i++)
            {
                SpectrConditionCompiledLine l = Data.CommonInformation.WorkingCond.Lines[i];
                if (l.IsActive == false && l.Type == SpectrCondition.CondTypes.Exposition)
                    ret.Add(l);
            }
            return ret;
        }

        public void Save()
        {
            DataBase.CheckPath(ref Path);
            string tmp = Path+"method";

            string fp = DataBase.GetFullPath(tmp);
            string fp_ = null;
            if (File.Exists(fp))
            {
                fp_ = fp + "_";
                File.Copy(fp, fp_);
            }
            FileStream fs = DataBase.OpenFile(ref tmp, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                fs.SetLength(0);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write("mt1");
                bw.Write(2);
                Data.Save(bw);
                bw.Write(SrcMethodPath);
                bw.Write(234324);
                bw.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                fs.Close();
                Common.Log(ex);
                if (fp_ != null)
                {
                    File.Delete(fp);
                    File.Copy(fp_, fp);
                }
            }
            finally
            {
                if (fp_ != null)
                    File.Delete(fp_);
            }
        }
    }
}
