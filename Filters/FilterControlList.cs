using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Items;
using System.Globalization;

namespace GeneLibrary.Common
{
    public partial class FilterControlList : FilterConrol
    {
        // Constructors
        public FilterControlList(FilterField filterField, Vocabulary vocabulary, string[] fields) : base()
        {
            InitializeComponent();
            this.vocabulary = vocabulary;
            this.fields = fields;
            base.filterField = filterField;
        }
        public FilterControlList(FilterField filterField, Vocabulary vocabulary, string[] fields, string filter) : 
            this(filterField, vocabulary, fields)
        {
            this.filter = filter;
        }

        // Events handlers
        private void ListFilter_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.filter))
                vocabulary.Open();
            else
                vocabulary.Open(null, this.filter);

            comboBoxValue.Items.AddRange(
                (from DataRow dataRow in vocabulary.DT.Rows
                 select new ComboBoxItem(Convert.ToInt32(dataRow[fields[0].ToUpper()], CultureInfo.InvariantCulture),
                                         dataRow[fields[1].ToUpper()].ToString()
                                         )).ToArray<ComboBoxItem>());
            if (comboBoxValue.Items.Count > 0)
                comboBoxValue.SelectedIndex = 0;

            checkBoxReportShow.Checked = filterField.IsReportShow;

            switch (filterField.FilterFieldType.GetType().Name)
            {
                case "FilterFieldOne":
                    FilterFieldOne filterFieldOne = this.filterField.FilterFieldType as FilterFieldOne;
                    if (filterFieldOne != null)
                    {
                        listBoxList.Items.Clear();
                        listBoxList.Items.Add(filterFieldOne.Value.First());
                    }
                    radioButtonList.Checked = true;
                    break;
                case "FilterFieldList":
                    FilterFieldList filterFieldList = this.filterField.FilterFieldType as FilterFieldList;
                    if (filterFieldList != null)
                    {
                        listBoxList.Items.Clear();
                        if (!String.IsNullOrEmpty(filterFieldList.Condition.Value))
                            listBoxList.Items.AddRange(filterFieldList.Value.ToArray<FieldCondition>());
                    }
                    radioButtonList.Checked = true;
                    break;
                default:
                    radioButtonAll.Checked = true;
                    break;
            }
        }
        private void checkBoxReportShow_CheckedChanged(object sender, EventArgs e)
        {
            filterField.IsReportShow = checkBoxReportShow.Checked;
        }
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            Control[] controls = { comboBoxValue, listBoxList, buttonAdd };

            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (!radioButton.Checked) return;
                switch (radioButton.Name)
                {
                    case "radioButtonList":
                        foreach (Control loopControl in (from Control innerControl in controls select innerControl))
                            loopControl.Enabled = true;

                        if (listBoxList.Items.Count > 1)
                        {
                            FilterFieldList filterFieldList = new FilterFieldList();
                            filterField.FilterFieldType = filterFieldList;
                            foreach (var item in listBoxList.Items)
                            {
                                FieldCondition fieldCondition = item as FieldCondition;
                                if (fieldCondition != null)
                                    filterFieldList.AddValue(fieldCondition);
                            }
                        }
                        else if (listBoxList.Items.Count == 1)
                        {
                            FieldCondition filterFieldFirst = listBoxList.Items[0] as FieldCondition;
                            filterField.FilterFieldType =
                                new FilterFieldOne(filterFieldFirst.Name, filterFieldFirst.Value);
                        }
                        break;
                    default:
                        foreach (Control loopControl in (from Control innerControl in controls select innerControl))
                            loopControl.Enabled = false;

                        filterField.FilterFieldType = new FilterFieldAll();
                        break;
                }
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(comboBoxValue.Text.Trim()))
            {
                ComboBoxItem comboBoxItem = comboBoxValue.SelectedItem as ComboBoxItem;
                if (comboBoxItem != null)
                {
                    FieldCondition fieldCondition = new FieldCondition(comboBoxItem.Name, comboBoxItem.Id.ToString());
                    listBoxList.Items.Add(fieldCondition);

                    if (listBoxList.Items.Count > 1)
                    {
                        filterField.FilterFieldType = new FilterFieldList();
                        ((FilterFieldList)filterField.FilterFieldType).AddRange(
                            (from FieldCondition fc in listBoxList.Items select fc).ToArray<FieldCondition>());
                    }
                    else if (listBoxList.Items.Count == 1)
                    {
                        filterField.FilterFieldType = new FilterFieldOne(comboBoxItem.Name, comboBoxItem.Id.ToString());
                    }
                }
            }
        }
        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            Object[] selectedItems = new Object[listBoxList.SelectedItems.Count];
            listBoxList.SelectedItems.CopyTo(selectedItems, 0);
            foreach (Object obj in selectedItems)
                listBoxList.Items.Remove(obj);

            if (listBoxList.Items.Count > 1)
            {
                filterField.FilterFieldType = new FilterFieldList();
                ((FilterFieldList)filterField.FilterFieldType).AddRange(
                    (from FieldCondition fc in listBoxList.Items select fc).ToArray<FieldCondition>());
            }
            else if (listBoxList.Items.Count == 1)
            {
                filterField.FilterFieldType = new FilterFieldOne();
                ((FilterFieldOne)filterField.FilterFieldType).AddValue(((FieldCondition)listBoxList.Items[0]));
            }
            else
            {
                filterField.FilterFieldType = new FilterFieldAll();
            }
        }


        // Fields
        private Vocabulary vocabulary;
        private string filter;
        private string[] fields;
    }
}
