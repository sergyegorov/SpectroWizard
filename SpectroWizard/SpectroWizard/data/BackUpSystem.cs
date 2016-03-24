using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpectroWizard.data
{
    public class BackUpSystem
    {
        const string MLSConst = "BkpSystem";
        public static void Restore(string name)
        {
        }

        public static void Store(string from,string to)
        {
            if (from[from.Length - 1] != '\\')
                from += "\\";
            if (to[to.Length - 1] != '\\')
                to += "\\";
            String[] from_items = Directory.GetDirectories(from);
            for (int i = 0; i < from_items.Length; i++)
            {
                string dir_name = from_items[i];
                int ind = dir_name.LastIndexOf("\\");
                dir_name = dir_name.Substring(ind + 1);

                string dest_name = to + dir_name+"\\";
                Directory.CreateDirectory(dest_name);
                Store(from_items[i], dest_name);
            }
            from_items = Directory.GetFiles(from);
            for (int i = 0; i < from_items.Length; i++)
            {
                string file_name = from_items[i];
                int ind = file_name.LastIndexOf("\\");
                file_name = file_name.Substring(ind + 1);

                string dest_name = to + file_name;
                File.Copy(from_items[i], dest_name);
            }
        }

        public static void RemoveDir(string name)
        {
            String[] items = Directory.GetDirectories(name);
            for (int i = 0; i < items.Length; i++)
                RemoveDir(items[i]);
            items = Directory.GetFiles(name);
            for (int i = 0; i < items.Length; i++)
                File.Delete(items[i]);
            Directory.Delete(name);
        }

        public static string[] GetRecordList()
        {
            string[] ret = Directory.GetDirectories(Common.Conf.BkpPath);
            int path_len = Common.Conf.BkpPath.Length;
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = ret[i].Substring(0,ret[i].Length-1);
                int ind = ret[i].IndexOf(Common.Conf.BkpPath);
                if (ind < 0)
                    continue;
                ret[i] = ret[i].Substring(ind+path_len);
            }
            return ret;
        }

        public static bool Enable
        {
            get
            {
                try
                {
                    if (Common.Conf.BkpPath == null || Common.Conf.BkpPath.Length == 0)
                        return false;
                    if (Directory.Exists(Environment.CurrentDirectory + Common.Conf.BkpPath) == false)
                    {
                        if (Directory.Exists(Common.Conf.BkpPath) == false)
                            return false;
                        else
                            BaseDir = Common.Conf.BkpPath;
                    }
                    else
                        BaseDir = Environment.CurrentDirectory + Common.Conf.BkpPath;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        static string GetCurrentDayOfWeek()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday: return Common.MLS.Get(MLSConst, "Воскресенье");
                case DayOfWeek.Monday: return Common.MLS.Get(MLSConst, "Понедельник");
                case DayOfWeek.Tuesday: return Common.MLS.Get(MLSConst, "Вторник");
                case DayOfWeek.Wednesday: return Common.MLS.Get(MLSConst, "Среда");
                case DayOfWeek.Thursday: return Common.MLS.Get(MLSConst, "Четверг");
                case DayOfWeek.Friday: return Common.MLS.Get(MLSConst, "Пятница");
                case DayOfWeek.Saturday: return Common.MLS.Get(MLSConst, "Субота");
                default: throw new Exception("Unknow day of the week");
            }
        }

        static string GetCurrentMonth()
        {
            switch (DateTime.Now.Month)
            {
                case 1: return Common.MLS.Get(MLSConst, "Январь");
                case 2: return Common.MLS.Get(MLSConst, "Февраль");
                case 3: return Common.MLS.Get(MLSConst, "Март");
                case 4: return Common.MLS.Get(MLSConst, "Апрель");
                case 5: return Common.MLS.Get(MLSConst, "Май");
                case 6: return Common.MLS.Get(MLSConst, "Июнь");
                case 7: return Common.MLS.Get(MLSConst, "Июль");
                case 8: return Common.MLS.Get(MLSConst, "Август");
                case 9: return Common.MLS.Get(MLSConst, "Сентябрь");
                case 10: return Common.MLS.Get(MLSConst, "Октябрь");
                case 11: return Common.MLS.Get(MLSConst, "Ноябрь");
                case 12: return Common.MLS.Get(MLSConst, "Декабрь");
                default: throw new Exception("Unknown month...");
            }
        }

        public static void DoBackup(string name,BkpMsg bmsg)
        {
            string path = BaseDir + name;
            Directory.CreateDirectory(path);

            if (bmsg.Visible == false)
            {
                bmsg.Visible = true;
                bmsg.Refresh();
            }

            try
            {
                Store(Common.Conf.DbPath, path);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }

            Common.Log("Made month's bkp...");
            //bmsg.Close();
        }

        static string BaseDir = null;
        public static void DoBackupIfNeed(BkpMsg bmsg)
        {
            if(Enable == false)
            {
                Common.LogWar("No backup path: '" + Common.Conf.BkpPath + "'");
                return;
            }

            //string path = BaseDir + DateTime.Now.DayOfWeek.ToString() + "\\";
            string path = BaseDir + GetCurrentDayOfWeek() + "\\";
            if (Common.Conf.BkpWeek && 
                (Common.Conf.BkpWeekDate.Year != DateTime.Now.Year ||
                Common.Conf.BkpWeekDate.Month != DateTime.Now.Month ||
                Common.Conf.BkpWeekDate.Day != DateTime.Now.Day ||
                Directory.Exists(path) == false))
            {
                if (bmsg.Visible == false)
                {
                    bmsg.Visible = true;
                    bmsg.Refresh();
                }

                if (Directory.Exists(path))
                    RemoveDir(path);

                Directory.CreateDirectory(path);

                Store(Common.Conf.DbPath,path);

                Common.Log("Made days bkp...");
                Common.Conf.BkpWeekDate = new DateTime(DateTime.Now.Ticks);
            }

            //path = BaseDir + DateTime.Now.Month.ToString() + "\\";
            path = BaseDir + GetCurrentMonth() + "\\";
            if (Common.Conf.BkpYear && 
                (Common.Conf.BkpYearDate.Year != DateTime.Now.Year ||
                Common.Conf.BkpYearDate.Month != DateTime.Now.Month ||
                Directory.Exists(path) == false))
            {
                if (bmsg.Visible == false)
                {
                    bmsg.Visible = true;
                    bmsg.Refresh();
                }

                if (Directory.Exists(path))
                    RemoveDir(path);

                Directory.CreateDirectory(path);

                Store(Common.Conf.DbPath, path);

                Common.Log("Made month's bkp...");
                Common.Conf.BkpYearDate = new DateTime(DateTime.Now.Ticks);
            }

            Common.Conf.Save();
        }
    }
}
