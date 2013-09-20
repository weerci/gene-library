namespace GeneLibrary.Dialog
{
    partial class AddEditRight
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEditRight));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.listBoxAllRight = new System.Windows.Forms.ListBox();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonRevoke = new System.Windows.Forms.Button();
            this.listBoxAcceptRight = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(501, 357);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(420, 357);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // listBoxAllRight
            // 
            this.listBoxAllRight.AllowDrop = true;
            this.listBoxAllRight.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.listBoxAllRight.DisplayMember = "name";
            this.listBoxAllRight.FormattingEnabled = true;
            this.listBoxAllRight.Location = new System.Drawing.Point(12, 25);
            this.listBoxAllRight.Name = "listBoxAllRight";
            this.listBoxAllRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAllRight.Size = new System.Drawing.Size(239, 316);
            this.listBoxAllRight.TabIndex = 8;
            this.listBoxAllRight.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxAllRight_MouseDoubleClick);
            this.listBoxAllRight.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // buttonAccept
            // 
            this.buttonAccept.Enabled = false;
            this.buttonAccept.Location = new System.Drawing.Point(257, 38);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 10;
            this.buttonAccept.Text = ">";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.btnToAccept_Click);
            // 
            // buttonRevoke
            // 
            this.buttonRevoke.Enabled = false;
            this.buttonRevoke.Location = new System.Drawing.Point(257, 67);
            this.buttonRevoke.Name = "buttonRevoke";
            this.buttonRevoke.Size = new System.Drawing.Size(75, 23);
            this.buttonRevoke.TabIndex = 12;
            this.buttonRevoke.Text = "<";
            this.buttonRevoke.UseVisualStyleBackColor = true;
            this.buttonRevoke.Click += new System.EventHandler(this.btnRevoke_Click);
            // 
            // listBoxAcceptRight
            // 
            this.listBoxAcceptRight.AllowDrop = true;
            this.listBoxAcceptRight.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.listBoxAcceptRight.DisplayMember = "name";
            this.listBoxAcceptRight.FormattingEnabled = true;
            this.listBoxAcceptRight.Location = new System.Drawing.Point(338, 25);
            this.listBoxAcceptRight.Name = "listBoxAcceptRight";
            this.listBoxAcceptRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAcceptRight.Size = new System.Drawing.Size(239, 316);
            this.listBoxAcceptRight.TabIndex = 14;
            this.listBoxAcceptRight.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxAcceptRight_MouseDoubleClick);
            this.listBoxAcceptRight.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Доступные права";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(335, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Назначенные права";
            // 
            // AddEditRight
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(588, 392);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxAcceptRight);
            this.Controls.Add(this.buttonRevoke);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.listBoxAllRight);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditRight";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Права для роли";
            this.Load += new System.EventHandler(this.AddEditRight_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListBox listBoxAllRight;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonRevoke;
        private System.Windows.Forms.ListBox listBoxAcceptRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}