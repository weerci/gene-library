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
    public partial class AddEditRight : Form
    {
        // Constructors
        public AddEditRight(RoleItem item)
        {
            InitializeComponent();
            _item = item;
        }

        // Events handlers
        private void AddEditRight_Load(object sender, EventArgs e)
        {
            // Доступные для назначения права
            listBoxAllRight.Items.Clear();
            foreach (ComboBoxItem cbi in _item.AllRight)
                listBoxAllRight.Items.Add(cbi);

            // Назначенные права
            listBoxAcceptRight.Items.Clear();
            foreach (ComboBoxItem cbi in _item.AcceptRight)
                listBoxAcceptRight.Items.Add(cbi);
        }
        private void btnToAccept_Click(object sender, EventArgs e)
        {
            while (listBoxAllRight.SelectedItems.Count > 0)
            {
                listBoxAcceptRight.Items.Add(listBoxAllRight.SelectedItems[0]);
                listBoxAllRight.Items.Remove(listBoxAllRight.SelectedItems[0]);
            }
            SelectedIndexChanged(listBoxAllRight, null);
        }
        private void btnRevoke_Click(object sender, EventArgs e)
        {
            while (listBoxAcceptRight.SelectedItems.Count > 0)
            {
                listBoxAllRight.Items.Add(listBoxAcceptRight.SelectedItems[0]);
                listBoxAcceptRight.Items.Remove(listBoxAcceptRight.SelectedItems[0]);
            }
            SelectedIndexChanged(listBoxAcceptRight, null);
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            _item.AllRight.Clear();
            _item.AcceptRight.Clear();
            foreach (ComboBoxItem cbi in listBoxAllRight.Items)
                _item.AllRight.Add(cbi);
            foreach (ComboBoxItem cbi in listBoxAcceptRight.Items)
                _item.AcceptRight.Add(cbi);

            ComboBoxItem[] comboBoxItems = new ComboBoxItem[listBoxAcceptRight.Items.Count];
            listBoxAcceptRight.Items.CopyTo(comboBoxItems, 0);
            if (OnAcceptRight != null)
                OnAcceptRight(comboBoxItems);

            this.DialogResult = DialogResult.OK;
        }
        private void lbxAcceptRight_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnRevoke_Click(null, null);
        }
        private void lbxAllRight_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnToAccept_Click(null, null);
        }
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                if (listBox.Name == "listBoxAllRight")
                    buttonAccept.Enabled = listBoxAllRight.SelectedItems.Count > 0;
                else
                    if (listBox.Name == "listBoxAcceptRight")
                        buttonRevoke.Enabled = listBoxAcceptRight.SelectedItems.Count > 0;
            }
        }

        // Private
        private RoleItem _item;

        // Event
        internal delegate void AcceptRight(ComboBoxItem[] cbi);
        internal event AcceptRight OnAcceptRight;


    }
}
