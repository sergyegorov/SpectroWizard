using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.dev.devices;

namespace SpectroWizard.dev
{
    public class KnownDevList
    {
        public static DevGen[] GenList = {
                                new DebugGen(),
                                new PromGen(),
                                new SparkGen(),
                                new HandGen(),
                                new USBConGen()
                            };

        public static DevReg[] RegList = {
                                new DebugReg(),
                                //---- 1
                                new MLRegDev(1,2048),
                                new MLRegDev(2,2048),
                                new MLRegDev(3,2048),
                                new MLRegDev(4,2048),
                                new MLRegDev(5,2048),
                                new MLRegDev(6,2048),
                                new MLRegDev(7,2048),
                                new MLRegDev(8,2048),
                                new MLRegDev(1,3650),
                                new MLRegDev(2,3650),
                                new MLRegDev(3,3650),
                                new MLRegDev(4,3650),
                                new MLRegDev(5,3650),
                                new MLRegDev(6,3650),
                                new MLRegDev(7,3650),
                                new MLRegDev(8,3650),
                                new MLRegDev(9,3650),
                                new MLRegDev(10,3650),
                                //---- 19
                                new USBConRegDev(1),
                                new USBConRegDev(2),
                                new USBConRegDev(3),
                                new USBConRegDev(4),
                                new USBConRegDev(5),
                                new USBConRegDev(6),
                                new USBConRegDev(7),
                                new USBConRegDev(8),
                                new USBConRegDev(9),
                                new USBConRegDev(10),
                                new USBConRegDev(11),
                                new USBConRegDev(12),
                                new USBConRegDev(13),
                                new USBConRegDev(14),
                                new USBConRegDev(15)
                            };

        public static DevFillLight[] DevFillLightList = 
        {
            new DevDebugFillLight(),
            new DevFillLightNull(),
            new DevFillLightHContr(),
            new MLFillLight(1),
            new MLFillLight(2),
            new MLFillLight(4),
            new MLFillLight(8),
            new MLFillLight(16),
            new MLFillLight(32),
            new MLFillLight(64),
            new MLFillLight(128),
            new MLFillLight(255),
            new USBConFillLight()
        };

        public static DevGas[] DevGasList = 
        {
            new DevGasNull()
        };

        static public Dev GetDev(uint reg_id,uint gen_id,
            uint fill_light_id,uint gas_id)
        {
            Dev ret;
            if (RegList[reg_id] is DebugReg)
                ret = new DeviceDebug();
            else
            {
                if (RegList[reg_id] is MLRegDev)
                    return new MLDevice(RegList[reg_id], GenList[gen_id],
                        DevFillLightList[fill_light_id],
                        DevGasList[gas_id]);
                else
                {
                    if(RegList[reg_id] is USBConRegDev)
                        return new USBConDev(RegList[reg_id], GenList[gen_id],
                        DevFillLightList[fill_light_id],
                        DevGasList[gas_id]);
                    else
                        throw new Exception("Not supported registrator id");
                }
            }
            return ret;
        }

        static public void InitFillLightList(ComboBox cb)
        {
            cb.Items.Clear();
            for (int i = 0; i < DevFillLightList.Length; i++)
                cb.Items.Add(DevFillLightList[i].GetName());
        }

        static public void InitGasList(ComboBox cb)
        {
            cb.Items.Clear();
            for (int i = 0; i < DevGasList.Length; i++)
                cb.Items.Add(DevGasList[i].GetName());
        }

        static public void InitGenList(ComboBox cb)
        {
            cb.Items.Clear();
            for (int i = 0; i < GenList.Length; i++)
                cb.Items.Add(GenList[i].GetName());
        }

        static public void InitRegList(ComboBox cb)
        {
            cb.Items.Clear();
            for (int i = 0; i < RegList.Length; i++)
                cb.Items.Add(RegList[i].GetName());
        }
    }
}
