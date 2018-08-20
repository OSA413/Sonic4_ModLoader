using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /*https://stackoverflow.com/questions/4206963/how-to-add-nested-registry-key-value-in-c
         * https://stackoverflow.com/questions/1925224/c-sharp-registry-setting
         * https://msdn.microsoft.com/en-us/ie/aa767914(v=vs.94)
         */
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form f1 = new Form1();
            Application.Run(f1);
        }
    }
}
