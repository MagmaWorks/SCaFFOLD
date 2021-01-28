using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Calcs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssemblies);
            MainWindow mainWindow = new MainWindow();
            MainWindow.Show();
        }

        private System.Reflection.Assembly ResolveAssemblies(object sender, ResolveEventArgs args)
        {
            string assemblyPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/Magma Works/Scaffold/Libraries/" + args.Name + ".dll";
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }


    }
}
