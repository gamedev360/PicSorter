using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

/**
 * PicSorter
 * (c)2018 William Wood Harter All Rights Reserved
 */

namespace PicSorter
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
            Application.Run(new Form1());
        }
    }
}
