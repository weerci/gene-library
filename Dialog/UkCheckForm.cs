using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Items;
using GeneLibrary.Common;
using System.Globalization;
using System.IO;


namespace GeneLibrary.Dialog
{
    public partial class UkCheckForm : Form
    {
        public UkCheckForm()
        {
            InitializeComponent();
        }

        // Event handers
        private void UkCheckForm_Load(object sender, EventArgs e)
        {
            ukVocablary = new UKVocabulary();
            ukVocablary.Open();
            Tools.ClearComboBox(comboBoxState);
            comboBoxState.Items.AddRange(
                (from DataRow row in ukVocablary.UKTable.Rows
                 where row["artcl"].ToString() != "0"
                 select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture),
                     resDataNames.shortState + row["artcl"].ToString(), row["note"].ToString())
                ).ToArray<ComboBoxItem>());
            Tools.ClearComboBox(comboBoxTexts);
            comboBoxTexts.Items.AddRange(
                (from DataRow row in ukVocablary.UKTable.Rows
                 where row["artcl"].ToString() == "0"
                 select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture),
                     row["note"].ToString(), row["note"].ToString())
                ).ToArray<ComboBoxItem>()
                );
            Tools.ClearComboBox(comboBoxPart);
            Tools.ClearComboBox(comboBoxPoint);
            
            if (comboBoxState.Items.Count > 0)
                comboBoxState.SelectedIndex = 0;
            if (comboBoxTexts.Items.Count > 0)
                comboBoxTexts.SelectedIndex = 0;
            
            FillContextMenu(contextMenuStrip, ref saveListUk);
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (listBoxState.Items.Count > 0)
            {
                if (OnCheckedClose != null)
                    OnCheckedClose((from ComboBoxItem comboBoxItem in selectedItems select comboBoxItem.Id).ToList<int>(),
                        Common.Tools.UkList((from ItemList itemList in listBoxState.Items select itemList.Text).ToArray<string>()));

                // Сохранение выбранного элементв в списке часто используемых
                WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + filePath, SaveContextMenu(saveListUk, comboBoxState));
            }
            else
            {
                OnCheckedClose(null, "");
            }
            

            
            this.DialogResult = DialogResult.OK;
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            switch (comboBox.Name)
            {
                case "comboBoxState":
                    buttonAddState.Enabled = comboBox.SelectedIndex > 0;
                    Tools.ClearComboBox(comboBoxPart);
                    Tools.ClearComboBox(comboBoxPoint);
                    // Загружаем данные по частям
                    ukVocablary.LoadItem(((ComboBoxItem)comboBox.SelectedItem).Id);
                    ukItem = ukVocablary.Item;
                    if (ukItem != null)
                    {
                        ukItem.Open();
                        int[] r = {1, 2};
                        comboBoxPart.Items.AddRange(
                            (from DataRow row in ukItem.UKDataTable.Rows
                             where Convert.ToInt32(row["level"], CultureInfo.InvariantCulture) == 2
                             select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture), 
                                 resDataNames.shortPart + row["artcl"].ToString(), row["note"].ToString())
                            ).ToArray<ComboBoxItem>());
                        textBoxItem.Clear();
                        textBoxItem.Text = ((ComboBoxItem)comboBoxState.SelectedItem).Short;
                    }
                    break;
                case "comboBoxPart":
                    // Загружаем данные по пунктам
                    Tools.ClearComboBox(comboBoxPoint);
                    if (ukItem != null)
                    {
                        comboBoxPoint.Items.AddRange(
                            (from DataRow row in ukItem.UKDataTable.Rows
                             where Convert.ToInt32(row["level"], CultureInfo.InvariantCulture) == 3
                             && Convert.ToInt32(row["parent_id"], CultureInfo.InvariantCulture) == ((ComboBoxItem)comboBoxPart.SelectedItem).Id
                             select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture),
                                 resDataNames.shortPoint + row["artcl"].ToString(), row["note"].ToString())
                            ).ToArray<ComboBoxItem>());
                        textBoxItem.Clear();
                        textBoxItem.Text = ((ComboBoxItem)comboBoxState.SelectedItem).Short + "\r\n"+
                            ((ComboBoxItem)comboBoxPart.SelectedItem).Short;
                    }
                    break;
                case "comboBoxPoint":
                    textBoxItem.Clear();
                    if (ukItem != null)
                    {
                        textBoxItem.Text = ((ComboBoxItem)comboBoxState.SelectedItem).Short + "\r\n" +
                        ((ComboBoxItem)comboBoxPart.SelectedItem).Short + "\r\n" +
                        ((ComboBoxItem)comboBoxPoint.SelectedItem).Short;
                    }
                    break;
                case "comboBoxTexts":
                    buttonAddText.Enabled = comboBox.SelectedIndex > 0;
                    break;
                default:
                    break;
            }
        }
        private void buttonAddState_Click(object sender, EventArgs e)
        {
            if (comboBoxPoint.SelectedIndex != 0)
            {
                if (!selectedItems.Contains(comboBoxPoint.SelectedItem))
                {
                    ComboBoxItem selectedItem = comboBoxPoint.SelectedItem as ComboBoxItem;
                    if (selectedItem != null)
                    {
                        selectedItems.Add(selectedItem);
                        listBoxState.Items.Add(new ItemList(selectedItem, comboBoxState.Text + " " + comboBoxPart.Text + " " + comboBoxPoint.Text));
                    }
                }
            }
            else if (comboBoxPart.SelectedIndex != 0)
                {
                    if (!selectedItems.Contains(comboBoxPart.SelectedItem))
                    {
                        ComboBoxItem selectedItem = comboBoxPart.SelectedItem as ComboBoxItem;
                        if (selectedItem != null)
                        {
                            selectedItems.Add(selectedItem);
                            listBoxState.Items.Add(new ItemList(selectedItem, comboBoxState.Text + " " + comboBoxPart.Text));
                        }
                    }
                }
            else if (comboBoxState.SelectedIndex != 0)
                {
                    if (!selectedItems.Contains(comboBoxState.SelectedItem))
                    {
                        ComboBoxItem selectedItem = comboBoxState.SelectedItem as ComboBoxItem;
                        if (selectedItem != null)
                        {
                            selectedItems.Add(selectedItem);
                            listBoxState.Items.Add(new ItemList(selectedItem, comboBoxState.Text));
                        }
                    }
                }
        }
        private void buttonAddText_Click(object sender, EventArgs e)
        {
            if (comboBoxTexts.SelectedItem != null)
            {
                ComboBoxItem selectedItem = comboBoxTexts.SelectedItem as ComboBoxItem;
                if (!selectedItems.Contains(comboBoxTexts.SelectedItem))
                {
                    selectedItems.Add(selectedItem);
                    listBoxState.Items.Add(new ItemList(selectedItem, comboBoxTexts.Text));
                }
            }
        }
        private void listBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxState.SelectedItem != null)
                buttonDelItem.Enabled = true;                    
        }
        private void buttonDelItem_Click(object sender, EventArgs e)
        {
            if (listBoxState.SelectedItem != null)
            { 
                ItemList itemList = listBoxState.SelectedItem as ItemList;
                if (itemList != null)
                {
                    if (selectedItems.Contains(itemList.Obj))
                    {
                        selectedItems.Remove(itemList.Obj);
                        listBoxState.Items.Remove(listBoxState.SelectedItem);
                    }
                }
            }
            buttonDelItem.Enabled = listBoxState.SelectedItem != null;
        }
        private void buttonState_Click(object sender, EventArgs e)
        {
            Button buttonUK = sender as Button;
            if (buttonUK != null)
                contextMenuStrip.Show(buttonState, 0, buttonState.Height + 2);

        }
        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            comboBoxState.SelectedIndex = comboBoxState.FindString(e.ClickedItem.Text);
        }


        // Private methods
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
            saveList = (SaveList)WFSerialize.Deserialize(Path.GetDirectoryName(Application.ExecutablePath) + filePath);
            if (saveList != null)
                foreach (SaveItem si in saveList.ListItems)
                    contextMenu.Items.Add(si.Name);
            else
                saveList = new SaveList();
        }

        //Private members
        private UKItem ukItem;
        private UKVocabulary ukVocablary;
        private List<object> selectedItems = new List<object>();
        private SaveList saveListUk;
        private static string filePath = "\\uk_card.dmp";

        // Events
        internal event GeneLibrary.Common.UpdateIdsText OnCheckedClose;

        // Nested classes
        class ItemList
        {
            public ItemList(object obj, string text)
            {
                this.Obj = obj;
                this.Text = text;
            }
            public object Obj { get; set;}
            public string Text { get; set;}
        }

    }
}
