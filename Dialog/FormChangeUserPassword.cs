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
using WFDatabase;

namespace GeneLibrary.Dialog
{
    public partial class FormChangeUserPassword : Form
    {
        public FormChangeUserPassword()
        {
            InitializeComponent();
        }

        // Events
        private void checkBoxShowPasswords_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPasswords.CheckState == CheckState.Checked)
            {
                textBoxConfirmPassword.PasswordChar = '\0';
                textBoxPassword.PasswordChar = '\0';
                textBoxOldPassword.PasswordChar = '\0';
            }
            else
            {
                textBoxConfirmPassword.PasswordChar = '*';
                textBoxPassword.PasswordChar = '*';
                textBoxOldPassword.PasswordChar = '*';
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxPassword.Text) && Tools.ShowMessage(ErrorsMsg.EmptyPassword) != DialogResult.OK)
                return;

            // Проверка правильности введенного пароля
            if (!GateFactory.LogOnBase().CheckPassword(textBoxOldPassword.Text))
                throw new WFException(ErrType.Message, ErrorsMsg.NotValidPassword);

            // Проверка подтверждения пароля
            if (textBoxConfirmPassword.Text != textBoxPassword.Text)
                throw new WFException(ErrType.Message, ErrorsMsg.NotConfirmPassword);

            this.Password = textBoxPassword.Text;
            this.DialogResult = DialogResult.OK;
        }

        // Fields
        public string Password { get; set; }
    }
}
