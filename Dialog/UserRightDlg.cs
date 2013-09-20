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

namespace GeneLibrary.Dialog
{
    public partial class UserRightForm : Form
    {
        // Constructors
        public UserRightForm(ExpertItem expertItem)
        {
            InitializeComponent();
            this.expertItem = expertItem;
        }

        // Events handlers
        private void UserRightDlg_Load(object sender, EventArgs e)
        {
            listBoxAllRole.Items.AddRange(expertItem.AllRoles.ToArray<ComboBoxItem>());
            listBoxAcceptRole.Items.AddRange(expertItem.AcceptRoles.ToArray<ComboBoxItem>());
            listBoxAllRight.Items.AddRange(expertItem.AllRight.ToArray<ComboBoxItem>());
            listBoxAcceptRight.Items.AddRange(expertItem.AcceptRight.ToArray<ComboBoxItem>());
            listBoxRevokeRight.Items.AddRange(expertItem.RevokeRight.ToArray<ComboBoxItem>());
        }
        private void btnRoleToAccept_Click(object sender, EventArgs e)
        {
            while (listBoxAllRole.SelectedItems.Count > 0)
            {
                listBoxAcceptRole.Items.Add(listBoxAllRole.SelectedItems[0]);
                listBoxAllRole.Items.Remove(listBoxAllRole.SelectedItems[0]);
            }
        }
        private void btnRoleToRevoke_Click(object sender, EventArgs e)
        {
            while (listBoxAcceptRole.SelectedItems.Count > 0)
            {
                listBoxAllRole.Items.Add(listBoxAcceptRole.SelectedItems[0]);
                listBoxAcceptRole.Items.Remove(listBoxAcceptRole.SelectedItems[0]);
            }
        }
        private void lbxAllRole_DoubleClick(object sender, EventArgs e)
        {
            btnRoleToAccept_Click(null, null);
        }
        private void lbxAcceptRole_DoubleClick(object sender, EventArgs e)
        {
            btnRoleToRevoke_Click(null, null);
        }
        private void btnRightToAccept_Click(object sender, EventArgs e)
        {
            while (listBoxAllRight.SelectedItems.Count > 0)
            {
                listBoxAcceptRight.Items.Add(listBoxAllRight.SelectedItems[0]);
                listBoxAllRight.Items.Remove(listBoxAllRight.SelectedItems[0]);
            }
        }
        private void btnRightToRevoke_Click(object sender, EventArgs e)
        {
            while (listBoxAcceptRight.SelectedItems.Count > 0)
            {
                listBoxAllRight.Items.Add(listBoxAcceptRight.SelectedItems[0]);
                listBoxAcceptRight.Items.Remove(listBoxAcceptRight.SelectedItems[0]);
            }
        }
        private void lbxAcceptRightUser_DoubleClick(object sender, EventArgs e)
        {
            btnRightToRevoke_Click(null, null);
        }
        private void btnAcceptRevoke_Click(object sender, EventArgs e)
        {
            while (listBoxAllRight.SelectedItems.Count > 0)
            {
                listBoxRevokeRight.Items.Add(listBoxAllRight.SelectedItems[0]);
                listBoxAllRight.Items.Remove(listBoxAllRight.SelectedItems[0]);
            }
        }
        private void RevokeRevoke_Click(object sender, EventArgs e)
        {
            while (listBoxRevokeRight.SelectedItems.Count > 0)
            {
                listBoxAllRight.Items.Add(listBoxRevokeRight.SelectedItems[0]);
                listBoxRevokeRight.Items.Remove(listBoxRevokeRight.SelectedItems[0]);
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            expertItem.AllRoles.Clear();
            expertItem.AcceptRoles.Clear();
            expertItem.AllRight.Clear();
            expertItem.AcceptRight.Clear();
            expertItem.RevokeRight.Clear();

            foreach (ComboBoxItem cbi in listBoxAllRole.Items)
                expertItem.AllRoles.Add(cbi);
            foreach (ComboBoxItem cbi in listBoxAllRight.Items)
                expertItem.AllRight.Add(cbi);
            foreach (ComboBoxItem cbi in listBoxAcceptRole.Items)
                expertItem.AcceptRoles.Add(cbi);
            foreach (ComboBoxItem cbi in listBoxAcceptRight.Items)
                expertItem.AcceptRight.Add(cbi);
            foreach (ComboBoxItem cbi in listBoxRevokeRight.Items)
                expertItem.RevokeRight.Add(cbi);

            this.DialogResult = DialogResult.OK;
        }
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                switch (listBox.Name)
                {
                    case "listBoxAllRole":
                        buttonRoleAccept.Enabled = listBoxAllRole.SelectedItems.Count > 0;
                        return;
                    case "listBoxAcceptRole":
                        buttonRevokeRole.Enabled = listBoxAcceptRole.SelectedItems.Count > 0;
                        return;
                    case "listBoxAllRight":
                        buttonRightAccept.Enabled = listBoxAllRight.SelectedItems.Count > 0;
                        buttonRevokeRight.Enabled = listBoxAllRight.SelectedItems.Count > 0;
                        return;
                    case "listBoxAcceptRight":
                        buttonAcceptRightRevoke.Enabled = listBoxAcceptRight.SelectedItems.Count > 0;
                        return;
                    case "listBoxRevokeRight":
                        buttonRevokeRevoke.Enabled = listBoxRevokeRight.SelectedItems.Count > 0;
                        return;
                    default:
                        break;
                }
            }
        }
        
        // Private members
        ExpertItem expertItem;

    }
}
