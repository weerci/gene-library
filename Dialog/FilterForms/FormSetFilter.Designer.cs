namespace GeneLibrary.MdiForms
{
    partial class FormSetFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetFilter));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Фильтры");
            this.toolStripApply = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonApply = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.treeViewFilters = new System.Windows.Forms.TreeView();
            this.splitContainerFilter = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxFilter = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemApply = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripApply.SuspendLayout();
            this.splitContainerFilter.Panel1.SuspendLayout();
            this.splitContainerFilter.Panel2.SuspendLayout();
            this.splitContainerFilter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStripFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripApply
            // 
            this.toolStripApply.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.toolStripApply.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddFilter,
            this.toolStripButtonEdit,
            this.toolStripButtonDeleteFilter,
            this.toolStripSeparator1,
            this.toolStripButtonApply,
            this.toolStripSeparator2,
            this.helpToolStripButton});
            this.toolStripApply.Location = new System.Drawing.Point(0, 0);
            this.toolStripApply.Name = "toolStripApply";
            this.toolStripApply.Size = new System.Drawing.Size(638, 25);
            this.toolStripApply.TabIndex = 1;
            this.toolStripApply.Text = "toolStrip1";
            // 
            // toolStripButtonAddFilter
            // 
            this.toolStripButtonAddFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddFilter.Image = global::GeneLibrary.Properties.Resources.Fonts_1;
            this.toolStripButtonAddFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddFilter.Name = "toolStripButtonAddFilter";
            this.toolStripButtonAddFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddFilter.Text = "Добавить фильтр";
            this.toolStripButtonAddFilter.Click += new System.EventHandler(this.toolStripButtonAddFilter_Click);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Enabled = false;
            this.toolStripButtonEdit.Image = global::GeneLibrary.Properties.Resources.Write;
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEdit.Text = "Редактировать";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
            // 
            // toolStripButtonDeleteFilter
            // 
            this.toolStripButtonDeleteFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteFilter.Enabled = false;
            this.toolStripButtonDeleteFilter.Image = global::GeneLibrary.Properties.Resources.Delete;
            this.toolStripButtonDeleteFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteFilter.Name = "toolStripButtonDeleteFilter";
            this.toolStripButtonDeleteFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDeleteFilter.Text = "Удалить фильтр";
            this.toolStripButtonDeleteFilter.Click += new System.EventHandler(this.toolStripButtonDeleteFilter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonApply
            // 
            this.toolStripButtonApply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonApply.Enabled = false;
            this.toolStripButtonApply.Image = global::GeneLibrary.Properties.Resources.Forward___Next;
            this.toolStripButtonApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonApply.Name = "toolStripButtonApply";
            this.toolStripButtonApply.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonApply.Text = "Применить";
            this.toolStripButtonApply.Click += new System.EventHandler(this.toolStripButtonApply_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            this.helpToolStripButton.Click += new System.EventHandler(this.helpToolStripButton_Click);
            // 
            // treeViewFilters
            // 
            this.treeViewFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFilters.HideSelection = false;
            this.treeViewFilters.Location = new System.Drawing.Point(0, 0);
            this.treeViewFilters.Name = "treeViewFilters";
            treeNode1.Name = "root";
            treeNode1.Text = "Фильтры";
            this.treeViewFilters.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeViewFilters.Size = new System.Drawing.Size(199, 459);
            this.treeViewFilters.TabIndex = 0;
            this.treeViewFilters.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilters_AfterSelect);
            this.treeViewFilters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewFilters_MouseDoubleClick);
            // 
            // splitContainerFilter
            // 
            this.splitContainerFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFilter.Location = new System.Drawing.Point(0, 25);
            this.splitContainerFilter.Name = "splitContainerFilter";
            // 
            // splitContainerFilter.Panel1
            // 
            this.splitContainerFilter.Panel1.Controls.Add(this.treeViewFilters);
            // 
            // splitContainerFilter.Panel2
            // 
            this.splitContainerFilter.Panel2.AutoScroll = true;
            this.splitContainerFilter.Panel2.Controls.Add(this.groupBox2);
            this.splitContainerFilter.Size = new System.Drawing.Size(638, 459);
            this.splitContainerFilter.SplitterDistance = 199;
            this.splitContainerFilter.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.richTextBoxFilter);
            this.groupBox2.Location = new System.Drawing.Point(2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(428, 453);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Условия фильтра";
            // 
            // richTextBoxFilter
            // 
            this.richTextBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxFilter.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxFilter.Name = "richTextBoxFilter";
            this.richTextBoxFilter.ReadOnly = true;
            this.richTextBoxFilter.Size = new System.Drawing.Size(422, 434);
            this.richTextBoxFilter.TabIndex = 10;
            this.richTextBoxFilter.TabStop = false;
            this.richTextBoxFilter.Text = "";
            // 
            // contextMenuStripFilter
            // 
            this.contextMenuStripFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemApply,
            this.toolStripSeparator3,
            this.toolStripMenuItemEdit,
            this.toolStripMenuItemDel});
            this.contextMenuStripFilter.Name = "contextMenuStripFilter";
            this.contextMenuStripFilter.Size = new System.Drawing.Size(155, 76);
            // 
            // toolStripMenuItemApply
            // 
            this.toolStripMenuItemApply.Image = global::GeneLibrary.Properties.Resources.Forward___Next;
            this.toolStripMenuItemApply.Name = "toolStripMenuItemApply";
            this.toolStripMenuItemApply.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItemApply.Text = "Применить";
            this.toolStripMenuItemApply.Click += new System.EventHandler(this.toolStripMenuItemApply_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(151, 6);
            // 
            // toolStripMenuItemEdit
            // 
            this.toolStripMenuItemEdit.Name = "toolStripMenuItemEdit";
            this.toolStripMenuItemEdit.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItemEdit.Text = "Редактировать";
            this.toolStripMenuItemEdit.Click += new System.EventHandler(this.toolStripMenuItemEdit_Click);
            // 
            // toolStripMenuItemDel
            // 
            this.toolStripMenuItemDel.Name = "toolStripMenuItemDel";
            this.toolStripMenuItemDel.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItemDel.Text = "Удалить";
            this.toolStripMenuItemDel.Click += new System.EventHandler(this.toolStripMenuItemDel_Click);
            // 
            // FormSetFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 484);
            this.Controls.Add(this.splitContainerFilter);
            this.Controls.Add(this.toolStripApply);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSetFilter";
            this.ShowInTaskbar = false;
            this.Text = "Выбрать карточки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSetFilter_FormClosing);
            this.Load += new System.EventHandler(this.FormSetFilter_Load);
            this.toolStripApply.ResumeLayout(false);
            this.toolStripApply.PerformLayout();
            this.splitContainerFilter.Panel1.ResumeLayout(false);
            this.splitContainerFilter.Panel2.ResumeLayout(false);
            this.splitContainerFilter.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.contextMenuStripFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripApply;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonApply;
        private System.Windows.Forms.TreeView treeViewFilters;
        private System.Windows.Forms.SplitContainer splitContainerFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddFilter;
        private System.Windows.Forms.RichTextBox richTextBoxFilter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFilter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemApply;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDel;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;

    }
}