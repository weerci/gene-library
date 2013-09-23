using GeneLibrary.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            if (String.IsNullOrEmpty(tbLocus.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyLocusField);

            _item.Name = tbLocus.Text.Trim();

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

            }
            finally
            {
                this.Cursor = Cursors.Default;
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
