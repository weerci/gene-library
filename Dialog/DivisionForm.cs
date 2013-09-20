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
    public partial class DivisionForm : Form
    {
        // Constructors
        public DivisionForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this.dictionary = (DivisionVocabulary)dictionary;
        }
        public DivisionForm(Vocabulary dictionary, int id) : this(dictionary)
        {
            this.IsEdit = true;
            this.id = id;
        }

        // Properties
        public bool IsEdit { get; set; }

        // Event handler
        private void DivDlg_Load(object sender, EventArgs e)
        {
            if (this.IsEdit)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.dictionary.LoadItem(this.id);
                    this.item = this.dictionary.Item;
                    this.item.Open();
                    this.tbName.Text = this.item.Name;
                    this.tbAddress.Text = this.item.Address;
                    this.tbPhone.Text = this.item.Phone;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                this.dictionary.LoadItem();
                this.item = this.dictionary.Item;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (String.IsNullOrEmpty(tbName.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyDivNameField);
            if (String.IsNullOrEmpty(tbAddress.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyDivAddrField);
            if (String.IsNullOrEmpty(tbPhone.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyDivPhoneField);

            this.item.Name = tbName.Text.Trim();
            this.item.Address = tbAddress.Text.Trim();
            this.item.Phone = tbPhone.Text.Trim();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.id = this.item.Save();
                if (OnDataLoad != null)
                    OnDataLoad(this.id);
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
                tbAddress.Clear();
                tbPhone.Clear();
                tbName.Focus();
                this.dictionary.LoadItem();
                this.item = this.dictionary.Item;
            }
        }
        
        // Private
        private int id;
        private DivisionItem item;
        private DivisionVocabulary dictionary;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;
        
    }
}
