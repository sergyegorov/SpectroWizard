using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
//using SpectroWizard.serv;

namespace SpectroWizard.data
{
    public class LineDb
    {
        public List<LineDbRecord> Data = new List<LineDbRecord>();

        public LineDb()
        {
        }

        public void Clear()
        {
            Data.Clear();
        }

        public void Add(LineDbRecord rec)
        {
            Data.Add(rec);
        }

        public LineDb(string file)
        {
            if (File.Exists(file) == false)
                return;
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte ver = br.ReadByte();
            int n = br.ReadInt32();
            for (int i = 0; i < n; i++)
                Data.Add(new LineDbRecord(br));
            ver = br.ReadByte();
            br.Close();

            if (File.Exists(file + "s") == false)
                return;
            fs = new FileStream(file + "s", FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);
            ver = br.ReadByte();
            n = br.ReadInt32();
            for (int i = 0; i < n; i++)
                Data[i].SrcText = br.ReadString();
            ver = br.ReadByte();
            br.Close();
        }

        public void Save(string file)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write((byte)1);
            bw.Write(Data.Count);
            for (int i = 0; i < Data.Count; i++)
                Data[i].Save(bw);
            bw.Write((byte)33);
            bw.Flush();
            bw.Close();

            fs = new FileStream(file + "s", FileMode.OpenOrCreate, FileAccess.Write);
            bw = new BinaryWriter(fs);
            bw.Write((byte)1);
            bw.Write(Data.Count);
            for (int i = 0; i < Data.Count; i++)
                bw.Write(Data[i].SrcText);
            bw.Write((byte)34);
            bw.Flush();
            bw.Close();
        }
    }

    public class LineDbRecord
    {
        public byte Element = 255;
        public string ElementName
        {
            get
            {
                return ElementTable.Elements[Element].Name;
            }
        }
        public byte IonLevel = 255;
        public float Ly;
        public short NistIntens = -1;
        public string NistIntensRem = "";
        public short ZIntensDuga = -1, ZIntensIskra = -1;
        public bool ZDugaR, ZIskraR;
        public float ZElemInt = -1;
        public string ZElemIntSrc = "";
        public short PDugaIntens = -1;
        public string SrcText = "";

        /*public int TypeOfIntens = 0;
        public float QIntens
        {
            get
            {
                switch (TypeOfIntens)
                {
                    case 0:
                        return NistIntens;
                    case 1:
                        return ZIntensIskra;
                    case 2:
                        return ZIntensDuga;
                    default: throw new NotImplementedException();
                }
            }
        }*/

        public float GetQIntens(int type)
        {
            switch (type)
            {
                case 0:
                    return NistIntens;
                case 1:
                    return ZIntensIskra;
                case 2:
                    return ZIntensDuga;
                default: throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            string ret = ElementTable.Elements[Element].Name;
            if (ret.Length == 1)
                ret += " ";

            ret += " " + IonLevel;
            while (ret.Length < 5)
                ret += " ";

            ret += Math.Round(Ly,2);
            while (ret.Length < 13)
                ret += " ";

            if (NistIntens >= 0)
                ret += "N:" + NistIntens + NistIntensRem;
            while (ret.Length < 20)
                ret += " ";

            if (ZIntensDuga >= 0)
                ret += " lD:"+ZIntensDuga;
            if (ZDugaR)
                ret += 'R';
            while (ret.Length < 27)
                ret += " ";

            if (ZIntensIskra >= 0)
                ret += " lI:" + ZIntensIskra;
            if (ZIskraR)
                ret += 'R';
            while (ret.Length < 34)
                ret += " ";

            if (ZElemInt >= 0)
                ret += " E:" + ZElemInt + ZElemIntSrc;
            while (ret.Length < 41)
                ret += " ";

            if (PDugaIntens >= 0)
                ret += " DPl:"+PDugaIntens;
            while (ret.Length < 48)
                ret += " ";

            return ret;
        }

        public LineDbRecord(BinaryReader br)
        {
            if (br.ReadByte() != 1)
                throw new Exception("Неправильная версия LineDbRecord.");
            Element = br.ReadByte();
            IonLevel = br.ReadByte();
            Ly = br.ReadSingle();
            NistIntens = br.ReadInt16();
            NistIntensRem = br.ReadString();
            ZIntensDuga = br.ReadInt16();
            ZIntensIskra = br.ReadInt16();
            ZDugaR = br.ReadBoolean();
            ZIskraR = br.ReadBoolean();
            ZElemInt = br.ReadSingle();
            ZElemIntSrc = br.ReadString();
            PDugaIntens = br.ReadInt16();
            if (br.ReadByte() != 38)
                throw new Exception("Неправильное окончание LineDbRecord.");
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)1);
            bw.Write(Element);
            bw.Write(IonLevel);
            bw.Write(Ly);
            bw.Write(NistIntens);
            bw.Write(NistIntensRem);
            bw.Write(ZIntensDuga);
            bw.Write(ZIntensIskra);
            bw.Write(ZDugaR);
            bw.Write(ZIskraR);
            bw.Write(ZElemInt);
            bw.Write(ZElemIntSrc);
            bw.Write(PDugaIntens);
            bw.Write((byte)38);
        }

        public LineDbRecord(string elem, byte ion, float ly)
        {
            for (int i = 0; i < ElementTable.Elements.Length; i++)
            {
                if (ElementTable.Elements[i].Name.Equals(elem))
                {
                    Element = (byte)i;
                    break;
                }
            }
            
            if (Element == 255)
                throw new Exception("Unknown element");

            IonLevel = ion;
            Ly = ly;
        }

        public LineDbRecord(string element, string ion_level,
            string ly)
        {
            for (int i = 0; i < ElementTable.Elements.Length; i++)
            {
                if (ElementTable.Elements[i].Name.Equals(element))
                {
                    Element = (byte)i;
                    break;
                }
            }

            if (Element == 255)
                throw new Exception("Неизвестный элемент: '"+element+"'");

            IonLevel = (byte)serv.ParseRim(ion_level);

            Ly = (float)serv.ParseDouble(ly);
        }
    }
}
