namespace GeneLibrary.Common
{
    partial class FilterControlString
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
            this.listBoxList = new System.Windows.Forms.ListBox();
            this.contextMenuStripListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.удалитьDelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxPartWord = new System.Windows.Forms.CheckBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.radioButtonList = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.checkBoxReportShow = new System.Windows.Forms.CheckBox();
            this.contextMenuStripListBox.SuspendLayout();
            this.SuspendLayout();
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
            this.listBoxList.Location = new System.Drawing.Point(25, 112);
            this.listBoxList.Name = "listBoxList";
            this.listBoxList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxList.Size = new System.Drawing.Size(224, 177);
            this.listBoxList.TabIndex = 0;
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
            this.удалитьDelToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuIDelete);
            // 
            // checkBoxPartWord
            // 
            this.checkBoxPartWord.AutoSize = true;
            this.checkBoxPartWord.Location = new System.Drawing.Point(25, 63);
            this.checkBoxPartWord.Name = "checkBoxPartWord";
            this.checkBoxPartWord.Size = new System.Drawing.Size(137, 17);
            this.checkBoxPartWord.TabIndex = 1;
            this.checkBoxPartWord.Text = "Поиск по части слова";
            this.checkBoxPartWord.UseVisualStyleBackColor = true;
            this.checkBoxPartWord.CheckedChanged += new System.EventHandler(this.checkBoxPartWord_CheckedChanged);
            // 
            // textBoxValue
            // 
            this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValue.Location = new System.Drawing.Point(25, 86);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(153, 20);
            this.textBoxValue.TabIndex = 2;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(184, 84);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(65, 23);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // radioButtonList
            // 
            this.radioButtonList.AutoSize = true;
            this.radioButtonList.Location = new System.Drawing.Point(3, 43);
            this.radioButtonList.Name = "radioButtonList";
            this.radioButtonList.Size = new System.Drawing.Size(62, 17);
            this.radioButtonList.TabIndex = 6;
            this.radioButtonList.Text = "Список";
            this.radioButtonList.UseVisualStyleBackColor = true;
            this.radioButtonList.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Location = new System.Drawing.Point(3, 24);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(94, 17);
            this.radioButtonAll.TabIndex = 7;
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
            this.checkBoxReportShow.TabIndex = 8;
            this.checkBoxReportShow.Text = "Отображать поле в отчете";
            this.checkBoxReportShow.UseVisualStyleBackColor = true;
            this.checkBoxReportShow.CheckedChanged += new System.EventHandler(this.checkBoxReportShow_CheckedChanged);
            // 
            // FilterControlString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxReportShow);
            this.Controls.Add(this.radioButtonAll);
            this.Controls.Add(this.radioButtonList);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.checkBoxPartWord);
            this.Controls.Add(this.listBoxList);
            this.Name = "FilterControlString";
            this.Size = new System.Drawing.Size(252, 292);
            this.Load += new System.EventHandler(this.StringFilter_Load);
            this.contextMenuStripListBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxList;
        private System.Windows.Forms.CheckBox checkBoxPartWord;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.RadioButton radioButtonList;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.CheckBox checkBoxReportShow;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListBox;
        private System.Windows.Forms.ToolStripMenuItem удалитьDelToolStripMenuItem;
    }
}
