using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace generic_object_client_cs
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // NB: removed the following line so that I get a global
            // variable (formMain) that I can access from the Client
            // class as 'callbacks'.
            // Application.Run(new FormMain());

            formMain = new FormMain();
            Application.Run(formMain);
        }

        public static FormMain formMain;
    }
}
