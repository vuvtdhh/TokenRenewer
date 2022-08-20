using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace TokenRenewer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implement
        // INotifyPropertyChanged event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        // ScrollViewer
        string Input = string.Empty;
        ObservableCollection<string> Output = new ObservableCollection<string>() { "TOKEN AUTO RENEWER LOG:" };

        public string ScrollViewerInput
        {
            get { return Input; }
            set
            {
                Input = value;
                OnPropertyChanged(nameof(ScrollViewerInput));
            }
        }
        public ObservableCollection<string> ScrollViewerOutput
        {
            get { return Output; }
            set
            {
                Output = value;
                OnPropertyChanged(nameof(ScrollViewerOutput));
            }
        }

        public void RunCommand()
        {
            ScrollViewerOutput.Add(ScrollViewerInput);
            // do your stuff here.
            ScrollViewerInput = String.Empty;
        }

        // Order control
        private int _countDown;
        public int CountDown
        {
            get { return _countDown; }
            set
            {
                _countDown = value;
                OnPropertyChanged(nameof(CountDown));
            }
        }

        public string RenewerStatus { get; set; }
        public NameValueCollection AppSettings = ConfigurationManager.AppSettings;
        private Timer Timer { get; set; }
        private int RenewCount { get; set; }

        private bool _autoRestartRenewer;
        public bool AutoRestartRenewer
        {
            get { return _autoRestartRenewer; }
            set
            {
                _autoRestartRenewer = value;
                OnPropertyChanged(nameof(AutoRestartRenewer));
            }
        }

        private bool _autoStartRenewer;
        public bool AutoStartRenewer
        {
            get { return _autoStartRenewer; }
            set
            {
                _autoStartRenewer = value;
                OnPropertyChanged(nameof(AutoStartRenewer));
            }
        }

        public void Start()
        {
            RenewerStatus = AppSettings["StartStatus"];
            AutoRenewButtonContent = AppSettings["AutoRenewButtonContentWhenStart"];
            CountDown = int.Parse(AppSettings["RenewInterval"]);

            Timer = new Timer();
            Timer.Interval = 1000;
            Timer.Elapsed += OnTimedEvent;
            Timer.Start();
        }

        public void Stop()
        {
            RenewerStatus = AppSettings["StopStatus"];
            AutoRenewButtonContent = AppSettings["AutoRenewButtonContentWhenStop"];
            Timer.Stop();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (CountDown-- == 0)
            {
                ScrollViewerInput = string.Format("Token has been renew at {0}. Order: {1}.", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), ++RenewCount);
                App.Current.Dispatcher.Invoke(() => {
                    RunCommand();
                    //NameValueCollection appSettings = ConfigurationManager.AppSettings;
                    if (AutoRestartRenewer && RenewCount >= int.Parse(AppSettings["RestartRenewerAfter"]))
                    {
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                });
                CountDown = 5;
            }
        }

        private string _autoRenewButtonContent;
        public string AutoRenewButtonContent
        {
            get { return _autoRenewButtonContent; }
            set
            {
                _autoRenewButtonContent = value;
                OnPropertyChanged(nameof(AutoRenewButtonContent));
            }
        }

        //
        public void UpdateConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["AutoStartRenewer"].Value = AutoStartRenewer.ToString();
            config.AppSettings.Settings["AutoRestartRenewer"].Value = AutoRestartRenewer.ToString();
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
