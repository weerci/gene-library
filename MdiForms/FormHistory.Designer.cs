namespace GeneLibrary.MdiForms
{
    partial class FormHistory
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHistory));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonFind = new System.Windows.Forms.Button();
            this.comboBoxCardFields = new System.Windows.Forms.ComboBox();
            this.textBoxFindString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonExpert = new System.Windows.Forms.Button();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.checkBoxIk2 = new System.Windows.Forms.CheckBox();
            this.checkBoxIkl = new System.Windows.Forms.CheckBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxCardsId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxExpert = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlResult = new System.Windows.Forms.TabControl();
            this.tabPageFilter = new System.Windows.Forms.TabPage();
            this.treeViewCard = new System.Windows.Forms.TreeView();
            this.tabPageFind = new System.Windows.Forms.TabPage();
            this.treeViewFind = new System.Windows.Forms.TreeView();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.contextMenuExpert = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlResult.SuspendLayout();
            this.tabPageFilter.SuspendLayout();
            this.tabPageFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 104);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonFind);
            this.groupBox2.Controls.Add(this.comboBoxCardFields);
            this.groupBox2.Controls.Add(this.textBoxFindString);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(384, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(394, 95);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Поиск карточки";
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.Location = new System.Drawing.Point(310, 67);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 2;
            this.buttonFind.Text = "Найти";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // comboBoxCardFields
            // 
            this.comboBoxCardFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCardFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCardFields.FormattingEnabled = true;
            this.comboBoxCardFields.Location = new System.Drawing.Point(68, 40);
            this.comboBoxCardFields.Name = "comboBoxCardFields";
            this.comboBoxCardFields.Size = new System.Drawing.Size(317, 21);
            this.comboBoxCardFields.TabIndex = 1;
            // 
            // textBoxFindString
            // 
            this.textBoxFindString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFindString.Location = new System.Drawing.Point(68, 15);
            this.textBoxFindString.Name = "textBoxFindString";
            this.textBoxFindString.Size = new System.Drawing.Size(317, 20);
            this.textBoxFindString.TabIndex = 0;
            this.textBoxFindString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFindString_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Поле";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Условие";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonExpert);
            this.groupBox1.Controls.Add(this.buttonFilter);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dtpTo);
            this.groupBox1.Controls.Add(this.checkBoxIk2);
            this.groupBox1.Controls.Add(this.checkBoxIkl);
            this.groupBox1.Controls.Add(this.dtpFrom);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxCardsId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxExpert);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(1, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(377, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Фильтр (история карточек)";
            // 
            // buttonExpert
            // 
            this.buttonExpert.Location = new System.Drawing.Point(286, 39);
            this.buttonExpert.Name = "buttonExpert";
            this.buttonExpert.Size = new System.Drawing.Size(28, 23);
            this.buttonExpert.TabIndex = 3;
            this.buttonExpert.Text = "...";
            this.buttonExpert.UseVisualStyleBackColor = true;
            this.buttonExpert.Click += new System.EventHandler(this.buttonExpert_Click);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFilter.Location = new System.Drawing.Point(296, 67);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 7;
            this.buttonFilter.Text = "Фильтр";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(129, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "по";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(154, 68);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(91, 20);
            this.dtpTo.TabIndex = 6;
            // 
            // checkBoxIk2
            // 
            this.checkBoxIk2.AutoSize = true;
            this.checkBoxIk2.Checked = true;
            this.checkBoxIk2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIk2.Location = new System.Drawing.Point(319, 42);
            this.checkBoxIk2.Name = "checkBoxIk2";
            this.checkBoxIk2.Size = new System.Drawing.Size(50, 17);
            this.checkBoxIk2.TabIndex = 4;
            this.checkBoxIk2.Text = "ИК-2";
            this.checkBoxIk2.UseVisualStyleBackColor = true;
            // 
            // checkBoxIkl
            // 
            this.checkBoxIkl.AutoSize = true;
            this.checkBoxIkl.Checked = true;
            this.checkBoxIkl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIkl.Location = new System.Drawing.Point(320, 17);
            this.checkBoxIkl.Name = "checkBoxIkl";
            this.checkBoxIkl.Size = new System.Drawing.Size(49, 17);
            this.checkBoxIkl.TabIndex = 1;
            this.checkBoxIkl.Text = "ИКЛ";
            this.checkBoxIkl.UseVisualStyleBackColor = true;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(32, 68);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(91, 20);
            this.dtpFrom.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "С:";
            // 
            // textBoxCardsId
            // 
            this.textBoxCardsId.Location = new System.Drawing.Point(72, 15);
            this.textBoxCardsId.Name = "textBoxCardsId";
            this.textBoxCardsId.Size = new System.Drawing.Size(242, 20);
            this.textBoxCardsId.TabIndex = 0;
            this.textBoxCardsId.TextChanged += new System.EventHandler(this.textBoxCardsId_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Карточки:";
            // 
            // comboBoxExpert
            // 
            this.comboBoxExpert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExpert.FormattingEnabled = true;
            this.comboBoxExpert.Location = new System.Drawing.Point(72, 40);
            this.comboBoxExpert.Name = "comboBoxExpert";
            this.comboBoxExpert.Size = new System.Drawing.Size(212, 21);
            this.comboBoxExpert.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Эксперт:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(8, 112);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.tabControlResult);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvHistory);
            this.splitContainer1.Size = new System.Drawing.Size(778, 340);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControlResult
            // 
            this.tabControlResult.Controls.Add(this.tabPageFilter);
            this.tabControlResult.Controls.Add(this.tabPageFind);
            this.tabControlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResult.Location = new System.Drawing.Point(0, 0);
            this.tabControlResult.Name = "tabControlResult";
            this.tabControlResult.SelectedIndex = 0;
            this.tabControlResult.Size = new System.Drawing.Size(257, 340);
            this.tabControlResult.TabIndex = 0;
            // 
            // tabPageFilter
            // 
            this.tabPageFilter.Controls.Add(this.treeViewCard);
            this.tabPageFilter.Location = new System.Drawing.Point(4, 22);
            this.tabPageFilter.Name = "tabPageFilter";
            this.tabPageFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilter.Size = new System.Drawing.Size(249, 314);
            this.tabPageFilter.TabIndex = 0;
            this.tabPageFilter.Text = "Фильтр";
            this.tabPageFilter.UseVisualStyleBackColor = true;
            // 
            // treeViewCard
            // 
            this.treeViewCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCard.HideSelection = false;
            this.treeViewCard.Location = new System.Drawing.Point(3, 3);
            this.treeViewCard.Name = "treeViewCard";
            this.treeViewCard.Size = new System.Drawing.Size(243, 308);
            this.treeViewCard.TabIndex = 0;
            this.treeViewCard.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            this.treeViewCard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            this.treeViewCard.Click += new System.EventHandler(this.treeViewCard_Click);
            // 
            // tabPageFind
            // 
            this.tabPageFind.Controls.Add(this.treeViewFind);
            this.tabPageFind.Location = new System.Drawing.Point(4, 22);
            this.tabPageFind.Name = "tabPageFind";
            this.tabPageFind.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFind.Size = new System.Drawing.Size(249, 314);
            this.tabPageFind.TabIndex = 1;
            this.tabPageFind.Text = "Результаты поиска";
            this.tabPageFind.UseVisualStyleBackColor = true;
            // 
            // treeViewFind
            // 
            this.treeViewFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFind.HideSelection = false;
            this.treeViewFind.Location = new System.Drawing.Point(3, 3);
            this.treeViewFind.Name = "treeViewFind";
            this.treeViewFind.Size = new System.Drawing.Size(243, 308);
            this.treeViewFind.TabIndex = 0;
            this.treeViewFind.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            this.treeViewFind.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            this.treeViewFind.Click += new System.EventHandler(this.treeViewCard_Click);
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistory.Location = new System.Drawing.Point(0, 0);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersWidth = 16;
            this.dgvHistory.Size = new System.Drawing.Size(517, 340);
            this.dgvHistory.TabIndex = 0;
            this.dgvHistory.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellClick);
            // 
            // contextMenuExpert
            // 
            this.contextMenuExpert.Name = "contextMenuExpert";
            this.contextMenuExpert.Size = new System.Drawing.Size(61, 4);
            this.contextMenuExpert.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuExpert_ItemClicked);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonClose);
            this.panel2.Controls.Add(this.buttonHelp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(8, 452);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(778, 35);
            this.panel2.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(622, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Закрыть";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(703, 12);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonHelp.TabIndex = 1;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(8);
            this.panel3.Size = new System.Drawing.Size(794, 495);
            this.panel3.TabIndex = 1;
            // 
            // FormHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 495);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormHistory";
            this.ShowInTaskbar = false;
            this.Text = "История карточек";
            this.Load += new System.EventHandler(this.FormHistory_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormHistory_FormClosed);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControlResult.ResumeLayout(false);
            this.tabPageFilter.ResumeLayout(false);
            this.tabPageFind.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.ComboBox comboBoxCardFields;
        private System.Windows.Forms.TextBox textBoxFindString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxCardsId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxExpert;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxIk2;
        private System.Windows.Forms.CheckBox checkBoxIkl;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.TabControl tabControlResult;
        private System.Windows.Forms.TabPage tabPageFilter;
        private System.Windows.Forms.TreeView treeViewCard;
        private System.Windows.Forms.TabPage tabPageFind;
        private System.Windows.Forms.TreeView treeViewFind;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.Button buttonExpert;
        private System.Windows.Forms.ContextMenuStrip contextMenuExpert;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonHelp;
    }
}