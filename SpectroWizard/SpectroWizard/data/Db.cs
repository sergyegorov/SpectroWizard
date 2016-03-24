using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace SpectroWizard.data
{
    public class DataBase
    {
        public static string BasePath;
        public DataBase(string[] base_folders,string[] old_folders)
        {
            if (Directory.Exists(Common.Conf.DbPath) == false)
                Directory.CreateDirectory(Common.Conf.DbPath);

            BasePath = Common.Conf.DbPath;
            if (BasePath[BasePath.Length - 1] != Path.DirectorySeparatorChar)
                BasePath += Path.DirectorySeparatorChar;

            for (int i = 0; i < old_folders.Length; i++)
            {
                if (Directory.Exists(BasePath + old_folders[i]) && 
                    Directory.Exists(BasePath + base_folders[i]) == false)
                    Directory.Move(BasePath + old_folders[i], BasePath + base_folders[i]);
            }

            for (int i = 0; i < base_folders.Length; i++)
            {
                string path = BasePath+base_folders[i];
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }
            if (Path.IsPathRooted(Common.Conf.DbPath) == false)
                BasePath = System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + BasePath;
        }

        public static string GetFullPath(string path)
        {
            string tmp = (string)path.Clone();
            CheckPath(ref tmp);
            return BasePath + tmp;
        }

        public static FileStream OpenFile(ref string path, FileMode fm, FileAccess fa)
        {
            CheckPath(ref path);
            string tmp = BasePath+path;
            return new FileStream(tmp, fm, fa);
        }

        public static void RemoveFile(string rel_path)
        {
            if (Common.ExtraSafe)
            {
                string dst;
                if (File.Exists(rel_path))
                    dst = rel_path;
                else
                    dst = BasePath + rel_path;
                if (File.Exists(dst + "_deleted"))
                    File.Delete(dst + "_deleted");
                File.Move(dst, dst + "_deleted");
            }
            else
            {
                if (File.Exists(rel_path))
                    File.Delete(rel_path);
                else
                    File.Delete(BasePath + rel_path);
            }
        }

        static public string CheckPath(ref string src)
        {
            if (src == null)
                return src;
            int ind = src.LastIndexOf(Common.Conf.DbPath);
            if (ind > 0)
                src = src.Substring(ind + Common.Conf.DbPath.Length);
            return src;
        }

        static public DateTime GetFileDateTime(ref string path)
        {
            CheckPath(ref path);
            //return File.GetCreationTime(BasePath + path);
            return File.GetLastWriteTime(BasePath + path);
        }

        static public bool FileExists(ref string path)
        {
            CheckPath(ref path);
            return File.Exists(BasePath + path);
        }

        static public bool DirectoryExists(ref string path)
        {
            CheckPath(ref path);
            return Directory.Exists(BasePath + path);
        }

        public string GetFoladerPath(string path)
        {
            return BasePath + path;
        }

        public DbFolder GetFolder(string path)
        {
            return new DbFolder(GetFoladerPath(path));
        }

        public DbFolder CreateFolder(string path)
        {
            if (Directory.Exists(GetFoladerPath(path)) == true)
                throw new Exception(Common.MLS.Get("DbErr", "Can't create folder. It exists."));
            Directory.CreateDirectory(GetFoladerPath(path));
            return new DbFolder(GetFoladerPath(path));
        }

        //List<SpectrCashRecord> SpectrCash = new List<SpectrCashRecord>();
        public Spectr LoadSpectr(string path)
        {
            /*
            for (int i = 0; i < SpectrCash.Count; i++)
                if (SpectrCash[i].Sp.GetPath().Equals(path))
                {
                    SpectrCash[i].Accessed = DateTime.Now.Ticks;
                    return SpectrCash[i].Sp;
                }
            Spectr sp = Spectr.GetSpectr_Call_Only_Db_Method(path);//new Spectr(path);
            SpectrCashRecord r = new SpectrCashRecord(sp);
            SpectrCash.Add(r);
            if (SpectrCash.Count > 20)
            {
                for (int at = 0; at < 2; at++)
                {
                    long min_time = DateTime.Now.Ticks;
                    int found = -1;
                    for (int i = 0; i < SpectrCash.Count; i++)
                        if (SpectrCash[i].Accessed < min_time)
                        {
                            min_time = SpectrCash[i].Accessed;
                            found = i;
                        }
                    if ((DateTime.Now.Ticks - min_time) < 60 * 10000000)
                        break;
                    SpectrCash.RemoveAt(found);
                }
            }//*/
            Spectr sp = new Spectr(path);
            return sp;
        }

        public void SpectrSaved(Spectr sp)
        {
            /*string path = sp.GetPath();
            for (int i = 0; i < SpectrCash.Count; i++)
                if (SpectrCash[i].Sp.GetPath().Equals(path))
                    SpectrCash.RemoveAt(i);*/
        }

        class SpectrCashRecord
        {
            public Spectr Sp;
            public long Accessed;

            public SpectrCashRecord(Spectr sp)
            {
                Sp = sp;
                Accessed = DateTime.Now.Ticks;
            }
        }
    }

    public class DbFolder
    {
        string Path;

        public string GetPath()
        {
            return (string)Path.Clone();
        }

        protected internal DbFolder(string path)
        {
            //DataBase.CheckPath(ref path);
            if(DataBase.DirectoryExists(ref path) == false)//if(Directory.Exists(path) == false)
                throw new Exception(Common.MLS.Get("DbErr", "Can't open unexisting folder. ")+path);
            Path = path;
            if (Path[Path.Length - 1] != '\\')
                Path += "\\";
        }

        public DbFolder GetParent()
        {
            int ind = Path.LastIndexOf('\\');
            string path = Path.Substring(0,ind);
            return new DbFolder(path);
        }

        protected internal DbFolder(string path,DbFolder base_folder)
        {
            //if (Directory.Exists(base_folder.Path+path) == false)
            string tmp = base_folder.Path + path;
            if (DataBase.DirectoryExists(ref tmp) == false)
                throw new Exception(Common.MLS.Get("DbErr", "Can't open unexisting folder. ") + path);
            Path = tmp;
            if (Path[Path.Length - 1] != '\\')
                Path += "\\";
        }

        public string[] GetFolderList(bool cut_of_ext)
        {
            return GetFolderList("*.*",cut_of_ext);
        }

        public string[] GetFolderList()
        {
            return GetFolderList("*.*", false);
        }

        public string[] GetFolderList(string mask, bool cut_of_ext)
        {
            string[] ret = Directory.GetDirectories(DataBase.BasePath + Path,mask);
            if (ret == null)
                return ret;
            //int len = DataBase.BasePath.Length;
            string p = DataBase.BasePath + Path;
            int len = p.Length;
            for (int i = 0; i < ret.Length; i++)
            {
                int index = ret[i].IndexOf(p);
                if (index >= 0)
                    ret[i] = ret[i].Substring(index+len);
                if (cut_of_ext == false)
                    continue;
                index = ret[i].LastIndexOf('.');
                if (index >= 0)
                    ret[i] = ret[i].Substring(0,index);
            }
            return ret;
        }

        public string[] GetFolderList(string mask)
        {
            return GetFolderList(mask, false);
        }

        public string[] GetRecordList(string prefix,
            string ext, 
            bool cut_of_ext)
        {
            string[] ret = Directory.GetFiles(DataBase.BasePath + Path, prefix + "." + ext);
            if (ret == null)
                return ret;
            string p = DataBase.BasePath + Path;
            int len = p.Length;
            for (int i = 0; i < ret.Length; i++)
            {
                int index = ret[i].IndexOf(p);
                if (index >= 0)
                    ret[i] = ret[i].Substring(index + len);
                if (cut_of_ext)
                {
                    index = ret[i].LastIndexOf('.');
                    if (index >= 0)
                        ret[i] = ret[i].Substring(0, index);
                }
            }
            return ret;
        }

        public string[] GetRecordList(string ext, bool cut_of_ext)
        {
            return GetRecordList("*", ext, cut_of_ext);
        }

        public string[] GetRecordList(string ext)
        {
            return GetRecordList(ext, false);
        }

        public void DeleteFolder(string folder_name)
        {
            string path = DataBase.BasePath + Path + folder_name;
            Directory.Delete(path);
        }

        public void DeleteFolder()
        {
            string path = DataBase.BasePath + Path;
            Directory.Delete(path);
        }

        public void ClearFolder(string folder_name)
        {
            string path = DataBase.BasePath + Path + folder_name;
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
                File.Delete(files[i]);
        }

        public void CreateFolder(string folder_name)
        {
            string path = DataBase.BasePath + Path + folder_name;
            Directory.CreateDirectory(path);
        }

        public string CreateRecordPath(string record_name)
        {
            string path = Path+record_name;
            return path;
        }

        public string GetRecordPath(string record_name)
        {
            string path = Path + record_name;
            return path;
        }
    }
}
