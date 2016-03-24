using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace SpectroWizard.method
{
    public partial class MethodSelector : Form
    {
        const string MLSConst = "MethSelector";

        public MethodSelector()
        {
            InitializeComponent();
            //lbDirList.Font = new System.Drawing.Font(FontFamily.GenericMonospace, Common.Env.DefaultFontSize);
        }

        public string Title
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
            }
        }

        string InitialDirectoryPriv = "\\";
        //string CurrentDir = "\\";
        public string InitialDirectory
        {
            get
            {
                return InitialDirectoryPriv;
            }
            set
            {
                InitialDirectoryPriv = (string)value.Clone();
                TreeNode tn = new TreeNode(Common.MLS.Get(MLSConst, "Градуировки"));
                tn.Tag = null;
                tvMethods.Nodes.Clear();
                AddFolder(tn, InitialDirectoryPriv);
                tvMethods.Nodes.Add(tn);
            }
        }

        void AddFolder(TreeNode parent,string folder)
        {
            string[] fld = Directory.GetDirectories(folder,"*.smf");
            for (int i = 0; i < fld.Length; i++)
            {
                string tmp = fld[i].Substring(0,fld[i].Length-4);
                int ind = tmp.LastIndexOf("\\");
                tmp = tmp.Substring(ind+1);
                TreeNode nd = new TreeNode(tmp);
                nd.Tag = null;
                parent.Nodes.Add(nd);
                AddFolder(nd, fld[i]);
            }
            fld = Directory.GetDirectories(folder, "*.sm");
            for (int i = 0; i < fld.Length; i++)
            {
                string tmp = fld[i].Substring(0, fld[i].Length - 3);
                int ind = tmp.LastIndexOf("\\");
                tmp = tmp.Substring(ind + 1);
                TreeNode nd = new TreeNode(tmp);
                nd.Tag = fld[i];
                parent.Nodes.Add(nd);
            }
        }

        string FileNamePriv;
        public string FileName
        {
            get
            {
                return FileNamePriv + "\\method";
            }
        }

        private void btUseMethod_Click(object sender, EventArgs e)
        {
            try
            {
                Visible = false;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void MethodSelector_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if(Visible == false)
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tvMethods_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                btUseMethod.Enabled = (e.Node.Tag != null);
                FileNamePriv = (string)e.Node.Tag;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        private void tvMethods_DoubleClick(object sender, EventArgs e)
        {
            btUseMethod_Click(null, null);
        }
    }
}
