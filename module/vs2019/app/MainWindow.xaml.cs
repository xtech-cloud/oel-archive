
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RichTextBox log { get; private set; }

        public static readonly DependencyProperty SubContentProperty = DependencyProperty.Register("SubContent", typeof(object), typeof(MainWindow));
        public object SubContent
        {
            get
            {
                return GetValue(MainWindow.SubContentProperty);
            }
            set
            {
                SetValue(MainWindow.SubContentProperty, value);
            }
        }

        private Dictionary<string, object> pages = new Dictionary<string, object>();

        public MainWindow()
        {
            InitializeComponent();
            log = this.rtbLog;
        }

        public void AddPage(string _key, object _page)
        {
            pages[_key] = _page;
            lbPages.Items.Add(_key);
        }

        private void lbPages_Selected(object sender, RoutedEventArgs e)
        {
            string lbi = lbPages.SelectedItem as string;
            SubContent = pages[lbi];
        }
    }
}

