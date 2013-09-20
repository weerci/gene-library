using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Items;
using GeneLibrary.Common;
using System.IO;
using WFExceptions;
using System.Globalization;
using GeneLibrary.Dialog;
using System.Collections.ObjectModel;
using WFExcel;
using System.Data.OracleClient;


namespace GeneLibrary.MdiForms
{
    public partial class IK2Form : CardForm
    {
        // Constructors
        public IK2Form(Vocabulary vocabulary)
        {
            InitializeComponent();
            this.vocabulary = (Ik2Vocabulary)vocabulary;
            this.Text = String.Format(resDataNames.formIk2, "New");

        }
        public IK2Form(Vocabulary vocabulary, int ik2Id) : this(vocabulary)
        {
            this.IK2Id = ik2Id;
            if (this.IK2Id == 0)
                this.Text = String.Format(resDataNames.formIk2, "New"); 
            else
            {
                toolStripButtonFind.Enabled = true;
                this.Text = String.Format(resDataNames.formIk2, ik2Id.ToString());
            }
        }
        public IK2Form(IK2Item ik2Item) : this(new Ik2Vocabulary())
        {
            this.ik2Item = ik2Item;
        }

        // Events handlers
        private void IK2Form_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.IsEdit)
                {
                    vocabulary.LoadItem(IK2Id);
                    ik2Item = vocabulary.Item;
                    ik2Item.Open();

                    // Заполениние полей данных словарей
                    OpenVocabulary();

                    textBoxId.Text = ik2Item.Id.ToString();
                    textBoxCardNumber.Text = ik2Item.CardNumber;
                    textBoxCaseNumber.Text = ik2Item.CriminalNumber;
                    comboBoxMvd.SelectedIndex = comboBoxMvd.FindString(
                        (from ComboBoxItem cbi in comboBoxMvd.Items where cbi.Id == ik2Item.MvdId select cbi.Name).First<string>());
                    comboBoxLin.SelectedIndex = comboBoxLin.FindString(
                        (from ComboBoxItem cbi in comboBoxLin.Items where cbi.Id == ik2Item.LinId select cbi.Name).First<string>());
                    comboBoxYear.SelectedIndex = comboBoxYear.FindString(ik2Item.YearState.ToString());
                    textBoxAddress.Text = ik2Item.AddressCrime;
                    dateTimeDateCrim.Value = ik2Item.DateCrime;
                    textBoxVictim.Text = ik2Item.Victim;
                    textBoxOrganization.Id = ik2Item.OrganizationId;
                    textBoxOrganization.Text = ik2Item.OrganizationVocabulary.OrgTable.Rows.Find(ik2Item.OrganizationId)["NOTE"].ToString();
                    comboBoxExpert.SelectedIndex = comboBoxExpert.FindString(
                        (from ComboBoxItem cbi in comboBoxExpert.Items where cbi.Id == ik2Item.ExpertId select cbi.Name).First<string>());
                    comboBoxClass.SelectedIndex = comboBoxClass.FindString(
                        (from ComboBoxItem cbi in comboBoxClass.Items where cbi.Id == ik2Item.ClassId select cbi.Name).First<string>());
                    comboBoxSort.SelectedIndex = comboBoxSort.FindString(
                        (from ComboBoxItem cbi in comboBoxSort.Items where cbi.Id == ik2Item.SortId select cbi.Name).First<string>());
                    textBoxExamNumber.Text = ik2Item.ExamNumber;
                    dateTimeExam.Value = ik2Item.ExamDate;
                    textBoxDateCreate.Text = ik2Item.DateIns.ToShortDateString();
                    textBoxSerialDNA.Text = ik2Item.SeralNumberDNA;
                    textBoxObject.Text = ik2Item.Object;
                    textBoxSize.Text = ik2Item.SpotSize;
                    textBoxConcent.Text = ik2Item.ConcentDna;
                    textBoxAmount.Text = ik2Item.AmountDna;
                    textBoxUk.Text = Tools.UkList((from ComboBoxItem comboBoxItem in ik2Item.UkIds select Tools.UkName(comboBoxItem.Name, comboBoxItem.Short)).ToArray<string>());
                    FillIdent();

                    // Заполняем профиль
                    FillProfile();
                }
                else if (this.ik2Item == null)
                {
                    vocabulary.LoadItem();
                    ik2Item = vocabulary.Item;
                    ik2Item.Open();

                    OpenVocabulary();

                    textBoxDateCreate.Text = DateTime.Today.ToShortDateString();
                }
                else
                {
                    OpenVocabulary();

                    textBoxId.Text = ik2Item.Id.ToString();
                    textBoxCardNumber.Text = ik2Item.CardNumber;
                    textBoxCaseNumber.Text = ik2Item.CriminalNumber;
                    comboBoxMvd.SelectedIndex = comboBoxMvd.FindString(
                        (from ComboBoxItem cbi in comboBoxMvd.Items where cbi.Id == ik2Item.MvdId select cbi.Name).First<string>());
                    comboBoxLin.SelectedIndex = comboBoxLin.FindString(
                        (from ComboBoxItem cbi in comboBoxLin.Items where cbi.Id == ik2Item.LinId select cbi.Name).First<string>());
                    comboBoxYear.SelectedIndex = comboBoxYear.FindString(ik2Item.YearState.ToString());
                    textBoxAddress.Text = ik2Item.AddressCrime;
                    dateTimeDateCrim.Value = ik2Item.DateCrime;
                    textBoxVictim.Text = ik2Item.Victim;
                    textBoxOrganization.Id = ik2Item.OrganizationId;
                    textBoxOrganization.Text = ik2Item.OrganizationVocabulary.OrgTable.Rows.Find(ik2Item.OrganizationId)["NOTE"].ToString();
                    comboBoxExpert.SelectedIndex = comboBoxExpert.FindString(
                        (from ComboBoxItem cbi in comboBoxExpert.Items where cbi.Id == ik2Item.ExpertId select cbi.Name).First<string>());
                    comboBoxClass.SelectedIndex = comboBoxClass.FindString(
                        (from ComboBoxItem cbi in comboBoxClass.Items where cbi.Id == ik2Item.ClassId select cbi.Name).First<string>());
                    comboBoxSort.SelectedIndex = comboBoxSort.FindString(
                        (from ComboBoxItem cbi in comboBoxSort.Items where cbi.Id == ik2Item.SortId select cbi.Name).First<string>());
                    textBoxExamNumber.Text = ik2Item.ExamNumber;
                    dateTimeExam.Value = ik2Item.ExamDate;
                    textBoxDateCreate.Text = ik2Item.DateIns.ToShortDateString();
                    textBoxSerialDNA.Text = ik2Item.SeralNumberDNA;
                    textBoxObject.Text = ik2Item.Object;
                    textBoxSize.Text = ik2Item.SpotSize;
                    textBoxConcent.Text = ik2Item.ConcentDna;
                    textBoxAmount.Text = ik2Item.AmountDna;
                    textBoxUk.Text = Tools.UkList((from ComboBoxItem comboBoxItem in ik2Item.UkIds select Tools.UkName(comboBoxItem.Name, comboBoxItem.Short)).ToArray<string>());

                    FillIdent();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            // При изменении данных на форме кнопка "Сохранить" - активируется
            GeneLibrary.Common.Tools.SetControlChangedHandler((from Control c in groupBoxFace.Controls
                                                               select c).Union(from Control c in groupBoxReverse.Controls
                                                                               select c), ChangeControlHandler);

        }
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CheckFieldsValue();
                FillIk2ItemForSave();
                IK2Id = ik2Item.Save();
                textBoxId.Text = IK2Id.ToString();
                string newFormText = String.Format(resDataNames.formIk2, this.ik2Item.Id.ToString());
                this.Text = newFormText;
                
