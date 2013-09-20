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
using WFExceptions;
using System.IO;

namespace GeneLibrary.Dialog
{
    public partial class OrganizationForm : Form
    {
        // Constructors
        public OrganizationForm(OrganizationVocabulary organizationVocabulary)
        {
            InitializeComponent();
            this.organizationVocabulary = organizationVocabulary;
        }

        // Events handlers
        private void OrganizationForm_Load(object sender, EventArgs e)
        {
            // Сбрасываем ранее установленные фильтры
            organizationVocabulary.DT.DefaultView.RowFilter = "";
            dataGridViewOrganization.DataSource = organizationVocabulary.DT;
            foreach (DataColumn dc in organizationVocabulary.DT.Columns)
            {
                DataGridViewColumn dataGridViewColumn = dataGridViewOrganization.Columns[dc.ColumnName];
                if (dc.ColumnName == "NOTE")
                    dataGridViewColumn.HeaderText = dc.Caption;
                else
                    dataGridViewColumn.Visible = false;
            }

            // Выбираем первую запись
            if (organizationVocabulary.DT.Rows.Count > 0)
                dataGridViewOrganization.CurrentCell = dataGridViewOrganization.Rows[dataGridViewOrganization.FirstDisplayedCell.RowIndex].
                    Cells[dataGridViewOrganization.FirstDisplayedCell.ColumnIndex];

            // Загружаем наиболее часто используемые организации
            saveListOrganization = (SaveList)WFSerialize.Deserialize(Path.GetDirectoryName(Application.ExecutablePath) + "\\FavoriteOrganization.dmp");
            if (saveListOrganization != null)
                foreach (SaveItem si in saveListOrganization.ListItems)
                    contextMenuStripOrganization.Items.Add(si.Name);
            else
                saveListOrganization = new SaveList();

        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (OnDataUpdate != null && dataGridViewOrganization.CurrentRow != null)
            {
                OnDataUpdate(
                    Convert.ToInt32(dataGridViewOrganization.CurrentRow.Cells["ID"].Value, CultureInfo.InvariantCulture),
                    dataGridViewOrganization.CurrentRow.Cells["NOTE"].Value.ToString()
                    );

                WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + "\\FavoriteOrganization.dmp", SaveOrganization());

                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;
        }
        private void dataGridViewOrganization_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnOk_Click(null, null);
        }
        private void textBoxFind_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox != null)
            {
                try
                {
                    if (String.IsNullOrEmpty(textBox.Text.Trim()))
                        organizationVocabulary.DT.DefaultView.RowFilter = "";
                    else
                        organizationVocabulary.DT.DefaultView.RowFilter = " note like '*" + textBox.Text + "*'";
                }
                catch
                {
                    throw new WFException(ErrType.Message, ErrorsMsg.NotValidFindSimbol);
                }
            }
        }
        private void buttonFavoritOrganization_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
                contextMenuStripOrganization.Show(button, 0, button.Height + 2);
        }
        private void dataGridViewOrganization_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOk_Click(null, null);
        }

        // Private members
        private SaveList SaveOrganization()
        {
            SaveItem saveItem = new SaveItem(
                    Convert.ToInt32(dataGridViewOrganization.CurrentRow.Cells["ID"].Value, CultureInfo.InvariantCulture),
                    dataGridViewOrganization.CurrentRow.Cells["NOTE"].Value.ToString());
            if (saveListOrganization != null)
            {
                saveListOrganization.Add(saveItem);
            }
            return saveListOrganization;
        }

        //Fields
        private OrganizationVocabulary organizationVocabulary;
        private SaveList saveListOrganization;

        // Events
        internal delegate void OrganizationSelected(int id, string note);
        internal event OrganizationSelected OnDataUpdate;

        private void contextMenuStripOrganization_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
            var result = from DataRow dataRow in organizationVocabulary.DT.Rows
                         where dataRow["NOTE"].ToString() == e.ClickedItem.Text
                         select Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture);

            if (result.Count() > 0)
            {
                organizationVocabulary.DT.DefaultView.Sort = "id";
                int idx = organizationVocabulary.DT.DefaultView.Find(result.First<int>());
                if (idx != -1)
                    dataGridViewOrganization.CurrentCell = dataGridViewOrganization.Rows[idx].Cells[dataGridViewOrganization.FirstDisplayedCell.ColumnIndex];
            }
        }

    }
}
