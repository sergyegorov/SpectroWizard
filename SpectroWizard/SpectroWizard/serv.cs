using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace SpectroWizard
{
    class serv
    {
        public static void StartCMD(string command)
        {
            string strCmdText = command.Trim();
            if (strCmdText.StartsWith("/C") == false)
                strCmdText = "/C "+strCmdText;
            //strCmdText = "/C copy /b Image1.jpg + Archive.rar Image2.jpg";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static Form FindParentForm(Control contr)
        {
            if (contr == null)
                return null;
            if (contr is Form)
                return (Form)contr;
            return FindParentForm(contr.Parent);
        }

        public static string Endl = "" + (char)0xD + (char)0xA;
        public static string GetLine(string src, ref int index)
        {
            string ret = "";
            if (src.Length <= index)
                return null;
            for (; index < src.Length && src[index] != '\n'; index++)
            {
                if (char.IsControl(src[index]))
                    continue;
                ret += src[index];
            }
            index++;
            return ret;
        }

        public static Icon ReadIcon16x16(string path)
        {
            Icon ret = new Icon(path, 16, 16);
            return ret;
        }

        public static string GetGoodValue(double val, int val_sig)
        {
            int sig_mul;
            if (val == 0)
            {
                string ret = "0.";
                while (val_sig > 0)
                {
                    ret += "0";
                    val_sig--;
                }
                return ret;
            }
            if (val < 0)
            {
                val = -val;
                sig_mul = -1;
            }
            else
                sig_mul = 1;
            double tmp = val;
            if (tmp < 1)
            {
                while (tmp < 0.1 && val_sig < 5)
                {
                    tmp *= 10;
                    val_sig++;
                }
            }
            else
            {
                while (tmp > 10 && val_sig > 0)
                {
                    tmp /= 10;
                    val_sig--;
                }
            }
            return Math.Round(val * sig_mul, val_sig).ToString();
        }

        public static string RemoveAllAfter(string str, char sym)
        {
            int ind = str.IndexOf(sym);
            if (ind < 0)
                return str;
            return str.Substring(0, ind);
        }

        public static string RemoveSpaces(string str)
        {
            string ret = "";
            for (int i = 0; i < str.Length; i++)
                if(str[i] != ' ' && str[i] != '\t')
                    ret += str[i];
            return ret;
        }


        public static string RemoveControlSym(string str)
        {
            string ret = "";
            for (int i = 0; i < str.Length; i++)
                if (str[i] >= ' ')
                    ret += str[i];
            return ret;
        }

        public static int ParseRim(string txt)
        {
            string tmp = txt.Trim().ToLower();
            if (tmp.Length == 0)
                return 0;
            if (tmp.Equals("i")) return 1;
            if (tmp.Equals("ii")) return 2;
            if (tmp.Equals("iii")) return 3;
            if (tmp.Equals("iv")) return 4;
            if (tmp.Equals("v")) return 5;
            if (tmp.Equals("vi")) return 6;
            if (tmp.Equals("vii")) return 7;
            if (tmp.Equals("viii")) return 8;
            if (tmp.Equals("ix")) return 9;
            if (tmp.Equals("x")) return 10;
            if (tmp.Equals("xi")) return 11;
            if (tmp.Equals("xii")) return 12;
            if (tmp.Equals("xiii")) return 13;
            if (tmp.Equals("xiv")) return 14;
            if (tmp.Equals("xv")) return 15;
            if (tmp.Equals("xvi")) return 16;
            if (tmp.Equals("xvii")) return 17;
            if (tmp.Equals("xviii")) return 18;
            if (tmp.Equals("xix")) return 19;
            if (tmp.Equals("xx")) return 20;
            if (tmp.Equals("xxi")) return 21;
            if (tmp.Equals("xxii")) return 22;
            if (tmp.Equals("xxiii")) return 23;
            if (tmp.Equals("xxiv")) return 24;
            if (tmp.Equals("xxv")) return 25;
            if (tmp.Equals("xxvi")) return 26;
            if (tmp.Equals("xxvii")) return 27;
            if (tmp.Equals("xxviii")) return 28;
            if (tmp.Equals("xxix")) return 29;
            if (tmp.Equals("xxx")) return 30;

            throw new Exception("Unknow format:" + tmp);
        }

        public static bool IsPathCorrect(string path)
        {
            char[] vals = { '>', '<', '|', '?', '*', '/', '"' };
            if (path.IndexOfAny(vals) > 0)
                return false;
            return true;
        }

        public static bool CheckFileName(string val)
        {
            val = val.Trim();
            if (val.Length == 0)
                return false;
            char[] vals = { '>', '<', '|', '?', '*', '/', '\\', ':', '"' };
            if (val.IndexOfAny(vals) > 0)
                return false;
            if(val.Trim().ToLower().Equals("nul"))
                return false;
            return true;
        }

        public static double ParseDouble(string text)
        {
            string sigstr = (1.1).ToString();
            char sig = sigstr[1];
            char anti_sig;
            if (sig == ',')
            {
                sig = ',';
                anti_sig = '.';
            }
            else
            {
                sig = '.';
                anti_sig = ',';
            }
            int index = text.IndexOf(anti_sig);
            if (index > 0)
                text = text.Substring(0, index) + sig + text.Substring(index + 1);
            return double.Parse(text);
        }

        /// <summary>
        /// This function returns good value for lVal
        /// </summary>
        /// <param name="lVal">1,1011</param>
        /// <returns>1</returns>
        public static double GetGoodValue(double lVal, double lStep)
        {
            if (double.IsInfinity(lVal) || double.IsNaN(lVal) || double.IsNegativeInfinity(lVal) || double.IsPositiveInfinity(lVal) ||
                double.IsInfinity(lStep) || double.IsNaN(lStep) || double.IsNegativeInfinity(lStep) || double.IsPositiveInfinity(lStep))
                return 0;
            if (lStep >= 1)
                return (int)lVal;

            if (lStep == 0)
                return lVal;

            if (lVal == 0)
                return 0;

            int por = 0;
            while (lStep < 1)
            {
                lStep *= 10;
                por++;
            }

            return Math.Round(lVal, por);
        }

        public static bool IsValid(double val)
        {
            if (double.IsInfinity(val) || double.IsNaN(val) || double.IsNegativeInfinity(val) || double.IsPositiveInfinity(val))
                return false;
            return true;
        }

        public static bool IsValid(float val)
        {
            if (float.IsInfinity(val) || float.IsNaN(val) || float.IsNegativeInfinity(val) || float.IsPositiveInfinity(val))
                return false;
            return true;
        }

        static double[] logVals = { 0.0001, 0.0002, 0.0005, 0.001, 0.002, 0.0015, 0.01, 0.02, 0.05, 0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000, 200000, 500000, 1000000 };
        public static double[] GetGoodValues(double lfrom, double lto, int n,bool isLog)
        {
            if (isLog == false)
                return GetGoodValues(lfrom, lto, n);
            int from = 0;
            for (; from <= logVals.Length && logVals[from] < lfrom; from++) ;
            int to = from;
            for (; to <= logVals.Length && logVals[to] < lto; to++) ;
            if(to-from < 2)
                return GetGoodValues(lfrom, lto, n);
            double[] ret = new double[to-from];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = logVals[i + from];
            return ret;
        }

        public static double[] GetGoodValues(double lfrom, double lto, int n)
        {
            if (IsValid(lfrom) == false || IsValid(lto) == false)
                return null;

            if (n <= 0)
                n = 1;

            double dlt = lto - lfrom;
            double step = dlt / n;

            if (step < 0.00000000001)
                step = 0.00000000001;

            int por = 0;
            double from = lfrom - step;
            double dir;
            if (step < 1)
                dir = 10;
            else
                dir = 0.1;

            while (step < 1 || step > 10)
            {
                from *= dir;
                step *= dir;
                por++;
            }

            from = (int)from;
            step = (int)step;
            for (int i = 0; i < por; i++)
            {
                step /= dir;
                from /= dir;
            }

            for (int i = 0; i < 10 && from >= lfrom; i++)
                from -= step;

            int rn = (int)((lto - from) / step);

            if (rn > 0 && rn < 200)
            {
                rn++;
                double[] ret = new double[rn];
                for (int i = 0; i < rn; i++)
                {
                    ret[i] = GetGoodValue(from, step);
                    from += step;
                }
                return ret;
            }
            return null;
        }

        public static void SetAllComboBoxesSelectOnly(Control con)
        {
            if (con is ComboBox)
            {
                ComboBox bx = (ComboBox)con;
                //string selected_item = (string)bx.SelectedItem;
                bx.DropDownStyle = ComboBoxStyle.DropDownList;
                bx.BackColor = SystemColors.Control;
            }
            else
            {
                foreach (Control c in con.Controls)
                    SetAllComboBoxesSelectOnly(c);
            }
        }
    }
}
