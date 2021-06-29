using System.IO;
using System.Collections.Generic;

namespace XTC.oelArchive
{
    public class FileWriter : Writer
    {
        private Dictionary<string, Entry> entryMap_ = new Dictionary<string, Entry>();
        private FileStream fileStream_ = null;
        private long offset_ = 0;
        private long flags = 0;
        private string pwd = "";

        public void Open(string _filepath, bool _overwrite)
        {
            if(File.Exists(_filepath))
            {
                if(_overwrite)
                    File.Delete(_filepath);
                else
                    return;

            }

            entryMap_.Clear();

            fileStream_ = File.OpenWrite(_filepath);
            // total size
            writeInt64(0);
            // flag
            writeInt64(0);
            // vtable offset
            writeInt64(0);
            // vtable size
            writeInt64(0);
        }

        public void SetPassword(string _pwd)
        {
            if (string.IsNullOrEmpty(_pwd))
                return;
            pwd = _pwd;
            flags = flags | TableUtility.Flag_PWD;
        }

        public void Close()
        {
            fileStream_.Close();
            fileStream_.Dispose();
            fileStream_ = null;
        }

        public bool HasPath(string _path)
        {
            return entryMap_.ContainsKey(_path);
        }

        public void Write(string _path, byte[] _data)
        {
            if(null ==  fileStream_)
                return;

            if(entryMap_.ContainsKey(_path))
                return;

            Entry entry = new Entry();
            entry.offset = offset_;
            entry.size = _data.Length;
            writeBytes(_data);
            entryMap_[_path] = entry;
        }

        public void Flush()
        {
            if(null ==  fileStream_)
                return;
                
            long vTableOffset = offset_;
            if(TableUtility.HasFlag(flags, TableUtility.Flag_PWD))
            {

            }
            // 写入entry数量
            writeInt32(entryMap_.Count);
            foreach(string path in entryMap_.Keys)
            {
                byte[] pathBytes = stringToBytes(path);
                // 写入entry的路径的长度
                writeInt32(pathBytes.Length);
                // 写入entry的路径
                writeBytes(pathBytes);
                // 写入entry的数据的偏移值
                writeInt64(entryMap_[path].offset);
                // 写入entry的数据的长度
                writeInt64(entryMap_[path].size);
            }
            long vTableSize = offset_ - vTableOffset;

            fileStream_.Seek(0, SeekOrigin.Begin);
            writeInt64(offset_);
            writeInt64(flags);
            writeInt64(vTableOffset);
            writeInt64(vTableSize);

            fileStream_.Flush();
        }

        private void writeInt32(int _value)
        {
            byte[] buffer = int32ToBytes(_value);
            fileStream_.Write(buffer, 0, buffer.Length);
            offset_ += buffer.Length;
        }

        private void writeInt64(long _value)
        {
            byte[] buffer = int64ToBytes(_value);
            fileStream_.Write(buffer, 0, buffer.Length);
            offset_ += buffer.Length;
        }

        private void writeBytes(byte[] _value)
        {
            fileStream_.Write(_value, 0, _value.Length);
            offset_ += _value.Length;
        }
    }
}

