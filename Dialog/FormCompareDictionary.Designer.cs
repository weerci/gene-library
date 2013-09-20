namespace GeneLibrary.Dialog
{
    partial class FormCompareDictionary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompareDictionary));
            this.textBoxComparer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridViewCompared = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCompare = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxComparer
            // 
            this.textBoxComparer.AllowDrop = true;
            this.textBoxComparer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComparer.Location = new System.Drawing.Point(15, 41);
            this.textBoxComparer.Name = "textBoxComparer";
            this.textBoxComparer.ReadOnly = true;
            this.textBoxComparer.Size = new System.Drawing.Size(529, 20);
            this.textBoxComparer.TabIndex = 1;
            this.textBoxComparer.TextChanged += new System.EventHandler(this.textBoxComparer_TextChanged);
            this.textBoxComparer.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox_DragDrop);
            this.textBoxComparer.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox_DragOver);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Сопоставляемые элементы";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(369, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Идентификатор, с которым будут сопоставлены элементы спрвочника";
            // 
            // dataGridViewCompared
            // 
            this.dataGridViewCompared.AllowDrop = true;
            this.dataGridViewCompared.AllowUserToAddRows = false;
            this.dataGridViewCompared.AllowUserToDeleteRows = false;
            this.dataGridViewCompared.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCompared.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCompared.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCompared.ColumnHeadersVisible = false;
            this.dataGridViewCompared.Location = new System.Drawing.Point(15, 80);
            this.dataGridViewCompared.Name = "dataGridViewCompared";
            this.dataGridViewCompared.ReadOnly = true;
            this.dataGridViewCompared.RowHeadersWidth = 16;
            this.dataGridViewCompared.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCompared.Size = new System.Drawing.Size(529, 281);
            this.dataGridViewCompared.StandardTab = true;
            this.dataGridViewCompared.TabIndex = 6;
            this.dataGridViewCompared.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            this.dataGridViewCompared.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox_DragOver);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDelete,
            this.toolStripButtonCompare});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(556, 25);
            this.toolStrip1.TabIndex = 7;
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
            // toolStripButtonCompare
            // 
            this.toolStripButtonCompare.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCompare.Image = global::GeneLibrary.Properties.Resources.Forward___Next;
            this.toolStripButtonCompare.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCompare.Name = "toolStripButtonCompare";
            this.toolStripButtonCompare.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCompare.Text = "Сопоставить";
            this.toolStripButtonCompare.ToolTipText = "Сопоставить";
            this.toolStripButtonCompare.Click += new System.EventHandler(this.toolStripButtonCompare_Click);
            // 
            // FormCompareDictionary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 373);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGridViewCompared);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxComparer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCompareDictionary";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сопоставление справочников";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormCompareDictionary_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxComparer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridViewCompared;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonCompare;
    }
}