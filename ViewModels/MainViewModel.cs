using AndersL.EtherHunter.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;

namespace AndersL.EtherHunter.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public static System.Timers.Timer Timer
        {
            get
            {
                return _Timer;
            }
        }

        public CommandHandler StartCommand
        {
            get
            {
                return new CommandHandler(() => Start(), () => true);
            }
        }

        public CommandHandler ResetCommand
        {
            get
            {
                return new CommandHandler(() => Reset(), () => true);
            }
        }

        public ObservableCollection<ObservableTuple> GeneratedKeys
        {
            get
            {
                return _GeneratedKeys;
            }
        }

        public static Random Random
        {
            get
            {
                return _Random;
            }
        }

        public string TotalKeysGeneratedString
        {
            get
            {
                return "Total Keys Generated: " + TotalKeysGeneratedThisSession;
            }
        }

        public uint TotalKeysGenerated
        {
            get
            {
                return _TotalKeysGenerated;
            }
            set
            {
                _TotalKeysGenerated = value;
                OnPropertyChanged(nameof(TotalKeysGenerated));
                OnPropertyChanged(nameof(TotalKeysGeneratedString));
            }
        }

        public string TotalTime
        {
            get
            {
                TimeSpan time = TimeSpan.FromSeconds(TotalSeconds);
                return "Time elapsed: " + time.ToString(@"hh\:mm\:ss");
            }
        }

        public uint ThreadCount
        {
            get
            {
                return _ThreadCount;
            }
            set
            {
                _ThreadCount = value;
                OnPropertyChanged(nameof(ThreadCount));
            }
        }

        public uint GweiCount
        {
            get
            {
                return _GweiCount;
            }
            set
            {
                _GweiCount = value;
                OnPropertyChanged(nameof(GweiCount));
                OnPropertyChanged(nameof(EtherCount));
            }
        }

        public string EtherCount
        {
            get
            {
                return "≈ " + Math.Round(GweiCount * 0.000000001, 8) + " ETH";
            }
        }


        public string KeysGeneratedPerSecondString
        {
            get
            {
                if(_TotalSeconds > 0)
                {
                    return "Keys Generated / Second: " + Math.Round((_TotalKeysGeneratedThisSession / (_TotalSeconds)), 2);
                }
                else
                {
                    return "Keys Generated / Second: 0";
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _IsRunning;
            }
            set
            {
                _IsRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

        public bool IsStoppingWhenKeyFound
        {
            get
            {
                return _IsStoppingWhenKeyFound;
            }
            set
            {
                _IsStoppingWhenKeyFound = value;
                OnPropertyChanged(nameof(IsStoppingWhenKeyFound));
            }
        }

        public double TotalKeysGeneratedThisSession
        {
            get
            {
                return _TotalKeysGeneratedThisSession;
            }
            set
            {
                _TotalKeysGeneratedThisSession = value;
                OnPropertyChanged(nameof(KeysGeneratedPerSecondString));
            }
        }

        public double TotalSeconds
        {
            get
            {
                return _TotalSeconds;
            }
            set
            {
                _TotalSeconds = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }


        public ObservableCollection<AndersL.EtherHunter.Helpers.ObservableTuple> SavedKeys
        {
            get
            {
                return _SavedKeys;
            }
            set
            {
                _SavedKeys = value;
                OnPropertyChanged(nameof(SavedKeys));
                OnPropertyChanged(nameof(TotalEthString));
                OnPropertyChanged(nameof(TotalKeysFoundWithBalance));
            }
        }

        public string TotalKeysFoundWithBalance
        {
            get
            {
                return "Total Keys Found with balance: " + SavedKeys.Count;
            }
        }

        public string TotalEthString
        {
            get
            {
                return "Total ETH: " + TotalEth;
            }
        }

        public double TotalEth
        {
            get
            {
                return _TotalEth;
            }
            set
            {
                _TotalEth = value;
                OnPropertyChanged(nameof(TotalEthString));
            }
        }

        private static System.Timers.Timer _Timer;
        private static Random _Random;
        private ObservableCollection<AndersL.EtherHunter.Helpers.ObservableTuple> _GeneratedKeys;
        private ObservableCollection<AndersL.EtherHunter.Helpers.ObservableTuple> _SavedKeys;
        private AndersL.EtherHunter.Helpers.EtherscanBalanceChecker _EtherscanBalanceChecker;
        private bool _IsRunning;
        private uint _ThreadCount;
        private uint _GweiCount;
        private bool _IsStoppingWhenKeyFound;
        private uint _TotalKeysGenerated;
        private double _TotalKeysGeneratedThisSession;
        private double _TotalSeconds;
        private double _TotalEth;

        public MainViewModel(Window mainWindow)
        {
            IsRunning = false;
            TotalKeysGenerated = 0;
            _Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += Timer_Elapsed;
            _Random = new Random();
            _GeneratedKeys = new ObservableCollection<AndersL.EtherHunter.Helpers.ObservableTuple>();
            SavedKeys = new ObservableCollection<AndersL.EtherHunter.Helpers.ObservableTuple>();
            _EtherscanBalanceChecker = new AndersL.EtherHunter.Helpers.EtherscanBalanceChecker(this);
            TotalKeysGeneratedThisSession = 0;
            TotalSeconds = 0;
            TotalEth = 0;

            LoadValues();
        }

        public void SaveValues()
        {
            Properties.Settings.Default.ThreadCount = ThreadCount;
            Properties.Settings.Default.GweiCount = GweiCount;
            Properties.Settings.Default.IsStoppingWhenKeyFound = IsStoppingWhenKeyFound;
            Properties.Settings.Default.Save();
        }

        public void LoadValues()
        {
            ThreadCount = Properties.Settings.Default.ThreadCount;
            GweiCount = Properties.Settings.Default.GweiCount;
            IsStoppingWhenKeyFound = Properties.Settings.Default.IsStoppingWhenKeyFound;
        }

        public string GetKeyHex(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            Random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
            {
                return result;
            }
            return result + Random.Next(16).ToString("X");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TotalSeconds += 1;
        }

        private void Start()
        {
            _EtherscanBalanceChecker.Start();
        }

        private void Reset()
        {
            Properties.Settings.Default.Reset();
            LoadValues();
        }
    }
}
