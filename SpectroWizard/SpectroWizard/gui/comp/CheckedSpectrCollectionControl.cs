using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.dev;
using SpectroWizard.data;

namespace SpectroWizard.gui.comp
{
    public delegate void AfterMeasuringDel();
    public class CheckedSpectrCollectionControl
    {
        const string MLSConst = "TTestMeas";

        public SpectrCondition DefaultCondition = null;
        TreeView clSpList;
        SpectrView SpView;
        //Button btMeasuringNewSpectr;
        //Button btReMeasuringSpectr;
        //Button btRemoveSpectr;
        DbFolder Folder;
        public CheckedSpectrCollectionControl(DbFolder folder,TreeView sp_list,//CheckedListBox sp_list,
            SpectrView sp_view,
            ToolStripMenuItem bt_measuring_new_spmi,
            ToolStripMenuItem bt_remeas_spmi,
            ToolStripMenuItem bt_remove_spmi)
        {
            Folder = folder;
            clSpList = sp_list;
            sp_list.CheckBoxes = true;
            //clSpList.SelectedIndexChanged += new EventHandler(clSpList_SelectedIndexChanged);
            clSpList.AfterCheck += new TreeViewEventHandler(clSpList_AfterCheck);
            //clSpList.ItemCheck += new ItemCheckEventHandler(clSpList_ItemCheck);
            clSpList.AfterSelect += new TreeViewEventHandler(clSpList_AfterSelect);
            SpView = sp_view;
            /*if (bt_meas_new_sp != null)
            {
                btMeasuringNewSpectr = bt_meas_new_sp;
                btMeasuringNewSpectr.Click += new EventHandler(btMeasuringNewSpectr_Click);
            }*/
            if (bt_measuring_new_spmi != null)
            {
                //btMeasuringNewSpectr = bt_meas_new_sp;
                bt_measuring_new_spmi.Click += new EventHandler(btMeasuringNewSpectr_Click);
            }
            /*if (bt_remeas_sp != null)
            {
                btReMeasuringSpectr = bt_remeas_sp;
                btReMeasuringSpectr.Click += new EventHandler(btReMeasuringSpectr_Click);
            }*/
            if (bt_remeas_spmi != null) 
            {
                //btReMeasuringSpectr = bt_rem eas_sp;
                bt_remeas_spmi.Click += new EventHandler(btReMeasuringSpectr_Click);
            }
            /*if (bt_remove != null)
            {
                btRemoveSpectr = bt_remove;
                btRemoveSpectr.Click += new EventHandler(btRemoveSpectr_Click);
            }*/
            if (bt_remove_spmi != null)
            {
                bt_remove_spmi.Click += new EventHandler(btRemoveSpectr_Click);
            }
            try
            {
                ReLoadList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }

            ContextMenu cm = new ContextMenu();
            //cm.MenuItems;
            MenuItem mi = new MenuItem(Common.MLS.Get(MLSConst, "Измерить новый спектр"));
            mi.Click += new EventHandler(btMeasuringNewSpectr_Click);
            cm.MenuItems.Add(mi);
            mi = new MenuItem(Common.MLS.Get(MLSConst, "Перемерить спектр"));
            mi.Click += new EventHandler(btReMeasuringSpectr_Click);
            cm.MenuItems.Add(mi);
            mi = new MenuItem(Common.MLS.Get(MLSConst, "Удалить спектр"));
            mi.Click += new EventHandler(btRemoveSpectr_Click);
            cm.MenuItems.Add(mi);

            cm.MenuItems.Add("-");

            mi = new MenuItem(Common.MLS.Get(MLSConst, "Создать копию спектра"));
            mi.Click += new EventHandler(miCopySpectr_Click);
            cm.MenuItems.Add(mi);

            cm.MenuItems.Add("-");
            mi = new MenuItem(Common.MLS.Get(MLSConst, "Создать папку"));
            mi.Click += new EventHandler(miAddFolder_Click);
            cm.MenuItems.Add(mi);
            mi = new MenuItem(Common.MLS.Get(MLSConst, "Удалить папку"));
            mi.Click += new EventHandler(miRemoveFolder_Click);
            cm.MenuItems.Add(mi);
            
            clSpList.ContextMenu = cm;

            clSpList.MouseUp += new MouseEventHandler(clSpList_MouseUp);
            clSpList.ImageList = MainForm.MForm.liCheckedTreeImages;
        }

