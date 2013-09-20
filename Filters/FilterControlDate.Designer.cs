namespace GeneLibrary.Common
{
    partial class FilterControlDate
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.radioButtonList = new System.Windows.Forms.RadioButton();
            this.radioButtonRange = new System.Windows.Forms.RadioButton();
            this.listBoxList = new System.Windows.Forms.ListBox();
            this.contextMenuStripListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.удалитьDelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.dateTimePickerValue = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.checkBoxReportShow = new System.Windows.Forms.CheckBox();
            this.contextMenuStripListBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonList
            // 
            this.radioButtonList.AutoSize = true;
            this.radioButtonList.Location = new System.Drawing.Point(3, 120);
            this.radioButtonList.Name = "radioButtonList";
            this.radioButtonList.Size = new System.Drawing.Size(62, 17);
            this.radioButtonList.TabIndex = 17;
            this.radioButtonList.Text = "Список";
            this.radioButtonList.UseVisualStyleBackColor = true;
            this.radioButtonList.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonRange
            // 
            this.radioButtonRange.AutoSize = true;
            this.radioButtonRange.Location = new System.Drawing.Point(3, 49);
            this.radioButtonRange.Name = "radioButtonRange";
            this.radioButtonRange.Size = new System.Drawing.Size(76, 17);
            this.radioButtonRange.TabIndex = 16;
            this.radioButtonRange.Text = "Диапазон";
            this.radioButtonRange.UseVisualStyleBackColor = true;
            this.radioButtonRange.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // listBoxList
            // 
            this.listBoxList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxList.ContextMenuStrip = this.contextMenuStripListBox;
            this.listBoxList.DisplayMember = "name";
            this.listBoxList.FormattingEnabled = true;
            this.listBoxList.IntegralHeight = false;
            this.listBoxList.Location = new System.Drawing.Point(14, 173);
            this.listBoxList.Name = "listBoxList";
            this.listBoxList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxList.Size = new System.Drawing.Size(228, 117);
            this.listBoxList.TabIndex = 15;
            // 
            // contextMenuStripListBox
            // 
            this.contextMenuStripListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.удалитьDelToolStripMenuItem});
            this.contextMenuStripListBox.Name = "contextMenuStripListBox";
            this.contextMenuStripListBox.Size = new System.Drawing.Size(147, 26);
            // 
            // удалитьDelToolStripMenuItem
            // 
            this.удалитьDelToolStripMenuItem.Name = "удалитьDelToolStripMenuItem";
            this.удалитьDelToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.удалитьDelToolStripMenuItem.Text = "Удалить (Del)";
            this.удалитьDelToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(168, 141);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 14;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // dateTimePickerValue
            // 
            this.dateTimePickerValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerValue.Location = new System.Drawing.Point(14, 143);
            this.dateTimePickerValue.Name = "dateTimePickerValue";
            this.dateTimePickerValue.Size = new System.Drawing.Size(148, 20);
            this.dateTimePickerValue.TabIndex = 13;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTo.Location = new System.Drawing.Point(31, 96);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerTo.TabIndex = 12;
            this.dateTimePickerTo.ValueChanged += new System.EventHandler(this.dateTimePickerTo_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "по";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerFrom.Location = new System.Drawing.Point(31, 69);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerFrom.TabIndex = 10;
            this.dateTimePickerFrom.ValueChanged += new System.EventHandler(this.dateTimePickerFrom_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "C";
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Location = new System.Drawing.Point(3, 26);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(94, 17);
            this.radioButtonAll.TabIndex = 18;
            this.radioButtonAll.Text = "Все значения";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // checkBoxReportShow
            // 
            this.checkBoxReportShow.AutoSize = true;
            this.checkBoxReportShow.Location = new System.Drawing.Point(3, 3);
            this.checkBoxReportShow.Name = "checkBoxReportShow";
            this.checkBoxReportShow.Size = new System.Drawing.Size(160, 17);
            this.checkBoxReportShow.TabIndex = 19;
            this.checkBoxReportShow.Text = "Отображать поле в отчете";
            this.checkBoxReportShow.UseVisualStyleBackColor = true;
            this.checkBoxReportShow.CheckedChanged += new System.EventHandler(this.checkBoxReportShow_CheckedChanged);
            // 
            // FilterControlDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxReportShow);
            this.Controls.Add(this.radioButtonAll);
            this.Controls.Add(this.radioButtonList);
            this.Controls.Add(this.radioButtonRange);
            this.Controls.Add(this.listBoxList);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.dateTimePickerValue);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.label1);
            this.Name = "FilterControlDate";
            this.Size = new System.Drawing.Size(248, 293);
            this.Load += new System.EventHandler(this.DateFilter_Load);
            this.contextMenuStripListBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonList;
        private System.Windows.Forms.RadioButton radioButtonRange;
        private System.Windows.Forms.ListBox listBoxList;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.DateTimePicker dateTimePickerValue;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.CheckBox checkBoxReportShow;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListBox;
        private System.Windows.Forms.ToolStripMenuItem удалитьDelToolStripMenuItem;

    }
}
