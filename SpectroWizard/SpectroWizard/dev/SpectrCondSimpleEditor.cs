using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpectroWizard.data;

namespace SpectroWizard.dev
{
    public partial class SpectrCondSimpleEditor : UserControl
    {
        const string MLSConst = "SpCondSE";
        public SpectrCondSimpleEditor()
        {
            InitializeComponent();
        }

        public SpectrCondEditor Editor;
        SpectrCondition Cond
        {
            get
            {
                return Editor.Cond;
            }
        }

        public void SetupHelpText(string txt)
        {
            if (txt == null)
                txt = "";
            lbHelp.Text = txt;
        }

        public string SavedText;
        public void Save()
        {
            //SavedText = 
            //string txt = "";
            SavedText = "";
            for (int i = 0; i < Lines.Count; i++)
                SavedText += Lines[i].ToString();
            Cond.Compile(SavedText);
            Editor.ReInitResultWindow(Cond);
        }

        public void AddAfter(int line)
        {
            SpCondSENewLineTypeSelector sel = new SpCondSENewLineTypeSelector();
            DialogResult dr = sel.ShowDialog(Editor);
            if (dr != DialogResult.OK)
                return;
            string code = "";
            SpectrCondition.CondTypes type = SpectrCondition.CondTypes.Comment;
            switch (sel.LineType)
            {
                case 0:
                    code = "#";
                    break;
                case 1:
                    code = "p: 1 On()";
                    type = SpectrCondition.CondTypes.Prespark;
                    break;
                case 2:
                    if (Cond.Lines[line].Type == SpectrCondition.CondTypes.Exposition)
                    {
                        code = Cond.Lines[line].SourceCode;
                        int on_index = code.ToLower().IndexOf("on");
                        int off_index = code.ToLower().IndexOf("off");
                        if (off_index > 0)
                            code = code.Substring(0,off_index) + "on()";
                    }
                    else
                    {
                        code = "e: 1 (";
                        int[] ss = Common.Dev.Reg.GetSensorSizes();
                        for (int i = 0; i < ss.Length; i++)
                        {
                            code += "0.1";
                            if (i < ss.Length - 1)
                                code += ";";
                        }
                        code += ") On()";
                    }
                    type = SpectrCondition.CondTypes.Exposition;
                    break;
                case 3:
                    if (Cond.Lines[line].Type == SpectrCondition.CondTypes.Exposition)
                    {
                        code = Cond.Lines[line].SourceCode;
                        int on_index = code.ToLower().IndexOf("on");
                        int off_index = code.ToLower().IndexOf("off");
                        if (on_index > 0)
                            code = code.Substring(0,on_index)+"off()";
                    }
                    else
                    {
                        code = "e: 1 (";
                        int[] ss = Common.Dev.Reg.GetSensorSizes();
                        for (int i = 0; i < ss.Length; i++)
                        {
                            code += "0.1";
                            if (i < ss.Length - 1)
                                code += ";";
                        }
                        code += ") Off()";
                    }
                    type = SpectrCondition.CondTypes.Exposition;
                    break;
                case 4:
                    code = "f:Off";
                    type = SpectrCondition.CondTypes.FillLight;
                    break;
            }
            line ++;
            bool fl = false;
            SpectrConditionCompiledLine cond =
                new SpectrConditionCompiledLine(type, code, line, 0, ref fl);
            if(line < Lines.Count)
                Lines.Insert(line,new SpCondLineEditor(cond,this,line));
            else
                Lines.Add(new SpCondLineEditor(cond, this,line));
            Save();
            ReInitList();
        }

