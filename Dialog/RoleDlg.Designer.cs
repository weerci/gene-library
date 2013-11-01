namespace GeneLibrary.Dialog
{
    partial class RoleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoleForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.buttonPermitions = new System.Windows.Forms.Button();
            this.textBoxRole = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOnRoles = new System.Windows.Forms.ComboBox();
            this.listBoxAcceptRight = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(202, 284);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(121, 284);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // buttonPermitions
            // 
            this.buttonPermitions.Location = new System.Drawing.Point(222, 246);
            this.buttonPermitions.Name = "buttonPermitions";
            this.buttonPermitions.Size = new System.Drawing.Size(54, 23);
            this.buttonPermitions.TabIndex = 2;
            this.buttonPermitions.Text = "Права";
            this.buttonPermitions.UseVisualStyleBackColor = true;
            this.buttonPermitions.Click += new System.EventHandler(this.btnSetPermission_Click);
            // 
            // textBoxRole
            // 
            this.textBoxRole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRole.Location = new System.Drawing.Point(50, 15);
            this.textBoxRole.MaxLength = 32;
            this.textBoxRole.Name = "textBoxRole";
            this.textBoxRole.Size = new System.Drawing.Size(226, 20);
            this.textBoxRole.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Роль";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Права доступа";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "На основе";
            // 
            // comboBoxOnRoles
            // 
            this.comboBoxOnRoles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOnRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOnRoles.FormattingEnabled = true;
            this.comboBoxOnRoles.Location = new System.Drawing.Point(78, 246);
            this.comboBoxOnRoles.Name = "comboBoxOnRoles";
            this.comboBoxOnRoles.Size = new System.Drawing.Size(138, 21);
            this.comboBoxOnRoles.TabIndex = 1;
            // 
            // listBoxAcceptRight
            // 
            this.listBoxAcceptRight.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.listBoxAcceptRight.DisplayMember = "name";
            this.listBoxAcceptRight.FormattingEnabled = true;
            this.listBoxAcceptRight.Location = new System.Drawing.Point(15, 57);
            this.listBoxAcceptRight.Name = "listBoxAcceptRight";
            this.listBoxAcceptRight.Size = new System.Drawing.Size(261, 173);
            this.listBoxAcceptRight.TabIndex = 25;
            this.listBoxAcceptRight.TabStop = false;
            // 
            // RoleForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(288, 319);
            this.Controls.Add(this.listBoxAcceptRight);
            this.Controls.Add(this.comboBoxOnRoles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonPermitions);
            this.Controls.Add(this.textBoxRole);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Роль";
            this.Load += new System.EventHandler(this.RoleDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button buttonPermitions;
        private System.Windows.Forms.TextBox textBoxRole;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOnRoles;
        private System.Windows.Forms.ListBox listBoxAcceptRight;
    }
}