        void SelectPath(string path)
        {
            //clSpList.SelectedNode.IsSelected = false;
        }

        void miCopySpectr_Click(object sender, EventArgs e)
        {
            try
            {
                if (Selected == null)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выберите спектр который нужно перемерить"),
                        Common.MLS.Get(MLSConst, "Предупреждение"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                    return;
                }

                LastPath = SelectedTag.GetPath();

                /*DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Вы действительно хотите перемерить спектр: ") + LastPath,
                        Common.MLS.Get(MLSConst, "Осторожно!!!"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (dr == DialogResult.No)
                    return;*/

                string name = util.StringDialog.GetString(MainForm.MForm,
                                Common.MLS.Get(MLSConst, "Копирование пробы"),
                                Common.MLS.Get(MLSConst, "Введите имя для копии пробы:") + LastPath,
                                "", true);

                if (name == null)
                    return;

                Spectr sp = new Spectr(LastPath);
                int path_ind = LastPath.LastIndexOf('\\');
                string path = LastPath.Substring(0,path_ind+1);
                sp.SaveAs(path + name);
                ReLoadList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void miAddFolder_Click(object sender, EventArgs e)
        {
            try
            {
                DbFolder curf;
                if (Selected == null)
                    curf = Folder;
                else
                    curf = ((CSTreeNodeTag)Selected.Tag).Folder;
                string name = util.StringDialog.GetString(MainForm.MForm,
                                    Common.MLS.Get(MLSConst, "Создание папки"),
                                    Common.MLS.Get(MLSConst, "Введите имя новой папки в подкаталоге '")+curf.GetPath()+"'",
                                    "", true);
                if (name == null)
                    return;
                curf.CreateFolder(name);
                ReLoadList();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        void miRemoveFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedTag == null || SelectedTag.SpName != null)
                    return;
                if (Selected.Nodes.Count != 0)
                    return;
                DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Вы действительно хотите удалить папку: ") + SelectedTag.GetPath(),
                        Common.MLS.Get(MLSConst, "Осторожно!!!"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (dr == DialogResult.No)
                    return;

                SelectedTag.Folder.DeleteFolder();

                ReLoadList();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void clSpList_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                    clSpList.ContextMenu.Show(clSpList, e.Location);
                    //clSpList.ContextMenu.Show(clSpList, e.Location);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        TreeNode Selected;
        CSTreeNodeTag SelectedTag;
        void clSpList_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                clSpList_ItemCheck(null, null);
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void clSpList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                Selected = e.Node;
                SelectedTag = (CSTreeNodeTag)e.Node.Tag;
                clSpList_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        public AfterMeasuringDel AfterMeasuringProc;
        
        public string FirstFileName = null;
        bool ReLoading = false;
        delegate void ReLoadListDel();
        void ReLoadList()
        {
            try
            {
                ReLoadListDel del = new ReLoadListDel(ReLoadListDelProc);
                MainForm.MForm.Invoke(del);
            }
            catch
            {
                ReLoadListDelProc();
            }
        }

        //List<TreeNode> clSpListItems = new List<TreeNode>();
        List<TreeNode> GetItemsList()
        {
            List<TreeNode> ret = new List<TreeNode>();

            for (int i = 0; i < clSpList.Nodes.Count; i++)
                if (clSpList.Nodes[i].Nodes.Count == 0)
                    ret.Add(clSpList.Nodes[i]);
                else
                    GetSubItems(ref ret, clSpList.Nodes[i]);
            return ret;
        }

        void GetSubItems(ref List<TreeNode> ret, TreeNode node)
        {
            ret.Add(node);
            for (int i = 0; i < node.Nodes.Count; i++)
                if (node.Nodes[i].Nodes.Count == 0)
                    ret.Add(node.Nodes[i]);
                else
                    GetSubItems(ref ret, node.Nodes[i]);
        }//*/

        void UpdateNodeList(TreeNodeCollection nodes, DbFolder folder)
        {
            string[] fnames = folder.GetFolderList();
            for (int i = 0; i < fnames.Length; i++)
            {
                TreeNode cur_node = null;
                for(int j = 0;j<nodes.Count;j++)
                    if (((CSTreeNodeTag)nodes[j].Tag).SpName == null && nodes[j].Text.Equals(fnames[i]))
                    {
                        cur_node = nodes[i];
                        break;
                    }
                if (cur_node == null)
                    cur_node = nodes.Add(fnames[i]);
                cur_node.Tag = new CSTreeNodeTag(new DbFolder(fnames[i],folder), null);
                cur_node.SelectedImageIndex = 1;
                cur_node.StateImageIndex = 0;
                UpdateNodeList(cur_node.Nodes,new DbFolder(fnames[i],folder));
            }

            string[] names = folder.GetRecordList("ss",true);
            //for (int i = 0; i < names.Length; i++)
            //    names[i] = names[i].Substring(0, names[i].Length - 3);
            for (int i = 0; i < names.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < nodes.Count; j++)
                    if (((CSTreeNodeTag)nodes[j].Tag).SpName != null && nodes[j].Text.Equals(names[i]))
                    {
                        found = true;
                        break;
                    }
                if (found == false)
                {
                    TreeNode nd = nodes.Add(names[i]);
                    nd.Tag = new CSTreeNodeTag(folder, names[i]);
                    nd.SelectedImageIndex = 3;
                    nd.ImageIndex = 2;
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < fnames.Length; j++)
                {
                    if (fnames[j].Equals(nodes[i].Text) == true)
                    {
                        found = true;
                        break;
                    }
                }
                for (int j = 0; found == false && j < names.Length; j++)
                {
                    if (names[j].Equals(nodes[i].Text) == true)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    nodes.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ReLoadListDelProc()
        {
            ReLoading = true;
            List<TreeNode> checked_items = new List<TreeNode>();
            List<TreeNode> items = GetItemsList();
            for (int i = 0; i < items.Count; i++)
                if (items[i].Checked)
                    checked_items.Add(items[i]);

            //string selected_item = (string)clSpList.SelectedItem;

            if (clSpList.Nodes.Count == 0)
            {
                TreeNode nd = clSpList.Nodes.Add(Common.MLS.Get(MLSConst, "Измерения"));
                nd.Tag = new CSTreeNodeTag(Folder, null);
                nd.SelectedImageIndex = 1;
                nd.ImageIndex = 0;
            }
            UpdateNodeList(clSpList.Nodes[0].Nodes, Folder);
            /*string[] names = Folder.GetRecordList("ss");

            for (int i = 0; i < names.Length; i++)
            {
                string tmp = names[i];
                int j = tmp.LastIndexOf(".ss");
                if (j > 0)
                    tmp = tmp.Substring(0, j);
                if (i < clSpList.Items.Count)
                    clSpList.Items[i] = tmp;
                else
                    clSpList.Items.Add(tmp);
            }

            while (clSpList.Items.Count > names.Length)
                clSpList.Items.RemoveAt(clSpList.Items.Count - 1);*/

            items = GetItemsList();
            for (int i = 0; i < items.Count; i++)
            {
                //bool found = false;
                for (int j = 0; j < checked_items.Count; j++)
                    if (checked_items[j].FullPath.Equals(items[i].FullPath))
                    {
                        //found = true;
                        items[j].Checked = true;
                        break;
                    }
                //clSpList.SetItemChecked(i, found);
            }

            ReLoading = false;

            SelectSpectr();
        }

        public void SelectSpectr()
        {
            clSpList_SelectedIndexChanged(null, null);
            //if(LastName != null)
            //    SelectSpectr(LastName);
        }

        /*delegate void SelectSpectrDel(string name);
        public void SelectSpectr(string name)
        {
            try
            {
                object[] args = { name };
                SelectSpectrDel del = new SelectSpectrDel(SelectSpectrDelProc);
                MainForm.MForm.Invoke(del, args);
            }
            catch
            {
                SelectSpectrDelProc(name);
            }
        }

        public void SelectSpectrDelProc(string path)
        {
            
        }//*/

        delegate Spectr GetSelectedSpectrDel();
        public Spectr GetSelectedSpectrCT()
        {
            GetSelectedSpectrDel del = new GetSelectedSpectrDel(GetSelectedSpectr);
            return (Spectr)MainForm.MForm.Invoke(del);
        }

        public Spectr GetSelectedSpectr()
        {
            if (Selected == null)
                return null;
            //string path = Folder.CreateRecordPath((string)clSpList.Items[clSpList.SelectedIndex]);
            //string path = (string)clSpList.Items[clSpList.SelectedIndex];
            //Spectr sp = new Spectr(Folder, path);
            return ((CSTreeNodeTag)Selected.Tag).GetSpectr();//sp;
        }

        string LastPath;
        CSTreeNodeTag CurTag;
        public void btMeasuringNewSpectr_Click(object sender, EventArgs e)
        {
            try
            {
                string name = null;
                SpectrCondition spc;
                if(FirstFileName != null && clSpList.Nodes[0].Nodes.Count == 0)
                {
                    name = FirstFileName;
                    spc = null;
                }
                else
                {
                    name = util.StringDialog.GetString(MainForm.MForm,
                                Common.MLS.Get(MLSConst, "Создание пробы"),
                                Common.MLS.Get(MLSConst, "Введите имя новой пробы"),
                                "", true);
                }

                if (name == null)
                    return;

                //DbFolder dbf;
                //CSTreeNodeTag selected;
                if (Selected == null)
                    CurTag = new CSTreeNodeTag(Folder, name);
                else
                    CurTag = new CSTreeNodeTag(((CSTreeNodeTag)(Selected.Tag)).Folder,name);
                //List<TreeNode> items = GetItemsList();
                //for (int i = 0; i < clSpList.Items.Count;i++ )
                if (CurTag.IsSpectrExists(name))//clSpList.Items[i].Equals(name))
                {
                    MessageBox.Show(MainForm.MForm, 
                            Common.MLS.Get(MLSConst,"Проба с таким именнем уже сужествует."), 
                            Common.MLS.Get(MLSConst,"Предупреждение"),
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                LastPath = CurTag.GetPath();

                if (FirstFileName != null && clSpList.Nodes[0].Nodes.Count != 0)
                {
                    //string path = selected.Folder.CreateRecordPath(name);
                    Spectr sp = new Spectr(CurTag.Folder, FirstFileName);
                    spc = sp.GetMeasuringCondition();
                }
                else
                    spc = null;

                SpectrCondition cond;
                if (DefaultCondition == null)
                {
                    cond = SpectrCondEditor.GetCond(MainForm.MForm, spc);
                    if (cond == null)
                        return;
                }
                else
                    cond = DefaultCondition;

                //Spectr sp = new Spectr(cond,Common.Env.DefaultDisp,Common.Env.DefaultOpticFk);
                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btMeasuringNewSpectr_Click_Final);
                Common.Dev.Measuring(cond, final_call);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }


        private void btMeasuringNewSpectr_Click_Final(List<SpectrDataView> rez, SpectrCondition cond)
        {
            try
            {
                //string sel = (string)LastName.Clone();
                Spectr sp = new Spectr(cond, Common.Env.DefaultDisp,Common.Env.DefaultOpticFk);
                for (int i = 0; i < rez.Count; i++)
                    sp.Add(rez[i]);

                //string path = Folder.CreateRecordPath(sel);

                sp.SaveAs(CurTag.GetPath());
                ReLoadList();
                //SelectSpectr(sel);
                if (AfterMeasuringProc != null)
                    AfterMeasuringProc();

                //SpView.ReDrawNow();
                //SpView.Invalidate();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void clSpList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ReLoading)
                    return;
                SpView.ClearSpectrList();
                List<TreeNode> list = GetItemsList();
                List<TreeNode> selected = new List<TreeNode>();
                if (Selected != null)
                    selected.Add(Selected);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Checked && list[i].FullPath.Equals(Selected.FullPath) == false)
                        selected.Add(list[i]);
                }
                for (int i = 0; i < selected.Count; i++)
                {
                    CSTreeNodeTag tag = (CSTreeNodeTag)selected[i].Tag;
                    Spectr sp = tag.GetSpectr();
                    if(sp != null)
                        SpView.AddSpectr(sp, selected[i].Text);
                }
                /*LastName = (string)clSpList.SelectedItem;
                if (LastName != null)
                {
                    Spectr sp = new Spectr(Folder, LastName);
                    SpView.AddSpectr(sp, (string)clSpList.SelectedItem);
                    for (int i = 0; i < clSpList.Items.Count; i++)
                    {
                        if (clSpList.GetItemChecked(i) &&
                            clSpList.Items[i] != clSpList.SelectedItem)
                        {
                            sp = new Spectr(Folder, (string)clSpList.Items[i]);
                            SpView.AddSpectr(sp, (string)clSpList.Items[i]);
                        }
                    }
                }*/
                SpView.ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        Spectr CurSp;
        public void btReMeasuringSpectr_Click(object sender, EventArgs e)
        {
            try
            {
                if (Selected == null)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выберите спектр который нужно перемерить"),
                        Common.MLS.Get(MLSConst, "Предупреждение"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                    return;
                }
                
                //LastName = (string)Selected.Text;
                LastPath = SelectedTag.GetPath();
                //LastName

                DialogResult dr = MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Вы действительно хотите перемерить спектр: ")+LastPath,
                        Common.MLS.Get(MLSConst, "Осторожно!!!"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (dr == DialogResult.No)
                    return;

                CurSp = new Spectr(LastPath);//(Folder, LastName);

                SpectrCondition cond;
                if (DefaultCondition == null)
                {
                    SpectrCondition spc = CurSp.GetMeasuringCondition();
                    cond = SpectrCondEditor.GetCond(MainForm.MForm, spc);
                    if (cond == null)
                        return;
                }
                else
                    cond = DefaultCondition;

                Dev.MeasuringResultFinalCall final_call = new Dev.MeasuringResultFinalCall(btReMeasuringSpectr_Click_Final);
                Common.Dev.Measuring(cond, final_call);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void btReMeasuringSpectr_Click_Final(List<SpectrDataView> rez, SpectrCondition cond)
        {
            try
            {
                Spectr sp = CurSp;// new Spectr(Folder, LastName);

                sp.SetMeasuringCondition(cond);

                sp.Clear();
                for (int i = 0; i < rez.Count; i++)
                    sp.Add(rez[i]);

                sp.SetDispers(Common.Env.DefaultDisp);
                sp.OFk = Common.Env.DefaultOpticFk;

                sp.Save();
                ReLoadList();
                
                if (AfterMeasuringProc != null)
                    AfterMeasuringProc();
                
                SpView.ReDrawNow();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void clSpList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            clSpList_SelectedIndexChanged(sender, null);
        }

        public void btRemoveSpectr_Click(object sender, EventArgs e)
        {
            try
            {
                if (Selected == null)
                {
                    MessageBox.Show(MainForm.MForm,
                        Common.MLS.Get(MLSConst, "Выберите спектр который нужно перемерить"),
                        Common.MLS.Get(MLSConst, "Предупреждение"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                    return;
                }

                //string selected = (string)clSpList.SelectedItem;
                LastPath = SelectedTag.GetPath();

                DialogResult dr = MessageBox.Show(MainForm.MForm, 
                    Common.MLS.Get(MLSConst,"Удалить выбранный спектр:'")+LastPath+"'?",
                    Common.MLS.Get(MLSConst, "Осторожно!!!"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (dr == DialogResult.No)
                    return;

                //string path = Folder.GetRecordPath(selected);
                Spectr.RemoveSpectr(LastPath);
                ReLoadList();
                //clSpList.SelectedIndex = -1;
                SpView.ClearSpectrList();
                SpView.ReDraw();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }

    class CSTreeNodeTag
    {
        public DbFolder Folder;
        public string SpName;
        public CSTreeNodeTag(DbFolder folder,string sp_name)
        {
            SpName = sp_name;
            Folder = folder;
        }

        public Spectr GetSpectr()
        {
            string path = Folder.GetPath() + SpName;
            if(Spectr.IsFileExists(path))
                return new Spectr(path);
            return null;
        }

        public bool IsSpectrExists(string name)
        {
            string path = Folder.GetPath() + name;
            return Spectr.IsFileExists(path);
        }

        public string GetPath()
        {
            string path = Folder.GetPath() + SpName;
            return path;
        }
    }
}
