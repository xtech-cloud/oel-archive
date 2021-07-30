
using HandyControl.Controls;
using Microsoft.Win32;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Controls;
using Forms = System.Windows.Forms;
using System.Windows.Media.Imaging;
using XTC.oelArchive;
using System.Collections.Generic;

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
        private FileWriter writer_ { get; set; }
        private string path_ { get; set; }

        public FileControl()
        {
            InitializeComponent();
            tbPassword.Visibility = System.Windows.Visibility.Visible;
            btnOpen.Visibility = System.Windows.Visibility.Visible;
            btnCreate.Visibility = System.Windows.Visibility.Visible;
            btnPack.Visibility = System.Windows.Visibility.Collapsed;
            btnUnpack.Visibility = System.Windows.Visibility.Collapsed;
            btnClose.Visibility = System.Windows.Visibility.Collapsed;
            spInfo.Visibility = System.Windows.Visibility.Hidden;
        }

        private void onOpenClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            path_ = string.Empty;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "All Files(*.*)|*.*"
            };

            bool result = (bool)openFileDialog.ShowDialog();
            if (!result)
                return;


            lbEntry.Items.Clear();
            path_ = openFileDialog.FileName;
            reader_ = new FileReader();
            if (!string.IsNullOrEmpty(tbPassword.Text))
                reader_.SetPassword(tbPassword.Text);
            try
            {
                reader_.Open(path_);
                foreach (string entry in reader_.entries)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = entry;
                    item.Uid = entry;
                    lbEntry.Items.Add(item);
                }
                btnOpen.Visibility = System.Windows.Visibility.Collapsed;
                btnCreate.Visibility = System.Windows.Visibility.Collapsed;
                btnPack.Visibility = System.Windows.Visibility.Collapsed;
                btnUnpack.Visibility = System.Windows.Visibility.Visible;
                btnClose.Visibility = System.Windows.Visibility.Visible;
            }
            catch (System.Exception ex)
            {
                (facade.getUiBridge() as IFileUiBridge).Alert(ex.Message);
            }
        }

        private void onEntrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lbEntry.SelectedItem as ListBoxItem;
            if (null == item)
                return;

            string entry = item.Content as string;

            spInfo.Visibility = System.Windows.Visibility.Visible;
            tbEntry.Text = entry;
            try
            {
                imgViewer.Visibility = entry.EndsWith(".png") || entry.EndsWith(".jpg") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                txtViewer.Visibility = entry.EndsWith(".json") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                if (entry.EndsWith(".png"))
                {
                    //不能适用using方式,否则图片的数据会被释放
                    Stream ms = null;
                    if (null != reader_)
                    {
                        ms = new MemoryStream(reader_.Read(entry));
                    }
                    else if (null != writer_)
                    {
                        ms = new MemoryStream(File.ReadAllBytes(Path.Combine(path_, entry)));
                    }
                    if (null != ms)
                    {
                        PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        var frame = decoder.Frames[0];
                        imgViewer.ImageSource = frame;
                        tbTip.Text = string.Format("{0}x{1}   {2}", frame.PixelWidth, frame.PixelHeight, formatSize(ms.Length));
                    }
                }
                else if (entry.EndsWith(".jpg"))
                {
                    //不能适用using方式,否则图片的数据会被释放
                    Stream ms = null;
                    if (null != reader_)
                    {
                        ms = new MemoryStream(reader_.Read(entry));
                    }
                    else if (null != writer_)
                    {
                        ms = new MemoryStream(File.ReadAllBytes(Path.Combine(path_, entry)));
                    }
                    if (null != ms)
                    {
                        JpegBitmapDecoder decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        var frame = decoder.Frames[0];
                        imgViewer.ImageSource = frame;
                        tbTip.Text = string.Format("{0}x{1}   {2}", frame.PixelWidth, frame.PixelHeight, formatSize(ms.Length));
                    }
                }
                else if (entry.EndsWith(".json"))
                {
                    JsonDocument doc = null;
                    if (null != reader_)
                    {
                        doc = JsonDocument.Parse(reader_.Read(entry));
                    }
                    else
                    {
                        doc = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(path_, entry)));
                    }
                    if (null != doc)
                    {
                        using (var ms = new MemoryStream())
                        {
                            var writer = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                            doc.WriteTo(writer);
                            writer.Flush();
                            txtViewer.Text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                            tbTip.Text = string.Format("{0}", formatSize(ms.Length));
                        }
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
            if (null != reader_)
            {
                reader_.Close();
                reader_ = null;
            }
            if (null != writer_)
            {
                writer_.Close();
                writer_ = null;
            }
            lbEntry.Items.Clear();
            spInfo.Visibility = System.Windows.Visibility.Hidden;
            btnOpen.Visibility = System.Windows.Visibility.Visible;
            btnCreate.Visibility = System.Windows.Visibility.Visible;
            btnPack.Visibility = System.Windows.Visibility.Collapsed;
            btnUnpack.Visibility = System.Windows.Visibility.Collapsed;
            btnClose.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void onUnpackClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Forms.FolderBrowserDialog dialog = new Forms.FolderBrowserDialog();
            if (Forms.DialogResult.OK != dialog.ShowDialog())
                return;

            string dir = dialog.SelectedPath;
            dir = Path.Combine(dir, Path.GetFileNameWithoutExtension(path_));
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);

            foreach (var entry in reader_.entries)
            {
                string subdir = Path.Combine(dir, Path.GetDirectoryName(entry));
                if (!Directory.Exists(subdir))
                    Directory.CreateDirectory(subdir);
                string filepath = Path.Combine(dir, entry);
                File.WriteAllBytes(filepath, reader_.Read(entry));
            }
        }

        private void onPackClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (false == dialog.ShowDialog())
                return;
            string file = dialog.FileName;
            if (!string.IsNullOrEmpty(tbPassword.Text))
                writer_.SetPassword(tbPassword.Text);
            writer_.Open(file, true);
            foreach (var item in lbEntry.Items)
            {
                var lbItem = item as ListBoxItem;
                writer_.Write((string)lbItem.Content, File.ReadAllBytes(lbItem.Uid));
            }
            writer_.Flush();
        }

        private void onCreateClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Forms.FolderBrowserDialog dialog = new Forms.FolderBrowserDialog();
            if (Forms.DialogResult.OK != dialog.ShowDialog())
                return;

            path_ = dialog.SelectedPath;

            btnOpen.Visibility = System.Windows.Visibility.Collapsed;
            btnCreate.Visibility = System.Windows.Visibility.Collapsed;
            btnPack.Visibility = System.Windows.Visibility.Visible;
            btnUnpack.Visibility = System.Windows.Visibility.Collapsed;
            btnClose.Visibility = System.Windows.Visibility.Visible;

            List<string> files = new List<string>();
            getAllFiles(path_, path_, ref files);
            foreach (var file in files)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = file;
                item.Uid = Path.Combine(path_, file);
                lbEntry.Items.Add(item);
            }

            writer_ = new FileWriter();
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

        private void getAllFiles(string _dir, string _subdir, ref List<string> _files)
        {
            DirectoryInfo folder = new DirectoryInfo(_subdir);
            DirectoryInfo[] di = folder.GetDirectories();
            FileInfo[] fi = folder.GetFiles();

            foreach (FileInfo file in fi)
            {
                string path = Path.GetRelativePath(_dir, file.FullName);
                _files.Add(path);
            }
            foreach (DirectoryInfo subdir in di)
            {
                getAllFiles(_dir, subdir.FullName, ref _files);
            }
        }
    }
}
