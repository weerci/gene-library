using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using GeneLibrary.Items;
using System.Globalization;
using GeneLibrary.Common;
using WFExcel;
using System.Collections.ObjectModel;
using System.IO;
using GeneLibrary.Items.Research;
using GeneLibrary.Items.Find;
using WFExceptions;

namespace GeneLibrary.Dialog
{
    public partial class FormStudy : CardForm
    {
        // Constructors
        public FormStudy()
        {
            InitializeComponent();
            SetActiveTab(panelDirect);
        }

        #region Event handlers
        
        private void FormStudy_Load(object sender, EventArgs e)
        {
            EnabledControl();
            _methodVocabulary.Open();
            FillComboBox(new ComboBox[] {tscbBlendMethod.ComboBox, tscbStripChildOneParetnMethod.ComboBox, tscbStripMethod.ComboBox, tscbSuppParentMethod.ComboBox,
                tscbTooBlendMethods.ComboBox, tscbTwoParentMethod.ComboBox});
        }
        // Отрисовка контрола
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                DrawTabText(panel, e);
                if (activeTab.Name == panel.Name)
                {
                    e.Graphics.DrawLines(new Pen(Color.Black, 1), new Point[4] 
                    { 
                        new Point(panel.Width, 1), 
                        new Point(panel.Left + 1, 1),
                        new Point(panel.Left+1, panel.Height-1),
                        new Point(panel.Width, panel.Height-1)
                    });
                }
                else
                {
                    e.Graphics.DrawLines(new Pen(Color.Black, 1), new Point[4] 
                    { 
                        new Point(panel.Width, 1), 
                        new Point(panel.Left + 4, 1),
                        new Point(panel.Left + 4, panel.Height-1),
                        new Point(panel.Width, panel.Height-1)
                    });
                }
            }
        }
        private void panelContent_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                e.Graphics.DrawLines(new Pen(Color.Black, 1), new Point[6] 
                { 
                    new Point(panel.Left, activeTab.Top+1), 
                    new Point(panel.Left, 1), 
                    new Point(panel.Width - 2, 1),
                    new Point(panel.Width - 2, panel.Height - 2),
                    new Point(panel.Left, panel.Height - 2),
                    new Point(panel.Left, activeTab.Top + activeTab.Height-1)
                });
            }
        }
        private void panelTab_Click(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null && panel.Name != ActiveTab.Name)
                SetActiveTab(panel);
        }
        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainerStudy.Invalidate(true);
        }
        private void FormStudy_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (OnCloseForm != null)
                OnCloseForm(new ComboBoxItem(0, FormId));
        }
        
        // Обработка общих событий для всех вкладок
        private void checkTextBoxValueAsCardId(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            switch (textBox.Name)
            {
                // Обработка для формы исследования родства ребенка и одного известного родителя
                case "textBoxDirect":
                    checkOnNumberValue(textBox, labelDirectKin.Text);
                    EnabledControl();
                    break;
                case "textBoxChildKin":
                    checkOnNumberValue(textBox, labelChildForOneParent.Text);
                    EnabledControl();
                    break;
                case "textBoxKnownParent":
                    checkOnNumberValue(textBox, labelKnowParent.Text);
                    EnabledControl();
                    break;

                // Обработка для формы исследования родства ребенка, одного известного и одного предполагаемого родителя
                case "tbChildIdTwoParent":
                    checkOnNumberValue(textBox, label12.Text);
                    EnabledControl();
                    break;
                case "tbKnownParentTwoParent":
                    checkOnNumberValue(textBox, labelKnownParentTwoParent.Text);
                    EnabledControl();
                    break;
                case "tbSupposedParentTwoParent":
                    checkOnNumberValue(textBox, label11.Text);
                    EnabledControl();
                    break;

                // Обработка для формы исследования родства ребенка, для двух предполагаемых родителей
                case "tbChildSuppParent":
                    checkOnNumberValue(textBox, label16.Text);
                    EnabledControl();
                    break;
                case "tbFirstSuppParent":
                    checkOnNumberValue(textBox, label14.Text);
                    EnabledControl();
                    break;
                case "tbSecondSuppParent":
                    checkOnNumberValue(textBox, label15.Text);
                    EnabledControl();
                    break;

                // Обработка для формы исследования участия в смеси, предполагаемого лица
                case "tbBlend":
                    checkOnNumberValue(textBox, label20.Text);
                    EnabledControl();
                    break;
                case "tbPersonBlend":
                    checkOnNumberValue(textBox, label19.Text);
                    EnabledControl();
                    break;

                // Обработка для формы исследования участия в смеси одного известного и одного предполагаемого лица
                case "textBoxTwoBlendIk2":
                    checkOnNumberValue(textBox, label26.Text);
                    EnabledControl();
                    break;
                case "textBoxTwoBlendKnown":
                    checkOnNumberValue(textBox, label22.Text);
                    EnabledControl();
                    break;
                case "textBoxTwoBlendUnknown":
                    checkOnNumberValue(textBox, label24.Text);
                    EnabledControl();
                    break;

                default:
                    break;

            }
        }
        private void ToolStripButtonGo_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ResearchGridDecorator researchGridDecorator = new ResearchGridDecorator(new Research(getMetodIdFromCombobox()), ActiveGridViewDirect, ActiveGridViewRatio);
                switch (ActiveTab.Name)
                {
                    case "panelDirect":
                        researchGridDecorator.DirectIdent(textBoxDirect.Text, labelDirectKin.Text);
                        break;
                    case "panelChildOneParent":
                        researchGridDecorator.OneChildAndParent(textBoxChildKin.Text, textBoxKnownParent.Text, labelChildForOneParent.Text, labelKnowParent.Text);
                        break;
                    case "panelChildTwoParent":
                        researchGridDecorator.TwoParent(
                            tbChildIdTwoParent.Text, tbKnownParentTwoParent.Text, tbSupposedParentTwoParent.Text,
                            label12.Text, labelKnownParentTwoParent.Text, label11.Text 
                            );
                        break;
                    case "panelChildTwoUnknownParent":
                        researchGridDecorator.SuppParent(
                            tbChildSuppParent.Text, tbFirstSuppParent.Text, tbSecondSuppParent.Text,
                            label16.Text, label14.Text, label15.Text
                            );
                        break;
                    case "panelBlend":
                        researchGridDecorator.Blend(
                            tbBlend.Text, tbPersonBlend.Text, 
                            label30.Text, label19.Text
                            );
                        break;
                    case "panelTwoBlend":
                        researchGridDecorator.TwoBlend(
                            textBoxTwoBlendIk2.Text, textBoxTwoBlendKnown.Text, textBoxTwoBlendUnknown.Text,
                            label26.Text, label22.Text, label24.Text
                            );
                        break;
                    default:
                        break;
                }
                EnabledControl();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                switch (ActiveTab.Name)
                {
                    case "panelDirect":
                        if (tabControlDirect.SelectedIndex == 0)
                            Tools.ToExcel(ResourceStudy.reportDirectProbably, new System.Drawing.Point(3, 1), ActiveGridViewDirect, ExcelCellOrientation.Vertical);
                        else
                            Tools.ToExcel(ResourceStudy.reportDirectRatio, new System.Drawing.Point(3, 1), ActiveGridViewRatio, ExcelCellOrientation.Vertical);
                        break;
                    case "panelChildOneParent":
                        if (tabControlChildOneParent.SelectedIndex == 0)
                            Tools.ToExcel(ResourceStudy.reportDirectOneParent, new System.Drawing.Point(3, 1), ActiveGridViewDirect, ExcelCellOrientation.Vertical);
                        else
                            Tools.ToExcel(ResourceStudy.reportRatioOneParent, new System.Drawing.Point(3, 1), ActiveGridViewRatio, ExcelCellOrientation.Vertical);
                        break;
                    case "panelChildTwoParent":
                        if (tcTwoParent.SelectedIndex == 0)
                            Tools.ToExcel(ResourceStudy.reportTwoParent, new System.Drawing.Point(3, 1), ActiveGridViewDirect, ExcelCellOrientation.Vertical);
                        else
                            Tools.ToExcel(ResourceStudy.reportTwoParentRatio, new System.Drawing.Point(3, 1), ActiveGridViewRatio, ExcelCellOrientation.Vertical);
                        break;
                    case "panelChildTwoUnknownParent":
                        if (tcSuppParent.SelectedIndex == 0)
                            Tools.ToExcel(ResourceStudy.reportSuppParent, new System.Drawing.Point(3, 1), ActiveGridViewDirect, ExcelCellOrientation.Vertical);
                        else
                            Tools.ToExcel(ResourceStudy.reportSuppParentRatio, new System.Drawing.Point(3, 1), ActiveGridViewRatio, ExcelCellOrientation.Vertical);
                        break;
                    case "panelBlend":
                        if (tcBlend.SelectedIndex == 0)
                            Tools.ToExcel(ResourceStudy.reportBlend, new System.Drawing.Point(3, 1), ActiveGridViewDirect, ExcelCellOrientation.Vertical);
                        else
                            Tools.ToExcel(ResourceStudy.reportBlendRatio, new System.Drawing.Point(3, 1), ActiveGridViewRatio, ExcelCellOrientation.Vertical);
                        break;
                    case "panelTwoBlend":
                        if (tcTwoBlend.SelectedIndex == 0)
                            Tools.ToExcel(ResourceStudy.reportTwoBlend, new System.Drawing.Point(3, 1), ActiveGridViewDirect, ExcelCellOrientation.Vertical);
                        else
                            Tools.ToExcel(ResourceStudy.reportTwoBlendRatio, new System.Drawing.Point(3, 1), ActiveGridViewRatio, ExcelCellOrientation.Vertical);
                        break;
                    default:
                        break;
                }
                
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        private void OpenFindCard(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView.SelectedRows.Count > 0)
            {
                switch (Convert.ToInt32(dataGridView.CurrentRow.Cells["card_type"].Value, CultureInfo.InvariantCulture))
                {
                    case 0:
                        Tools.MainForm().NewIklCard(new IklVocabulary(),
                            Convert.ToInt32(dataGridView.CurrentRow.Cells["profile_id"].Value, CultureInfo.InvariantCulture));
                        break;
                    case 1:
                        Tools.MainForm().NewIk2Card(new Ik2Vocabulary(),
                            Convert.ToInt32(dataGridView.CurrentRow.Cells["profile_id"].Value,
                            CultureInfo.InvariantCulture));
                        break;
                }
            }
        }
        private void buttonFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                FindGridDecorator findGridDecorator = new FindGridDecorator(new FindCoincidence(), ActiveGridViewFind);
                switch (activeTab.Name)
                {
                    case "panelDirect":
                        #region panelDirect
                        listViewProfilesDirect.Items.Clear();
                        findGridDecorator.FoundCondition.CardsString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { labelDirectKin.Text },
                            FieldValue = new string[] { textBoxDirect.Text }
                        };
                        findGridDecorator.FoundCondition.CountCoincidenceString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label1.Text },
                            FieldValue = new string[] { numericUpDown.Value.ToString() }
                        };
                        findGridDecorator.FoundCondition.CountLocusString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label4.Text },
                            FieldValue = new string[] { numericUpDownLocusCount.Value.ToString() }
                        };
                        findGridDecorator.FindOnMinimum();
                        #endregion
                        break;
                    case "panelChildOneParent":
                        #region ChildAndOneParent
                        listViewChildOneParent.Items.Clear();
                        findGridDecorator.FoundCondition.CardsString = new FindCondition.FindField()
                            {
                                FieldName = new string[] { labelChildForOneParent.Text, labelKnowParent.Text },
                                FieldValue = new string[] { textBoxChildKin.Text, textBoxKnownParent.Text }
                            };
                        findGridDecorator.FoundCondition.CountLocusString = new FindCondition.FindField()
                            {
                                FieldName = new string[] { label2.Text },
                                FieldValue = new string[] { numericUpDownChildOneParent.Value.ToString() }
                            };
                        findGridDecorator.FindOneParent(); 
                        #endregion
                        break;
                    case "panelChildTwoParent":
                        #region ChildAndOneParent
                        lvTwoParent.Items.Clear();
                        findGridDecorator.FoundCondition.CardsString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label12.Text, label11.Text },
                            FieldValue = new string[] { tbChildIdTwoParent.Text, tbKnownParentTwoParent.Text }
                        };
                        findGridDecorator.FoundCondition.CountLocusString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label13.Text },
                            FieldValue = new string[] { numericUpDownTwoParetns.Value.ToString() }
                        };
                        findGridDecorator.FindOneParent();
                        #endregion
                        break;
                    case "panelChildTwoUnknownParent":
                        #region ChildSuppParent
                        lvSuppParent.Items.Clear();
                        findGridDecorator.FoundCondition.CardsString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label14.Text, label15.Text },
                            FieldValue = new string[] { tbFirstSuppParent.Text, tbSecondSuppParent.Text }
                        };
                        findGridDecorator.FoundCondition.CountLocusString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label13.Text },
                            FieldValue = new string[] { numericUpDownTwoParetns.Value.ToString() }
                        };
                        findGridDecorator.FindChildByParents();
                        #endregion
                        break;
                    case "panelBlend":
                        #region panelDirect
                        lvBlend.Items.Clear();
                        findGridDecorator.FoundCondition.CardsString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label20.Text },
                            FieldValue = new string[] { tbBlend.Text }
                        };
                        findGridDecorator.FoundCondition.CountCoincidenceString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label27.Text },
                            FieldValue = new string[] { nudErrorCountBlend.Value.ToString() }
                        };
                        findGridDecorator.FoundCondition.CountLocusString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label21.Text },
                            FieldValue = new string[] { nudLocusCountBlend.Value.ToString() }
                        };
                        findGridDecorator.FindIklForBlend();
                        #endregion
                        break;
                    case "panelTwoBlend":
                        #region panelTwoBlend
                        lvTwoBlend.Items.Clear();
                        findGridDecorator.FoundCondition.CardsString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label22.Text, label24.Text },
                            FieldValue = new string[] { textBoxTwoBlendKnown.Text, textBoxTwoBlendUnknown.Text }
                        };
                        findGridDecorator.FoundCondition.CountCoincidenceString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label39.Text },
                            FieldValue = new string[] { nudTwoBlendError.Value.ToString() }
                        };
                        findGridDecorator.FoundCondition.CountLocusString = new FindCondition.FindField()
                        {
                            FieldName = new string[] { label34.Text },
                            FieldValue = new string[] { nudTwoBlendLocusCount.Value.ToString() }
                        };
                        findGridDecorator.FindTwoIklForBlend(); 
                        #endregion
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void buttonShowCard_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int cardId;
                Button button = sender as Button;
                if (button != null)
                {
                    switch (activeTab.Name)
                    {
                        #region panelDirect
                        case "panelDirect":
                            cardId = Tools.GetIntFromText(textBoxDirect.Text, labelDirectKin.Text);
                            switch (CardItem.CardKind(cardId))
                            {
                                case CardKind.ikl:
                                    Tools.MainForm().NewIklCard(new IklVocabulary(), cardId);
                                    break;
                                case CardKind.ik2:
                                    Tools.MainForm().NewIk2Card(new Ik2Vocabulary(), cardId);
                                    break;
                            }
                            break; 
                        #endregion
                        #region panelChildOneParent
                        case "panelChildOneParent":
                            if (button.Name == "btnChildIdTwoParent")
                                cardId = Tools.GetIntFromText(textBoxChildKin.Text, labelChildForOneParent.Text);
                            else
                                cardId = Tools.GetIntFromText(textBoxKnownParent.Text, labelKnowParent.Text);
                            switch (CardItem.CardKind(cardId))
                            {
                                case CardKind.ikl:
                                    Tools.MainForm().NewIklCard(new IklVocabulary(), cardId);
                                    break;
                                case CardKind.ik2:
                                    Tools.MainForm().NewIk2Card(new Ik2Vocabulary(), cardId);
                                    break;
                            }
                            break; 
                        #endregion
                        #region panelChildTwoParent
                        case "panelChildTwoParent":
                            switch (button.Name)
                            {
                                case "btnChildCardIdTwoParent":
                                    cardId = Tools.GetIntFromText(tbChildIdTwoParent.Text, label12.Text);
                                    break;
                                case "btnKnownParentTwoParent":
                                    cardId = Tools.GetIntFromText(tbKnownParentTwoParent.Text, labelKnownParentTwoParent.Text);
                                    break;
                                case "btnUnknownParentIdTwoParent":
                                    cardId = Tools.GetIntFromText(tbSupposedParentTwoParent.Text, label11.Text);
                                    break;
                                default:
                                    throw new WFException(ErrType.Critical, ErrorsMsg.UnknownError);
                            }
                            switch (CardItem.CardKind(cardId))
                            {
                                case CardKind.ikl:
                                    Tools.MainForm().NewIklCard(new IklVocabulary(), cardId);
                                    break;
                                case CardKind.ik2:
                                    Tools.MainForm().NewIk2Card(new Ik2Vocabulary(), cardId);
                                    break;
                            }
                            break; 
                        #endregion
                        #region panelChildTwoUnknownParent
                        case "panelChildTwoUnknownParent":
                            switch (button.Name)
                            {
                                case "btnChildSuppParentCard":
                                    cardId = Tools.GetIntFromText(tbChildSuppParent.Text, label16.Text);
                                    break;
                                case "btnFirstSuppParentCard":
                                    cardId = Tools.GetIntFromText(tbFirstSuppParent.Text, label14.Text);
                                    break;
                                case "btnSecondSuppParentCard":
                                    cardId = Tools.GetIntFromText(tbSecondSuppParent.Text, label15.Text);
                                    break;
                                default:
                                    throw new WFException(ErrType.Critical, ErrorsMsg.UnknownError);
                            }
                            switch (CardItem.CardKind(cardId))
                            {
                                case CardKind.ikl:
                                    Tools.MainForm().NewIklCard(new IklVocabulary(), cardId);
                                    break;
                                case CardKind.ik2:
                                    Tools.MainForm().NewIk2Card(new Ik2Vocabulary(), cardId);
                                    break;
                            }
                            break; 
                        #endregion
                        #region panelBlend
                        case "panelBlend":
                            switch (button.Name)
                            {
                                case "btnCardBlend":
                                    cardId = Tools.GetIntFromText(tbBlend.Text, label20.Text);
                                    break;
                                case "btnPersonCardBlend":
                                    cardId = Tools.GetIntFromText(tbPersonBlend.Text, label19.Text);
                                    break;
                                default:
                                    throw new WFException(ErrType.Critical, ErrorsMsg.UnknownError);
                            }
                            switch (CardItem.CardKind(cardId))
                            {
                                case CardKind.ikl:
                                    Tools.MainForm().NewIklCard(new IklVocabulary(), cardId);
                                    break;
                                case CardKind.ik2:
                                    Tools.MainForm().NewIk2Card(new Ik2Vocabulary(), cardId);
                                    break;
                            }
                            break; 
                        #endregion
                        #region panelTwoBlend
                        case "panelTwoBlend":
                            switch (button.Name)
                            {
                                case "btnTwoBlendIk2":
                                    cardId = Tools.GetIntFromText(textBoxTwoBlendIk2.Text, label26.Text);
                                    break;
                                case "btnTwoBlendKnown":
                                    cardId = Tools.GetIntFromText(textBoxTwoBlendKnown.Text, label22.Text);
                                    break;
                                case "btnTwoBlendUnknown":
                                    cardId = Tools.GetIntFromText(textBoxTwoBlendUnknown.Text, label24.Text);
                                    break;
                                default:
                                    throw new WFException(ErrType.Critical, ErrorsMsg.UnknownError);
                            }
                            switch (CardItem.CardKind(cardId))
                            {
                                case CardKind.ikl:
                                    Tools.MainForm().NewIklCard(new IklVocabulary(), cardId);
                                    break;
                                case CardKind.ik2:
                                    Tools.MainForm().NewIk2Card(new Ik2Vocabulary(), cardId);
                                    break;
                            }
                            break;
                        default:
                            break; 
                        #endregion
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        
        // Вызов справки
        private void toolStripHelp_Click(object sender, EventArgs e)
        {
            ToolStripButton tsb = sender as ToolStripButton;
            if (tsb != null)
            {
                switch (tsb.Name)
                {
                    case "tsbDirectHelp":
                        Common.Tools.GetHelp("mf_study_method1", HelpNavigator.Topic);
                        break;
                    case "tsbChildOneParentHelp":
                        Common.Tools.GetHelp("mf_study_method2", HelpNavigator.Topic);
                        break;
                    case "tsbChildTwoParent":
                        Common.Tools.GetHelp("mf_study_method3", HelpNavigator.Topic);
                        break;
                    case "tsbTwoUnknownParent":
                        Common.Tools.GetHelp("mf_study_method4", HelpNavigator.Topic);
                        break;
                    case "tsbBlendHelp":
                        Common.Tools.GetHelp("sf_study_mehod5", HelpNavigator.Topic);
                        break;
                    case "tsbTwoBlendHelp":
                        Common.Tools.GetHelp("sf_study_method6", HelpNavigator.Topic);
                        break;

                    default:
                        return;
                }
            }
        }

        // События формы для Direct
        private void tabControlDirectFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1 && dataGridViewDirectFind.SelectedRows.Count > 0)
                {
                    listViewProfilesDirect.BeginUpdate();
                    Tools.FillListViewProfilesData(
                        Tools.GetProfileById(Tools.GetIntFromText(textBoxDirect.Text, labelDirectKin.Text), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Convert.ToInt32(dataGridViewDirectFind.SelectedRows[0].Cells["profile_id"].Value,
                            CultureInfo.InstalledUICulture), getMetodIdFromCombobox()),
                        listViewProfilesDirect
                        );
                    listViewProfilesDirect.EndUpdate();
                    label9.Text = String.Format("{0} {1}", ResourceStudy.profileKin, textBoxDirect.Text);
                    label10.Text = String.Format("{0} {1}", ResourceStudy.profileKin, dataGridViewDirectFind.SelectedRows[0].Cells["profile_id"].Value);
                }
            }
        }
        
        // События для формы ребенка и одного известного родителя
        private void tabControlChildOneKnownParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1 && dataGridViewChildOneParentFound.SelectedRows.Count > 0)
                {
                    listViewChildOneParent.BeginUpdate();
                    int shiftSecondProfileName;
                    int shiftThridProfileName;

                    Tools.FillListViewTreeProfiles(
                        Tools.GetProfileById(Tools.GetIntFromText(textBoxChildKin.Text, labelChildForOneParent.Text), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Tools.GetIntFromText(textBoxKnownParent.Text, labelKnowParent.Text), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Convert.ToInt32(dataGridViewChildOneParentFound.SelectedRows[0].Cells["profile_id"].Value, CultureInfo.InstalledUICulture)
                        , getMetodIdFromCombobox()),
                        listViewChildOneParent, out shiftSecondProfileName, out shiftThridProfileName);
                    
                    label7.Left = shiftSecondProfileName;
                    label7.Text = String.Format(ResourceStudy.parentCaption, textBoxKnownParent.Text);
                    label8.Left = shiftThridProfileName;
                    label8.Text = String.Format(ResourceStudy.parentCaption, dataGridViewChildOneParentFound.SelectedRows[0].Cells["profile_id"].Value.ToString());

                    listViewChildOneParent.EndUpdate();
                }
            }
        }

        // События для формы исследований одного известного и одного предполагаемого родителя
        private void tcTwoParentFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1 && dgvDirectFindTwoParent.SelectedRows.Count > 0)
                {
                    lvTwoParent.BeginUpdate();
                    int shiftSecondProfileName;
                    int shiftThridProfileName;

                    Tools.FillListViewTreeProfiles(
                        Tools.GetProfileById(Tools.GetIntFromText(tbChildIdTwoParent.Text, label12.Text), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Tools.GetIntFromText(tbKnownParentTwoParent.Text, labelKnownParentTwoParent.Text), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Convert.ToInt32(dgvDirectFindTwoParent.SelectedRows[0].Cells["profile_id"].Value, CultureInfo.InstalledUICulture), getMetodIdFromCombobox()),
                        lvTwoParent, out shiftSecondProfileName, out shiftThridProfileName);

                    lbKnownParent.Left = shiftSecondProfileName;
                    lbKnownParent.Text = String.Format(ResourceStudy.parentCaption, tbKnownParentTwoParent.Text);
                    lbUnkonowParent.Left = shiftThridProfileName;
                    lbUnkonowParent.Text = String.Format(ResourceStudy.parentCaption, dgvDirectFindTwoParent.SelectedRows[0].Cells["profile_id"].Value.ToString());

                    lvTwoParent.EndUpdate();
                }
            }
        }

        // События для формы исследований двух предполагаемых родителей
        private void tcSuppParentFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1 && dgvSuppParentFind.SelectedRows.Count > 0)
                {
                    lvSuppParent.BeginUpdate();
                    int shiftSecondProfileName;
                    int shiftThridProfileName;

                    Tools.FillListViewTreeProfiles(
                        Tools.GetProfileById(Convert.ToInt32(dgvSuppParentFind.SelectedRows[0].Cells["profile_id"].Value, CultureInfo.InstalledUICulture), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Tools.GetIntFromText(tbFirstSuppParent.Text, label16.Text), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Tools.GetIntFromText(tbSecondSuppParent.Text, label15.Text), getMetodIdFromCombobox()),
                        lvSuppParent, out shiftSecondProfileName, out shiftThridProfileName);

                    label31.Text = String.Format(ResourceStudy.child, dgvSuppParentFind.SelectedRows[0].Cells["profile_id"].Value.ToString());
                    label30.Left = shiftSecondProfileName;
                    label30.Text = ResourceStudy.parentOne;
                    label29.Left = shiftThridProfileName;
                    label29.Text = ResourceStudy.parentSecond;

                    lvSuppParent.EndUpdate();
                }
            }
        }

        // События для формы исследования участия в смеси предполагаемого лица
        private void tcFindBlend_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1 && dgvFindBlend.SelectedRows.Count > 0)
                {
                    lvBlend.BeginUpdate();
                    Tools.FillListViewProfilesData(
                        Tools.GetProfileById(Convert.ToInt32(dgvFindBlend.SelectedRows[0].Cells["profile_id"].Value, CultureInfo.InstalledUICulture), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Tools.GetIntFromText(tbBlend.Text, label20.Text), getMetodIdFromCombobox()),
                        lvBlend
                        );
                    lvBlend.EndUpdate();
                    label25.Text = String.Format("{0} {1}", ResourceStudy.profileKin, tbBlend.Text);
                    label23.Text = String.Format("{0} {1}", ResourceStudy.profileKin, dgvFindBlend.SelectedRows[0].Cells["profile_id"].Value.ToString());
                }
            }
        }

        // События для формы исследования участия в смеси одного прдеполагаемого и одного известного лица
        private void tcTwoBlendFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (tabControl.SelectedIndex == 1 && dgvTwoBlendFind.SelectedRows.Count > 0)
                {
                    lvTwoBlend.BeginUpdate();

                    int shiftSecondProfileName;
                    int shiftThridProfileName;

                    Tools.FillListViewTreeProfiles(
                        Tools.GetProfileById(Convert.ToInt32(dgvTwoBlendFind.SelectedRows[0].Cells["profile_id"].Value, CultureInfo.InstalledUICulture), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Tools.GetIntFromText(textBoxTwoBlendKnown.Text, label22.Text), getMetodIdFromCombobox()),
                        Tools.GetProfileById(Tools.GetIntFromText(textBoxTwoBlendUnknown.Text, label24.Text), getMetodIdFromCombobox()),
                        lvTwoBlend, out shiftSecondProfileName, out shiftThridProfileName);
                    lvTwoBlend.EndUpdate();

                    label37.Text = String.Format(ResourceStudy.CardIk2, dgvTwoBlendFind.SelectedRows[0].Cells["profile_id"].Value.ToString());
                    label36.Left = shiftSecondProfileName;
                    label36.Text = String.Format(ResourceStudy.KnownPerson, textBoxTwoBlendKnown.Text);
                    label35.Left = shiftThridProfileName;
                    label35.Text = String.Format(ResourceStudy.UnknownPerson, textBoxTwoBlendUnknown.Text);
                }
            }
        }

        #endregion

        public Panel ActiveContent
        {
            get { return this.activeContent; }
            set 
            {
                foreach (Panel loopPanel in new Panel[6]{
                        panelContentBlend, panelContentTwoBlend, panelContentChildTwoUnknownParent, 
                        panelContentChildTwoParent, panelContentChildOneParent, panelContentDirect
                        })
                {
                    loopPanel.Visible = false;
                }    
                activeContent = value;
                activeContent.Visible = true;
            }
        }
        public Panel ActiveTab
        {
            get { return this.activeTab; }
            set 
            {
                foreach (Panel loopPanel in new Panel[6]{
                        panelDirect, panelChildOneParent, panelChildTwoParent,
                        panelChildTwoUnknownParent, panelBlend, panelTwoBlend
                        })
                {
                    loopPanel.BackColor = SystemColors.Control;
                }    
                activeTab = value;
                activeTab.BackColor = SystemColors.Window;
                panelLeft.Invalidate(true);
            }
        }
        public DataGridView ActiveGridViewDirect { get; set; }
        public DataGridView ActiveGridViewRatio { get; set; }
        public DataGridView ActiveGridViewFind { get; set; }

        // Events
        new internal event FormInTree OnCloseForm;

        // Fields
        private Panel activeContent;
        private Panel activeTab;
        private MethodVocabulary _methodVocabulary = new MethodVocabulary();

        // Private methods
        private void DrawTabText(Panel panel, PaintEventArgs e)
        {
            Font font = new Font(FontFamily.GenericSansSerif, 8);
            StringFormat stringFlags = new StringFormat();
            stringFlags.Alignment = StringAlignment.Center;
            stringFlags.LineAlignment = StringAlignment.Center;
        
            switch (panel.Name)
            {
                case "panelDirect":
                    e.Graphics.DrawString(ResourceStudy.captionDirection, font, Brushes.Black,
                        new Rectangle(new Point(0, 0), panelDirect.Size), 
                        new StringFormat(stringFlags));
                    break;
                case "panelChildOneParent":
                    e.Graphics.DrawString(ResourceStudy.panelChildOneParent, font, Brushes.Black,
                        new Rectangle(new Point(0,0), panelChildOneParent.Size), 
                        new StringFormat(stringFlags));
                    break;
                case "panelChildTwoParent":
                    e.Graphics.DrawString(ResourceStudy.captionOneParent, font, Brushes.Black,
                        new Rectangle(new Point(0, 0), panelChildTwoParent.Size), 
                        new StringFormat(stringFlags));
                    break;
                case "panelChildTwoUnknownParent":
                    e.Graphics.DrawString(ResourceStudy.captionTwoParent, font, Brushes.Black,
                        new Rectangle(new Point(0, 0), panelChildTwoUnknownParent.Size),
                        new StringFormat(stringFlags));
                    break;
                case "panelBlend":
                    e.Graphics.DrawString(ResourceStudy.captionBlend, font, Brushes.Black,
                        new Rectangle(new Point(0, 0), panelBlend.Size),
                        new StringFormat(stringFlags));
                    break;
                case "panelTwoBlend":
                    e.Graphics.DrawString(ResourceStudy.captionTwoBlend, font, Brushes.Black,
                        new Rectangle(new Point(0, 0), panelBlend.Size),
                        new StringFormat(stringFlags));
                    break;
                default:
                    break;
            }
        }
        private void SetActiveTab(Panel activeTab)
        {
            switch (activeTab.Name)
            {
                case "panelDirect":
                    ActiveTab = panelDirect;
                    ActiveContent = panelContentDirect;
                    ActiveGridViewDirect = dataGridViewDirect;
                    ActiveGridViewRatio = dataGridViewDirectRatio;
                    ActiveGridViewFind = dataGridViewDirectFind;
                    EnabledControl();
                    break;
                case "panelChildOneParent":
                    ActiveTab = panelChildOneParent;
                    ActiveContent = panelContentChildOneParent;
                    ActiveGridViewDirect = dataGridViewChildOneParentDirect;
                    ActiveGridViewRatio = dataGridViewChildOneParentRatio;
                    ActiveGridViewFind = dataGridViewChildOneParentFound;
                    EnabledControl();
                    break;
                case "panelChildTwoParent":
                    ActiveTab = panelChildTwoParent;
                    ActiveContent = panelContentChildTwoParent;
                    ActiveGridViewDirect = dgwDirectTwoParent;
                    ActiveGridViewRatio = dgwRatioTwoParent;
                    ActiveGridViewFind = dgvDirectFindTwoParent;
                    EnabledControl();
                    break;
                case "panelChildTwoUnknownParent":
                    ActiveTab = panelChildTwoUnknownParent;
                    ActiveContent = panelContentChildTwoUnknownParent;
                    ActiveGridViewDirect = dgvDirectSuppParent;
                    ActiveGridViewRatio = dgvRatioSuppParent;
                    ActiveGridViewFind = dgvSuppParentFind;
                    EnabledControl();
                    break;
                case "panelBlend":
                    ActiveTab = panelBlend;
                    ActiveContent = panelContentBlend;
                    ActiveGridViewDirect = dgvBlendDirect;
                    ActiveGridViewRatio = dgvBlendRatio;
                    ActiveGridViewFind = dgvFindBlend;
                    EnabledControl();
                    break;
                case "panelTwoBlend":
                    ActiveTab = panelTwoBlend;
                    ActiveContent = panelContentTwoBlend;
                    ActiveGridViewDirect = dgvTwoBlendDirect;
                    ActiveGridViewRatio = dgvTwoBlendRatio;
                    ActiveGridViewFind = dgvTwoBlendFind;
                    EnabledControl();
                    break;
                default:
                    break;
            }
        }
        private void checkOnNumberValue(TextBox textBox, string fieldCaption)
        {
            if (textBox != null)
            {
                if (Regex.IsMatch(textBox.Text, "[^1234567890]"))
                {
                    textBox.ForeColor = Color.Red;
                    Common.Tools.ShowTip(textBox, ErrorsMsg.ErrorFormat, String.Format(ErrorsMsg.NotInteger, fieldCaption), ToolTipIcon.Error);
                }
                else
                {
                    textBox.ForeColor = SystemColors.WindowText;
                }
            }
        }
        private void tabControlFind_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (sender != null)
            {
                Graphics g = e.Graphics;
                TabPage tabPage = tabControl.TabPages[e.Index];
                Rectangle tabBounds = tabControl.GetTabRect(e.Index);

                if (e.State == DrawItemState.Selected)
                    g.FillRectangle(Brushes.White, e.Bounds);
                else
                    e.DrawBackground();
                StringFormat stringFlags = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString(tabPage.Text, new Font(FontFamily.GenericSansSerif, 8), Brushes.Black,
                             tabBounds, new StringFormat(stringFlags));
            }
        }
        private void EnabledControl()
        {
            switch (ActiveTab.Name)
            {
                case "panelDirect":
                    #region panelDirect
                    buttonShowChidCard.Enabled = Tools.CorrectCardId(new Control[] { textBoxDirect });
                    buttonDirect.Enabled = buttonShowChidCard.Enabled;
                    toolStripButtonGo.Enabled = buttonShowChidCard.Enabled;
                    if (tabControlDirect.SelectedIndex == 0)
                        printToolStripButton.Enabled = dataGridViewDirect.Rows.Count > 0;
                    else
                        printToolStripButton.Enabled = dataGridViewDirectRatio.Rows.Count > 0;
                    break; 
                    #endregion
                case "panelChildOneParent":
                    #region panelChildOneParent
                    btnChildIdTwoParent.Enabled = Tools.CorrectCardId(new Control[] { textBoxChildKin });
                    btnParentIdTwoParent.Enabled = Tools.CorrectCardId(new Control[] { textBoxKnownParent });

                    toolStripButtonChildOneParentGo.Enabled = Tools.CorrectCardId(new Control[] { textBoxChildKin, textBoxKnownParent });
                    buttonChildOneParentFind.Enabled = toolStripButtonChildOneParentGo.Enabled;

                    if (tabControlChildOneParent.SelectedIndex == 0)
                        toolStripButtonChildOneParentPrint.Enabled = dataGridViewChildOneParentDirect.Rows.Count > 0;
                    else
                        toolStripButtonChildOneParentPrint.Enabled = dataGridViewChildOneParentRatio.Rows.Count > 0;
                    break; 
                    #endregion
                case "panelChildTwoParent":
                    #region panelChildTwoParent
                    btnChildCardIdTwoParent.Enabled = Tools.CorrectCardId(new Control[] { tbChildIdTwoParent });
                    btnKnownParentTwoParent.Enabled = Tools.CorrectCardId(new Control[] { tbKnownParentTwoParent });
                    btnUnknownParentIdTwoParent.Enabled = Tools.CorrectCardId(new Control[] { tbSupposedParentTwoParent });

                    tsbTwoParentGo.Enabled = Tools.CorrectCardId(new Control[] { tbChildIdTwoParent, tbKnownParentTwoParent, tbSupposedParentTwoParent });
                    btnFindTwoParent.Enabled = Tools.CorrectCardId(new Control[] { tbChildIdTwoParent, tbKnownParentTwoParent });
                    if (tcTwoParent.SelectedIndex == 0)
                        tsbTwoParetnPrint.Enabled = dgwDirectTwoParent.Rows.Count > 0;
                    else
                        tsbTwoParetnPrint.Enabled = dgwRatioTwoParent.Rows.Count > 0;
                    break; 
                    #endregion
                case "panelChildTwoUnknownParent":
                    #region panelChildTwoUnknownParent
                    btnChildSuppParentCard.Enabled = Tools.CorrectCardId(new Control[] { tbChildSuppParent });
                    btnFirstSuppParentCard.Enabled = Tools.CorrectCardId(new Control[] { tbFirstSuppParent });
                    btnSecondSuppParentCard.Enabled = Tools.CorrectCardId(new Control[] { tbSecondSuppParent });

                    tsbSuppParentGo.Enabled = Tools.CorrectCardId(new Control[] { tbChildSuppParent, tbFirstSuppParent, tbSecondSuppParent });
                    btnSuppParentFind.Enabled = Tools.CorrectCardId(new Control[] { tbFirstSuppParent, tbSecondSuppParent });
                    if (tcSuppParent.SelectedIndex == 0)
                        tsbSuppParentPrint.Enabled = dgvDirectSuppParent.Rows.Count > 0;
                    else
                        tsbSuppParentPrint.Enabled = dgvRatioSuppParent.Rows.Count > 0;
                    break; 
                    #endregion
                case "panelBlend":
                    #region panelBlend
                    btnCardBlend.Enabled = Tools.CorrectCardId(new Control[] { tbBlend });
                    btnPersonCardBlend.Enabled = Tools.CorrectCardId(new Control[] { tbPersonBlend });

                    tsbBlendGo.Enabled = Tools.CorrectCardId(new Control[] { tbBlend, tbPersonBlend });
                    btnFindBlend.Enabled = Tools.CorrectCardId(new Control[] { tbBlend });
                    nudErrorCountBlend.Enabled = btnFindBlend.Enabled;
                    nudLocusCountBlend.Enabled = btnFindBlend.Enabled;
                    if (tcBlend.SelectedIndex == 0)
                        tsbBlendPrint.Enabled = dgvBlendDirect.Rows.Count > 0;
                    else
                        tsbBlendPrint.Enabled = dgvBlendRatio.Rows.Count > 0;
                    break; 
                    #endregion
                case "panelTwoBlend":
                    #region panelTwoBlend
                    btnTwoBlendIk2.Enabled = Tools.CorrectCardId(new Control[] { textBoxTwoBlendIk2 });
                    btnTwoBlendKnown.Enabled = Tools.CorrectCardId(new Control[] { textBoxTwoBlendKnown });
                    btnTwoBlendUnknown.Enabled = Tools.CorrectCardId(new Control[] { textBoxTwoBlendUnknown });

                    tsbTwoBlendGo.Enabled = btnTwoBlendIk2.Enabled && btnTwoBlendKnown.Enabled && btnTwoBlendUnknown.Enabled;
                    btnTwoBlendFind.Enabled = btnTwoBlendKnown.Enabled && btnTwoBlendUnknown.Enabled;
                    nudTwoBlendError.Enabled = btnTwoBlendKnown.Enabled && btnTwoBlendUnknown.Enabled;
                    nudTwoBlendLocusCount.Enabled = btnTwoBlendKnown.Enabled && btnTwoBlendUnknown.Enabled;
                    if (tcTwoBlend.SelectedIndex == 0)
                        tsbTwoBlendPrint1.Enabled = dgvTwoBlendDirect.Rows.Count > 0;
                    else
                        tsbTwoBlendPrint1.Enabled = dgvTwoBlendRatio.Rows.Count > 0;
                    break; 
                    #endregion
                default:
                    break;
            }
        }
        private void FillComboBox(ComboBox[] comboBoxes)
        {
            ComboBoxItem[] comboBoxItemArray = (from DataRow dataRow in _methodVocabulary.DT.Rows
                                                select new GeneLibrary.Common.ComboBoxItem(
                                                  Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                                  String.Format(CultureInfo.InvariantCulture, "{0}", dataRow["NAME"].ToString()))).OrderBy(n => n.Id).ToArray<GeneLibrary.Common.ComboBoxItem>();

            foreach (ComboBox item in comboBoxes)
            {
                item.DisplayMember = "name";
                item.Items.Clear();
                item.Items.AddRange(comboBoxItemArray);
                if (item.Items.Count > 0)
                    item.SelectedIndex = 0;
            }
        }
        private int getMetodIdFromCombobox()
        {
            ComboBoxItem cbItem = null;
            switch (ActiveTab.Name)
            {
                case "panelDirect":
                    cbItem = tscbStripMethod.ComboBox.Items[tscbStripMethod.ComboBox.SelectedIndex] as ComboBoxItem;
                    break;
                case "panelChildOneParent":
                    cbItem = tscbStripChildOneParetnMethod.ComboBox.Items[tscbStripChildOneParetnMethod.ComboBox.SelectedIndex] as ComboBoxItem;
                    break;
                case "panelChildTwoParent":
                    cbItem = tscbTwoParentMethod.ComboBox.Items[tscbTwoParentMethod.ComboBox.SelectedIndex] as ComboBoxItem;
                    break;
                case "panelChildTwoUnknownParent":
                    cbItem = tscbSuppParentMethod.ComboBox.Items[tscbSuppParentMethod.ComboBox.SelectedIndex] as ComboBoxItem;
                    break;
                case "panelBlend":
                    cbItem = tscbBlendMethod.ComboBox.Items[tscbBlendMethod.ComboBox.SelectedIndex] as ComboBoxItem;
                    break;
                case "panelTwoBlend":
                    cbItem = tscbTooBlendMethods.ComboBox.Items[tscbTooBlendMethods.ComboBox.SelectedIndex] as ComboBoxItem;
                    break;
                default:
                    cbItem.Id = 1;
                    break;
            }

            if (cbItem != null)
                return cbItem.Id;
            else
                return -1;
        }


    }
}
