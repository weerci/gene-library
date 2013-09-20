using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Dialog;
using GeneLibrary.Common;
using GeneLibrary.Items;
using System.IO;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GeneLibrary.MdiForms
{

    public partial class FindCards : Form
    {
        // Constructors
        public FindCards(string dictionaryName, string formNameId)
        {
            InitializeComponent();
            this.vocabularyName = dictionaryName;
            this.FormNameId = formNameId;
            IklForm.onDataLoad += new UpdateId(Form_OnDataLoad);
            IK2Form.onDataLoad += new UpdateId(Form_OnDataLoad);
        }
        
        // Events handlers
        private void FindCards_Load(object sender, EventArgs e)
        {
            tcCards_Selected(null, new TabControlEventArgs(tpIKL, 0, TabControlAction.Selected));
            dataGridView.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView_CellDoubleClick);
            dataGridView.MouseDown += new MouseEventHandler(dataGridView_MouseDown);
            dataGridView.MouseUp += new MouseEventHandler(dataGridView_MouseUp);
            dataGridView.MouseMove += new MouseEventHandler(dataGridView_MouseMove);
            this.Text = vocabularyName;
            toolStripTextBoxFind.Focus();
        }
        private void tcCards_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Refreshed(e.TabPage);
            }
            finally
            {
                this.Cursor = Cursors.Default;                
            }            
        }
        private void FindCards_FormClosed(object sender, FormClosedEventArgs e)
        {
            IklForm.onDataLoad -= Form_OnDataLoad;
            IK2Form.onDataLoad -= Form_OnDataLoad;
        }
        private void FindCards_KeyDown(object sender, KeyEventArgs e)
        {
            if (!dataGridView.Focused)
                return;

            if (e.Control && e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        toolStripButtonFind_Click(null, null);
                        break;
                }
            }
            else if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N:
                        newToolStripButton_Click(null, null);
                        break;
                    case Keys.O:
                        openToolStripButton_Click(null, null);
                        break;
                    case Keys.R:
                        tsbRefresh_Click(null, null);
                        break;
                    case Keys.F:
                        toolStripTextBoxFind.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        cutToolStripButton_Click(null, null);
                        break;
                }
            }
        }
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            Refreshed(tcCards.SelectedTab);
        }
        
        // Menu events
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                switch (tcCards.SelectedTab.Name)
                {
                    case "tpIKL":
                        if (OnActiveIklCard != null)
                            OnActiveIklCard(vocabulary, 0);
                        break;
                    case "tpIK2":
                        if (OnActiveIk2Card != null)
                            OnActiveIk2Card(vocabulary, 0);
                        break;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (AwareMessageBox.Show(
                                this,
                                Properties.Resources.ConfirmDictDel,
                                Properties.Resources.MessageBoxTitle,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                                (MessageBoxOptions)0) == DialogResult.OK)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        int idx = dataGridView.CurrentCell.RowIndex;
                        int sc = dataGridView.SelectedRows.Count;
                        vocabulary.Del(
                            (from DataGridViewRow row in dataGridView.SelectedRows
                             select Convert.ToInt32(row.Cells["id"].Value, CultureInfo.InvariantCulture)).ToArray<int>());

                        vocabulary.Open(dataGridView, toolStripTextBoxFind.Text);
                        if ((sc == 1) && (idx > 0))
                            dataGridView.CurrentCell = this.dataGridView.Rows[--idx].Cells[dataGridView.FirstDisplayedCell.ColumnIndex];
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                switch (tcCards.SelectedTab.Name)
                {
                    case "tpIKL":
                        if (OnActiveIklCard != null)
                            OnActiveIklCard(vocabulary, Convert.ToInt32(dataGridView.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                        break;
                    case "tabPageIdent":
                    case "tpIK2":
                        if (OnActiveIk2Card != null)
                            OnActiveIk2Card(vocabulary, Convert.ToInt32(dataGridView.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                        break;
                }

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                FormFind formFind = new FormFind(Convert.ToInt32(row.Cells["id"].Value, CultureInfo.InvariantCulture), this.CardKind);
                formFind.OnActiveIklCard += OnActiveIklCard;
                formFind.OnActiveIk2Card += OnActiveIk2Card;
                formFind.OnActiveStudyForm += new ActiveStudyForm(Tools.MainForm().NewStudyForm);
                formFind.MdiParent = Tools.MainForm();
                formFind.Show();
            }
        }
        private void toolStripButtonCardFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                switch (tcCards.SelectedTab.Name)
                {
                    case "tpIKL":
                    case "tpIK2":
                        this.vocabulary.Open(this.dataGridView, toolStripTextBoxFind.Text);
                        break;
                    case "tabPageIdent":
                        ((Ik2Vocabulary)vocabulary).OpenIdent(this.dataGridView, toolStripTextBoxFind.Text);
                        break;
                    case "tabPageArchive":
                        ((ArchiveVocabulary)vocabulary).Open(this.dataGridView, toolStripTextBoxFind.Text);
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripTextBoxFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                toolStripButtonCardFind_Click(null, null);
        }
        private void toolStripButtonUnIdent_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int idx = dataGridView.CurrentCell.RowIndex;
                int sc = dataGridView.SelectedRows.Count;

                Ik2Vocabulary ik2Vocabulary = this.vocabulary as Ik2Vocabulary;
                if (ik2Vocabulary != null)
                {
                    DataGridViewRow row = dataGridView.SelectedRows[0];
                    int selectedIk2 = Convert.ToInt32(row.Cells["id"].Value, CultureInfo.InvariantCulture);
                    ik2Vocabulary.LoadItem(selectedIk2);
                    ik2Vocabulary.Item.UnIdentAll();
                }

                // Обновление данных на форме
                ik2Vocabulary.OpenIdent(dataGridView, toolStripTextBoxFind.Text);
                if ((sc == 1) && (idx > 0))
                    dataGridView.CurrentCell = this.dataGridView.Rows[--idx].Cells[dataGridView.FirstDisplayedCell.ColumnIndex];
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripButtonUnArchive_Click(object sender, EventArgs e)
        {
            switch (tcCards.SelectedTab.Name)
            {
                case "tabPageArchive":
                    ArchiveVocabulary archiveVocabulary = new ArchiveVocabulary();
                    FormArchiveComment formArchiveComment = new FormArchiveComment();
                    if (formArchiveComment.ShowDialog() == DialogResult.OK)
                    {
                        archiveVocabulary.Note = formArchiveComment.Note;
                        archiveVocabulary.ExtractFromArchive(
                            (from DataGridViewRow row in dataGridView.SelectedRows
                             select Convert.ToInt32(row.Cells["id"].Value, CultureInfo.InvariantCulture)).ToArray<int>()
                             );
                    }
                    break;
                default:
                    break;
            }
            Refreshed(tcCards.SelectedTab);
        }
        private void toolStripButtonClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                toolStripTextBoxFind.Text = "";
                toolStripButtonCardFind_Click(null, null);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripButtonArchive_Click(object sender, EventArgs e)
        {
            switch (tcCards.SelectedTab.Name)
            {
                case "tabPageIdent": 
                case "tpIKL":
                    ArchiveVocabulary archiveVocabulary = new ArchiveVocabulary();
                    FormArchiveComment formArchiveComment = new FormArchiveComment();
                    if (formArchiveComment.ShowDialog() == DialogResult.OK)
                    {
                        archiveVocabulary.Note = formArchiveComment.Note;
                        archiveVocabulary.RemoveToArchive(
                            (from DataGridViewRow row in dataGridView.SelectedRows
                             select Convert.ToInt32(row.Cells["id"].Value, CultureInfo.InvariantCulture)).ToArray<int>()
                             );
                    }
                    break;
                default:
                    break;
            }
            this.Refreshed(tcCards.SelectedTab);
        }
        private void toolStripButtonGeneTable_Click(object sender, EventArgs e)
        {
            if (Tools.IsMdiFormOpen("FormGeneTable"))
                Application.OpenForms["FormGeneTable"].Activate();
            else
            {
                FormGeneTable formGeneTable = new FormGeneTable();
                formGeneTable.MdiParent = Tools.MainForm();
                formGeneTable.Show();
                Common.NativeMethods.SetParent((int)formGeneTable.Handle, (int)Tools.MainForm().Handle);
            }
        }
        private void toolStripTextBoxFind_TextChanged(object sender, EventArgs e)
        {
            EnabledControls();
        }
        private void toolStripButtonExcel_Click(object sender, EventArgs e)
        {
            switch (tcCards.SelectedTab.Name)
            {
                case "tpIKL":
                    Tools.ToExcel(String.Format(resDataNames.listOfCards, Tools.CardKindName(CardKind.ikl)) + ", " + Properties.Resources.DataOn + " " + DateTime.Today.ToLongDateString(),
                        new System.Drawing.Point(3, 1), dataGridView, ExcelCellOrientation.Horizontal);
                    break;
                case "tpIK2":
                    Tools.ToExcel(String.Format(resDataNames.listOfCards, Tools.CardKindName(CardKind.ik2)) + ", " + Properties.Resources.DataOn + " " + DateTime.Today.ToLongDateString(),
                        new System.Drawing.Point(3, 1), dataGridView, ExcelCellOrientation.Horizontal);
                    break;
                case "tabPageArchive":
                    Tools.ToExcel(resDataNames.CombineReport + ", " + Properties.Resources.DataOn + " " + DateTime.Today.ToLongDateString(), new System.Drawing.Point(3, 1), dataGridView, ExcelCellOrientation.Horizontal);
                    break;
                case "tabPageIdent":
                    Tools.ToExcel(resDataNames.CombineReport  + ", " + Properties.Resources.DataOn + " " + DateTime.Today.ToLongDateString(), new System.Drawing.Point(3, 1), dataGridView, ExcelCellOrientation.Horizontal);
                    break;
                default:
                    break;
            }
        }
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            Common.Tools.GetHelp("mf_card_find", HelpNavigator.Topic);
        }
        
        // Private methods
        private void Refreshed(TabPage tp)
        {
            switch (tp.Name)
            {
                case "tpIKL":
                    vocabulary = new IklVocabulary();
                    dataGridView.Parent = tpIKL;
                    tpIK2.Controls.Clear();
                    tabPageIdent.Controls.Clear();
                    tabPageArchive.Controls.Clear();
                    tpIKL.Controls.Add(dataGridView);
                    vocabulary.Open(dataGridView, toolStripTextBoxFind.Text);
                    break;
                case "tpIK2":
                    vocabulary = new Ik2Vocabulary();
                    dataGridView.Parent = tpIK2;
                    tpIKL.Controls.Clear();
                    tabPageIdent.Controls.Clear();
                    tabPageArchive.Controls.Clear();
                    tpIK2.Controls.Add(dataGridView);
                    vocabulary.Open(dataGridView, toolStripTextBoxFind.Text);
                    break;
                case "tabPageIdent": 
                    vocabulary = new Ik2Vocabulary();
                    dataGridView.Parent = tabPageIdent;
                    tpIKL.Controls.Clear();
                    tpIK2.Controls.Clear();
                    tabPageArchive.Controls.Clear();
                    tabPageIdent.Controls.Add(dataGridView);
                    ((Ik2Vocabulary)vocabulary).OpenIdent(dataGridView, toolStripTextBoxFind.Text);
                    break;
                case "tabPageArchive":
                    vocabulary = new ArchiveVocabulary();
                    dataGridView.Parent = tabPageArchive;
                    tpIKL.Controls.Clear();
                    tpIK2.Controls.Clear();
                    tabPageIdent.Controls.Clear();
                    tabPageArchive.Controls.Add(dataGridView);
                    ((ArchiveVocabulary)vocabulary).Open(dataGridView, toolStripTextBoxFind.Text);
                    break;
            }
            EnabledControls();
        }
        private void Form_OnDataLoad(int id)
        {
            Refreshed(tcCards.SelectedTab);
            vocabulary.DT.DefaultView.Sort = "id";
            int idx = vocabulary.DT.DefaultView.Find(id);
            if (idx != -1)
                dataGridView.CurrentCell = dataGridView.Rows[idx].Cells[dataGridView.FirstDisplayedCell.ColumnIndex];
        }
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            openToolStripButton_Click(null, null);
        }
        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            openGeneTableForm = Tools.IsMdiFormOpen("FormGeneTable");
            Size dragSize = SystemInformation.DragSize;
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                           e.Y - (dragSize.Height / 2)), dragSize);
        }
        private void dataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            dragBoxFromMouseDown = Rectangle.Empty;
        }
        private void dataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            bool leftMouseDown = (e.Button & MouseButtons.Left) == MouseButtons.Left;
            bool rigthMouseDown = (e.Button & MouseButtons.Right) == MouseButtons.Right;

            if ((leftMouseDown || rigthMouseDown) && openGeneTableForm)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    screenOffset = SystemInformation.WorkingArea.Location;
                    DragDropEffects dropEffect = dataGridView.DoDragDrop(
                        new GeneTableRow(dataGridView.SelectedRows, this.CardKind),
                        DragDropEffects.All | DragDropEffects.Link);
                }
            }
        }
        private void EnabledControls()
        {
            bool EmptyGrid = this.dataGridView.RowCount == 0;
            bool EmptyFindString = String.IsNullOrEmpty(toolStripTextBoxFind.Text);
            bool IsIdentPage = tcCards.SelectedTab.Name == "tabPageIdent";
            bool Ik2Page = tcCards.SelectedTab.Name == "tpIK2";
            bool IklPage = tcCards.SelectedTab.Name == "tpIKL"; 
            bool ArchivePage = tcCards.SelectedTab.Name == "tabPageArchive";

            toolStripButtonFind.Enabled = !EmptyGrid;
            openToolStripButton.Enabled = !EmptyGrid;
            newToolStripButton.Visible = !IsIdentPage && !ArchivePage;

            cutToolStripButton.Visible = !IsIdentPage && !ArchivePage;
            cutToolStripButton.Enabled = !EmptyGrid;

            toolStripButtonUnIdent.Visible = IsIdentPage;
            toolStripButtonUnIdent.Enabled = !EmptyGrid;

            toolStripButtonArchive.Enabled = !EmptyGrid;
            toolStripButtonArchive.Visible = !Ik2Page && !ArchivePage;
            toolStripButtonUnArchive.Enabled = !EmptyGrid;
            toolStripButtonUnArchive.Visible = ArchivePage;

            toolStripButtonGeneTable.Visible = IklPage || Ik2Page;
            toolStripButtonGeneTable.Enabled = !EmptyGrid;

            toolStripButtonExcel.Enabled = !EmptyGrid;

            toolStripButtonClearFilter.Enabled = !EmptyFindString;
        }

        // Fields
        private Vocabulary vocabulary;
        private DataGridView dataGridView = new DataGridView() { Dock = DockStyle.Fill, AllowUserToDeleteRows = false, 
            AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true, 
            MultiSelect = true, RowHeadersWidth = 16, StandardTab = true };
        private string vocabularyName;
        private Rectangle dragBoxFromMouseDown;
        private Point screenOffset;
        private bool openGeneTableForm;
        
        // Properties
        public CardKind CardKind
        {
            get
            {
                switch (tcCards.SelectedTab.Name)
                {
                    case "tpIKL":
                        return CardKind.ikl;
                    case "tpIK2": 
                        return CardKind.ik2;
                    case "tabPageArchive":
                        return CardKind.archive;
                    case "tabPageIdent":
                        return CardKind.ident;
                    default:
                        return CardKind.none;
                }
            }
        }
        public string FormNameId { get; set; }

        // Events
        internal event ActiveIklCard OnActiveIklCard;
        internal event ActiveIk2Card OnActiveIk2Card;
     }
}
