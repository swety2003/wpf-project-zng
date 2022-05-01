using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IntellectTestTool.UHFReader
{
    class EpcInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _no;
        private string _epc;
        private int _num;
        private string _rssi;
        private string _antenna;

        public string no
        {
            get { return _no; }
            set
            {
                _no = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("no"));
            }
        }

        public string epc
        {
            get { return _epc; }
            set
            {
                _epc = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("epc"));
            }
        }

        public int num
        {
            get { return _num; }
            set
            {
                _num = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("num"));
            }
        }

        public string rssi
        {
            get { return _rssi; }
            set
            {
                _rssi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("rssi"));
            }
        }

        public string antenna
        {
            get { return _antenna; }
            set
            {
                _antenna = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("antenna"));
            }
        }

        private void Notify([CallerMemberName]string obj="")
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(obj));
            }
        }
        public EpcInfo(string no, string epc, int num, string rssi, string antenna)
        {
            _no = no;
            _epc = epc;
            _num = num;
            _rssi = rssi;
            _antenna = antenna;
        }
    }
}
