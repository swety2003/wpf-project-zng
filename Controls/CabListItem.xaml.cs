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

namespace WPF_Project
{
    /// <summary>
    /// CabListItem.xaml 的交互逻辑
    /// </summary>
    public partial class CabListItem : UserControl
    {
        public CabListItem()
        {
            InitializeComponent();
        }
        #region Label
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(CabListItem));
        public string LabelText
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion


        #region Button
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("BTNClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CabListItem));
        public event RoutedEventHandler BTNClick
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        void RaiseSelectedEvent()
        {
            var arg = new RoutedEventArgs(ClickEvent);
            RaiseEvent(arg);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseSelectedEvent();
        }
 
        #endregion





    }
}
