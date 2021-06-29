using System.IO;
using System.Collections.Generic;

namespace XTC.oelArchive
{
    public class StreamReader : Reader
    {
        protected Dictionary<string, Entry> entryMap = new Dictionary<string, Entry>();
        protected Stream stream = null;
        protected long offset = 0;

        public string[] entries {
            get{
                string[] paths = new string[entryMap.Count];
                entryMap.Keys.CopyTo(paths, 0);
                return paths;
            }
        }

        public byte[] Read(string _path)
        {
            if(null ==  stream)
                return null;
            if(!entryMap.ContainsKey(_path))
                return null;

            Entry entry = entryMap[_path];
            stream.Seek(entry.offset, SeekOrigin.Begin);
            byte[] data = readBytes(entry.size);
            return data;
        }

        public void Close()
        {
            if (null == stream)
                return;
            stream.Close();
            stream.Dispose();
        }

        protected void parseHeader()
        {
            long totalSize = readInt64();
            long flags = readInt64();
            long vTableOffset = readInt64();
            long vTableSize = readInt64();

            if(stream.Length != totalSize)
                throw new System.Exception(string.Format("archive want {0} bytes, but only has {1} bytes", totalSize, stream.Length));
            
            stream.Seek(vTableOffset, SeekOrigin.Begin);
            int entryCount = readInt32();
            for(int i =0; i < entryCount; ++i)
            {
                Entry entry = new Entry();
                int length = readInt32();
                string entryPath = readString(length);
                entry.offset = readInt64();
                entry.size = readInt64();
                entryMap[entryPath] = entry;
            }
        }

        private int readInt32()
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            int value = bytesToInt32(buffer);
            return value;
        }

        private long readInt64()
        {
            byte[] buffer = new byte[8];
            stream.Read(buffer, 0, buffer.Length);
            long value = bytesToInt64(buffer);
            return value;
        }

        private string readString(int _size)
        {
            byte[] buffer = new byte[_size];
            stream.Read(buffer, 0, buffer.Length);
            string value = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return value;
        }

        private byte[] readBytes(long _size)
        {
            byte[] buffer = new byte[_size];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}

