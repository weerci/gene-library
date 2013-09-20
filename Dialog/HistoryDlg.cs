using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeneLibrary.Dialog
{
    public partial class HistoryDlg : Form
    {
        public HistoryDlg(DataGridViewRow dataRow)
        {
            InitializeComponent();
            this.dataRow = dataRow;
        }
        private void HistoryDlg_Load(object sender, EventArgs e)
        {
            rtbHistory.Clear();
            AddFormatedText("ID:\t\t", new Font("Tahoma", 10, FontStyle.Bold), Color.Black, "");
            AddFormatedText(dataRow.Cells["ID"].Value.ToString(), new Font("Tahoma", 10, FontStyle.Regular), Color.Black, "");
            AddFormatedText("Дата:\t\t", new Font("Tahoma", 10, FontStyle.Bold), Color.Black, "\n");
            AddFormatedText(dataRow.Cells["DATE_INSERT"].Value.ToString(), new Font("Tahoma", 10, FontStyle.Regular), Color.Black, "");
            AddFormatedText("Дейстие:\t", new Font("Tahoma", 10, FontStyle.Bold), Color.Black, "\n");
            AddFormatedText(dataRow.Cells["ACTION"].Value.ToString(), new Font("Tahoma", 10, FontStyle.Regular), Color.Black, "");
            AddFormatedText("Эксперт:\t", new Font("Tahoma", 10, FontStyle.Bold), Color.Black, "\n");
            AddFormatedText(dataRow.Cells["EXPERT"].Value.ToString(), new Font("Tahoma", 10, FontStyle.Regular), Color.Black, "");
            AddFormatedText("Текст:\t\t", new Font("Tahoma", 10, FontStyle.Bold), Color.Black, "\n");
            AddFormatedText(dataRow.Cells["NOTE"].Value.ToString(), new Font("Tahoma", 10, FontStyle.Regular), Color.Black, "\n");
            
        }
        private void AddFormatedText(string text, Font font, Color color, string ident)
        {
            int StartPosition = rtbHistory.Text.Length;
            int lengthSelection;
            if (!String.IsNullOrEmpty(text.Trim()))
            {
                rtbHistory.AppendText(ident + text);
                lengthSelection = text.Length + ident.Length;
            }
            else
            {
                rtbHistory.AppendText(ident + Properties.Resources.NoDate);
                lengthSelection = Properties.Resources.NoDate.Length + ident.Length;
            }
            rtbHistory.Select(StartPosition, lengthSelection);
            rtbHistory.SelectionFont = font;
            rtbHistory.SelectionColor = color;
            rtbHistory.Select(0, 0);
        }

        private DataGridViewRow dataRow;

    }
}
