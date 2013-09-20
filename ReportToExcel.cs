using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Drawing;
using GeneLibrary.Common;

namespace WFExcel
{
    public class ReportToExcel
    {
        // Constructors
        private ReportToExcel(){}
        public ReportToExcel(string templateExcel): this()
        {
            this.templateExcel = templateExcel;
        }

        // Methods
        public void Load(Collection<ReportField> reportFields, Collection<ReportCollection> reportCollections)
        {
            try
            {
                applicationExcel = Tools.ExcelApplication();
                workbookExcel = applicationExcel.Workbooks.Add(templateExcel);

                // Load fields
                foreach (Excel.Worksheet workSheet in applicationExcel.Sheets)
                {
                    foreach (Excel.Range item in workSheet.UsedRange.Cells)
                    {
                        if (item.Text.ToString().StartsWith("#"))
                            SetValue(item, reportFields);
                        else if (item.Text.ToString().StartsWith("~^"))
                            SetCollectionVertical(workSheet, item, reportCollections);
                        else if (item.Text.ToString().StartsWith("~>"))
                            SetCollectionHorisontal(workSheet, item, reportCollections);
                    }
                }

                applicationExcel.Visible = true;
                applicationExcel.UserControl = true;
            }
            catch (Exception e)
            {
                applicationExcel.Quit();
                throw new WFExceptions.WFException(WFExceptions.ErrType.Message, e.Message, e);
            }
        }
        public void LoadWithHeaders(Collection<ReportField> reportFields, Collection<Collection<string>> bodyReport,
            Collection<string> headerCaptions, Collection<string> rowsCaption)
        {
            try
            {
                applicationExcel = Tools.ExcelApplication();
                workbookExcel = applicationExcel.Workbooks.Add(templateExcel);

                // Load fields
                foreach (Excel.Worksheet workSheet in applicationExcel.Sheets)
                {
                    foreach (Excel.Range item in workSheet.UsedRange.Cells)
                    {
                        if (item.Text.ToString().StartsWith("#"))
                            SetValue(item, reportFields);
                        else if (item.Text.ToString().StartsWith("~b"))
                            SetCollectionBodyReport(workSheet, item, bodyReport);
                        else if (item.Text.ToString().StartsWith("~c"))
                            SetColumns(workSheet, item, headerCaptions);
                        else if (item.Text.ToString().StartsWith("~r"))
                            SetRows(workSheet, item, rowsCaption);
                    }
                }

                applicationExcel.Visible = true;
                applicationExcel.UserControl = true;
            }
            catch (Exception e)
            {
                applicationExcel.DisplayAlerts = false;
                applicationExcel.Quit();
                throw new WFExceptions.WFException(WFExceptions.ErrType.Message, e.Message, e);
            }
        
        }
        public void LoadWithHeadersInsert(Collection<ReportField> reportFields, Collection<Collection<string>> bodyReport,
            Collection<string> headerCaptions, Collection<string> rowsCaption)
        {
            try
            {
                applicationExcel = Tools.ExcelApplication();
                workbookExcel = applicationExcel.Workbooks.Add(templateExcel);
                int row = 0;
                int column = 0;

                // Load fields
                foreach (Excel.Worksheet workSheet in applicationExcel.Sheets)
                {
                    foreach (Excel.Range item in workSheet.UsedRange.Cells)
                    {
                        if (item.Text.ToString().StartsWith("#"))
                            SetValue(item, reportFields);
                        else if (item.Text.ToString().StartsWith("~b"))
                        {
                            row = item.Row;
                            column = item.Column;
                        }
                        else if (item.Text.ToString().StartsWith("~c"))
                            SetColumns(workSheet, item, headerCaptions);
                        else if (item.Text.ToString().StartsWith("~r"))
                            SetRows(workSheet, item, rowsCaption);
                    }
                }
                Excel.Worksheet ws = (Excel.Worksheet)workbookExcel.ActiveSheet;
                Excel.Range rg = ws.get_Range(ws.Cells[row, column],
                    ws.Cells[row + bodyReport.Count()-2, column + bodyReport[0].Count()]);
                rg.Insert(Excel.XlInsertShiftDirection.xlShiftDown, null);
                SetCollectionBodyReport(
                    (Excel.Worksheet)workbookExcel.ActiveSheet, 
                    ws.get_Range(ws.Cells[row, column], ws.Cells[row, column]), 
                    bodyReport);

                applicationExcel.Visible = true;
                applicationExcel.UserControl = true;
            }
            catch (Exception e)
            {
                applicationExcel.DisplayAlerts = false;
                applicationExcel.Quit();
                throw new WFExceptions.WFException(WFExceptions.ErrType.Message, e.Message, e);
            }
        }

