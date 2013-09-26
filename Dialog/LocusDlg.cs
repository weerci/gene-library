using GeneLibrary.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WFExceptions;

namespace GeneLibrary.Dialog
{
    public partial class LocusForm : Form
    {
     // Constructors
        public LocusForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this._dict = (LocuseVocabulary)dictionary;
        }
        public LocusForm(Vocabulary dictionary, int id)
            : this(dictionary)
        {
            this.IsEdit = true;
            this._id = id;
        }

        // Events handler
        private void LocusForm_Load(object sender, EventArgs e)
        {
            if (this.IsEdit)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    _dict.LoadItem(_id);
                    _item = _dict.Item;
                    _item.Open();
                    this.tbLocus.Text = _item.Name;
                    LoadAlleliesToGrid();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                _dict.LoadItem(_id);
                _item = _dict.Item;
            }
        }

        private void LoadAlleliesToGrid()
        {
            dgvAllelies.Rows.Clear();
            foreach (ChangedAllele item in _item.Allelies)
            {
                dgvAllelies.Rows.Add(item.Id, item.Name, item.Val);
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (String.IsNullOrEmpty(tbLocus.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyLocusField);

            if (_item.Name != tbLocus.Text.Trim())
            {
                _item.Name = tbLocus.Text.Trim();
                _item.LocusNameIsChanged = true;            
            }
            SetChangeAllelies();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                _id = _item.Save();
                if (OnDataLoad != null)
                    OnDataLoad(_id);

                if (!this.IsEdit)
                {
                    this.IsEdit = true;
                    _item.Id = _id;
                }

                _item.LocusNameIsChanged = false;
                LoadAlleliesToGrid();

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
     
        #region Обработка ошибок ввода данных в сетку

        private void dgvAllele_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyUp += new KeyEventHandler(dgvAllelies_KeyUp);
        }
        private void dgvAllelies_KeyUp(object sender, KeyEventArgs e)
        {
            DataGridViewTextBoxEditingControl editBox = sender as DataGridViewTextBoxEditingControl;
            if (editBox != null)
            {
                int columnIndex = dgvAllelies.SelectedCells[0].ColumnIndex;
                if (columnIndex == 2)
                {
                    if (Regex.IsMatch(editBox.Text, "[^1234567890.]"))
                    {
                        editBox.ForeColor = Color.Red;
                        Common.Tools.ShowTip(editBox, ErrorsMsg.ErrorFormat, String.Format(ErrorsMsg.ErrorNotNumber, dgvAllelies.Columns[columnIndex].HeaderText), ToolTipIcon.Error);
                    }
                    else
                        editBox.ForeColor = SystemColors.WindowText;
                }
                else if (columnIndex == 1)
                {
                    if (editBox.Text.Length > 5)
                    {
                        editBox.ForeColor = Color.Red;
                        Common.Tools.ShowTip(editBox, ErrorsMsg.ErrorFormat, String.Format(ErrorsMsg.ErrorStringLength, dgvAllelies.Columns[columnIndex].HeaderText, 5), ToolTipIcon.Error);
                    }
                }
            }
        }

        #endregion

        private void SetChangeAllelies()
        {
            ChangedAllele[] alleliesFromGrid = dgvAllelies.Rows.Cast<DataGridViewRow>().Select(n => new ChangedAllele() {
                Id = n.Cells["ID"].Value == null ? 0 : Convert.ToInt32(n.Cells["ID"].Value, CultureInfo.InvariantCulture),
                Name = n.Cells["NAME"].Value == null ? "" : n.Cells["NAME"].Value.ToString(),
                Val = n.Cells["VAL"].Value == null ? 0 : Convert.ToDouble(n.Cells["VAL"].Value, CultureInfo.InvariantCulture),
                }).ToArray();

            ChangedAllele[] onlyExists = alleliesFromGrid.Where(n => n.Id != 0).ToArray();
            foreach (ChangedAllele item in _item.Allelies)
            {
                ChangedAllele[] cha = onlyExists.Where(n => n.Id == item.Id).ToArray();
                ChangedAllele ch = cha.Count() == 0 ? null : cha.ElementAt(0);

                if (ch == null) // Аллель удалена
                    item.State = ChangedLocusState.Deleted;
                else if (ch.Name != item.Name || ch.Val != item.Val) // Если аллель находится в наборе и не изменилась - пропускаем
                {
                    item.State = ChangedLocusState.Edited;
                    item.Name = ch.Name;
                    item.Val = ch.Val;
                }
                else
                    continue;
            }
            // Добавляем новые элементы
            foreach (ChangedAllele item in alleliesFromGrid.Where(n => n.Id == 0 && (!String.IsNullOrEmpty(n.Name) || n.Val != 0 )).ToArray())
            {
                item.State = ChangedLocusState.Added;
                _item.Allelies.Add(item);
            }

        }

        // Property
        public bool IsEdit { get; set; }

        // Private fields
        private int _id;
        private LocuseItem _item;
        private LocuseVocabulary _dict;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }

}
