using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

//using System.Xml;
//using System.Xml.Serialization;
//using System.IO;
//using System.Xml.Schema;

using SpectroWizard.data;
using System.Collections;

namespace SpectroWizard
{
    //[Serializable()]
    public class Env
    {
        public string LinkintEditorText = "";
        public string LinkintEditorTextOnEnter = "";
        public string LinkingEditorGlobalLyText = "";
        public string LinkingEditorLinePixelText = "";
        public string TestFlat = "";
        public string TestSens = "";
        public string TestShift = "";
        byte[] DefaultDispData = null;
        public string DefaultDispText = "";
        byte[] DefaultOpticK = null;
        public double GausW = 1.5;
        public double GausWBest = 1.5;
        public string OFkLogCalc = "";
        public int ReportNumber = 0;
        Hashtable StringValues = new Hashtable();
        public int MaxAmplDltPrs = 33;
        public float MaxAmplDlt
        {
            get
            {
                return MaxAmplDltPrs / 100F;
            }
        }
        public string GetStringVal(string key, string default_val)
        {
            if (StringValues.ContainsKey(key) == false)
                return default_val;
            string ret = (string)StringValues[key];
            return ret;
        }
        public void SetStringVal(string key, string val)
        {
            if (StringValues.ContainsKey(key))
                StringValues[key] = val;
            else
                StringValues.Add(key, val);
        }
        public Hashtable BoolValues = new Hashtable();
        public bool GetBoolVal(string key, bool default_val)
        {
            if(BoolValues.ContainsKey(key) == false)
                return default_val;
            bool ret = (bool)BoolValues[key];
            return ret;
        }
        public void SetBoolVal(string key, bool val)
        {
            if (BoolValues.ContainsKey(key))
                BoolValues[key] = val;
            else
                BoolValues.Add(key, val);
        }
        public Hashtable LongValues = new Hashtable();
        public Hashtable DoubleValues = new Hashtable();
        public int DefaultFontSize = 11;

        //[NonSerialized()]
        OpticFk DefaultOpticFkPriv = null;
        public OpticFk DefaultOpticFk
        {
            get
            {
                if (DefaultOpticFkPriv == null)
                {
                    try
                    {
                        if (DefaultOpticK != null)
                        {
                            MemoryStream ms = new MemoryStream(DefaultOpticK);
                            BinaryReader br = new BinaryReader(ms);
                            DefaultOpticFkPriv = new OpticFk(br);
                            br.Close();
                        }
                        else
                            DefaultOpticFkPriv = new OpticFk();
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                        DefaultOpticFkPriv = new OpticFk();
                    }
                }
                return DefaultOpticFkPriv;
            }
            set
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                value.Save(bw);
                bw.Flush();
                ms.Position = 0;

                BinaryReader br = new BinaryReader(ms);
                DefaultOpticFkPriv = new OpticFk(br);//value;
                br.Close();
                bw.Close();
                
                Commit();
            }
        }

        void Commit()
        {
            if (DefaultOpticFkPriv != null)
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                DefaultOpticFkPriv.Save(bw);
                bw.Flush();
                DefaultOpticK = ms.GetBuffer();
                bw.Close();
            }

