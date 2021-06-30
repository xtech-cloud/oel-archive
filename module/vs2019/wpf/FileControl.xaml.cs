
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
            tbEntry.Text = entry;
            try
            {
                imgViewer.Visibility = entry.EndsWith(".png") || entry.EndsWith(".jpg") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                txtViewer.Visibility = entry.EndsWith(".json") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                if (entry.EndsWith(".png"))
                {
                    Stream ms = new MemoryStream(reader_.Read(entry));
                    PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    imgViewer.ImageSource = decoder.Frames[0];
                }
                else if (entry.EndsWith(".jpg"))
                {
                    Stream ms = new MemoryStream(reader_.Read(entry));
                    JpegBitmapDecoder decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    imgViewer.ImageSource = decoder.Frames[0];
                }
                else if(entry.EndsWith(".json"))
                {
                    using var stream = new MemoryStream();
                    var document = JsonDocument.Parse(reader_.Read(entry));
                    var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
                    document.WriteTo(writer);
                    writer.Flush();
                    txtViewer.Text = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch(System.Exception ex)
            {
                Growl.Warning(ex.Message, "StatusGrowl");
            }
        }
    }
}
