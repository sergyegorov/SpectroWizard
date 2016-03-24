using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.IO;
using SpectroWizard.util;

namespace SpectroWizard.data
{
    public class PlainSpectr
    {
        void Log(Exception ex)
        {
            Common.Log(ex);
        }

        public PlainSpectr()
        {
        }

        public PlainSpectr(string path)
        {
            Load(path);
        }

        public FRectangle GetSize()
        {
            float min_ly = float.MaxValue;
            float max_ly = -float.MaxValue;
            float min_val = float.MaxValue;
            float max_val = -float.MaxValue;

            for (int f = 0; f < Frames.Count; f++)
            {
                PlainSpectrFrame fr = Frames[f];
                for (int p = 0; p < fr.Pixels.Count; p++)
                {
                    if (min_ly > fr.Pixels[p].LyFrom)
                        min_ly = fr.Pixels[p].LyFrom;
                    if (max_ly < fr.Pixels[p].LyTo)
                        max_ly = fr.Pixels[p].LyTo;
                    if (min_val > fr.Pixels[p].Value)
                        min_val = fr.Pixels[p].Value;
                    if (max_val < fr.Pixels[p].Value)
                        max_val = fr.Pixels[p].Value;
                }
            }

            return new FRectangle(min_ly, min_val, max_ly, max_val);
        }

        public int FrameCount { get { return Frames.Count; } }

        public List<PlainSpectrFrame> Frames = new List<PlainSpectrFrame>();

        public void Save(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write('p');
            bw.Write('s');
            bw.Write('p');
            bw.Write(1);
            bw.Write(Frames.Count);
            foreach (PlainSpectrFrame f in Frames)
                f.Save(bw);
            bw.Flush();
            bw.Close();
        }

        public void Load(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            if(br.ReadChar() != 'p' ||//bw.Write('p');
                br.ReadChar() != 's' ||//bw.Write('s');
                br.ReadChar() != 'p')//bw.Write('p');
                throw new Exception("Wrong type of PlainSpectr...");
            if (br.ReadInt32() != 1)//bw.Write(1);
                throw new Exception("Wrong version of PlainSpectr...");
            //bw.Write(Frames.Count);
            //foreach (PlainSpectrFrame f in Frames)
                //f.Save(bw);
            int n = br.ReadInt32();
            for (int i = 0; i < n; i++)
                Frames.Add(new PlainSpectrFrame(br));//bw.Flush();
            br.Close();
        }

        public int AddFrame(float time)
        {
            Frames.Add(new PlainSpectrFrame(time));
            return Frames.Count - 1;
        }

        public void AddPixel(int frame, float ly_from, float ly_to, float signal_value,float nul_value,float max_line)
        {
            float val;
            if (signal_value < max_line)
                val = (signal_value - nul_value) / Frames[frame].Time;
            else
                val = float.MaxValue;
            if (serv.IsValid(val) == false)
                val = float.MaxValue;
            Frames[frame].AddPixel(ly_from, ly_to, val);
        }

        #region FinalCompile functions -------------------------------------------
        public void FinalCompile_SpredMax()
        {
            for (int f = 0; f < Frames.Count; f++)
            {
                int found = 0;
                for (int p = 0; p < Frames[f].Pixels.Count; p++)
                {
                    if (Frames[f].Pixels[p].Value == float.MaxValue)
                        found++;
                    else
                    {
                        if (found > 3)
                        {
                            if (p > 0)
                                Frames[f].Pixels[p - 1].Value = float.MaxValue;
                            if (p > 1)
                                Frames[f].Pixels[p - 2].Value = float.MaxValue;
                            if (p < Frames[f].Pixels.Count - 1)
                                Frames[f].Pixels[p + 1].Value = float.MaxValue;
                            if (p < Frames[f].Pixels.Count - 2)
                                Frames[f].Pixels[p + 2].Value = float.MaxValue;
                        }
                        found = 0;
                    }
                }
            }
        }

        public float[] RateFrames()
        {
            float[] ret = new float[Frames.Count];
            ret[0] = 1;
            for (int f = 1; f < Frames.Count; f++)
            {
                double val = 0;
                double count = 0;
                for (int p = 0; p < Frames[0].Pixels.Count; p++)
                {
                    if (Frames[f].Pixels[p].Value == float.MaxValue ||
                        Frames[f - 1].Pixels[p].Value == float.MaxValue ||
                        Frames[f - 1].Pixels[p].Value < 1 || 
                        Frames[f].Pixels[p].Value < 1)
                        continue;
                    double k = Math.Abs(Frames[f].Pixels[p].Value / 20);
                    k = k * k * k;
                    double v = (Frames[f].Pixels[p].Value / Frames[f - 1].Pixels[p].Value) * k;
                    if (serv.IsValid(v) == false)
                        continue;
                    count += k;
                    val += v;
                }
                ret[f] = (float)(val / count);
            }
            for (int f = 0; f < Frames.Count; f++)
            {
                for (int p = 0; p < Frames[f].Pixels.Count; p++)
                {
                    if(Frames[f].Pixels[p].Value == float.MaxValue) 
                        Frames[f].Pixels[p].Corrected = true;
                }
            }
            return ret;
        }
        #endregion

