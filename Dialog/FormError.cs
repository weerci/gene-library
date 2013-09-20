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
    public partial class FormError : Form
    {
        public FormError(string formCaption)
        {
            InitializeComponent();
            this.formCaption = formCaption;
        }


        // Fields
        private string formCaption;
    }
}
