using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpectroWizard.data
{
    public class StLibStandart
    {
        List<StLibElement> Elements = new List<StLibElement>();
        public StLibElement this[int index]
        {
            get
            {
                return Elements[index];
            }
        }
            
        public StLibElement FindByElementIndex(int e_index)
        {
            for (int i = 0; i < Elements.Count; i++)
                if (Elements[i].ElementIndex == e_index)
                    return Elements[i];
            return null;
        }

        public StLibElement FindByElementName(String name)
        {
            for (int i = 0; i < Elements.Count; i++)
                if (Elements[i].Element.Equals(name))
                    return Elements[i];
            return null;
        }

        public int Count
        {
            get
            {
                return Elements.Count;
            }
        }

        public void Add(StLibElement elem)
        {
            Elements.Add(elem);
        }
    }

    public class StLibElement
    {
        //public string Element;
        public int ElementIndex;
        public string StandartName = "";
        public string Element
        {
            get
            {
                return ElementTable.Elements[ElementIndex].Name;
            }
        }
        public double Con;
        public bool IsBase;
        public bool IsAproxim;

        public StLibElement()
        {
        }

        public StLibElement(BinaryReader br)
        {
            throw new Exception("Not implemented");
        }

        public void Save(BinaryWriter bw)
        {
            throw new Exception("Not implemented");
        }
    }

    public class StLib
    {
        public const string MLSConst = "StLib";
        List<StLibStandart> St = new List<StLibStandart>();
        public StLibStandart this[int index]
        {
            get
            {
                return St[index];
            }
        }

        public int Count
        {
            get
            {
                return St.Count;
            }
        }

        /*public static string FindSt(string in_name)
        {
            string name;
            int index = in_name.LastIndexOf('\\');
            if (index > 0)
                name = in_name.Substring(index + 1);
            else
                name = in_name;
            index = name.IndexOf(".stl");
            if (index < 0)
                name += ".stl";
            string[] dirs = Directory.GetDirectories(Common.DBBaseStLib);
            for (int i = 0; i < dirs.Length; i++)
            {
                string find = FindSt(name,dirs[i]);
                if (find != null)
                    return find;
            }
            string[] files = Directory.GetFiles(Common.DBBaseStLib);
            for (int i = 0; i < files.Length; i++)
            {
                if(files[i].Substring(files[i].LastIndexOf("\\")+1).Equals(name))
                    return files[i];
            }
            return null;
        }

        static string FindSt(string name,string base_path)
        {
            string[] dirs = Directory.GetDirectories(base_path);
            for (int i = 0; i < dirs.Length; i++)
            {
                string find = FindSt(name, dirs[i]);
                if (find != null)
                    return find;
            }
            string[] files = Directory.GetFiles(base_path);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Substring(files[i].LastIndexOf("\\") + 1).Equals(name))
                    return files[i];
            }
            return null;
        }*/

        string BaseName = "";
        public StLib(string name)
        {
            //string path = FindSt(name);
            //FileStream fs = DataBase.OpenFile(ref path,FileMode.Open,FileAccess.Read);
            string path = (string)name.Clone();
            int from = path.LastIndexOf("\\");
            from++;
            if (from < 0)
                from = 0;
            int to = path.LastIndexOf(".");
            if (to > 0)
                BaseName = path.Substring(from, to - from);
            else
                BaseName = path.Substring(from);
            FileStream fs = DataBase.OpenFile(ref path, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[fs.Length];
            fs.Read(buf,0,buf.Length);
            fs.Close();
            string txt = "";
            for (int i = 0; i < buf.Length; i++)
                txt += (char)buf[i];
            InitByText(txt);
        }

        public string SrcText,ResultText;
        public StLib()
        {
            //InitByText(init_text);
        }

        public bool InitByText(string init_text)
        {
            bool ret = true;
            SrcText = init_text;
            int i = 0;
            string error = "";
            string e_name = "";
            string sig = "";
            string val = "";
            int st_count = 0;
            string name = null;

            StLibStandart cur_st = null;

            while (i < init_text.Length)
            {
                string warn = null;
                string line = GetString(init_text, ref i);
                try
                {
                    int comment_index = line.IndexOf("#");
                    if (comment_index >= 0)
                        line = line.Substring(0, comment_index);
                    line = line.Trim();
                    if (line.Length == 0)
                    {
                        ResultText += " ";
                    }
                    else
                    {
                        if (line[0] == '-')
                        {
                            st_count ++;
                            bool error_found = false;
                            /*for (int t = 0; t < line.Length;t++ )
                                if (line[t] != '-')
                                {
                                    error_found = true;
                                    break;
                                }*/
                            name = "";
                            for (int t = 0; t < line.Length; t++)
                            {
                                if(line[t] == '#')
                                    break;
                                if(line[t] == '-')
                                    continue;
                                name += line[t];
                            }
                            name = name.Trim();
                            if (name.Length == 0)
                                name = BaseName + st_count;
                            if (error_found == false)
                            {
                                cur_st = new StLibStandart();
                                St.Add(cur_st);
                                ResultText += Common.MLS.Get(MLSConst, "-Начало стандарта ") + name;
                            }
                            else
                            {
                                ResultText += Common.MLS.Get(MLSConst, "Ошибка!!! В строке начала нового стандарта присутствуют символы отличные от '-'");
                                ret = false;
                                cur_st = new StLibStandart();
                                St.Add(cur_st);
                            }
                        }
                        else
                        {
                            if (cur_st == null)
                            {
                                ResultText += Common.MLS.Get(MLSConst,"Ошибка!!! С помощью '-' укажите начало нового стандарта перед заданием концентрации");
                                ret = false;
                                cur_st = new StLibStandart();
                                St.Add(cur_st);
                            }
                            string nline = "";
                            int j = 0;
                            for (; j < line.Length; j++)
                                if (char.IsLetterOrDigit(line[j]) ||
                                    line[j] == '.' ||
                                    line[j] == ',' ||
                                    line[j] == '=' ||
                                    line[j] == '~' ||
                                    line[j] == '?')
                                    nline += line[j];
                            e_name = "";
                            sig = "";
                            val = "";
                            line = nline;
                            for (j = 0; j < line.Length; j++)
                                if (char.IsLetter(line[j]) || line[j] == ' ')
                                    e_name += line[j];
                                else
                                    break;
                            bool is_duble = false;
                            int e_index = ElementTable.FindIndex(e_name);
                            if(e_index < 0)
                            {
                                ResultText += Common.MLS.Get(MLSConst,"Не существующее имя элемента: ") + e_name;
                                ret = false;
                                is_duble = true;
                            }
                            for (int k = 0; k < cur_st.Count; k++)
                            {
                                if (cur_st[k].ElementIndex == e_index)//if (e_name.Equals(cur_st[k].Element))
                                {
                                    ResultText += Common.MLS.Get(MLSConst,"Дубликат элемента: ") + e_name;
                                    ret = false;
                                    is_duble = true;
                                    break;
                                }
                            }
                            if (is_duble == false)
                            {
                                if (j < line.Length && (line[j] == '=' || line[j] == '~'))
                                {
                                    sig += line[j];
                                    j++;
                                }
                                for (; j < line.Length; j++)
                                    if (char.IsDigit(line[j]) || line[j] == '.' ||
                                        line[j] == ',' || line[j] == '?')
                                        val += line[j];
                                    else
                                        break;
                                if (j < line.Length)
                                {
                                    if(line[j] != '#')
                                    {
                                        warn = Common.MLS.Get(MLSConst, "Некорректное завершение строки: ") + 
                                            "'"+line.Substring(j)+"'"+
                                            Common.MLS.Get(MLSConst, " Для комментария используйте #.");
                                        ret = false;
                                    }       
                                }
                                ResultText += e_name;
                                StLibElement el = new StLibElement();
                                el.StandartName = name;
                                el.ElementIndex = e_index;//e_name;
                                if (sig.Length == 0 ||
                                    val.Length == 0)
                                {
                                    ResultText += Common.MLS.Get(MLSConst,"-основа");
                                    el.IsBase = true;
                                }
                                else
                                {
                                    if (sig[0] == '=')
                                    {
                                        el.IsAproxim = false;
                                        if (val[0] != '?')
                                        {
                                            el.Con = serv.ParseDouble(val);
                                            ResultText += " = " + el.Con + Common.MLS.Get(MLSConst," (точно)");
                                        }
                                        else
                                        {
                                            el.Con = -1;
                                            ResultText += Common.MLS.Get(MLSConst," - не регламентируется");
                                        }
                                    }
                                    else
                                    {
                                        el.IsAproxim = true;
                                        if (val[0] != '?')
                                        {
                                            el.Con = serv.ParseDouble(val);
                                            ResultText += " ~ " + el.Con + Common.MLS.Get(MLSConst," (приблизительно)");
                                        }
                                        else
                                        {
                                            el.Con = -1;
                                            ResultText += Common.MLS.Get(MLSConst," - может присутствовать");
                                        }
                                    }
                                }
                                cur_st.Add(el);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.OutNoMsg(ex);
                    ResultText += Common.MLS.Get(MLSConst,"Ошибка!!! ")+error+". '"+e_name+"''"+sig+"''"+val+"'";
                    ret = false;
                }
                if (warn != null)
                    ResultText += Common.MLS.Get(MLSConst, " Предупреждение:")+warn;
                ResultText += ((char)0xD);
                ResultText += ((char)0xA);
            }
            try
            {
                ResultText += Common.MLS.Get(MLSConst, "--------- Конец файла ---------") + serv.Endl;
                ResultText += Common.MLS.Get(MLSConst, "Всего распознано:") + Count + Common.MLS.Get(MLSConst, " стандартов.") + serv.Endl;
                double[] max_cons = null;
                if (Count > 0)
                {
                    max_cons = new double[this[0].Count];
                    for (int mc = 0; mc < max_cons.Length; mc++)
                        max_cons[mc] = -double.MaxValue;
                    for (int st = 0; st < Count; st++)
                    {
                        for (i = 0; i < this[st].Count; i++)
                            try
                            {
                                if (this[st][i].Con > max_cons[i])
                                    max_cons[i] = this[st][i].Con;
                            }
                            catch
                            {
                                ResultText += Common.MLS.Get(MLSConst, "Oшибка. В стандарте:") + (st + 1) + " нехватает элементов." + serv.Endl;
                                ret = false;
                            }
                    }
                }
                for (int st = 0; st < Count; st++)
                {
                    double sum = 0;
                    double sko = 0;
                    for (i = 0; i < this[st].Count; i++)
                    {
                        if (this[st][i].Con > 0)
                        {
                            sum += this[st][i].Con;
                            double dlt;
                            try
                            {
                                dlt = this[st][i].Con - max_cons[i];
                            }
                            catch (Exception ex)
                            {
                                dlt = 0;
                                ret = false;
                                ResultText += Common.MLS.Get(MLSConst, "Ошибка вычисления суммы на стандарте "+(st+1)+" элемент "+(i+1));
                            }
                            if (max_cons[i] > 0)
                                dlt *= 100 / max_cons[i];
                            else
                                dlt = 0;
                            sko += dlt * dlt;
                        }
                    }
                    sko = Math.Sqrt(sko / this[st].Count);
                    if (sum <= 100)
                        ResultText += Common.MLS.Get(MLSConst, "Сумма концентраций по ") + (st + 1) + Common.MLS.Get(MLSConst, " стандарту ") + Math.Round(sum, 5) + "%";
                    else
                    {
                        ResultText += Common.MLS.Get(MLSConst, "Предупруждение!!!! Сумма концентраций по ") + (st + 1) + Common.MLS.Get(MLSConst, " стандарту больше 100%: ") + Math.Round(sum, 1) + "%";
                        ret = false;
                    }
                    ResultText += Common.MLS.Get(MLSConst, " СКО от максимума ") + Math.Round(sko, 1) + "%" + serv.Endl;
                }
            }
            catch (Exception ex)
            {
                ResultText += Common.MLS.Get(MLSConst, "Ошибка вычисления суммы...");
                Log.Out(ex);
            }
            return ret;
        }

        string GetString(string text, ref int index)
        {
            int to;
            for (to = index; to < text.Length; to++)
            {
                if (text[to] == '\n')
                    break;
            }
            string ret = text.Substring(index, to - index);
            index = to + 1;
            return ret;
        }
    }
}
