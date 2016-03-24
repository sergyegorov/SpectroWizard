using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace SpectroWizard.gui.comp
{
    public partial class DeletedProbLogViewer : UserControl
    {
        public DeletedProbLogViewer()
        {
            InitializeComponent();
        }

        static string GetPath(string path)
        {
            int i = path.LastIndexOf("\\method");
            string tmp;
            if (i > 0)
                tmp = path.Substring(0,i);
            else
                tmp = (string)path.Clone();
            tmp = Common.Db.GetFoladerPath(tmp);
            tmp = tmp + "\\журнал удалённых проб.csv";
            return tmp;
        }
        public static void SaveData(string path,string txt)
        {
            string tmp = GetPath(path);// +"\\журнал удалённых проб.csv";
            FileStream fs = new FileStream(tmp, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buf = Encoding.Default.GetBytes(txt);
            fs.Write(buf, 0, buf.Length);
            fs.SetLength(buf.Length);
            fs.Flush();
            fs.Close();
        }

        public static string LoadData(string path)
        {
            string tmp = GetPath(path); //path + "\\журнал удалённых проб.csv";

            if (File.Exists(tmp) == false)
                return "";

            FileStream fs = new FileStream(tmp, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[(int)fs.Length];
            fs.Read(buf, 0, buf.Length);
            fs.Close();

            char[] ch = new char[buf.Length];
            Encoding.Default.GetDecoder().GetChars(buf, 0, buf.Length, ch, 0);
            string txt = new string(ch);

            return txt;
        }

        public void Init(string path)
        {
            try
            {
                string tmp = LoadData(path);
                tbLogField.Text = tmp;
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
