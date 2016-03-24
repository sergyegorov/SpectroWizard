using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SpectroWizard.data;

namespace SpectroWizard.dev.devices
{
    public class USBConDev : Dev
    {
        public USBConDev(DevReg reg, DevGen gen,
            DevFillLight fill,
            DevGas gas) :
            base(reg, gen,fill,gas)
        {
        }

        public void CollectDataMiner(List<SpectrDataView> data, SpectrCondition cond)
        {
            float max_level = Common.Conf.MaxLevel;
            int[] line_sizes = Reg.GetSensorSizes();
            int[] orientations = Common.Conf.USBOrientationVals;
            int line_size;
            if (line_sizes[0] > 3048)
                line_size = 4096;
            else
                line_size = 2048;
            for (int step = 0; step < cond.Lines.Count; step++)
            {
                if (cond.Lines[step].Type == SpectrCondition.CondTypes.Exposition)
                {
                    /*byte[] bbuffer = new byte[4096 * 2 * 15];
                    short[] bbuffer_sub_count = new short[4096 * 2 * 15];
                    FileStream fs = new FileStream("data" + cond.Lines[step].TmpInteger, FileMode.Open, FileAccess.Read);
                    int size = fs.Read(bbuffer, 0, bbuffer.Length);
                    fs.Close();

                    short[] sbuffer = new short[size / 2];
                    for (int i = 0; i < size; i += 2)
                    {
                        short val = (short)(bbuffer[i + 1] & 0xFF);
                        if (val > 0xF)
                            throw new Exception("Val is too big");
                        val <<= 8;
                        val |= (short)(bbuffer[i] & 0xFF);
                        sbuffer[i / 2] = val;
                    }*/

                    FileStream fs = new FileStream("data" + cond.Lines[step].TmpInteger, FileMode.Open, FileAccess.Read);
                    long[] sbuffer = new long[15 * line_size];
                    short[] sbuffer_counter = new short[sbuffer.Length];
                    short[] sbuffer_fill_counter = new short[15];
                    bool[] sbuffer_fill_flag = new bool[15];
                    byte[] bbuffer = new byte[line_size*2];
                    do
                    {
                        int size = fs.Read(bbuffer, 0, bbuffer.Length);
                        if (size == 0)
                            break;
                        if (size != bbuffer.Length)
                            throw new Exception("Can't load full data...");
                        for (int i = 0; i < bbuffer.Length; i+=2)
                        {
                            short val = (short)(bbuffer[i + 1] & 0xF);
                            val <<= 8;
                            val |= (short)(bbuffer[i] & 0xFF);
                            if (val < 0)
                                throw new Exception("Negative value");
                            int sn = (bbuffer[i + 1] >> 4);
                            sbuffer_fill_flag[sn] = true;
                            int index = line_size*sn+sbuffer_fill_counter[sn];
                            sbuffer[index] += val;
                            if (sbuffer[index] < 0)
                                throw new Exception("Negative value");
                            sbuffer_counter[index]++;
                            sbuffer_fill_counter[sn]++;
                            if (sbuffer_fill_counter[sn] >= line_size)
                                sbuffer_fill_counter[sn] = 0;
                        }
                    } while (true);
                    fs.Close();
                    for (int i = 0; i < sbuffer.Length; i++)
                        if (sbuffer_counter[i] > 0)
                            sbuffer[i] /= sbuffer_counter[i];
                        else
                            sbuffer[i] = -1;
                    int sbuffer_shift = 0;
                    for (int s = 0; s < 15; s++)
                    {
                        if (sbuffer_fill_flag[s] == false)
                            sbuffer_shift += line_size;
                        else
                        {
                            if(sbuffer_shift == 0)
                                continue;
                            int from = s * line_size;
                            int to = from-sbuffer_shift;
                            for (int i = 0; i < line_size; i++)
                                sbuffer[to + i] = sbuffer[from + i];
                        }
                    }

                    //if (size < 4096 * line_sizes.Length)
                    //    throw new Exception("No data for expoistion " + step + "-" + cond.Lines[step].TmpInteger);

                    float[][] line_data = new float[line_sizes.Length][];
                    short[][] bb = new short[line_sizes.Length][];
                    short[][] be = new short[line_sizes.Length][];
                    int[] index_in_data = new int[line_sizes.Length];
                    for (int i = 0; i < orientations.Length; i++)
                        index_in_data[i] = (int)Math.Abs(orientations[i]) - 1;
                    for (int i = 0; i < index_in_data.Length; i++)
                    {
                        bool found = false;
                        for (int j = 1; j < index_in_data.Length; j++)
                        {
                            if (index_in_data[j - 1] > index_in_data[j])
                            {
                                int tmp = index_in_data[j - 1];
                                index_in_data[j - 1] = index_in_data[j];
                                index_in_data[j] = tmp;
                                found = true;
                            }
                        }
                        if (found == false)
                            break;
                    }
                    for (int li = 0; li < line_sizes.Length; li++)
                    {
                        int l = 0;
                        int or = (int)Math.Abs(orientations[li]) - 1;
                        for (int i = 0; i < orientations.Length; i++)
                            if (or == index_in_data[i])
                            {
                                l = i;
                                break;
                            }
                        int orient = orientations[li];
                        int index;
                        int beg_blank_size, end_blank_size;
                        if (orient < 0)
                        {
                            beg_blank_size = Common.Conf.BlakPixelEnd;
                            end_blank_size = Common.Conf.BlakPixelStart;
                        }
                        else
                        {
                            end_blank_size = Common.Conf.BlakPixelEnd;
                            beg_blank_size = Common.Conf.BlakPixelStart;
                        }
                        index = l;
                        int pos = index * 4096;
                        short[] rez = new short[4096];
                        for (int i = 0; i < 4096; i++)
                        {
                            short val = (short)sbuffer[pos + i];
                            if (orient > 0)
                                rez[i] = val;//(short)sbuffer[pos + i];
                            else
                                rez[4095 - i] = val;//(short)sbuffer[pos + i];
                        }
                        int base_level = 0, bl = 0;
                        for (int i = 0; i < 15; i++, bl++)
                            base_level += (short)sbuffer[pos + i];
                        base_level /= bl;
                        int blank_level = 0;
                        bl = 0;
                        for (int i = 17; i < 28; i++, bl++)
                            blank_level += (short)sbuffer[pos + i];
                        blank_level /= bl;
                        int ever = blank_level - base_level + 10;
                        line_data[li] = new float[4096 - end_blank_size - beg_blank_size];
                        bb[li] = new short[beg_blank_size];
                        be[li] = new short[end_blank_size];
                        int ind;
                        //int collect_to;
                        //if(beg_blank_size
                        for (ind = 0; ind < beg_blank_size; ind++)
                            bb[li][ind] = rez[ind];
                        int index_from = ind;
                        for (; ind < 4096 - end_blank_size; ind++)
                            line_data[li][ind - index_from] = rez[ind];
                        index_from = ind;
                        for (; ind < 4096; ind++)
                            be[li][ind - index_from] = rez[ind];
                        if (Common.Conf.BlankSub == false)
                            ever = 0;
                        for (int i = 0; i < line_data[li].Length; i++)
                        {
                            if (line_data[li][i] > max_level)
                                line_data[li][i] = max_level + 1;
                            else
                                line_data[li][i] = line_data[li][i] - ever;//*/
                            if (line_data[li][i] < 0)
                                throw new Exception("Negative value...");
                        }
                    }
                    SpectrDataView cur_spview = new SpectrDataView(new SpectrCondition(Tick, cond.Lines[step]), line_data, bb, be,
                                            Common.Dev.Reg.GetMaxValue(),
                                            Common.Dev.Reg.GetMaxLinarValue());
                    data.Add(cur_spview);
                }
            }
        }

