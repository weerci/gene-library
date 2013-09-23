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
using System.Data.OracleClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using WFExceptions;
using GeneLibrary.Dialog;

namespace GeneLibrary.MdiForms
{
    public partial class FormHistory : CardForm
    {
        public FormHistory(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        // Events handlers
        private void FormHistory_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;

            FillComboBoxExpert();
            FillComobBoxFields();
            FillContextMenu(contextMenuExpert, ref saveListExpert);

            if (this.id != 0) // Фильтруем данные по конкретной карточке
            {
                textBoxCardsId.Text = this.id.ToString();
                history.GetFilteredCards(new FilterHistoryField[] {
                    new FilterHistoryField("h", HistoryFields.date_ins, dtpFrom.Value.ToShortDateString(), 
                        dtpTo.Value.ToShortDateString(), OracleType.DateTime),
                    new FilterHistoryField("h", HistoryFields.id, id.ToString(), OracleType.Number)
                });
                SaveToTreeViewCard();
            }
            else
            {
                history.GetFilteredCards(new FilterHistoryField[] {
                    new FilterHistoryField("h", HistoryFields.date_ins, dtpFrom.Value.ToShortDateString(), 
                        dtpTo.Value.ToShortDateString(), OracleType.DateTime)
                });
                SaveToTreeViewCard();
            }

        }
        private void FormHistory_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (OnCloseForm != null)
                OnCloseForm(new ComboBoxItem(0, FormId));
        }
        private void buttonFilter_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ClearDataGid();

                List<FilterHistoryField> fields = new List<FilterHistoryField>();

                // Добавляем фильтр по дате
                fields.Add(new FilterHistoryField("h", HistoryFields.date_ins, dtpFrom.Value.ToShortDateString(),
                            dtpTo.Value.ToShortDateString(), OracleType.DateTime));

                // Добавляем фильтр по типу карточки
                if (!checkBoxIkl.Checked && !checkBoxIk2.Checked)
                    fields.Add(new FilterHistoryField("c", HistoryFields.kind_id, 0, OracleType.Number));
                else if (checkBoxIkl.Checked && checkBoxIk2.Checked) { }
                else if (checkBoxIkl.Checked)
                    fields.Add(new FilterHistoryField("c", HistoryFields.kind_id, 1, OracleType.Number));
                else if (checkBoxIk2.Checked)
                    fields.Add(new FilterHistoryField("c", HistoryFields.kind_id, 2, OracleType.Number));

                // Добавляем фильтр по эксперту
                if (comboBoxExpert.SelectedIndex != 0)
                    fields.Add(new FilterHistoryField("h", HistoryFields.expert, ((ComboBoxItem)comboBoxExpert.SelectedItem).Id,
                        OracleType.Number));

                // Добавляем фильтр по карточкам
                if (!String.IsNullOrEmpty(textBoxCardsId.Text))
                {
                    string[] cards = textBoxCardsId.Text.Split(',');
                    if (cards.Count() > 1)
                        fields.Add(new FilterHistoryField("h", HistoryFields.id, cards, OracleType.Number));
                    else
                        fields.Add(new FilterHistoryField("h", HistoryFields.id, textBoxCardsId.Text, OracleType.Number));
                }

                history.GetFilteredCards(fields.ToArray());
                SaveToTreeViewCard();

