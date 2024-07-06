using System.IO;
using System.Collections.Generic;

namespace XTC.OpenEL.Archive
{
    public class StreamWriter : Writer
    {
        protected Stream stream_ = null;
        private Dictionary<string, Entry> entryMap_ = new Dictionary<string, Entry>();
        private long flags_ = 0;
        private string pwd_ = "";

        protected void prepare()
        {
            entryMap_.Clear();

            // total size
            writeInt64(stream_, 0);
            // flag
            writeInt64(stream_, 0);
            // vtable offset
            writeInt64(stream_, 0);
            // vtable size
            writeInt64(stream_, 0);
        }

        public void SetPassword(string _pwd)
        {
            if (string.IsNullOrEmpty(_pwd))
                return;
            pwd_ = _pwd;
            flags_ = flags_ | TableUtility.Flag_PWD;
        }

        public void Close()
        {
            stream_.Close();
            stream_.Dispose();
            stream_ = null;
        }

        public bool HasPath(string _path)
        {
            return entryMap_.ContainsKey(_path);
        }

        public void Write(string _path, byte[] _data)
        {
            if (null == stream_)
                return;

            if (entryMap_.ContainsKey(_path))
                return;

            Entry entry = new Entry();
            entry.offset = stream_.Length;
            entry.size = _data.Length;
            writeBytes(stream_, _data);
            entryMap_[_path] = entry;
        }

        public void Flush()
        {
            if (null == stream_)
                return;

            // vTable的偏移值在包的最后面
            long vTableOffset = stream_.Length;
            byte[] vtable = generateVTable();
            if (TableUtility.HasFlag(flags_, TableUtility.Flag_PWD))
            {
                vtable = TableUtility.Encrypt(pwd_, vtable);
            }
            // 写入vTable
            writeBytes(stream_, vtable);
            // 退出开始重写header中的关键数据
            stream_.Seek(0, SeekOrigin.Begin);
            writeInt64(stream_, stream_.Length);
            writeInt64(stream_, flags_);
            writeInt64(stream_, vTableOffset);
            writeInt64(stream_, vtable.Length);

            stream_.Flush();
        }

        private void writeInt32(Stream _stream, int _value)
        {
            byte[] buffer = int32ToBytes(_value);
            _stream.Write(buffer, 0, buffer.Length);
        }

        private void writeInt64(Stream _stream, long _value)
        {
            byte[] buffer = int64ToBytes(_value);
            _stream.Write(buffer, 0, buffer.Length);
        }

        private void writeBytes(Stream _stream, byte[] _value)
        {
            _stream.Write(_value, 0, _value.Length);
        }

        private byte[] generateVTable()
        {
            byte[] data = new byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                // 写入entry数量
                writeInt32(ms, entryMap_.Count);

                foreach (string path in entryMap_.Keys)
                {
                    byte[] pathBytes = stringToBytes(path);
                    // 写入entry的路径的长度
                    writeInt32(ms, pathBytes.Length);
                    // 写入entry的路径
                    writeBytes(ms, pathBytes);
                    // 写入entry的数据的偏移值
                    writeInt64(ms, entryMap_[path].offset);
                    // 写入entry的数据的长度
                    writeInt64(ms, entryMap_[path].size);
                }
                data = ms.ToArray();
            }
            return data;
        }
    }
}

