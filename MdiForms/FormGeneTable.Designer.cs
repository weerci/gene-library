namespace GeneLibrary.Dialog
{
    partial class FormGeneTable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGeneTable));
            this.buttonClose = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButtonPrint = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItemVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlCards = new System.Windows.Forms.TabControl();
            this.tabPageCard = new System.Windows.Forms.TabPage();
            this.dataGridViewCards = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CARD_NUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PERSON = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CRIM_NUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATE_INS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPagePreview = new System.Windows.Forms.TabPage();
            this.dataGridViewPreview = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            this.tabControlCards.SuspendLayout();
            this.tabPageCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCards)).BeginInit();
            this.tabPagePreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(435, 291);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Закрыть";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDelete,
            this.toolStripSplitButtonPrint});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(522, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Enabled = false;
            this.toolStripButtonDelete.Image = global::GeneLibrary.Properties.Resources.Stop_2;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "toolStripButton2";
            this.toolStripButtonDelete.ToolTipText = "Удалить";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSplitButtonPrint
            // 
            this.toolStripSplitButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemVertical,
            this.toolStripMenuItemHorizontal});
            this.toolStripSplitButtonPrint.Image = global::GeneLibrary.Properties.Resources.Print;
            this.toolStripSplitButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonPrint.Name = "toolStripSplitButtonPrint";
            this.toolStripSplitButtonPrint.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonPrint.Text = "toolStripSplitButton1";
            this.toolStripSplitButtonPrint.ToolTipText = "Печать";
            this.toolStripSplitButtonPrint.ButtonClick += new System.EventHandler(this.toolStripSplitButtonPrint_ButtonClick);
            // 
            // toolStripMenuItemVertical
            // 
            this.toolStripMenuItemVertical.Name = "toolStripMenuItemVertical";
            this.toolStripMenuItemVertical.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemVertical.Text = "По вертикали";
            this.toolStripMenuItemVertical.Click += new System.EventHandler(this.toolStripMenuItemVertical_Click);
            // 
            // toolStripMenuItemHorizontal
            // 
            this.toolStripMenuItemHorizontal.Name = "toolStripMenuItemHorizontal";
            this.toolStripMenuItemHorizontal.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemHorizontal.Text = "По горизонтали";
            this.toolStripMenuItemHorizontal.Click += new System.EventHandler(this.toolStripMenuItemHorizontal_Click);
            // 
            // tabControlCards
            // 
            this.tabControlCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlCards.Controls.Add(this.tabPageCard);
            this.tabControlCards.Controls.Add(this.tabPagePreview);
            this.tabControlCards.Location = new System.Drawing.Point(12, 33);
            this.tabControlCards.Name = "tabControlCards";
            this.tabControlCards.SelectedIndex = 0;
            this.tabControlCards.Size = new System.Drawing.Size(498, 252);
            this.tabControlCards.TabIndex = 5;
            this.tabControlCards.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlCards_Selected);
            // 
            // tabPageCard
            // 
            this.tabPageCard.Controls.Add(this.dataGridViewCards);
            this.tabPageCard.Location = new System.Drawing.Point(4, 22);
            this.tabPageCard.Name = "tabPageCard";
            this.tabPageCard.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCard.Size = new System.Drawing.Size(490, 226);
            this.tabPageCard.TabIndex = 0;
            this.tabPageCard.Text = "Карточки";
            this.tabPageCard.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCards
            // 
            this.dataGridViewCards.AllowDrop = true;
            this.dataGridViewCards.AllowUserToAddRows = false;
            this.dataGridViewCards.AllowUserToDeleteRows = false;
            this.dataGridViewCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.CARD_NUM,
            this.PERSON,
            this.CRIM_NUM,
            this.DATE_INS});
            this.dataGridViewCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCards.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewCards.Name = "dataGridViewCards";
            this.dataGridViewCards.ReadOnly = true;
            this.dataGridViewCards.RowHeadersWidth = 16;
            this.dataGridViewCards.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCards.Size = new System.Drawing.Size(484, 220);
            this.dataGridViewCards.TabIndex = 5;
            this.dataGridViewCards.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewCards_DragOver);
            this.dataGridViewCards.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewCards_DragDrop);
            // 
            // ID
            // 
            this.ID.HeaderText = "КИН";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 50;
            // 
            // CARD_NUM
            // 
            this.CARD_NUM.HeaderText = "№ Карты";
            this.CARD_NUM.Name = "CARD_NUM";
            this.CARD_NUM.ReadOnly = true;
            this.CARD_NUM.Width = 80;
            // 
            // PERSON
            // 
            this.PERSON.HeaderText = "Лицо";
            this.PERSON.Name = "PERSON";
            this.PERSON.ReadOnly = true;
            // 
            // CRIM_NUM
            // 
            this.CRIM_NUM.HeaderText = "№ Уг. дела";
            this.CRIM_NUM.Name = "CRIM_NUM";
            this.CRIM_NUM.ReadOnly = true;
            // 
            // DATE_INS
            // 
            this.DATE_INS.HeaderText = "Дата";
            this.DATE_INS.Name = "DATE_INS";
            this.DATE_INS.ReadOnly = true;
            // 
            // tabPagePreview
            // 
            this.tabPagePreview.Controls.Add(this.dataGridViewPreview);
            this.tabPagePreview.Location = new System.Drawing.Point(4, 22);
            this.tabPagePreview.Name = "tabPagePreview";
            this.tabPagePreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePreview.Size = new System.Drawing.Size(490, 226);
            this.tabPagePreview.TabIndex = 1;
            this.tabPagePreview.Text = "Просмотр";
            this.tabPagePreview.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPreview
            // 
            this.dataGridViewPreview.AllowUserToAddRows = false;
            this.dataGridViewPreview.AllowUserToDeleteRows = false;
            this.dataGridViewPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPreview.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPreview.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPreview.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewPreview.Name = "dataGridViewPreview";
            this.dataGridViewPreview.ReadOnly = true;
            this.dataGridViewPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPreview.Size = new System.Drawing.Size(484, 220);
            this.dataGridViewPreview.TabIndex = 0;
            // 
            // FormGeneTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 326);
            this.Controls.Add(this.tabControlCards);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.buttonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGeneTable";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Таблица генотипов";
            this.TopMost = true;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControlCards.ResumeLayout(false);
            this.tabPageCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCards)).EndInit();
            this.tabPagePreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.TabControl tabControlCards;
        private System.Windows.Forms.TabPage tabPageCard;
        private System.Windows.Forms.DataGridView dataGridViewCards;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CARD_NUM;
        private System.Windows.Forms.DataGridViewTextBoxColumn PERSON;
        private System.Windows.Forms.DataGridViewTextBoxColumn CRIM_NUM;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATE_INS;
        private System.Windows.Forms.TabPage tabPagePreview;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonPrint;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVertical;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHorizontal;
        private System.Windows.Forms.DataGridView dataGridViewPreview;
    }
}