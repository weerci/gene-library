using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Dialog;
using GeneLibrary.Common;

namespace GeneLibrary.MdiForms
{
    public partial class FormFilter : CardForm
    {
        public FormFilter(string formNameId)
        {
            InitializeComponent();
        }
        
        // Events handler
        private void FormFilter_Load(object sender, EventArgs e)
        {
            EnabledControls();
        }
        private void toolStripButtonToExcel_Click(object sender, EventArgs e)
        {
            Tools.ToExcel(resDataNames.CombineReport + ", " + Properties.Resources.DataOn + " " + DateTime.Today.ToLongDateString(), new System.Drawing.Point(3, 1), dataGridViewFilter, ExcelCellOrientation.Horizontal);
        }
        private void FormFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnCloseForm != null)
                OnCloseForm(new ComboBoxItem(0, base.FormId));
        }
 
        // Private members
        public void EnabledControls()
        {
            bool emptyTable = dataGridViewFilter.Rows.Count == 0;
            toolStripButtonToExcel.Enabled = !emptyTable;
        }

        // Properties
        public DataGridView DataGridFilter { get { return this.dataGridViewFilter; } }

        // Events
        internal event FormInTree OnCloseForm;

    }

}
