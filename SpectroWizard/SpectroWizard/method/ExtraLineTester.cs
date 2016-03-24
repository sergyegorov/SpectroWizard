using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;
using SpectroWizard.gui.comp;
using System.IO;

namespace SpectroWizard.method
{
    public partial class ExtraLineTester : UserControl
    {
        public ExtraLineTester()
        {
            InitializeComponent();
        }

        MethodSimple Method;
        SpectrView SpView;
        public void Setup(SpectrView view)
        {
            SpView = view;
        }

        public void Setup(MethodSimple method)
        {
            Method = method;
        }

        public List<ExtraLineInfo> CheckSpectr(Spectr sp)
        {
            List<ExtraLineInfo> ret = new List<ExtraLineInfo>();
            if (EmptySpaces.Count == 0)
                return ret;

            List<SpectrDataView> views = sp.GetViewsSet();

            int[] active_view_indexes = sp.GetShotIndexes();
            int[] ss = sp.GetCommonDispers().GetSensorSizes();

            for (int esi = 0; esi < EmptySpaces.Count; esi++)
            {

                for (int vi = 0; vi < active_view_indexes.Length; vi++)
                {
                    SpectrDataView view = views[active_view_indexes[vi]];
                    SpectrDataView nul = sp.GetNullFor(active_view_indexes[vi]);
                    Dispers d = sp.GetCommonDispers();

                    List<int> sns = d.FindSensors(EmptySpaces[esi][1]);
                    if (sns.Count == 0)
                        continue;

                    int sn = sns[0];
                    int pixel_from = (int)d.GetLocalPixelByLy(sn,EmptySpaces[esi][1])-(int)nmMargine.Value;
                    if (pixel_from < 0)
                        pixel_from = 0;
                    int pixel_to = (int)d.GetLocalPixelByLy(sn, EmptySpaces[esi][2]) + (int)nmMargine.Value;
                    if (pixel_to >= ss[sn])
                        pixel_to = ss[sn] - 1;

                    float[] data_s = view.GetSensorData(sn);//.GetFullData()[sn];
                    float[] data_n = nul.GetSensorData(sn);//.GetFullData()[sn];
                    float[] data = new float[pixel_to - pixel_from];
                    for (int i = 0; i < data.Length; i++)
                        data[i] = data_s[pixel_from + i] - data_n[pixel_from + i];
                    int side = (int)nmMinLineWidth.Value/2;
                    for (int i = (int)nmMinLineWidth.Value; i < data.Length - (int)nmMinLineWidth.Value; i++)
                    {
                        while (i < data.Length-5 && data[i] < data[i + 1])
                            i++;
                        int left = 0;
                        int right = 0;
                        if (isLine(data, data.Length - 2, i, view.MaxLinarLevel, side, side * 2 + 2, ref left, ref right))
                        {
                            int start_pixel = i - 2;
                            int final_pixel = i + 2;
                            for (; start_pixel > 0 && data[start_pixel] < data[start_pixel + 1]; start_pixel--) ;
                            for (; final_pixel < data.Length - 1 && data[final_pixel] < data[final_pixel - 1]; final_pixel++) ;
                            if (data[i] - (data[start_pixel]+data[final_pixel])/2 > (int)nmMinAmpl.Value)
                                ret.Add(new ExtraLineInfo(sn, i + pixel_from, d, (data[i] + data[i + 1] + data[i - 1]) / 3));
                            i += right;
                        }
                    }
                }
            }
            return ret;
        }

        public class ExtraLineInfo
        {
            public float Ly;
            public float Ampl;
            public ExtraLineInfo(int sn,int pixel, Dispers disp,float ampl)
            {
                Ampl = ampl;
                Ly = (float)disp.GetLyByLocalPixel(sn, pixel);
            }

            public ExtraLineInfo(BinaryReader br)
            {
                byte ver = br.ReadByte();
                if (ver != 0)
                    throw new Exception("Wrong version of ExtraLineInfo.");
                Ly = br.ReadSingle();
                Ampl = br.ReadSingle();
            }

            public void Save(BinaryWriter bw)
            {
                bw.Write((byte)0);
                bw.Write(Ly);
                bw.Write(Ampl);
            }
        }

