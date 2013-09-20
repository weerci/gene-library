using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WFExceptions;
using GeneLibrary.Items;

namespace GeneLibrary.Dialog
{
    public partial class OrganForm : Form
    {
        // Constructors
        public OrganForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this._dict = (OrganizationVocabulary)dictionary;
        }
        public OrganForm(Vocabulary dictionary, int id) : this(dictionary)
        {
            this.IsEdit = true;
            this._id = id;
        }

        // Events handler
        private void PostDlg_Load(object sender, EventArgs e)
        {
            if (this.IsEdit)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    _dict.LoadItem(_id);
                    _item = _dict.Item;
                    _item.Open();
                    this.tbNote.Text = _item.Note;
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
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (String.IsNullOrEmpty(tbNote.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyPostNameField);

            _item.Note = tbNote.Text.Trim();

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

            if (this.IsEdit)
                this.DialogResult = DialogResult.OK;
            else
            {
                tbNote.Clear();
                tbNote.Focus();
                _dict.LoadItem();
                _item = _dict.Item;
            }
        }

        // Property
        public bool IsEdit { get; set; }

        // Private fields
        private int _id;
        private OrganizationItem _item;
        private OrganizationVocabulary _dict;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
