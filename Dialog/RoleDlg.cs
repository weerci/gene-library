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

namespace GeneLibrary.Dialog
{
    public partial class RoleForm : Form
    {
        // Constructors
        public RoleForm(Vocabulary dictionary)
        {
            InitializeComponent();
            this.roleVocabulary = (RoleVocabulary)dictionary;
        }
        public RoleForm(Vocabulary dictionary, int id) : this(dictionary)
        {
            this.IsEdit = true;
            this.id = id;
        }

        // Event handlers
        private void RoleDlg_Load(object sender, EventArgs e)
        {
            if (this.IsEdit)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    roleVocabulary.LoadItem(id);
                    roleItem = roleVocabulary.Item;
                    roleItem.Open();
                    this.textBoxRole.Text = roleItem.Name;

                    LoadCbxRoles();
                    LoadAcceptRight();

                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                roleVocabulary.LoadItem(id);
                roleItem = roleVocabulary.Item;
                roleItem.Open();
                LoadCbxRoles();
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Проверка заполненности полей
            if (String.IsNullOrEmpty(textBoxRole.Text.Trim()))
                throw new WFException(ErrType.Message, ErrorsMsg.EmptyRoleNameField);

            roleItem.Name = textBoxRole.Text.Trim();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                id = roleItem.Save();
                if (OnDataLoad != null)
                    OnDataLoad(id);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            if (this.IsEdit)
                this.DialogResult = DialogResult.OK;
            else
            {
                textBoxRole.Clear();
                listBoxAcceptRight.Items.Clear();
                textBoxRole.Focus();
                roleVocabulary.LoadItem();
                roleItem = roleVocabulary.Item;
            }

        }
        private void btnSetPermission_Click(object sender, EventArgs e)
        {
            AddEditRight addEditRight = new AddEditRight(roleItem);
            addEditRight.OnAcceptRight += new AddEditRight.AcceptRight(LoadRight);
            addEditRight.ShowDialog();
        }

        // Property
        public bool IsEdit { get; set; }

        // Private methods
        private void LoadCbxRoles()
        {
            comboBoxOnRoles.DisplayMember = "Name";
            foreach (ComboBoxItem cbi in roleItem.Roles)
                comboBoxOnRoles.Items.Add(cbi);
            if (comboBoxOnRoles.Items.Count > 0)
                comboBoxOnRoles.SelectedIndex = 0;
        }
        private void LoadAcceptRight()
        {
            listBoxAcceptRight.Items.Clear();
            listBoxAcceptRight.Items.AddRange(roleItem.AcceptRight.ToArray());
        }
        private void LoadRight(ComboBoxItem[] cbi)
        {
            listBoxAcceptRight.Items.Clear();
            listBoxAcceptRight.Items.AddRange(cbi);
        }

        // Private fields
        private int id;
        private RoleItem roleItem;
        private RoleVocabulary roleVocabulary;

        // Events
        internal event GeneLibrary.Common.UpdateId OnDataLoad;

    }
}
