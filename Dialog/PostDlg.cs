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
    public partial class PostForm : Form
    {
        // Constructors
        public PostForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this._dict = (PostVocabulary)dictionary;
        }
        public PostForm(Vocabulary dictionary, int id) : this(dictionary)
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
                    this.tbName.Text = _item.Name;
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
            if (String.IsNullOrEmpty(tbName.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyPostNameField);

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
                tbName.Clear();
                tbName.Focus();
                _dict.LoadItem();
                _item = _dict.Item;
            }
        }

        // Property
        public bool IsEdit { get; set; }

        // Private fields
        private int _id;
        private PostItem _item;
        private PostVocabulary _dict;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