        // Private methods
        private void SetValue(Excel.Range item, Collection<ReportField> reportFields)
        {
            var result = from ReportField reportField in reportFields where reportField.Key == item.Value2.ToString() select reportField;
            if (result.Count<ReportField>() != 0)
            {
                item.NumberFormat = "@";
                item.Value2 = result.First<ReportField>().Value;
            }
            else
                item.Value2 = "";
        }
        private void SetCollectionVertical(Excel.Worksheet workSheet, Excel.Range item, Collection<ReportCollection> reportCollections)
        {
            bool withNumber = item.Value2.ToString().StartsWith("~^№");
            Excel.Range range;
            for (int i = 0; i < reportCollections.Count; i++)
            {
                range = workSheet.get_Range(workSheet.Cells[item.Row + i, item.Column], workSheet.Cells[item.Row + i, item.Column]);
                if (withNumber)
                    range.Value2 = String.Format("{0}  {1}", i+1, reportCollections[i].Key);
                else
                    range.Value2 = reportCollections[i].Key;

                for (int j = 0; j < reportCollections[i].Value.Count(); j++)
                {
                    range = workSheet.get_Range(workSheet.Cells[item.Row + i, item.Column + j + 1], workSheet.Cells[item.Row + i, item.Column + j + 1]);
                    range.Value2 = reportCollections[i].Value[j];
                }
            }
        }
        private void SetCollectionHorisontal(Excel.Worksheet workSheet, Excel.Range item, Collection<ReportCollection> reportCollections)
        {
            bool withNumber = item.Value2.ToString().StartsWith("~>№");
            Excel.Range range;
            for (int i = 0; i < reportCollections.Count; i++)
            {
                range = workSheet.get_Range(workSheet.Cells[item.Row, item.Column + i], workSheet.Cells[item.Row, item.Column + i]);
                range.Value2 = reportCollections[i].Key;
                for (int j = 0; j < reportCollections[i].Value.Count(); j++)
                {
                    range = workSheet.get_Range(workSheet.Cells[item.Row + j + 1, item.Column + i], workSheet.Cells[item.Row + j + 1, item.Column + i]);
                    if (withNumber)
                        range.Value2 = String.Format("{0}  {1}", j+1, reportCollections[i].Value[j]);
                    else
                        range.Value2 = reportCollections[i].Value[j];
                }
            }
        }
        private void SetCollectionBodyReport(Excel.Worksheet workSheet, Excel.Range item, Collection<Collection<string>> bodyReport)
        {
            Excel.Range range;
            for (int i = 0; i < bodyReport.Count; i++)
                for (int j = 0; j < bodyReport[i].Count; j++)
                {
                    range = workSheet.get_Range(workSheet.Cells[item.Row + i, item.Column + j], workSheet.Cells[item.Row + i, item.Column + j]);
                    range.NumberFormat = "@";                    
                    range.Value2 = bodyReport[i][j];			 
                }
        }
        private void SetCollectionBodyReportInsert(Excel.Worksheet workSheet, Excel.Range item, Collection<Collection<string>> bodyReport)
        {
            Excel.Range range;
            for (int i = 0; i < bodyReport.Count; i++)
                for (int j = 0; j < bodyReport[i].Count; j++)
                {
                    range = workSheet.get_Range(workSheet.Cells[item.Row + i, item.Column + j], workSheet.Cells[item.Row + i, item.Column + j]);
                    range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, null);
                    range.NumberFormat = "@";
                    range.Value2 = bodyReport[i][j];
                }
        }
        private static void SetColumns(Excel.Worksheet workSheet, Excel.Range item, Collection<string> columnCaptions)
        {
            if (columnCaptions == null)
                return;

            bool withNumber = item.Value2.ToString().StartsWith("~c№");
            Excel.Range range;
            for (int i = 0; i < columnCaptions.Count; i++)
            {
                range = workSheet.get_Range(workSheet.Cells[item.Row, item.Column + i], workSheet.Cells[item.Row, item.Column + i]);
                if (withNumber)
                    range.Value2 = String.Format("{0}  {1}", i + 1, columnCaptions[i]);
                else
                    range.Value2 = columnCaptions[i];

            }
        }
        private static void SetRows(Excel.Worksheet workSheet, Excel.Range item, Collection<string> rowCaptions)
        {
            if (rowCaptions == null)
                return;

            bool withNumber = item.Value2.ToString().StartsWith("~r№");
            Excel.Range range;
            for (int i = 0; i < rowCaptions.Count; i++)
            {
                range = workSheet.get_Range(workSheet.Cells[item.Row + i, item.Column], workSheet.Cells[item.Row + i, item.Column]);
                if (withNumber)
                    range.Value2 = String.Format("{0}  {1}", i + 1, rowCaptions[i]);
                else
                    range.Value2 = rowCaptions[i];
            }
        }

        // Fields
        private string templateExcel;
        private Excel.Application applicationExcel;
        private Excel.Workbook workbookExcel;
    }
    public class ReportField
    {
        // Constructors
        public ReportField() { }
        public ReportField(string key, string value)
            : this()
        {
            this.Key = key;
            this.Value = value;
        }

        // Properties
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ReportCollection
    {
        // Constructors
        public ReportCollection() { }
        public ReportCollection(string key, Collection<string> value)
            : this()
        {
            this.Key = key;
            this.Value = value;
        }

        // Properties
        public string Key { get; set; }
        public Collection<string> Value
        {
            get
            {
                return new Collection<string>(this.listValue);
            }
            set
            {
                this.listValue = value.ToList<string>();
            }
        }

        // Private members
        List<string> listValue = new List<string>();
    }
}

