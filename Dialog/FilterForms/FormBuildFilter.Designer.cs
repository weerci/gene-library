namespace GeneLibrary.Dialog
{
    partial class FormBuildFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBuildFilter));
            this.tabControlCardType = new System.Windows.Forms.TabControl();
            this.tabPageIkl = new System.Windows.Forms.TabPage();
            this.listBoxIkl = new System.Windows.Forms.ListBox();
            this.tabPageIk2 = new System.Windows.Forms.TabPage();
            this.listBoxIk2 = new System.Windows.Forms.ListBox();
            this.groupBoxCondition = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFilterName = new System.Windows.Forms.TextBox();
            this.checkBoxInArchive = new System.Windows.Forms.CheckBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabControlCardType.SuspendLayout();
            this.tabPageIkl.SuspendLayout();
            this.tabPageIk2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlCardType
            // 
            this.tabControlCardType.Controls.Add(this.tabPageIkl);
            this.tabControlCardType.Controls.Add(this.tabPageIk2);
            this.tabControlCardType.Location = new System.Drawing.Point(12, 39);
            this.tabControlCardType.Name = "tabControlCardType";
            this.tabControlCardType.SelectedIndex = 0;
            this.tabControlCardType.Size = new System.Drawing.Size(230, 371);
            this.tabControlCardType.TabIndex = 1;
            this.tabControlCardType.SelectedIndexChanged += new System.EventHandler(this.tabControlCardType_SelectedIndexChanged);
            // 
            // tabPageIkl
            // 
            this.tabPageIkl.Controls.Add(this.listBoxIkl);
            this.tabPageIkl.Location = new System.Drawing.Point(4, 22);
            this.tabPageIkl.Name = "tabPageIkl";
            this.tabPageIkl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIkl.Size = new System.Drawing.Size(222, 345);
            this.tabPageIkl.TabIndex = 0;
            this.tabPageIkl.Text = "ИКЛ";
            this.tabPageIkl.UseVisualStyleBackColor = true;
            // 
            // listBoxIkl
            // 
            this.listBoxIkl.DisplayMember = "Caption";
            this.listBoxIkl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxIkl.FormattingEnabled = true;
            this.listBoxIkl.IntegralHeight = false;
            this.listBoxIkl.Location = new System.Drawing.Point(3, 3);
            this.listBoxIkl.Name = "listBoxIkl";
            this.listBoxIkl.Size = new System.Drawing.Size(216, 339);
            this.listBoxIkl.TabIndex = 0;
            this.listBoxIkl.SelectedIndexChanged += new System.EventHandler(this.Ikl_SelectedIndexChanged);
            // 
            // tabPageIk2
            // 
            this.tabPageIk2.Controls.Add(this.listBoxIk2);
            this.tabPageIk2.Location = new System.Drawing.Point(4, 22);
            this.tabPageIk2.Name = "tabPageIk2";
            this.tabPageIk2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIk2.Size = new System.Drawing.Size(222, 345);
            this.tabPageIk2.TabIndex = 1;
            this.tabPageIk2.Text = "ИК-2";
            this.tabPageIk2.UseVisualStyleBackColor = true;
            // 
            // listBoxIk2
            // 
            this.listBoxIk2.DisplayMember = "Caption";
            this.listBoxIk2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxIk2.FormattingEnabled = true;
            this.listBoxIk2.IntegralHeight = false;
            this.listBoxIk2.Location = new System.Drawing.Point(3, 3);
            this.listBoxIk2.Name = "listBoxIk2";
            this.listBoxIk2.Size = new System.Drawing.Size(216, 339);
            this.listBoxIk2.TabIndex = 0;
            this.listBoxIk2.SelectedIndexChanged += new System.EventHandler(this.Ik2_SelectedIndexChanged);
            // 
            // groupBoxCondition
            // 
            this.groupBoxCondition.Location = new System.Drawing.Point(248, 34);
            this.groupBoxCondition.Name = "groupBoxCondition";
            this.groupBoxCondition.Size = new System.Drawing.Size(239, 346);
            this.groupBoxCondition.TabIndex = 2;
            this.groupBoxCondition.TabStop = false;
            this.groupBoxCondition.Text = "Условия";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(330, 419);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Имя фильтра";
            // 
            // textBoxFilterName
            // 
            this.textBoxFilterName.Location = new System.Drawing.Point(100, 12);
            this.textBoxFilterName.Name = "textBoxFilterName";
            this.textBoxFilterName.Size = new System.Drawing.Size(387, 20);
            this.textBoxFilterName.TabIndex = 0;
            // 
            // checkBoxInArchive
            // 
            this.checkBoxInArchive.AutoSize = true;
            this.checkBoxInArchive.Location = new System.Drawing.Point(248, 389);
            this.checkBoxInArchive.Name = "checkBoxInArchive";
            this.checkBoxInArchive.Size = new System.Drawing.Size(56, 17);
            this.checkBoxInArchive.TabIndex = 5;
            this.checkBoxInArchive.Text = "Архив";
            this.checkBoxInArchive.UseVisualStyleBackColor = true;
            this.checkBoxInArchive.CheckedChanged += new System.EventHandler(this.checkBoxInArchive_CheckedChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(412, 419);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Закрыть";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormBuildFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 454);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.checkBoxInArchive);
            this.Controls.Add(this.textBoxFilterName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBoxCondition);
            this.Controls.Add(this.tabControlCardType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormBuildFilter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Фильтр";
            this.Load += new System.EventHandler(this.FormBulidFilter_Load);
            this.tabControlCardType.ResumeLayout(false);
            this.tabPageIkl.ResumeLayout(false);
            this.tabPageIk2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlCardType;
        private System.Windows.Forms.TabPage tabPageIkl;
        private System.Windows.Forms.TabPage tabPageIk2;
        private System.Windows.Forms.ListBox listBoxIkl;
        private System.Windows.Forms.ListBox listBoxIk2;
        private System.Windows.Forms.GroupBox groupBoxCondition;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFilterName;
        private System.Windows.Forms.CheckBox checkBoxInArchive;
        private System.Windows.Forms.Button buttonClose;

    }
}