using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WFExceptions;
using System.Threading;

namespace GeneLibrary
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WFException.HandleError(e);
        }
        static void OnThreadException(object sender, ThreadExceptionEventArgs t)
        {
            WFException.HandleError(t.Exception);
        }

    }
}
