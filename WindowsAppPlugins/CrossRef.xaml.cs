using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowsAppPlugins
{
    /// <summary>
    /// Interaction logic for CrossRef.xaml
    /// </summary>
    public partial class CrossRef : UserControl
    {
        public CrossRef()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "csv";
            dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.ShowDialog();
            button.Content = dialog.FileName;

        }
    }
}
