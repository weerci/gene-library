using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLibrary.Items;
using System.Globalization;
using GeneLibrary.Common;
using WFExceptions;
using WFDatabase;
using GeneLibrary.Items.Find;

namespace GeneLibrary.Dialog
{
    public partial class FormFind : Form
    {
        public FormFind(int cardId, CardKind cardKind)
        {
            this.CardId = cardId;
            this.cardKind = cardKind;
            InitializeComponent();
        }

        // Events handlers
        private void FormFind_Load(object sender, EventArgs e)
        {
            label1.Text = String.Format(resDataNames.formFindProfileFrom, this.CardId.ToString());
            this.Text = String.Format(resDataNames.formFindText, this.CardId.ToString()) + String.Format(" ({0})", Tools.CardKindName(this.cardKind));
            profileCard.Load(CardId, GateFactory.GLDefault().DefaultMethod);
            //            numericUpDownLocusCount.Value = profileCard.Locus.Where(n=>n.CheckedAlleleCount > 0 && n.Name != "Amelogenin").Count();
            numericUpDownLocusCount.Value = 8;
            EnableControls();
        }
        private void buttonFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                label2.Text = String.Format(resDataNames.formFindProfileTo, "");

                Tools.ClearListViewText(listViewProfiles);

                FindGridDecorator findGridDecorator = new FindGridDecorator(
                    new FindCoincidence(), dataGridViewFind
                    );
                findGridDecorator.FoundCondition.CardsString = new FindCondition.FindField()
                {
                    FieldName = new string[] { },
                    FieldValue = new string[] { this.CardId.ToString() }
                };
                findGridDecorator.FoundCondition.CountCoincidenceString = new FindCondition.FindField()
                {
                    FieldName = new string[] { radioButtonFindDirect.Text },
                    FieldValue = new string[] { numericUpDown.Value.ToString() }
                };
                findGridDecorator.FoundCondition.CountLocusString = new FindCondition.FindField()
                {
                    FieldName = new string[] { label4.Text },
                    FieldValue = new string[] { numericUpDownLocusCount.Value.ToString() }
                };
                findGridDecorator.FoundCondition.AccurencyString = new FindCondition.FindField()
                {
                    FieldName = new string[] { labelAccurency.Text },
                    FieldValue = new string[] { nudCountLocusesWithNoEqualAllelies.Value.ToString() }
                };

                if (radioButtonFindDirect.Checked)
                    findGridDecorator.FindOnMinimum();
                else
                    findGridDecorator.FindOnHalf();

                //dataGridViewFind.DataSource = findCoincidence.FindResult;
                //foreach (DataColumn dc in findCoincidence.FindResult.Columns)
                //    dataGridViewFind.Columns[dc.ColumnName].HeaderText = dc.Caption;
                //dataGridViewFind.Columns["card_type"].Visible = false;