        public void DeleteAt(int line)
        {
            DialogResult dr = MessageBox.Show(Editor,
                Common.MLS.Get(MLSConst,"Удалить комманду?")+" '"+Cond.Lines[line].SourceCode+"'",
                Common.MLS.Get(MLSConst,"Удаление"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Hand);
            if (dr != DialogResult.Yes)
                return;
            Lines.RemoveAt(line);
            Save();
            ReInitList();
        }

        List<SpCondLineEditor> Lines = new List<SpCondLineEditor>();
        int LineHeight = 15;
        public void Setup(string source_code, SpectrCondEditor editor)
        {
            Editor = editor;
            Editor.Cond.Compile(source_code);
            Editor.ReInitResultWindow(Editor.Cond);
            ReInitList();
        }

        void ReInitList()
        {
            Lines.Clear();
            for (int i = 0; i < Editor.Cond.Lines.Count; i++)
                Lines.Add(new SpCondLineEditor(Editor.Cond.Lines[i],this,i));
            MainScrollPanel.Controls.Clear();
            LineHeight = SpCondLineEditor.NeedHeight;
            for (int l = 0; l < Lines.Count; l++)
            {
                MainScrollPanel.Controls.Add(Lines[l]);
                Lines[l].SetBounds(0, l * LineHeight, Lines[l].NWidth, LineHeight);
            }
        }

        private void SpectrCondSimpleEditor_SizeChanged(object sender, EventArgs e)
        {
        }
    }

    class SpCondLineEditor : Panel
    {
        public static int NeedHeight
        {
            get
            {
                return (int)(Common.Env.DefaultFontSize*2);
            }
        }
        const string MLSConst = "SpCLE";
        SpectrConditionCompiledLine Line;
        public int NWidth = 20;
        //int EPrefixSzie = 20;
        //int ESufixSize = 20;
        object[] Fields;
        int[] XFrom, XTo;
        enum FTypes
        {
            String,
            CommonTime,
            Exposition,
            GeneratorState,
            OnOffState
        }
        FTypes[] FieldTypes;
        string[] FieldHelps;
        string GenOnStr;
        string GenOffStr;
        string OnStr;
        string OffStr;
        SpectrCondSimpleEditor Editor;
        int LineIndex;
        TextBox ActiveText;
        public SpCondLineEditor(SpectrConditionCompiledLine line,
            SpectrCondSimpleEditor editor,int line_index)
        {
            Editor = editor;
            LineIndex = line_index;

            GenOnStr = Common.MLS.Get(MLSConst, "Спектр");
            GenOffStr = Common.MLS.Get(MLSConst, "Ноль...");
            OnStr = Common.MLS.Get(MLSConst, "Вкл");
            OffStr = Common.MLS.Get(MLSConst, "Выкл");

            Line = line;
            BackColor = SystemColors.ControlDark;
            switch (Line.Type)
            {
                default: 
                    Fields = new object[1];
                    Fields[0] = Line.SourceCode;
                    FieldHelps = new string[1];
                    FieldHelps[0] = Common.MLS.Get(MLSConst, "Это просто текст");
                    FieldTypes = new FTypes[1];
                    FieldTypes[0] = FTypes.String;
                    break;
                case SpectrCondition.CondTypes.Prespark:
                    Fields = new object[1];
                    Fields[0] = Line.CommonTime;
                    FieldHelps = new string[1];
                    FieldHelps[0] = Common.MLS.Get(MLSConst, "Время обжига образца без регитрации спектра.");
                    FieldTypes = new FTypes[1];
                    FieldTypes[0] = FTypes.CommonTime;
                    break;
                case SpectrCondition.CondTypes.Exposition:
                    Fields = new object[2 + Line.Expositions.Length];
                    FieldTypes = new FTypes[Fields.Length];
                    FieldHelps = new string[Fields.Length];
                    FieldHelps[0] = Common.MLS.Get(MLSConst, "Общее время измерения сперктра.");
                    FieldTypes[0] = FTypes.CommonTime;
                    Fields[0] = Line.CommonTime;
                    for (int i = 0; i < Line.Expositions.Length; i++)
                    {
                        Fields[1 + i] = Line.Expositions[i];
                        FieldTypes[1 + i] = FTypes.Exposition;
                        FieldHelps[1 + i] = Common.MLS.Get(MLSConst, "Длительность экспонирования линейки №")+(i+1);
                    }
                    Fields[1 + Line.Expositions.Length] = Line.IsGenOn;
                    FieldTypes[1 + Line.Expositions.Length] = FTypes.GeneratorState;
                    FieldHelps[1 + Line.Expositions.Length] = Common.MLS.Get(MLSConst, "Экспонируется линейка или калибруются нулевой уровень. Что-бы изменить нажмите клавишу ВВЕРХ/ВНИЗ.");
                    break;
                case SpectrCondition.CondTypes.FillLight:
                    Fields = new object[1];
                    Fields[0] = Line.IsFillLight;
                    FieldHelps = new string[1];
                    FieldHelps[0] = Common.MLS.Get(MLSConst, "Состояние источника заливающего света");
                    FieldTypes = new FTypes[1];
                    FieldTypes[0] = FTypes.OnOffState;
                    break;
            }
            XFrom = new int[Fields.Length];
            XTo = new int[Fields.Length];
            Paint += new PaintEventHandler(SpCondLineEditor_Paint);
            //Validating += new CancelEventHandler(SpCondLineEditor_Validating);
            MouseUp += new MouseEventHandler(SpCondLineEditor_MouseUp);
            ActiveText = new TextBox();
            Controls.Add(ActiveText);
            ActiveText.SetBounds(-100, -100, 10, 10);
            ActiveText.TextChanged += new EventHandler(ActiveText_TextChanged);
            ActiveText.KeyUp += new KeyEventHandler(ActiveText_KeyUp);
            //PreviewKeyDown += new PreviewKeyDownEventHandler(SpCondLineEditor_PreviewKeyDown);
            //Leave += new EventHandler(SpCondLineEditor_Leave);
            ActiveText.Leave += new EventHandler(ActiveText_Leave);
        }

