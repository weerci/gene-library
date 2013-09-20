namespace GeneLibrary.Common
{
    partial class FilterControlInteger
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
            this.radioButtonRange = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.radioButtonList = new System.Windows.Forms.RadioButton();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxList = new System.Windows.Forms.ListBox();
            this.contextMenuStripListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxReportShow = new System.Windows.Forms.CheckBox();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.contextMenuStripListBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonRange
            // 
            this.radioButtonRange.AutoSize = true;
            this.radioButtonRange.Location = new System.Drawing.Point(3, 49);
            this.radioButtonRange.Name = "radioButtonRange";
            this.radioButtonRange.Size = new System.Drawing.Size(76, 17);
            this.radioButtonRange.TabIndex = 2;
            this.radioButtonRange.Text = "Диапазон";
            this.radioButtonRange.UseVisualStyleBackColor = true;
            this.radioButtonRange.CheckedChanged += new System.EventHandler(this.radioButtonFilterType_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "C";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "по";
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFrom.Location = new System.Drawing.Point(38, 69);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(199, 20);
            this.textBoxFrom.TabIndex = 3;
            this.textBoxFrom.TextChanged += new System.EventHandler(this.textBoxFrom_TextChanged);
            // 
            // textBoxTo
            // 
            this.textBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTo.Location = new System.Drawing.Point(38, 96);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(199, 20);
            this.textBoxTo.TabIndex = 4;
            this.textBoxTo.TextChanged += new System.EventHandler(this.textBoxTo_TextChanged);
            // 
            // radioButtonList
            // 
            this.radioButtonList.AutoSize = true;
            this.radioButtonList.Location = new System.Drawing.Point(3, 119);
            this.radioButtonList.Name = "radioButtonList";
            this.radioButtonList.Size = new System.Drawing.Size(62, 17);
            this.radioButtonList.TabIndex = 5;
            this.radioButtonList.Text = "Список";
            this.radioButtonList.UseVisualStyleBackColor = true;
            this.radioButtonList.CheckedChanged += new System.EventHandler(this.radioButtonFilterType_CheckedChanged);
            // 
            // textBoxValue
            // 
            this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValue.Location = new System.Drawing.Point(21, 140);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(135, 20);
            this.textBoxValue.TabIndex = 6;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(162, 138);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 7;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
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
            this.listBoxList.Location = new System.Drawing.Point(21, 167);
            this.listBoxList.Name = "listBoxList";
            this.listBoxList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxList.Size = new System.Drawing.Size(216, 115);
            this.listBoxList.TabIndex = 8;
            // 
            // contextMenuStripListBox
            // 
            this.contextMenuStripListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDelete});
            this.contextMenuStripListBox.Name = "contextMenuStripListBox";
            this.contextMenuStripListBox.Size = new System.Drawing.Size(147, 26);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItemDelete.Text = "Удалить (Del)";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // checkBoxReportShow
            // 
            this.checkBoxReportShow.AutoSize = true;
            this.checkBoxReportShow.Location = new System.Drawing.Point(3, 3);
            this.checkBoxReportShow.Name = "checkBoxReportShow";
            this.checkBoxReportShow.Size = new System.Drawing.Size(160, 17);
            this.checkBoxReportShow.TabIndex = 0;
            this.checkBoxReportShow.Text = "Отображать поле в отчете";
            this.checkBoxReportShow.UseVisualStyleBackColor = true;
            this.checkBoxReportShow.CheckedChanged += new System.EventHandler(this.checkBoxReportShow_CheckedChanged);
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Location = new System.Drawing.Point(3, 26);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(94, 17);
            this.radioButtonAll.TabIndex = 1;
            this.radioButtonAll.Text = "Все значения";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.CheckedChanged += new System.EventHandler(this.radioButtonFilterType_CheckedChanged);
            // 
            // FilterControlInteger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxReportShow);
            this.Controls.Add(this.radioButtonAll);
            this.Controls.Add(this.listBoxList);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.radioButtonList);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonRange);
            this.Name = "FilterControlInteger";
            this.Size = new System.Drawing.Size(240, 285);
            this.Load += new System.EventHandler(this.IntFilter_Load);
            this.contextMenuStripListBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.RadioButton radioButtonList;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListBox listBoxList;
        private System.Windows.Forms.CheckBox checkBoxReportShow;
        private System.Windows.Forms.RadioButton radioButtonAll;
        protected System.Windows.Forms.ContextMenuStrip contextMenuStripListBox;
        protected System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;

    }
}
