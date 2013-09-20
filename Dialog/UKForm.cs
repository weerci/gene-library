using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WFExceptions;
using GeneLibrary.MdiForms;
using GeneLibrary.Items;

namespace GeneLibrary.Dialog
{
    public partial class UKForm : Form
    {
        // Constructors
        public UKForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this._dict = (UKVocabulary)dictionary;
        }
        public UKForm(Vocabulary dictionary, int id) : this(dictionary)
        {
            this.IsEdit = true;
            this._id = id;
        }

        // Event handlers
        private void dgrParts_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["parent_id"].Value = _item.ParentId;
        }
        private void UKDlg_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.IsEdit)
                {
                    _dict.LoadItem(_id);
                    _item = _dict.Item;
                    _item.Open();

                    this.tbArtcl.Text = _item.Article;
                    if (_item.IsArticle)
                    {
                        this.tbNote.Text = _item.Note;
                        tcUK.TabPages.Remove(tpTexts);
                    }
                    else
                    {
                        this.tbText.Text = _item.Note;
                        tcUK.TabPages.Remove(tpArtcl);
                    }
                }
                else
                {
                    _dict.LoadItem();
                    _item = _dict.Item;
                    _item.Open();

                    DataRow newRow = _item.UKDataTable.NewRow();
                    newRow["id"] = 1;
                    newRow["level"] = 1;
                    newRow["parent_id"] = 0;
                    newRow["artcl"] = tbArtcl.Text;
                    newRow["note"] = tbNote.Text;

                    _item.UKDataTable.Rows.Add(newRow);
                }

                if (_item.IsArticle)
                {
                    DataView parts = new DataView(_item.UKDataTable);
                    parts.RowFilter = "level=2";
                    dgrParts.DataSource = parts;
                    dgrParts.Columns["id"].Visible = false;
                    dgrParts.Columns["level"].Visible = false;
                    dgrParts.Columns["parent_id"].Visible = false;
                    dgrParts.Columns["artcl"].HeaderText = resDataNames.ukTableArtcle;
                    dgrParts.Columns["note"].HeaderText = resDataNames.ukTableName;

                    DataView item = new DataView(_item.UKDataTable);
                    item.RowFilter = "level=3";
                    dgrItems.DataSource = item;
                    dgrItems.Columns["id"].Visible = false;
                    dgrItems.Columns["level"].Visible = false;
                    dgrItems.Columns["parent_id"].Visible = false;
                    dgrItems.Columns["artcl"].HeaderText = resDataNames.ukTableArtcle;
                    dgrItems.Columns["note"].HeaderText = resDataNames.ukTableName;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (tcUK.SelectedTab.Name == "tpArtcl")
            {
                if (String.IsNullOrEmpty(tbArtcl.Text.Trim()))
                    throw new WFException(ErrType.Message, ErrorsMsg.EmptyUKArtclField);
                if (String.IsNullOrEmpty(tbNote.Text.Trim()))
                    throw new WFException(ErrType.Message, ErrorsMsg.EmptyUKNoteField);
                _item.Article = this.tbArtcl.Text;
                _item.Note = this.tbNote.Text;
            }
            else
            {
                if (String.IsNullOrEmpty(tbText.Text.Trim()))
                    throw new WFException(ErrType.Message, ErrorsMsg.EmptyUKTextField);
                _item.Text = this.tbText.Text;
            }

            // Сохранение данных
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _id = _item.Save();
                if (OnDataLoad != null)
                    OnDataLoad(_id);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            // Закрытие формы (при редактировании записи) или очистка полей (для внесения новой записи)
            if (this.IsEdit)
                this.DialogResult = DialogResult.OK;
            else
            {
                if (tcUK.SelectedTab.Name == "tpArtcl")
                {
                    tbArtcl.Clear();
                    tbNote.Clear();
                    tbArtcl.Focus();
                }
                else
                {
                    tbText.Clear();
                    tbText.Focus();
                }
                _dict.LoadItem();
                _item = _dict.Item;
                _item.Open();
            }
        }
        
        // Properties
        public bool IsEdit { get; set; }

        // Private members
        private int _id;
        private UKItem _item;
        private UKVocabulary _dict;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
