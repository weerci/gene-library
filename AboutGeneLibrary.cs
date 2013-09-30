using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WFExceptions;

namespace GeneLibrary
{
    partial class AboutGeneLibrary : Form
    {
        public AboutGeneLibrary()
        {
            InitializeComponent();
            this.Text = String.Format("{0} ", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            string[] arrayVersion = AssemblyVersion.Split('.');
            this.labelVersion.Text = String.Format("Версия {0} ", String.Format("{0}.{1}.{2}.{3}", arrayVersion[0], arrayVersion[1], arrayVersion[2], arrayVersion[3]));
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (String.IsNullOrEmpty(titleAttribute.Title))
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length != 0)
                {
                    StringBuilder s = new StringBuilder(((AssemblyDescriptionAttribute)attributes[0]).Description + "\r\n\r\n" + "Список ресурсов:\r\n");

                    foreach (var refAsmName in Assembly.GetEntryAssembly().GetReferencedAssemblies())
                    {
                        s.Append(Assembly.Load(refAsmName).FullName + "\r\n");
                    }
                    return s.ToString();
                }
                else
                    return "";
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void btnVersion_Click(object sender, EventArgs e)
        {
            string filePath = Path.ChangeExtension(Application.ExecutablePath, ".ver");
            try
            {
                Process.Start("notepad.exe", filePath);
            }
            catch
            {
                throw new WFException(ErrType.Message, String.Format(ErrorsMsg.FileNotFound, filePath));
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://googledrive.com/host/0B49CBXu70uZAbmlid0NYaGxwQ0k/");
        }

 
    }
}
