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
    public partial class FilterControlDate : FilterConrol
    {
        // Constructors
        public FilterControlDate(FilterField filterField) : base()
        {
            InitializeComponent();
            base.filterField = filterField;
        }

        // Events handlers
        private void DateFilter_Load(object sender, EventArgs e)
        {
            checkBoxReportShow.Checked = filterField.IsReportShow;

            switch (filterField.FilterFieldType.GetType().Name)
            {
                case "FilterFieldRange":
                    FilterFieldRange filterFieldRange = this.filterField.FilterFieldType as FilterFieldRange;
                    if (filterFieldRange != null)
                    {
                        dateTimePickerFrom.Text = filterFieldRange.ValueFrom;
                        dateTimePickerTo.Text = filterFieldRange.ValueTo;
                    }
                    radioButtonRange.Checked = true;
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
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            Control[] controls = { dateTimePickerFrom, dateTimePickerTo, dateTimePickerValue, buttonAdd, listBoxList };

            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (!radioButton.Checked) return;
                switch (radioButton.Name)
                {
                    case "radioButtonRange":
                        foreach (Control loopControl in
                            (from Control innerControl in controls select innerControl).
                            Except(new Control[2] { dateTimePickerFrom, dateTimePickerTo })
                            )
                            loopControl.Enabled = false;

                        FilterFieldRange filterFieldRange = new FilterFieldRange();
                        filterField.FilterFieldType = filterFieldRange;
                        filterFieldRange.ValueFrom = dateTimePickerFrom.Text;
                        filterFieldRange.ValueTo = dateTimePickerTo.Text;

                        dateTimePickerFrom.Enabled = true;
                        dateTimePickerTo.Enabled = true;
                        break;
                    case "radioButtonList":
                        foreach (Control loopControl in (from Control innerControl in controls select innerControl).
                            Except(new Control[3] { dateTimePickerValue, listBoxList, buttonAdd }))
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
                        }
                        else if (listBoxList.Items.Count == 1)
                        {
                            FieldCondition filterFieldFirst = listBoxList.Items[0] as FieldCondition;
                            filterField.FilterFieldType =
                                new FilterFieldOne(filterFieldFirst.Name, filterFieldFirst.Value);
                        }


                        dateTimePickerValue.Enabled = true;
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
            if (!String.IsNullOrEmpty(dateTimePickerValue.Text.Trim()))
            {

                FieldCondition fieldCondition = new FieldCondition(dateTimePickerValue.Text.Trim(), dateTimePickerValue.Text.Trim());
                listBoxList.Items.Add(fieldCondition);

                if (listBoxList.Items.Count > 1)
                {
                    filterField.FilterFieldType = new FilterFieldList();
                    ((FilterFieldList)filterField.FilterFieldType).AddRange(
                        (from FieldCondition fc in listBoxList.Items select fc).ToArray<FieldCondition>());
                }
                else if (listBoxList.Items.Count == 1)
                {
                    filterField.FilterFieldType = new FilterFieldOne(dateTimePickerValue.Text.Trim(), dateTimePickerValue.Text.Trim());
                }
            }
        }
        private void toolStripMenuDelete_Click(object sender, EventArgs e)
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
        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            FilterFieldRange filterFieldRange = filterField.FilterFieldType as FilterFieldRange;
            DateTimePicker dateTimePicker = sender as DateTimePicker;
            if (filterFieldRange != null && dateTimePicker != null)
                filterFieldRange.ValueFrom = dateTimePicker.Text;
        }
        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            FilterFieldRange filterFieldRange = filterField.FilterFieldType as FilterFieldRange;
            DateTimePicker dateTimePicker = sender as DateTimePicker;
            if (filterFieldRange != null && dateTimePicker != null)
                filterFieldRange.ValueTo = dateTimePicker.Text;

        }
    }
    
}
