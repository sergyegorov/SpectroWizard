using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpectroWizard.util
{
    public class SpectrKeeper
    {
        public static void bkpData(String name)
        {
            try
            {
                if (File.Exists("mb") == false)
                    return;
                String path = "Записанные Данные\\" + name + "\\";
                if (File.Exists(path) == false)
                    Directory.CreateDirectory(path);
                String fname = "" + DateTime.Now.Ticks;
                path += fname + "\\";
                Directory.CreateDirectory(path);
                File.Copy("cur_prog.txt", path + "cur_prog.txt");
                for (int i = 0; i < 100; i++)
                {
                    fname = "data" + i;
                    if (File.Exists(fname) == false)
                        break;
                    File.Copy(fname, path + fname);
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
