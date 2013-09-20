using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeneLibrary.Common
{
    public partial class FilterControlInteger : FilterConrol
    {
        // Constructors
        public FilterControlInteger(FilterField filterField) : base()
        {
            InitializeComponent();
            base.filterField = filterField;
        }

        // Events handlers
        private void IntFilter_Load(object sender, EventArgs e)
        {
            checkBoxReportShow.Checked = filterField.IsReportShow;

            switch (filterField.FilterFieldType.GetType().Name)
            {
                case "FilterFieldRange":
                    FilterFieldRange filterFieldRange = this.filterField.FilterFieldType as FilterFieldRange;
                    if (filterFieldRange != null)
                    {
                        textBoxFrom.Text = filterFieldRange.ValueFrom;
                        textBoxTo.Text = filterFieldRange.ValueTo;
                    }
                    radioButtonRange.Checked = true;
                    break;
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
        private void radioButtonFilterType_CheckedChanged(object sender, EventArgs e)
        {
            Control[] controls = { textBoxFrom, textBoxTo, textBoxValue, listBoxList, buttonAdd };

            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (!radioButton.Checked) return;
                switch (radioButton.Name)
                {
                    case "radioButtonRange":
                        foreach (Control loopControl in 
                            (from Control innerControl in controls
                             select innerControl).Except(new Control[2] { textBoxFrom, textBoxTo })
                            )
                            loopControl.Enabled = false;

                        FilterFieldRange filterFieldRange = new FilterFieldRange();
                        filterField.FilterFieldType = filterFieldRange;
                        filterFieldRange.ValueFrom = textBoxFrom.Text;
                        filterFieldRange.ValueTo = textBoxTo.Text;

                        textBoxFrom.Enabled = true;
                        textBoxTo.Enabled = true;
                        break;
                    case "radioButtonList":
                        foreach (Control loopControl in (from Control innerControl in controls select innerControl).
                                Except(new Control[3] { textBoxValue, listBoxList, buttonAdd }))
                            loopControl.Enabled = false;

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
                        } else if (listBoxList.Items.Count == 1)
                        {
                            FieldCondition filterFieldFirst = listBoxList.Items[0] as FieldCondition;
                            filterField.FilterFieldType =
                                new FilterFieldOne(filterFieldFirst.Name, filterFieldFirst.Value);
                        }

                        textBoxValue.Enabled = true;
                        listBoxList.Enabled = true;
                        buttonAdd.Enabled = true;
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
            if (!String.IsNullOrEmpty(textBoxValue.Text.Trim()))
            {
                FieldCondition fieldCondition = new FieldCondition(textBoxValue.Text.Trim(), textBoxValue.Text.Trim());
                listBoxList.Items.Add(fieldCondition);

                if (listBoxList.Items.Count > 1)
                {
                    filterField.FilterFieldType = new FilterFieldList();
                    ((FilterFieldList)filterField.FilterFieldType).AddRange(
                        (from FieldCondition fc in listBoxList.Items select fc).ToArray<FieldCondition>());
                }
                else if (listBoxList.Items.Count == 1)
                {
                    filterField.FilterFieldType = new FilterFieldOne(textBoxValue.Text.Trim(), textBoxValue.Text.Trim());
                }

                textBoxValue.Clear();
                textBoxValue.Focus();
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
        private void textBoxFrom_TextChanged(object sender, EventArgs e)
        {
            FilterFieldRange filterFieldRange = filterField.FilterFieldType as FilterFieldRange;
            TextBox textBox = sender as TextBox;
            if (filterFieldRange != null && textBox != null)
                filterFieldRange.ValueFrom = textBox.Text;
        }
        private void textBoxTo_TextChanged(object sender, EventArgs e)
        {
            FilterFieldRange filterFieldRange = filterField.FilterFieldType as FilterFieldRange;
            TextBox textBox = sender as TextBox;
            if (filterFieldRange != null && textBox != null)
                filterFieldRange.ValueTo = textBox.Text;
        }
        
    }
}
