using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.MdiForms;
using WFExceptions;
using WFDatabase;
using GeneLibrary.Dialog;
using VS2005ToolBox;
using GeneLibrary.Common;
using GeneLibrary.Items;
using System.IO;

[assembly: CLSCompliant(true)]
namespace GeneLibrary
{
    public partial class Main : Form
    {
        // Constructors
        public Main()
        {
            InitializeComponent();
        }
        
        // Methods
        public void CloseCurrentMdi()
        {
            Form[] result = (from Form form in Application.OpenForms where form.Parent != null select form).ToArray<Form>();
            for (int i = result.Count()-1; i >= 0; i-- )
            {
                result[i].Close();
            }
        }
        public void RefreshMenu()
        {
            bool inConnect = GateFactory.DB().IsActive();
            tsmEdit.Enabled = inConnect;
            tsmDictionary.Enabled = inConnect;
            tsmCards.Enabled = inConnect;
            toolStripMenuItemCloseConnection.Enabled = inConnect;
            toolStripMenuItemChangePassword.Enabled = inConnect;
        }
        public void NewIklCard(Vocabulary vocabulary, int id)
        {
            IklForm iklForm = new IklForm(vocabulary, id);
            iklForm.onCloseForm += new FormInTree(CloseOpenForm);
            iklForm.onUpdateFormId += new FormInTree(UpdateTextOpenForm);
            AddOpenFormToTree(iklForm.FormId, iklForm.Text);
            OpenMdiForm(iklForm);
        }
        public void NewCardCopy(CardItem cardItem)
        {
            IklItem iklItem = cardItem as IklItem;
            CardForm cardForm;
            if (iklItem != null)
                cardForm = new IklForm(iklItem);
            else
                cardForm = new IK2Form((IK2Item)cardItem);

            cardForm.onCloseForm += new FormInTree(CloseOpenForm);
            cardForm.onUpdateFormId += new FormInTree(UpdateTextOpenForm);
            AddOpenFormToTree(cardForm.FormId, cardForm.Text);
            OpenMdiForm(cardForm);
        }
        public FormFilter NewFilterResultForm(string formNameId)
        {
            FormFilter formFilter = new FormFilter(formNameId);
            formFilter.OnCloseForm += new FormInTree(CloseOpenForm);
            AddOpenFormToTree(formFilter.FormId, formNameId);
            OpenMdiForm(formFilter);
            return formFilter;
        }

        public void NewIk2Card(Vocabulary vocabulary, int id)
        {
            IK2Form ik2Form = new IK2Form(vocabulary, id);
            ik2Form.onCloseForm += new FormInTree(CloseOpenForm);
            ik2Form.onUpdateFormId += new FormInTree(UpdateTextOpenForm);
            AddOpenFormToTree(ik2Form.FormId, ik2Form.Text);
            OpenMdiForm(ik2Form);
        }
        public void NewFindCard()
        {
            if (Tools.IsMdiFormOpen("FindCards"))
            {
                Application.OpenForms["FindCards"].Activate();
                return;
            } 
            FindCards findCards = new FindCards("Найти карточку", "tviCardFindOpen");
            findCards.OnActiveIklCard += new ActiveIklCard(NewIklCard);
            findCards.OnActiveIk2Card += new ActiveIk2Card(NewIk2Card);
            OpenMdiForm(findCards);
        }
        public void NewFilterCard()
        {
            if (Tools.IsMdiFormOpen("FormSetFilter"))
            {
                Application.OpenForms["FormSetFilter"].Activate();
                return;
            }

            FormSetFilter formFilter = new FormSetFilter();
            OpenMdiForm(formFilter);
        }
        public void NewStudyForm(int id)
        {
            if (Tools.IsMdiFormOpen("FormStudy"))
            {
                Application.OpenForms["FormStudy"].Activate();
                return;
            }

            FormStudy formStudy = new FormStudy();
            formStudy.OnCloseForm += new FormInTree(CloseOpenForm);
            AddOpenFormToTree(formStudy.FormId, formStudy.Text);
            OpenMdiForm(formStudy);
        }
        public void NewHistoryForm(int id)
        {
            if (Tools.IsMdiFormOpen("FormHistory"))
            {
                Application.OpenForms["FormHistory"].Activate();
                return;
            }

            FormHistory formHistory = new FormHistory(id);
            formHistory.OnCloseForm += new FormInTree(CloseOpenForm);
            AddOpenFormToTree(formHistory.FormId, formHistory.Text);
            OpenMdiForm(formHistory);
        }
        public void OpenMdiForm(Form form)
        {
            form.MdiParent = this;
            form.Show();
            form.Activate();
        }
        public void CloseOpenForm(ComboBoxItem comboBoxItem)
        {
            RemoveClosedFormFromTree(comboBoxItem.Name);
        }
        
