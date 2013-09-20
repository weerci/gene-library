using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeneLibrary.Dialog
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(string errorMessage)
        {
            InitializeComponent();
            this.errorMessage = errorMessage;
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {
            textBoxMessage.Text = Properties.Resources.rsGlobErrorMsg;
            textBoxError.Text = errorMessage;
        }

        // Fields
        private string errorMessage;

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
