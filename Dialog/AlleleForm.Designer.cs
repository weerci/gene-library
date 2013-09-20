namespace GeneLibrary.Dialog
{
    partial class AlleleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlleleForm));
            this.checkedListBoxAllele = new System.Windows.Forms.CheckedListBox();
            this.listBoxLocus = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBoxAllele
            // 
            this.checkedListBoxAllele.CheckOnClick = true;
            this.checkedListBoxAllele.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkedListBoxAllele.FormattingEnabled = true;
            this.checkedListBoxAllele.IntegralHeight = false;
            this.checkedListBoxAllele.Location = new System.Drawing.Point(160, 26);
            this.checkedListBoxAllele.Name = "checkedListBoxAllele";
            this.checkedListBoxAllele.Size = new System.Drawing.Size(160, 441);
            this.checkedListBoxAllele.TabIndex = 0;
            this.checkedListBoxAllele.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxAllele_ItemCheck);
            // 
            // listBoxLocus
            // 
            this.listBoxLocus.BackColor = System.Drawing.SystemColors.Window;
            this.listBoxLocus.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxLocus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxLocus.FormattingEnabled = true;
            this.listBoxLocus.IntegralHeight = false;
            this.listBoxLocus.ItemHeight = 16;
            this.listBoxLocus.Location = new System.Drawing.Point(0, 26);
            this.listBoxLocus.Name = "listBoxLocus";
            this.listBoxLocus.Size = new System.Drawing.Size(160, 441);
            this.listBoxLocus.TabIndex = 1;
            this.listBoxLocus.SelectedIndexChanged += new System.EventHandler(this.listBoxLocus_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 26);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Аллели";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Список локусов";
            // 
            // AlleleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 467);
            this.Controls.Add(this.checkedListBoxAllele);
            this.Controls.Add(this.listBoxLocus);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlleleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список локусов";
            this.Load += new System.EventHandler(this.AlleleForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AlleleForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxAllele;
        private System.Windows.Forms.ListBox listBoxLocus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;




    }
}