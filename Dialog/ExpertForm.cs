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
using GeneLibrary.Items;
using GeneLibrary.Common;
using System.Globalization;


namespace GeneLibrary.Dialog
{
    public partial class ExpertForm : Form
    {
        // Constructors
        public ExpertForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this.dictionary = (ExpertVocabulary)dictionary;
        }
        public ExpertForm(Vocabulary dictionary, int id) : this(dictionary)
        {
            this.IsEdit = true;
            this.id = id;
        }

        // Event handers
        private void ExpDlg_Load(object sender, EventArgs e)
        {
            if (this.IsEdit)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.dictionary.LoadItem(this.id);
                    this.item = this.dictionary.Item;
                    this.item.Open();
                    this.tbSurname.Text = this.item.Surname;
                    this.tbName.Text = this.item.Name;
                    this.tbPatronic.Text = this.item.Patronymic;
                    this.tbLogin.Text = this.item.LogOn;
                    this.tbSign.Text = this.item.Sign;
                    cbDiv.Items.AddRange(
                        (from DataRow row in this.item.DTDivision.Rows 
                        select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture), row["name"].ToString())
                        ).ToArray<ComboBoxItem>());
                    cbPost.Items.AddRange(
                        (from DataRow row in this.item.DTPost.Rows
                        select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture), row["name"].ToString())
                        ).ToArray<ComboBoxItem>());

                    cbDiv.SelectedIndex = cbDiv.FindString(this.item.DivisionName);
                    cbPost.SelectedIndex = cbPost.FindString(this.item.PostName);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                this.dictionary.LoadItem(this.id);
                this.item = this.dictionary.Item;
                this.item.Open();
                cbDiv.Items.AddRange(
                    (from DataRow row in this.item.DTDivision.Rows
                     select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture), row["name"].ToString())
                    ).ToArray<ComboBoxItem>());
                if (cbDiv.Items.Count > 0)
                    cbDiv.SelectedIndex = 0;
                cbPost.Items.AddRange(
                    (from DataRow row in this.item.DTPost.Rows
                     select new ComboBoxItem(Convert.ToInt32(row["id"], CultureInfo.InvariantCulture), row["name"].ToString())
                    ).ToArray<ComboBoxItem>());
                if (cbPost.Items.Count > 0)
                    cbPost.SelectedIndex = 0;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (String.IsNullOrEmpty(cbDiv.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyExpDivField);
            if (String.IsNullOrEmpty(cbPost.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyExpPostField);
            if (String.IsNullOrEmpty(tbSurname.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyExpSurnameField);
            if (String.IsNullOrEmpty(tbName.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyExpNameField);
            if (String.IsNullOrEmpty(tbPatronic.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyExpPatronicField);
            if (String.IsNullOrEmpty(tbLogin.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyExpLoginField);

            this.item.DivisionId = ((ComboBoxItem)cbDiv.SelectedItem).Id;
            this.item.PostId = ((ComboBoxItem)cbPost.SelectedItem).Id;
            this.item.Surname = tbSurname.Text.Trim();
            this.item.Name = tbName.Text.Trim();
            this.item.Patronymic = tbPatronic.Text.Trim();
            this.item.LogOn = tbLogin.Text.Trim();
            this.item.Sign = tbSign.Text.Trim();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                int _id = this.item.Save();
                if (passwordChanged)
                    this.item.ChangePassword();
                if (OnDataLoad != null)
                    OnDataLoad(_id);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            if (this.IsEdit)
                this.DialogResult = DialogResult.OK;
            else
            {
                tbSurname.Clear();
                tbName.Clear();
                tbPatronic.Clear();
                tbLogin.Clear();
                tbSign.Clear();
                cbDiv.Focus();
                this.dictionary.LoadItem();
                this.item = this.dictionary.Item;
            }
        }
        private void btnRight_Click(object sender, EventArgs e)
        {
            UserRightForm userRightDlg = new UserRightForm(this.item);
            userRightDlg.ShowDialog();
        }
        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            FormChangePassword formChangePassword = new FormChangePassword();
            if (formChangePassword.ShowDialog() == DialogResult.OK)
            {
                this.item.Password = formChangePassword.Password;
                passwordChanged = true;
            }
        }

        // Properties
        public bool IsEdit { get; set; }

        // Private members
        private int id;
        private ExpertItem item;
        private ExpertVocabulary dictionary;
        private bool passwordChanged;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