        // Events
        private void Main_Load(object sender, EventArgs e)
        {
            // Назначение событий пунктам меню
            this.tsmClose.Click += new System.EventHandler(OnMainFormClose);
            this.tsmConnect.Click += new System.EventHandler(OnLoginFormShow);
            this.toolStripMenuItemChangePassword.Click += new EventHandler(OnChangePassword);
            this.toolStripMenuItemCloseConnection.Click +=new EventHandler(OnConnectionClose);
    
            // Вызов формы коннекта к базе данных
            LogOnForm loginForm = new LogOnForm();
            loginForm.StartPosition = FormStartPosition.CenterScreen;

            // Настройка контроллера mdi
            this._mdiClientController.ParentForm = this;
            
            if (loginForm.ShowDialog() == DialogResult.OK)
                FillMainForm();
            this.WindowState = FormWindowState.Maximized;
        }
        private void OnMainFormClose(object sender, EventArgs e)
        {
            this.Close();
        }
        private void OnLoginFormShow(object sender, EventArgs e)
        {
            LogOnForm loginForm = new LogOnForm();
            loginForm.ShowInTaskbar = false;
            if (loginForm.ShowDialog() == DialogResult.OK)
                FillMainForm();
        }
        private void ToolStripPreferences_Click(object sender, EventArgs e)
        {
            FormPreferences formPreferences = new FormPreferences();
            if (formPreferences.ShowDialog() == DialogResult.OK)
                //TODO Реализовать
                MessageBox.Show("test");
        }
        private void OnChangePassword(object sender, EventArgs e)
        {
            FormChangeUserPassword formChangeUserPassword = new FormChangeUserPassword();
            if (formChangeUserPassword.ShowDialog() == DialogResult.OK)
                GateFactory.LogOnBase().ChangePassword(formChangeUserPassword.Password);

        }
        private void OnConnectionClose(object sender, EventArgs e)
        {
            GateFactory.DB().Close();
            ClearMainForm();

        }
        private void tvMdi_DoubleClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tvMdi.BeginUpdate();
            try
            {
                TreeNode currentNode = ((TreeView)sender).SelectedNode;
                TreeNode currentParent = ((TreeView)sender).SelectedNode.Parent;

                #region Переключение фокуса ввода между карточками
                if ( currentParent != null && currentParent.Name == "tviListOpenCard")
                {
                    foreach (Form form in this.MdiChildren)
                    {
                        CardForm cardForm = form as CardForm;
                        if (cardForm != null)
                        {
                            if (cardForm.FormId == tvMdi.SelectedNode.Name)
                            {
                                cardForm.Activate();
                                return;
                            }
                        }
                    }               
                    return;
                }
                #endregion

                #region Некоторые формы могут должны быть открыты в единственном экземпляре
                foreach (Form form in this.MdiChildren)
                {

                    DictionaryForm dictForm = form as DictionaryForm;
                    if (dictForm != null && (dictForm.Text == tvMdi.SelectedNode.Text))
                    {
                        form.Activate();
                        return;
                    }

                }
                #endregion

                #region Создание формы
                OpenForm(currentNode);
                #endregion
            }
            finally
            {
                tvMdi.EndUpdate();
                this.Cursor = Cursors.Default;
            }
        }

