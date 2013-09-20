using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WFExceptions;
using System.Globalization;
using System.Collections.Generic;
using WFExcel;
using System.Collections.ObjectModel;

namespace GeneLibrary.MdiForms
{
    using GeneLibrary.Dialog;
    using GeneLibrary.Items;
    using GeneLibrary.Common;
    using System.Data.OracleClient;

    public partial class IklForm : CardForm
    {
        // Constructors
        public IklForm(Vocabulary vocabulary)
        {
            InitializeComponent();
            this.vocabulary = (IklVocabulary)vocabulary;
            this.Text = String.Format(resDataNames.formIkl, "New");
        }
        public IklForm(Vocabulary vocabulary, int iklId) : this(vocabulary)
        {
            this.IklId = iklId;
            if (iklId == 0)
                this.Text = String.Format(resDataNames.formIkl, "New");
            else
                this.Text = String.Format(resDataNames.formIkl, iklId.ToString());
        }
        public IklForm(IklItem iklItem) : this(new IklVocabulary())
        {
            this.iklItem = iklItem;
        }

        // Properties
        public int IklId { get; set; }
        public bool IsEdit { get { return IklId != 0; } }

        // Events handlers
        private void IklForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.IsEdit)
                {
                    vocabulary.LoadItem(IklId);
                    iklItem = vocabulary.Item;
                    iklItem.Open();

                    OpenVocabulary();

                    textBoxID.Text = iklItem.Id.ToString();
                    textBoxCardNumber.Text = iklItem.CardNumber;
                    textBoxCaseNumber.Text = iklItem.CriminalNumber;
                    textBoxSuspect.Text = iklItem.Person;
                    textBoxOrganization.Id = iklItem.OrganizationId;
                    textBoxOrganization.Text = iklItem.OrganizationVocabulary.OrgTable.Rows.Find(textBoxOrganization.Id)["NOTE"].ToString();
                    textBoxExamNumber.Text = iklItem.ExamNumber;
                    dateTimeExam.Value = iklItem.ExamDate;
                    comboBoxExpert.SelectedIndex = comboBoxExpert.FindString(
                        (from ComboBoxItem cbi in comboBoxExpert.Items where cbi.Id == iklItem.ExpertId select cbi.Name).First<string>());
                    comboBoxExpert_SelectedIndexChanged(comboBoxExpert, null);
                    comboBoxClass.SelectedIndex = comboBoxClass.FindString(
                        (from ComboBoxItem cbi in comboBoxClass.Items where cbi.Id == iklItem.ClassIklId select cbi.Name).First<string>());
                    textBoxExpertConclusion.Text = iklItem.ExamNote;
                    textBoxAncillary.Text = iklItem.Ancillary;
                    textBoxDateCardCreate.Text = iklItem.DateIns.ToShortDateString();
                    textBoxExpertSign.Text = iklItem.ExpertSign;
                    textBoxUk.Text = Tools.UkList((from ComboBoxItem comboBoxItem in iklItem.UkIds select Tools.UkName(comboBoxItem.Name, comboBoxItem.Short)).ToArray<string>());

                    // Заполняем профиль
                    FillProfile();
                }
                else if (this.iklItem == null)
                {
                    vocabulary.LoadItem();
                    iklItem = vocabulary.Item;
                    iklItem.Open();

                    OpenVocabulary();

                    textBoxDateCardCreate.Text = DateTime.Today.ToShortDateString();
                }
                else
                {
                    OpenVocabulary();

                    textBoxID.Text = iklItem.Id.ToString();
                    textBoxCardNumber.Text = iklItem.CardNumber;
                    textBoxCaseNumber.Text = iklItem.CriminalNumber;
                    textBoxSuspect.Text = iklItem.Person;
                    textBoxOrganization.Id = iklItem.OrganizationId;
                    textBoxOrganization.Text = iklItem.OrganizationVocabulary.OrgTable.Rows.Find(textBoxOrganization.Id)["NOTE"].ToString();
                    textBoxExamNumber.Text = iklItem.ExamNumber;
                    dateTimeExam.Value = iklItem.ExamDate;
                    comboBoxExpert.SelectedIndex = comboBoxExpert.FindString(
                        (from ComboBoxItem cbi in comboBoxExpert.Items where cbi.Id == iklItem.ExpertId select cbi.Name).First<string>());
                    comboBoxExpert_SelectedIndexChanged(comboBoxExpert, null);
                    comboBoxClass.SelectedIndex = comboBoxClass.FindString(
                        (from ComboBoxItem cbi in comboBoxClass.Items where cbi.Id == iklItem.ClassIklId select cbi.Name).First<string>());
                    textBoxExpertConclusion.Text = iklItem.ExamNote;
                    textBoxAncillary.Text = iklItem.Ancillary;
                    textBoxDateCardCreate.Text = iklItem.DateIns.ToShortDateString();
                    textBoxExpertSign.Text = iklItem.ExpertSign;
                    textBoxUk.Text = Tools.UkList((from ComboBoxItem comboBoxItem in iklItem.UkIds select Tools.UkName(comboBoxItem.Name, comboBoxItem.Short)).ToArray<string>());

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
        private void toolStripCardSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CheckFieldsValue();
                FillIklItemForSave();
                int cardId = iklItem.Save();
                textBoxID.Text = cardId.ToString();
                string newFormText = String.Format(resDataNames.formIkl, this.iklItem.Id.ToString());
                this.Text = newFormText;

                toolStripDropDownButtonSave.Enabled = false;
                formChanged = false;
                WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + ContextFileName(contextMenuExpert), SaveContextMenu(saveListExpert, comboBoxExpert));
                base.OnDataLoad(cardId);
                base.OnUpdateFormId(new ComboBoxItem(0, this.FormId, newFormText));
            }
            catch (OracleException err)
            {
                if (err.Message.Contains("unique constraint (MODERN.CARD_KIND_UX)"))
                    throw new WFException(ErrType.Message, String.Format(ErrorsMsg.DublicateCardNumIKL, iklItem.CardNumber));
                throw new WFException(ErrType.Message, err.Message, err);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void buttonCrimNumber_Click(object sender, EventArgs e)
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
        private void buttonExpert_Click(object sender, EventArgs e)
        {
            Button buttonUK = sender as Button;
            if (buttonUK != null)
                contextMenuExpert.Show(buttonExpert, 0, buttonExpert.Height + 2);
        }
        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (((Control)sender).Name)
            {
                //case "contextMenuUK":
                //    comboBoxCriminalNumber.SelectedIndex = comboBoxCriminalNumber.FindString(e.ClickedItem.Text);
                //    break;
                case "contextMenuExpert":
                    comboBoxExpert.SelectedIndex = comboBoxExpert.FindString(e.ClickedItem.Text);
                    break;
            }
        }
        private void toolStripAddAllele_Click(object sender, EventArgs e)
        {
            AlleleForm alleleForm = new AlleleForm(iklItem.Profile);
            iklItem.Profile.CheckAllele += new Profiles.CheckAlleleEventHandler(OnCheckAllele);
            alleleForm.ShowDialog();
        }
        private void OnCheckAllele(Locus locus, Allele allele, bool condition)
        {
            ListViewItem listViewItem;
            ListViewItem[] listViewItems = listViewLocuses.Items.Find(locus.Name, false);

            if (listViewItems.Length == 0) 
                listViewItem = listViewLocuses.Items.Add(locus.Name);
            else
                listViewItem = listViewItems[0];

            // Удаляется пустой локус
            if (locus.CheckedAlleleCount == 0)
            {
                listViewItem.Remove();
                return;
            }

            // Ограничение на трехаллельные локусы для карточки
            if (locus.CheckedAlleleCount > 2)
            {
                locus.CheckAllele(allele, false);
                throw new WFException(ErrType.Message, ErrorsMsg.IklAlleleCount);
            }

            listViewItem.SubItems.Clear();
            listViewItem.SubItems[0].Name = locus.Name;
            listViewItem.SubItems[0].Text = locus.Name;
            
            if (locus.CheckedAlleleCount == 1) // Гомозиготный локус
            {
                Allele alleleif = (from Allele alleleLocus in locus.Allele 
                                           where alleleLocus.Checked
                                           orderby Tools.AlleleConvert(alleleLocus.Name)
                                           select alleleLocus).First<Allele>();
                listViewItem.SubItems.Add(alleleif.Name).Name = alleleif.Name;
                // Дублируется гомозиготный локус
                listViewItem.SubItems.Add(alleleif.Name).Name = alleleif.Name;
            }
            else
                foreach (Allele alleleLoop in (from Allele alleleLocus in locus.Allele 
                                           where alleleLocus.Checked
                                           orderby Tools.AlleleConvert(alleleLocus.Name)
                                           select alleleLocus))
                    listViewItem.SubItems.Add(alleleLoop.Name).Name = alleleLoop.Name;
 
            ChangeControlHandler(null, null);
        }
        private void buttonOrganization_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OrganizationForm organizationForm = new OrganizationForm(iklItem.OrganizationVocabulary);
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
        private void comboBoxExpert_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                ComboBoxItem comboBoxItem = comboBox.SelectedItem as ComboBoxItem;
                if (comboBoxItem != null)
                {
                    textBoxDepartment.Text =
                        (from DataRow dataRow in iklItem.ExpertVocabulary.DT.Rows
                         where Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture) == comboBoxItem.Id
                         select dataRow["DIVISION"].ToString()).First<string>();
                }
            }
        }
        private void IklForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        toolStripAddAllele_Click(null, null);
                        break;
                    case Keys.O:
                        toolStripLoad_Click(null, null);
                        break;
                    case Keys.S:
                        toolStripCardSave_Click(null, null);
                        break;
                    default:
                        break;
                }
            }
        }
        private void toolStripLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileListForm fileListForm = new FileListForm(openFileDialog.FileName);
                if (fileListForm.ShowDialog() == DialogResult.OK)
                {
                    iklItem.Profile = fileListForm.Profile;
                    FillProfile();
                    ChangeControlHandler(null, null);
                }
            }
        }
        private void toolStripPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Collection<ReportField> reportField = new Collection<ReportField>();

                #region Заполнение полей отчета
                reportField.Add(new ReportField("#КИН", textBoxID.Text.Trim()));
                reportField.Add(new ReportField("#НОМЕР_ИКЛ", textBoxCardNumber.Text.Trim()));
                reportField.Add(new ReportField("#НОМЕР_ДЕЛА", textBoxCaseNumber.Text.Trim()));
                reportField.Add(new ReportField("#ПРОВЕРЯЕМОЕ_ЛИЦО", textBoxSuspect.Text.Trim()));
                reportField.Add(new ReportField("#СТАТЬЯ_УК", textBoxUk.Text));
                reportField.Add(new ReportField("#ОРГАН", textBoxOrganization.Text));
                reportField.Add(new ReportField("#КАРТА_ОФОРМЛЕНА", textBoxDepartment.Text));
                reportField.Add(new ReportField("#ЭКСПЕРТ", comboBoxExpert.Text));
                reportField.Add(new ReportField("#НОМЕР_ЭКСПЕРТИЗЫ", textBoxExamNumber.Text.Trim()));
                reportField.Add(new ReportField("#ДАТА_ЭКСПЕРТИЗЫ", dateTimeExam.Text));
                reportField.Add(new ReportField("#ЗАКЛЮЧЕНИЕ", textBoxExpertConclusion.Text));
                reportField.Add(new ReportField("#ДАТА_ЗАПОЛНЕНИЯ", textBoxDateCardCreate.Text));
                reportField.Add(new ReportField("#ПОДПИСЬ", textBoxExpertSign.Text));
                reportField.Add(new ReportField("#СЛУЖЕБНЫЕ_ОТМЕТКИ", textBoxAncillary.Text));

                // Заполнение полей из профиля
                Tools.FillReportFields(reportField,
                    (from Locus locus in iklItem.Profile.Locus orderby locus.Name select locus).ToArray<Locus>());

                #endregion

                ReportToExcel reportToExcel = new ReportToExcel(
                    Path.GetDirectoryName(Application.ExecutablePath) + "\\Шаблоны\\форма_икл.xlt");

                Collection<ReportCollection> reportCollections = new Collection<ReportCollection>();
                foreach (Locus locusLoop in (from Locus locusInner in iklItem.Profile.Locus
                                             where locusInner.CheckedAlleleCount > 0
                                             orderby locusInner.Name
                                             select locusInner))
                {
                    if (locusLoop.CheckedAlleleCount == 1)
                    {
                        string alleleName = (from Allele alleleInner in locusLoop.Allele where alleleInner.Checked select alleleInner.Name).First<string>();
                        reportCollections.Add(new ReportCollection(locusLoop.Name, new Collection<string>() { alleleName, alleleName }));
                    }
                    else
                    {
                        reportCollections.Add(new ReportCollection(locusLoop.Name,
                            new Collection<string>((from Allele allele in locusLoop.Allele
                                                    where allele.Checked
                                                    orderby Tools.AlleleConvert(allele.Name)
                                                    select allele.Name).ToList<string>())));
                    }
                }
                reportToExcel.Load(reportField, reportCollections);

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            if (iklItem.Id > 0)
            {
                FormFind formFind = new FormFind(iklItem.Id, CardKind.ikl);
                formFind.MdiParent = Tools.MainForm();
                formFind.OnActiveIk2Card += new ActiveIk2Card(Tools.MainForm().NewIk2Card);
                formFind.OnActiveIklCard += new ActiveIklCard(Tools.MainForm().NewIklCard);
                formFind.OnActiveStudyForm += new ActiveStudyForm(Tools.MainForm().NewStudyForm);
                formFind.Show();
            }
        }
        private void IklForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.OnCloseForm(new ComboBoxItem(0, FormId));
        }
        private void tspiCopyCard_Click(object sender, EventArgs e)
        {
            toolStripCardSave_Click(null, null);
            Common.Tools.MainForm().NewCardCopy(this.iklItem.CardCopy());
        }
        private void helpToolStripHelp_Click(object sender, EventArgs e)
        {
            Common.Tools.GetHelp("mf_find_card_new", HelpNavigator.Topic);
        }

        // Private methods
        private void CheckedCloseUkForm(List<int> ids, string text)
        {
            iklItem.UkIds.Clear();
            textBoxUk.Text = text;
            string[] itemsName = text.Split(',');
            if (ids != null)
                for (int i = 0; i < ids.Count; i++)
                    iklItem.UkIds.Add(new ComboBoxItem(ids[i], itemsName[i]));
        }
        private void ChangeControlHandler(object sender, EventArgs e)
        {
            if (formChanged)
                return;
            else
            {
                toolStripDropDownButtonSave.Enabled = true;
                formChanged = true;
            }
        }
        private static void FillComboBox(ComboBox comboBox, ComboBoxItem[] comboBoxItemArray)
        {
            comboBox.DisplayMember = "name";
            comboBox.Items.AddRange(comboBoxItemArray);
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
        }
        private static void FillContextMenu(ContextMenuStrip contextMenu, ref SaveList saveList)
        {
            saveList = (SaveList)WFSerialize.Deserialize(Path.GetDirectoryName(Application.ExecutablePath) + ContextFileName(contextMenu));
            if (saveList != null)
                foreach (SaveItem si in saveList.ListItems)
                    contextMenu.Items.Add(si.Name);
            else
                saveList = new SaveList();
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
        private static string ContextFileName(ContextMenuStrip contextMenuStrip)
        {
            switch (contextMenuStrip.Name)
            {
                case "contextMenuUK":
                    return "\\UK.dmp";
                case "contextMenuExpert":
                    return "\\Expert.dmp";
                default:
                    throw new WFException(ErrType.Assert, ErrorsMsg.NotFoundValue);
            }
        }
        private void OpenVocabulary()
        {
            FillContextMenu(contextMenuUK, ref saveListUK);
            FillComboBox(comboBoxExpert, (from DataRow dataRow in iklItem.ExpertVocabulary.ExpTable.Rows
                                          select new GeneLibrary.Common.ComboBoxItem(
                                            Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                            String.Format(CultureInfo.InvariantCulture, "{0} {1} {2}",
                                                dataRow["SURNAME"].ToString(),
                                                dataRow["NAME"].ToString(),
                                                dataRow["PATRONIC"].ToString()
                                            ))).ToArray<GeneLibrary.Common.ComboBoxItem>());
            FillComboBox(comboBoxClass, (from DataRow dataRow in iklItem.ClassIklVocabulary.DT.Rows
                                          select new GeneLibrary.Common.ComboBoxItem(
                                            Convert.ToInt32(dataRow["ID"], CultureInfo.InvariantCulture),
                                            dataRow["NAME"].ToString()
                                            )).ToArray<GeneLibrary.Common.ComboBoxItem>());
            FillContextMenu(contextMenuExpert, ref saveListExpert);
        }
        private void FillIklItemForSave()
        {
            iklItem.CardNumber = textBoxCardNumber.Text;
            iklItem.CriminalNumber = textBoxCaseNumber.Text;
            iklItem.Person = textBoxSuspect.Text;
            iklItem.OrganizationId = textBoxOrganization.Id;
            iklItem.ExamNumber = textBoxExamNumber.Text;
            iklItem.ExamDate = dateTimeExam.Value;
            iklItem.ExpertId = ((ComboBoxItem)comboBoxExpert.SelectedItem).Id;
            iklItem.ClassIklId = ((ComboBoxItem)comboBoxClass.SelectedItem).Id;
            iklItem.ExamNote = textBoxExpertConclusion.Text;
            iklItem.Ancillary = textBoxAncillary.Text;
            iklItem.DateIns = Convert.ToDateTime(textBoxDateCardCreate.Text);
            foreach (int i in textBoxUk.Ids)
                iklItem.UkIds.Add(new ComboBoxItem(i, ""));
        }
        private void CheckFieldsValue()
        {
            if (String.IsNullOrEmpty(textBoxCardNumber.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIklCardNumberField);
            if (String.IsNullOrEmpty(textBoxSuspect.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIklSuspectField);
            if (String.IsNullOrEmpty(textBoxOrganization.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIklOrganizationField);
            if (String.IsNullOrEmpty(comboBoxClass.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyIklClassField);
        }
        private void FillProfile()
        {
            listViewLocuses.Items.Clear();
            var result = from Locus locus in iklItem.Profile.Locus 
                         where locus.CheckedAlleleCount > 0 
                         orderby locus.Name select locus;
            foreach (Locus locus in result)
            {
                ListViewItem listViewItem = listViewLocuses.Items.Add(locus.Name);
                listViewItem.Name = locus.Name;

                foreach (Allele allele in (from Allele allele in locus.Allele where allele.Checked orderby Tools.AlleleConvert(allele.Name) select allele))
                    listViewItem.SubItems.Add(allele.Name).Name = allele.Name;
                
                // дублируется гомозиготные аллели
                if (locus.CheckedAlleleCount == 1)
                {
                    string alleleChecked = (from Allele alleleLoop in locus.Allele where alleleLoop.Checked select alleleLoop.Name).First<string>();
                    if (!String.IsNullOrEmpty(alleleChecked))
                        listViewItem.SubItems.Add(alleleChecked).Name = alleleChecked;
                }
            }
        }

        // Fields
        private IklVocabulary vocabulary;
        private bool formChanged;
        private IklItem iklItem;
        private SaveList saveListUK;
        private SaveList saveListExpert;

    }
}