                toolStripDropDownButtonSave.Enabled = false;
                formChanged = false;
                WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + ContextFileName(contextMenuExpert), SaveContextMenu(saveListExpert, comboBoxExpert));
                WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + ContextFileName(contextMenuMvd), SaveContextMenu(saveListMvd, comboBoxMvd));
                WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + ContextFileName(contextMenuLin), SaveContextMenu(saveListLin, comboBoxLin));
                base.OnDataLoad(IK2Id);
                base.OnUpdateFormId(new ComboBoxItem(0, this.FormId, newFormText));

                toolStripButtonRefresh_Click(null, null);
            }
            catch (OracleException err)
            {
                if (err.Message.Contains("unique constraint (MODERN.CARD_KIND_UX)"))
                    throw new WFException(ErrType.Message, String.Format(ErrorsMsg.DublicateCardNumIKL2, ik2Item.CardNumber));
                throw new WFException(ErrType.Message, err.Message, err);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void comboBoxExpert_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                ComboBoxItem comboBoxItem = comboBox.SelectedItem as ComboBoxItem;
                if (comboBoxItem != null)
                {
                    textBoxDepartment.Text =
                        (from DataRow dataRow in ik2Item.ExpertVocabulary.DT.Rows
                         where Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture) == comboBoxItem.Id
                         select dataRow["DIVISION"].ToString()).First<string>();
                }
            }
        }
        private void toolStripButtonAddAllele_Click(object sender, EventArgs e)
        {
            AlleleForm alleleForm = new AlleleForm(ik2Item.Profile);
            ik2Item.Profile.CheckAllele += new Profiles.CheckAlleleEventHandler(OnCheckAllele);
            alleleForm.ShowDialog();
        }
        private void IK2Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        toolStripButtonAddAllele_Click(null, null);
                        break;
                    case Keys.O:
                        toolStripButtonLoad_Click(null, null);
                        break;
                    case Keys.S:
                        toolStripButtonSave_Click(null, null);
                        break;
                    default:
                        break;
                }
            }
        }
        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Collection<ReportField> reportField = new Collection<ReportField>();

                #region Заполнение полей отчета
                reportField.Add(new ReportField("#КИН", textBoxId.Text));
                reportField.Add(new ReportField("#НОМЕР_ИК2", textBoxCardNumber.Text));
                reportField.Add(new ReportField("#НОМЕР_ДЕЛА", textBoxCaseNumber.Text));
                reportField.Add(new ReportField("#КОД_МВД", ((ComboBoxItem)comboBoxMvd.SelectedItem).Short));
                reportField.Add(new ReportField("#КОД_ГОРОРГАНА", ((ComboBoxItem)comboBoxLin.SelectedItem).Short));
                reportField.Add(new ReportField("#ГОД_УЧЕТА", comboBoxYear.Text));
                reportField.Add(new ReportField("#СТАТЬЯ_УК", textBoxUk.Text));
                reportField.Add(new ReportField("#АДРЕС", textBoxAddress.Text));
                reportField.Add(new ReportField("#ДАТА_ПРЕСТУПЛЕНИЯ", dateTimeDateCrim.Text));
                reportField.Add(new ReportField("#ПОТЕРПЕВШИЙ", textBoxVictim.Text));
                reportField.Add(new ReportField("#ОРГАН", textBoxOrganization.Text));
                reportField.Add(new ReportField("#КАРТА_ОФОРМЛЕНА", textBoxDepartment.Text));
                reportField.Add(new ReportField("#НОМЕР_ЭКСПЕРТИЗЫ", textBoxExamNumber.Text));
                reportField.Add(new ReportField("#ДАТА_ЭКСПЕРТИЗЫ", dateTimeExam.Text));
                reportField.Add(new ReportField("#ЭКСПЕРТ", comboBoxExpert.Text));
                reportField.Add(new ReportField("#ДАТА_ЗАПОЛНЕНИЯ", textBoxDateCreate.Text));
                reportField.Add(new ReportField("#СЛУЖЕБНЫЕ_ОТМЕТКИ", textBoxSerialDNA.Text));
                reportField.Add(new ReportField("#ОБЪЕКТ", textBoxObject.Text));
                reportField.Add(new ReportField("#РАЗМЕР", textBoxSize.Text));
                reportField.Add(new ReportField("#КОНЦЕНТРАЦИЯ", textBoxConcent.Text));
                reportField.Add(new ReportField("#КОЛИЧЕСТВО", textBoxAmount.Text));

                // Заполнение полей из профиля
                Tools.FillReportFields(reportField,
                    (from Locus locus in ik2Item.Profile.Locus orderby locus.Name select locus).ToArray<Locus>());

                #endregion

                ReportToExcel reportToExcel = new ReportToExcel(
                    Path.GetDirectoryName(Application.ExecutablePath) + "\\Шаблоны\\форма_ик2.xlt");

                Collection<ReportCollection> reportCollections = new Collection<ReportCollection>();
                foreach (Locus locusLoop in (from Locus locusInner in ik2Item.Profile.Locus where locusInner.CheckedAlleleCount > 0 orderby locusInner.Name select locusInner))
                    reportCollections.Add(new ReportCollection(locusLoop.Name,
                        new Collection<string>((from Allele allele in locusLoop.Allele
                                                where allele.Checked
                                                orderby Tools.AlleleConvert(allele.Name)
                                                select allele.Name).ToList<string>())));
                reportToExcel.Load(reportField, reportCollections);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void buttonUk_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UkCheckForm ukForm = new UkCheckForm();
                ukForm.OnCheckedClose += new UpdateIdsText(CheckedCloseUkForm);
                ukForm.ShowDialog();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void buttonOrganization_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OrganizationForm organizationForm = new OrganizationForm(ik2Item.OrganizationVocabulary);
                organizationForm.OnDataUpdate += new OrganizationForm.OrganizationSelected((i, s) =>
                {
                    textBoxOrganization.Id = i;
                    textBoxOrganization.Text = s;
                });
                organizationForm.ShowDialog();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void buttonList_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
                switch (button.Name)
                {
                    case "buttonExpert":
                        contextMenuExpert.Show(buttonExpert, 0, buttonExpert.Height + 2);
                        break;
                    case "buttonMvd":
                        contextMenuMvd.Show(buttonMvd, 0, buttonMvd.Height + 2);
                        break;
                    case "buttonLin":
                        contextMenuLin.Show(buttonLin, 0, buttonLin.Height + 2);
                        break;
                    default:
                        break;
                }
        }
        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileListForm fileListForm = new FileListForm(openFileDialog.FileName);
                if (fileListForm.ShowDialog() == DialogResult.OK)
                {
                    ik2Item.Profile = fileListForm.Profile;
                    FillProfile();
                    ChangeControlHandler(null, null);
                }
            }
        }
        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            if (ik2Item.Id > 0)
            {
                FormFind formFind = new FormFind(ik2Item.Id, CardKind.ik2);
                formFind.MdiParent = Tools.MainForm();
                formFind.OnActiveIk2Card += new ActiveIk2Card(Tools.MainForm().NewIk2Card);
                formFind.OnActiveIklCard += new ActiveIklCard(Tools.MainForm().NewIklCard);
                formFind.OnActiveStudyForm += new ActiveStudyForm(Tools.MainForm().NewStudyForm);
                formFind.Show();
            }
        }
        private void listViewIk2_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ChangeControlHandler(sender, e);
        }
        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            IK2Form_Load(null, null);
            toolStripDropDownButtonSave.Enabled = false;
            formChanged = false;
        }
        private void IK2Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.OnCloseForm(new ComboBoxItem(0, FormId));
        }
        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip contextMenu = sender as ContextMenuStrip;
            if (contextMenu != null)
                switch (contextMenu.Name)
                {
                    case "contextMenuExpert":
                        comboBoxExpert.SelectedIndex = comboBoxExpert.FindString(e.ClickedItem.Text);
                        break;
                    case "contextMenuMvd":
                        comboBoxMvd.SelectedIndex = comboBoxMvd.FindString(e.ClickedItem.Text);
                        break;
                    case "contextMenuLin":
                        comboBoxLin.SelectedIndex = comboBoxLin.FindString(e.ClickedItem.Text);
                        break;
                    default:
                        break;
                }
        }
        private void stmSaveAndCopy_Click(object sender, EventArgs e)
        {
            toolStripButtonSave_Click(null, null);
            Common.Tools.MainForm().NewCardCopy(this.ik2Item.CardCopy());
        }
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            Common.Tools.GetHelp("mf_find_card_new", HelpNavigator.Topic);
        }

        // Private members
        private void ChangeControlHandler(object sender, EventArgs e)
        {
            if (formChanged)
                return;
            else
            {
                toolStripDropDownButtonSave.Enabled = true;
                formChanged = true;
            }
            toolStripButtonFind.Enabled = ik2Item.Id != 0; 
        }
        private static void FillComboBox(ComboBox comboBox, ComboBoxItem[] comboBoxItemArray)
        {
            comboBox.DisplayMember = "name";
            comboBox.Items.Clear();
            comboBox.Items.AddRange(comboBoxItemArray);
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
        }
        private static string ContextFileName(ContextMenuStrip contextMenuStrip)
        {
            switch (contextMenuStrip.Name)
            {
                case "contextMenuUk":
                    return "\\UkIk2.dmp";
                case "contextMenuExpert":
                    return "\\ExpertIk2.dmp";
                case "contextMenuMvd":
                    return "\\MvdIk2.dmp";
                case "contextMenuLin":
                    return "\\LinIk2.dmp";
                case "contextMenuOrganization":
                    return "\\OrganIk2.dmp";
                default:
                    throw new WFException(ErrType.Assert, ErrorsMsg.NotFoundValue);
            }
        }
        private static void FillContextMenu(ContextMenuStrip contextMenu, ref SaveList saveList)
        {
            saveList = (SaveList)WFSerialize.Deserialize(Path.GetDirectoryName(Application.ExecutablePath) + ContextFileName(contextMenu));
            if (saveList != null)
            {
                contextMenu.Items.Clear();
                foreach (SaveItem si in saveList.ListItems)
                    contextMenu.Items.Add(si.Name);
            }
            else
                saveList = new SaveList();
        }
        private void OpenVocabulary()
        {
            FillComboBox(comboBoxExpert, (from DataRow dataRow in ik2Item.ExpertVocabulary.ExpTable.Rows
                                          select new ComboBoxItem(
                                                Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                                String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}",
                                                dataRow["SURNAME"].ToString(),
                                                dataRow["NAME"].ToString(),
                                                dataRow["PATRONIC"].ToString()
                                            ))).ToArray<ComboBoxItem>());
            FillContextMenu(contextMenuExpert, ref saveListExpert);
            FillComboBox(comboBoxMvd, (from DataRow dataRow in ik2Item.MvdVocabulary.DT.Rows
                                       select new ComboBoxItem(
                                           Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                           String.Format(CultureInfo.InvariantCulture, "{0}, {1}",
                                           dataRow["CODE"].ToString(),
                                           dataRow["SHORT_NAME"].ToString()),
                                           dataRow["CODE"].ToString()
                                           )).ToArray<ComboBoxItem>());
            FillContextMenu(contextMenuMvd, ref saveListMvd);
            FillComboBox(comboBoxLin, (from DataRow dataRow in ik2Item.LinVocabulary.DT.Rows
                                       select new ComboBoxItem(
                                           Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                           String.Format(CultureInfo.InvariantCulture, "{0}, {1}",
                                           dataRow["CODE"].ToString(),
                                           dataRow["ORGAN"].ToString()),
                                           dataRow["CODE"].ToString()
                                           )).ToArray<ComboBoxItem>());
            FillContextMenu(contextMenuLin, ref saveListLin);
            FillComboBox(comboBoxClass, (from DataRow dataRow in ik2Item.ClassObjectVocabulary.DT.Rows
                                            select new ComboBoxItem(
                                            Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                            dataRow["NAME"].ToString())
                                            ).ToArray<ComboBoxItem>());
            FillComboBox(comboBoxSort, (from DataRow dataRow in ik2Item.SortObjectVocabulary.DT.Rows
                                       select new ComboBoxItem(
                                           Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                           dataRow["NAME"].ToString()
                                           )).ToArray<ComboBoxItem>());
            FillYear();
        }
        private void FillProfile()
        {
            listViewLocuses.Items.Clear();
            var result = from Locus locus in ik2Item.Profile.Locus
                         where locus.CheckedAlleleCount > 0
                         orderby locus.Name
                         select locus;
            foreach (Locus locus in result)
            {
                ListViewItem listViewItem = listViewLocuses.Items.Add(locus.Name);
                listViewItem.Name = locus.Name;

                foreach (Allele allele in (from Allele allele in locus.Allele where allele.Checked orderby Tools.AlleleConvert(allele.Name) select allele))
                        listViewItem.SubItems.Add(allele.Name).Name = allele.Name;
            }

        }
        private static SaveList SaveContextMenu(SaveList saveList, ComboBox comboBox)
        {
            ComboBoxItem comboBoxItem = comboBox.Items[comboBox.SelectedIndex] as ComboBoxItem;
            if (comboBox != null)
            {
                saveList.Add(new SaveItem(comboBoxItem.Id, comboBoxItem.Name));
            }
            return saveList;
        }
        private void CheckFieldsValue()
        {
            if (String.IsNullOrEmpty(textBoxCardNumber.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIk2CardNumberField);
            if (String.IsNullOrEmpty(textBoxOrganization.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIk2OrganField);
            if (String.IsNullOrEmpty(comboBoxMvd.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIk2MvdField);
            if (String.IsNullOrEmpty(comboBoxLin.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIk2LinField);
            if (String.IsNullOrEmpty(comboBoxYear.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIk2YearField);
            if (String.IsNullOrEmpty(comboBoxClass.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIk2ClassField);
            if (String.IsNullOrEmpty(comboBoxSort.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIk2SortField);
        }
        private void FillIk2ItemForSave()
        {
            ik2Item.CardNumber = textBoxCardNumber.Text;
            ik2Item.CriminalNumber = textBoxCaseNumber.Text;
            ik2Item.MvdId = ((ComboBoxItem)comboBoxMvd.SelectedItem).Id;
            ik2Item.LinId = ((ComboBoxItem)comboBoxLin.SelectedItem).Id;
            ik2Item.YearState = Convert.ToInt32(comboBoxYear.Text, CultureInfo.InvariantCulture);
            ik2Item.AddressCrime = textBoxAddress.Text;
            ik2Item.DateCrime = dateTimeDateCrim.Value;
            ik2Item.Victim = textBoxVictim.Text;
            ik2Item.SeralNumberDNA = textBoxSerialDNA.Text;
            ik2Item.OrganizationId = textBoxOrganization.Id;
            ik2Item.ExamNumber = textBoxExamNumber.Text;
            ik2Item.ExamDate = dateTimeExam.Value;
            ik2Item.ExpertId = ((ComboBoxItem)comboBoxExpert.SelectedItem).Id;
            ik2Item.ClassId = ((ComboBoxItem)comboBoxClass.SelectedItem).Id;
            ik2Item.SortId = ((ComboBoxItem)comboBoxSort.SelectedItem).Id;
            ik2Item.DateIns = Convert.ToDateTime(textBoxDateCreate.Text);
            ik2Item.Object = textBoxObject.Text;
            ik2Item.SpotSize = textBoxSize.Text;
            ik2Item.ConcentDna = textBoxConcent.Text;
            ik2Item.AmountDna = textBoxAmount.Text;
            ik2Item.CardUnIdent.Clear();
            ik2Item.CardUnIdent = new Collection<CardIdent>((from ListViewItem listViewItemInner in listViewIk2.Items
                      where listViewItemInner.Checked
                      select new CardIdent(
                          Convert.ToInt32(listViewItemInner.SubItems[1].Name, CultureInfo.InvariantCulture),
                          listViewItemInner.Text, listViewItemInner.SubItems[1].Text, CardKind.ik2, 0
                          )).
                      Union(from ListViewItem listViewItemInner in listViewIkl.Items
                            where listViewItemInner.Checked
                            select new CardIdent(
                          Convert.ToInt32(listViewItemInner.SubItems[1].Name, CultureInfo.InvariantCulture),
                          listViewItemInner.Text, listViewItemInner.SubItems[1].Text, CardKind.ikl, 0
                          )).ToList<CardIdent>());
        }
        private void FillYear()
        {
            int year = DateTime.Today.Year;
            int startYear = Convert.ToInt32(Properties.Settings.Default.startYear, CultureInfo.InvariantCulture);

            comboBoxYear.Items.Clear();
            for (int i = startYear; i < year+1; i++)
                comboBoxYear.Items.Add(i.ToString());
            comboBoxYear.SelectedIndex = comboBoxYear.FindString(year.ToString());
        }
        private void OnCheckAllele(Locus locus, Allele allele, bool condition)
        {
            ListViewItem[] listViewItems = listViewLocuses.Items.Find(locus.Name, false);
            if (condition)
            {
                if (listViewItems.Length == 0)
                {
                    ListViewItem listViewItem = listViewLocuses.Items.Add(locus.Name);
                    listViewItem.Name = locus.Name;
                    listViewItem.SubItems.Add(allele.Name).Name = allele.Name;
                    ChangeControlHandler(null, null);
                }
                else
                {
                    if (listViewItems[0].SubItems.IndexOfKey(allele.Name) == -1)
                    {
                        listViewItems[0].SubItems.Add(allele.Name).Name = (allele.Name);
                        ChangeControlHandler(null, null);
                    }
                }
            }
            else
            {
                if (listViewItems.Length > 0)
                {
                    int index = listViewItems[0].SubItems.IndexOfKey(allele.Name);
                    if (index > -1)
                        listViewItems[0].SubItems.RemoveAt(index);
                    if (locus.CheckedAlleleCount == 0)
                        listViewLocuses.Items.Remove(listViewItems[0]);
                    ChangeControlHandler(null, null);
                }
            }
        }
        private void FillIdent()
        {
            listViewIkl.Items.Clear();
            listViewIk2.Items.Clear();
            foreach (CardIdent cardIdentLoop in from CardIdent cardIdent in ik2Item.CardIdent
                                                select cardIdent)
            {
                string arch;
                if (cardIdentLoop.Arch == 0)
                    arch = "";
                else
                    arch = String.Format("({0})", resDataNames.archive);

                ListViewItem listViewItem;
                if (cardIdentLoop.CardKind == CardKind.ikl)
                {
                    listViewItem = listViewIkl.Items.Add("");
                    listViewItem.SubItems.Add(String.Format("КИН-{0}  №{1} {2}", cardIdentLoop.CardId, cardIdentLoop.CardNumber, arch)).Name = cardIdentLoop.CardId.ToString();
                }
                else
                {
                    listViewItem = listViewIk2.Items.Add("");
                    listViewItem.SubItems.Add(String.Format("КИН-{0}  №{1} {2}", cardIdentLoop.CardId, cardIdentLoop.CardNumber, arch)).Name = cardIdentLoop.CardId.ToString();
                }
            }
        }

        // Properties
        public int IK2Id { get; set; }
        public bool IsEdit { get { return IK2Id != 0; } }

        // Fields
        private void CheckedCloseUkForm(List<int> ids, string text)
        {
            textBoxUk.Text = text;
            ik2Item.UkIds.Clear();
            if (ids != null)
                foreach (int id in ids)
                    ik2Item.UkIds.Add(new ComboBoxItem(id, ""));
        }
        private Ik2Vocabulary vocabulary;
        private bool formChanged;
        private IK2Item ik2Item;
        private SaveList saveListExpert;
        private SaveList saveListMvd;
        private SaveList saveListLin;

    }
}
