using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Common;
using WFExceptions;

namespace GeneLibrary.Dialog
{
    public partial class FormChangePassword : Form
    {
        public FormChangePassword()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxPassword.Text) && Tools.ShowMessage(ErrorsMsg.EmptyPassword) != DialogResult.OK)
                return;

            // Проверка подтверждения пароля
            if (textBoxConfirmPassword.Text != textBoxPassword.Text)
                throw new WFException(ErrType.Message, ErrorsMsg.NotConfirmPassword);

            this.Password = textBoxPassword.Text;
            this.DialogResult = DialogResult.OK;
        }

        // Fields
        public String Password { get; set; }

    }
}
