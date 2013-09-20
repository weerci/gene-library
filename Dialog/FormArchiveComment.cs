using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using WFExceptions;
using System.Windows.Forms;

namespace GeneLibrary.Dialog
{
    public partial class FormArchiveComment : Form
    {
        public FormArchiveComment()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxNote.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyComment);
            this.Note = textBoxNote.Text;
        }

       // Fields
        public string Note { get; set; }
    }
}
