using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using WFExceptions;
using GeneLibrary;

namespace GeneLibrary.Dialog
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowErrorProfileFile : Window
    {
        public WindowErrorProfileFile(string pathFile)
        {
            InitializeComponent();
            this.pathFile = pathFile;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try 
	        {
		        Process.Start("notepad.exe", pathFile);
	        }
	        catch 
	        {
                throw new WFException(ErrType.Message, String.Format(ErrorsMsg.FileNotFound, pathFile));
	        }
        }
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Fields
        private string pathFile;
    }
}
