namespace GeneLibrary.MdiForms
{
    partial class FormFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFilter));
            this.toolStripFilter = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonToExcel = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewFilter = new System.Windows.Forms.DataGridView();
            this.toolStripFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripFilter
            // 
            this.toolStripFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonToExcel});
            this.toolStripFilter.Location = new System.Drawing.Point(0, 0);
            this.toolStripFilter.Name = "toolStripFilter";
            this.toolStripFilter.Size = new System.Drawing.Size(615, 25);
            this.toolStripFilter.TabIndex = 1;
            this.toolStripFilter.Text = "toolStrip1";
            // 
            // toolStripButtonToExcel
            // 
            this.toolStripButtonToExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToExcel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToExcel.Image")));
            this.toolStripButtonToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToExcel.Name = "toolStripButtonToExcel";
            this.toolStripButtonToExcel.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonToExcel.Text = "toolStripButton1";
            this.toolStripButtonToExcel.ToolTipText = "Excel";
            this.toolStripButtonToExcel.Click += new System.EventHandler(this.toolStripButtonToExcel_Click);
            // 
            // dataGridViewFilter
            // 
            this.dataGridViewFilter.AllowUserToAddRows = false;
            this.dataGridViewFilter.AllowUserToDeleteRows = false;
            this.dataGridViewFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFilter.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewFilter.Name = "dataGridViewFilter";
            this.dataGridViewFilter.ReadOnly = true;
            this.dataGridViewFilter.RowHeadersWidth = 16;
            this.dataGridViewFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFilter.ShowCellErrors = false;
            this.dataGridViewFilter.ShowCellToolTips = false;
            this.dataGridViewFilter.ShowEditingIcon = false;
            this.dataGridViewFilter.ShowRowErrors = false;
            this.dataGridViewFilter.Size = new System.Drawing.Size(615, 421);
            this.dataGridViewFilter.StandardTab = true;
            this.dataGridViewFilter.TabIndex = 2;
            // 
            // FormFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 446);
            this.Controls.Add(this.dataGridViewFilter);
            this.Controls.Add(this.toolStripFilter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFilter";
            this.Text = "Выбранные карточки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFilter_FormClosing);
            this.Load += new System.EventHandler(this.FormFilter_Load);
            this.toolStripFilter.ResumeLayout(false);
            this.toolStripFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripFilter;
        private System.Windows.Forms.DataGridView dataGridViewFilter;
        private System.Windows.Forms.ToolStripButton toolStripButtonToExcel;


    }
}