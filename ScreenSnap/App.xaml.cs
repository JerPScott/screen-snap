using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace ScreenSnap
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Process proc = Process.GetCurrentProcess();
            if (Process.GetProcesses().Where(p => p.ProcessName == proc.ProcessName).Count() > 1)
            {
                MessageBox.Show("Already an instance is running...");
                App.Current.Shutdown(); 
            }
       }
    }
}
