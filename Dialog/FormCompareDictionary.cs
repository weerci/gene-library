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
using System.Globalization;

namespace GeneLibrary.Dialog
{
    public partial class FormCompareDictionary : Form
    {
        // Constructors
        public FormCompareDictionary(int columnCount, Vocabulary vocabulary)
        {
            InitializeComponent();
            this.columnCount = columnCount;
            this.vocabulary = vocabulary;
        }

        // Drag and drop
        private void FormCompareDictionary_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < this.columnCount; i++)
            {
                dataGridViewCompared.Columns.Add("", "");
            }
            EnabledControls();
        }
        private void textBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(CompareTableRow)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        private void textBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(CompareTableRow)))
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    CompareTableRow rows = (CompareTableRow)e.Data.GetData(typeof(CompareTableRow));
                    if (textBox.Name == "textBoxComparer" && rows.Rows.Count > 0)
                        textBox.Text = String.Format(
                                "{0} {1}",
                                rows.Rows[0].Cells[rows.ColumnCaption[0]].Value.ToString(),
                                rows.Rows[0].Cells[rows.ColumnCaption[1]].Value.ToString()
                                );
                    else
                        foreach (DataGridViewRow row in rows.Rows)
                        {
                            textBox.Text += String.Format(
                                "{0} {1}\r\n",
                                row.Cells[rows.ColumnCaption[0]].Value.ToString(),
                                row.Cells[rows.ColumnCaption[1]].Value.ToString()
                                );
                        }
                }
            }
        }
        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(CompareTableRow)))
            {
                int indexAdded;
                CompareTableRow rows = (CompareTableRow)e.Data.GetData(typeof(CompareTableRow));
                foreach (DataGridViewRow row in rows.Rows)
                {
                    DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
                    for (Int32 index = 0; index < row.Cells.Count; index++)
                         clonedRow.Cells[index].Value = row.Cells[index].Value;
                    
                    indexAdded = dataGridViewCompared.Rows.Add(clonedRow);
                    //if (rows.CardKind == CardKind.ikl)
                    //    dataGridViewCards.Rows[indexAdded].DefaultCellStyle.BackColor = Color.FromArgb(182, 189, 210); //Warning!!! 
                    //This color used for define kind of card. If you are changed this color, you are need change and that code too. Ctlr+F can help you!
                }
            }
            EnabledControls();
        }
        private void textBoxComparer_TextChanged(object sender, EventArgs e)
        {
            EnabledControls();
        }
        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow[] rows = new DataGridViewRow[dataGridViewCompared.SelectedRows.Count];
            dataGridViewCompared.SelectedRows.CopyTo(rows, 0);
            foreach (DataGridViewRow row in rows)
                dataGridViewCompared.Rows.Remove(row);
            EnabledControls();
        }
        private void toolStripButtonCompare_Click(object sender, EventArgs e)
        {
            List<int> listItems = new List<int>();
            foreach (DataGridViewRow item in dataGridViewCompared.Rows)
            {
                listItems.Add(Convert.ToInt32(item.Cells[0].Value, CultureInfo.InvariantCulture));
            }
            bool result = vocabulary.CompareDictionary(
                Convert.ToInt32(textBoxComparer.Text.Split(' ')[0], CultureInfo.InvariantCulture),
                listItems.ToArray()
                );
            if (result)
            {
                dataGridViewCompared.DataMember = null;
                if (OnVacabularyUpdate != null)
                    OnVacabularyUpdate(Convert.ToInt32(textBoxComparer.Text.Split(' ')[0], CultureInfo.InvariantCulture));
                EnabledControls();
                Tools.ShowMessage(ErrorsMsg.VocabulareCompared);
            }
            else
                Tools.ShowMessage(ErrorsMsg.VocabulareNotCompared);
        }


        // Private members
        private void EnabledControls()
        {
            bool emptyGrid = dataGridViewCompared.Rows.Count == 0;
            bool emptyTextBox = String.IsNullOrEmpty(textBoxComparer.Text.Trim());
            bool notSelectedRows = dataGridViewCompared.SelectedRows.Count == 0;

            toolStripButtonCompare.Enabled = !emptyGrid && !emptyTextBox;
            toolStripButtonDelete.Enabled = !notSelectedRows;
        }

        // Fields
        private int columnCount;
        private Vocabulary vocabulary;

        // Events
        internal event GeneLibrary.Common.UpdateId OnVacabularyUpdate;

    }
}
