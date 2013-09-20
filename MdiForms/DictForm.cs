using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Common;
using GeneLibrary.Items;
using GeneLibrary.Dialog;
using WFExceptions;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Globalization;
using System.Collections.ObjectModel;


namespace GeneLibrary.MdiForms
{
    public partial class DictionaryForm : Form
    {
        // Fields
        private DictionaryKind _dictKind;
        private Vocabulary _activeDict;
        private string _dictName;
        private Rectangle dragBoxFromMouseDown;
        private Point screenOffset;
        private bool openCompareForm;

        // Constructors
        public DictionaryForm(DictionaryKind dictionaryKind, string dictionaryName, string formNameId)
        {
            InitializeComponent();
            _dictKind = dictionaryKind;
            _dictName = dictionaryName;
            this.FormNameId = formNameId;
        }

        // Обработка событий toolbara и сетки
        private void DictForm_Load(object sender, EventArgs e)
        {
            switch (_dictKind)
            {
                case DictionaryKind.Division:
                    _activeDict = new DivisionVocabulary();
                    break;
                case DictionaryKind.Exp:
                    _activeDict = new ExpertVocabulary();
                    break;
                case DictionaryKind.Lin:
                    _activeDict = new LinVocabulary();
                    break;
                case DictionaryKind.Method:
                    _activeDict = new MethodVocabulary();
                    break;
                case DictionaryKind.Mvd:
                    _activeDict = new MvdVocabulary();
                    break;
                case DictionaryKind.Post:
                    _activeDict = new PostVocabulary();
                    break;
                case DictionaryKind.UK:
                    _activeDict = new UKVocabulary();
                    break;
                case DictionaryKind.Organization:
                    _activeDict = new OrganizationVocabulary();
                    break;
                case DictionaryKind.Role:
                    _activeDict = new RoleVocabulary();
                    break;
                case DictionaryKind.Sort:
                    _activeDict = new SortObjectVocabulary();
                    break;
                case DictionaryKind.ClassObject:
                    _activeDict = new ClassObjectVocabulary();
                    break;
                case DictionaryKind.ClassIkl:
                    _activeDict = new ClassIklVocabulary();
                    break;
            }
            _activeDict.Open(dataGridVocabulary);
            this.Text = _dictName;

            EnableControls();
            ThisDeserivalize();
        }
        private void OnNewDict(object sender, EventArgs e)
        {
            switch (_dictKind)
            {
                case DictionaryKind.Division:
                    DivisionForm divDlg = new DivisionForm(_activeDict);
                    divDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    divDlg.ShowDialog();
                    break;
                case DictionaryKind.Exp:
                    ExpertForm expDlg = new ExpertForm(_activeDict);
                    expDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    expDlg.ShowDialog();
                    break;
                case DictionaryKind.Lin:
                    LinForm linDlg = new LinForm(_activeDict);
                    linDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    linDlg.ShowDialog();
                    break;
                case DictionaryKind.Method:
                    MethodForm methDlg = new MethodForm(_activeDict, this.Location);
                    methDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    methDlg.ShowDialog();
                    break;
                case DictionaryKind.Mvd:
                    MvdForm mvdDlg = new MvdForm(_activeDict);
                    mvdDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    mvdDlg.ShowDialog();
                    break;
                case DictionaryKind.Post:
                    PostForm postDlg = new PostForm(_activeDict);
                    postDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    postDlg.ShowDialog();
                    break;
                case DictionaryKind.UK:
                    UKForm ukDlg = new UKForm(_activeDict);
                    ukDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    ukDlg.ShowDialog();
                    break;
                case DictionaryKind.Role:
                    RoleForm roleDlg = new RoleForm(_activeDict);
                    roleDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    roleDlg.ShowDialog();
                    break;
                case DictionaryKind.Organization:
                    OrganForm orgDlg = new OrganForm(_activeDict);
                    orgDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    orgDlg.ShowDialog();
                    break;
                case DictionaryKind.Sort:
                    FormSortObject formSortObject = new FormSortObject(_activeDict);
                    formSortObject.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    formSortObject.ShowDialog();
                    break;
                case DictionaryKind.ClassObject:
                    FormClassObject formClassObject = new FormClassObject(_activeDict);
                    formClassObject.OnDataLoad += new UpdateId(RefreshForm);
                    formClassObject.ShowDialog();
                    break;
                case DictionaryKind.ClassIkl:
                    FormClassIkl formClassIkl = new FormClassIkl(_activeDict);
                    formClassIkl.OnDataLoad += new UpdateId(RefreshForm);
                    formClassIkl.ShowDialog();
                    break;
            }
        }
        private void OnOpenDict(object sender, EventArgs e)
        {
            switch (_dictKind)
            {
                case DictionaryKind.Division:
                    DivisionForm divDlg = new DivisionForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    divDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm); ;
                    divDlg.ShowDialog();
                    break;
                case DictionaryKind.Exp:
                    ExpertForm expDlg = new ExpertForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    expDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    expDlg.ShowDialog();
                    break;
                case DictionaryKind.Lin:
                    LinForm linDlg = new LinForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    linDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    linDlg.ShowDialog();
                    break;
                case DictionaryKind.Method:
                    MethodForm methDlg = new MethodForm(_activeDict, this.Location, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    methDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    methDlg.ShowDialog();
                    break;
                case DictionaryKind.Mvd:
                    MvdForm mvdDlg = new MvdForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    mvdDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    mvdDlg.ShowDialog();
                    break;
                case DictionaryKind.Post:
                    PostForm postDlg = new PostForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    postDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    postDlg.ShowDialog();
                    break;
                case DictionaryKind.UK:
                    UKForm ukDlg = new UKForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    ukDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    ukDlg.ShowDialog();
                    break;
                case DictionaryKind.Sort:
                    FormSortObject formSortObject = new FormSortObject(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    formSortObject.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    formSortObject.ShowDialog();
                    break;
                case DictionaryKind.Role:
                    if (dataGridVocabulary.Rows.Count > 0)
                    {
                        RoleForm roleDlg = new RoleForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                        roleDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                        roleDlg.ShowDialog();
                    }
                    break;
                case DictionaryKind.Organization:
                    OrganForm orgDlg = new OrganForm(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    orgDlg.OnDataLoad += new GeneLibrary.Common.UpdateId(RefreshForm);
                    orgDlg.ShowDialog();
                    break;
                case DictionaryKind.ClassObject:
                    FormClassObject formClassObject = new FormClassObject(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    formClassObject.OnDataLoad += new UpdateId(RefreshForm);
                    formClassObject.ShowDialog();
                    break;
                case DictionaryKind.ClassIkl:
                    FormClassIkl formClassIkl = new FormClassIkl(_activeDict, Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
                    formClassIkl.OnDataLoad += new UpdateId(RefreshForm);
                    formClassIkl.ShowDialog();
                    break;
            }
        }
        private void OnDeleteDict(object sender, EventArgs e)
        {
            if (dataGridVocabulary.Rows.Count == 0)
                return;

            if (Tools.ShowMessage(Properties.Resources.ConfirmDictDel) == DialogResult.OK)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    int idx = dataGridVocabulary.CurrentCell.RowIndex;
                    int sc = dataGridVocabulary.SelectedRows.Count;
                    _activeDict.Del((from DataGridViewRow dataGridRow in dataGridVocabulary.SelectedRows
                                     select Convert.ToInt32(dataGridRow.Cells["id"].Value, CultureInfo.InvariantCulture)).ToArray<int>());
                    _activeDict.Open(dataGridVocabulary);

                    if ((sc == 1) && (idx > 0))
                        dataGridVocabulary.CurrentCell = this.dataGridVocabulary.Rows[--idx].Cells[dataGridVocabulary.FirstDisplayedCell.ColumnIndex];

                    tstbQuickSearch_TextChanged(tstbQuickSearch, null);
                    EnableControls();
                    ThisDeserivalize();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }
        private void dgr_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OnOpenDict(null, null);
        }
        private void tstbQuickSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(tstbQuickSearch.Text.Trim()))
                    _activeDict.DT.DefaultView.RowFilter = "";
                else
                    _activeDict.DT.DefaultView.RowFilter = " fhash like '*" + ((ToolStripTextBox)sender).Text + "*'";
                EnableControls();
            }
            catch
            {
                throw new WFException(ErrType.Message, ErrorsMsg.NotValidFindSimbol);
            }
        }
        private void OnExportToExcel(object sender, EventArgs e)
        {
            _activeDict.ToExcel(_dictName, new System.Drawing.Point(3, 1), dataGridVocabulary);
        }
        private void OnDictionaryRefresh(object sender, EventArgs e)
        {
            if (dataGridVocabulary.Rows.Count > 0)
                RefreshForm(Convert.ToInt32(dataGridVocabulary.CurrentRow.Cells["id"].Value, CultureInfo.InvariantCulture));
        }
        private void DictForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ThisSerivalize();
            }
            catch (Exception err)
            {
                // Ошибка сериализации тихо записывается в лог-файл
                WFException.HandleError(err);
            }
        }
        private void dataGridVocabulary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableControls();
        }
        private void dataGridVocabulary_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            EnableControls();
        }
        private void dataGridVocabulary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N:
                        OnNewDict(null, null);
                        return;
                    case Keys.O:
                        OnOpenDict(null, null);
                        return;
                    case Keys.R:
                        OnDictionaryRefresh(null, null);
                        return;
                    default:
                        return;
                }
            }
            if (e.KeyCode == Keys.Delete)
                OnDeleteDict(null, null);
        }
        private void toolStripButtonVerification_Click(object sender, EventArgs e)
        {
            if (Tools.IsMdiFormOpen("FormCompareDictionary"))
                Application.OpenForms["FormCompareDictionary"].Activate();
            else
            {
                FormCompareDictionary formCompare = new FormCompareDictionary(dataGridVocabulary.Columns.Count, _activeDict);
                formCompare.OnVacabularyUpdate += new UpdateId(RefreshForm);
                formCompare.MdiParent = Tools.MainForm();
                formCompare.Show();
                Common.NativeMethods.SetParent((int)formCompare.Handle, (int)Tools.MainForm().Handle);
            }
        }

        // Private methods
        private void ThisSerivalize()
        {
            WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + _dictKind.ToString() + "_dict.dmp",
                new GridSerialize(dataGridVocabulary.Columns, this.Location, this.Size));
        }
        private void ThisDeserivalize()
        {
            GridSerialize fs =
                (GridSerialize)WFSerialize.Deserialize(Path.GetDirectoryName(Application.ExecutablePath) +
                    "\\" + _dictKind.ToString() + "_dict.dmp");
            if ((fs != null) && (fs._dgc != null))
                foreach (DataGridViewColumn dc in dataGridVocabulary.Columns)
                    dc.Width = fs._dgc[dc.Name].Width;
            if ((fs != null) && (fs.point != null) && (fs.size != null))
            {
                this.Location = new Point(fs.point.X, fs.point.Y);
                this.Size = new Size(fs.size.Width, fs.size.Height);
            }
        }
        private void EnableControls()
        {
            toolStripButtonEdit.Enabled = dataGridVocabulary.SelectedRows.Count == 1;
            toolStripButtonDelete.Enabled = dataGridVocabulary.SelectedRows.Count > 0;
            //DataTable dataTable = dataGridVocabulary.DataSource as DataTable;
            //toolStripButtonExcel.Enabled = dataTable.Rows.Count > 0;
            toolStripButtonExcel.Enabled = dataGridVocabulary.Rows.Count > 0;
        }

        // Public methods
        public void RefreshForm(int id)
        {
            _activeDict.Open(dataGridVocabulary);
            _activeDict.DT.DefaultView.Sort = "id";
            int idx = _activeDict.DT.DefaultView.Find(id);
            if (idx != -1)
                dataGridVocabulary.CurrentCell = dataGridVocabulary.Rows[idx].Cells[dataGridVocabulary.FirstDisplayedCell.ColumnIndex];
            EnableControls();
            tstbQuickSearch_TextChanged(tstbQuickSearch, null);
            ThisDeserivalize();
        }
        public bool IsSelectRow()
        {
            return this.dataGridVocabulary.SelectedRows.Count > 0;
        }

        // Properties
        public bool IsSelectedRows
        {
            get
            {
                return this.dataGridVocabulary.SelectedRows[0] == null;
            }
        }
        public string FormNameId { get; set; }
        public DictionaryKind DictionaryKind
        {
            get
            {
                return _dictKind;
            }
        }

        [Serializable]
        private class GridSerialize : ISerializable
        {
            public GridSerialize(DataGridViewColumnCollection dgc, Point point, Size size)
            {
                _dgc = dgc;
                this.point = point;
                this.size = size;
            }
            private GridSerialize(SerializationInfo info, StreamingContext context)
            {
                _dgc = new DataGridViewColumnCollection(new DataGridView());
                string[] s_array = info.GetString("STR_VAL").Split(';');
                foreach (string s_inner in s_array)
                {
                    _dgc.Add(
                        new DataGridViewColumn()
                        {
                            Width = Convert.ToInt32(s_inner.Split(',')[0], CultureInfo.InvariantCulture),
                            Name = s_inner.Split(',')[1]
                        }
                        );
                }

                string[] locationArray = info.GetString("LOCATION").Split(';');
                point.X = Convert.ToInt32(locationArray[0].Split(',')[0], CultureInfo.InvariantCulture);
                point.Y = Convert.ToInt32(locationArray[0].Split(',')[1], CultureInfo.InvariantCulture);
                size.Width = Convert.ToInt32(locationArray[1].Split(',')[0], CultureInfo.InvariantCulture);
                size.Height = Convert.ToInt32(locationArray[1].Split(',')[1], CultureInfo.InvariantCulture);
            }
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                string result =
                    (from DataGridViewColumn dc in _dgc select dc.Width + "," + dc.Name)
                    .Aggregate((first, second) => first + ";" + second);
                info.AddValue("STR_VAL", result);

                info.AddValue("LOCATION", String.Format("{0},{1};{2},{3}", point.X, point.Y, size.Width, size.Height));
            }

            public DataGridViewColumnCollection _dgc;
            public Point point;
            public Size size;
        }

        // Drag and drop
        private void dataGridVocabulary_MouseDown(object sender, MouseEventArgs e)
        {
            openCompareForm = Tools.IsMdiFormOpen("FormCompareDictionary");
            Size dragSize = SystemInformation.DragSize;
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                           e.Y - (dragSize.Height / 2)), dragSize);
        }
        private void dataGridVocabulary_MouseUp(object sender, MouseEventArgs e)
        {
            dragBoxFromMouseDown = Rectangle.Empty;
        }
        private void dataGridVocabulary_MouseMove(object sender, MouseEventArgs e)
        {
            bool leftMouseDown = (e.Button & MouseButtons.Left) == MouseButtons.Left;
            bool rigthMouseDown = (e.Button & MouseButtons.Right) == MouseButtons.Right;

            if ((leftMouseDown || rigthMouseDown) && openCompareForm)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    screenOffset = SystemInformation.WorkingArea.Location;
                    DragDropEffects dropEffect = dataGridVocabulary.DoDragDrop(
                        new CompareTableRow(dataGridVocabulary.SelectedRows, _activeDict),
                        DragDropEffects.All | DragDropEffects.Link);
                }
            }
        }

    }
}
