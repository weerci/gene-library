namespace GeneLibrary.MdiForms
{
    partial class DictionaryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DictionaryForm));
            this.toolStripPanel1 = new System.Windows.Forms.ToolStripPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tstbQuickSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExcel = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonVerification = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dataGridVocabulary = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripPanel1
            // 
            this.toolStripPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripPanel1.Location = new System.Drawing.Point(0, 0);
            this.toolStripPanel1.Name = "toolStripPanel1";
            this.toolStripPanel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.toolStripPanel1.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripPanel1.Size = new System.Drawing.Size(510, 0);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.tstbQuickSearch,
            this.toolStripSeparator,
            this.newToolStripButton,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonExcel,
            this.tsbRefresh,
            this.toolStripButtonVerification,
            this.toolStripSeparator1,
            this.helpToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(510, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.TabStop = true;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(93, 22);
            this.toolStripLabel1.Text = "Быстрый поиск";
            // 
            // tstbQuickSearch
            // 
            this.tstbQuickSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tstbQuickSearch.Name = "tstbQuickSearch";
            this.tstbQuickSearch.Size = new System.Drawing.Size(200, 25);
            this.tstbQuickSearch.TextChanged += new System.EventHandler(this.tstbQuickSearch_TextChanged);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = global::GeneLibrary.Properties.Resources.New_Doc;
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New";
            this.newToolStripButton.ToolTipText = "Создать (Ctrl+N)";
            this.newToolStripButton.Click += new System.EventHandler(this.OnNewDict);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Enabled = false;
            this.toolStripButtonEdit.Image = global::GeneLibrary.Properties.Resources.Write;
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEdit.Text = "&Open";
            this.toolStripButtonEdit.ToolTipText = "Редактировать (Ctrl+O)";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.OnOpenDict);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Enabled = false;
            this.toolStripButtonDelete.Image = global::GeneLibrary.Properties.Resources.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "C&ut";
            this.toolStripButtonDelete.ToolTipText = "Удалить (Del)";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.OnDeleteDict);
            // 
            // toolStripButtonExcel
            // 
            this.toolStripButtonExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExcel.Enabled = false;
            this.toolStripButtonExcel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExcel.Image")));
            this.toolStripButtonExcel.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonExcel.Name = "toolStripButtonExcel";
            this.toolStripButtonExcel.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonExcel.Text = "toolStripButtonExcel";
            this.toolStripButtonExcel.ToolTipText = "Экспорт в Excel";
            this.toolStripButtonExcel.Click += new System.EventHandler(this.OnExportToExcel);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = global::GeneLibrary.Properties.Resources.Refresh;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbRefresh.Text = "toolStripButton1";
            this.tsbRefresh.ToolTipText = "Обновить (Ctrl+R)";
            this.tsbRefresh.Click += new System.EventHandler(this.OnDictionaryRefresh);
            // 
            // toolStripButtonVerification
            // 
            this.toolStripButtonVerification.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonVerification.Image = global::GeneLibrary.Properties.Resources.Advanced_Options;
            this.toolStripButtonVerification.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonVerification.Name = "toolStripButtonVerification";
            this.toolStripButtonVerification.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonVerification.Text = "Выверка справочника";
            this.toolStripButtonVerification.Click += new System.EventHandler(this.toolStripButtonVerification_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            this.helpToolStripButton.ToolTipText = "Помощь";
            // 
            // dataGridVocabulary
            // 
            this.dataGridVocabulary.AllowUserToAddRows = false;
            this.dataGridVocabulary.AllowUserToDeleteRows = false;
            this.dataGridVocabulary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVocabulary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridVocabulary.Location = new System.Drawing.Point(0, 25);
            this.dataGridVocabulary.Name = "dataGridVocabulary";
            this.dataGridVocabulary.ReadOnly = true;
            this.dataGridVocabulary.RowHeadersWidth = 16;
            this.dataGridVocabulary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridVocabulary.ShowCellErrors = false;
            this.dataGridVocabulary.ShowCellToolTips = false;
            this.dataGridVocabulary.ShowEditingIcon = false;
            this.dataGridVocabulary.ShowRowErrors = false;
            this.dataGridVocabulary.Size = new System.Drawing.Size(510, 306);
            this.dataGridVocabulary.StandardTab = true;
            this.dataGridVocabulary.TabIndex = 3;
            this.dataGridVocabulary.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridVocabulary_CellClick);
            this.dataGridVocabulary.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgr_CellDoubleClick);
            this.dataGridVocabulary.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridVocabulary_CellMouseUp);
            this.dataGridVocabulary.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridVocabulary_KeyDown);
            this.dataGridVocabulary.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridVocabulary_MouseDown);
            this.dataGridVocabulary.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridVocabulary_MouseMove);
            this.dataGridVocabulary.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridVocabulary_MouseUp);
            // 
            // DictionaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 331);
            this.Controls.Add(this.dataGridVocabulary);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.toolStripPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DictionaryForm";
            this.ShowInTaskbar = false;
            this.Text = "Справочники";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DictForm_FormClosed);
            this.Load += new System.EventHandler(this.DictForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripPanel toolStripPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tstbQuickSearch;
        private System.Windows.Forms.ToolStripButton toolStripButtonExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridView dataGridVocabulary;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton toolStripButtonVerification;
    }
}