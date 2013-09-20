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
using WFExceptions;

namespace GeneLibrary.Dialog
{
    public partial class FormSortObject : Form
    {
        // Constructors
        public FormSortObject(Vocabulary dictionary)
        {
            InitializeComponent();
            this.vocabulary = (SortObjectVocabulary)dictionary;
        }
        public FormSortObject(Vocabulary dictionary, int id)
            : this(dictionary)
        {
            this.IsEdit = true;
            this.id = id;
        }

        // Events
        private void FormSortObject_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.IsEdit)
                {
                    vocabulary.LoadItem(this.id);
                    item = vocabulary.Item;
                    item.Open();
                    this.textBoxName.Text = item.Name;
                    this.textBoxShortName.Text = item.ShortName;
                }
                else
                {
                    vocabulary.LoadItem();
                    item = vocabulary.Item;
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
            if (String.IsNullOrEmpty(textBoxName.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptySortObjectNameField);

            item.Name = textBoxName.Text.Trim();
            item.ShortName = textBoxShortName.Text.Trim();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.id = item.Save();
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
                textBoxName.Clear();
                textBoxShortName.Clear();
                textBoxName.Focus();
                vocabulary.LoadItem();
                item = vocabulary.Item;
            }
        }

        // Properties
        public bool IsEdit { get; set; }

        // Private members
        private int id;
        private SortObjectItem item;
        private SortObjectVocabulary vocabulary;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
