using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;
using System.Threading;
using SpectroWizard.gui;
using SpectroWizard.data;

namespace SpectroWizard
{
    public partial class Login : Form
    {
        public Login()
        {
            /*try
            {
                FileStream fs = new FileStream("test", FileMode.OpenOrCreate, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                //String str = "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111";
                String str = "1111111111111111";
                bw.Write(str);
                bw.Flush();
                bw.Close();
            }
            catch (Exception ex)
            {
                String msg = ex.ToString();
            }*/
            InitializeComponent();
            //btnLoginLaborant.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 9);
            //Size = new System.Drawing.Size(799, 355);
            Common.Start();
            try
            {
                Common.Log("Program started");
                if (File.Exists("lang_new.txt"))
                    Common.MLS = new Mls("lang_new.txt", "lang.txt", true);
                else
                    Common.MLS = new Mls("lang_new.txt", "lang.txt", false);
                Common.Reg(this, "Login");
                Text = Common.ProgramFullInfo;
                btnLoginLaborant.Font = Common.GetDefaultFont(btnLoginLaborant.Font,FontStyle.Bold | FontStyle.Underline);
                btnLoginMetodist.Font = Common.GetDefaultFont(btnLoginLaborant.Font,FontStyle.Regular);
                label1.Font = Common.GetDefaultFont(label1.Font);
                btRestore.Enabled = BackUpSystem.Enable;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            CheckForIllegalCrossThreadCalls = false;
        }

        MainForm MF = null;
        void ShowMainWindow()
        {
            Enabled = false;
            BkpMsg bmsg = new BkpMsg();
            try
            {
                Common.Log("Check bkp.");
                if (chbBackupSkip.Checked)
                    Common.Log("Skipped by user....");
                else
                    BackUpSystem.DoBackupIfNeed(bmsg);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            finally
            {
                Enabled = true;
                bmsg.Close();
            }
            Common.Log("Loged as '"+Common.UserRole+"'");
            //MainForm mf = null;
            try
            {
                MF = new MainForm();
                /*Process p = Process.GetCurrentProcess();
                do
                {
                    Thread.Sleep(50);
                } while (p.MainWindowHandle == null);*/
                Common.SetupLogInfo();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            try
            {
                //Control.CheckForIllegalCrossThreadCalls = false;
                Visible = false;
               /*Process p = Process.GetCurrentProcess();
                do
                {
                    Thread.Sleep(50);
                } while (p.MainWindowHandle == null);
                MF.ShowDialog();*/
                MF.ShowDialog();
                //Control.CheckForIllegalCrossThreadCalls = true;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                try
                {
                    MF.Close();
                }
                catch
                {
                }
            }
            try
            {
                Close();
            }
            catch(Exception ex)
            {
                Common.Log(ex);
            }
            Common.Stop();
        }

        int ClickCount = 0;
        private void lbLoginSience_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                
                ClickCount++;
                if (ClickCount < 2)
                    return;
                Common.UserRole = Common.UserRoleTypes.Sientist;
                ShowMainWindow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void CheckConfig()
        {
            if (Common.Conf.LastStartedBuild < 57)
            {
                Common.Conf.LastStartedBuild = Common.DevelopersBuild;
                Common.Conf.Save();
                BkpMsg msg = new BkpMsg();
                BackUpSystem.DoBackup("Before_V57", msg);
                string[] dirs = Directory.GetDirectories(Common.Db.GetFoladerPath(""));
                foreach (string tmp in dirs)
                {
                    UpdateSpectrs(tmp);
                }   
                msg.Close();
            }
        }

        void UpdateSpectrs(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string tmp in dirs)
                UpdateSpectrs(tmp);
            string[] sp = Directory.GetFiles(path, "*.ss");
            foreach (string tmp in sp)
                try
                {
                    Spectr tsp = new Spectr(tmp);
                }
                catch (Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
        }

        private void btnLoginLaborant_Click(object sender, EventArgs e)
        {
            try
            {
                CheckConfig();
                Common.UserRole = Common.UserRoleTypes.Laborant;
                ShowMainWindow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btnLoginMetodist_Click(object sender, EventArgs e)
        {
            try
            {
                CheckConfig();
                Common.UserRole = Common.UserRoleTypes.Metodist;
                ShowMainWindow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btnLoginDebuger_Click(object sender, EventArgs e)
        {
            try
            {
                CheckConfig();
                Common.UserRole = Common.UserRoleTypes.Debuger;
                ShowMainWindow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Common.Stop();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
            try
            {
                Process proc = Process.GetCurrentProcess();
                proc.Kill();
            }
            catch
            {
            }
        }

        private void lbLoginSience_Click(object sender, EventArgs e)
        {
            try
            {
                CheckConfig();
                if (Common.Debug == false)
                    return;
                Common.UserRole = Common.UserRoleTypes.Sientist;
                ShowMainWindow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btRestore_Click(object sender, EventArgs e)
        {
            try
            {
                RestoreDialog rd = new RestoreDialog();
                rd.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        static Mutex MyMutex;
        private void Login_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Visible == true)
                {
                    try
                    {
                        MyMutex = Mutex.OpenExisting("SpectroWizard");
                        FirstInstWarn warn = new FirstInstWarn();
                        warn.Show();
                        Common.Log("Это НЕ первая копия запущенной программы.");
                    }
                    catch (WaitHandleCannotBeOpenedException ex)
                    {
                        Common.Log("Это первая копия запущенной программы.");
                        MyMutex = new Mutex(false, "SpectroWizard");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawString("(c) SPDFL Yegorov Sergey Anatolijovich.", DefaultFont, Brushes.Red, 10, 10);
                e.Graphics.DrawString("Tel. +380506389486", DefaultFont, Brushes.Red, 10, 30);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
