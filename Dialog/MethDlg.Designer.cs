namespace GeneLibrary.Dialog
{
    partial class MethodForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MethodForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxFrequency = new System.Windows.Forms.TextBox();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddLocus = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteLocus = new System.Windows.Forms.ToolStripButton();
            this.dgrLocus = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsbAddAllele = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteAllele = new System.Windows.Forms.ToolStripButton();
            this.dgrAllele = new System.Windows.Forms.DataGridView();
            this.toolTipMethodForm = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrLocus)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrAllele)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(445, 509);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(364, 509);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Применить";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxFrequency);
            this.groupBox1.Controls.Add(this.tbDesc);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Метод";
            // 
            // textBoxFrequency
            // 
            this.textBoxFrequency.Location = new System.Drawing.Point(135, 46);
            this.textBoxFrequency.Name = "textBoxFrequency";
            this.textBoxFrequency.Size = new System.Drawing.Size(117, 20);
            this.textBoxFrequency.TabIndex = 1;
            this.textBoxFrequency.TextChanged += new System.EventHandler(this.textBoxFrequency_TextChanged);
            // 
            // tbDesc
            // 
            this.tbDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDesc.Location = new System.Drawing.Point(261, 29);
            this.tbDesc.MaxLength = 1024;
            this.tbDesc.Multiline = true;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(241, 51);
            this.tbDesc.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(258, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Описание";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Частота по умолчанию";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Название";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(69, 13);
            this.tbName.MaxLength = 100;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(183, 20);
            this.tbName.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Controls.Add(this.dgrLocus);
            this.groupBox2.Location = new System.Drawing.Point(12, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(172, 428);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Локусы";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddLocus,
            this.tsbDeleteLocus});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(166, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.TabStop = true;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAddLocus
            // 
            this.tsbAddLocus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddLocus.Image = global::GeneLibrary.Properties.Resources.New_Doc;
            this.tsbAddLocus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddLocus.Name = "tsbAddLocus";
            this.tsbAddLocus.Size = new System.Drawing.Size(23, 22);
            this.tsbAddLocus.Text = "Создать локус";
            this.tsbAddLocus.Click += new System.EventHandler(this.tsbAddLocus_Click);
            // 
            // tsbDeleteLocus
            // 
            this.tsbDeleteLocus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteLocus.Enabled = false;
            this.tsbDeleteLocus.Image = global::GeneLibrary.Properties.Resources.Delete;
            this.tsbDeleteLocus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteLocus.Name = "tsbDeleteLocus";
            this.tsbDeleteLocus.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteLocus.Text = "Удалить локус";
            this.tsbDeleteLocus.Click += new System.EventHandler(this.tsbDeleteLocus_Click);
            // 
            // dgrLocus
            // 
            this.dgrLocus.AllowUserToAddRows = false;
            this.dgrLocus.AllowUserToDeleteRows = false;
            this.dgrLocus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgrLocus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgrLocus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrLocus.Location = new System.Drawing.Point(3, 44);
            this.dgrLocus.Name = "dgrLocus";
            this.dgrLocus.ReadOnly = true;
            this.dgrLocus.RowHeadersWidth = 16;
            this.dgrLocus.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgrLocus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrLocus.ShowRowErrors = false;
            this.dgrLocus.Size = new System.Drawing.Size(166, 381);
            this.dgrLocus.StandardTab = true;
            this.dgrLocus.TabIndex = 0;
            this.dgrLocus.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrLocus_CellClick);
            this.dgrLocus.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrLocus_RowEnter);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.toolStrip2);
            this.groupBox3.Controls.Add(this.dgrAllele);
            this.groupBox3.Location = new System.Drawing.Point(189, 104);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(331, 399);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Аллели";
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddAllele,
            this.tsbDeleteAllele});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip2.Size = new System.Drawing.Size(325, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.TabStop = true;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsbAddAllele
            // 
            this.tsbAddAllele.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddAllele.Image = global::GeneLibrary.Properties.Resources.New_Doc;
            this.tsbAddAllele.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddAllele.Name = "tsbAddAllele";
            this.tsbAddAllele.Size = new System.Drawing.Size(23, 22);
            this.tsbAddAllele.Text = "Добавить аллель";
            this.tsbAddAllele.Click += new System.EventHandler(this.tsbAddAllele_Click);
            // 
            // tsbDeleteAllele
            // 
            this.tsbDeleteAllele.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteAllele.Enabled = false;
            this.tsbDeleteAllele.Image = global::GeneLibrary.Properties.Resources.Delete;
            this.tsbDeleteAllele.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteAllele.Name = "tsbDeleteAllele";
            this.tsbDeleteAllele.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteAllele.Text = "Удалить аллель";
            this.tsbDeleteAllele.Click += new System.EventHandler(this.tsbRemoveAllele_Click);
            // 
            // dgrAllele
            // 
            this.dgrAllele.AllowUserToAddRows = false;
            this.dgrAllele.AllowUserToDeleteRows = false;
            this.dgrAllele.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgrAllele.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgrAllele.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrAllele.Location = new System.Drawing.Point(3, 44);
            this.dgrAllele.MultiSelect = false;
            this.dgrAllele.Name = "dgrAllele";
            this.dgrAllele.RowHeadersWidth = 16;
            this.dgrAllele.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgrAllele.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgrAllele.Size = new System.Drawing.Size(325, 352);
            this.dgrAllele.StandardTab = true;
            this.dgrAllele.TabIndex = 0;
            this.dgrAllele.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrLocus_CellClick);
            this.dgrAllele.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgrAllele_EditingControlShowing);
            // 
            // MethodForm
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(532, 544);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MethodForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Метод расчета";
            this.Load += new System.EventHandler(this.FreqDlg_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrLocus)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrAllele)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgrLocus;
        private System.Windows.Forms.DataGridView dgrAllele;
        private System.Windows.Forms.ToolTip toolTipMethodForm;
        private System.Windows.Forms.TextBox textBoxFrequency;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddLocus;
        private System.Windows.Forms.ToolStripButton tsbDeleteLocus;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsbAddAllele;
        private System.Windows.Forms.ToolStripButton tsbDeleteAllele;
    }
}