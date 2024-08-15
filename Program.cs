using System;
using System.Windows.Forms;

namespace AutomateMailingOfBirForm {
    internal static class Program {
        private const int V = 0x7E9;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            if (!ValidLicense()) return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static bool ValidLicense() {
            if (DateTime.Now.Year < V)
            {
                return true;
            }
            else
            {
                MessageBox.Show(
                    "Please contact the programer to continue using his program " +
                    "at daviduntalan@gmail.com or +63(927)414-1835. Thank you.",
                    "Oppsss! This is an intellectual property",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop
                );
                return false;
            }
        }
    }
}
