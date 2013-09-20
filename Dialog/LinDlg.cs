using System;
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

namespace GeneLibrary.Dialog
{
    public partial class LinForm : Form
    {
        // Constructors
        public LinForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this._dict = (LinVocabulary)dictionary;
        }
        public LinForm(Vocabulary dictionary, int id) : this(dictionary)
        {
            this.IsEdit = true;
            this._id = id;
        }

        // Events
        private void LinDlg_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.IsEdit)
                {
                    _dict.LoadItem(_id);
                    _item = _dict.Item;
                    _item.Open();
                    this.tbCode.Text = _item.Code;
                    this.tbOrgan.Text = _item.Organ;
                }
                else
                {
                    _dict.LoadItem();
                    _item = _dict.Item;
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
            if (String.IsNullOrEmpty(tbCode.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyLinCodeField);
            if (String.IsNullOrEmpty(tbOrgan.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyLinOrganField);

            _item.Code = tbCode.Text.Trim();
            _item.Organ = tbOrgan.Text.Trim();

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
                tbCode.Clear();
                tbOrgan.Clear();
                tbCode.Focus();
                _dict.LoadItem();
                _item = _dict.Item;
            }
        }

        // Properties
        public bool IsEdit { get; set; }

        // Private members
        private int _id;
        private LinItem _item;
        private LinVocabulary _dict;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
