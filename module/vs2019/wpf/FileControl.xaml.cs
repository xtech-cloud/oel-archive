
using HandyControl.Controls;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using XTC.oelArchive;

namespace oel.archive
{
    public partial class FileControl : UserControl
    {
        public class FileUiBridge : IFileUiBridge
        {
            public FileControl control { get; set; }

            public object getRootPanel()
            {
                return control;
            }

            public void Alert(string _message)
            {
            }
        }

        public FileFacade facade { get; set; }

        private FileReader reader_ { get; set; }

        public FileControl()
        {
            InitializeComponent();
            tbPassword.Visibility = System.Windows.Visibility.Visible;
            btnOpen.Visibility = System.Windows.Visibility.Visible;
            btnClose.Visibility = System.Windows.Visibility.Collapsed;
            spInfo.Visibility = System.Windows.Visibility.Hidden;
        }

        private void onOpenClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            string path = string.Empty;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "All Files(*.*)|*.*"
            };

            bool result = (bool)openFileDialog.ShowDialog();
            if (!result)
                return;

            tbPassword.Visibility = System.Windows.Visibility.Hidden;
            btnOpen.Visibility = System.Windows.Visibility.Collapsed;
            btnClose.Visibility = System.Windows.Visibility.Visible;

            lbEntry.Items.Clear();
            path = openFileDialog.FileName;
            reader_ = new FileReader();
            if (!string.IsNullOrEmpty(tbPassword.Text))
                reader_.SetPassword(tbPassword.Text);
            reader_.Open(path);
            foreach (string entry in reader_.entries)
            {
                lbEntry.Items.Add(entry);
            }
        }

        private void onEntrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string entry = lbEntry.SelectedItem as string;
            if (string.IsNullOrEmpty(entry))
                return;

            spInfo.Visibility = System.Windows.Visibility.Visible;
            tbEntry.Text = entry;
            try
            {
                imgViewer.Visibility = entry.EndsWith(".png") || entry.EndsWith(".jpg") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                txtViewer.Visibility = entry.EndsWith(".json") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                if (entry.EndsWith(".png"))
                {
                    //不能适用using方式,否则图片的数据会被释放
                    Stream ms = new MemoryStream(reader_.Read(entry));
                    PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    var frame = decoder.Frames[0];
                    imgViewer.ImageSource = frame;
                    tbTip.Text = string.Format("{0}x{1}   {2}", frame.PixelWidth, frame.PixelHeight, formatSize(ms.Length));
                }
                else if (entry.EndsWith(".jpg"))
                {
                    //不能适用using方式,否则图片的数据会被释放
                    Stream ms = new MemoryStream(reader_.Read(entry));
                    JpegBitmapDecoder decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    var frame = decoder.Frames[0];
                    imgViewer.ImageSource = frame;
                    tbTip.Text = string.Format("{0}x{1}   {2}", frame.PixelWidth, frame.PixelHeight, formatSize(ms.Length));
                }
                else if (entry.EndsWith(".json"))
                {
                    using (var ms = new MemoryStream())
                    {
                        var document = JsonDocument.Parse(reader_.Read(entry));
                        var writer = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                        document.WriteTo(writer);
                        writer.Flush();
                        txtViewer.Text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                        tbTip.Text = string.Format("{0}", formatSize(ms.Length));
                    }
                }
            }
            catch (System.Exception ex)
            {
                Growl.Warning(ex.Message, "StatusGrowl");
            }
        }

        private void onCloseClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            lbEntry.Items.Clear();
            spInfo.Visibility = System.Windows.Visibility.Hidden;
            tbPassword.Visibility = System.Windows.Visibility.Visible;
            btnOpen.Visibility = System.Windows.Visibility.Visible;
            btnClose.Visibility = System.Windows.Visibility.Collapsed;
            reader_.Close();
        }


        private string formatSize(long _size)
        {
            if (_size < 1024)
                return string.Format("{0} B", _size);
            if (_size < 1024 * 1024)
                return string.Format("{0} KB", _size / 1024);
            if (_size < 1024 * 1024 * 1024)
                return string.Format("{0} MB", _size / 1024 / 1024);
            return string.Format("{0} GB", _size / 1024 / 1024 / 1024);
        }
    }
}
