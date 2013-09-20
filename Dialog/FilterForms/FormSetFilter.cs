using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Items;
using GeneLibrary.Common;
using System.Globalization;
using WFExceptions;
using GeneLibrary.Dialog;

namespace GeneLibrary.MdiForms
{
    public partial class FormSetFilter : Form
    {
        // Constructors
        public FormSetFilter()
        {
            InitializeComponent();
        }
        public FormSetFilter(DataGridView dataGridFilter) : this()
        {
            this.dataGridFilter = dataGridFilter;
        }

        // Events handler
        private void FormSetFilter_Load(object sender, EventArgs e)
        {
            LoadTree();
            EnabledControl();
        }
        private void toolStripButtonAddFilter_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                formBuildFilter = new FormBuildFilter(0);
                formBuildFilter.OnUpdateId += new UpdateId(UpdateFilter);
                formBuildFilter.ShowDialog();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (treeViewFilters.SelectedNode.Name != "root")
                {
                    formBuildFilter = new FormBuildFilter(Convert.ToInt32(treeViewFilters.SelectedNode.Name, CultureInfo.InvariantCulture));
                    formBuildFilter.OnUpdateId += new UpdateId(UpdateFilter);
                    formBuildFilter.ShowDialog();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripButtonDeleteFilter_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (treeViewFilters.SelectedNode != null && treeViewFilters.SelectedNode.Name != "root")
                {
                    filterVocabulary.Del(new int[1] { Convert.ToInt32(treeViewFilters.SelectedNode.Name, CultureInfo.InvariantCulture) });
                    richTextBoxFilter.Clear();
                    LoadTree();
                    EnabledControl();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void treeViewFilters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name != "root")
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    filterVocabulary.LoadItem(Convert.ToInt32(e.Node.Name, CultureInfo.InstalledUICulture));
                    FillRichTextBox();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            EnabledControl();
        }
        private void treeViewFilters_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView != null && treeView.SelectedNode != null)
            {
                if (treeView.SelectedNode.Name != "root")
                    toolStripButtonEdit_Click(treeView.SelectedNode, null);
            }
        }
        private void toolStripButtonApply_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (treeViewFilters.SelectedNode.Name != "root")
                {
                    FormFilter formFilter = Tools.MainForm().NewFilterResultForm(treeViewFilters.SelectedNode.Text);
                    filterVocabulary.Item.DataGridView = formFilter.DataGridFilter;
                    filterVocabulary.Item.Apply();
                    formFilter.EnabledControls();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void FormSetFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formBuildFilter != null)
                formBuildFilter.Close();
        }
        private void toolStripMenuItemApply_Click(object sender, EventArgs e)
        {
            toolStripButtonApply_Click(sender, e);
        }
        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            toolStripButtonEdit_Click(sender, e);
        }
        private void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {
            toolStripButtonDeleteFilter_Click(sender, e);
        }
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            Common.Tools.GetHelp("mf_card_select", HelpNavigator.Topic);
        }
        

        // Private methods
        private void EnabledControl()
        {
            bool isNodeSelected = (treeViewFilters.SelectedNode != null && treeViewFilters.SelectedNode.Name != "root");

            toolStripButtonApply.Enabled = isNodeSelected;
            toolStripButtonDeleteFilter.Enabled = isNodeSelected;
            toolStripButtonEdit.Enabled = isNodeSelected;
        }
        private void LoadTree()
        {
            filterVocabulary.Open();
            treeViewFilters.BeginUpdate();
            try
            {
                TreeNode rootNode = treeViewFilters.Nodes.Find("root", false)[0];
                rootNode.Nodes.Clear();
                for (int i = 0; i < filterVocabulary.DT.Rows.Count; i++)
                {
                    TreeNode treeNode = new TreeNode(filterVocabulary.DT.Rows[i]["NAME"].ToString());
                    treeNode.Name = filterVocabulary.DT.Rows[i]["ID"].ToString();
                    treeNode.ContextMenuStrip = contextMenuStripFilter;
                    rootNode.Nodes.Add(treeNode);
                }
                rootNode.Expand();
            }
            finally
            {
                treeViewFilters.EndUpdate();
            }
        }
        private void FillRichTextBox()
        {
            richTextBoxFilter.Clear();
            AddFormatedText(Properties.Resources.filterCard, new Font("Tahoma", 10, FontStyle.Bold), Color.Green, "");

            int cnt = 0;
            #region Поля ИКЛ
            if (filterVocabulary.Item.InIkl)
            {
                FilterField[] filterFieldsIkl = (from FilterField filterField in filterVocabulary.Item.IklFilter
                                                 where !String.IsNullOrEmpty(filterField.FilterFieldType.Condition.Name)
                                                 select filterField).ToArray<FilterField>();
                if (filterFieldsIkl.Count() > 0)
                {
                    AddFormatedTextLine(Properties.Resources.filterNameIkl, new Font("Tahoma", 9, FontStyle.Bold), Color.MidnightBlue, "  ");
                    foreach (FilterField filterField in filterFieldsIkl)
                    {
                        AddFormatedTextLine(filterField.Caption, new Font("Tahoma", 8, FontStyle.Bold), Color.Black, "\t");
                        AddFormatedText(filterField.FilterFieldType.Condition.Name, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
                        if (filterField.IsPartValue)
                            AddFormatedText(Properties.Resources.IsPartValue, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
                    }
                    cnt++;
                }
            }
            #endregion

            #region Поля ИК-2
            if (filterVocabulary.Item.InIk2)
            {
                FilterField[] filterFieldsIk2 = (from FilterField filterField in filterVocabulary.Item.Ik2Filter
                                                 where !String.IsNullOrEmpty(filterField.FilterFieldType.Condition.Name)
                                                 select filterField).ToArray<FilterField>();
                if (filterFieldsIk2.Count() > 0)
                {
                    AddFormatedTextLine(Properties.Resources.filterNameIk2, new Font("Tahoma", 9, FontStyle.Bold), Color.MidnightBlue, "");
                    foreach (FilterField filterField in filterFieldsIk2)
                    {
                        AddFormatedTextLine(filterField.Caption, new Font("Tahoma", 8, FontStyle.Bold), Color.Black, "\t");
                        AddFormatedText(filterField.FilterFieldType.Condition.Name, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
                        if (filterField.IsPartValue)
                            AddFormatedText(Properties.Resources.IsPartValue, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
                    }
                    cnt++;
                }
            }
            #endregion

            if (cnt == 0)
                AddFormatedTextLine(Properties.Resources.NoDate, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, "\t");


            #region Область поиска
            AddFormatedTextLine(Properties.Resources.cardAreaFind, new Font("Tahoma", 10, FontStyle.Bold), Color.Green, "");
            AddFormatedTextLine(AreaFind(), new Font("Tahoma", 9, FontStyle.Italic), Color.Black, "\t"); 
            #endregion

            #region Поля, отображаемые в отчетах
            StringBuilder iklFields = new StringBuilder("");
            StringBuilder ik2Fields = new StringBuilder("");

            AddFormatedTextLine(Properties.Resources.FieldReport, new Font("Tahoma", 10, FontStyle.Bold), Color.Green, "");
            var ikl = filterVocabulary.Item.IklFilter.
                Where(item => item.IsReportShow == true).
                Select(item => item.Caption);
            var ik2 = filterVocabulary.Item.Ik2Filter.
                Where(item => item.IsReportShow == true).
                Select(item => item.Caption);

            if (ikl.Count() > 0 && filterVocabulary.Item.InIkl)
            {
                iklFields.Append(ikl.Aggregate((curr, next) => curr + ", " + next));
                AddFormatedTextLine(resDataNames.ikl, new Font("Tahoma", 8, FontStyle.Bold), Color.Black, "\t");
                if (String.IsNullOrEmpty(iklFields.ToString().Trim()))
                    AddFormatedText(Properties.Resources.NoDate, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
                else
                    AddFormatedText(iklFields.ToString(), new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
            }
            if (ik2.Count() > 0 && filterVocabulary.Item.InIk2)
            {
                ik2Fields.Append(ik2.Aggregate((curr, next) => curr + ", " + next));
                AddFormatedTextLine(resDataNames.ik2, new Font("Tahoma", 8, FontStyle.Bold), Color.Black, "\t");
                if (String.IsNullOrEmpty(ik2Fields.ToString().Trim()))
                    AddFormatedText(Properties.Resources.NoDate, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
                else
                    AddFormatedText(ik2Fields.ToString(), new Font("Tahoma", 9, FontStyle.Italic), Color.Black, " ");
            }

            if (String.IsNullOrEmpty(iklFields.ToString() + ik2Fields.ToString()))
                AddFormatedTextLine(Properties.Resources.NoDate, new Font("Tahoma", 9, FontStyle.Italic), Color.Black, "\t");

            #endregion

        }
        private void AddFormatedText(string text, Font font, Color color, string ident)
        {
            int StartPosition = richTextBoxFilter.Text.Length;
            int lengthSelection;
            if (!String.IsNullOrEmpty(text.Trim()))
            {
                richTextBoxFilter.AppendText(ident + text);
                lengthSelection = text.Length+ident.Length;
            }
            else
            {
                richTextBoxFilter.AppendText(ident + Properties.Resources.NoDate);
                lengthSelection = Properties.Resources.NoDate.Length + ident.Length;
            }
            richTextBoxFilter.Select(StartPosition, lengthSelection);
            richTextBoxFilter.SelectionFont = font;
            richTextBoxFilter.SelectionColor = color;
        }
        private void AddFormatedTextLine(string text, Font font, Color color, string ident)
        {
            AddFormatedText('\n' + ident + text, font, color, "");
        }
        private string AreaFind()
        {
            StringBuilder stringTemp = new StringBuilder();
            if (filterVocabulary.Item.InIkl)
                stringTemp.Append(Properties.Resources.areaFindIkl);
            if (filterVocabulary.Item.InIk2)
            {
                if (stringTemp.Length == 0)
                    stringTemp.Append(Properties.Resources.areaFindIk2);
                else
                    stringTemp.Append(", " + Properties.Resources.areaFindIk2);
            }
            if (filterVocabulary.Item.InArchive)
            {
                if (stringTemp.Length == 0)
                    stringTemp.Append(Properties.Resources.areaFindArchive);
                else
                    stringTemp.Append(", " + Properties.Resources.areaFindArchive);
            }
            if (stringTemp.Length == 0)
                stringTemp.Append(Properties.Resources.NoDate);
            return stringTemp.ToString();
        }
        private void UpdateFilter(int Id)
        {
            LoadTree();
            TreeNode[] treeNodes = treeViewFilters.Nodes.Find(Id.ToString(), true);
            if (treeNodes.Count() > 0)
                treeViewFilters.SelectedNode = treeNodes[0];
            EnabledControl();
            this.Activate();
        }

        // Fields
        private FilterVocabulary filterVocabulary = new FilterVocabulary();
        private DataGridView dataGridFilter;
        private FormBuildFilter formBuildFilter;

    }
}
