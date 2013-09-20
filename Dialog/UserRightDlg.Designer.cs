namespace GeneLibrary.Dialog
{
    partial class UserRightForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserRightForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRevokeRole = new System.Windows.Forms.Button();
            this.buttonRoleAccept = new System.Windows.Forms.Button();
            this.listBoxAcceptRole = new System.Windows.Forms.ListBox();
            this.listBoxAllRole = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRevokeRevoke = new System.Windows.Forms.Button();
            this.buttonRevokeRight = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.listBoxRevokeRight = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAcceptRightRevoke = new System.Windows.Forms.Button();
            this.buttonRightAccept = new System.Windows.Forms.Button();
            this.listBoxAcceptRight = new System.Windows.Forms.ListBox();
            this.listBoxAllRight = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(477, 475);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(396, 475);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonRevokeRole);
            this.groupBox1.Controls.Add(this.buttonRoleAccept);
            this.groupBox1.Controls.Add(this.listBoxAcceptRole);
            this.groupBox1.Controls.Add(this.listBoxAllRole);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 218);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Роли";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Назначенные";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Доступные";
            // 
            // buttonRevokeRole
            // 
            this.buttonRevokeRole.Enabled = false;
            this.buttonRevokeRole.Location = new System.Drawing.Point(246, 61);
            this.buttonRevokeRole.Name = "buttonRevokeRole";
            this.buttonRevokeRole.Size = new System.Drawing.Size(47, 23);
            this.buttonRevokeRole.TabIndex = 3;
            this.buttonRevokeRole.Text = "<";
            this.buttonRevokeRole.UseVisualStyleBackColor = true;
            this.buttonRevokeRole.Click += new System.EventHandler(this.btnRoleToRevoke_Click);
            // 
            // buttonRoleAccept
            // 
            this.buttonRoleAccept.Enabled = false;
            this.buttonRoleAccept.Location = new System.Drawing.Point(246, 32);
            this.buttonRoleAccept.Name = "buttonRoleAccept";
            this.buttonRoleAccept.Size = new System.Drawing.Size(47, 23);
            this.buttonRoleAccept.TabIndex = 2;
            this.buttonRoleAccept.Text = ">";
            this.buttonRoleAccept.UseVisualStyleBackColor = true;
            this.buttonRoleAccept.Click += new System.EventHandler(this.btnRoleToAccept_Click);
            // 
            // listBoxAcceptRole
            // 
            this.listBoxAcceptRole.DisplayMember = "name";
            this.listBoxAcceptRole.FormattingEnabled = true;
            this.listBoxAcceptRole.Location = new System.Drawing.Point(299, 32);
            this.listBoxAcceptRole.Name = "listBoxAcceptRole";
            this.listBoxAcceptRole.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAcceptRole.Size = new System.Drawing.Size(234, 173);
            this.listBoxAcceptRole.TabIndex = 4;
            this.listBoxAcceptRole.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            this.listBoxAcceptRole.DoubleClick += new System.EventHandler(this.lbxAcceptRole_DoubleClick);
            // 
            // listBoxAllRole
            // 
            this.listBoxAllRole.DisplayMember = "name";
            this.listBoxAllRole.FormattingEnabled = true;
            this.listBoxAllRole.Location = new System.Drawing.Point(6, 32);
            this.listBoxAllRole.Name = "listBoxAllRole";
            this.listBoxAllRole.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAllRole.Size = new System.Drawing.Size(234, 173);
            this.listBoxAllRole.TabIndex = 1;
            this.listBoxAllRole.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            this.listBoxAllRole.DoubleClick += new System.EventHandler(this.lbxAllRole_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRevokeRevoke);
            this.groupBox2.Controls.Add(this.buttonRevokeRight);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.listBoxRevokeRight);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonAcceptRightRevoke);
            this.groupBox2.Controls.Add(this.buttonRightAccept);
            this.groupBox2.Controls.Add(this.listBoxAcceptRight);
            this.groupBox2.Controls.Add(this.listBoxAllRight);
            this.groupBox2.Location = new System.Drawing.Point(12, 236);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(540, 218);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Права";
            // 
            // buttonRevokeRevoke
            // 
            this.buttonRevokeRevoke.Enabled = false;
            this.buttonRevokeRevoke.Location = new System.Drawing.Point(246, 152);
            this.buttonRevokeRevoke.Name = "buttonRevokeRevoke";
            this.buttonRevokeRevoke.Size = new System.Drawing.Size(47, 23);
            this.buttonRevokeRevoke.TabIndex = 15;
            this.buttonRevokeRevoke.Text = "<";
            this.buttonRevokeRevoke.UseVisualStyleBackColor = true;
            this.buttonRevokeRevoke.Click += new System.EventHandler(this.RevokeRevoke_Click);
            // 
            // buttonRevokeRight
            // 
            this.buttonRevokeRight.Enabled = false;
            this.buttonRevokeRight.Location = new System.Drawing.Point(246, 123);
            this.buttonRevokeRight.Name = "buttonRevokeRight";
            this.buttonRevokeRight.Size = new System.Drawing.Size(47, 23);
            this.buttonRevokeRight.TabIndex = 14;
            this.buttonRevokeRight.Text = ">";
            this.buttonRevokeRight.UseVisualStyleBackColor = true;
            this.buttonRevokeRight.Click += new System.EventHandler(this.btnAcceptRevoke_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(296, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Индивидуальные запрещения";
            // 
            // listBoxRevokeRight
            // 
            this.listBoxRevokeRight.DisplayMember = "name";
            this.listBoxRevokeRight.FormattingEnabled = true;
            this.listBoxRevokeRight.Location = new System.Drawing.Point(299, 123);
            this.listBoxRevokeRight.Name = "listBoxRevokeRight";
            this.listBoxRevokeRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxRevokeRight.Size = new System.Drawing.Size(234, 82);
            this.listBoxRevokeRight.TabIndex = 12;
            this.listBoxRevokeRight.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(296, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Индивидуальные назначения";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Доступные";
            // 
            // buttonAcceptRightRevoke
            // 
            this.buttonAcceptRightRevoke.Enabled = false;
            this.buttonAcceptRightRevoke.Location = new System.Drawing.Point(246, 61);
            this.buttonAcceptRightRevoke.Name = "buttonAcceptRightRevoke";
            this.buttonAcceptRightRevoke.Size = new System.Drawing.Size(47, 23);
            this.buttonAcceptRightRevoke.TabIndex = 8;
            this.buttonAcceptRightRevoke.Text = "<";
            this.buttonAcceptRightRevoke.UseVisualStyleBackColor = true;
            this.buttonAcceptRightRevoke.Click += new System.EventHandler(this.btnRightToRevoke_Click);
            // 
            // buttonRightAccept
            // 
            this.buttonRightAccept.Enabled = false;
            this.buttonRightAccept.Location = new System.Drawing.Point(246, 32);
            this.buttonRightAccept.Name = "buttonRightAccept";
            this.buttonRightAccept.Size = new System.Drawing.Size(47, 23);
            this.buttonRightAccept.TabIndex = 7;
            this.buttonRightAccept.Text = ">";
            this.buttonRightAccept.UseVisualStyleBackColor = true;
            this.buttonRightAccept.Click += new System.EventHandler(this.btnRightToAccept_Click);
            // 
            // listBoxAcceptRight
            // 
            this.listBoxAcceptRight.DisplayMember = "name";
            this.listBoxAcceptRight.FormattingEnabled = true;
            this.listBoxAcceptRight.Location = new System.Drawing.Point(299, 32);
            this.listBoxAcceptRight.Name = "listBoxAcceptRight";
            this.listBoxAcceptRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAcceptRight.Size = new System.Drawing.Size(234, 69);
            this.listBoxAcceptRight.TabIndex = 9;
            this.listBoxAcceptRight.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            this.listBoxAcceptRight.DoubleClick += new System.EventHandler(this.lbxAcceptRightUser_DoubleClick);
            // 
            // listBoxAllRight
            // 
            this.listBoxAllRight.DisplayMember = "name";
            this.listBoxAllRight.FormattingEnabled = true;
            this.listBoxAllRight.Location = new System.Drawing.Point(6, 32);
            this.listBoxAllRight.Name = "listBoxAllRight";
            this.listBoxAllRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAllRight.Size = new System.Drawing.Size(234, 173);
            this.listBoxAllRight.TabIndex = 6;
            this.listBoxAllRight.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // UserRightForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(564, 510);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserRightForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Права пользователя";
            this.Load += new System.EventHandler(this.UserRightDlg_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonRevokeRole;
        private System.Windows.Forms.Button buttonRoleAccept;
        private System.Windows.Forms.ListBox listBoxAcceptRole;
        private System.Windows.Forms.ListBox listBoxAllRole;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonAcceptRightRevoke;
        private System.Windows.Forms.Button buttonRightAccept;
        private System.Windows.Forms.ListBox listBoxAcceptRight;
        private System.Windows.Forms.ListBox listBoxAllRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBoxRevokeRight;
        private System.Windows.Forms.Button buttonRevokeRight;
        private System.Windows.Forms.Button buttonRevokeRevoke;
    }
}