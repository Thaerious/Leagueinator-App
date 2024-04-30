using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace SortableCardContainer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            this.DispatcherUnhandledException += this.HndUIException;
            AppDomain.CurrentDomain.UnhandledException += this.HndDomainException;
        }

        private void HndDomainException(object sender, UnhandledExceptionEventArgs e) {
            Exception ex = e.ExceptionObject as Exception;
            Debug.WriteLine(ex.ToString());
            MessageBox.Show("A non-UI thread exception occurred: " + ex.Message, "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void HndUIException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            Debug.WriteLine(e.Exception.ToString());
            string msg = "An unhandled exception occurred:\n" + e.Exception.Message;

            if (e.Exception.InnerException is not null) {
                msg += "\n\n" + e.Exception.InnerException.Message;
            }

            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}

