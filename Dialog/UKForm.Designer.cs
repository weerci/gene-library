namespace GeneLibrary.Dialog
{
    partial class UKForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UKForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tcUK = new System.Windows.Forms.TabControl();
            this.tpArtcl = new System.Windows.Forms.TabPage();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.dgrParts = new System.Windows.Forms.DataGridView();
            this.dgrItems = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbNote = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbArtcl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpTexts = new System.Windows.Forms.TabPage();
            this.tbText = new System.Windows.Forms.TextBox();
            this.tcUK.SuspendLayout();
            this.tpArtcl.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrParts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgrItems)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tpTexts.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(414, 390);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(333, 390);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tcUK
            // 
            this.tcUK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcUK.Controls.Add(this.tpArtcl);
            this.tcUK.Controls.Add(this.tpTexts);
            this.tcUK.Location = new System.Drawing.Point(0, 0);
            this.tcUK.Name = "tcUK";
            this.tcUK.SelectedIndex = 0;
            this.tcUK.Size = new System.Drawing.Size(501, 384);
            this.tcUK.TabIndex = 8;
            this.tcUK.TabStop = false;
            // 
            // tpArtcl
            // 
            this.tpArtcl.Controls.Add(this.scMain);
            this.tpArtcl.Controls.Add(this.groupBox1);
            this.tpArtcl.Location = new System.Drawing.Point(4, 22);
            this.tpArtcl.Name = "tpArtcl";
            this.tpArtcl.Padding = new System.Windows.Forms.Padding(3);
            this.tpArtcl.Size = new System.Drawing.Size(493, 358);
            this.tpArtcl.TabIndex = 0;
            this.tpArtcl.Text = "Статьи";
            this.tpArtcl.UseVisualStyleBackColor = true;
            // 
            // scMain
            // 
            this.scMain.Location = new System.Drawing.Point(3, 102);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.dgrParts);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.dgrItems);
            this.scMain.Size = new System.Drawing.Size(484, 253);
            this.scMain.SplitterDistance = 244;
            this.scMain.TabIndex = 10;
            this.scMain.TabStop = false;
            // 
            // dgrParts
            // 
            this.dgrParts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgrParts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgrParts.Location = new System.Drawing.Point(0, 0);
            this.dgrParts.Name = "dgrParts";
            this.dgrParts.Size = new System.Drawing.Size(244, 253);
            this.dgrParts.TabIndex = 3;
            this.dgrParts.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgrParts_UserAddedRow);
            // 
            // dgrItems
            // 
            this.dgrItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgrItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgrItems.Location = new System.Drawing.Point(0, 0);
            this.dgrItems.Name = "dgrItems";
            this.dgrItems.Size = new System.Drawing.Size(236, 253);
            this.dgrItems.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbNote);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbArtcl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 93);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Статья";
            // 
            // tbNote
            // 
            this.tbNote.Location = new System.Drawing.Point(194, 32);
            this.tbNote.MaxLength = 512;
            this.tbNote.Multiline = true;
            this.tbNote.Name = "tbNote";
            this.tbNote.Size = new System.Drawing.Size(290, 55);
            this.tbNote.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Описание";
            // 
            // tbArtcl
            // 
            this.tbArtcl.Location = new System.Drawing.Point(9, 32);
            this.tbArtcl.Name = "tbArtcl";
            this.tbArtcl.Size = new System.Drawing.Size(179, 20);
            this.tbArtcl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер";
            // 
            // tpTexts
            // 
            this.tpTexts.Controls.Add(this.tbText);
            this.tpTexts.Location = new System.Drawing.Point(4, 22);
            this.tpTexts.Name = "tpTexts";
            this.tpTexts.Padding = new System.Windows.Forms.Padding(3);
            this.tpTexts.Size = new System.Drawing.Size(493, 358);
            this.tpTexts.TabIndex = 1;
            this.tpTexts.Text = "Текст";
            this.tpTexts.UseVisualStyleBackColor = true;
            // 
            // tbText
            // 
            this.tbText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbText.Location = new System.Drawing.Point(3, 3);
            this.tbText.MaxLength = 512;
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(487, 352);
            this.tbText.TabIndex = 1;
            // 
            // UKForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(501, 425);
            this.Controls.Add(this.tcUK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UKForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Статьи УК";
            this.Load += new System.EventHandler(this.UKDlg_Load);
            this.tcUK.ResumeLayout(false);
            this.tpArtcl.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgrParts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgrItems)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpTexts.ResumeLayout(false);
            this.tpTexts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TabControl tcUK;
        private System.Windows.Forms.TabPage tpArtcl;
        private System.Windows.Forms.TabPage tpTexts;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbNote;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbArtcl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.DataGridView dgrParts;
        private System.Windows.Forms.DataGridView dgrItems;
    }
}