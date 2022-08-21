using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TokenRenewer.Helpers;
using TokenRenewer.Models;

namespace TokenRenewer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties
        private TokenRenewerConfig _config;
        public TokenRenewerConfig Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged(nameof(Config));
            }
        }

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

        private string _renewerStatus;
        public string RenewerStatus
        {
            get { return _renewerStatus; }
            set
            {
                _renewerStatus = value;
                OnPropertyChanged(nameof(RenewerStatus));
                OnPropertyChanged(nameof(AutoRenewButtonContent));
            }
        }

        public string AutoRenewButtonContent
        {
            get 
            {
                if(RenewerStatus == null || RenewerStatus == Config.StopStatus)
                {
                    return Config.AutoRenewButtonContentWhenStop;
                }
                else
                {
                    return Config.AutoRenewButtonContentWhenStart;
                }
            }
        }

        // ScrollViewer
        string _input = string.Empty;
        ObservableCollection<string> _output;

        public string ScrollViewerInput
        {
            get { return _input; }
            set
            {
                _input = value;
                OnPropertyChanged(nameof(ScrollViewerInput));
            }
        }
        public ObservableCollection<string> ScrollViewerOutput
        {
            get { return _output; }
            set
            {
                _output = value;
                OnPropertyChanged(nameof(ScrollViewerOutput));
            }
        }
        #endregion

        #region Construction
        public MainWindowViewModel()
        {
            // Load config from DB.
            try
            {
                Config = SqlFunctions.GetRenewerConfig();
                if (Config == null)
                {
                    MessageBox.Show("Error", "Can't connect to database.");
                }
                else
                {
                    ScrollViewerOutput = new ObservableCollection<string>();
                    ScrollViewerOutput.Add(Config.ScrollViewerHeader);
                    CountDown = Config.RenewInterval;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
        #endregion

        #region Commands
        void WindowLoaded()
        {
            // Chạy Renewer nếu config AutoStartRenewer.
            if (Config.AutoStartRenewer)
            {
                Start();
            }
            else
            {
                RenewerStatus = Config.StopStatus;
            }
        }
        bool CanWindowLoadedExecute()
        {
            return true;
        }
        public ICommand Window_Loaded
        {
            get
            {
                return new RelayCommand(WindowLoaded, CanWindowLoadedExecute);
            }
        }

        #region Window_Closed
        void WindowClosed()
        {
            // Lưu config vào DB.
            SqlFunctions.TokenRenewerConfigUpdate(Config);
        }
        bool CanWindowClosedExecute()
        {
            return true;
        }
        public ICommand Window_Closed
        {
            get
            {
                return new RelayCommand(WindowClosed, CanWindowClosedExecute);
            }
        }
        #endregion

        void AutoRenewButtonClick()
        {
            if(RenewerStatus == Config.StopStatus)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }
        bool CanAutoRenewButtonClickExecute()
        {
            return true;
        }
        public ICommand AutoRenewButton_Click
        {
            get
            {
                return new RelayCommand(AutoRenewButtonClick, CanAutoRenewButtonClickExecute);
            }
        }
        #endregion

        #region INotifyPropertyChanged implement
        // INotifyPropertyChanged event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        

        public void ShowInScrollViewer()
        {
            ScrollViewerOutput.Add(ScrollViewerInput);
            // do your stuff here.
            ScrollViewerInput = string.Empty;
        }

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
            RenewerStatus = Config.StartStatus;

            Timer = new Timer();
            Timer.Interval = Config.TimerInterval;
            Timer.Elapsed += OnTimedEvent;
            Timer.Start();
        }

        public void Stop()
        {
            RenewerStatus = Config.StopStatus;
            Timer.Stop();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (CountDown-- == 0)
            {
                Timer.Stop();
                // Update Token.
                ScrollViewerInput = Renewer.Work(Config);
                App.Current.Dispatcher.Invoke(() => {
                    ShowInScrollViewer();
                    if (AutoRestartRenewer && RenewCount >= Config.RestartRenewerAfter)
                    {
                        Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                });
                CountDown = Config.RenewInterval;
                Timer.Start();
            }
        }
    }
}
