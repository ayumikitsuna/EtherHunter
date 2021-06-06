using AndersL.EtherHunter.ViewModels;

namespace AndersL.EtherHunter.Helpers
{
    public class ObservableTuple : ViewModel
    {
        public string Item1
        {
            get
            {
                return _Item1;
            }
            set
            {
                _Item1 = value;
                OnPropertyChanged(nameof(Item1));
            }
        }

        public string Item2
        {
            get
            {
                return _Item2;
            }
            set
            {
                _Item2 = value;
                OnPropertyChanged(nameof(Item2));
            }
        }

        private string _Item1;
        private string _Item2;

        public ObservableTuple(string item1, string item2)
        {
            _Item1 = item1;
            _Item2 = item2;
        }
    }
}
