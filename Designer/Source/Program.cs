namespace Leagueinator.Designer {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            AllocConsole();
            Console.WriteLine("Designer");

            try {
                ApplicationConfiguration.Initialize();
                Application.Run(new PrinterLayoutDesigner());
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}
