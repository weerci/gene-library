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
using WFExcel;
using System.Collections.ObjectModel;
using System.IO;

namespace GeneLibrary.Dialog
{
    public partial class FormGeneTable : Form
    {
        public FormGeneTable()
        {
            InitializeComponent();
        }

        // Menu handers
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void dataGridViewCards_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(GeneTableRow)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        private void dataGridViewCards_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(GeneTableRow)))
            {
                int indexAdded;
                GeneTableRow rows = (GeneTableRow)e.Data.GetData(typeof(GeneTableRow));
                foreach (DataGridViewRow row in rows.Rows)
                {
                    DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
                    for (Int32 index = 0; index < row.Cells.Count; index++)
                    {
                        if (index == 4)
                            clonedRow.Cells[index].Value = ((DateTime)row.Cells[index].Value).ToShortDateString();
                        else
                            clonedRow.Cells[index].Value = row.Cells[index].Value;
                    }
                    indexAdded = dataGridViewCards.Rows.Add(clonedRow);
                    if (rows.CardKind == CardKind.ikl)
                        dataGridViewCards.Rows[indexAdded].DefaultCellStyle.BackColor = Color.FromArgb(182, 189, 210); //Warning!!! 
                    //This color used for define kind of card. If you are changed this color, you are need change and that code too. Ctlr+F can help you!
                }
                EnabledControl();
            }
        }

        private void listBoxSelectedCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnabledControl();
        }
        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow[] rows = new DataGridViewRow[dataGridViewCards.SelectedRows.Count];
            dataGridViewCards.SelectedRows.CopyTo(rows, 0);
            foreach (DataGridViewRow row in rows)
                dataGridViewCards.Rows.Remove(row);
            EnabledControl();
        }
        private void dataGridViewCards_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            EnabledControl();
        }
        private void toolStripTextBoxVertical_Click(object sender, EventArgs e)
        {

        }
        private void toolStripTextBoxHorisontal_Click(object sender, EventArgs e)
        {

        }
        private void tabControlCards_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (tabControlCards.SelectedTab.Name == "tabPagePreview")
                {
                    FillPreview();
                    dataGridViewPreview.Columns.Clear();
                    dataGridViewPreview.Rows.Clear();
                    foreach (string columnName in listLocusesName.Distinct().OrderBy(locusName => locusName))
                    {
                        int indexColumn = dataGridViewPreview.Columns.Add(columnName, columnName);
                        dataGridViewPreview.Columns[indexColumn].Width = 75;
                    }

                    foreach (Vocabulary vocabulary in listVocabulary)
                    {
                        int indexRow = dataGridViewPreview.Rows.Add();
                        #region Заполнение ИК-2
                        Ik2Vocabulary ik2Vocabulary = vocabulary as Ik2Vocabulary;
                        if (ik2Vocabulary != null)
                        {
                            dataGridViewPreview.Rows[indexRow].HeaderCell.Value = ik2Vocabulary.Item.Object;
                            foreach (Locus locus in (from Locus innerLocus in ik2Vocabulary.Item.Profile.Locus
                                                         where innerLocus.CheckedAlleleCount > 0
                                                         select innerLocus))
                            {
                                DataGridViewColumn column = dataGridViewPreview.Columns[locus.Name];
                                if (column != null)
                                    dataGridViewPreview.Rows[indexRow].Cells[column.Index].Value = 
                                        (from Allele allele in locus.Allele 
                                            where allele.Checked
                                            orderby Tools.AlleleConvert(allele.Name)
                                            select allele.Name).Aggregate(
                                            (item, next) => item + ", "+next
                                            );
                            }
                        }
                        #endregion
                        else
                        {
                        #region Заполнение ИКЛ
                            IklVocabulary iklVocabulary = vocabulary as IklVocabulary;
                            if (iklVocabulary != null)
                            {
                                dataGridViewPreview.Rows[indexRow].HeaderCell.Value = iklVocabulary.Item.Person;
                                foreach (Locus locus in (from Locus innerLocus in iklVocabulary.Item.Profile.Locus 
                                                             where innerLocus.CheckedAlleleCount > 0
                                                             select innerLocus))
                                {
                                    DataGridViewColumn column = dataGridViewPreview.Columns[locus.Name];
                                    if (column != null)
                                        dataGridViewPreview.Rows[indexRow].Cells[column.Index].Value =
                                            (from Allele allele in locus.Allele
                                             where allele.Checked
                                             orderby Tools.AlleleConvert(allele.Name)
                                             select allele.Name).Aggregate(
                                                (item, next) => item + ", " + next
                                                );
                                }
                            }
                            #endregion
                        }
                    }
                }
                EnabledControl();
            }
            finally
            {
                dataGridViewPreview.AutoResizeRowHeadersWidth(
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader);

                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripSplitButtonPrint_ButtonClick(object sender, EventArgs e)
        {
        }
        private void toolStripMenuItemVertical_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Collection<ReportField> reportField = new Collection<ReportField>();

                reportField.Add(new ReportField("#ДАТА", DateTime.Today.ToShortDateString()));

                ReportToExcel reportToExcel = new ReportToExcel(
                    Path.GetDirectoryName(Application.ExecutablePath) + "\\Шаблоны\\таблица_генотипов.xlt");

                Collection<Collection<string>> reportBody = new Collection<Collection<string>>();
                for (int i = 0; i < dataGridViewPreview.Columns.Count; i++)
                {
                    Collection<string> rowCell = new Collection<string>();
                    for (int j = 0; j < dataGridViewPreview.Rows.Count; j++)
                        rowCell.Add(Tools.AsNullString(dataGridViewPreview.Rows[j].Cells[i].Value));
                    reportBody.Add(rowCell);
                }

                Collection<string> header = new Collection<string>(
                    (from DataGridViewColumn column in dataGridViewPreview.Columns select column.HeaderText)
                    .ToList<string>());
                Collection<string> rows = new Collection<string>(
                    (from DataGridViewRow row in dataGridViewPreview.Rows
                     select row.HeaderCell.Value.ToString())
                    .ToList<string>());

                reportToExcel.LoadWithHeaders(reportField, reportBody, rows, header);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripMenuItemHorizontal_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Collection<ReportField> reportField = new Collection<ReportField>();

                reportField.Add(new ReportField("#ДАТА", DateTime.Today.ToShortDateString()));

                ReportToExcel reportToExcel = new ReportToExcel(
                    Path.GetDirectoryName(Application.ExecutablePath) + "\\Шаблоны\\таблица_генотипов.xlt");

                Collection<Collection<string>> reportBody = new Collection<Collection<string>>();
                foreach (DataGridViewRow row in dataGridViewPreview.Rows)
                {
                    Collection<string> rowCells = new Collection<string>((from DataGridViewCell cell in row.Cells
                                                                          select Tools.AsNullString(cell.Value)).ToList<string>());
                    reportBody.Add(rowCells);
                }

                Collection<string> header = new Collection<string>(
                    (from DataGridViewColumn column in dataGridViewPreview.Columns select column.HeaderText)
                    .ToList<string>());
                Collection<string> rows = new Collection<string>(
                    (from DataGridViewRow row in dataGridViewPreview.Rows
                     select row.HeaderCell.Value.ToString())
                    .ToList<string>());

                reportToExcel.LoadWithHeaders(reportField, reportBody, header, rows);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        // Private methdos
        private void EnabledControl() 
        {
            bool gridNotEmpty = dataGridViewCards.Rows.Count > 0;
            bool rowsIsSelected = dataGridViewCards.SelectedRows.Count > 0;
            bool tabPreviewActive = tabControlCards.SelectedTab.Name == "tabPagePreview";

            toolStripButtonDelete.Enabled = rowsIsSelected && !tabPreviewActive;
            toolStripSplitButtonPrint.Enabled = gridNotEmpty && tabPreviewActive;
        }
        private void FillPreview()
        {
            listLocusesName.Clear();
            listVocabulary.Clear();
            foreach (DataGridViewRow row in dataGridViewCards.Rows)
            {
                if (row.DefaultCellStyle.BackColor == Color.FromArgb(182, 189, 210))
                {
                    IklVocabulary iklVocabulary = new IklVocabulary();
                    iklVocabulary.LoadItem(Convert.ToInt32(row.Cells["id"].Value, CultureInfo.InvariantCulture));
                    iklVocabulary.Item.Open();
                    listVocabulary.Add(iklVocabulary);
                    listLocusesName.AddRange((
                        from Locus locusInner in iklVocabulary.Item.Profile.Locus
                        where locusInner.CheckedAlleleCount > 0
                        select locusInner.Name).ToArray<string>()
                        );
                }
                else
                {
                    Ik2Vocabulary ik2Vocabulary = new Ik2Vocabulary();
                    ik2Vocabulary.LoadItem(Convert.ToInt32(row.Cells["id"].Value, CultureInfo.InvariantCulture));
                    ik2Vocabulary.Item.Open();
                    listVocabulary.Add(ik2Vocabulary);
                    listLocusesName.AddRange((
                        from Locus locusInner in ik2Vocabulary.Item.Profile.Locus
                        where locusInner.CheckedAlleleCount > 0
                        select locusInner.Name).ToArray<string>()
                        );
                }
            }
        }

        // Private members
        private List<Vocabulary> listVocabulary = new List<Vocabulary>();
        private List<string> listLocusesName = new List<string>();
    }
}