            if (DefaultDispPriv != null)
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                DefaultDispPriv.Save(bw);
                bw.Flush();
                DefaultDispData = ms.GetBuffer();
                bw.Close();
            }
        }

        //[NonSerialized()]
        Dispers DefaultDispPriv = null;
        public Dispers DefaultDisp
        {
            get
            {
                if (DefaultDispPriv == null)
                {
                    try
                    {
                        if (DefaultDispData != null)
                        {
                            MemoryStream ms = new MemoryStream(DefaultDispData);
                            BinaryReader br = new BinaryReader(ms);
                            DefaultDispPriv = new Dispers(br);
                            br.Close();
                        }
                        else
                            DefaultDispPriv = new Dispers();
                    }
                    catch (Exception ex)
                    {
                        Common.Log(ex);
                        DefaultDispPriv = new Dispers();
                    }
                }
                return DefaultDispPriv;
            }
            set
            {
                DefaultDispPriv = new Dispers(value,false);
                Commit();
            }
        }

        //static string BaseFolder;
        public void Store()
        {
            Store(Common.Conf.DbPath);
        }

        static void Store(BinaryWriter bw, byte[] buf)
        {
            if (buf == null)
                bw.Write(-1);
            else
            {
                bw.Write(buf.Length);
                foreach (byte tmp in buf)
                    bw.Write(tmp);
            }
        }

        static void StoreHash(BinaryWriter bw,Hashtable hash)
        {
            if (hash == null)
                hash = new Hashtable();
            //BinaryWriter bw = new BinaryWriter(str);
            bw.Write(1);
            bw.Write(hash.Count);
            foreach(object key in hash.Keys)
            {
                string skey = (string)key;
                bw.Write(skey);
                object val = hash[key];
                if (val is string)
                {
                    bw.Write((byte)1);
                    bw.Write((string)val);
                    continue;
                }
                else
                {
                    if (val is bool)
                    {
                        bw.Write((byte)2);
                        bw.Write((bool)val);
                        continue;
                    }
                    else
                    {
                        if (val is long)
                        {
                            bw.Write((byte)3);
                            bw.Write((long)val);
                            continue;
                        }
                        else
                        {
                            if (val is double)
                            {
                                bw.Write((byte)4);
                                bw.Write((double)val);
                                continue;
                            }
                            else
                            {
                                if (val is decimal)
                                {
                                    bw.Write((byte)5);
                                    bw.Write((decimal)val);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }
        public int DefaultLineDb = 1;


        public void Store(string base_folder)
        {
            //BaseFolder = (string)base_folder.Clone();
            Commit();
            Stream stream = File.Open(Common.DBBaseEnv + "env.bin", FileMode.OpenOrCreate);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(4);
            //public string LinkintEditorText = "";
            bw.Write(LinkintEditorText);
            bw.Write(LinkingEditorGlobalLyText);
            bw.Write(LinkingEditorLinePixelText);
            //public string TestFlat = "";
            bw.Write(TestFlat);
            //public string TestSens = "";
            bw.Write(TestSens);
            //public string TestShift = "";
            bw.Write(TestShift);
            //byte[] DefaultDispData = null;
            Store(bw,DefaultDispData);
            //byte[] DefaultOpticK = null;
            Store(bw, DefaultOpticK);
            //public double GausW = 1.5;
            bw.Write(GausW);
            //public double GausWBest = 1.5;
            bw.Write(GausWBest);
            //public string OFkLogCalc = "";
            bw.Write(OFkLogCalc);

            StoreHash(bw, StringValues);
            StoreHash(bw, BoolValues);
            StoreHash(bw, LongValues);
            StoreHash(bw, DoubleValues);

            bw.Write(ReportNumber);

            bw.Write(DefaultFontSize);

            bw.Write(MaxAmplDltPrs);

            if (DefaultDispText == null)
                bw.Write("");
            else
                bw.Write(DefaultDispText.Trim());

            bw.Write(DefaultLineDb);

           // bw.Write(2394852);

            bw.Flush();
            stream.Close();
        }

        static byte[] ResotreByteArray(BinaryReader br)
        {
            int len = br.ReadInt32();
            if (len < 0)
                return null;
            return br.ReadBytes(len);
        }

        static void Restore(BinaryReader br, ref Hashtable hash)
        {
            //BinaryReader br = new BinaryReader(str);
            int ver = br.ReadInt32();
            if (ver != 1)
                throw new Exception("Not valid version of hash record");
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = br.ReadString();
                byte type = br.ReadByte();
                object val;
                switch (type)
                {
                    case 1: val = br.ReadString();  break;
                    case 2: val = br.ReadBoolean(); break;
                    case 3: val = br.ReadInt64(); break;
                    case 4: val = br.ReadDouble(); break;
                    case 5: val = br.ReadDecimal(); break;
                    default:
                        throw new Exception("Not implemented hash data type: " + type);
                }
                hash.Add(key, val);
            }
        }

        static public Env Restore(string base_folder)
        {
            base_folder = (string)base_folder.Clone();
            if (File.Exists(base_folder + "env.bin") == false)
                return new Env();
            Stream stream = File.Open(base_folder + "env.bin", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);

            Env ret = new Env();

            int ver = br.ReadInt32();//bw.Write(1);
            //if (ver < 1 || ver > 3)
            //    throw new Exception();
            //public string LinkintEditorText = "";
            ret.LinkintEditorText = br.ReadString(); //bw.Write(LinkintEditorText);
            ret.LinkintEditorTextOnEnter = (string)ret.LinkintEditorText.Clone();

            if (ver >= 3)
            {
                ret.LinkingEditorGlobalLyText = br.ReadString();
                ret.LinkingEditorLinePixelText = br.ReadString();
            }
                //public string TestFlat = "";
            ret.TestFlat = br.ReadString(); //bw.Write(TestFlat);
            //public string TestSens = "";
            ret.TestSens = br.ReadString(); //bw.Write(TestSens);
            //public string TestShift = "";
            ret.TestShift = br.ReadString(); //bw.Write(TestShift);
            //byte[] DefaultDispData = null;
            ret.DefaultDispData = ResotreByteArray(br);//Store(bw, DefaultDispData);
            //byte[] DefaultOpticK = null;
            ret.DefaultOpticK = ResotreByteArray(br);//Store(bw, DefaultOpticK);
            //public double GausW = 1.5;
            ret.GausW = br.ReadDouble();//bw.Write(GausW);
            //public double GausWBest = 1.5;
            ret.GausWBest = br.ReadDouble();//bw.Write(GausWBest);
            //public string OFkLogCalc = "";
            ret.OFkLogCalc = br.ReadString();//bw.Write(OFkLogCalc);

            Restore(br, ref ret.StringValues);
            Restore(br, ref ret.BoolValues);
            Restore(br, ref ret.LongValues);
            Restore(br, ref ret.DoubleValues);

            ret.ReportNumber = br.ReadInt32();
            ret.DefaultFontSize = br.ReadInt32();

            int tmp;
            try
            {
                ret.MaxAmplDltPrs = br.ReadInt32();
                
                if (ver >= 2)
                    ret.DefaultDispText = br.ReadString();
     
                tmp = br.ReadInt32();
            }
            catch
            {
                tmp = 0;
            }

            if (ver >= 4)
                ret.DefaultLineDb = tmp;
            if (ret.DefaultLineDb < 0 || ret.DefaultLineDb > 2)
                ret.DefaultLineDb = 1;

            /*if (tmp != 2394852)
            {
                ret.MaxAmplDltPrs = 30;
            }*/

            stream.Close();
            return ret;
        }
    }
}
