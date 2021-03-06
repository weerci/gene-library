﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.MdiForms;
using WFExceptions;
using GeneLibrary.Items;
using System.Globalization;
using System.Text.RegularExpressions;
using GeneLibrary.Common;

namespace GeneLibrary.Dialog
{
    public partial class MethodForm : Form
    {
        // Constructors
        public MethodForm(Vocabulary dictionary, Point point)
        {
            InitializeComponent();
            this._dict = (MethodVocabulary)dictionary;
        }
        public MethodForm(Vocabulary dictionary, Point point, int id) : this(dictionary, point)
        {
            this.IsEdit = true;
            this._id = id;
        }

        // Events handlers
        private void FreqDlg_Load(object sender, EventArgs e)
        {
            if (this.IsEdit)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    _dict.LoadItem(_id);
                    _methItem = _dict.Item;
                    _methItem.Open();
                    this.tbName.Text = _methItem.Name;
                    this.textBoxFrequency.Text = _methItem.DefFreq.ToString(CultureInfo.InvariantCulture);
                    this.tbDesc.Text = _methItem.Description;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                _dict.LoadItem();
                _methItem = _dict.Item;
                _methItem.Open();
            }
            TurningGrid();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (String.IsNullOrEmpty(tbName.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyFreqNameField);
            if (String.IsNullOrEmpty(textBoxFrequency.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyFreqFreqField);
            if (String.IsNullOrEmpty(tbDesc.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyFreqDescField);

            _methItem.Name = tbName.Text.Trim();
            _methItem.DefFreq = Convert.ToDecimal(textBoxFrequency.Text.Trim(), CultureInfo.InvariantCulture);
            _methItem.Description = tbDesc.Text.Trim();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                _id = _methItem.Save();
                if (OnDataLoad != null)
                    OnDataLoad(_id);

                if (!this.IsEdit)
                {
                    this.IsEdit = true;
                    _methItem.Id = _id;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        private void textBoxFrequency_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (Regex.IsMatch(textBox.Text, "[^1234567890.]"))
                {
                    textBox.ForeColor = Color.Red;
                    Common.Tools.ShowTip(textBoxFrequency, ErrorsMsg.ErrorFormat, String.Format(ErrorsMsg.ErrorNotNumber, label2.Text), ToolTipIcon.Error);
                }
                else
                    textBox.ForeColor = SystemColors.WindowText;
            }
        }
        private void dgrLocus_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                ((DataView)dgrAllele.DataSource).RowFilter = "LOCUS_ID=-1";
            }
            else
            {
                ((DataView)dgrAllele.DataSource).RowFilter = "LOCUS_ID=" + dgrLocus.Rows[e.RowIndex].Cells["id"].Value.ToString();
            }

        }
        //private void dgrAllele_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        //{
        //    DataGridView dataGridView = sender as DataGridView;
        //    if (dataGridView != null)
        //    {
        //        if (dataGridView.SelectedCells.Count > 0)
        //        {
        //            DataGridViewCell dataGridViewCell = dataGridView.SelectedCells[0];

        //            if (Regex.IsMatch(dataGridViewCell.Value.ToString(), "[^1234567890.]"))
        //            {
        //                dataGridViewCell.Style.ForeColor = Color.Red;
        //                Common.Tools.ShowTip(tbDesc, ErrorsMsg.ErrorFormat, String.Format(ErrorsMsg.NotNumber, label2.Text), ToolTipIcon.Error, 4000);
        //            }
        //            else
        //                dataGridViewCell.Style.ForeColor = SystemColors.WindowText;

        //            dgrAllele.CommitEdit(DataGridViewDataErrorContexts.Commit);
        //        }
        //    }
        //}

        #region Обработка ошибок ввода данных в сетку
        private void dgrAllele_KeyUp(object sender, KeyEventArgs e)
        {
            DataGridViewTextBoxEditingControl editBox = sender as DataGridViewTextBoxEditingControl;
            if (editBox != null)
            {
                if (Regex.IsMatch(editBox.Text, "[^1234567890.,]"))
                {
                    editBox.ForeColor = Color.Red;
                    Common.Tools.ShowTip(editBox, ErrorsMsg.ErrorFormat, String.Format(ErrorsMsg.ErrorNotNumber, label2.Text), ToolTipIcon.Error);
                }
                else
                    editBox.ForeColor = SystemColors.WindowText;
            }
        }
        private void dgrAllele_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyUp += new KeyEventHandler(dgrAllele_KeyUp);
        }
        #endregion

        private void dgrAllele_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgrAllele.Columns[e.ColumnIndex].Name.ToUpper() == "freq".ToUpper())
            {
                if (String.IsNullOrEmpty(e.Value.ToString()))
                    e.Value = "По умолчанию";
            }
        }


        // Properties
        public bool IsEdit { get; set; }

        private void TurningGrid()
        {
            DataView dw = new DataView(_methItem.DTAllele);
            dgrAllele.DataSource = dw;

            dgrLocus.DataSource = _methItem.DTLocus;
            dgrLocus.Columns["id"].Visible = false;
            dgrLocus.Columns["name"].HeaderText = resDataNames.freqLocusTableName;
            dgrLocus.CurrentCell = dgrLocus.Rows[0].Cells["name"];

            dw.RowFilter = "LOCUS_ID=" + dgrLocus.CurrentRow.Cells["id"].Value.ToString();
            dgrAllele.Columns["id"].Visible = false;
            dgrAllele.Columns["locus_id"].Visible = false;
            dgrAllele.Columns["name"].HeaderText = resDataNames.freqAlleleTableName;
            dgrAllele.Columns["name"].ReadOnly = true;
            dgrAllele.Columns["val"].HeaderText = resDataNames.freqAlleleTableVal;
            dgrAllele.Columns["val"].ReadOnly = true;
            dgrAllele.Columns["freq"].HeaderText = resDataNames.freqAlleleTableFreq;
            dgrAllele.AllowUserToResizeColumns = false;
            dgrAllele.AllowUserToResizeRows = false;
        }
        private MethodItem _methItem;
        private int _id;
        private MethodVocabulary _dict;
        
        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;


    }
}
