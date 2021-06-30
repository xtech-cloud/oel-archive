using System.IO;
using System.Collections.Generic;

namespace XTC.oelArchive
{
    public class StreamReader : Reader
    {
        protected Dictionary<string, Entry> entryMap_ = new Dictionary<string, Entry>();
        protected Stream stream_ = null;
        protected string pwd = "";

        public string[] entries
        {
            get
            {
                string[] paths = new string[entryMap_.Count];
                entryMap_.Keys.CopyTo(paths, 0);
                return paths;
            }
        }

        public void SetPassword(string _pwd)
        {
            if (string.IsNullOrEmpty(_pwd))
                return;
            pwd = _pwd;
        }

        public byte[] Read(string _path)
        {
            if (null == stream_)
                return null;
            if (!entryMap_.ContainsKey(_path))
                return null;

            Entry entry = entryMap_[_path];
            stream_.Seek(entry.offset, SeekOrigin.Begin);
            byte[] data = readBytes(stream_, entry.size);
            return data;
        }

        public void Close()
        {
            if (null == stream_)
                return;
            stream_.Close();
            stream_.Dispose();
        }

        protected void parseHeader()
        {
            long totalSize = readInt64(stream_);
            long flags = readInt64(stream_);
            long vTableOffset = readInt64(stream_);
            long vTableSize = readInt64(stream_);

            if (stream_.Length != totalSize)
                throw new System.Exception(string.Format("archive want {0} bytes, but only has {1} bytes", totalSize, stream_.Length));

            stream_.Seek(vTableOffset, SeekOrigin.Begin);
            byte[] vTable = readBytes(stream_, vTableSize);
            if (TableUtility.HasFlag(flags, TableUtility.Flag_PWD))
            {
                if (string.IsNullOrEmpty(pwd))
                    throw new System.Exception("need password");
                vTable = TableUtility.Decrypt(pwd, vTable);
            }


            using (MemoryStream ms = new MemoryStream(vTable))
            {
                int entryCount = readInt32(ms);

                for (int i = 0; i < entryCount; ++i)
                {
                    Entry entry = new Entry();
                    int length = readInt32(ms);
                    string entryPath = readString(ms, length);
                    entry.offset = readInt64(ms);
                    entry.size = readInt64(ms);
                    entryMap_[entryPath] = entry;
                }
            }
        }

        private int readInt32(Stream _stream)
        {
            byte[] buffer = new byte[4];
            _stream.Read(buffer, 0, buffer.Length);
            int value = bytesToInt32(buffer);
            return value;
        }

        private long readInt64(Stream _stream)
        {
            byte[] buffer = new byte[8];
            _stream.Read(buffer, 0, buffer.Length);
            long value = bytesToInt64(buffer);
            return value;
        }

        private string readString(Stream _stream, int _size)
        {
            byte[] buffer = new byte[_size];
            _stream.Read(buffer, 0, buffer.Length);
            string value = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return value;
        }

        private byte[] readBytes(Stream _stream, long _size)
        {
            byte[] buffer = new byte[_size];
            _stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}