        #region Save/Load
        public void LoadTech(BinaryReader br,MethodSimple method)
        {
            Method = method;
            int ver = br.ReadInt32();
            if (ver != 1)
                throw new Exception("Wrong version of the ExtraLineTester");
            nmMargine.Value = br.ReadDecimal();
            nmMinFreeSpaceSize.Value = br.ReadDecimal();
            nmMinLineWidth.Value = br.ReadDecimal();
            nmMinAmpl.Value = br.ReadDecimal();
            
            int count = br.ReadInt32();
            EmptySpaces.Clear();
            for (int i = 0; i < count; i++)
            {
                int len = br.ReadInt16();
                float[] data = new float[len];
                for (int j = 0; j < len; j++)
                    data[j] = br.ReadSingle();
                EmptySpaces.Add(data);
            }
            
            count = br.ReadInt32();
            ExceptionLy.Clear();
            for (int i = 0; i < count; i++)
                ExceptionLy.Add(br.ReadSingle());

            ver = br.ReadInt32();
            if (ver != 234853)
                throw new Exception("Wrong finish of the ExtraLineTester");
            ReInitList();
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(1);
            bw.Write(nmMargine.Value);
            bw.Write(nmMinFreeSpaceSize.Value);
            bw.Write(nmMinLineWidth.Value);
            bw.Write(nmMinAmpl.Value);
            
            bw.Write(EmptySpaces.Count);
            foreach (float[] li in EmptySpaces)
            {
                bw.Write((short)li.Length);
                for (int i = 0; i < li.Length;i++ )
                    bw.Write(li[i]);
            }
            
            bw.Write(ExceptionLy.Count);
            foreach (float li in ExceptionLy)
                bw.Write(li);

            bw.Write(234853);
        }
        #endregion

        void ReInitList()
        {
            for (int i = 0; i < EmptySpaces.Count; i++)
            {
                string val = (i+1)+". "+serv.GetGoodValue(EmptySpaces[i][1],1) + "..." + serv.GetGoodValue(EmptySpaces[i][2],1) + " " + (int)EmptySpaces[i][3];
                if (i >= lbFreeSpaces.Items.Count)
                    lbFreeSpaces.Items.Add(val);
                else
                    lbFreeSpaces.Items[i] = val;
            }
            if (EmptySpaces.Count >= lbFreeSpaces.SelectedIndex)
                lbFreeSpaces.SelectedIndex = EmptySpaces.Count - 1;
            while (EmptySpaces.Count < lbFreeSpaces.Items.Count)
                lbFreeSpaces.Items.RemoveAt(lbFreeSpaces.Items.Count - 1);
        }

