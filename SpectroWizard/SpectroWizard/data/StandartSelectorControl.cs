using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpectroWizard.data
{
    public partial class StandartSelectorControl : UserControl
    {
        public StandartSelectorControl()
        {
            InitializeComponent();
            try
            {
                ReloadTree();
            }
            catch 
            {
            }
        }

        public bool SelectColumn
        {
            set
            {
                if (value)
                    StandartDetails.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                else
                    StandartDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        /*
         * string path = (string)e.Node.Tag;
                if (DataBase.FileExists(ref path) == false)//File.Exists(path) == false)
                {
                    SelectedStName = null;
                    return;
                }
                SelectedStName = path;
                StLib lib = new StLib(path);
         */
        public List<StLib> FullProbSet = new List<StLib>();
        void ReloadTree()
        {
            string selected;
            if (LibTree.SelectedNode != null)
                selected = (string)LibTree.SelectedNode.Tag;
            else
                selected = null;

            LibTree.Nodes.Clear();
            FullProbSet.Clear();
            StandartDetails.Rows.Clear();
            StandartDetails.Columns.Clear();

            TreeNode tn = LibTree.Nodes.Add("Комплекты стандартов");
            tn.Tag = Common.DBBaseStLib;
            
            string[] dirs = Directory.GetDirectories(Common.DBBaseStLib);
            for (int i = 0; i < dirs.Length; i++)
            {
                TreeNode n = LibTree.Nodes.Add(PathToName(dirs[i]));
                n.Tag = dirs[i];
                ReloadTree(dirs[i], n);
            }

            string[] files = Directory.GetFiles(Common.DBBaseStLib);
            for (int i = 0; i < files.Length; i++)
            {
                TreeNode n = LibTree.Nodes.Add(PathToName(files[i]));
                n.Tag = files[i];
                FullProbSet.Add(new StLib(files[i]));
            }

            LibTree.ExpandAll();
            if (selected != null)
                FindSelect(tn.Nodes, selected);
            /*else
                try { StandartDetails[0, 0].Selected = true; }
                catch { }//LibTree_AfterSelect(null, null);*/
        }

        void FindSelect(TreeNodeCollection nodes,string selected)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                TreeNode tn = nodes[i];
                if (tn.Tag.Equals(selected))
                {
                    LibTree.SelectedNode = tn;
                    return;
                }
                FindSelect(tn.Nodes, selected);
            }
        }

        string PathToName(string path)
        {
            string tmp = path.Substring(path.LastIndexOf("\\") + 1);
            return tmp;
        }

        void ReloadTree(string base_path, TreeNode base_node)
        {
            string[] dirs = Directory.GetDirectories(base_path);
            for (int i = 0; i < dirs.Length; i++)
            {
                TreeNode n = base_node.Nodes.Add(PathToName(dirs[i]));
                n.Tag = dirs[i];
                ReloadTree(dirs[i], n);
            }
            string[] files = Directory.GetFiles(base_path, "*.stl");
            for (int i = 0; i < files.Length; i++)
            {
                TreeNode n = base_node.Nodes.Add(PathToName(files[i]));
                n.Tag = files[i];
                FullProbSet.Add(new StLib(files[i]));
            }
        }

        public static bool NeedReloadCons = true;
        private void splitContainer1_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (NeedReloadCons == false)
                    return;
                ReloadTree();
                NeedReloadCons = false;
            }
            catch
            {
            }
        }

        public int StandartCount
        {
            get
            {
                return StandartDetails.RowCount;
            }
        }

        public int ElementCount
        {
            get
            {
                return StandartDetails.ColumnCount;
            }
        }

        public string SelectedStName;
        public int SelectedProb;
        public int SelectedElementNum;
        public string SelectedElementName{
            get
            {
                if (SelectedElementNum < 0)
                    return null;
                return (string)StandartDetails.Columns[SelectedElementNum].HeaderCell.Value;
            }
        }
        public string SelectedProbName
        {
            get
            {
                if (SelectedProb < 0)
                    return null;
                return (string)StandartDetails.Rows[SelectedProb].HeaderCell.Value;
            }
        }
        float[,] Cons;
        bool[,] ConsPrelim;
        List<String> ElementNames;
        public List<String> GetElementList()
        {
            return ElementNames;
        }

        public List<String> GetProbList()
        {
            //StandartDetails.Rows[e.RowIndex].HeaderCell.Value;
            List<String> ret = new List<string>();
            for (int i = 0; i < StandartDetails.Rows.Count; i++)
                ret.Add((string)StandartDetails.Rows[i].HeaderCell.Value);
            return ret;
        }

        private void LibTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string path = (string)e.Node.Tag;
                if (DataBase.FileExists(ref path) == false)//File.Exists(path) == false)
                {
                    SelectedStName = null;
                    return;
                }
                SelectedStName = path;
                StLib lib = new StLib(path);

                ElementNames = new List<string>();

                //StandartDetails.Def
                StandartDetails.Rows.Clear();
                StandartDetails.Columns.Clear();
                cboxElementToCompare.Items.Clear();
                cboxElementToCompare.Items.Add("Все");
                int col_count = 0;
                for (int p = 0; p < lib.Count; p++)
                {
                    StLibStandart st = lib[p];
                    for (int el = 0; el < st.Count; el++)
                    {
                        StLibElement elem = st[el];
                        int col = -1;
                        for(int i = 0;i<StandartDetails.Columns.Count;i++)
                            if (StandartDetails.Columns[i].Name.Equals(elem.Element))
                            {
                                col = i;
                                break;
                            }
                        if (col == -1)
                        {
                            DataGridViewColumn acol = new DataGridViewColumn();
                            acol.SortMode = DataGridViewColumnSortMode.NotSortable;
                            acol.CellTemplate = new DataGridViewTextBoxCell();  
                            StandartDetails.Columns.Add(acol);
                            StandartDetails.Columns[StandartDetails.ColumnCount - 1].Name = elem.Element;
                            col_count ++;
                            ElementNames.Add(elem.Element);
                            cboxElementToCompare.Items.Add(elem.Element);
                        }
                    }
                }

                Cons = new float[lib.Count, col_count];
                ConsPrelim = new bool[lib.Count, col_count];

                for (int p = 0; p < lib.Count; p++)
                {
                    //DataGridViewRow row = new DataGridViewRow();
                    string[] row = new string[StandartDetails.ColumnCount];
                    StLibStandart st = lib[p];
                    for (int el = 0; el < st.Count; el++)
                    {
                        StLibElement elem = st[el];
                        int col = -1;
                        for (int i = 0; i < StandartDetails.Columns.Count; i++)
                            if (StandartDetails.Columns[i].Name.Equals(elem.Element))
                            {
                                col = i;
                                break;
                            }

                        
                        row[col] = elem.Con.ToString();
                        if (elem.IsAproxim)
                            row[col] = "~" + row[col];
                        Cons[p, el] = (float)elem.Con;
                        ConsPrelim[p, el] = elem.IsAproxim;
                    }

                    StandartDetails.Rows.Add(row);
                    StandartDetails.Rows[StandartDetails.Rows.Count - 1].HeaderCell.Value = st[0].StandartName;// StandartDetails.Rows.Count.ToString();
                }

                for (int el = 0; el < StandartDetails.Columns.Count; el++)
                    StandartDetails.Columns[el].SortMode = DataGridViewColumnSortMode.NotSortable;

                SelectedProb = 0;
                //StandartDetails.AutoResizeColumn(0);
                StandartDetails.AutoResizeColumnHeadersHeight();
                StandartDetails.AutoResizeRow(0);
                StandartDetails.AutoResizeRows();
                StandartDetails.AutoResizeColumns();
                StandartDetails.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                try { 
                    //StandartDetails[0, 0].Selected = false;
                    //StandartDetails[0, 0].Selected = true;
                    StandartDetails.ClearSelection();
                }
                catch { }

                RowTranslate = null;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }


        private void StandartDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SelectedProb = e.RowIndex;
                SelectedElementNum = e.ColumnIndex;
                //if (e.ColumnIndex >= 0 && e.ColumnIndex < StandartDetails.Columns.Count)
                //    SelectedElementName = StandartDetails.Columns[e.ColumnIndex].HeaderText;
                //if (e.RowIndex >= 0 && e.RowIndex < StandartDetails.Rows.Count)
                //    SelectedProbName = (string)StandartDetails.Rows[e.RowIndex].HeaderCell.Value;
                checkGraph();
            }
            catch(Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void StandartDetails_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                SelectedProb = e.RowIndex;
                SelectedElementNum = e.ColumnIndex;
                //SelectedElementName = StandartDetails.Columns[e.ColumnIndex].HeaderText;
                //SelectedProbName = (string)StandartDetails.Rows[e.RowIndex].HeaderCell.Value;
                //if (e.ColumnIndex >= 0 && e.ColumnIndex < StandartDetails.Columns.Count)
                //    SelectedElementName = StandartDetails.Columns[e.ColumnIndex].HeaderText;
                //if (e.RowIndex >= 0 && e.RowIndex < StandartDetails.Rows.Count)
                //    SelectedProbName = (string)StandartDetails.Rows[e.RowIndex].HeaderCell.Value;
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        private void StandartDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SelectedProb = e.RowIndex;
                SelectedElementNum = e.ColumnIndex;
                //SelectedElementName = StandartDetails.Columns[e.ColumnIndex].HeaderText;
                //SelectedProbName = (string)StandartDetails.Rows[e.RowIndex].HeaderCell.Value;
                //if (e.ColumnIndex >= 0 && e.ColumnIndex < StandartDetails.Columns.Count)
                //    SelectedElementName = StandartDetails.Columns[e.ColumnIndex].HeaderText;
                //if (e.RowIndex >= 0 && e.RowIndex < StandartDetails.Rows.Count)
                //    SelectedProbName = (string)StandartDetails.Rows[e.RowIndex].HeaderCell.Value;
                checkGraph();
            }
            catch (Exception ex)
            {
                Log.Out(ex);
            }
        }

        int[] RowTranslate;
        public void checkGraph()
        {
            RowTranslate = new int[Cons.GetLength(0)];
            for (int i = 0; i < RowTranslate.Length; i++)
                RowTranslate[i] = i;
            if (SelectedElementNum >= 0)
            {
                for (int pr1 = 0; pr1 < RowTranslate.Length; pr1++)
                {
                    int found = 0;
                    for (int pr2 = 1; pr2 < RowTranslate.Length; pr2++)
                    {
                        int ind1 = RowTranslate[pr2-1];
                        int ind2 = RowTranslate[pr2];
                        if (Cons[ind1, SelectedElementNum] > Cons[ind2, SelectedElementNum])
                        {
                            int tmp = RowTranslate[pr2-1];
                            RowTranslate[pr2 - 1] = RowTranslate[pr2];
                            RowTranslate[pr2] = tmp;
                            found++;
                        }
                    }
                    if (found < 0)
                        break;
                }
            }
            panel1.Invalidate();
        }

        Pen[] ElPens = {   Pens.Black, Pens.Red, Pens.Blue, Pens.Green, 
                           Pens.Gray, Pens.Gold, Pens.GreenYellow, Pens.LightGray, 
                           Pens.HotPink, Pens.IndianRed, Pens.Indigo, Pens.Ivory, 
                           Pens.Khaki, Pens.Lavender, Pens.LawnGreen,Pens.LemonChiffon,
                           Pens.LightBlue,Pens.LightCoral,Pens.LightCyan ,Pens.LightGreen
                       };
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (SelectedElementNum < 0 || RowTranslate == null || Cons == null)
                    return;

                double kx = (double)(panel1.Width - 20) / Cons.GetLength(0);
                int color = 0;
                for (int pt = 0; pt < RowTranslate.Length; pt++)
                {
                    int x = (int)(pt * kx);
                    e.Graphics.DrawString((RowTranslate[pt]+1).ToString(), DefaultFont, Brushes.Black, x, 0);
                }

                for (int el = 0; el < Cons.GetLength(1); el++)
                {
                    if (cboxElementToCompare.SelectedIndex > 0)
                    {
                        if (el != SelectedElementNum && el != cboxElementToCompare.SelectedIndex - 1)
                            continue;
                    }
                    double min = Cons[0, el];
                    double max = min;
                    for (int i = 1; i < RowTranslate.Length; i++)
                    {
                        if (min > Cons[i, el])
                            min = Cons[i, el];
                        if (max < Cons[i, el])
                            max = Cons[i, el];
                    }

                    double ky = (double)(panel1.Height-20) / (max - min);

                    Pen cp;
                    try { cp = ElPens[color]; }
                    catch { cp = Pens.Plum; }
                    color++;
                    SolidBrush sb = new SolidBrush(cp.Color);

                    e.Graphics.DrawString(ElementNames[el], DefaultFont, sb, panel1.Width - 20, (el + 1) * 10);

                    int prev_x = -10000;
                    int prev_y = 0;
                    int psize = 5;
                    for (int pt = 0; pt < RowTranslate.Length; pt++)
                    {
                        int x = (int)(pt * kx);
                        int y = panel1.Height - (int)((Cons[RowTranslate[pt], el] - min) * ky);
                        e.Graphics.DrawLine(cp, x - psize, y + psize, x + psize, y - psize);
                        e.Graphics.DrawLine(cp, x + psize, y + psize, x - psize, y - psize);
                        e.Graphics.DrawLine(cp, prev_x, prev_y, x, y);

                        prev_x = x;
                        prev_y = y;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        private void cboxElementToCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                panel1.Invalidate();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }
    }
}
