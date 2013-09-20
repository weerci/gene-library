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
    public partial class AlleleForm : Form
    {
        public AlleleForm(Profiles profiles)
        {
            InitializeComponent();
            this.profile = profiles;
        }

        // Events handlers
        private void AlleleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        // Private methods
        private void AlleleForm_Load(object sender, EventArgs e)
        {
            listBoxLocus.DisplayMember = "name";
            listBoxLocus.Items.AddRange(
                (from Locus locus in profile.Locus
                 orderby locus.Name 
                 select locus).ToArray<Locus>());
        }
        private void listBoxLocus_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBoxAllele.DisplayMember = "name";
            checkedListBoxAllele.Items.Clear();
            Locus locus = listBoxLocus.SelectedItem as Locus;
            if (locus != null)
            {
                var result = from Allele allele in locus.Allele
                             orderby allele.Value
                             select allele;
                foreach (Allele resultAllele in result)
                    checkedListBoxAllele.Items.Add(resultAllele, resultAllele.Checked);
            }
        }
        private void checkedListBoxAllele_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool condition = e.NewValue == CheckState.Checked;
            profile.CheckAlleleInLocus((Locus)listBoxLocus.SelectedItem, 
                (Allele)checkedListBoxAllele.Items[e.Index], condition);
        }

        // Fields
        Profiles profile;

    }
}
