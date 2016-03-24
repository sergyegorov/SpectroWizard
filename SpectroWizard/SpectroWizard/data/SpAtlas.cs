using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using System.Collections;

namespace SpectroWizard.data
{
    public class SpAtlas
    {
        #region Pixel container functions
        List<SpAtlasPixel> PixelList = new List<SpAtlasPixel>();
        public SpAtlasPixel this[int index]
        {
            get
            {
                return PixelList[index];
            }
        }

        public int Length
        {
            get
            {
                return PixelList.Count;
            }
        }

        void AddRangeHi(SpAtlasPixel[] pixels)
        {
        }

        public void AddRange(float[] data, int sn, Dispers disp)
        {
            SpAtlasPixel[] data_hi = new SpAtlasPixel[data.Length*10];
            for (int pixel = 0; pixel < data.Length - 1; pixel++)
            {
                float ly_from = (float)disp.GetLyByLocalPixel(sn, 0);
                float val_from = data[0];
                float ly_to = (float)disp.GetLyByLocalPixel(sn, data.Length-1);
                float val_to = data[data.Length-1];
                int from = pixel * 10;
                //for(data_hi)
            }
            int insert_into = 0;
            if (PixelList.Count > 0)
            {
                float ly_from = (float)disp.GetLyByLocalPixel(sn,0);
                float ly_to = (float)disp.GetLyByLocalPixel(sn,data.Length-1);
                if (ly_from > PixelList[PixelList.Count - 1].Ly ||
                    ly_to < PixelList[0].Ly)
                {
                    Load();
                    throw new Exception("Нельзя создать атлаc с разрывами...");
                }

                if (ly_from < PixelList[0].Ly)
                {
                    while (PixelList.Count > 0 && PixelList[0].Ly < ly_to)
                        PixelList.RemoveAt(0);
                }
                else
                {
                    for (; insert_into < PixelList.Count && PixelList[insert_into].Ly < ly_from; insert_into++) ;
                    if (ly_to > PixelList[PixelList.Count - 1].Ly)
                    {
                        PixelList.RemoveRange(insert_into, PixelList.Count - insert_into);
                    }
                    else
                    {
                    }
                }
                return;
            }
            for (int pixel = 0; pixel < data.Length; pixel++)
                PixelList.Insert(insert_into,new SpAtlasPixel((float)disp.GetLyByLocalPixel(sn, pixel), data[pixel]));

        }
        #endregion

        const string FileName = "link_attlas.bin";
        public void Save()
        {
            FileStream fs = new FileStream(Common.SpAtlasPath + "\\" + FileName, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(1);
                bw.Write(PixelList.Count);
                for (int i = 0; i < PixelList.Count; i++)
                    PixelList[i].Save(bw);
                bw.Flush();
            }
            finally
            {
                fs.Close();
            }
        }

        public void Load()
        {
            FileStream fs = new FileStream(Common.SpAtlasPath + "\\" + FileName, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                BinaryReader br = new BinaryReader(fs);
                int ver = br.ReadInt32();// bw.Write(1);
                int n = br.ReadInt32();// bw.Write(PixelList.Count);
                PixelList.Clear();
                for (int i = 0; i < n; i++)
                    PixelList.Add(new SpAtlasPixel(br));//PixelList[i].Save(bw);
            }
            finally
            {
                fs.Close();
            }
        }
    }

    public class SpAtlasPixel
    {
        public float Ly;
        public float Val;

        public SpAtlasPixel(float ly, float val)
        {
            Ly = ly;
            Val = val;
        }

        public void InitBy(float ly, float val)
        {
            Ly = ly;
            Val = val;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)1);
            bw.Write(Ly);
            bw.Write(Val);
        }

        public SpAtlasPixel(BinaryReader br)
        {
            int ver = br.ReadByte();
            if (ver < 1 || ver > 1)
                throw new Exception("Wrong version of atlas pixel");
            Ly = br.ReadSingle();
            Val = br.ReadSingle();
        }
    }
}
