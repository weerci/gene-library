using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WFExceptions;
using GeneLibrary.Items;
using GeneLibrary.Common;

namespace GeneLibrary.Dialog
{
    public partial class FileListForm : Form
    {
        public FileListForm(string fileName)
        {
            this.fileName = fileName;
            InitializeComponent();
        }

        // Events handlers
        private void FileListForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format(resDataNames.profilesFromFile, fileName);
            LoadProfilesFromFile();
            CreateProfileList();
            foreach (Profiles profiles in this.profileList)
            {
                listBoxProfiles.Items.Add(profiles.Name);
            }
        }
        private void listBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewProfile.Items.Clear();
            foreach (Locus locus in profileList.Find((item)=>(item.Name == listBoxProfiles.SelectedItem.ToString())).
                Locus.Where(locus => locus.CheckedAlleleCount > 0).OrderBy(locus => locus.Name))
            {
                ListViewItem listItem = listViewProfile.Items.Add(locus.Name);
                listItem.UseItemStyleForSubItems = false;
                listItem.Font = new Font("Arial", 10, FontStyle.Bold);
                listItem.SubItems.AddRange((from Allele allele in locus.Allele where allele.Checked orderby Tools.AlleleConvert(allele.Name) select allele.Name).ToArray<string>());
            }
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Profile = profileList.Find((item) => (item.Name == listBoxProfiles.SelectedItem.ToString()));
            this.DialogResult = DialogResult.OK;
        }
        
        // Private methods
        private void LoadProfilesFromFile() 
        {
            if (!File.Exists(fileName))
                throw new WFException(ErrType.Message, String.Format(ErrorsMsg.FileNotFound, fileName));

            try
            {
                using (StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding(Properties.Settings.Default.CodePage))) 
                {
                    String input;
                    stringList.Clear();
                    while ((input = sr.ReadLine()) != null)
                        stringList.Add(new List<string>(from string s in input.Split(new Char[] { ',', ';' })
                                                        select s.ToUpper().Trim()));
                    sr.Close();
                }
            }
            catch (Exception err)
            {
                throw new WFException(ErrType.Message, err.Message);
            }
        }
        private void CreateProfileList()
        {
            #region Нахождение значимых позиций в загруженных из файла профилях
            
            stringPosition.Clear();

            int position = stringList[0].IndexOf("SAMPLE NAME");
            if (position != -1)
                stringPosition.Add(position);
            else
            {
                new WindowErrorProfileFile(fileName).ShowDialog();
                return;
            }
            position = stringList[0].IndexOf("MARKER");
            if (position != -1)
                stringPosition.Add(position);
            else
            {
                new WindowErrorProfileFile(fileName).ShowDialog();
                return;
            }
            for (int i = 0; i < 9; i++)
            {
                position = stringList[0].IndexOf(String.Format("ALLELE {0}", i));
                if (position != -1)
                    stringPosition.Add(position);
            }
            #endregion

            #region Создание профилей

            Profiles profile = new Profiles(stringList[1][0]);
            profile.Load();
            
            string nameProfile = stringList[1][0];
            for (int i = 1; i < stringList.Count(); i++ )
            {
                if (nameProfile != stringList[i][0])
                {
                    profileList.Add(profile);
                    nameProfile = stringList[i][0];
                    profile = new Profiles(stringList[i][0]);
                    profile.Load();
                }
                for (int j = 2; j < stringPosition.Count; j++)
			    {
                    Locus locus = profile[stringList[i][1]];
                    if (locus != null)
                    {
                        Allele allele = locus[stringList[i][j]];
                        if (allele != null)
                            profile.CheckAlleleInLocus(locus, allele, true);
                    }
    			}
            }
            profileList.Add(profile);
            #endregion
        }
        public Profiles Profile { get; set; }

        // Fields
        private string fileName;
        private List<List<string>> stringList = new List<List<string>>();
        private List<int> stringPosition = new List<int>();
        private List<Profiles> profileList = new List<Profiles>();
       
    }
}
