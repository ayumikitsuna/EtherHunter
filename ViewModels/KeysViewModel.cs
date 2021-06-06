using System.Collections.ObjectModel;

namespace AndersL.EtherHunter.ViewModels
{
    public class KeysViewModel : ViewModel
    {
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

        private ObservableCollection<AndersL.EtherHunter.Helpers.ObservableTuple> _SavedKeys;
        private double _TotalEth;

        public KeysViewModel()
        {
            _SavedKeys = new ObservableCollection<Helpers.ObservableTuple>();
            _TotalEth = 0;
        }
    }
}
