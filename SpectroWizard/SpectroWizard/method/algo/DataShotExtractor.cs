using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpectroWizard.data;

namespace SpectroWizard.method.algo
{
    public class DataShotExtractor
    {
        static public List<DataShot> extract(MethodSimple method,string element,int formula,
            double ly,int widthPlusMinus,bool useConDlt,
            double min,double max,bool relative)
        {
            int mul_k = (int)Common.Conf.MultFactor;
            int is_ok = 0;
            int is_over = 0;
            List<DataShot> ret = new List<DataShot>();
            Element[] list = method.GetElementList();
            for(int el = 0;el<list.Length;el++)
                if(list[el].Name.Equals(element) || element == null)
                {
                    MethodSimpleElementFormula calcFormula = method.GetElHeader(el).Formula[formula];
                    bool[] frames = calcFormula.Formula.GetUsedFrames();
                    for (int prob_index = 0; prob_index < method.GetProbCount(); prob_index++)
                    {
                        MethodSimpleProb prob = method.GetProbHeader(prob_index);
                        MethodSimpleCell msc = method.GetCell(el, prob_index);
                        if (useConDlt == false)
                        {
                            double fkVal = msc.Con;
                            double con = fkVal;
                            /*if (useConDlt)
                            {
                                double sko, sko1;
                                double rcon = //msc.CalcRealCon(out sko, out sko1);
                                fkVal -= rcon;
                            }//*/
                            for (int measuring_index = 0; measuring_index < prob.MeasuredSpectrs.Count; measuring_index++)
                            {
                                MethodSimpleCellFormulaResult mscfr = msc.GetData(measuring_index,formula);
                                if(mscfr.Enabled == false)
                                    continue;
                                MethodSimpleProbMeasuring mspm = prob.MeasuredSpectrs[measuring_index];
                                Spectr sp = mspm.Sp;
                                if (sp == null)
                                    continue;
                                List<SpectrDataView> viewSet = sp.GetViewsSet();
                                int[] shotIndexes = sp.GetShotIndexes();
                                Dispers disp = sp.GetCommonDispers();
                                List<int> sensors = disp.FindSensors(ly);
                                bool isEnabled;
                                if (con >= 0)
                                    isEnabled = msc.Enabled;
                                else
                                    isEnabled = false;
                                for (int shot_index = 0; shot_index < shotIndexes.Length; shot_index++)
                                {
                                    if (frames[shot_index] == false)
                                        continue;
                                    SpectrDataView sig = viewSet[shotIndexes[shot_index]];
                                    SpectrDataView nul = sp.GetNullFor(shotIndexes[shot_index]);
                                    for (int sn = 0; sn < 1 && sn < sensors.Count; sn++)
                                    {
                                        int sensorIndex = sensors[sn];
                                        int n = (int)disp.GetLocalPixelByLy(sensorIndex, ly);
                                        float[] sigData = sig.GetSensorData(sensorIndex);
                                        float[] nulData = nul.GetSensorData(sensorIndex);
                                        float minSignal = float.MaxValue;
                                        float[] signal = new float[sigData.Length];
                                        for (int i = 0; i < signal.Length; i++)
                                            signal[i] = sigData[i] - nulData[i];
                                        for (int i = 500; i < sigData.Length - 500; i++)
                                        {
                                            float val = (signal[i - 1] + signal[i] + signal[i + 1]) / 3;
                                            if (val < minSignal)
                                                minSignal = val;
                                        }

                                        float[] data = new float[widthPlusMinus * 2 + 1];
                                        double maxSignal = -double.MaxValue;
                                        for (int i = 0; i < data.Length; i++)
                                        {
                                            int index = n - widthPlusMinus + i;
                                            if (index < 0 || index >= sigData.Length)
                                            {
                                                data[i] = -float.MaxValue;
                                                isEnabled = false;
                                                continue;
                                            }
                                            data[i] = signal[index];//sigData[index] - nulData[index];
                                            if (data[i] > max)
                                                isEnabled = false;
                                            if (data[i] > maxSignal && i > widthPlusMinus-4 && i < widthPlusMinus+4)
                                                maxSignal = data[i];
                                        }
                                        if (maxSignal < min)
                                            isEnabled = false;
                                        if (isEnabled)
                                            is_ok++;
                                        else
                                            is_over++;
                                        DataShot dsh = new DataShot(ly, fkVal, data, isEnabled);
                                        ret.Add(dsh);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int measuring_index = 0; measuring_index < prob.MeasuredSpectrs.Count; measuring_index++)
                            {
                                MethodSimpleProbMeasuring mspm = prob.MeasuredSpectrs[measuring_index];
                                Spectr sp = mspm.Sp;
                                if (sp == null)
                                    continue;
                                List<SpectrDataView> viewSet = sp.GetViewsSet();
                                int[] shotIndexes = sp.GetShotIndexes();
                                Dispers disp = sp.GetCommonDispers();
                                List<int> sensors = disp.FindSensors(ly);
                                bool isEnabled;
                                if (msc.Con >= 0)
                                    isEnabled = msc.Enabled;
                                else
                                    isEnabled = false;
                                MethodSimpleCellFormulaResult result = msc.GetData(measuring_index, formula);
                                int data_index = 0;
                                for (int shot_index = 0; shot_index < shotIndexes.Length; shot_index++)
                                {
                                    if (frames[shot_index] == false)
                                        continue;
                                    double tmpAnalit = result.AnalitValue[data_index];
                                    double fkVal;
                                    if (relative == false)
                                        fkVal = calcFormula.Formula.CalcCon(0, tmpAnalit, 0) - msc.Con;
                                    else
                                    {
                                        if(msc.Con > 0.01)
                                            fkVal = (calcFormula.Formula.CalcCon(0, tmpAnalit, 0) - msc.Con) / msc.Con;
                                        else
                                            fkVal = Double.NaN;
                                    }
                                    SpectrDataView sig = viewSet[shotIndexes[shot_index]];
                                    SpectrDataView nul = sp.GetNullFor(shotIndexes[shot_index]);
                                    for (int sn = 0; sn < sensors.Count; sn++)
                                    {
                                        int sensorIndex = sensors[sn];
                                        int n = (int)disp.GetLocalPixelByLy(sensorIndex, ly);
                                        float[] sigData = sig.GetSensorData(sensorIndex);
                                        float[] nulData = nul.GetSensorData(sensorIndex);

                                        float[] data = new float[widthPlusMinus * 2 + 1];
                                        for (int i = 0; i < data.Length; i++)
                                        {
                                            int index = n - widthPlusMinus + i;
                                            if (index < 0 || index >= sigData.Length)
                                            {
                                                data[i] = -float.MaxValue;
                                                isEnabled = false;
                                                continue;
                                            }
                                            data[i] = sigData[index] - nulData[index];
                                            if (data[i] > max)
                                                isEnabled = false;
                                        }
                                        if (isEnabled)
                                            is_ok++;
                                        else
                                            is_over++;
                                        DataShot dsh = new DataShot(ly, fkVal, data, isEnabled);
                                        ret.Add(dsh);
                                    }
                                    data_index++;
                                }
                            }
                        }
                    }
                    break;
                }
            if (is_ok == 0 || is_over / is_ok > 0.1)
                return null;
            return ret;
        }
    }
}