        void ActiveText_Leave(object sender, EventArgs e)
        {
            try
            {
                SaveEditedValue(true);
                SelectedField = -1;
                Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void ActiveText_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SaveEditedValue(true);
                    e.Handled = true;
                }
                else
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        SaveEditedValue(false);
                        e.Handled = true;
                    }
                    else
                    {
                        if (SelectedField < 0)
                            return;
                        if (FieldTypes[SelectedField] == FTypes.GeneratorState)
                        {
                            if (ActiveText.Text.Equals(GenOnStr))
                                ActiveText.Text = GenOffStr;
                            else
                                ActiveText.Text = GenOnStr;
                            Editor.Editor.DisableExit();
                        }
                        else
                        {
                            if (FieldTypes[SelectedField] == FTypes.OnOffState)
                            {
                                if (ActiveText.Text.Equals(OnStr))
                                    ActiveText.Text = OffStr;
                                else
                                    ActiveText.Text = OnStr;
                                Editor.Editor.DisableExit();
                            }
                            else
                                ActiveTextTyped = true;
                        }
                        Editor.Editor.DisableExit();
                    }
                }
                Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        bool ActiveTextTyped = false;
        void ActiveText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ActiveTextTyped == false)
                    return;
                ActiveTextTyped = false;
                Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void SaveEditedValue(bool save)
        {
            if (SelectedField < 0 ||
                SelectedField >= FieldTypes.Length)
                return;
            try
            {
                ActiveText.ReadOnly = false;
                if (save)
                {
                    switch (FieldTypes[SelectedField])
                    {
                        case FTypes.String:
                            Fields[SelectedField] = ActiveText.Text;
                            break;
                        case FTypes.Exposition:
                            Fields[SelectedField] = (float)serv.ParseDouble(ActiveText.Text);
                            break;
                        case FTypes.CommonTime:
                            Fields[SelectedField] = (float)serv.ParseDouble(ActiveText.Text);
                            break;
                        case FTypes.GeneratorState:
                            ActiveText.ReadOnly = true;
                            if (ActiveText.Text.Equals(GenOnStr))
                                Fields[SelectedField] = true;
                            else
                                Fields[SelectedField] = false;
                            break;
                        case FTypes.OnOffState:
                            ActiveText.ReadOnly = true;
                            if (ActiveText.Text.Equals(OnStr))
                                Fields[SelectedField] = true;
                            else
                                Fields[SelectedField] = false;
                            break;
                    }
                }
                SelectedField++;
                if (SelectedField >= FieldTypes.Length)
                    SelectedField = 0;
                SetActiveField(SelectedField);
                Editor.Save();
                return;
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
            finally
            {
                System.Media.SystemSounds.Beep.Play();
            }
        }

        int SelectedField = -2;
        void CheckMouseUp(int x)
        {
            if (SelectedField >= 0)
                SaveEditedValue(true);
            SelectedField = -1;
            for (int i = 0; i < Fields.Length; i++)
            {
                if (XFrom[i] < x && x < XTo[i])
                {
                    //SelectedField = i;
                    SetActiveField(i);
                    Refresh();
                    return;
                }
            }

            if (x > AddBtnX)
            {
                Editor.AddAfter(LineIndex);
                return;
            }
            if (x > DeleteBtnX)
            {
                Editor.DeleteAt(LineIndex);
                return;
            }
        }
        int LatestMouseUpX = 0;
        void SpCondLineEditor_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                LatestMouseUpX = e.X;
                CheckMouseUp(e.X);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        void SetActiveField()
        {
            CheckMouseUp(LatestMouseUpX);
        }

        void SetActiveField(int fld)
        {
            if (ActiveText.Focus() == false)
            {
                bool gainded = false;
                for (int i = 0; i < 10; i++)
                    if (ActiveText.Focus() == true)
                    {
                        gainded = true;
                        break;
                    }
                    else
                        System.Threading.Thread.Sleep(1);
                if(gainded == false)
                    return;// throw new Exception("Can't gain focus");
            }
            SelectedField = fld;
            Editor.SetupHelpText(FieldHelps[fld]);
            if (fld < 0 || fld >= Fields.Length)
                return;
            ActiveTextTyped = false;
            switch (FieldTypes[fld])
            {
                case FTypes.GeneratorState:
                    if ((bool)Fields[fld])
                        ActiveText.Text = GenOnStr;
                    else
                        ActiveText.Text = GenOffStr;
                    break;
                case FTypes.OnOffState:
                    if ((bool)Fields[fld])
                        ActiveText.Text = OnStr;
                    else
                        ActiveText.Text = OffStr;
                    break;
                default:
                    ActiveText.Text = Fields[fld].ToString();
                    break;
            }
            ActiveText.SelectionStart = 0;
            ActiveText.SelectionLength = ActiveText.Text.Length;
        }

        void SpCondLineEditor_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = false;
                Refresh();
            }
            catch (Exception ex)
            {
                Common.Log(ex);
            }
        }

        int DeleteBtnX;
        int AddBtnX;
        Font F = new Font(FontFamily.GenericSerif, Common.Env.DefaultFontSize);
        Font FB = new Font(FontFamily.GenericSerif, Common.Env.DefaultFontSize, FontStyle.Bold);
        void SpCondLineEditor_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Size s = Size;
                g.FillRectangle(Brushes.White,0,0,s.Width,s.Height);
                string type = ""+(LineIndex+1)+". ";
                switch (Line.Type)
                {
                    default: type += Common.MLS.Get(MLSConst, "Неиз"); break;
                    case SpectrCondition.CondTypes.Comment: type += Common.MLS.Get(MLSConst, "#"); break;
                    case SpectrCondition.CondTypes.Prespark: type += Common.MLS.Get(MLSConst, "Обж."); break;
                    case SpectrCondition.CondTypes.Exposition: type += Common.MLS.Get(MLSConst, "Эксп."); break;
                    case SpectrCondition.CondTypes.FillLight: type += Common.MLS.Get(MLSConst, "Зал.Св."); break;
                }
                Font f = F;
                int x = 1;
                int y = 2;
                Brush br;
                string to_print;
                g.DrawString(type, f, Brushes.Black, x, y);
                x += (int)g.MeasureString(type, f).Width;
                x += 2;
                for (int i = 0; i < Fields.Length; i++)
                {
                    f = F;
                    int from_x = x;
                    int dlt_x = 1;
                    switch (FieldTypes[i])
                    {
                        default:
                            br = Brushes.Gray;
                            to_print = Fields[i].ToString();
                            break;
                        case FTypes.CommonTime:
                            br = Brushes.Blue;
                            to_print = Math.Round((float)Fields[i],1).ToString();
                            f = FB;
                            dlt_x = 10;
                            break;
                        case FTypes.Exposition:
                            if((i%2) == 0)
                                br = Brushes.Black;
                            else
                                br = Brushes.Gray;
                            to_print = serv.GetGoodValue((float)Fields[i], 2).ToString();
                            break;
                        case FTypes.GeneratorState:
                            x += 4;
                            if ((bool)Fields[i])
                            {
                                br = Brushes.Green;
                                to_print = GenOnStr;
                            }
                            else
                            {
                                br = Brushes.Green;
                                to_print = GenOffStr;
                            }
                            break;
                        case FTypes.OnOffState:
                            x += 4;
                            if ((bool)Fields[i])
                            {
                                br = Brushes.Green;
                                to_print = OnStr;
                            }
                            else
                            {
                                br = Brushes.Green;
                                to_print = OffStr;
                            }
                            break;
                    }
                    XFrom[i] = x;
                    if (i == SelectedField)
                    {
                        to_print = ActiveText.Text;
                        br = Brushes.Red;
                        string prefix = to_print.Substring(0,ActiveText.SelectionStart);
                        string selection = to_print.Substring(ActiveText.SelectionStart, ActiveText.SelectionLength);
                        string sufix = to_print.Substring(ActiveText.SelectionStart+ActiveText.SelectionLength);
                        if (prefix != null && prefix.Length > 0)
                        {
                            g.DrawString(prefix, f, br, x, y);
                            x += (int)g.MeasureString(prefix, f).Width;
                        }
                        if (selection != null && selection.Length > 0)
                        {
                            int x_from = x;
                            int x_to = x + (int)g.MeasureString(selection, f).Width;
                            g.FillRectangle(Brushes.LightGray, x_from, 0, x_to - x_from, Height);
                            g.DrawString(selection, f, br, x, y);
                            x = x_to;
                        }
                        else
                        {
                            g.DrawLine(Pens.Black, x, 0, x, Height);
                            g.DrawLine(Pens.Black, x + 1, 0, x + 1, Height);
                        }
                        if (sufix != null && sufix.Length > 0)
                        {
                            g.DrawString(sufix, f, br, x, y);
                            x += (int)g.MeasureString(sufix, f).Width;
                        }
                    }
                    else
                    {
                        g.DrawString(to_print, f, br, x, y);
                        x += (int)g.MeasureString(to_print, f).Width;
                    }
                    XTo[i] = x;
                    x += dlt_x;
                }

                x = (x / 20 + 1) * 20;

                to_print = "x";
                DeleteBtnX = x;
                g.DrawString(to_print, f, Brushes.Red, x, y);
                int w = (int)g.MeasureString(to_print, f).Width;
                g.DrawRectangle(Pens.Black, x, 0, w, Height-1);
                x += w;
                x++;

                AddBtnX = x;
                to_print = "+";
                g.DrawString(to_print, f, Brushes.Green, x, y);
                w = (int)g.MeasureString(to_print, f).Width;
                g.DrawRectangle(Pens.Black, x, 0, w, Height-1);
                x += w;
                x++;

                NWidth = x;
                if (NWidth != Width)
                {
                    Size = new Size(NWidth, Height);
                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                Common.LogNoMsg(ex);
            }
        }

        public override string ToString()
        {
            string ret;
            switch (Line.Type)
            {
                default: ret = ""; return ret;
                case SpectrCondition.CondTypes.Comment: 
                    ret = "#";
                    for (int i = 0; i < Fields.Length;i++ )
                        ret += Fields[i]+" ";
                        ret += serv.Endl; 
                    break;
                case SpectrCondition.CondTypes.Prespark:
                    ret = "p:" + Fields[0] + " On ()" + serv.Endl; 
                    break;
                case SpectrCondition.CondTypes.Exposition:
                    ret = "e:" + Fields[0] + " (";
                    for (int i = 1; i < Fields.Length - 1; i++)
                    {
                        ret += Fields[i];
                        if (i < Fields.Length - 2)
                            ret += ";";
                    }
                    ret += ")";
                    if ((bool)Fields[Fields.Length - 1])
                        ret += "On";
                    else
                        ret += "Off";
                    ret += "()" + serv.Endl;
                    break;
                case SpectrCondition.CondTypes.FillLight:
                    ret = "f:";
                    if ((bool)Fields[0])
                        ret += "On";
                    else
                        ret += "Off";
                    ret += serv.Endl;
                    break;
            }
            return ret;
        }
    }
}