                if (comboBoxExpert.SelectedIndex != 0)
                {
                    WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + "\\history.dmp", SaveContextMenu(saveListExpert, comboBoxExpert));
                    FillContextMenu(contextMenuExpert, ref saveListExpert);
                }

                tabControlResult.SelectTab(0);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ClearDataGid()
        {
            dgvHistory.DataSource = null;
            dgvHistory.Columns.Clear();
        }
        private void textBoxCardsId_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (Regex.IsMatch(textBox.Text, "[^1234567890, ]"))
                {
                    textBox.ForeColor = Color.Red;
                    Common.Tools.ShowTip(textBox, ErrorsMsg.ErrorFormat,
                        String.Format(ErrorsMsg.NotValidIntegerEnum, label4.Text), ToolTipIcon.Error, 4000);
                }
                else
                {
                    textBox.ForeColor = SystemColors.WindowText;
                }
            }
        }
        private void buttonFind_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (!checkInputFormat())
                    return;

                ClearDataGid();

                if (comboBoxCardFields.SelectedIndex == 0)
                    history.GetSearchedCards(
                        new FilterHistoryField[] { 
                        new FilterHistoryField("c", HistoryFields.note, textBoxFindString.Text.Trim(), OracleType.NVarChar) 
                    },
                        CardFields.none
                     );
                else
                {
                    FilterHistoryField filterField = new FilterHistoryField();
                    filterField.FilterType = TypeHistoryFilter.single;
                    filterField.prefix = "c";
                    filterField.Value.Add(textBoxFindString.Text.Trim());
                    switch (comboBoxCardFields.SelectedIndex)
                    {
                        case 1:
                            filterField.Name = HistoryFields.id;
                            filterField.OracleType = OracleType.Number;
                            break;
                        case 2:
                            filterField.Name = HistoryFields.card_num;
                            filterField.OracleType = OracleType.NVarChar;
                            break;
                        case 3:
                            filterField.Name = HistoryFields.card_num;
                            filterField.OracleType = OracleType.NVarChar;
                            break;
                        case 4:
                            filterField.Name = HistoryFields.date_ins;
                            filterField.OracleType = OracleType.DateTime;
                            break;
                        default:
                            break;
                    }
                    history.GetSearchedCards(new FilterHistoryField[] { filterField },
                        CardFieldsFromComboBox(comboBoxCardFields.SelectedIndex));

                }
                tabControlResult.SelectTab(1);
                SaveToTreeFindCard();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void textBoxFindString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonFind_Click(null, null);
        }
        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode currentNode = ((TreeView)sender).SelectedNode;
            if (currentNode != null)
            {
                int cardId = Convert.ToInt32(currentNode.Name);

                switch (CardItem.CardKind(cardId))
                {
                    case CardKind.ikl:
                        Tools.MainForm().NewIklCard(new IklVocabulary(), cardId);
                        break;
                    case CardKind.ik2:
                        Tools.MainForm().NewIk2Card(new Ik2Vocabulary(), cardId);
                        break;
                }
            }
        }
        private void treeViewCard_Click(object sender, EventArgs e)
        {
            
        }
        private void dgvHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not on button cells. 
            if (e.RowIndex < 0 || e.ColumnIndex != dgvHistory.Columns["Text"].Index)
                return;

            HistoryDlg dlg = new HistoryDlg(dgvHistory.Rows[e.RowIndex]);
            dlg.Show();
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            Common.Tools.GetHelp("mf_history_cards", HelpNavigator.Topic);
        }

        // Events
        new internal event FormInTree OnCloseForm;

        // Private methods
        private void SaveToTreeViewCard()
        {
            treeViewCard.BeginUpdate();
            
            treeViewCard.Nodes.Clear();
            TreeNode newNode = new TreeNode(String.Format("{0} ({1})", resDataNames.treeViewCard, history.DT.Rows.Count));
            foreach (DataRow item in history.DT.Rows)
                newNode.Nodes.Add(item["obj_id"].ToString(), 
                    String.Format("{0} ({1})", item["obj_id"].ToString(), item["name"].ToString()));
            treeViewCard.Nodes.Add(newNode);
            treeViewCard.ExpandAll();
            
            treeViewCard.EndUpdate();
        }
        private void SaveToTreeFindCard()
        {
            treeViewFind.BeginUpdate();

            treeViewFind.Nodes.Clear();
            TreeNode newNode = new TreeNode(String.Format("{0} ({1})", resDataNames.treeViewCard, history.DT.Rows.Count));
            foreach (DataRow item in history.DT.Rows)
                newNode.Nodes.Add(item["id"].ToString(), item["name"].ToString());
            treeViewFind.Nodes.Add(newNode);
            treeViewFind.ExpandAll();

            treeViewFind.EndUpdate();
        }

        private void FillComboBoxExpert()
        {
            comboBoxExpert.Items.Clear();
            experts.Open();
            comboBoxExpert.DisplayMember = "name";
            comboBoxExpert.Items.Add(resDataNames.allExperts);
            comboBoxExpert.Items.AddRange((from DataRow dataRow in experts.ExpTable.Rows
                                            select new GeneLibrary.Common.ComboBoxItem(
                                            Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                            String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}",
                                                dataRow["SURNAME"].ToString(),
                                                dataRow["NAME"].ToString(),
                                                dataRow["PATRONIC"].ToString()
                                            ))).ToArray<GeneLibrary.Common.ComboBoxItem>());
            comboBoxExpert.SelectedIndex = 0;

        }
        private void FillComobBoxFields()
        {
            comboBoxCardFields.Items.Clear();
            comboBoxCardFields.Items.Add(resDataNames.allFields);
            comboBoxCardFields.Items.Add(CardFieldName(CardFields.id));
            comboBoxCardFields.Items.Add(CardFieldName(CardFields.number));
            comboBoxCardFields.Items.Add(CardFieldName(CardFields.uk_number));
            comboBoxCardFields.Items.Add(CardFieldName(CardFields.date_ins));
            comboBoxCardFields.SelectedIndex = 0;
        }
        private static SaveList SaveContextMenu(SaveList saveList, ComboBox comboBox)
        {
            ComboBoxItem comboBoxItem = comboBox.Items[comboBox.SelectedIndex] as ComboBoxItem;
            if (comboBox != null)
            {
                saveList.Add(new SaveItem(comboBoxItem.Id, comboBoxItem.Name));
            }
            return saveList;
        }
        private static void FillContextMenu(ContextMenuStrip contextMenu, ref SaveList saveList)
        {
            saveList = (SaveList)WFSerialize.Deserialize(Path.GetDirectoryName(Application.ExecutablePath) + "\\history.dmp");
            if (saveList != null)
            {
                contextMenu.Items.Clear();
                foreach (SaveItem si in saveList.ListItems)
                    contextMenu.Items.Add(si.Name);
            }
            else
                saveList = new SaveList();
        }
        private void buttonExpert_Click(object sender, EventArgs e)
        {
            Button buttonUK = sender as Button;
            if (buttonUK != null)
                contextMenuExpert.Show(buttonExpert, 0, buttonExpert.Height + 2);
        }
        private void contextMenuExpert_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            comboBoxExpert.SelectedIndex = comboBoxExpert.FindString(e.ClickedItem.Text);
        }
        private string CardFieldName(CardFields cardField)
        {
            switch (cardField)
            {
                case CardFields.id:
                    return resDataNames.ik2TableID;
                case CardFields.number:
                    return String.Format("{0}/{1}", resDataNames.iklTableCardNum, resDataNames.ik2TableCardNum);
                case CardFields.uk_number:
                    return String.Format("{0}", resDataNames.iklTableCrimNum);
                case CardFields.date_ins:
                    return String.Format("{0}", resDataNames.iklTableDateIns);
                default:
                    throw new WFException(ErrType.Assert, String.Format(ErrorsMsg.NotValueInEnum, "CardFields", cardField.ToString()));
            }
        }
        private CardFields CardFieldsFromComboBox(int index)
        {
            switch (index)
            {
                case 0:
                    return CardFields.none;
                case 1:
                    return CardFields.id;
                case 2:
                    return CardFields.number;
                case 3:
                    return CardFields.uk_number;
                case 4:
                    return CardFields.date_ins;
                default:
                    return CardFields.none;
            }
        }
        private bool checkInputFormat()
        { 
            if (comboBoxCardFields.SelectedIndex == 1)
                try
                {
                    Convert.ToInt32(textBoxFindString.Text, CultureInfo.InvariantCulture);
                }
                catch 
                {
                    AwareMessageBox.Show(
                                this,
                                String.Format(ErrorsMsg.NotInteger, label1.Text),
                                Properties.Resources.MessageBoxTitle,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                                (MessageBoxOptions)0);
                    return false;
                }
            if (comboBoxCardFields.SelectedIndex == 4)
                try
                {
                    Convert.ToDateTime(textBoxFindString.Text);
                }
                catch
                {
                    AwareMessageBox.Show(
                                this,
                                String.Format(ErrorsMsg.NotDate, label1.Text),
                                Properties.Resources.MessageBoxTitle,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                                (MessageBoxOptions)0);
                    return false;
                }
            return true;
        }

        // Fields
        private int id;
        private History history = new History();
        private ExpertVocabulary experts = new ExpertVocabulary();
        private SaveList saveListExpert;

        private void treeViewCard_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            ClearDataGid();
        }

        private void treeViewCard_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name != null)
            {
                try
                {
                    history.GetCardHistory(Convert.ToInt32(e.Node.Name));
                }
                catch
                {
                    // Отлов ошибки при клике вне элемента дерева поднятие ошибки наверх не нужно
                    return;
                }

                dgvHistory.Columns.Clear();
                dgvHistory.DataSource = history.HT;

                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Text = "...";
                buttonColumn.HeaderText = "";
                buttonColumn.Name = "Text";
                buttonColumn.UseColumnTextForButtonValue = true;
                buttonColumn.Width = 32;
                dgvHistory.Columns.Add(buttonColumn);

                foreach (DataColumn dc in history.HT.Columns)
                {
                    dgvHistory.Columns[dc.ColumnName].HeaderText = dc.Caption;
                    dgvHistory.Columns[dc.ColumnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                dgvHistory.Columns["ID"].Visible = false;
                dgvHistory.Columns["NOTE"].Visible = false;

            }
        }

    }
}