        bool isLine(float[] data, int len, int pix, float max_level,int side_size,int sum_size,
            ref int left,ref int right)
        {
            left = 1;
            right = 1;
            if (data[pix] > max_level)
            {
                int lpix = pix - 1;
                while (lpix > 0 && data[lpix] < data[lpix + 1])
                    lpix--;
                while (pix < len && data[pix] > max_level)
                    pix++;
                int rpix = pix;
                while (rpix < len && data[rpix - 1] > data[rpix])
                    rpix++;
                left = pix - lpix;
                right = rpix - pix;
                return true;
            }
            else
            {
                if (data[pix] >= data[pix + 1] &&
                    data[pix] > data[pix - 1])
                {
                    int p = pix + 1;
                    while (p < len - 1 && data[p] > data[p + 1])
                    {
                        p++;
                        right++;
                    }
                    p = pix - 1;
                    while (p > 1 && data[p] > data[p - 1])
                    {
                        p--;
                        left++;
                    }
                    if (left > side_size && right > side_size &&
                    left + right > sum_size)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        List<float[]> EmptySpaces = new List<float[]>();
        List<float> ExceptionLy = new List<float>();
        private void btCallectSpaces_Click(object sender, EventArgs e)
        {
            try
            {
                EmptySpaces.Clear();
                //ExceptionLy.Clear();
                Dispers d = null;
                int pr_count = Method.GetProbCount();
                int[] ss = Common.Dev.Reg.GetSensorSizes();
                int[][] founds = new int[ss.Length][];
                for (int i = 0; i < ss.Length; i++)
                    founds[i] = new int[ss[i]];
                int side = (int)nmMinLineWidth.Value / 2;
                for (int pr = 0; pr < pr_count; pr++)
                {
                    MethodSimpleProb msp = Method.GetProbHeader(pr);
                    int sp_count = msp.MeasuredSpectrs.Count;
                    for (int sp = 0; sp < sp_count; sp++)
                    {
                        MethodSimpleProbMeasuring mspm = msp.MeasuredSpectrs[sp];
                        Spectr spectr = mspm.Sp;
                        if(spectr.IsEmpty())
                            continue;
                        int[] view = mspm.Sp.GetShotIndexes();
                        List<SpectrDataView> views = mspm.Sp.GetViewsSet();
                        int sum = side * 2 + 2;
                        for (int vi = 0; vi < view.Length; vi++)
                        {
                            SpectrDataView cur_view = views[view[vi]];
                            float max_level = cur_view.MaxLinarLevel;
                            SpectrDataView nul_view = mspm.Sp.GetNullFor(view[vi]);
                            d = spectr.GetCommonDispers();//cur_view.GetDispersRO();
                            for (int sn = 0; sn < ss.Length; sn++)
                            {
                                float[] sig = cur_view.GetSensorData(sn);
                                float[] nul = nul_view.GetSensorData(sn);
                                float[] data = new float[sig.Length];
                                for (int pix = 0; pix < data.Length; pix++)
                                    data[pix] = sig[pix] - nul[pix];
                                int len = ss[sn] - 10;
                                float min = 0;
                                for (int pix = 0; pix < 10; pix++)
                                    min += data[pix];
                                for (int pix = 0; pix < len; pix++)
                                {
                                    float tmp = 0;
                                    for(int j = pix; j < pix+10;j++)
                                        tmp += data[j];
                                    if (tmp < min)
                                        min = tmp;
                                }
                                len = ss[sn] - side-3;
                                min /= 10;
                                min *= 3;
                                int left = 0;
                                int right = 0;
                                for (int pix = side+3; pix < len; pix++)
                                {
                                    if (isLine(data, len + side + 1, pix, max_level, side, sum, ref left, ref right))
                                    {
                                        for (int i = pix - left; i <= pix + right; i++)
                                            founds[sn][i]++;
                                        pix += right;
                                    }
                                }
                            }
                        }
                    }
                }

                for (int sn = 0; sn < ss.Length; sn++)
                {
                    int margin_before = (int)nmMargine.Value;
                    int len = 0;
                    int pix_from = 0;
                    for (int pix = 0; pix < ss[sn]; pix++)
                    {
                        if (founds[sn][pix] <= 1 && pix < ss[sn]-1)
                        {
                            if (margin_before > 0)
                            {
                                margin_before--;
                                pix_from = pix;
                            }
                            else
                                len++;
                        }
                        else
                        {
                            len -= (int)nmMargine.Value;
                            if (len >= nmMinFreeSpaceSize.Value)
                            {
                                float[] es = {1,(float)d.GetLyByLocalPixel(sn,pix_from),
                                                  (float)d.GetLyByLocalPixel(sn,pix-(int)nmMargine.Value),len};
                                EmptySpaces.Add(es);
                            }
                            margin_before = (int)nmMargine.Value;
                            len = 0;
                        }
                    }
                }

                ReInitList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        const string MLSConst = "ExtLnTester";
        private void btClear_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(serv.FindParentForm(this),
                    Common.MLS.Get(MLSConst,"Удалить все найденные привязки?"),
                    Common.MLS.Get(MLSConst,"Удаление..."),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                EmptySpaces.Clear();
                ReInitList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void lbFreeSpaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                if (SpView == null || lbFreeSpaces.SelectedIndex < 0)
                    return;

                SpView.ClearAnalitMarkers();
                SpView.AddAnalitMarker((float)EmptySpaces[lbFreeSpaces.SelectedIndex][1], "От...",
                    Color.Green, false);
                SpView.AddAnalitMarker((float)EmptySpaces[lbFreeSpaces.SelectedIndex][2], "...до",
                    Color.Green, false);
                SpView.ZoomAnalitMarkers(-1);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
