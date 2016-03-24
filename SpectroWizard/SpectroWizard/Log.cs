using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using System.Collections;
//using SpectroWizard.serv;
using System.Drawing;

namespace SpectroWizard
{
    public class Log
    {
        static string EOL = "" + ((char)0xD) + ((char)0xA);
        static string LogText = "Started at "+DateTime.Now+EOL;

        //static LogForm LF = new LogForm();
        static public void Reg(string mls_const, Control contr)
        {
        }

        /*
        #region Registration of controls for user login
        static public void Reg(string mls_const, MenuItem mi)
        {
            return;
            for (int i = 0; i < mi.MenuItems.Count; i++)
                Reg(mls_const,mi.MenuItems[i]);
        }

        static void Mark(Control contr)
        {
            contr.BackColor = System.Drawing.Color.Blue;
            contr.ForeColor = System.Drawing.Color.Red;
        }

        static void SetupToolTip(Control contr, string tool_tip)
        {
            return;
            ToolTip tt = new ToolTip();
            tt.SetToolTip(contr, tool_tip);
            //Mark(contr);
        }

        static public void Reg(string mls_const, MenuStrip ms)
        {
            return;
            for (int i = 0; i < ms.Items.Count; i++)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)ms.Items[i];
                tsi.Click += new EventHandler(tsi_Click);
                for (int j = 0; j < tsi.DropDownItems.Count; j++)
                    if (tsi.DropDownItems[j] is ToolStripMenuItem)
                        Reg(mls_const, (ToolStripMenuItem)tsi.DropDownItems[j]);
            }
        }

        static public void Reg(string mls_const, ToolStripMenuItem mi)
        {
            return;
            mi.Click += new EventHandler(tsi_Click);
            for (int j = 0; j < mi.DropDownItems.Count; j++)
                if (mi.DropDownItems[j] is ToolStripMenuItem)
                    Reg(mls_const, (ToolStripMenuItem)mi.DropDownItems[j]);
        }

        static Hashtable Contrls = new Hashtable();
        static bool IsUnic(Control cont)
        {
            if (Contrls.ContainsKey(cont))
                return false;
            Contrls.Add(cont, true);
            return true;
        }

        static public void Reg(string mls_const,Control contr)
        {
            return;
            if (contr is Form)
            {
                Form c = (Form)contr;
                if (c.Menu != null)
                    for (int i = 0; i < c.Menu.MenuItems.Count; i++)
                        Reg(mls_const, c.Menu.MenuItems[i]);
            }
            if (contr is Panel)
            {
                Panel p = (Panel)contr;
                //if (p.Tag != null)
                //    p = p;
                for (int j = 0; j < p.Controls.Count; j++)
                    Reg(mls_const, p.Controls[j]);
                return;
            }
            if (contr is DataGridView)
            {
                DataGridView dgv = new DataGridView();
                dgv.CellContentClick += new DataGridViewCellEventHandler(dgv_CellContentClick);
                return;
            }
            if (contr is TabPage)
            {
                TabPage tp = (TabPage)contr;
                for (int j = 0; j < tp.Controls.Count; j++)
                    Reg(mls_const, tp.Controls[j]);
                return;
            }
            if (contr is SplitContainer)
            {
                SplitContainer sc = (SplitContainer)contr;
                Reg(mls_const, sc.Panel1);
                Reg(mls_const, sc.Panel2);
                return;
            }
            if (contr is TabControl)
            {
                TabControl tc = (TabControl)contr;
                if (IsUnic(tc))
                {
                    tc.SelectedIndexChanged += new EventHandler(tc_SelectedIndexChanged);
                    for (int j = 0; j < tc.TabPages.Count; j++)
                        Reg(mls_const, tc.TabPages[j]);
                }
                return;
            }
            if (contr is Button)
            {
                Button bt = (Button)contr;
                if (IsUnic(bt))
                    bt.Click += new EventHandler(bt_Click);
                if (bt.Text != null && bt.Text.Length > 0)
                    SetupToolTip(contr, Common.MLS.Get(mls_const, "hint_btn_" + bt.Text));
                else
                {
                    if (bt.Tag != null)
                        SetupToolTip(contr, Common.MLS.Get(mls_const, "hint_btn_" + bt.Tag));
                }
                return;
            }
            if (contr is CheckBox)
            {
                CheckBox chb = (CheckBox)contr;
                if (IsUnic(chb))
                    chb.CheckedChanged += new EventHandler(chb_CheckedChanged);
                SetupToolTip(contr, Common.MLS.Get(mls_const, "hint_chb_" + chb.Text));
                return;
            }
            if (contr is TextBox)
            {
                TextBox tb = (TextBox)contr;
                if (IsUnic(tb))
                    tb.Leave += new EventHandler(tb_Leave);
                if (tb.Tag != null)
                    SetupToolTip(contr, Common.MLS.Get(mls_const, "hint_tb_" + tb.Tag));
                return;
            }
            if (contr is ListBox)
            {
                ListBox lb = (ListBox)contr;
                if (IsUnic(lb))
                {
                    lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);
                    lb.DoubleClick += new EventHandler(lb_DoubleClick);
                }
                return;
            }
            if (contr is TreeView)
            {
                TreeView tv = (TreeView)contr;
                if (IsUnic(tv))
                {
                    tv.AfterSelect += new TreeViewEventHandler(tv_AfterSelect);
                    tv.DoubleClick += new EventHandler(tv_DoubleClick);
                }
                return;
            }
            if (contr is NumericUpDown)
            {
                NumericUpDown nud = (NumericUpDown)contr;
                if (IsUnic(nud))
                {
                    //nud.ValueChanged += new EventHandler(nud_ValueChanged);
                    nud.KeyUp += new KeyEventHandler(nud_KeyUp);
                    nud.Scroll += new ScrollEventHandler(nud_Scroll);
                    nud.Leave += new EventHandler(nud_Leave);
                }
                if (nud.Tag != null)
                    SetupToolTip(contr, Common.MLS.Get(mls_const, "hint_numud_" + nud.Tag));
                return;
            }
            if (contr is CheckedListBox)
            {
                CheckedListBox chbl = (CheckedListBox)contr;
                if (IsUnic(chbl))
                {
                    chbl.SelectedIndexChanged += new EventHandler(chbl_SelectedIndexChanged);
                    chbl.ItemCheck += new ItemCheckEventHandler(chbl_ItemCheck);
                }
                return;
            }
            if (contr is ComboBox)
            {
                ComboBox cmbb = (ComboBox)contr;
                if (IsUnic(cmbb))
                    cmbb.SelectedValueChanged += new EventHandler(cmbb_SelectedValueChanged);
                if (cmbb.Tag != null)
                    SetupToolTip(contr, Common.MLS.Get(mls_const, "hint_cbox_" + cmbb.Tag));
                return;
            }
            if (contr is TrackBar)
            {
                TrackBar trbar = (TrackBar)contr;
                if (IsUnic(trbar))
                    trbar.Scroll += new EventHandler(trbar_Scroll);
                if (trbar.Tag != null)
                    SetupToolTip(contr, Common.MLS.Get(mls_const, "hint_trbar_" + trbar.Tag));
                return;
            }

            for (int i = 0; i < contr.Controls.Count; i++)
                Reg(mls_const,contr.Controls[i]);
        }

        static void nud_Leave(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown bt = (NumericUpDown)sender;
                Common.UserLog((Control)sender, "Value of NumericUpDoen" + bt.Name + " left with value: "+bt.Value);
            }
            catch
            {
            }
        }

        static void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView bt = (DataGridView)sender;
                Common.UserLog((Control)sender, "Table cell click '" + bt.SelectedCells[0].ToString() + "' ");
            }
            catch
            {
            }
        }

        static void nud_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                NumericUpDown bt = (NumericUpDown)sender;
                Common.UserLog((Control)sender, "Value of NumericUpDoen" + bt.Name + "changed!");
            }
            catch
            {
            }
        }

        static void nud_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                NumericUpDown bt = (NumericUpDown)sender;
                Common.UserLog((Control)sender, "Value of NumericUpDoen" + bt.Name + "changed!");
            }
            catch
            {
            }
        }

        static void tc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TabControl tc = (TabControl)sender;
                Common.UserLog((Control)sender, "Tab changed '" + tc.TabPages[tc.SelectedIndex].Text + "'");
            }
            catch
            {
            }
        }

        static void tsi_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripItem tsi = (ToolStripItem)sender;
                Common.UserLog("Menu selected '" + tsi.Text + "'");
            }
            catch
            {
            }
        }

        static void tv_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeView bt = (TreeView)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log", "TreeView double click: ") + bt.SelectedNode.ToString());
                Common.UserLog((Control)sender, "TreeView '"+bt.Name+"' double click: " + bt.SelectedNode.Text);
            }
            catch
            {
            }
        }

        static void trbar_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar bt = (TrackBar)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log", "TrackBar value:") + bt.Value);
                Common.UserLog((Control)sender, "TrackBar '"+bt.Name+"' value:" + bt.Value);
            }
            catch
            {
            }
        }

        static void vscroll_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                VScrollBar bt = (VScrollBar)sender;
                //Common.UserLog((Control)sender, Common.MLS.Get("Log", "Scroll value:") + bt.Value);
                Common.UserLog((Control)sender, "VScroll '" + bt.Name + "' to:" + bt.Value);
            }
            catch
            {
            }
        }

        static void scroll_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                HScrollBar bt = (HScrollBar)sender;
                //Common.UserLog((Control)sender, Common.MLS.Get("Log", "Scroll value:") + bt.Value);
                Common.UserLog((Control)sender, "HScroll '"+bt.Name+"' to:" + bt.Value);
            }
            catch
            {
            }
        }

        static void cmbb_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox bt = (ComboBox)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log", "Select:") + bt.SelectedValue);
                Common.UserLog((Control)sender, "ComboBox '"+bt.Name+"' Select:'" + bt.SelectedValue+"'");
            }
            catch
            {
            }
        }

        static void chbl_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                CheckedListBox bt = (CheckedListBox)sender;
                Common.UserLog((Control)sender, "CheckedListBox'" + bt.Name + "' item: '" + bt.SelectedItem + "' changed state: " + e.NewValue);
            }
            catch
            {
            }
        }

        static void chbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CheckedListBox bt = (CheckedListBox)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log", " Selected: ") + bt.SelectedItem);
                string tmp = "CheckedListBox '" + bt.Name + "' item '" + bt.SelectedItem+"' selected";
                Common.UserLog((Control)sender, tmp);
            }
            catch
            {
            }
        }

        static void lb_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ListBox bt = (ListBox)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log", "Double click value: ")+bt.SelectedItem);
                Common.UserLog((Control)sender, "List box '"+bt.Name+"'double click: " + bt.SelectedItem);
            }
            catch
            {
            }
        }

        static void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeView bt = (TreeView)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log", "TreeView select value: ") + bt.SelectedNode.ToString());
                Common.UserLog((Control)sender, "TreeView '"+bt.Name+"' selected: '" + bt.SelectedNode.Text+"'");
            }
            catch
            {
            }
        }

        static void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListBox bt = (ListBox)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log", "Value changed:") + bt.SelectedItem);
                Common.UserLog((Control)sender, "Selectrion of ListBox '"+bt.Name+"' changed to:" + bt.SelectedItem);
            }
            catch
            {
            }
        }

        static void tb_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox bt = (TextBox)sender;
                //Common.UserLog((Control)sender,Common.MLS.Get("Log","Fucus leave with text:")+bt.Text);
                Common.UserLog((Control)sender, "Leave TextBox '"+bt.Name+"' with text:'" + bt.Text+"'");
            }
            catch
            {
            }
        }

        static void chb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox bt = (CheckBox)sender;
                //Common.UserLog((Control)sender, Common.MLS.Get("Log", " CheckBox State:") + bt.CheckState);
                Common.UserLog((Control)sender," CheckBox '" + bt.Text + "' State:" + bt.CheckState);
            }
            catch
            {
            }
        }

        static void bt_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                //Common.UserLog((Control)sender, Common.MLS.Get("Log", "Click button:") + "'" + bt.Text + "'");
                Common.UserLog((Control)sender, "Click button:" + "'" + bt.Text + "'");
            }
            catch
            {
            }
        }
        #endregion //*/

