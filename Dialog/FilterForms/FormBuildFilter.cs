using System;
using System.Linq;
using System.Windows.Forms;
using GeneLibrary.Items;
using GeneLibrary.Common;
using WFExceptions;
using WFDatabase;


namespace GeneLibrary.Dialog
{
    public partial class FormBuildFilter : Form
    {
        // Constructors
        public FormBuildFilter(int id)
        {
            InitializeComponent();
            this.Id = id;
        }

        // Events handler
        private void FormBulidFilter_Load(object sender, EventArgs e)
        {
            filterItem = GateFactory.FilterItem(this.Id);
            listBoxIkl.Items.AddRange((from FilterField filterValue in filterItem.IklFilter select filterValue).ToArray<FilterField>());
            listBoxIk2.Items.AddRange((from FilterField filterValue in filterItem.Ik2Filter select filterValue).ToArray<FilterField>());
            
            if (this.Id > 0)
            {
                filterItem.Load();
                SetFilterCondition();
            }

            if (listBoxIkl.Items.Count > 0)
                listBoxIkl.SelectedIndex = 0;

            if (filterItem.InIk2)
                tabControlCardType.SelectedIndex = 1;
            else
                tabControlCardType.SelectedIndex = 0;
        }
        private void Ikl_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxCondition.Controls.Clear();
            FilterField filterValue = listBoxIkl.SelectedItem as FilterField;
            if (filterValue != null)
            {
                switch (filterValue.Name)
                {
                    case "cardIklId":
                        currentFilterControl = new FilterControlInteger(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First()
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIklCrimNumber":
                    case "cardIklSuspect":
                    case "cardIklNumber":
                    case "cardIklExamNumber":
                    case "cardIklExpertConclusion":
                    case "cardIklExpertSign":
                    case "cardIklAncillary":
                        currentFilterControl = new FilterControlString(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First()
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIklExamDate":
                    case "cardIklCreateData":
                        currentFilterControl = new FilterControlDate(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First()
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIklCategory":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new ClassIklVocabulary(), new string[2] {"ID", "NAME"}
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIklUkNumber":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new UKVocabulary(), new string[2] { "ID", "ARTCL" },
                             "where artcl <> '0'"
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIklOrgan":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new OrganizationVocabulary(), new string[2] { "ID", "NOTE" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIklDepartment":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new DivisionVocabulary(), new string[2] { "ID", "NAME" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIklExpert":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.IklFilter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new ExpertVocabulary(), new string[2] { "ID", "NAME" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    default:
                        break;
                }
            }
        }
        private void Ik2_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxCondition.Controls.Clear();
            FilterField filterValue = listBoxIk2.SelectedItem as FilterField;
            if (filterValue != null)
            {
                switch (filterValue.Name)
                {
                    case "cardIk2Id":
                    case "cardIk2Conclusion":
                    case "cardIk2PersonInfo":
                        currentFilterControl = new FilterControlInteger(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First()
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2Number":
                    case "cardIk2CrimNumber":
                    case "cardIk2CodeAccount":
                    case "cardIk2Year":
                    case "cardIk2Address":
                    case "cardIk2Victim":
                    case "cardIk2ExamNumber":
                    case "cardIk2Ancillary":
                    case "cardIk2Object":
                    case "cardIk2Size":
                    case "cardIk2Concent":
                    case "cardIk2Amount":
                        currentFilterControl = new FilterControlString(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First()
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2CrimDate":
                    case "cardIk2ExamDate":
                    case "cardIk2CreateData":
                        currentFilterControl = new FilterControlDate(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First()
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2CodeLin":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new LinVocabulary(), new string[2] { "ID", "CODE" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2CodeMvd":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new MvdVocabulary(), new string[2] { "ID", "CODE" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2Category":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new ClassObjectVocabulary(), new string[2] { "ID", "NAME" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2SortObject":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new SortObjectVocabulary(), new string[2] { "ID", "NAME" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2Expert":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new ExpertVocabulary(), new string[2] { "ID", "NAME" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2Department":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new DivisionVocabulary(), new string[2] { "ID", "NAME" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2UkNumber":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new UKVocabulary(), new string[2] { "ID", "ARTCL" },
                             "where artcl <> '0'"
                             );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    case "cardIk2Organ":
                        currentFilterControl = new FilterControlList(
                            (from FilterField innerFilterValue in filterItem.Ik2Filter
                             where innerFilterValue.Name == filterValue.Name
                             select innerFilterValue).First(),
                             new OrganizationVocabulary(), new string[2] { "ID", "NOTE" }
                                 );
                        currentFilterControl.Parent = groupBoxCondition;
                        currentFilterControl.Dock = DockStyle.Fill;
                        break;
                    default:
                        break;
                }
            }
        }
        private void tabControlCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterItem.InIkl = tabControlCardType.SelectedTab.Name == "tabPageIkl";
            filterItem.InIk2 = tabControlCardType.SelectedTab.Name == "tabPageIk2";

            switch (tabControlCardType.SelectedTab.Name)
            {
                case "tabPageIkl":
                    if (listBoxIkl.SelectedItems.Count > 0)
                        Ikl_SelectedIndexChanged(null, null);
                    else
                        listBoxIkl.SelectedIndex = 0;
                    break;
                case "tabPageIk2":
                    if (listBoxIk2.SelectedItems.Count > 0)
                        Ik2_SelectedIndexChanged(null, null);
                    else
                        listBoxIk2.SelectedIndex = 0;
                    break;
                default:
                    groupBoxCondition.Controls.Clear();
                    break;
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxFilterName.Text.Trim()))
                    throw new WFException(ErrType.Message, ErrorsMsg.EmptyFilterName);
                filterItem.Name = textBoxFilterName.Text.Trim();
                filterItem.InIkl  = tabControlCardType.SelectedIndex == 0;
                filterItem.InIk2 = tabControlCardType.SelectedIndex == 1;
                filterItem.InArchive = checkBoxInArchive.Checked;
                this.Id = filterItem.Save();
                if (OnUpdateId != null)
                    OnUpdateId(this.Id);
            }
            catch 
            {
                throw;
            }
        }
        private void checkBoxInArchive_CheckedChanged(object sender, EventArgs e)
        {
            filterItem.InArchive = checkBoxInArchive.Checked; 
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
        
        //Properties
        public int Id {get; set;}

        // Private members
        private void SetFilterCondition()
        {
            textBoxFilterName.Text = filterItem.Name;
            checkBoxInArchive.Checked = filterItem.InArchive;
        }

        // Fields
        private FilterConrol currentFilterControl;
        private FilterItem filterItem;

        //Events
        internal event GeneLibrary.Common.UpdateId OnUpdateId;

    }

}