        public void CollectData(List<SpectrDataView> data, SpectrCondition cond)
        {
            float max_level = Common.Conf.MaxLevel;
            byte[] bbuffer = new byte[4096*2*15];
            int[] line_sizes = Reg.GetSensorSizes();
            int[] orientations = Common.Conf.USBOrientationVals;
            for (int step = 0; step < cond.Lines.Count; step++)
            {
                if (cond.Lines[step].Type == SpectrCondition.CondTypes.Exposition)
                {
                    FileStream fs = new FileStream("data" + cond.Lines[step].TmpInteger, FileMode.Open,FileAccess.Read);
                    int size = fs.Read(bbuffer, 0, bbuffer.Length);
                    fs.Close();

                    short[] sbuffer = new short[size/2];
                    for(int i = 0;i<size;i+=2){
                        short val = (short)(bbuffer[i + 1]&0xFF);
                        if (val > 0xF)
                            throw new Exception("Val is too big");
                        val <<= 8;
                        val |= (short)(bbuffer[i] & 0xFF);
                        sbuffer[i/2] = val;
                    }

                    /*int ever = 0;
                    for(int i = 0;i<4096;i++)
                        ever += sbuffer[i];
                    ever /= 4096;//*/

                    if (size < 4096 * line_sizes.Length)
                        throw new Exception("No data for expoistion " + step + "-" + cond.Lines[step].TmpInteger);

                    float[][] line_data = new float[line_sizes.Length][];
                    short[][] bb = new short[line_sizes.Length][];
                    short[][] be = new short[line_sizes.Length][];
                    int[] index_in_data = new int[line_sizes.Length];
                    for (int i = 0; i < orientations.Length; i++)
                        index_in_data[i] = (int)Math.Abs(orientations[i])-1;
                    for (int i = 0; i < index_in_data.Length; i++)
                    {
                        bool found = false;
                        for (int j = 1; j < index_in_data.Length; j++)
                        {
                            if (index_in_data[j - 1] > index_in_data[j])
                            {
                                int tmp = index_in_data[j - 1];
                                index_in_data[j-1] = index_in_data[j];
                                index_in_data[j] = tmp;
                                found = true;
                            }
                        }
                        if (found == false)
                            break;
                    }
                    for (int li = 0; li < line_sizes.Length; li++)
                    {
                        int l = 0;
                        int or = (int)Math.Abs(orientations[li]) - 1;
                        for(int i = 0;i<orientations.Length;i++)
                            if (or == index_in_data[i])
                            {
                                l = i;
                                break;
                            }
                        int orient = orientations[li];
                        int index;
                        int beg_blank_size, end_blank_size;
                        if (orient < 0)
                        {
                            //index = -orient;
                            beg_blank_size = Common.Conf.BlakPixelEnd;
                            end_blank_size = Common.Conf.BlakPixelStart;
                        }
                        else
                        {
                            //index = orient;
                            end_blank_size = Common.Conf.BlakPixelEnd;
                            beg_blank_size = Common.Conf.BlakPixelStart;
                        }
                        index = l;
                        //index--;
                        int pos = index * 4096;
                        short[] rez = new short[4096];
                        for (int i = 0; i < 4096; i ++)
                        {
                            //short val = (short)(buffer[pos+i+1]&0xFF);
                            //val <<= 8;
                            //val |= (short)((buffer[pos+i])&0xFF);
                            if (orient > 0)
                                rez[i] = sbuffer[pos+i];
                            else
                                rez[4095 - i] = sbuffer[pos + i];
                        }
                        int base_level = 0,bl = 0;
                        for (int i = 0; i < 15; i++,bl++)
                            base_level += sbuffer[pos + i];
                        base_level /= bl;
                        int blank_level = 0;
                        bl = 0;
                        for (int i = 17; i < 28; i++,bl ++)
                            blank_level += sbuffer[pos + i];
                        blank_level /= bl;
                        int ever = blank_level - base_level + 10;
                        line_data[li] = new float[4096 - end_blank_size - beg_blank_size];
                        bb[li] = new short[beg_blank_size];
                        be[li] = new short[end_blank_size];
                        int ind;
                        //int collect_to;
                        //if(beg_blank_size
                        for (ind = 0; ind < beg_blank_size; ind++)
                            bb[li][ind] = rez[ind];
                        int index_from = ind;
                        for (; ind < 4096 - end_blank_size; ind++)
                            line_data[li][ind - index_from] = rez[ind];
                        index_from = ind;
                        for (; ind < 4096; ind++)
                            be[li][ind-index_from] = rez[ind];
                        /*short[] to_sum;
                        int to_sum_from = 0;
                        if (be[li].Length < 100)//> 400)
                        {
                            to_sum = be[li];
                            to_sum_from = to_sum.Length - 18;//50;
                        }
                        else
                        {
                            to_sum = bb[li];
                            to_sum_from = 0;
                        }
                        int ever = 0;
                        /*for (int i = 0; i < 18; i++)
                            ever += to_sum[i + to_sum_from];
                        ever /= 18;*/
                        if (Common.Conf.BlankSub == false)
                            ever = 0;
                        for (int i = 0; i < line_data[li].Length; i++)
                        {
                            if (line_data[li][i] > max_level)
                                line_data[li][i] = max_level + 1;
                            else
                                line_data[li][i] = line_data[li][i] - ever;//*/
                        }
                    }
                    SpectrDataView cur_spview = new SpectrDataView(new SpectrCondition(Tick, cond.Lines[step]), line_data, bb, be,
                                            Common.Dev.Reg.GetMaxValue(),
                                            Common.Dev.Reg.GetMaxLinarValue());
                    data.Add(cur_spview);
                }
            }
        }