        static string Path;
        public static void SetupPath(string file_name)
        {
            if (File.Exists(file_name))
                try
                {
                    File.Delete(file_name);
                }
                catch
                {
                }
            Path = Environment.CurrentDirectory + "\\" + file_name;
        }

        public static void Save()
        {
            try
            {
                byte[] buf = Encoding.Default.GetBytes(LogText);
                FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.SetLength(0);
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            catch
            {
            }
        }

        static void Put(string msg)
        {
            DateTime dt = new DateTime(DateTime.Now.Ticks);
            LogText += dt.ToLongTimeString()+".";
            string ms = dt.Millisecond.ToString();
            while (ms.Length < 3) 
                ms = "0" + ms;
            LogText += ms;
            LogText += "  " + msg+EOL;
            if (LogText.Length > 100000)
                LogText = LogText.Substring(LogText.Length - 90000);
        }

        static int ExC = 0;
        public static void OutNoMsg(Exception ex)
        {
            try
            {
                Put(" E " + ex);
                //Common.SetWaitStatus(false);
                ExC++;
                //MainWizardFrom.StateErrors(ExC);
            }
            catch
            {
            }
        }

        public static void Out(Exception ex)
        {
            try
            {
                //LF.Show(ex);
                Put(" E " + ex);
                //Common.SetWaitStatus(false);
                ExC++;
                //MainWizardFrom.StateErrors(ExC);
            }
            catch
            {
            }
        }

        public static void Out(string msg)
        {
            try
            {
                Put(" M " + msg);
            }
            catch
            {
            }
        }

        public static void OutUf(string msg)
        {
            try
            {
                Put(" A " + msg);
            }
            catch
            {
            }
        }
    }
}
