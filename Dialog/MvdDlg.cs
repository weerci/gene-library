using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Items;
using GeneLibrary.MdiForms;
using WFExceptions;

namespace GeneLibrary.Dialog
{
    public partial class MvdForm : Form
    {
        // Constructors
        public MvdForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this._dict = (MvdVocabulary)dictionary;
        }
        public MvdForm(Vocabulary dictionary, int id) : this(dictionary)
        {
            this.IsEdit = true;
            this._id = id;
        }
        
        // Events handlers
        private void MvdDlg_Load(object sender, EventArgs e)
        {
            if (this.IsEdit)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    _dict.LoadItem(_id);
                    _item = _dict.Item;
                    _item.Open();
                    this.tbCode.Text = _item.Code;
                    this.tbShortName.Text = _item.ShortName;
                    this.tbName.Text = _item.Name;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                _dict.LoadItem();
                _item = _dict.Item;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (String.IsNullOrEmpty(tbCode.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyMvdCodeField);
            if (String.IsNullOrEmpty(tbShortName.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyMvdShortNameField);
            if (String.IsNullOrEmpty(tbName.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyMvdNameField);

            _item.Code = tbCode.Text.Trim();
            _item.ShortName = tbShortName.Text.Trim();
            _item.Name = tbName.Text.Trim();

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
                tbShortName.Clear();
                tbName.Clear();
                tbCode.Focus();
                _dict.LoadItem();
                _item = _dict.Item;
            }

        }

        // Properties
        public bool IsEdit { get; set; }

        // Private members
        private MvdItem _item;
        private MvdVocabulary _dict;
        private int _id;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