        override public string DefaultDipsers()
        {
            string ret = "#default dispers" + serv.Endl +
                    "" + serv.Endl;
            int[] line_sizes = Reg.GetSensorSizes();
            int base_pixel = 0;
            for (int i = 0; i < line_sizes.Length; i++)
            {
                int from = base_pixel;
                int to = base_pixel + line_sizes[i] - 1;
                ret += "s" + (i + 1) + ":1" + serv.Endl +
                "" + from + "-" + from + serv.Endl +
                "" + to + "-" + to + serv.Endl +
                "" + serv.Endl;
                base_pixel += line_sizes[i];
            }
            return ret;
        }

        override public bool IsUSBConsole()
        {
            return true;
        }

        public override void BeforeMeasuring()
        {

        }

        public override void AfterMeasuring()
        {
        }

        public override string GetName()
        {
            return "USB Console MultiLine Device";
        }
    }

    public class USBConRegDev : DevReg
    {
        int LineCount;
        public USBConRegDev(int line_number)
            : base(4)
        {
            LineCount = line_number;
        }

        override protected short[][] RegFrameProc(int common, int[] exps, out short[][] blank_start, out short[][] blank_end)
        {
            throw new NotImplementedException("short[][] RegFrameProc(int common, int[] exps, out short[][] blank_start, out short[][] blank_end)");
        }

