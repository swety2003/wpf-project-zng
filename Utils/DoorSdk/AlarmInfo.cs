using System.ComponentModel;

namespace WPF_Project.Utils.DoorSdk
{
    class AlarmInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _no;
        private string _time;
        private string _alarm;

        public string no
        {
            get { return _no; }
            set
            {
                _no = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("no"));
            }
        }

        public string time
        {
            get { return _time; }
            set
            {
                _time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("time"));
            }
        }

        public string alarm
        {
            get { return _alarm; }
            set
            {
                _alarm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("alarm"));
            }
        }
        public AlarmInfo(string no, string time, string alarm)
        {
            _no = no;
            _time = time;
            _alarm = alarm;
        }
    }
}
