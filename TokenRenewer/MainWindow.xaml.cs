using System;
using System.Collections.Generic;
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
using TokenRenewer.ViewModels;

namespace TokenRenewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel VM;
        public MainWindow()
        {
            InitializeComponent();
            VM = (MainWindowViewModel)this.DataContext;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VM.AutoStartRenewer = bool.Parse(VM.AppSettings["AutoStartRenewer"]);
            VM.AutoRestartRenewer = bool.Parse(VM.AppSettings["AutoRestartRenewer"]);
            
            if (bool.Parse(VM.AppSettings["AutoStartRenewer"]))
            {
                VM.Start();
            }
            else
            {
                VM.RenewerStatus = VM.AppSettings["StopStatus"];
            }
        }

        //void InputBlock_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        MainWindowViewModel.ScrollViewerInput = InputBlock.Text;
        //        MainWindowViewModel.RunCommand();
        //        InputBlock.Focus();
        //        Scroller.ScrollToBottom();
        //    }
        //}

        private void AutoRenewButton_Click(object sender, RoutedEventArgs e)
        {
            if(VM.RenewerStatus == VM.AppSettings["StopStatus"])
            {
                VM.Start();
            }
            else
            {
                VM.Stop();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            VM.UpdateConfig();
        }
    }
}