                //dataGridViewFind.Columns[0].HeaderText += String.Format(resDataNames.formFindCountOnHeader,
                //    findCoincidence.FindResult.Rows.Count);
            }
            finally
            {
                EnableControls();
                this.Cursor = Cursors.Default;
            }
        }
        private void dataGridViewFind_RowHeightInfoNeeded(object sender, DataGridViewRowHeightInfoNeededEventArgs e)
        {
            e.Height = 16;
        }
        private void dataGridViewFind_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (Convert.ToInt32(dataGridViewFind.CurrentRow.Cells["card_type"].Value, CultureInfo.InvariantCulture))
            {
                case 0:
                    if (OnActiveIklCard != null)
                        OnActiveIklCard(new IklVocabulary(), Convert.ToInt32(dataGridViewFind.CurrentRow.Cells["profile_id"].Value, CultureInfo.InvariantCulture));
                    break;
                case 1:
                    if (OnActiveIk2Card != null)
                        OnActiveIk2Card(new Ik2Vocabulary(), Convert.ToInt32(dataGridViewFind.CurrentRow.Cells["profile_id"].Value, CultureInfo.InvariantCulture));
                    break;
            }
        }
        private void dataGridViewFind_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridViewFind.SelectedRows.Count > 1)
                {
                    listViewProfiles.Items.Clear();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                label2.Text = String.Format(resDataNames.formFindProfileTo,
                     Convert.ToInt32(dataGridViewFind.Rows[e.RowIndex].Cells["profile_id"].Value));
                int cardKind = Convert.ToInt32(dataGridViewFind.Rows[e.RowIndex].Cells["card_type"].Value,
                CultureInfo.InvariantCulture);

                Profiles profiles = new Profiles();
                profiles.Load(Convert.ToInt32(dataGridViewFind.Rows[e.RowIndex].Cells["profile_id"].Value,
                    CultureInfo.InvariantCulture), GateFactory.GLDefault().DefaultMethod);

                Tools.FillListViewProfilesData(this.profileCard, profiles, listViewProfiles);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        private void radioButtonFindDirect_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
        private void radioButtonFindHalf_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void buttonRefreshParent_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (dataGridViewFind.SelectedRows.Count == 0)
                    return;

                int cardKind = Convert.ToInt32(dataGridViewFind.SelectedRows[0].Cells["card_type"].Value, CultureInfo.InvariantCulture);
                int ProfileId = Convert.ToInt32(dataGridViewFind.SelectedRows[0].Cells["profile_id"].Value, CultureInfo.InvariantCulture);
                Profiles profile = new Profiles();
                profile.Load(ProfileId, GateFactory.GLDefault().DefaultMethod);

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void buttonOpenCard_Click(object sender, EventArgs e)
        {
            switch (this.cardKind)
            {
                case CardKind.ikl:
                    if (OnActiveIklCard != null)
                        OnActiveIklCard(new IklVocabulary(), this.CardId);
                    break;
                case CardKind.ik2:
                    if (OnActiveIk2Card != null)
                        OnActiveIk2Card(new Ik2Vocabulary(), this.CardId);
                    break;
                default:
                    break;
            }
        }
        private void buttonIdent_Click(object sender, EventArgs e)
        {
            string cardNames = (from DataGridViewRow dataGridViewRow in dataGridViewFind.SelectedRows
                                select dataGridViewRow.Cells["profile_id"].Value).OrderBy(cardId => cardId).Aggregate((currValue, next) =>
                               currValue + "\t\r" + next).ToString();
            if (Tools.ShowMessage(String.Format(resDataNames.CardItemMessage, this.CardId.ToString(), cardNames)) != DialogResult.Cancel)
            {

                if (cardKind == CardKind.ik2)
                {
                    Ik2Vocabulary ik2Vocabulary = new Ik2Vocabulary();
                    ik2Vocabulary.LoadItem(this.CardId);
                    IK2Item ik2Item = ik2Vocabulary.Item;
                    ik2Item.Open();
                    foreach (int i in (from DataGridViewRow dataGridViewRow in dataGridViewFind.SelectedRows
                                       select Convert.ToInt32(dataGridViewRow.Cells["profile_id"].Value, CultureInfo.InvariantCulture)).
                                       Except(from CardIdent cardIdent in ik2Item.CardIdent select cardIdent.CardId))
                        ik2Item.Ident(i);
                }
                else
                {
                    var rows = (from DataGridViewRow row in dataGridViewFind.SelectedRows
                                where Convert.ToInt32(row.Cells["card_type"].Value, CultureInfo.InvariantCulture) == 1
                                select row);

                    // Проверяем, есть ли среди выбранных карточек формы ИК-2
                    if (rows.Count() == 0)
                        throw new WFException(ErrType.Message, ErrorsMsg.ListNotConsistIk2);

                    Ik2Vocabulary ik2Vocabulary = new Ik2Vocabulary();
                    ik2Vocabulary.LoadItem(Convert.ToInt32(rows.First().Cells["profile_id"].Value, CultureInfo.InvariantCulture));
                    IK2Item ik2Item = ik2Vocabulary.Item;
                    ik2Item.Open();
                    ik2Item.Ident(this.CardId);
                    foreach (int i in (from DataGridViewRow dataGridViewRow in dataGridViewFind.SelectedRows
                                       where Convert.ToInt32(dataGridViewRow.Cells["profile_id"].Value, CultureInfo.InvariantCulture) != ik2Item.Id
                                       select Convert.ToInt32(dataGridViewRow.Cells["profile_id"].Value, CultureInfo.InvariantCulture)).
                                       Except(from CardIdent cardIdent in ik2Item.CardIdent select cardIdent.CardId))
                        ik2Item.Ident(i);
                }
            }
        }
        private void TextBoxValue(TextBox textBox, bool HaveValue)
        {
            if (HaveValue)
            {
                textBox.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                textBox.ForeColor = Color.Black;
            }
            else
            {
                textBox.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Italic);
                textBox.ForeColor = Color.Gray;
            }
        }
        private void buttonToExcel_Click(object sender, EventArgs e)
        {
            Button buttonExcel = sender as Button;
            if (buttonExcel != null)
            {
                switch (buttonExcel.Name)
                {
                    case "buttonDirectExcel":
                        Study.ExportToExcel(KindStudy.Probably, this.profileCard, new Point(1, 2));
                        break;
                    case "buttonOneParentExcel":
                        Study.ExportToExcel(KindStudy.OneParent, this.profileCard, new Point(1, 2));
                        break;
                    case "buttonTooPartnExcel":
                        Study.ExportToExcel(KindStudy.TooParent, this.profileCard, new Point(1, 2));
                        break;
                    default:
                        break;
                }
            }
        }
        private void buttonStudyChild_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                switch (button.Name)
                {
                    case "buttonStudyChild":
                        if (OnActiveStudyForm != null)
                            OnActiveStudyForm(CardId);
                        break;
                    case "buttonStudyParent":
                        if (dataGridViewFind.SelectedRows.Count > 0)
                        {
                            if (OnActiveStudyForm != null)
                                OnActiveStudyForm(Convert.ToInt32(dataGridViewFind.SelectedRows[0].Cells["profile_id"].Value,
                                    CultureInfo.InvariantCulture));
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        // Drag and drop
        private void dataGridViewFind_MouseDown(object sender, MouseEventArgs e)
        {
            openFormStudyOpen = Tools.IsMdiFormOpen("FormStudy");
            Size dragSize = SystemInformation.DragSize;
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                           e.Y - (dragSize.Height / 2)), dragSize);
        }
        private void dataGridViewFind_MouseUp(object sender, MouseEventArgs e)
        {
            dragBoxFromMouseDown = Rectangle.Empty;
        }
        private void dataGridViewFind_MouseMove(object sender, MouseEventArgs e)
        {
            bool leftMouseDown = (e.Button & MouseButtons.Left) == MouseButtons.Left;
            bool rigthMouseDown = (e.Button & MouseButtons.Right) == MouseButtons.Right;

            if ((leftMouseDown || rigthMouseDown) && openFormStudyOpen && dataGridViewFind.SelectedRows.Count > 0)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    screenOffset = SystemInformation.WorkingArea.Location;
                    DragDropEffects dropEffect = dataGridViewFind.DoDragDrop(
                        Convert.ToInt32(dataGridViewFind.SelectedRows[0].Cells["profile_id"].Value,
                            CultureInfo.InvariantCulture),
                        DragDropEffects.All | DragDropEffects.Link);
                }
            }
        }

        // Properties
        public int CardId { get; set; }
        public CardKind cardKind { get; set; }

        // Private members
        private Profiles profileCard = new Profiles();
        private void EnableControls()
        {
            bool emptyGrid = dataGridViewFind.RowCount == 0;
            bool checkRadioButtonFindDirect = radioButtonFindDirect.Checked;

            buttonIdent.Enabled = !emptyGrid;
            buttonStudyParent.Enabled = !emptyGrid;
            nudCountLocusesWithNoEqualAllelies.Enabled = !checkRadioButtonFindDirect;
        }
        private Rectangle dragBoxFromMouseDown;
        private Point screenOffset;
        private bool openFormStudyOpen;

        // Events
        internal event ActiveIklCard OnActiveIklCard;
        internal event ActiveIk2Card OnActiveIk2Card;
        internal event ActiveStudyForm OnActiveStudyForm;

    }
}
