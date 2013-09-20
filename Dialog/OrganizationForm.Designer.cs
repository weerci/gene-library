namespace GeneLibrary.Dialog
{
    partial class OrganizationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrganizationForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonFavoritOrganization = new System.Windows.Forms.Button();
            this.contextMenuStripOrganization = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dataGridViewOrganization = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrganization)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(205, 378);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(124, 378);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // textBoxFind
            // 
            this.textBoxFind.Location = new System.Drawing.Point(53, 6);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(192, 20);
            this.textBoxFind.TabIndex = 0;
            this.textBoxFind.TextChanged += new System.EventHandler(this.textBoxFind_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Поиск";
            // 
            // buttonFavoritOrganization
            // 
            this.buttonFavoritOrganization.Location = new System.Drawing.Point(251, 4);
            this.buttonFavoritOrganization.Name = "buttonFavoritOrganization";
            this.buttonFavoritOrganization.Size = new System.Drawing.Size(29, 23);
            this.buttonFavoritOrganization.TabIndex = 1;
            this.buttonFavoritOrganization.Text = "...";
            this.buttonFavoritOrganization.UseVisualStyleBackColor = true;
            this.buttonFavoritOrganization.Click += new System.EventHandler(this.buttonFavoritOrganization_Click);
            // 
            // contextMenuStripOrganization
            // 
            this.contextMenuStripOrganization.Name = "contextMenuStripOrganization";
            this.contextMenuStripOrganization.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripOrganization.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripOrganization_ItemClicked);
            // 
            // dataGridViewOrganization
            // 
            this.dataGridViewOrganization.AllowUserToAddRows = false;
            this.dataGridViewOrganization.AllowUserToDeleteRows = false;
            this.dataGridViewOrganization.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOrganization.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrganization.Location = new System.Drawing.Point(8, 33);
            this.dataGridViewOrganization.Name = "dataGridViewOrganization";
            this.dataGridViewOrganization.ReadOnly = true;
            this.dataGridViewOrganization.RowHeadersWidth = 16;
            this.dataGridViewOrganization.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOrganization.ShowCellErrors = false;
            this.dataGridViewOrganization.ShowCellToolTips = false;
            this.dataGridViewOrganization.ShowEditingIcon = false;
            this.dataGridViewOrganization.ShowRowErrors = false;
            this.dataGridViewOrganization.Size = new System.Drawing.Size(272, 339);
            this.dataGridViewOrganization.StandardTab = true;
            this.dataGridViewOrganization.TabIndex = 8;
            this.dataGridViewOrganization.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewOrganization_CellDoubleClick);
            // 
            // OrganizationForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 413);
            this.Controls.Add(this.dataGridViewOrganization);
            this.Controls.Add(this.buttonFavoritOrganization);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFind);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Организация ";
            this.Load += new System.EventHandler(this.OrganizationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrganization)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox textBoxFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonFavoritOrganization;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOrganization;
        private System.Windows.Forms.DataGridView dataGridViewOrganization;
    }
}