        public void FinalCompile()
        {
            int pixel_count = Frames[0].Pixels.Count();
            FinalCompile_SpredMax();
            float[] rates = RateFrames();
            for (int p = 0; p < Frames[0].Pixels.Count; p++)
            {
                int prev_fr_err = Frames.Count;
                int fr_err = 0;
                for (int f = 0; f < Frames.Count; f++)
                    if (Frames[f].Pixels[p].Value == float.MaxValue)
                        fr_err++;
                if (fr_err > 0)
                {
                    for (int f = 1; f < Frames.Count; f++)
                    {
                        if (Frames[f].Pixels[p].Value != float.MaxValue)
                            continue;
                        if (Frames[f-1].Pixels[p].Value == float.MaxValue)
                            continue;
                        Frames[f].Pixels[p].Value = Frames[f - 1].Pixels[p].Value * rates[f];
                    }
                    do//while (is_ok == false)
                    {
                        prev_fr_err = fr_err;
                        fr_err = 0;
                        for (int f = 0; f < Frames.Count - 1; f++)
                        {
                            if (Frames[f].Pixels[p].Value < float.MaxValue &&
                                Frames[f + 1].Pixels[p].Value == float.MaxValue)
                                Frames[f+1].Pixels[p].Value = Frames[f].Pixels[p].Value;
                        }
                        for (int f = Frames.Count - 2; f >= 0; f--)
                        {
                            if (Frames[f].Pixels[p].Value == float.MaxValue &&
                                Frames[f + 1].Pixels[p].Value < float.MaxValue)
                                Frames[f].Pixels[p].Value = Frames[f+1].Pixels[p].Value;
                        }
                        for (int f = 0; f < Frames.Count; f++)
                            if (Frames[f].Pixels[p].Value == float.MaxValue)
                                fr_err++;
                    } while (prev_fr_err > fr_err);
                }
            }
            for (int f = 0; f < Frames.Count; f++)
            {
                for (int p = 0; p < Frames[f].Pixels.Count; p++)
                {
                    float val = Frames[f].Pixels[p].Value;
                    if (val == float.MaxValue || serv.IsValid(val) == false)
                    {
                        float ever = 0;
                        int ever_count = 0;
                        int i_from = p;
                        int i_to = p;
                        for (int i = p - 1; i >= 0; i--)
                        {
                            val = Frames[f].Pixels[i].Value;
                            if (val < float.MaxValue && serv.IsValid(val) != false)
                            {
                                ever += val;
                                ever_count++;
                                break;
                            }
                            else
                                i_from = i;
                        }
                        for (int i = p + 1; i < Frames[f].Pixels.Count; i++)
                        {
                            val = Frames[f].Pixels[i].Value;
                            if (val < float.MaxValue && serv.IsValid(val) != false)
                            {
                                ever += val;
                                ever_count++;
                                break;
                            }
                            else
                                i_to = i;
                        }
                        if (ever_count == 0)
                            ever_count = 1;
                        Frames[f].Pixels[p].Value = ever / ever_count;
                        if(i_from < 0)
                            i_from = 0;
                        if(i_to >= Frames[f].Pixels.Count)
                            i_to = Frames[f].Pixels.Count-1;
                        for (int i = i_from; i <= i_to; i++)
                            Frames[f].Pixels[p].Value = Frames[f].Pixels[p].Value;
                    }
                }
            }
        }
    }

    public class PlainSpectrFrame
    {
        public float Time;
        public PlainSpectrFrame(float time)
        {
            Time = time;
        }

        public List<PlainSpectrFramePixel> Pixels = new List<PlainSpectrFramePixel>();
        public void AddPixel(float ly_from, float ly_to, float val)
        {
            if (Pixels.Count == 0)
            {
                Pixels.Add(new PlainSpectrFramePixel(ly_from, ly_to, val));
            }
            else
            {
                if(Pixels[Pixels.Count-1].LyTo < ly_to)
                    Pixels.Add(new PlainSpectrFramePixel(ly_from, ly_to, val));
            }
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);
            bw.Write(Time);
            bw.Write(Pixels.Count);
            foreach (PlainSpectrFramePixel p in Pixels)
                p.Save(bw);
        }

        public PlainSpectrFrame(BinaryReader br)
        {
            if (br.ReadInt32() != 1)
                throw new Exception("Wrong version of PlainSpectrFrame");
            Time = br.ReadSingle();
            int n = br.ReadInt32();
            for (int i = 0; i < n; i++)
                Pixels.Add(new PlainSpectrFramePixel(br));
        }
    }

    public class PlainSpectrFramePixel
    {
        public float LyFrom, LyTo, Value;
        public bool Corrected;
        public PlainSpectrFramePixel(float ly_from, float ly_to, float val)
        {
            LyFrom = ly_from;
            LyTo = ly_to;
            Value = val;
            Corrected = (val == float.MaxValue);
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)1);
            bw.Write(LyFrom);
            bw.Write(LyTo);
            bw.Write(Value);
            bw.Write(Corrected);
        }

        public PlainSpectrFramePixel(BinaryReader br)
        {
            if (br.ReadByte() != 1)
                throw new Exception("Wrong version of PlainSpectrPixel");
            LyFrom = br.ReadSingle();
            LyTo = br.ReadSingle();
            Value = br.ReadSingle();
            Corrected = br.ReadBoolean();
        }
    }
}
