using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace SpectroWizard.util
{
    public class CSV
    {
        string Delim,Endl;
        public CSV(string delimeter, string endl)
        {
            Delim = delimeter;
            //DecimalDel = decimal_del;
            Endl = endl;
        }

        public void Save(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                fs.SetLength(0);
                for (int r = 0; r < Data.Count; r++)
                {
                    for (int c = 0; c < Data[r].Count; c++)
                    {
                        byte[] buf = Encoding.Default.GetBytes(Data[r][c].ToString());
                        fs.Write(buf, 0, buf.Length);
                    }
                }
                fs.Flush();
            }
            finally
            {
                fs.Close();
            }
        }

        public CSV()
        {
            Delim = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            //DecimalDel = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            Endl = ""+(char)0xd+(char)0xa;
        }

        public List<List<object>> Data = new List<List<object>>();
        public void AddFile(string file_name)
        {
            //foreach(string line in File.ReadAllLines(file_name))
            //    string[] fields = line.Split(';');
            FileStream fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
            byte[] buf;
            try
            {
                buf = new byte[fs.Length];
                fs.Read(buf, 0, buf.Length);
            }
            catch (Exception ex)
            {
                fs.Close();
                Common.Log(ex);
                return;
            }
            finally
            {
                fs.Close();
            }

            string txt = Encoding.Default.GetString(buf);
            int ind_from = 0;
            List<object> line = new List<object>();
            while (ind_from < txt.Length)
            {
                string found = Delim;
                int ind_to = txt.IndexOf(Delim, ind_from);
                if (ind_to < 0)
                {
                    line.Add(txt.Substring(ind_from));
                    break;
                }
                int endl_ind = txt.IndexOf(Endl, ind_from, ind_to - ind_from);
                if (endl_ind > 0)
                {
                    found = Endl;
                    ind_to = endl_ind;
                }
                line.Add(txt.Substring(ind_from, ind_to - ind_from).Trim());
                ind_from = ind_to + found.Length;
                if (endl_ind > 0)
                {
                    Data.Add(line);
                    line = new List<object>();
                }
                ind_from = ind_to+1;
            }
            if (line.Count > 0)
                Data.Add(line);
        }
    }
}