        int[] LineSizes = null;
        override public int[] GetSensorSizes()
        {
            if (LineSizes == null)
            {
                LineSizes = new int[LineCount];
                for (int i = 0; i < LineSizes.Length; i++)
                    LineSizes[i] = 4096 - Common.Conf.BlakPixelEnd - Common.Conf.BlakPixelStart;
            }
            return LineSizes;
        }

        override public void Connect()
        {
        }

        override public void Disconnect()
        {
        }

        override public string GetName()
        {
            return "USBCon" + LineCount + "x4096";
        }

        override public bool IsConnected()
        {
            return true;
        }

        public override short GetMaxLinarValue()
        {
            return (short)(Common.Conf.MaxLevel * 0.9);
        }

        public override short GetMaxValue()
        {
            return (short)Common.Conf.MaxLevel;
        }

        override public string Test() { return null; }
        override public string TimeConstTest(int divider, out float val) { val = (float)(0.00876*Math.Pow(2,divider)); return null; }
        override public string MaxLevelTest(out float level) { level = 4096F; return null; }
        override public string NoiseTest(int exp) { return null; }
        override public void Reset()
        {
        }
    }

    public class USBConGen : DevGen
    {
        public USBConGen()
            : base(5)
        {
        }

        override protected void SetStatusProc(bool on_off)
        {
        }

        override public void Connect()
        {
        }

        override public void Disconnect()
        {
        }

        override public string GetName()
        {
            return "USBCon Generator";
        }

        override public bool IsConnected()
        {
            return true;
        }

        override public string Test()
        {
            return "";
        }
    }

    public class USBConFillLight : DevFillLight
    {
        public USBConFillLight()
            : base()
        {
        }

        override public void SetFillLight(bool on_off)
        {
        }

        override public bool Has()
        {
            return false;
        }

        override public string GetName()
        {
            return "USBCon Fill Light";
        }

        override public string Test()
        {
            return "";
        }

        override public string Test(bool status)
        {
            return "";
        }
    }
}