        private void OpenForm(TreeNode currentNode)
        {
            Form newForm;
            switch (currentNode.Name)
            {
                case "tviIk2New":
                    NewIk2Card(new Ik2Vocabulary(), 0);
                    break;
                case "tviIklNew":
                    NewIklCard(new IklVocabulary(), 0);
                    break;
                case "tviMvd":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Mvd, tvMdi.SelectedNode.Text, "tviMvdOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviLin":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Lin, tvMdi.SelectedNode.Text, "tviLinOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviDiv":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Division, tvMdi.SelectedNode.Text, "tviMvdOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviPost":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Post, tvMdi.SelectedNode.Text, "tviPostOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviExp":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Exp, tvMdi.SelectedNode.Text, "tviExpOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviMeth":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Method, tvMdi.SelectedNode.Text, "tviMethOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviUK":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.UK, tvMdi.SelectedNode.Text, "tviUKOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviRole":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Role, tvMdi.SelectedNode.Text, "tviRoleOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviClassObject":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.ClassObject, tvMdi.SelectedNode.Text, "tviClassObjectOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviSort":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Sort, tvMdi.SelectedNode.Text, "tviSortOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviClassIkl":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.ClassIkl, tvMdi.SelectedNode.Text, "tviClassIklOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviOrg":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Organization, tvMdi.SelectedNode.Text, "tviOrgOpen");
                    OpenMdiForm(newForm);
                    break;
                case "tviCardFind":
                    NewFindCard();
                    break;
                case "tviStudy":
                    NewStudyForm(0);
                    break;
                case "tviFilter":
                    toolStripMenuItemSelect_Click(null, null);
                    break;
                case "tviLocus":
                    newForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Locus, tvMdi.SelectedNode.Text, "tviLocusOpen");
                    OpenMdiForm(newForm);
                    break;
                default:
                    break;
            }
        }
        private void toolStripMenuItemStudy_Click(object sender, EventArgs e)
        {
            NewStudyForm(0);
        }
        private void ToolStripMenuItemNewIkl_Click(object sender, EventArgs e)
        {
            NewIklCard(new IklVocabulary(), 0);
        }
        private void ToolStripMenuItemNewIk2_Click(object sender, EventArgs e)
        {
            NewIk2Card(new Ik2Vocabulary(), 0);
        }
        private void tsmFindAndEdit_Click(object sender, EventArgs e)
        {
            NewFindCard();
        }
        private void toolStripMenuItemSelect_Click(object sender, EventArgs e)
        {
            NewFilterCard();
        }
        private void toolStripMenuItemHistory_Click(object sender, EventArgs e)
        {
            NewHistoryForm(0);
        }
        private void toolStripMenuItemCodeMVD_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Коды МВД");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Mvd, "Коды МВД", "tviMvdOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemCodeLin_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Коды райлинорганов");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Lin, "Коды райлинорганов", "tviLinOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemDivizion_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Список подразделений");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Division, "Список подразделений", "tviDivOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemPost_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Должности");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Post, "Должности", "tviPostOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemExperts_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Эксперты");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Exp, "Эксперты", "tviPostOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemMethods_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Методы расчета");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Method, "Методы расчета", "tviMethOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void ToolStripMenuItemUK_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Статьи УК");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.UK, "Статьи УК", "tviUKOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemKindObject_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Виды объекта");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Sort, "Виды объекта", "tviSortOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemClassObject_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Категории объекта");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.ClassObject, "Категории объекта", "tviClassObjectOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemKindIkl_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Категории ИКЛ");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.ClassIkl, "Категории ИКЛ", "tviClassIklOpen");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemOrganization_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Организации");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Organization, "Организации", "tviOrg");
                OpenMdiForm(dictForm);
            }
        }
        private void toolStripMenuItemLocuses_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = ChekOpenDictionary("Локусы");
            if (dictForm != null)
            {
                dictForm.Activate();
                return;
            }
            else
            {
                dictForm = new DictionaryForm(GeneLibrary.Common.DictionaryKind.Locus, "Локусы", "tviLocus");
                OpenMdiForm(dictForm);
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutGeneLibrary().ShowDialog();
        }
        private void tsmHelp_Click(object sender, EventArgs e)
        {
            Common.Tools.GetHelp("", HelpNavigator.TableOfContents);
        }

        // Private methods
        private void FillMainForm()
        {
            // Закрываем все MDI-формы
            CloseCurrentMdi();
            LogOnBase logonBase = GateFactory.LogOnBase();

            // Выводим имя подключившегося пользователя
            sslUser.Text = Properties.Resources.ConnectedUser + " " + logonBase.LogOn;

            // Загрузка дерева
            TreeBuilderBase treeBuilder = WFDatabase.GateFactory.TreeBuilderBase();
            treeBuilder.CreateMainTree(this.tvMdi);

            // Загрузка меню в зависимости от разрешений пользователя
            bool menuCard = logonBase.HashSet.Any(n => n == 15);
            bool newIkl = logonBase.HashSet.Any(n => n == 3);
            bool newIk2 = logonBase.HashSet.Any(n => n == 4);
            bool newFind = logonBase.HashSet.Any(n => n == 2);
            bool newFilter = logonBase.HashSet.Any(n => n == 22);
            bool newStudy = logonBase.HashSet.Any(n => n == 23);
            bool newDictionary = logonBase.HashSet.Any(n => n == 23);
            bool newPreferences = true;

            tsmCards.Visible = menuCard;
            tsmDictionary.Visible = newDictionary;
            tsmFindAndEdit.Visible = newFind;
            toolStripMenuItemNewIkl.Visible = newIkl;
            toolStripMenuItemNewIk2.Visible = newIk2;
            toolStripMenuItemSelect.Visible = newFilter;
            toolStripMenuItemStudy.Visible = newStudy;
            
            #region Добавление справочников
            toolStripMenuItemCodeMVD.Visible = logonBase.HashSet.Any(n => n == 5);
            toolStripMenuItemCodeLin.Visible = logonBase.HashSet.Any(n => n == 6);
            toolStripMenuItemOrganization.Visible = logonBase.HashSet.Any(n => n == 7);
            toolStripMenuItemPost.Visible = logonBase.HashSet.Any(n => n == 8);
            toolStripMenuItemExperts.Visible = logonBase.HashSet.Any(n => n == 9);
            toolStripMenuItemMethods.Visible = logonBase.HashSet.Any(n => n == 10);
            toolStripMenuItemUK.Visible = logonBase.HashSet.Any(n => n == 11);
            toolStripMenuItemKindObject.Visible = logonBase.HashSet.Any(n => n == 19);
            toolStripMenuItemClassObject.Visible = logonBase.HashSet.Any(n => n == 20);
            toolStripMenuItemKindIkl.Visible = logonBase.HashSet.Any(n => n == 21);
            toolStripMenuItemOrgan.Visible = logonBase.HashSet.Any(n => n == 14);
            toolStripMenuItemLocuses.Visible = logonBase.HashSet.Any(n => n == 24); 
            #endregion

            // Отображение разделителей меню
            bool haveTopMenuItems = menuCard || newFind || newFilter;
            bool haveBottomMenuItem = newStudy || newDictionary || newPreferences;
            bool haveStudyItem = newStudy;

            tssFirst.Visible = haveTopMenuItems && haveBottomMenuItem;
            tssSecond.Visible = haveTopMenuItems && haveBottomMenuItem && haveStudyItem;
        }
        private void ClearMainForm()
        {
            // Закрываем все MDI-формы
            CloseCurrentMdi();
            
            // Удаляем имя подключившегося пользователя
            sslUser.Text = Properties.Resources.ConnectedUser;

            // Очищаем дерево
            tvMdi.Nodes.Clear();

            // Обновляем меню
            RefreshMenu();
        }
        private void AddOpenFormToTree(string formName, string formCaption) 
        {
            TreeNode[] openedNodes = tvMdi.Nodes.Find("tviListOpenCard", true);
            if (openedNodes.Count() > 0)
            {
                TreeNode newOpenedForm = new TreeNode(formCaption);
                newOpenedForm.Name = formName;
                openedNodes[0].Nodes.Add(newOpenedForm);
            }
        }
        private void UpdateTextOpenForm(ComboBoxItem comboBoxItem)
        {
            TreeNode[] openedNodes = tvMdi.Nodes.Find(comboBoxItem.Name, true);
            if (openedNodes.Count() > 0)
            {
                openedNodes[0].Text = comboBoxItem.Short;
            }
        }
        private void RemoveClosedFormFromTree(string formName)
        {
            TreeNode[] treeNodes = tvMdi.Nodes.Find(formName, true);
            if (treeNodes.Count() > 0)
                treeNodes[0].Remove();
        }
        private DictionaryForm ChekOpenDictionary(string dictionaryName)
        {
            foreach (Form form in this.MdiChildren)
            {
                DictionaryForm dictForm = form as DictionaryForm;
                if (dictForm != null && (dictForm.Text == dictionaryName))
                {
                    dictForm.Activate();
                    return dictForm;
                }
            }
            return null;
        }

        // Fields
        private Slusser.Components.MdiClientController _mdiClientController = new Slusser.Components.MdiClientController();

        private void tvMdi_KeyDown(object sender, KeyEventArgs e)
        {
            TreeNode current = tvMdi.SelectedNode;
            if (current != null && e.KeyCode == Keys.Enter)
                OpenForm(current);
        }

    }

    /// <summary>
    /// Типы mdi-форм
    /// </summary>
    public enum KindForm { None, Cards, Dictionary };
}
